/**
 * Created by wayneaustin on 14/01/2015.
 */

(function () {
    "use strict";

    var app = angular.module("app", ['ngAnimate', 'controllers', 'directives']);

    var controllers = angular.module("controllers", [])
        .controller("CompletionController", ['$scope', 'CredentialService', 'Model',  function ($scope, credentialService, viewModel) {

        $scope.awaitingServerResponse = false;
        $scope.passwordServerFail = false;
        $scope.passwordValidity = {
            specialChars:false,
            minChars:false,
            capsUsed:false,
            numUsed:false,
            check: function(pswd) {
                this.minChars = pswd.length >= 8;
                this.capsUsed = pswd.match(/[a-z].*[A-Z]|[A-Z].*[a-z]/);
                this.numUsed = pswd.match(/\d/);
                this.specialChars = pswd.match(/[-!$£@%^&#*()_+|~=`{}\[\]:";'<>?¬,.\\\/]/);
                this.isValid();
            },
            isValid: function() {
                return _.reduce(this, function(result, num) {
                    if(typeof(num) != "function")
                        return result && num;
                    return result;
                }, true);
            }
        };
        $scope.isFormDisabled = function() {
            return ($scope.awaitingServerResponse || $scope.account.passwordSet);
        };

        var viewStateModel = function() {
            return {
                view: "create-new-password",
                setView: function() {
                    if (!$scope.account.passwordSet)
                        return this.view = "create-new-password";
                    if ($scope.model.submitted && $scope.model.email)
                        return this.view = "email-verification-message";
                    if ($scope.model.submitted && $scope.model.mobile)
                        return this.view = "mobile-verification-message";
                    if ($scope.account.primaryEmailExists && $scope.account.primaryPhoneExists)
                        window.location.replace("continue");
                    if (!$scope.account.primaryEmailExists)
                        return this.view = "capture-email";
                    if ($scope.account.primaryEmailExists && !$scope.account.primaryPhoneExists)
                        return this.view = "capture-mobile";
                }
            }
        };

        $scope.viewState = new viewStateModel();

        $scope.model = {
            password:null,
            email:null,
            mobile:null,
            submitted:false
        };

        var account = function() {
            return {
                passwordSet:viewModel.passwordSet,
                primaryEmailExists: viewModel.primaryEmailExists,
                primaryPhoneExists: viewModel.primaryPhoneExists,
                updatedPassword: function() {
                    this.passwordSet = true;
                }
            }
        };

        $scope.account = new account();

        $scope.updatePassword = function() {
            $scope.awaitingServerResponse = true;
            credentialService.submitCredentials(
                $scope.model.password,
                function() {
                    $scope.account.updatedPassword();
                    $scope.awaitingServerResponse = false;
                    $scope.viewState.setView();
                },
                function() {
                    $scope.awaitingServerResponse = false;
                    //console.log("password failure response from server.");
                }
            );
        };

        $scope.updateContactDetails = function() {
            $scope.awaitingServerResponse = true;
            credentialService.submitContactDetails(
                $scope.model.email,
                $scope.model.mobile,
                function() {
                    $scope.awaitingServerResponse = false;
                    $scope.model.submitted = true;
                    $scope.viewState.setView();
                },
                function() {
                    //console.log("FAIL");
                }
            );
        };

        $scope.viewState.setView();
    }]);

    app.directive("loaderWrap", function() {
        function link(scope, element, attrs) {
            scope.loadertext = attrs.loadertext;
            if(element.is(":visible"))
                element.children('.loader').addClass('spinner');
            else
                element.children('.loader').removeClass('spinner');
        }
        return {
            link: link,
            template: '<div class="loader"><div class="rect1"></div><div class="rect2"></div><div class="rect3"></div><div class="rect4"></div><div class="rect5"></div></div><p>{{loadertext}}</p>'
        }
    });

    app.service("CredentialService", ['$http', function ($http) {
        return {
            submitCredentials: function (password, successFn, failFn) {
                $http.post("UpdatePassword", { password: password })
                    .success(successFn)
                    .error(failFn);
            },
            submitContactDetails: function(email, mobile, successFn, failFn) {
                $http.post("UpdateContactDetails", { email:email, mobileNumber:mobile })
                    .success(successFn)
                    .error(failFn);
            }
        }
    }]);
})();

(function () {
    try {
        var json = document.getElementById("modelJson").textContent;
        var model = JSON.parse(json);
        angular.module("app").constant("Model", model);
    } catch (e) {

    }
})();

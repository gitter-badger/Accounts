/**
 * Created by wayneaustin on 14/01/2015.
 */

(function () {
    "use strict";

    var app = angular.module("app", []);


    app.controller("ResetController", ['$scope', '$timeout', function ($scope, $timeout) {

    }]);

    app.controller("CompletionController", ['$scope', 'CredentialService',  function ($scope, credentialService) {

        $scope.isFormDisabled = function() {
            return ($scope.awaitingServerRepsonse || $scope.account.passwordSet);
        };
        $scope.awaitingServerRepsonse = false;
        $scope.passwordServerFail = false;

        var account = function() {
            return {
                passwordSet:false,
                primaryEmail:"keef@beefmail.com",
                alternativeEmail:null,
                primaryPhone:"012348008135",
                alternativePhone:null,
                updatedPassword: function() {
                    this.passwordSet = true;
                }
            }
        };

        $scope.account = new account();

        $scope.updatePassword = function() {
            $scope.awaitingServerRepsonse = true;
            credentialService.submitCredentials(
                $scope.model.password,
                function() {
                    $scope.account.updatedPassword();
                    $scope.awaitingServerRepsonse = false;
                    determineCommsRequest();
                },
                function() {
                    $scope.awaitingServerRepsonse = false;
                    console.log("password failure response from server.");
                }
            );
        };

        var determineCommsRequest = function() {
            if(account.alternativeEmail && account.alternativePhone) {
                //we have all user's details, forward them to homepage
            } else if(account.alternativeEmail && !account.alternativePhone) {
                $('#capture-phone').addClass('bounceInDown');
            } else if(!account.alternativeEmail && account.alternativePhone) {
                $('#capture-email').addClass('bounceInDown');
            } else if(!account.alternativeEmail && !account.alternativePhone) {
                $('#capture-all').addClass('bounceInDown');
            }
        };

        $('#password-submit').on('click', function(e) {
            e.preventDefault();
            $scope.updatePassword();
        });
    }]);


    app.service("CredentialService", ['$http', function ($http) {
        return {
            submitCredentials: function (password, successFn, failFn) {
                $http.post("UpdatePassword", { password: password })
                    .success(successFn)
                    .error(failFn);
            },
            _submitCredentials: function(successFn, failFn) {
                console.log("hit service");
                setTimeout(function() {
                    console.log("success callback");

                    successFn();
                }, 2000);
            }
        }
    }]);

})();


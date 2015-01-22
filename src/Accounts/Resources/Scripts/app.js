/**
 * Created by wayneaustin on 14/01/2015.
 */

(function () {
    "use strict";

    var app = angular.module("app", []);


    app.controller("ResetController", ['$scope', '$timeout', function ($scope, $timeout) {

    }]);

    app.controller("CompletionController", ['$scope', 'CredentialService',  function ($scope, credentialService) {

        $scope.awaitingServerResponse = false;
        $scope.passwordServerFail = false;
        $scope.passwordValidity = {
            minChars:false,
            specialChars:false,
            capsUsed:false,
            numUsed:false,
            isValid: function() {
                return _.reduce(this, function(result, num) {
                    if(typeof(num) != "function") {
                        return result && num;
                    }
                    return result;
                }, true);
            }
        };
        $scope.isFormDisabled = function() {
            return ($scope.awaitingServerResponse || $scope.account.passwordSet);
        };

        $scope.model = {
            password:""
        };

        var account = function() {
            return {
                passwordSet:false,
                primaryEmailExists:false,
                primaryPhoneExists:false,
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
                    determineCommsRequest();
                },
                function() {
                    $scope.awaitingServerResponse = false;
                    //console.log("password failure response from server.");
                }
            );
        };

        var determineCommsRequest = function() {
            if($scope.account.primaryEmailExists && $scope.account.primaryPhoneExists) {
                //we have all user's details, forward them to homepage
            } else if($scope.account.primaryEmailExists && !$scope.account.primaryPhoneExists) {
                $('#capture-phone').addClass('bounceInDown');
            } else if(!$scope.account.primaryEmailExists && $scope.account.primaryPhoneExists) {
                $('#capture-email').addClass('bounceInDown');
            } else if(!$scope.account.primaryEmailExists && !$scope.account.primaryPhoneExists) {
                $('#capture-all').addClass('bounceInDown');
            }
        };

        $('#password-submit').on('click', function(e) {
            e.preventDefault();
            $scope.updatePassword();
        });

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

    app.directive("passwordMaskControl", function() {

        function link() {

            function changeType(x, type) {
                if(x.prop('type') == type)
                    return x;
                try {
                    return x.prop('type', type);
                } catch(e) {

                    var html = $("<div>").append(x.clone()).html();
                    var regex = /type=(")?([^"\s]+)(")?/;
                    var tmp = $(html.match(regex) == null ?
                        html.replace(">", ' type="' + type + '">') :
                        html.replace(regex, 'type="' + type + '"') );
                    tmp.data('type', x.data('type') );
                    var events = x.data('events');
                    var cb = function(events) {
                        return function() {
                            for(var i in events)
                            {
                                var y = events[i];
                                for(var j in y)
                                    tmp.bind(i, y[j].handler);
                            }
                        }
                    }(events);
                    x.replaceWith(tmp);
                    setTimeout(cb, 10);
                    return tmp;
                }
            }

            $('.unmask').on('click', function(){
                if($(this).prev('input').attr('type') == 'password')
                    changeType($(this).prev('input'), 'text');
                else
                    changeType($(this).prev('input'), 'password');
                return false;
            });
        }

        return {
            link:link
        }
    });

    app.directive("passwordValidator", function() {
        var html = '<ul class="password-validation">';
        html += '<li id="specialCharacter" data-ng-class="{valid:validity.specialChars}"><span>*!£^</span><p>A symbol</p></li>';
        html += '<li id="number" data-ng-class="{valid:validity.numUsed}"><span>3</span><p>A number</p></li>';
        html += '<li id="letter" data-ng-class="{valid:validity.capsUsed}"><span>Aa</span><p>Upper / lower case</p></li>';
        html += '<li id="length" data-ng-class="{valid:validity.minChars}"><span>8+</span><p>8 characters long</p></li>';
        html += '</ul>';
        return {
            scope: {
                "validity":"=",
                "content":"="
            },
            link: function(scope) {
                var checkValidity = function(pswd) {

                    scope.validity.minChars = pswd.length >= 8;

                    scope.validity.capsUsed = pswd.match(/[a-z].*[A-Z]|[A-Z].*[a-z]/);

                    scope.validity.numUsed = pswd.match(/\d/);

                    scope.validity.specialChars = pswd.match(/[-!$%^&*()_+|~=`{}\[\]:";'<>?,.\/]/);

                    scope.validity.isValid();
                };

                scope.$watch('content', function(newVal){
                    if(!newVal) newVal = "";
                    checkValidity(newVal);
                }, true);
            },
            template: html
        }
    });

    app.service("CredentialService", ['$http', function ($http) {
        return {
            submitCredentials: function (password, successFn, failFn) {
                $http.post("UpdatePassword", { password: password })
                    .success(successFn)
                    .error(failFn);
            }
            /*_submitCredentials: function(successFn, failFn) {
                //console.log("hit service");
                setTimeout(function() {
                    //console.log("success callback");
                    successFn();
                }, 2000);
            }*/
        }
    }]);

})();


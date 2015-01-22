/**
 * Created by wayneaustin on 19/01/2015.
 */

describe("First login account completion controller", function() {

    beforeEach(function () {
        module("app");
    });

    describe("CompletionController", function () {
        var completionController;
        var scope;
        var timeout;
        var model;

        beforeEach(function() {
            inject(function ($rootScope, $controller) {
                scope = $rootScope.$new();
                timeout = {};
                model = {
                    passwordSet:false,
                    primaryEmailExists:false,
                    primaryPhoneExists:false 
                };
                completionController = $controller('CompletionController', {
                    $scope: scope,
                    $timeout: timeout,
                    Model: model
                });
            });
        });

        it("When model requires password expect account to require password", function() {
            model = {
                passwordSet:false
            };
            inject(function ($rootScope, $controller) {
                scope = $rootScope.$new();
                timeout = function(){};
                model = {
                    passwordSet:false,
                    primaryEmailExists:false,
                    primaryPhoneExists:false 
                };
                completionController = $controller('CompletionController', {
                    $scope: scope,
                    $timeout: timeout,
                    Model: model
                });
            });

            expect(scope.account).not.toBe(null);
            expect(scope.account.passwordSet).toBe(false);
        });

        it("When model does not require password expect account does not require password", function() {
            model = {
                passwordSet:false
            };
            inject(function ($rootScope, $controller) {
                scope = $rootScope.$new();
                timeout = function(){};
                model = {
                    passwordSet:true,
                    primaryEmailExists:false,
                    primaryPhoneExists:false
                };
                completionController = $controller('CompletionController', {
                    $scope: scope,
                    $timeout: timeout,
                    Model: model
                });
            });

            expect(scope.account).not.toBe(null);
            expect(scope.account.passwordSet).toBe(true);
        });
    });
});
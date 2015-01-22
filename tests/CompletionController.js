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
        });

        function setupDependencies(model) {
            inject(function ($rootScope, $controller) {
                scope = $rootScope.$new();
                timeout = function(){};
                completionController = $controller('CompletionController', {
                    $scope: scope,
                    $timeout: timeout,
                    Model: (model || {
                        passwordSet:false,
                        primaryEmailExists:false,
                        primaryPhoneExists:false
                    })
                });
            });
        }

        it("When model requires password expect account to require password", function() {
            setupDependencies({
                    passwordSet:false,
                    primaryEmailExists:false,
                    primaryPhoneExists:false
                });

            expect(scope.account).not.toBe(null);
            expect(scope.account.passwordSet).toBe(false);
        });

        it("When model does not require password expect account does not require password", function() {
            setupDependencies({
                    passwordSet:true,
                    primaryEmailExists:false,
                    primaryPhoneExists:false
                });

            expect(scope.account).not.toBe(null);
            expect(scope.account.passwordSet).toBe(true);
        });

        it("When model does not require email expect that account does not require email", function() {
            setupDependencies({
                passwordSet:false,
                primaryEmailExists:true,
                primaryPhoneExists:false
            });

            expect(scope.account).not.toBe(null);
            expect(scope.account.primaryEmailExists).toBe(true);
        });

        it("When model requires email expect that account requires email", function() {
            setupDependencies({
                passwordSet:false,
                primaryEmailExists:false,
                primaryPhoneExists:false
            });

            expect(scope.account).not.toBe(null);
            expect(scope.account.primaryEmailExists).toBe(false);
        });

        it("When model does not require phone expect that account does not require phone", function() {
            setupDependencies({
                passwordSet:false,
                primaryEmailExists:false,
                primaryPhoneExists:true
            });

            expect(scope.account).not.toBe(null);
            expect(scope.account.primaryPhoneExists).toBe(true);
        });

        it("When model requires phone expect that account requires phone", function() {
            setupDependencies({
                passwordSet:false,
                primaryEmailExists:false,
                primaryPhoneExists:false
            });

            expect(scope.account).not.toBe(null);
            expect(scope.account.primaryPhoneExists).toBe(false);
        });
    });
});
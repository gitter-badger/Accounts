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

        // Password Validation
        it("When special character entered in to password field ensure special character validation property returns true", function() {
            setupDependencies({
                passwordSet:false,
                primaryEmailExists:false,
                primaryPhoneExists:false
            });

            var chars = ["!","£","$","%","^","&","*","(",")","_","-","+","=","@","#","~","?","/","\\","|","`","¬","\"","<",">",",",".",":",";"];

            var result = true;

            _.forEach(chars, function(c) {
                scope.passwordValidity.check(c);
                if(!scope.passwordValidity.specialChars) {
                    return result = false;
                }
            });

            expect(result).toBe(true);
        });

        it("When a number is entered in to password field ensure number usage validation property returns true", function() {
            setupDependencies({
                passwordSet:false,
                primaryEmailExists:false,
                primaryPhoneExists:false
            });

            scope.passwordValidity.check("123");
            expect(scope.passwordValidity.numUsed).toBe(true);
        });

        it("When both lowercase and uppercase letters are used entered in to password field ensure lowercase and uppercase usage validation property returns true", function() {
            setupDependencies({
                passwordSet:false,
                primaryEmailExists:false,
                primaryPhoneExists:false
            });

            scope.passwordValidity.check("Aa");
            expect(scope.passwordValidity.capsUsed).toBe(true);
        });

        it("When password field value is 8 characters long ensure minimum character count validation property returns true", function() {
            setupDependencies({
                passwordSet:false,
                primaryEmailExists:false,
                primaryPhoneExists:false
            });

            scope.passwordValidity.check("12345678");
            expect(scope.passwordValidity.minChars).toBe(true);
        });

        it("When password field value is more than 8 characters long ensure minimum character count validation property returns true", function() {
            setupDependencies({
                passwordSet:false,
                primaryEmailExists:false,
                primaryPhoneExists:false
            });

            scope.passwordValidity.check("123456789101112");
            expect(scope.passwordValidity.minChars).toBe(true);
        });

        it("When password field value is less than 8 characters long ensure minimum character count validation property returns false", function() {
            setupDependencies({
                passwordSet:false,
                primaryEmailExists:false,
                primaryPhoneExists:false
            });

            scope.passwordValidity.check("1234567");
            expect(scope.passwordValidity.minChars).toBe(false);
        });

        it("When the password field is empty ensure validation fails", function() {
            setupDependencies({
                passwordSet:false,
                primaryEmailExists:false,
                primaryPhoneExists:false
            });

            scope.passwordValidity.check("");
            expect(scope.passwordValidity.isValid()).toBe(false);
        });
    });
});
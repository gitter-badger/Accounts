/**
 * Created by wayneaustin on 19/01/2015.
 */

describe("First login account completion controller", function() {

    beforeEach(function () {
        module("app");
    });

    describe("CompletionController", function () {
        var completionController, scope, timeout;

        beforeEach(function() {
            inject(function ($rootScope, $controller) {
                scope = $rootScope.$new();
                timeout = {};
                completionController = $controller('CompletionController', {
                    $scope: scope,
                    $timeout: timeout
                });
            });
        });
    });
});
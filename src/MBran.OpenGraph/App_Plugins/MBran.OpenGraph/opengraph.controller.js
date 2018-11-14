angular.module('umbraco')
    .controller('MBran.OpenGraph.OpenGraphController', function ($scope, $element, assetsService, dialogService, entityResource) {

        function init() {

            initOptions();
            initModel();
            initControls();
        }

        init();

        function initOptions() {
            $scope.model.hideLabel = true;
        }

        function initModel() {

            if (!$scope.model.value) {
                $scope.model.value = {};
            }

            if (!$scope.model.value.image) {
                initThumbnailModel();
            }
            else {
                entityResource.getById($scope.model.value.image, "Media").then(function (ent) {
                    $scope.thumbnail = {};
                    $scope.thumbnail.src = ent.metaData.umbracoFile.Value.src;
                    $scope.thumbnail.width = ent.metaData.umbracoWidth.Value;
                    $scope.thumbnail.height = ent.metaData.umbracoHeight.Value;
                });
            }
        }

        function initThumbnailModel() {
            $scope.thumbnail = {};
            $scope.thumbnail.src = '';
            $scope.thumbnail.width = 0;
            $scope.thumbnail.height = 0;
        }

        function initControls() {
            assetsService
                .loadJs("~/Umbraco/lib/slider/js/bootstrap-slider.js")
                .then(function () {

                   

                });

            assetsService.loadCss("~/Umbraco/lib/slider/bootstrap-slider.css");
            assetsService.loadCss("~/Umbraco/lib/slider/bootstrap-slider-custom.css");
        }

        $scope.pickImage = function () {
            dialogService.mediaPicker({
                multiPicker: false, 
                callback: function (data) {
                    $scope.model.value.image = data.id;
                    $scope.thumbnail.src = data.thumbnail;
                    $scope.thumbnail.width = data.originalWidth;
                    $scope.thumbnail.height = data.originalHeight;

                }
            });
        };
        $scope.removeImage = function () {
            $scope.thumbnail.src = '';
            $scope.model.value.image = '';
        };	
});
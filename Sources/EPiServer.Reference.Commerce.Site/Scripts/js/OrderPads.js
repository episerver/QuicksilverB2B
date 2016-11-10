var orderPadsComponent = (function () {

    var $subOrganizationRows,
        $userRows,
        $productRows,
        $expandUserRowsBtn,
        $expandProductRowsBtn;

    function bindEvents() {
        $expandUserRowsBtn.click(function (e) {
            e.preventDefault();
            var $this = $(this);
            var $thisIcon = $this.find('span');
            var dataToExpandClassForUsers = $this.attr('data-expand');
            var $usersRows = $subOrganizationRows.siblings('.' + dataToExpandClassForUsers);

            if ($this.hasClass('js-users-row-collapsed')) {
                $thisIcon.addClass('glyphicon-minus').removeClass('glyphicon-plus');
                $usersRows.addClass('tr-show');
                $this.removeClass('js-users-row-collapsed');
            } else {
                $usersRows.each(function () {

                    var $this = $(this);
                    var $btn = $this.find('.btn');
                    var $icon = $btn.find('span');
                    var dataToExpandClassForProducts = $btn.attr('data-expand');

                    if (!$btn.hasClass('js-products-row-collapsed')) {
                        $subOrganizationRows.siblings('.' + dataToExpandClassForProducts).removeClass('tr-show');
                        $btn.addClass('js-products-row-collapsed');
                        $icon.addClass('glyphicon-plus').removeClass('glyphicon-minus');
                    }
                    
                    $this.removeClass('tr-show');
                });
                $thisIcon.addClass('glyphicon-plus').removeClass('glyphicon-minus');
                $this.addClass('js-users-row-collapsed');
            }
        });

        $expandProductRowsBtn.click(function (e) {
            e.preventDefault();
            var $this = $(this);
            var $thisIcon = $this.find('span');
            var dataToExpandClassForProducts = $this.attr('data-expand');
            if ($this.hasClass('js-products-row-collapsed')) {
                $this.removeClass('js-products-row-collapsed');
                $thisIcon.addClass('glyphicon-minus').removeClass('glyphicon-plus');
                $subOrganizationRows.siblings('.' + dataToExpandClassForProducts).addClass('tr-show');
            } else {
                $thisIcon.addClass('glyphicon-plus').removeClass('glyphicon-minus');
                $subOrganizationRows.siblings('.' + dataToExpandClassForProducts).removeClass('tr-show');
                $this.addClass('js-products-row-collapsed');
            }
        });
    }

    function init() {
        $subOrganizationRows = $('.sub-organization-row');
        $userRows = $('.user-row');
        $productRows = $('.product-row');
        $expandUserRowsBtn = $subOrganizationRows.find('.btn');
        $expandProductRowsBtn = $userRows.find('.btn');

        bindEvents();
    }

    return {
        init: init
    }

})();

$(document).ready(function () {
    orderPadsComponent.init();
});
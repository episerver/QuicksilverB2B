var OrderPadsComponent = function(options) {

    var $table,
        $firstRows,
        $secondRows,
        $thirdRows,
        $expandFirstRowsBtn,
        $expandSecondRowsBtn;

    function expandUserRows(e) {
        e.preventDefault();
        var $this = $(this);
        var $thisIcon = $this.find('span');
        var dataToExpandClassForUsers = $this.attr('data-expand');
        var $usersRows = $firstRows.siblings('.' + dataToExpandClassForUsers);

        if ($this.hasClass('js-second-row-collapsed')) {
            $thisIcon.addClass('glyphicon-minus').removeClass('glyphicon-plus');
            $usersRows.addClass('tr-show');
            $this.removeClass('js-second-row-collapsed');
        }
        else {
            $usersRows.each(function () {

                var $this = $(this);
                var $btn = $this.find('.btn');
                var $icon = $btn.find('span');
                var dataToExpandClassForProducts = $btn.attr('data-expand');

                if (!$btn.hasClass('js-third-row-collapsed')) {
                    $firstRows.siblings('.' + dataToExpandClassForProducts).removeClass('tr-show');
                    $btn.addClass('js-third-row-collapsed');
                    $icon.addClass('glyphicon-plus').removeClass('glyphicon-minus');
                }

                $this.removeClass('tr-show');
            });
            $thisIcon.addClass('glyphicon-plus').removeClass('glyphicon-minus');
            $this.addClass('js-second-row-collapsed');
        }
    }

    function expandProductRows(e) {
        e.preventDefault();
        var $this = $(this);
        var $thisIcon = $this.find('span');
        var dataToExpandClassForProducts = $this.attr('data-expand');

        if ($this.hasClass('js-third-row-collapsed')) {
            $this.removeClass('js-third-row-collapsed');
            $thisIcon.addClass('glyphicon-minus').removeClass('glyphicon-plus');
            $firstRows.siblings('.' + dataToExpandClassForProducts).addClass('tr-show');
        }
        else {
            $thisIcon.addClass('glyphicon-plus').removeClass('glyphicon-minus');
            $firstRows.siblings('.' + dataToExpandClassForProducts).removeClass('tr-show');
            $this.addClass('js-third-row-collapsed');
        }
    }

    function bindEvents() {
        $expandFirstRowsBtn.click(expandUserRows);

        if ($thirdRows.length > 0) {
            $expandSecondRowsBtn.click(expandProductRows);
        }
    }

    function init(options) {
        var table = options.table ? options.table : '';
        var firstRow = options.firstRowClass ? options.firstRowClass : '.first-row';
        var secondRow = options.secondRowClass ? options.secondRowClass : '.second-row';
        var thirdRow = options.thirdRowClass ? options.thirdRowClass : '.third-row';

        $table = $(table);
        $firstRows = $(firstRow, $table); // $('.sub-organization-row');
        $secondRows = $(secondRow, $table); // $('.user-row');
        $thirdRows = $(thirdRow, $table); // $('.product-row');

        if ($table.length > 0) {
            if ($secondRows.length > 0) {
                $expandFirstRowsBtn = $firstRows.find('.btn');

                if ($thirdRows.length > 0)
                    $expandSecondRowsBtn = $secondRows.find('.btn');

                bindEvents();
            }
        }
    }

    init(options);
};

$(document).ready(function () {

    
});
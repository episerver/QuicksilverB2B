$(document).ready(function () {
    var $cloner = $('.js-cloner');

    $cloner.each(function () {
        $(this).click(function (e) {
            var $this = $(this);

            e.preventDefault();
            var $rowToClone = $this.siblings('.location-row').last();
            var $clone = $rowToClone.clone();
            $clone.find('input').each(function () {
                var $this = $(this);
                var nameAttr = $this.attr('name')
                var arrNum = nameAttr.match(/\d+/);
                var nr = arrNum ? arrNum[0] : 0;
                var subStr = nameAttr.substring(0, nameAttr.indexOf(nr));
                var endStr = nameAttr.substring(nameAttr.indexOf(nr) + 1, nameAttr.length);
                var newName = subStr + (++nr) + endStr;
                $this.attr('name', newName);
                $this.val('');

                var validation = $this.siblings().last();
                var valAttr = validation.attr('data-valmsg-for');
                var subValStr = nameAttr.substring(0, valAttr.indexOf(nr));
                var endValStr = nameAttr.substring(valAttr.indexOf(nr) + 1, valAttr.length);
                var newVal = subValStr + (++nr) + endValStr;
                validation.attr('data-valmsg-for', newVal);
            });
            $clone.insertBefore($this);
        });
    });

    $('#suborg-form').on('click', '.delete-address-icon', function (e) {
        e.preventDefault();

        var $deleteIcon = $(this);
        if ($('#suborg-form').find('.location-row').length > 1) {
            var parent = $deleteIcon.closest('.location-row');
            parent.hide();
            parent.find('input[name*=Name]').val("removed");
            parent.find('input[name*=Street]').val("0");
            parent.find('input[name*=City]').val("0");
            parent.find('input[name*=PostalCode]').val("0");
            parent.find('input[name*=Country]').val("0");
            parent.removeClass('location-row').addClass('location-row-removed');
        }
    });

    var $productTableSummary = $('.product-table-summary');
    var productTableSummaryOffset = ($productTableSummary.length > 0) ? $productTableSummary.offset().top : 0;
    $(window).on('scroll', function () {
        if ($productTableSummary.length > 0) {
            if (productTableSummaryOffset - $(window).scrollTop() <= 0) {
                $productTableSummary.addClass('fixed');
            } else {
                if ($productTableSummary.hasClass('fixed')) {
                    $productTableSummary.removeClass('fixed');
                }
            }
        }
    });

    calendarComponent.init('startDate', 'dueDate');
    var firstTable = new OrderPadsComponent({
        table: '#firstTable'
    });

    var secondTable = new OrderPadsComponent({
        table: '#secondTable'
    });
    var quickOrderInputTypeFile = new InputTypeFile();
    QuickOrderComponent.init();

    addUsersAutocompleteComponent.init();
    viewUsersAutocompleteComponent.init();
});
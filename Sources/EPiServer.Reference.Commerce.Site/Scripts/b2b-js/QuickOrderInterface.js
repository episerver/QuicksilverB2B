var QuickOrderComponent = function () {

    var $quickOrderForm,
        $orderRows;

    function onChooseEvent() {
        var selectedItemData = this.getSelectedItemData();
        var parent = this.closest('.order-row');
        parent.find('input[name*=ProductName]').val(selectedItemData.ProductName);
        parent.find('input[name*=UnitPrice]').val(selectedItemData.UnitPrice);
    }

    function onDeleteIcon() {
        $quickOrderForm.on('click', '.delete-icon', function (e) {
            e.preventDefault();

            var $deleteIcon = $(this);
            if ($quickOrderForm.children('.order-row').length > 1) {
                var parent = $deleteIcon.closest('.order-row');
                parent.hide();
                parent.find('input[name*=ProductName]').val("removed");
                parent.find('input[name*=Sku]').val("removed");
                parent.find('input[name*=UnitPrice]').val("0");
                parent.find('input[name*=Quantity]').val("0");
                parent.find('input[name*=TotalPrice]').val("0");
                parent.removeClass('order-row').addClass('order-row-removed');
            }
        });
    }

    function onAddRow() {
        $('.js-add-row-btn').click(function () {
            var $this = $(this);
            var $rowToClone = $this.siblings('.order-row').last();
            var $clone = $rowToClone.clone();
            $clone.find('input').each(function () {
                var $this = $(this);

                // update name
                var nameAttr = $this.attr('name');
                var arrNum = nameAttr.match(/\d+/);
                var nr = arrNum ? arrNum[0] : 0;
                var substr = nameAttr.substring(0, nameAttr.indexOf(nr));
                var endStr = nameAttr.substring(nameAttr.indexOf(nr) + 1, nameAttr.length);
                var newString = substr + (++nr) + endStr;
                $this.attr('name', newString);

                // update id
                var idAttr = $this.attr('id');
                arrNum = idAttr.match(/\d+/);
                nr = arrNum ? arrNum[0] : 0;
                substr = idAttr.substring(0, idAttr.indexOf(nr));
                endStr = idAttr.substring(idAttr.indexOf(nr) + 1, idAttr.length);
                newString = substr + (++nr) + endStr;
                $this.attr('id', newString);

                $this.val('');
            });
            $clone.insertBefore($this);

            var $skuInput = $('input[name*=Sku]', $clone);
            var $parentOfSku = $skuInput.parent();
            var options = {
                url: function (phrase) {
                    return "/QuickOrderBlock/GetSku?query=" + phrase;
                },
                getValue: "Sku",
                list: {
                    match: {
                        enabled: true
                    },
                    onChooseEvent: onChooseEvent.bind($skuInput)
                }
            };
            $skuInput.prependTo($skuInput.parent().parent());
            $parentOfSku.remove();
            $skuInput.easyAutocomplete(options);
        });
    }

    function onUpdateQuantity() {
        $quickOrderForm.on('change', 'input[name*=Quantity]', function () {
            var $quantity = $(this);
            var $row = $quantity.closest('.order-row');
            var $totalPrice = $row.find('input[name*=TotalPrice]');
            var $unitPrice = $row.find('input[name*=UnitPrice]');
            var quantityVal = parseInt($quantity.val());
            var unitPriceVal = parseFloat($unitPrice.val());
            var total = quantityVal * unitPriceVal;
            $totalPrice.val(total);
        });
    }

    function bindEvents() {

        onDeleteIcon();
        onAddRow();
        onUpdateQuantity();
    }

    function init() {

        $quickOrderForm = $('#quickOrderForm');
        $orderRows = $('.order-row', $quickOrderForm);

        if ($quickOrderForm.length > 0 && $orderRows.length > 0) {

            $orderRows.each(function (index, element) {
                var $this = $(this);
                var $autocompleteInput = $this.find('input[name*=Sku]');

                var options = {
                    url: function (phrase) {
                        return "/QuickOrderBlock/GetSku?query=" + phrase;
                    },
                    getValue: "Sku",
                    list: {
                        match: {
                            enabled: true
                        },
                        onChooseEvent: onChooseEvent.bind($autocompleteInput)
                    }
                };
                $autocompleteInput.easyAutocomplete(options);
            });
            bindEvents();
        }
    }

    return {
        init: init
    };
}();
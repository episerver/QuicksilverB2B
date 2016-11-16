var QuickOrderComponent = function () {

    var $quickOrderForm;

    function bindEvents() {
        $quickOrderForm.on('click', '.delete-icon', function (e) {
            e.preventDefault();

            var $deleteIcon = $(this);
            if ($quickOrderForm.children('.order-row').length > 1)
                $deleteIcon.closest('.order-row').remove();
        });

        $('.js-add-row-btn').click(function () {
            var $this = $(this);
            var $rowToClone = $this.siblings('.order-row').last();
            var $clone = $rowToClone.clone();
            $clone.find('input').each(function () {
                var $this = $(this);
                var nameAttr = $this.attr('name')
                var arrNum = nameAttr.match(/\d+/);
                var nr = arrNum ? arrNum[0] : 0;
                var substr = nameAttr.substring(0, nameAttr.indexOf(nr));
                var endStr = nameAttr.substring(nameAttr.indexOf(nr) + 1, nameAttr.length);
                var newName = substr + (++nr) + endStr;
                $this.attr('name', newName);
                $this.val('');
            });
            $clone.insertBefore($this);
        });
    }

    function init() {
        $quickOrderForm = $('#quickOrderForm');
        if ($quickOrderForm.length > 0) {
            bindEvents();
        }
    }

    return {
        init: init
    };
}();
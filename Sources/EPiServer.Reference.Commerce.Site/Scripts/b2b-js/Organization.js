$(document).ready(function () {
    var $cloner = $('.js-cloner');

    $cloner.each(function() {
        $(this).click(function (e) {
            var $this = $(this),
                target = $this.attr('data-target'),
                $temp = $("." + target);

            e.preventDefault();
            $temp.clone().insertBefore($this).removeClass(target + " hidden");
        });
        $(this).trigger('click');
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
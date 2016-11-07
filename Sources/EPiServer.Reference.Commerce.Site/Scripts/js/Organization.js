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

    $('.suborg-form').submit(function (e) {
        e.preventDefault();
        var a = Misc.updateValidation('suborg-form');
        console.log(a);
    });
});
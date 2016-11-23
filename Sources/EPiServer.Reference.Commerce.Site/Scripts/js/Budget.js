var Budget = {
    init: function () {
        $(document)
            .on('click', '.jsNewBudget', Budget.NewBudget)
            .on('click', '.jsUpdateBudget', Budget.UpdateBudget);
    },
    NewBudget: function (e) {
        e.preventDefault();
        var form = $(this).closest('form');
        var data = {
            startDateTime: $("#startDate").val(), finisDateTime: $("#dueDate").val(),
            ammount: $("#ammount").val(), currency: $("#currencyBudget").val(), status: $("#statusBudget").val()
        };
        console.log(form.serialize());
        $.ajax({
            type: "POST",
            url: form[0].action,
            data: data,
            success: function (result) {
                window.location = $(".cancelNewBudget").attr('href');
            }
        });

    },
    UpdateBudget: function (e) {
        e.preventDefault();
        var form = $(this).closest('form');

        $.ajax({
            type: "POST",
            url: form[0].action,
            data: form.serialize(),
            success: function (result) {

            }
        });

    },
    preventSubmit: function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
        }
    }
};
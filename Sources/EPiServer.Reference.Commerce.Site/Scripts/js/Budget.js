var Budget = {
    init: function () {
        $(document)
            .on('click', '.jsNewBudget', Budget.NewBudget)
            .on('click', '.jsNewUserBudget', Budget.NewUserBudget)
            .on('click', '.jsUpdateBudget', Budget.UpdateBudget)
            .on('click', '.jsUpdateUserBudget', Budget.UpdateUserBudget);
    },
    NewBudget: function (e) {
        e.preventDefault();
        var form = $(this).closest('form');
        var data = {
            startDateTime: $("#startDate").val(), finishDateTime: $("#dueDate").val(),
            amount: $("#amount").val(), currency: $("#currencyBudget").val(), status: $("#statusBudget").val()
        };
        console.log(form.serialize());
        $.ajax({
            type: "POST",
            url: form[0].action,
            data: data,
            success: function (result) {
                if (result.result === "true")
                    window.location = $(".cancelNewBudget").attr('href');
                else {
                    $("#BudgetWarningMessage").show();
                    $(".warning-message-data").text(result.result);
                }
            }
        });

    },
    NewUserBudget: function (e) {
        e.preventDefault();
        var form = $(this).closest('form');
        var data = {
            startDateTime: $("#startDate").val(), finishDateTime: $("#dueDate").val(),
            amount: $("#amount").val(), currency: $("#currencyBudget").val(), status: $("#statusBudget").val(), userEmail: $("#userEmail").val()
        };
        console.log(form.serialize());
        $.ajax({
            type: "POST",
            url: form[0].action,
            data: data,
            success: function (result) {
                if (result.result === "true")
                    window.location = $(".cancelNewBudget").attr('href');
                else {
                    $("#BudgetWarningMessage").show();
                    $(".warning-message-data").text(result.result);
                }
            }
        });

    },
    UpdateBudget: function (e) {
        e.preventDefault();
        $("#BudgetWarningMessage").hide();
        var form = $(this).closest('form');
        var data = {
            startDateTime: $("#startDate").val(), finishDateTime: $("#dueDate").val(),
            amount: $("#amount").val(), currency: $("#currencyBudget").val(),
            status: $("#statusBudget").val(), budgetId: $(".jsUpdateBudget").attr("data-budget-id")
        };
        $.ajax({
            type: "POST",
            url: form[0].action,
            data: data,
            success: function (result) {
                if (result.result === "true")
                    window.location = $(".cancelUpdateBudget").attr('href');
                else {
                    $("#BudgetWarningMessage").show();
                    $(".warning-message-data").text(result.result);
                }
            }
        });

    },
    UpdateUserBudget: function (e) {
        e.preventDefault();
        $("#BudgetWarningMessage").hide();
        var form = $(this).closest('form');
        var data = {
            startDateTime: $("#startDate").val(), finishDateTime: $("#dueDate").val(),
            amount: $("#amount").val(), currency: $("#currencyBudget").val(),
            status: $("#statusBudget").val(), budgetId : $(".jsUpdateUserBudget").attr("data-budget-id")
        };
        $.ajax({
            type: "POST",
            url: form[0].action,
            data: data,
            success: function (result) {
                if (result.result === "true")
                    window.location = $(".cancelUpdateBudget").attr('href');
                else {
                    $("#BudgetWarningMessage").show();
                    $(".warning-message-data").text(result.result);
                }
            }
        });
    },
    preventSubmit: function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
        }
    }
};
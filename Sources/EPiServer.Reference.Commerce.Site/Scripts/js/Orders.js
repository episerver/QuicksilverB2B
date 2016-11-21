var OrdersPage = {
    init: function () {
        $('.orderStatusPicker').change(OrdersPage.orderStatusChange);
    },
    displaySelectedOrderByStatus: function (selectedStatus) {
        $(".orderElement").hide();
        $("."+selectedStatus).show();
    },
    orderStatusChange: function () {
        var selectedStatus = $(".orderStatusPicker option:selected").val();
        if (selectedStatus === "All")
            $(".orderElement").show();
        else
            OrdersPage.displaySelectedOrderByStatus(selectedStatus);
    }
};
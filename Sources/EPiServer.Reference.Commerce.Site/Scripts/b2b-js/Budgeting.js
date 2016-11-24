var calendarComponent = function() {
    var startDatePiker, endDatePiker;

    function updateEndDate(piker) {
        var currentDate = piker.getDate();
        endDatePiker.setMinDate(currentDate);
    }

    function updateStartDate(piker) {
        var currentDate = piker.getDate();
        startDatePiker.setMaxDate(currentDate);
    }

    function init(startDateId, endDateId) {
        startDatePiker = new Pikaday({
            field: document.getElementById(startDateId),
            format: "M/D/YYYY",
            onSelect: function() {
                updateEndDate(this);
            }
        });
        endDatePiker = new Pikaday({
            field: document.getElementById(endDateId),
            format: "M/D/YYYY",
            minDate: startDatePiker.getDate(),
            onSelect: function () {
                updateStartDate(this);
            }
        });
    }
    
    return {
        init: init
    }
}();
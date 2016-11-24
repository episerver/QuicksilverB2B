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
            onSelect: function() {
                updateEndDate(this);
            }
        });
        endDatePiker = new Pikaday({
            field: document.getElementById(endDateId),
            onSelect: function () {
                updateStartDate(this);
            }
        });
    }
    
    return {
        init: init
    }
}();
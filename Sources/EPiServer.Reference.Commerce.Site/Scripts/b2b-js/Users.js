var addUsersAutocompleteComponent = function () {

    var $addUserForm,
        $firstName,
        $lastName,
        $userEmail,
        $autocompleteInput,
        $resetIcon,
        $searchIcon;

    function onChooseEvent() {
        var selectedItem = $autocompleteInput.getSelectedItemData();
        $firstName.val(selectedItem.firstname);
        $lastName.val(selectedItem.lastname);
        $userEmail.val(selectedItem.email);
        $searchIcon.hide();
        $resetIcon.show();
    }

    function bindEvents() {
        $resetIcon.click(function (e) {
            e.preventDefault();
            $firstName.val('');
            $lastName.val('');
            $userEmail.val('');
            $autocompleteInput.val('');
            $resetIcon.hide();
            $searchIcon.show();
        });
    }

    function init() {

        $addUserForm = $('#addUserForm');
        $firstName = $('#Contact_FirstName', $addUserForm);
        $lastName = $('#Contact_LastName', $addUserForm);
        $userEmail = $('#Contact_Email', $addUserForm);
        $autocompleteInput = $('#addUsersAutocomplete');
        $resetIcon = $('.js-reset-icon');
        $searchIcon = $('.js-search-icon');

        var options = {
            url: function (phrase) {
                return "/UsersPage/GetUsers?query=" + phrase;
            },
            getValue: "fullname",
            list: {
                match: {
                    enabled: true
                },
                onChooseEvent: onChooseEvent
            }
        };

        if ($addUserForm.length > 0 && $autocompleteInput.length > 0) {
            bindEvents();
            $autocompleteInput.easyAutocomplete(options);
        }
    }

    return {
        init: init
    }
}();


var viewUsersAutocompleteComponent = function () {

    var $autocompleteInput,
        $usersTable,
        $resetIcon,
        $searchIcon;

    function onChooseEvent() {
        var selectedItem = $autocompleteInput.getSelectedItemData();
        var $rowToHide = $usersTable.find('a[data-user="' + selectedItem.fullname + '"]');
        $rowToHide.parents('tr').siblings().hide();
        $searchIcon.hide();
        $resetIcon.show();
    }

    function bindEvents() {
        $resetIcon.click(function(e) {
            e.preventDefault();
            $usersTable.children().show();
            $resetIcon.hide();
            $searchIcon.show();
            $autocompleteInput.val('');
        });
    }

    function init() {
        $autocompleteInput = $('#viewUsersAutocomplete');
        $usersTable = $('.js-users-table-body');
        $resetIcon = $('.js-reset-icon');
        $searchIcon = $('.js-search-icon');


        var options = {
            url: function (phrase) {
                return "/UsersPage/GetUsers?query=" + phrase;
            },
            getValue: "fullname",
            list: {
                match: {
                    enabled: true
                },
                onChooseEvent: onChooseEvent
            }
        };

        if ($autocompleteInput.length > 0) {
            bindEvents();
            $autocompleteInput.easyAutocomplete(options);
        }
    }

    return {
        init: init
    }
}();

$("#select-role").change(function () {
    var role = this.value;
    if (role === "Admin") {
        $('#location-div').hide();
    } else {
        $('#location-div').show();
    }  
});


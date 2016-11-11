$(document).ready(function () {

    var $addUserForm = $('#addUserForm');
    var $firstName = $('#Contact_FirstName', $addUserForm);
    var $lastName = $('#Contact_LastName', $addUserForm);
    var $userEmail = $('#Contact_Email', $addUserForm);
    var $autocompleteWrapper = $('.js-autocomplete-wrapper');
    var $autocompleteInput = $('#usersAutocomplete', $autocompleteWrapper);
    var $autocompleteResult = $('.js-autocomplete-result');
    var $autocompleteResultClose = $('.js-close-icon', $autocompleteResult);
    var $autocompleteResultUserName = $('.js-user-name', $autocompleteResult);

    $autocompleteResultClose.click(function (e) {
        e.preventDefault();
        $autocompleteWrapper.show();
        $autocompleteResult.hide();
        $firstName.val('');
        $lastName.val('');
        $userEmail.val('');
        $autocompleteInput.val('');
    });

    var options = {
        url: function (phrase) {
            return "/UsersPage/GetUsers?query=" + phrase;
        },
        getValue: "fullname",
        list: {
            match: {
                enabled: true
            },
            onChooseEvent: function () {
                var selectedItem = $autocompleteInput.getSelectedItemData();
                $autocompleteWrapper.hide();
                $autocompleteResult.show();
                $autocompleteResultUserName.text(selectedItem.fullname);
                $firstName.val(selectedItem.firstname);
                $lastName.val(selectedItem.lastname);
                $userEmail.val(selectedItem.email);
            }
        }
    };

    $autocompleteInput.easyAutocomplete(options);

});
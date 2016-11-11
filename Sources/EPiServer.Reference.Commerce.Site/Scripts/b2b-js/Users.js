$(document).ready(function () {

    var $addUserForm = $('#addUserForm');
    var $userName = $('#userName', $addUserForm);
    var $userEmail = $('#email', $addUserForm);
    var $autocompleteInput = $('#usersAutocomplete');
    var $autoCompleteWrapper = $('.custom-search');
    var $result = $('.autocomplete-result');
    var $resultClose = $('.close-icon', $result);
    var $resultUserName = $('.user-name', $result);

    $resultClose.click(function (e) {
        e.preventDefault();
        $autoCompleteWrapper.show();
        $result.hide();
        $userName.val('');
        $userEmail.val('');
        $autocompleteInput.val('');
    });

    var options = {
        url: function (phrase) {
            return "/UsersPage/GetUsers?query=" + phrase;
        },
        getValue: "name",
        list: {
            match: {
                enabled: true
            },
            onChooseEvent: function () {
                var selectedItem = $("#usersAutocomplete").getSelectedItemData();
                $resultUserName.text(selectedItem.name);
                $autoCompleteWrapper.hide();
                $result.show();
                $userName.val(selectedItem.name);
                $userEmail.val(selectedItem.email);
            }
        }
    };



    $autocompleteInput.easyAutocomplete(options);

});
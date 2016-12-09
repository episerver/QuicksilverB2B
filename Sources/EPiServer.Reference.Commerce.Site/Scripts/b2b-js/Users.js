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
        $firstName.val(selectedItem.FirstName);
        $lastName.val(selectedItem.LastName);
        $userEmail.val(selectedItem.Email);
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
            getValue: "FullName",
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
        $searchIcon,
        $activeImpersonateUserButtons;

    function onChooseEvent() {
        var selectedItem = $autocompleteInput.getSelectedItemData();
        var $rowToHide = $usersTable.find('a[data-user="' + selectedItem.FullName + '"]');
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

        $activeImpersonateUserButtons.click(function(e) {
            e.preventDefault();
            $.ajax({
                url: '/UsersPage/ImpersonateUser',
                type: 'post',
                dataType: 'json',
                data: {
                    username: $(e.currentTarget).parent().siblings('.user-email').text()
                },
                success: function(data) {
                    if (data) {
                        if (data.success)
                            location.href = '/';
                        else {
                            //show some message
                        }
                    }
                },
                error: function(a, b) {
                    alert(a);
                }
            });
        });
    }

    function init() {
        $autocompleteInput = $('#viewUsersAutocomplete');
        $usersTable = $('.js-users-table-body');
        $resetIcon = $('.js-reset-icon');
        $searchIcon = $('.js-search-icon');
        $activeImpersonateUserButtons = $('.js-users-table-body a[Title="Impersonate"]:not([disabled])');

        var options = {
            url: function (phrase) {
                return "/UsersPage/GetUsers?query=" + phrase;
            },
            getValue: "FullName",
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
        $('#organization-div').hide();
    } else {
        $('#location-div').show();
        $('#organization-div').show();
    }  
});

$("#select-suborganization").change(function () {
    var organizationId = this.value;
    $.ajax({
        url: "/UsersPage/GetAddresses",
        type: 'get',
        dataType: 'json',
        data: {
            id: organizationId
        },
        success: function (data) {
            if (data) {
                $('#select-location').empty();
                var options = '';
                for (var i = 0; i < data.length; i++) {
                    options += '<option value="' + data[i].AddressId + '">' + data[i].Name + '</option>';
                }
                $('#select-location').append(options);
            }
        }
    });
});

$(document).ready(function () {
    $("#select-role").change();
    $("#select-suborganization").change();
});


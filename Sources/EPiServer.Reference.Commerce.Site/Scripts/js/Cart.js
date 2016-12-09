var Cart = {
    init: function () {

        $(document)
            .on('keypress', '.jsChangeCartItem', Cart.preventSubmit)
            .on('click', '.jsRemoveCartItem', Cart.removeCartItem)
            .on('change', '.jsChangeCartItem', Cart.changeCartItem)
            .on('click', '.jsAddToCart', Cart.addCartItem)
            .on('click', '.jsCartRequestQuote', Cart.requestQuote)
            .on('click', '.jsCartLoadOrder', Cart.loadOrder)
            .on('click', '.jsClearQuotedCart', Cart.clearQuotedCart)
            .on('change', '#MiniCart', function () { $("#MiniCartResponsive").html($(this).html()); })
            .on('change', '#WishListMiniCart', function () { $("#WishListMiniCartResponsive").html($(this).html()); })
            .on('click', '.jsCartContinueShopping', function () {
                if ($(this).closest('#cart-dropdown')) {
                    $(this).closest('#cart-dropdown').collapse('hide');
                }                 
            })
            .on('click', '.jsWishListContinueShopping', function () {
                if ($(this).closest('#wishlist-dropdown')) {
                    $(this).closest('#wishlist-dropdown').collapse('hide');
                }                
            })
            .on('click', '.jsCartDropdown', function (e) {
                return ($(e.target).hasClass('btn') || $(e.target).parent().is('a'));
            });

        $('.cart-dropdown').on('show.bs.dropdown', function (e) {
            if ($('#CartItemCount', $(this)).val() == 0) {
                e.preventDefault();
            }
        });

    },
    changeCartItem: function (e) {

        e.preventDefault();
        var form = $(this).closest("form");
        var quantity = $("#quantity", form).val();

        if (parseInt(quantity, 10) < 0) {
            return;
        }

        var formContainer = $("#" + form.data("container"));
        $.ajax({
            type: "POST",
            url: form[0].action,
            data: form.serialize(),
            success: function (result) {

                formContainer.html($(result));
                $('.cartItemCountLabel', formContainer.parent()).text($('#CartItemCount', formContainer).val());
                $('.cartTotalAmountLabel', formContainer.parent()).text($('#CartTotalAmount', formContainer).val());

                formContainer.change();

                if (formContainer.is($('#WishListMiniCart'))) {
                    if (result.indexOf('list-group-item') === -1) {
                        $('.delete-wishlist').hide();
                    }
                    // If items where removed from the wishlist cart from the wishlist view, they should be removed from the view.
                    var wishListAction = form.closest(".wishlist-actions");
                    if (wishListAction.length > 0) {
                        wishListAction.parent().remove();
                    }
                }
            }
        });

    },
    removeCartItem: function (e) {

        e.preventDefault();
        var form = $(this).closest('form');
        $("#quantity", form).val(0).change();
        $(this).closest(".jsProductTile").remove();
        if (!$(".jsProductTile").length) {
            $(".wishlist-noitem").show();
        }
        if (window.location.pathname.toLowerCase().indexOf("checkout") > 0)
            window.location.reload();
    },
    addCartItem: function (e) {

        e.preventDefault();
        var form = $(this).closest("form");
        var formContainer = $("#" + form.data("container"));
        var skuCode = $("#code", form).val();

        $("#CartWarningMessage").hide()
        $(".warning-message", $("#CartWarningMessage")).html("");

        $.ajax({
            type: "POST",
            url: form[0].action,
            data: { code: skuCode },
            success: function (result) {

                formContainer.html($(result));
                $('.cartItemCountLabel', formContainer.parent()).text($('#CartItemCount', formContainer).val());
                $('.cartTotalAmountLabel', formContainer.parent()).text($('#CartTotalAmount', formContainer).val());

                formContainer.change();
            },
            error: function (xhr, status, error) {
                $(".warning-message", $("#CartWarningMessage")).html(xhr.statusText);
                $("#CartWarningMessage").show();
            }
        });
    },
    requestQuote: function(e) {
        var form = $(this).closest("form");
        $.ajax({
            type: "POST",
            url:   form[0].action,
            success: function (result) {
                $("#CartQuoteSucceedMessage").show();
                window.location.reload();
            },
            error: function (xhr, status, error) {
                $(".warning-message", $("#CartWarningMessage")).html(xhr.statusText);
                $("#CartWarningMessage").show();
            }
        });
    },
    loadOrder: function (e) {
        var form = $(this).closest("form");
        var orderLink = e.currentTarget.getAttribute("data-order-link");
        $.ajax({
            type: "POST",
            url: form[0].action,
            data: {orderLink : orderLink},
            success: function (result) {
                window.location = result.link;
            },
            error: function (xhr, status, error) {
                $(".warning-message", $("#CartWarningMessage")).html(xhr.statusText);
                $("#CartWarningMessage").show();
            }
        });
    },
    clearQuotedCart: function (e) {
        var form = $(this).closest("form");
        $.ajax({
            type: "POST",
            url: form[0].action,
            success: function (result) {
                window.location.reload();
            },
            error: function (xhr, status, error) {
                $(".warning-message", $("#CartWarningMessage")).html(xhr.statusText);
                $("#CartWarningMessage").show();
            }
        });
    },
    preventSubmit: function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
        }
    }
};
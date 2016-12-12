function InputTypeFile() {
    var $component = $('body').find('input:file[data-interface="lattice"]');

    function render() {
        $('body').find('input:file[data-interface="lattice"]').each(function() {
            $(this).addClass("lattice-file");

            // defining label 
            if($(this).attr('data-label') != undefined) {
                var label = $(this).attr("data-label");
            } else {
                var label = "Add file..."; 
            }
            
            // wrapping the element 
            $(this).wrap('<div class="lattice-file-wrapper"></div>');
         
            // adding the accept attr icon (image, audio or video)
            //if($(this).attr('accept')) {
            //    var iconToBeUsed = $(this).attr('accept').replace(/[^\w\s!?]/g,'');
            //    $(this).parent(".lattice-file-wrapper").prepend('<div class="lattice-file-icon-'+ iconToBeUsed +'"></div>');
            //} 

            // adding the label
            $(this).parent(".lattice-file-wrapper").prepend("<div class='btn btn-primary lattice-file-button'>" + label + "</div>");

            // adding the disabled 
            if($(this).attr('disabled')) { 
                $(this).siblings('.lattice-file-button').addClass('disabled');        
            }

            // hide the original input
            $(this).hide();

            // emulate browse click
            $('.lattice-file-button').click(function() {
                $(this).siblings('.lattice-file').click();
            });

            // adding the file name
            var fileName = $('<input class="lattice-file-name" type="text" readonly="readonly">');
            $(this).parent(".lattice-file-wrapper").append(fileName);

            // adding the the validation icon
            //var validationIcon = $('<div class="lattice-file-icon"></div>');
            //$(this).parent(".lattice-file-wrapper").append(validationIcon);

            // displaying the file name and removing fakepath crap
            $(this).on("change", function() {
                var fullPath = $(this).val();
                var pathArray = fullPath.split(/[/\\]/);
                fileName.val(pathArray[pathArray.length - 1]);

                var file = $("#fileUploaded")[0].files[0];
                var formData = new FormData();
                formData.append('file', file);

                $.ajax({
                    url: "/QuickOrderBlock/AddFromFile",
                    type: "POST",
                    dataType: "json",
                    data: formData,
                    contentType: false,
                    processData: false,
                    error: function (data) {
                        alert("Error:" + data);
                    },
                    success: function (result) {
                        window.location.href = result.data;
                    }
                });

            });
        });
    }

    function attachEvents() {
        // nothing here
    }

    function init() {
        render();
        attachEvents();
    }

    if($component.length > 0) {
        init();
    }
};
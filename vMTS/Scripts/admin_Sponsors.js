$(document).ready(function () {
    $('[data-toggle="popover"]').popover();
    var img;
    var subsite = "";///qa/

    $('#form_NewSponsor').on('submit', function (e) {
        e.preventDefault();  //prevent form from submitting
        if ($('#name').val() == '') { $('#NameAlert').removeClass('hidden'); return false; } else { $('#NameAlert').addClass('hidden'); }
        if ($('#Image').val() == '') { $('#ImageAlert').removeClass('hidden'); return false; } else { $('#ImageAlert').addClass('hidden'); }
        if ($('#url').val() == '') { $('#UrlAlert').removeClass('hidden'); return false; } else { $('#UrlAlert').addClass('hidden'); }

        UploadImageFile($('#Image').val(), $('#Image'));
    });

    window.UploadImageFile = function (file, element) {
        $('#btnSave').addClass('hidden');

        var scriptURL = "/scripts/php/SponsorUpload.php";
        var return_value = false;
        filename = file.replace(/C:\\fakepath\\/i, '');
        var dbFileName = filename;

        filename = filename.replace(/.jpg/i, '');

        /*RESIZE THE IMAGE CLIENT SIDE BEFORE UPLOAD*/
        var MAX_WIDTH = 600;
        var MAX_HEIGHT = 600;

        var canvas = document.createElement('canvas');
        canvas.id = "canvas";
        canvas.width = MAX_WIDTH;
        canvas.height = MAX_HEIGHT;
        canvas.style.visibility = "hidden";

        document.body.appendChild(canvas);

        var ctx = canvas.getContext("2d");
        file = element.get(0).files[0];

        var img = document.createElement("img");
        var reader = new FileReader();
        if (file.type.match('image.*')) {
            reader.readAsDataURL(file);

        } else {
            alert('Wrong file type');
            return false;
        };
        reader.onload = function (a) { img.src = this.result; }

        img.onload = function () {
            if (this.width == 0 || this.height == 0) {
                alert("Image is empty");
            }
            else {
                ctx.drawImage(img, 0, 0);

                var width = img.width;
                var height = img.height;

                if (width > height) {
                    if (width > MAX_WIDTH) {
                        height *= MAX_WIDTH / width;
                        width = MAX_WIDTH;
                    }
                } else {
                    if (height > MAX_HEIGHT) {
                        width *= MAX_HEIGHT / height;
                        height = MAX_HEIGHT;
                    }
                }
                canvas.width = width;
                canvas.height = height;
                ctx = canvas.getContext("2d");
                ctx.drawImage(img, 0, 0, width, height);

                var newImage = canvas.toDataURL("image/png");

                var jqxhr = $.post(scriptURL, {
                    imageData: newImage,
                    imageName: filename
                }).done(function (r) {
                    //var t = r;
                    //if (t == "Good") {
                    //    // SAVE THE PRODUCT TO THE DATABASE
                    //    alert(r);

                    //} else if (r == 'Fail') {
                    //}
                })
                .fail(function () {
                        // SEND A MESSAGE TO CLIENT
                        alert("Could Not Add Image.");
                        return_value = false;
                });

                jqxhr.done(function () {
                    var parameters = { name: $('#name').val(), logo: dbFileName, url: $('#url').val()  };

                    //console.log(parameters);

                    NewImage(parameters, function (result) {
                        if (result == "Success") {

                            alert('Sponsor added successfully.');
                            window.location.reload();
                            $('#btnSave').removeClass('hidden');

                        }
                        else {
                            alert('Sponsor could not be added');
                            $('#btnSave').removeClass('hidden');

                        }

                    });

                });

            };
        };

        img.onerror = function () {
            alert("An error occurred while loading sponsor information.");
            $('#btnSave').removeClass('hidden');

        };
        return return_value;
    }

    window.NewImage = function (parameters, callback) {
        $.ajax({
            type: "POST",
            url: "AddSponsor",
            data: parameters,
            dataType: "json",
            success: function (list) {
                callback(list);
                // add image from directory
            },
            // THE REQUEST DID NOT GET PROCESSED
            error: function (result) {
                //console.log(result);
                callback("Fail");
            }
        });

    };

    /* DELETE SPONSOR ACTIONS */

    // SHOW THE MODAL DELETE DIALOG WITH THE LARGE IMAGE FOR CONFIRMATION
    $("input[type=button]").click(function (e) {
        e.preventDefault();
        var name = $(this).attr('name');
        img = $(this).prop('id');
        //alert(name);
        if (name == 'delete') {
            //console.log('show delete confirm ' + img);

            $('#ConfirmDelete').modal({ toggle: true, backdrop: false });
        }
    });

    /* SHOW THE CONFIRM DELETE MODAL DIALOG */
    $('#ConfirmDelete').on('shown.bs.modal', function () {
        if (img == '') {
            $("#deleteForm").html('The sponsor logo could not be found, please try again.');
        } else {
            /* SHOW THE IMAGE TO THE USER TO CONFIRM */
            $.ajax({
                url: "_deletesponsor/" + img,
                cache: false,
                type: "get",
                dataType: "html",
                success: function (result) {
                    $("#deleteForm").html(result);
                }
            });

        }
    });

    $('#form_DeleteSponsor').on('submit', function (e) {
        e.preventDefault();  //prevent form from submitting

        var parameters = { id: $('#id').val() };
        var f = $('#Logo').val();
        ConfirmDelete(parameters, function (result) {
            if (result == "Success") {
                RemoveImageFile(f, function (r) {
                    if (r == 'Fail') {
                        alert('The sponsor image could not be removed.');
                    } else {
                        alert('Sponsor removed successfully.');
                        $('#ConfirmDelete').modal('hide');
                        window.location.reload();
                    };
                });
            }
            else {
                alert('Sponsor could not be removed');
            }

        });

        //console.log('Image ' + f);

    });


    window.RemoveImageFile = function (file, element) {
        var scriptURL = "/scripts/php/SponsorRemove.php"
        filename = file.replace(/C:\\fakepath\\/i, '');
        filename = filename.replace(/.jpg/i, '');

        var jqxhr = $.post(scriptURL, {
            //imageData: newImage,
            imageName: filename
        }).done(function (r) {
        })
.fail(function () {
    // SEND A MESSAGE TO CLIENT
    alert("Could Not Remove Sponsor.");
    return_value = false;
});

        //jqxhr.done(function () {
        //var form = $('#form_DeleteSponsor');
        //var parameters = form.serialize();
        //    //console.log(parameters);

        //ConfirmDelete(parameters, function (result) {
        //        if (result == "Pass") {

        //            alert('Sponsor removed successfully.');
        //            $('#ConfirmDelete').modal('hide');

        //            window.location.reload();
        //        }
        //        else {
        //            alert('Sponsor could not be removed');
        //        }

        //    });

        //});

    }

    window.ConfirmDelete = function (parameters, callback) {
        $.ajax({
            type: "POST",
            url: "DeleteSponsor",
            data: parameters,
            dataType: "json",
            success: function (result) {
                callback(result);
            },
            // THE REQUEST DID NOT GET PROCESSED
            error: function (result) {
                //console.log(result);
                callback("Fail");
            }
        });
    }
});
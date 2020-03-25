$(document).ready(function () {
    $('[data-toggle="popover"]').popover();
    var img;
    var subsite = "";///qa/

    $('#form_NewImage').on('submit', function (e) {
        e.preventDefault();  //prevent form from submitting
        if ($('#Image').val() == '') { $('#ImgAlert').removeClass('hidden'); return false; } else { $('#ImgAlert').addClass('hidden'); }

        UploadImageFile($('#Image').val(), $('#Image'));

        //console.log(parameters);
        //console.log();
        //return false;
    });

    window.UploadImageFile = function (file, element) {
        var scriptURL = "/scripts/php/ImageUpload.php";
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
                    var parameters = { name: dbFileName, caption: $('#caption').val(), seq: $('#seq').val() };

                    //console.log(parameters);

                    NewImage(parameters, function (result) {
                        if (result == "Pass") {

                            alert('Image added successfully.');
                            // window.location.reload();
                        }
                        else {
                            alert('Image could not be added');
                        }

                    });

                });

            };
        };

        img.onerror = function () {
            alert("An error occured while loading image.");
        };
        return return_value;
    }

    window.NewImage = function (parameters, callback) {
        $.ajax({
            type: "POST",
            url: subsite + "AddImage",
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
            $("#deleteForm").html('The image could not be found, please try again.');
        }else{
            $.ajax({
                url: subsite + "_deleteimage/" + img,
                cache: false,
                type: "get",
                dataType: "html",
                success: function (result) {
                    $("#deleteForm").html(result);
                }
            });
        }


    })

    $('#ConfirmDelete').on('hidden.bs.modal', function (e) {

        RemoveImageFile($('#Image').val(), $('#Image'));
    })

    window.RemoveImageFile = function (file, element) {
        var scriptURL = "/scripts/php/ImageRemove.php"
        filename = file.replace(/C:\\fakepath\\/i, '');
        filename = filename.replace(/.jpg/i, '');

        var jqxhr = $.post(scriptURL, {
            //imageData: newImage,
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
    alert("Could Not Remove Image.");
    return_value = false;
});

        jqxhr.done(function () {
        var form = $('#form_DeleteImage');
        var parameters = form.serialize();
            //console.log(parameters);

        ConfirmDelete(parameters, function (result) {
                if (result == "Pass") {

                    alert('Image removed successfully.');
                    window.location.reload();
                }
                else {
                    alert('Image could not be removed');
                }

            });

        });

    }

    window.ConfirmDelete = function (parameters, callback) {
        $.ajax({
            type: "POST",
            url: subsite + "DeleteImage",
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

    $('input[type=number]').change(function () {
        var s = $(this).val(); //sequence value
        var i = $(this).attr('name'); //image id

        SequenceChange(i,s);
    });


    window.SequenceChange = function (i,s) {

        var parameters = { id: i, seq: s };

        $.ajax({
            type: "POST",
            url: "UpdateSequence",
            data: parameters,
            dataType: "json",
            success: function (list) {
                console.log(parameters);
            },
            // THE REQUEST DID NOT GET PROCESSED
            error: function (result) {
                console.log(result);
                
            }
        });

    }
});
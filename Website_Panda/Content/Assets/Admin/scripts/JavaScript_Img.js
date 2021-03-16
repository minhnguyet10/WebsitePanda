function ShowImagePreview(imageUploader, previewImage) {
    if (imageUploader.files && imageUploader.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.target.result);

        }
        reader.readAsDataURL(imageUploader.files[0]);
    }
}

function ShowImagePreview(imageUploader, previewImage1) {
    if (imageUploader.files && imageUploader.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage1).attr('src', e.target.result);

        }
        reader.readAsDataURL(imageUploader.files[0]);
    }
}
function ShowImagePreview(imageUploader, previewImage2) {
    if (imageUploader.files && imageUploader.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage2).attr('src', e.target.result);

        }
        reader.readAsDataURL(imageUploader.files[0]);
    }
}
$(function () {
    if ($("a.confirmDeletion").length) {
        $("a.confirmDeletion").click(() => {
            if (!confirm("Er du nu helt sikker på, at du vil slette?")) return false;
        });
    }

    if ($("div.alert.notification").length) {
        setTimeout(() => {
            $("div.alert.notification").fadeOut();
        }, 3000);
    }
});

function readURL(input) {
    if (input.files && input.files[0]) {
        let reader = new FileReader();

        reader.onload = function (e) {
            $("img#imgpreview").attr("src", e.target.result).width(200).height(200);
        };
        reader.readAsDataURL(input.files[0]);
    }
}

function readURL(input) {
    if (input.files && input.files[0]) {
        let reader = new FileReader();

        reader.onload = function (e) {
            $("img#imgpreviewInvoice").attr("src", e.target.result).width(650).height(650);
        };
        reader.readAsDataURL(input.files[0]);
    }
}



$(function () {
    $('[data-toggle="tooltip"]').tooltip()
});


$(function () {
    $('.datetimepicker').datetimepicker();
});
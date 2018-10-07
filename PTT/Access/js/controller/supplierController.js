var supplier = {
    init: function () {
        supplier.registerEvents();
    },
    registerEvents: function () {
        $('#Cities').on('change', function (e) {
           // e.preventDefault();
            var drlCity = $(this);
            var id = drlCity.val();
            $.ajax({
                url: "/supplier/ChangeDistrict",
                data: { cityid: id },
                dataType: "json",
                type: "POST",
                success: function (district) {
                    $("#District").html(""); // clear before appending new list 
                    $.each(district, function (i, dt) {
                        $("#District").append(
                            $('<option></option>').val(dt.DistrictCode).html(dt.Name));
                    });
                }
            });
        });


    }
}
supplier.init();
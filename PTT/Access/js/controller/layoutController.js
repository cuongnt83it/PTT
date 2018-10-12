var layout = {
    init: function () {
        layout.registerEvents();
    },
    registerEvents: function () {
        $('.btn-topFeedback').on('click', function (e) {
            e.preventDefault();
           // alert("Cập nhật thành công!");
            var projectID = $(this).data('prid');
            var feedID = $(this).data('id');
           // alert(feedID);
            $.ajax({
                url: "/Feedback/UpdateUserRead",
                data: { feedID: feedID },
                dataType: "json",
                type: "POST",
                success: function (contradt) {
                   //  alert("Cập nhật thành công!");
                    window.parent.location.href = "/Feedback/" + projectID;
                }
            })
            
        });
        $('.btn-topMessage').on('click', function (e) {
            e.preventDefault();
            // alert("Cập nhật thành công!");
         
            var msID = $(this).data('id');
            // alert(feedID);
            $.ajax({
                url: "/Mesage/UpdateUserRead",
                data: { msID: msID },
                dataType: "json",
                type: "POST",
                success: function (contradt) {
                    //  alert("Cập nhật thành công!");
                    window.parent.location.href = "/Mesage/DetailMesage/" + msID;
                }
            })

        });
    }
}
layout.init();
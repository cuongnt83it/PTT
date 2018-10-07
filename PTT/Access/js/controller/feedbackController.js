var feedback = {
    init: function () {
        feedback.registerEvents();
    },
    registerEvents: function () {
      
        $('#btnSave').on('click', function (e) {
            e.preventDefault();
            var projectID = $('#hidProjectID').val();
            var chilid = $('#hidID').val();
            
            //alert(processid);
            var content = $('#txtMessage').val();
           // alert(content);
            if (content.trim()=="") {
                alert("Mời nhập nội dung !");
               
            }else{
               // alert(chilid);
            $.ajax({
                url: "/Feedback/CreateFeedBack",
                data: { projectID: projectID, content: content, chilID: chilid },
                dataType: "json",
                type: "POST",
                success: function (contradt) {
                    // alert("Cập nhật thành công!");
                    window.parent.location.reload();
                }
            }).done(function () {

                window.parent.$("ul .treeview-menu").css("display", "block");

            });
            }
        });
     
        $('.btn-Message').on('click', function (e) {
            e.preventDefault();
            $('#MessageModal').modal('show');
            $('#hidID').val($(this).data('id'));
            $('#hidProjectID').val($(this).data('prid'));
           // alert($(this).data('prid'));
        });

    }
}
feedback.init();
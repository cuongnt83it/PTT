var process = {
    init: function () {
        process.registerEvents();
       
    },
    registerEvents: function () {
      

       
        $('#btnSave').on('click', function (e) {
            e.preventDefault();
            var processid = $('#hidProcessID').val();
            var chilid = $('#hidID').val();
            
            //alert(processid);
            var content = $('#txtMessage').val();
           // alert(content);
            if (content.trim()=="") {
                alert("Mời nhập nội dung !");
               
            }else{
            //alert(userid);
            $.ajax({
                url: "/Process/CreateProcess",
                data: { processID: processid, content: content, chilID: chilid },
                dataType: "json",
                type: "POST",
                success: function (contradt) {
                   // alert("Cập nhật thành công!");
                    window.parent.location.reload();
                  
                }
            }).done(function(){ 
               
                    window.parent.$("ul .treeview-menu").css("display", "block");
               
            }) ; 
           
            }
        });
        $('.btn-Process').on('click', function (e) {
            e.preventDefault();
           
            $('#MessageModal').modal('show');
            $('#hidProcessID').val($(this).data('id'));
            $('#hidID').val("null");
        });
        $('.btn-Message').on('click', function (e) {
            e.preventDefault();
            $('#MessageModal').modal('show');
            $('#hidID').val($(this).data('id'));
            $('#hidProcessID').val($(this).data('processid'));
            // alert($(this).data('processid'));
           
        });

    }
}
process.init();
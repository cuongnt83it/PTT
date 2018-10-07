var user = {
    init: function () {
        user.registerEvents();
    },
    registerEvents: function () {
      
        $('.btn-group').on('click', function (e) {
            e.preventDefault();
            
            $('#GroupModal').modal('show');

            $('#hidUserID').val($(this).data('usid'));
            $('.modal-title').html("Quản lý nhóm cho tài khoản: <b>" + $(this).data('usname') +"</b>");
            var userid = $('#hidUserID').val();
           // alert(userid);
            $.ajax({
                url: "/User/ListGroupUser",
                data: { userid: userid },
                dataType: "json",
                type: "POST",
                success: function (lst) {

                    //alert(lst[0].GroupID);
                    var j = 0;
                    $("#drlGroup").html(""); // clear before appending new list 
                    $.each(lst, function (i, dt) {
                        if (dt.isGranted == true) {
                            $("#drlGroup").append(
                                $('<option selected></option>').val(dt.GroupID).html(dt.GroupName));
                        } else {
                            $("#drlGroup").append(
                           $('<option></option>').val(dt.GroupID).html(dt.GroupName));
                        }
                       
                    });

                    //$('#drlGroup option').each(function () {

                        
                    //    for (var i = 0; i < lst.length; i++) {
                    //       // alert(this.value);
                    //        //alert(lst[i].GroupID);
                    //        if (this.value == lst[i].GroupID) {
                    //           //alert(this.value);
                    //            this.selected =true;
                    //        }
                    //        j++;
                    //    }
                        
                    //});
                }
            });

        });
        $('#btnSave').on('click', function (e) {
            var userid = $('#hidUserID').val();
            var listGroup = $('#drlGroup').val();
           // alert(userid);
            $.ajax({
                url: "/User/UpdateGroup",
                data: { userid: userid, groups: listGroup },
                dataType: "json",
                type: "POST",
                success: function (contradt) {
                    alert("Cập nhật thành công!");
                }
            });

        });


    }
}
user.init();
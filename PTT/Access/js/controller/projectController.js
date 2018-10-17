var project = {
    init: function () {
        project.registerEvents();
    },
    registerEvents: function () {
        $('#drlCityID').on('change', function (e) {
            e.preventDefault();
            var drlCity = $(this);
            var id = drlCity.val();
            $.ajax({
                url: "/project/ChangeDistrict",
                data: { cityid: id },
                dataType: "json",
                type: "POST",
                success: function (district) {
                    $("#drlDistrict").html(""); // clear before appending new list 
                    $.each(district, function (i, dt) {
                        $("#drlDistrict").append(
                            $('<option></option>').val(dt.DistrictCode).html(dt.Name));
                    });
                }
            });
            
        });
        $('#btnCreateCode').on('click', function (e) {
            // e.preventDefault();
            //var drlCity = $(this);
            //var id = drlCity.val();
            $.ajax({
                url: "/project/CreateCode",
                data: { code: 'PTT', lengh: 3 },
                dataType: "json",
                type: "POST",
                success: function (district) {
                    $("#txtCode").val(district); // clear before appending new list 

                    //$.each(district, function (i, dt) {
                    //    $("#txtCode").val(dt);
                          
                    //});
                }
            });
        });
        //Chủ đầu tư
        $('#btnContraID').on('click', function (e) {
            $('#ContratorModal').modal('show');
        });
        $('#ContratorChoice').on('click', function (e) {
            var code = $('#drlContrator').val();
            //alert(code);
            $("#contentContrator").html("");
            $('#txtContratorID').val(code);
            $.ajax({
                url: "/project/ContratorDT",
                data: { code: code },
                dataType: "json",
                type: "POST",
                success: function (contradt) {
                    var str = "<p><b>Tên chủ đầu tư: </b>" + contradt.ContraName + "</p>";
                    str += "<p><b>Địa chỉ: </b>" + contradt.Address + "</p>";
                    str += "<p><b>Thông tin liên hệ: </b>" + contradt.FullName + "<b> &nbsp;&nbsp;&nbsp;  Điện thoại: </b>" + contradt.Phone + "</p>";

                    $("#contentContrator").html(str);
                    //$.each(district, function (i, dt) {
                    //    $("#txtCode").val(dt);

                    //});
                }
            });

        });
        $('#txtContratorID').on('change', function (e) {
            e.preventDefault();
            var txtCode = $(this);
            var val = txtCode.val();
            $("#contentContrator").html(""); // clear before appending new list 
            // alert(val);
            $.ajax({
                url: "/project/ContratorDT",
                data: { code: val },
                dataType: "json",
                type: "POST",
                success: function (contradt) {
                    var str = "<p><b>Tên chủ đầu tư: </b>" + contradt.ContraName + "</p>";
                    str += "<p><b>Địa chỉ: </b>" + contradt.Address + "</p>";
                    str += "<p><b>Thông tin liên hệ: </b>" + contradt.FullName + "<b> &nbsp;&nbsp;&nbsp; Điện thoại: </b>" + contradt.Phone + "</p>";

                    $("#contentContrator").html(str);
                    //$.each(district, function (i, dt) {
                    //    $("#txtCode").val(dt);

                    //});
                }
            });
        });
        //Nhà thầu
        $('#btnBuilderID').on('click', function (e) {
            $('#BuilderModal').modal('show');
        });
        $('#BuilderChoice').on('click', function (e) {
            var code = $('#drlBuilder').val();
            //alert(code);
            $("#contentBuilder").html("");
            $('#txtBuilder').val(code);
            $.ajax({
                url: "/project/BuilderDT",
                data: { code: code },
                dataType: "json",
                type: "POST",
                success: function (contradt) {
                    var str = "<p><b>Tên nhà thầu: </b>" + contradt.BuilderName + "</p>";
                    str += "<p><b>Địa chỉ: </b>" + contradt.Address + "</p>";
                    str += "<p><b>Thông tin liên hệ: </b>" + contradt.FullName + "<b>&nbsp;&nbsp; &nbsp; Điện thoại: </b>" + contradt.Phone + "</p>";

                    $("#contentBuilder").html(str);
                    //$.each(district, function (i, dt) {
                    //    $("#txtCode").val(dt);

                    //});
                }
            });

        });

        $('#txtBuilder').on('change', function (e) {
            e.preventDefault();
            var txtCode = $(this);
            var val = txtCode.val();
            $("#contentBuilder").html(""); // clear before appending new list 
            // alert(val);
            $.ajax({
                url: "/project/BuilderDT",
                data: { code: val },
                dataType: "json",
                type: "POST",
                success: function (contradt) {
                    var str = "<p><b>Tên nhà thầu: </b>" + contradt.BuilderName + "</p>";
                    str += "<p><b>Địa chỉ: </b>" + contradt.Address + "</p>";
                    str += "<p><b>Thông tin liên hệ: </b>" + contradt.FullName + "<b>&nbsp;&nbsp; &nbsp; Điện thoại: </b>" + contradt.Phone + "</p>";
                    $("#contentBuilder").html(str);
                    //$.each(district, function (i, dt) {
                    //    $("#txtCode").val(dt);

                    //});
                }
            });
        });

        $('#drlCompetitor').on('change', function (e) {
           e.preventDefault();
           var listCom = $(this);
           var lstVal = listCom.val();
          
         
           var arrCompe= new Array();
           $('#drlCompetitor option').each(function () {
               arrCompe.push(this.value);
           });

            for (var i = 0; i < lstVal.length; i++) {
               
                var index = arrCompe.indexOf(lstVal[i]);
                if (index !== -1) arrCompe.splice(index, 1);
                var str =  "#dv" +  lstVal[i];
                $(str).removeClass("collapse");
               
            }
            for (var i = 0; i < arrCompe.length; i++) {
                var str = "#dv" + arrCompe[i];
                $(str).removeClass("box box-solid collapse ");
                $(str).addClass("box box-solid collapse ");


            }
                      
        });
        //Trang duyệt kết thúc
        $('#drlCitySupplier').on('change', function (e) {
            // e.preventDefault();
            var drlCity = $(this);
            var id = drlCity.val();
           // alert(id);
            $.ajax({
                url: "/project/ChangeSupeelier",
                data: { cityid: id},
                dataType: "json",
                type: "POST",
                success: function (district) {
                    
                    $("#drlSupplier").html(""); // clear before appending new list 
                    $.each(district, function (i, dt) {
                        $("#drlSupplier").append(

                            $('<option></option>').val(dt.ID).html(dt.SupplierName));
                    });
                }
            });

        });
        //Kiểm tra bntSubmit validate form
      
    }
}
project.init();
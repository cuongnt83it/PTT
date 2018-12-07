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

        (function ($) {
            $.fn.simpleMoneyFormat = function () {
                this.each(function (index, el) {
                    var elType = null; // input or other
                    var value = null;
                    // get value
                    if ($(el).is('input') || $(el).is('textarea')) {
                        value = $(el).val().replace(/,/g, '');
                        elType = 'input';
                    } else {
                        value = $(el).text().replace(/,/g, '');
                        elType = 'other';
                    }
                    // if value changes
                    $(el).on('paste keyup', function () {
                        value = $(el).val().replace(/,/g, '');
                        formatElement(el, elType, value); // format element
                    });
                    formatElement(el, elType, value); // format element
                });
                function formatElement(el, elType, value) {
                    var result = '';
                    var valueArray = value.split('');
                    var resultArray = [];
                    var counter = 0;
                    var temp = '';
                    for (var i = valueArray.length - 1; i >= 0; i--) {
                        temp += valueArray[i];
                        counter++
                        if (counter == 3) {
                            resultArray.push(temp);
                            counter = 0;
                            temp = '';
                        }
                    };
                    if (counter > 0) {
                        resultArray.push(temp);
                    }
                    for (var i = resultArray.length - 1; i >= 0; i--) {
                        var resTemp = resultArray[i].split('');
                        for (var j = resTemp.length - 1; j >= 0; j--) {
                            result += resTemp[j];
                        };
                        if (i > 0) {
                            result += ','
                        }
                    };
                    if (elType == 'input') {
                        $(el).val(result);
                    } else {
                        $(el).empty().text(result);
                    }
                }
            };
        }(jQuery));
    }
}
layout.init();
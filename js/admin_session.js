$(document).ready(function () {

    ChkSessionValus();
    Getuserdetails();

    function ChkSessionValus() {
        $.ajax({
            type: "POST",
            url: "wbsvc/AL.asmx/GetUserID",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: onSuccessGetUserId,
            // error: onError,
            cache: false
        });

        function onSuccessGetUserId(data, status) {

            var ChkUsrName = data.d;
            if (ChkUsrName != "") {




            }
            else {
                window.location = "Driver_list.html";


            }

        }
    }

    $('#btnlogout').click(function () {
        logout();
    });


    function logout() {
        $.ajax({
            type: "POST",
            url: "wbsvc/_Driver_Service.asmx/Logout",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: onSuccessGetUserId,
            // error: onError,
            cache: false
        });

        function onSuccessGetUserId(data, status) {

            window.location = "index.html";

        }
    }

    $(".GSTNo").change(function () {
        var regex = /^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$/;
        if (regex.test($(".GSTNo").val())) {
            alert('Invalid GST No...');
            $(".GSTNo").focus();
        }
    });

    $(".allow_numeric").on("input", function (evt) {
        var self = $(this);
        self.val(self.val().replace(/\D/g, ""));
        if ((evt.which < 48 || evt.which > 57)) {
            evt.preventDefault();
        }
    });


    $('.sidebar-menu tree').on('click', 'li', function () {
        $('.sidebar-menu tree li').removeClass('active');
        $(this).addClass('active');
    });

    $('a[data-toggle="tab"]').on('show.bs.tab', function (e) {
        localStorage.setItem('activeTab', $(e.target).attr('href'));
    });
    var activeTab = localStorage.getItem('activeTab');
    if (activeTab) {
        $('#mytabs a[href="' + activeTab + '"]').tab('show');
    }


    function Getuserdetails() {

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "wbsvc/AL.asmx/GetUserName",
            data: "{}",
            dataType: "json",
            success: function (data) {

                $('#lblUserName').text("WELCOME  -  " + data.d);

            },
            error: function (result_cmpname) {
                alert(result_cmpname.d);
            }
        });



        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "wbsvc/AL.asmx/GetUserID",
            data: "{}",
            dataType: "json",
            success: function (UTData) {


                $('#lblUserId').text(UTData.d);


            },
            error: function (result_ut) {
                alert(result_ut.d);
            }
        });







        //$.ajax({
        //    type: "POST",
        //    contentType: "application/json; charset=utf-8",
        //    url: "wbsvc/login.asmx/GetLogo",
        //    data: "{}",
        //    dataType: "json",
        //    success: function (logodata) {


        //        $('#imglogo').attr('src', logodata.d);


        //    },
        //    error: function (result_cmpname) {
        //        alert(result_cmpname.d);
        //    }
        //});
    }


})
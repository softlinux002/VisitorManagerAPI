
$(document).ready(function () {

    var params = (new URL(document.location)).searchParams;
    var status = params.get("status");
    var orderId = params.get("orderid");
    var amount = params.get("amount");
    var email = params.get("email");
    var username = params.get("username");
    var date = params.get("date");

    if (status != null && status != undefined && status != "") {
        //alert("")
        var html = "<h1>Congratulations</h1>";
        html += "<div>Your payment has been processed successfully. Please find the order Details below</div><br />";
        html += "<div>Order ID - "+orderId+"</div><br />";
        html += "<div>Amount - $ "+amount+"</div><br />";
        html += "<div>Registered Email ID - "+email+"</div><br />";
        html += "<div>Username - " + username + "</div><br />";
        html += "<div>Date of Purchase - " + date + "</div><br />";
        html += "<div><button onclick='CancelPopup()'>Cancel</button></div>";
        $.blockUI({
            css: {
                width: "450px",
                height: "500px",
                top: '50%',
                left: '55%',
                margin: (-450 / 2) + 'px 0 0 ' + (-500 / 2) + 'px',
                cursor: 'auto'
            },
            cursor: "Default",
            message: html,

        });
    }

    var username1 = $("#username").val();
    var password1 = $("#password").val();
    if (username1 != "" && username1 != null && username1 != undefined) {
        $("#forUserName").addClass("active");
    }
    if (password1 != "" && password1 != null && password1 != undefined) {
        $("#forPassword").addClass("active");
    }
    $("#loginButton").click(function (event) {
        event.preventDefault();
        var username = $("#username").val();
        var password = $("#password").val();
        //alert(username + " " + password);
        $.ajax({
            url: "/Home/Login",
            data: '{username:"' + username + '",password:"' + password + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                
                var result = data.split(',');
                if (result[0] == "SuperAdmin") {
                    window.location.replace("/Home/Dashboard");
                } else if (result[0] == "Admin") {
                    if (result[1] == "Allowed") {
                        window.location.replace("/Home/AddStaffUser");
                    } else {
                        window.location.replace("/Home/Payment");
                    }
                } else {
                    alert("Invalid Username or Password");
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });

        //var url = "/Home/Index";
        //var name = $("#Name").val();
        //var address = $("#Address").val();
        //$.post(url, { username: username, password: password }, function (data) {
        //    alert("yes");
        //});
    });
});

function CancelPopup() {
    $.unblockUI();
}
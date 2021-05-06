$(document).ready(function () {

    var params = (new URL(document.location)).searchParams;
    var status = params.get("status");

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
        //alert(username + " " + password);
        var isValid = isValidEmailAddress(username);
        if (isValid) {
            $.ajax({
                url: "/Home/ForgetPassword",
                data: '{email:"' + username + '"}',
                cache: false,
                type: "POST",
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {

                    if (data == "Success") {
                        alert("Your password sent to your registered email.");
                        window.location.replace("/Home/Login");
                    } else {
                        alert(data);
                    }
                },
                error: function () {
                    alert("Error occured!!");
                }
            });
        } else {
            alert("Invalid Email.");
        }

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

function isValidEmailAddress(emailAddress) {
    var pattern = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);
    return pattern.test(emailAddress);
}
$(document).ready(function () {
    mainPage.GetPackage();
    mainPage.GetCountry();
    //$("#userTable").dataTable();
    var params = (new URL(document.location)).searchParams;
    var queryString = params.get("loginpage");
    if (queryString != null && queryString != undefined && queryString != "") {
        $("#signupPopup").trigger("click");
    }
});

var Ids = {
    Id: 0,
    userGroupId: 0
}

var mainPage = {

    GetPackage: function () {
        var ddl_Package = "#ddl_Package";
        var html = "<option value='0'>Select Package</option>";
        $(ddl_Package).append(html);
        $.ajax({
            url: "/Service/GetUserGroup",
            data: '',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var opt = new Option(data[i].PackageName, data[i].Id);
                    $(ddl_Package).append(opt);
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
        //$("#ddl_UserGroup").material_select();
    },

    GetCountry: function () {
        var ddl_Country = "#ddl_Country";
        var html = "<option value='0'>Select Country</option>";
        $(ddl_Country).append(html);
        $.ajax({
            url: "/Service/GetCountry",
            data: '',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var opt = new Option(data[i].name, data[i].country_id);
                    $(ddl_Country).append(opt);
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
        //$("#ddl_UserGroup").material_select();
    },

    GetState: function () {
        debugger
        var country = $("#ddl_Country option:selected").val();
        var ddl_State = "#ddl_State";
        var html = "<option value='0'>Select State</option>";
        $(ddl_State).append(html);
        var dataUrl = "/Service/GetState";
        $.ajax({
            url: dataUrl,
            data: '{countryId:"' + country + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                debugger
                for (var i = 0; i < data.length; i++) {
                    var opt = new Option(data[i].name, data[i].zone_id);
                    $(ddl_State).append(opt);
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
        //$("#ddl_UserGroup").material_select();
    },

    CheckAlreadyExistEmail: function () {
        var txt_Email = $("#txt_Email").val();
        $.ajax({
            url: "/Service/CheckAlreadyExistEmail",
            data: '{email:"' + txt_Email + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                //alert(data);
                debugger
                if (data == "Success") {
                    $("#lbl_Email").text("Email already exists");
                    $("#btn_Submit").prop('disabled', true);
                } else {
                    $("#lbl_Email").text("");
                    if ($("#lbl_Username").text() == "") {
                        $("#btn_Submit").prop('disabled', false);
                    }
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
    },

    CheckAlreadyExistUserName: function () {
        var txt_Username = $("#txt_Username").val();
        $.ajax({
            url: "/Service/CheckAlreadyExistUsername",
            data: '{username:"' + txt_Username + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                //alert(data);
                debugger
                if (data == "Success") {
                    $("#lbl_Username").text("This username already exists");
                    $("#btn_Submit").prop('disabled', true);
                } else {
                    $("#lbl_Username").text("");
                    if ($("#lbl_Email").text() == "") {
                        $("#btn_Submit").prop('disabled', false);
                    }
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
    },

    InsertNewUser: function () {
        var packageId = $("#ddl_Package option:selected").val();
        var packageName = $("#ddl_Package option:selected").text();
        var txt_Name = $("#txt_Name").val();
        var txt_Email = $("#txt_Email").val();
        var txt_Mobile = $("#txt_Mobile").val();
        var txt_Country = $("#ddl_Country option:selected").val();
        var txt_State = $("#ddl_State option:selected").val();
        var txt_City = $("#txt_City").val();
        var txt_ZipCode = $("#txt_ZipCode").val();
        var txt_Username = $("#txt_Username").val();
        var txt_Password = $("#txt_Password").val();
        var txt_Confirm = $("#txt_Confirm").val();
        var txt_AdminId = $("#txt_AdminId").val();

        var isValid = mainPage.CheckValidation();
        if (!isValid)
            return false;
        else
            //debugger
            if (txt_Email != "") {
                var isValidEmail = mainPage.isValidEmailAddress(txt_Email);
                if (!isValidEmail) {
                    $("#lbl_Email").text("Enter valid email");
                    return false;
                }
            }
        $.ajax({
            url: "/Service/SignupNewUser",
            data: '{adminAppId:"' + txt_AdminId + '",packageId:"' + packageId + '",username:"' + txt_Username + '",password:"' + txt_Password + '",name:"' + txt_Name + '",email:"' + txt_Email + '",mobile:"' + txt_Mobile + '",country:"' + txt_Country + '",state:"' + txt_State + '",city:"' + txt_City + '",zipCode:"' + txt_ZipCode + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                //alert(data);
                if (data == "Success") {
                    //debugger
                    alert("Your Account Created Successfully");
                    var url = window.location.href;
                    var baseUrl = window.location.origin + "/";
                    if (packageName == "Free") {
                        if (url == baseUrl) {
                            window.location.replace("Home/Login");
                        } else {
                            window.location.replace("Login");
                        }
                    } else {
                        
                        if (url == baseUrl) {
                            window.location.replace("Home/PaymentWithPaypal");
                        } else {
                            window.location.replace("PaymentWithPaypal");
                        }
                    }
                } else {
                    alert("Error occured!!");
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
    },

    isValidEmailAddress: function (emailAddress) {
        var pattern = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);
        return pattern.test(emailAddress);
    },

    CheckValidation: function () {
        var isValid = false;
        
        var isAccept = $("#ckb_Terms").prop("checked");
        if (!isAccept) {
            $("#lbl_Accept").text("Accept terms and conditions is required");
            return false;
        } else {
            $("#lbl_Accept").text("");
        }
        
        var packageId = $("#ddl_Package option:selected").val();
        if (packageId == "0" || packageId == undefined) {
            $("#lbl_Package").text("Select Package");
            return false;
        } else {
            $("#lbl_Package").text("");
            isValid = true;
        }
        var txt_Name = $("#txt_Name").val();
        if (txt_Name == "" || txt_Name == undefined) {
            $("#lbl_Name").text("Enter Name");
            return false;
        } else {
            $("#lbl_Name").text("");
            isValid = true;
        }
        var txt_Email = $("#txt_Email").val();
        if (txt_Email == "" || txt_Email == undefined) {
            $("#lbl_Email").text("Enter Email");
            return false;
        } else {
            $("#lbl_Email").text("");
            isValid = true;
        }
        var txt_Mobile = $("#txt_Mobile").val();
        if (txt_Mobile == "" || txt_Mobile == undefined) {
            $("#lbl_Mobile").text("Enter Mobile Number");
            return false;
        } else {
            $("#lbl_Mobile").text("");
            isValid = true;
        }
        var txt_Country = $("#ddl_Country option:selected").val();
        if (txt_Country == "0" || txt_Country == undefined) {
            $("#lbl_Country").text("Enter Country");
            return false;
        } else {
            $("#lbl_Country").text("");
            isValid = true;
        }
        var txt_State = $("#ddl_State option:selected").val();
        if (txt_State == "0" || txt_State == undefined) {
            $("#lbl_State").text("Enter State");
            return false;
        } else {
            $("#lbl_State").text("");
            isValid = true;
        }
        var txt_City = $("#txt_City").val();
        if (txt_City == "" || txt_City == undefined) {
            $("#lbl_City").text("Enter City");
            return false;
        } else {
            $("#lbl_City").text("");
            isValid = true;
        }
        var txt_ZipCode = $("#txt_ZipCode").val();
        if (txt_ZipCode == "" || txt_ZipCode == undefined) {
            $("#lbl_ZipCode").text("Enter Zip Code");
            return false;
        } else {
            $("#lbl_ZipCode").text("");
            isValid = true;
        }
        var txt_Username = $("#txt_Username").val();
        if (txt_Username == "" || txt_Username == undefined) {
            $("#lbl_Username").text("Enter Username");
            return false;
        } else {
            $("#lbl_Username").text("");
            isValid = true;
        }
        var txt_Password = $("#txt_Password").val();
        if (txt_Password == "" || txt_Password == undefined) {
            $("#lbl_Password").text("Enter Password");
            return false;
        } else {
            $("#lbl_Password").text("");
            isValid = true;
        }
        var txt_Confirm = $("#txt_Confirm").val();
        if (txt_Confirm == "" || txt_Confirm == undefined) {
            $("#lbl_Confirm").text("Enter Conmfirm Password");
            return false;
        } else {
            if (txt_Confirm != txt_Password) {
                $("#lbl_Confirm").text("Enter Same Password");
                return false;
            } else {
                $("#lbl_Confirm").text("");
                isValid = true;
            }
        }

        return isValid;
    },

    OpenServiceManUpdatePopUp: function (id) {
        var flatName = "";
        var area = "";
        $.ajax({
            url: "/Service/GetEditServiceMan",
            data: '{Id:"' + id + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#txt_Id").val(data["ServiceManId"]);
                //$("#ddl_Designation option:selected").html(data["Designation"]);
                $("#txt_Name").val(data["Name"]);
                $("#txt_Mobile").val(data["Mobile"]);
                $("#txt_Address").val(data["Address"]);
                //$("#ddl_Category1 option:selected").html(data["ServiceCategory1"]);
                //$("#ddl_Category2 option:selected").html(data["ServiceCategory2"]);

                if (data["ServiceCategory1"] == "Electrician") {
                    $("#ddl_Category1").val("1");
                } else if (data["ServiceCategory1"] == "Carpenter") {
                    $("#ddl_Category1").val("2");
                } else if (data["ServiceCategory1"] == "Designer") {
                    $("#ddl_Category1").val("3");
                }

                if (data["ServiceCategory2"] == "Electrician") {
                    $("#ddl_Category2").val("1");
                } else if (data["ServiceCategory2"] == "Carpenter") {
                    $("#ddl_Category2").val("2");
                } else if (data["ServiceCategory2"] == "Designer") {
                    $("#ddl_Category2").val("3");
                }

                if (data["Designation"] == "Senior Electrician") {
                    $("#ddl_Designation").val("1");
                } else if (data["Designation"] == "Senior Carpenter") {
                    $("#ddl_Designation").val("2");
                } else if (data["Designation"] == "Senior Designer") {
                    $("#ddl_Designation").val("3");
                }

                $("#userUpdate").show();
                $("#userSubmit").hide();
            },
            error: function () {
                alert("Error occured!!");
            }
        });
        $("#ddl_Designation").material_select();
        $("#ddl_Category1").material_select();
        $("#ddl_Category2").material_select();
        Ids.Id = id;
    },

    OpenServicemanDeletePopUp: function (id) {
        Ids.Id = id;
    },

    UpdateServiceMan: function () {
        var txt_Id = $("#txt_Id").val();
        var ddl_Designation = $("#ddl_Designation option:selected").html();
        var txt_Name = $("#txt_Name").val();
        var txt_Mobile = $("#txt_Mobile").val();
        var txt_Address = $("#txt_Address").val();
        var ddl_Category1 = $("#ddl_Category1 option:selected").html();
        var ddl_Category2 = $("#ddl_Category2 option:selected").html();
        var id = Ids.Id;
        $.ajax({
            url: "/Service/UpdateServiceMan",
            data: '{serviceManId:"' + txt_Id + '",name:"' + txt_Name + '",mobile:"' + txt_Mobile + '",designation:"' + ddl_Designation + '",address:"' + txt_Address + '",serviceCategory1:"' + ddl_Category1 + '",serviceCategory2:"' + ddl_Category2 + '",id:"' + id + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data == "Success") {
                    alert("Recored updated successfully");
                    window.location.replace("ServiceMan");
                } else {
                    alert("Error occured!!");
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
    },

    DeleteServiceMan: function () {
        var id = Ids.Id;
        $.ajax({
            url: "/Service/ServiceManDelete",
            data: '{id:"' + id + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data == "Success") {
                    alert("Recored Deleted successfully");
                    window.location.replace("ServiceMan");
                } else {
                    alert("Error occured!!");
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
    },

    RedirectToLogin: function (e) {
        e.preventDefault();
        window.location.href = "/Home/Login";
    }
}
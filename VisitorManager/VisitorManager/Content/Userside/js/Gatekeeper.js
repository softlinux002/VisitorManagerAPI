$(document).ready(function () {
    
    gateKeeper.GetGatekeeperTableInPartialView();
    $("#userTable").dataTable();

    var params = (new URL(document.location)).searchParams;
    var id = params.get("id");
    if (id != undefined && id != "" && id != null) {
        gateKeeper.OpenUserUpdatePopUp(id);
    }
});

var Ids = {
    Id: 0,
    userGroupId:0
}

var gateKeeper = {
    GetUserGroup: function () {
        var ddl_UserGroup = "#ddl_UserGroup";
        $.ajax({
            url: "/Service/GetUserGroup",
            data: '',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var opt = new Option(data[i].UserGroupName, data[i].Id);
                    $(ddl_UserGroup).append(opt);
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
        //$("#ddl_UserGroup").material_select();
    },

    InserUserDetails: function () {
        var txt_Id = $("#txt_Id").val();
        var txt_StaffName = $("#txt_StaffName").val();
        var txt_Email = $("#txt_Email").val();
        var txt_mobile = $("#txt_mobile").val();
        var txt_Country = $("#txt_Country").val();
        var txt_State = $("#txt_State").val();
        var txt_City = $("#txt_City").val();
        var txt_ZipCode = $("#txt_ZipCode").val();
        var txt_Password = $("#txt_Password").val();
        var txt_Confirm = $("#txt_Confirm").val();
        var isValid = false;
        isValid = gateKeeper.CheckValidation();
        if (isValid) {
            if (txt_Email != "") {
                var isValidEmail = gateKeeper.isValidEmailAddress(txt_Email);
                if (!isValidEmail) {
                    $("#spn_Email").text("Enter valid email");
                    return false;
                }
            }
            $.ajax({
                url: "/Service/AddGatekeeper",
                data: '{gatekeeperId:"' + txt_Id + '",name:"' + txt_StaffName + '",email:"' + txt_Email + '",mobile:"' + txt_mobile + '",country:"' + txt_Country + '",state:"' + txt_State + '",city:"' + txt_City + '",zipCode:"' + txt_ZipCode + '",password:"' + txt_Password + '"}',
                cache: false,
                type: "POST",
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    //alert(data);
                    if (data == "Success") {
                        alert("Record Submitted Successfully");
                        window.location.replace("Gatekeeper");
                    } else {
                        alert("Error occured!!");
                    }
                },
                error: function () {
                    alert("Error occured!!");
                }
            });
        } else {
            return false;
        }
    },

    isValidEmailAddress: function (emailAddress) {
        var pattern = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);
        return pattern.test(emailAddress);
    },

    CheckValidation: function () {
        var isValid = false;
        
        var txt_Name = $("#txt_StaffName").val();
        if (txt_Name == "" || txt_Name == undefined) {
            $("#spn_StaffName").text("Enter Name");
            return false;
        } else {
            $("#spn_StaffName").text("");
            isValid = true;
        }
        var txt_Email = $("#txt_Email").val();
        if (txt_Email == "" || txt_Email == undefined) {
            $("#spn_Email").text("Enter Email");
            return false;
        } else {
            $("#spn_Email").text("");
            isValid = true;
        }
        var txt_Mobile = $("#txt_mobile").val();
        if (txt_Mobile == "" || txt_Mobile == undefined) {
            $("#spn_Mobile").text("Enter Mobile Number");
            return false;
        } else {
            $("#spn_Mobile").text("");
            isValid = true;
        }
        var txt_Country = $("#txt_Country").val();
        if (txt_Country == "" || txt_Country == undefined) {
            $("#spn_Country").text("Enter Country");
            return false;
        } else {
            $("#spn_Country").text("");
            isValid = true;
        }
        var txt_State = $("#txt_State").val();
        if (txt_State == "" || txt_State == undefined) {
            $("#spn_State").text("Enter State");
            return false;
        } else {
            $("#spn_State").text("");
            isValid = true;
        }
        var txt_City = $("#txt_City").val();
        if (txt_City == "" || txt_City == undefined) {
            $("#spn_City").text("Enter City");
            return false;
        } else {
            $("#spn_City").text("");
            isValid = true;
        }
        var txt_ZipCode = $("#txt_ZipCode").val();
        if (txt_ZipCode == "" || txt_ZipCode == undefined) {
            $("#spn_ZipCode").text("Enter Zip Code");
            return false;
        } else {
            $("#spn_ZipCode").text("");
            isValid = true;
        }
        
        var txt_Password = $("#txt_Password").val();
        if (txt_Password == "" || txt_Password == undefined) {
            $("#spn_Password").text("Enter Password");
            return false;
        } else {
            $("#spn_Password").text("");
            isValid = true;
        }
        var txt_Confirm = $("#txt_Confirm").val();
        if (txt_Confirm == "" || txt_Confirm == undefined) {
            $("#spn_Confirm").text("Enter Conmfirm Password");
            return false;
        } else {
            if (txt_Confirm != txt_Password) {
                $("#spn_Confirm").text("Enter Same Password");
                return false;
            } else {
                $("#spn_Confirm").text("");
                isValid = true;
            }
        }

        return isValid;
    },

    CheckValidationForUpdate: function () {
        var isValid = false;

        var txt_Name = $("#txt_StaffName").val();
        if (txt_Name == "" || txt_Name == undefined) {
            $("#spn_StaffName").text("Enter Name");
            return false;
        } else {
            $("#spn_StaffName").text("");
            isValid = true;
        }
        var txt_Email = $("#txt_Email").val();
        if (txt_Email == "" || txt_Email == undefined) {
            $("#spn_Email").text("Enter Email");
            return false;
        } else {
            $("#spn_Email").text("");
            isValid = true;
        }
        var txt_Mobile = $("#txt_mobile").val();
        if (txt_Mobile == "" || txt_Mobile == undefined) {
            $("#spn_Mobile").text("Enter Mobile Number");
            return false;
        } else {
            $("#spn_Mobile").text("");
            isValid = true;
        }
        var txt_Country = $("#txt_Country").val();
        if (txt_Country == "" || txt_Country == undefined) {
            $("#spn_Country").text("Enter Country");
            return false;
        } else {
            $("#spn_Country").text("");
            isValid = true;
        }
        var txt_State = $("#txt_State").val();
        if (txt_State == "" || txt_State == undefined) {
            $("#spn_State").text("Enter State");
            return false;
        } else {
            $("#spn_State").text("");
            isValid = true;
        }
        var txt_City = $("#txt_City").val();
        if (txt_City == "" || txt_City == undefined) {
            $("#spn_City").text("Enter City");
            return false;
        } else {
            $("#spn_City").text("");
            isValid = true;
        }
        var txt_ZipCode = $("#txt_ZipCode").val();
        if (txt_ZipCode == "" || txt_ZipCode == undefined) {
            $("#spn_ZipCode").text("Enter Zip Code");
            return false;
        } else {
            $("#spn_ZipCode").text("");
            isValid = true;
        }

        
        return isValid;
    },


    GetGatekeeperTableInPartialView: function () {
        $.ajax({
            url: "/Home/GetGatekeeperTablePartial",
            //data: {},
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/html; charset=utf-8',
            dataType: 'html',
            success: function (data) {
                $("#gatekeeperTablePartial").html(data);
            },
            error: function (data) {
                alert("Error occured!!");
            }
        });
    },

    OpenUserUpdatePopUp: function (id) {
        
        $.ajax({
            url: "/Service/GetEditGatekeeperDetails",
            data: '{Id:"' + id + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#txt_Id").val(data["GatekeeperId"]);
                $("#txt_StaffName").val(data["Name"]);
                $("#txt_Email").val(data["Email"]);
                $("#txt_mobile").val(data["Mobile"]);
                $("#txt_Country").val(data["Country"]);
                $("#txt_State").val(data["State"]);
                $("#txt_City").val(data["City"]);
                $("#txt_ZipCode").val(data["ZipCode"]);
                //$("label").addClass("active");
            },
            error: function () {
                alert("Error occured!!");
            }
        });

        Ids.Id = id;
    },

    OpenUserDeletePopUp: function (id) {
        Ids.Id = id;
    },

    UpdateGateKeeperDetail: function () {
        var txt_Id = $("#txt_Id").val();
        var txt_StaffName = $("#txt_StaffName").val();
        var txt_Email = $("#txt_Email").val();
        var txt_mobile = $("#txt_mobile").val();
        var txt_Country = $("#txt_Country").val();
        var txt_State = $("#txt_State").val();
        var txt_City = $("#txt_City").val();
        var txt_ZipCode = $("#txt_ZipCode").val();
        var id = Ids.Id;

        isValid = gateKeeper.CheckValidationForUpdate();
        if (isValid) {
            if (txt_Email != "") {
                var isValidEmail = gateKeeper.isValidEmailAddress(txt_Email);
                if (!isValidEmail) {
                    $("#spn_Email").text("Enter valid email");
                    return false;
                }
            }
            $.ajax({
                url: "/Service/GatekeeperDetailUpdate",
                data: '{gatekeeperId:"' + txt_Id + '",name:"' + txt_StaffName + '",email:"' + txt_Email + '",mobile:"' + txt_mobile + '",country:"' + txt_Country + '",state:"' + txt_State + '",city:"' + txt_City + '",zipCode:"' + txt_ZipCode + '",id:"' + id + '"}',
                cache: false,
                type: "POST",
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data == "Success") {
                        alert("Recored updated successfully");
                        window.location.replace("VisitorManager");
                    } else {
                        alert("Error occured!!");
                    }
                },
                error: function () {
                    alert("Error occured!!");
                }
            });
        }
    },

    DeleteGatekeeperDetail: function () {
        var id = Ids.Id;
        $.ajax({
            url: "/Service/GatekeeperDelete",
            data: '{id:"' + id + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data == "Success") {
                    alert("Recored Deleted successfully");
                    window.location.replace("Home/VisitorManager");
                } else {
                    alert("Error occured!!");
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
    },
}


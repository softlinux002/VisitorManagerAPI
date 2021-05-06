$(document).ready(function () {
    
    userInfo.GetUserTableInPartialView();
    userInfo.GetPackage();
    userInfo.GetAdminUser();
    //$("#userTable").dataTable();
    //$("#userUpdate").hide();
    //$("#userSubmit").show();
    $("#userTable").dataTable();
    var params = (new URL(document.location)).searchParams;
    var id = params.get("id");
    if (id != undefined && id != "" && id != null) {
        userInfo.OpenUserUpdatePopUp(id);
    }
});

var Ids = {
    Id: 0,
    userGroupId:0
}

var userInfo = {

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
                    if(i!=0){
                    var opt = new Option(data[i].PackageName, data[i].Id);
                    $(ddl_Package).append(opt);
                    }
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
        //$("#ddl_Admin").material_select();
    },

    GetAdminUser: function () {
        var ddl_Package = "#ddl_Admin";
        var html = "<option value='0'>Select Admin</option>";
        $(ddl_Package).append(html);
        $.ajax({
            url: "/Service/GetAdminUser",
            data: '',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    if (i != 0) {
                        var opt = new Option(data[i].Name, data[i].Id);
                        $(ddl_Package).append(opt);
                    }
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
        //$("#ddl_UserGroup").material_select();
    },

    GetPackageDetails: function () {
        var packageId = $("#ddl_Package option:selected").val();
        $.ajax({
            url: "/Service/GetPackageDetails",
            data: '{Id:"' + packageId + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#PackageName").html(data["PackageName"]);
                $("#PackageDescription").html(data["Description"]);
                $("#PackageAmount").html("$" + data["AnnuallyPrice"]);
            },
            error: function () {
                alert("Error occured!!");
            }
        });

        Ids.Id = id;
    },

    RenewPackage: function () {
        var packageId = $("#ddl_Package option:selected").val();
            $.ajax({
                url: "/Service/RenewPackage",
                data: '{packageId:"' + packageId + '"}',
                cache: false,
                type: "POST",
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    //alert(data);
                    if (data == "Success") {
                        
                        window.location.replace("PaymentWithPaypal");
                    } else {
                        alert("Error occured!!");
                    }
                },
                error: function () {
                    alert("Error occured!!");
                }
            });
    },

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
    CancelModal: function () {
        $('#modal121').closeModal();
    },

    InserUserDetails: function () {
        var adminId = $("#ddl_Admin option:selected").val();
        var txt_StaffId = $("#txt_Id").val();
        var txt_Designation = $("#txt_Designation").val();
        var txt_StaffName = $("#txt_StaffName").val();
        var txt_Email = $("#txt_Email").val();
        var txt_mobile1 = $("#txt_mobile1").val();
        var txt_mobile2 = $("#txt_mobile2").val();
        var txt_Country = $("#txt_Country").val();
        var txt_Address = $("#txt_Address").val();
        var txt_State = $("#txt_State").val();
        var txt_City = $("#txt_City").val();
        var txt_ZipCode = $("#txt_ZipCode").val();
        var txt_Comment = $("#txt_Comment").val();
        var txt_Designation = $("#txt_Designation").val();
        var txt_Password = $("#txt_Password").val();
        var txt_Confirm = $("#txt_Confirm").val();

        var isValid = userInfo.CheckValidation();
        if (!isValid) {
            return false;
        }
        else {
            //debugger
            if (txt_Email != "") {
                var isValidEmail = userInfo.isValidEmailAddress(txt_Email);
                if (!isValidEmail) {
                    $("#lbl_Email").text("Enter valid email");
                    return false;
                }
            }
            $.ajax({
                url: "/Service/AddUser",
                data: '{adminId:"' + adminId+'",password:"' + txt_Password + '",staffId:"' + txt_StaffId + '",staffName:"' + txt_StaffName + '",email:"' + txt_Email + '",mobile1:"' + txt_mobile1 + '",mobile2:"' + txt_mobile2 + '",address:"' + txt_Address + '",state:"' + txt_State + '",city:"' + txt_City + '",zipCode:"' + txt_ZipCode + '",comment:"' + txt_Comment + '",designation:"' + txt_Designation + '"}',
                cache: false,
                type: "POST",
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    //alert(data);
                    if (data == "Success") {
                        alert("Record Submitted Successfully");
                        window.location.replace("AddStaffUser");
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

    isValidEmailAddress: function (emailAddress) {
        var pattern = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);
        return pattern.test(emailAddress);
    },

    CheckValidation: function () {
        var isValid = false;

        var txt_Designation = $("#txt_Designation").val();
        if (txt_Designation == "" || txt_Designation == undefined) {
            $("#lbl_Designation").text("Enter designation");
            return false;
        } else {
            $("#lbl_Designation").text("");
            isValid = true;
        }
        var txt_Name = $("#txt_StaffName").val();
        if (txt_Name == "" || txt_Name == undefined) {
            $("#lbl_StaffName").text("Enter staff name");
            return false;
        } else {
            $("#lbl_StaffName").text("");
            isValid = true;
        }
        var txt_Email = $("#txt_Email").val();
        if (txt_Email == "" || txt_Email == undefined) {
            $("#lbl_Email").text("Enter email");
            return false;
        } else {
            $("#lbl_Email").text("");
            isValid = true;
        }
        var txt_Mobile = $("#txt_mobile1").val();
        if (txt_Mobile == "" || txt_Mobile == undefined) {
            $("#lbl_mobile1").text("Enter Mobile Number");
            return false;
        } else {
            $("#lbl_mobile1").text("");
            isValid = true;
        }
        var txt_Address = $("#txt_Address").val();
        if (txt_Address == "" || txt_Address == undefined) {
            $("#lbl_Address").text("Enter address");
            return false;
        } else {
            $("#lbl_Address").text("");
            isValid = true;
        }
        var txt_State = $("#txt_State").val();
        if (txt_State == "" || txt_State == undefined) {
            $("#lbl_State").text("Enter state");
            return false;
        } else {
            $("#lbl_State").text("");
            isValid = true;
        }
        var txt_City = $("#txt_City").val();
        if (txt_City == "" || txt_City == undefined) {
            $("#lbl_City").text("Enter city");
            return false;
        } else {
            $("#lbl_City").text("");
            isValid = true;
        }
        var txt_ZipCode = $("#txt_ZipCode").val();
        if (txt_ZipCode == "" || txt_ZipCode == undefined) {
            $("#lbl_ZipCode").text("Enter zipcode");
            return false;
        } else {
            $("#lbl_ZipCode").text("");
            isValid = true;
        }
        //var txt_Username = $("#txt_Username").val();
        //if (txt_Username == "" || txt_Username == undefined) {
        //    $("#lbl_Username").text("Enter Username");
        //    return false;
        //} else {
        //    $("#lbl_Username").text("");
        //    isValid = true;
        //}
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

    CheckValidationUpdate: function () {
        var isValid = false;

        var txt_Designation = $("#txt_DesignationUpdate").val();
        if (txt_Designation == "" || txt_Designation == undefined) {
            $("#lbl_DesignationUpdate").text("Enter designation");
            return false;
        } else {
            $("#lbl_DesignationUpdate").text("");
            isValid = true;
        }
        var txt_Name = $("#txt_StaffNameUpdate").val();
        if (txt_Name == "" || txt_Name == undefined) {
            $("#lbl_StaffNameUpdate").text("Enter staff name");
            return false;
        } else {
            $("#lbl_StaffNameUpdate").text("");
            isValid = true;
        }
        var txt_Email = $("#txt_EmailUpdate").val();
        if (txt_Email == "" || txt_Email == undefined) {
            $("#lbl_EmailUpdate").text("Enter email");
            return false;
        } else {
            $("#lbl_EmailUpdate").text("");
            isValid = true;
        }
        var txt_Mobile = $("#txt_mobile1Update").val();
        if (txt_Mobile == "" || txt_Mobile == undefined) {
            $("#lbl_mobile1Update").text("Enter Mobile Number");
            return false;
        } else {
            $("#lbl_mobile1Update").text("");
            isValid = true;
        }
        var txt_Address = $("#txt_AddressUpdate").val();
        if (txt_Address == "" || txt_Address == undefined) {
            $("#lbl_AddressUpdate").text("Enter address");
            return false;
        } else {
            $("#lbl_AddressUpdate").text("");
            isValid = true;
        }
        var txt_State = $("#txt_StateUpdate").val();
        if (txt_State == "" || txt_State == undefined) {
            $("#lbl_StateUpdate").text("Enter state");
            return false;
        } else {
            $("#lbl_StateUpdate").text("");
            isValid = true;
        }
        var txt_City = $("#txt_CityUpdate").val();
        if (txt_City == "" || txt_City == undefined) {
            $("#lbl_CityUpdate").text("Enter city");
            return false;
        } else {
            $("#lbl_CityUpdate").text("");
            isValid = true;
        }
        var txt_ZipCode = $("#txt_ZipCodeUpdate").val();
        if (txt_ZipCode == "" || txt_ZipCode == undefined) {
            $("#lbl_ZipCodeUpdate").text("Enter zipcode");
            return false;
        } else {
            $("#lbl_ZipCodeUpdate").text("");
            isValid = true;
        }
        
        return isValid;
    },

    GetUserTableInPartialView: function () {
        $.ajax({
            url: "/Home/GetUserTablePartial",
            //data: {},
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/html; charset=utf-8',
            dataType: 'html',
            success: function (data) {
                $("#userTablePartial").html(data);
            },
            error: function (data) {
                alert("Error occured!!");
            }
        });
    },

    OpenUserUpdatePopUp: function (id) {
        var flatName = "";
        var area = "";
        $.ajax({
            url: "/Service/GetEditUserDetails",
            data: '{Id:"' + id + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                debugger
                var adminId = data["AdminId"];
                $("#txt_StaffIdUpdate").val(data["StaffId"]);
                $("#txt_StaffNameUpdate").val(data["StaffName"]);
                $("#txt_EmailUpdate").val(data["Email"]);
                $("#txt_mobile1Update").val(data["Mobile1"]);
                $("#txt_mobile2Update").val(data["Mobile2"]);
                $("#txt_AddressUpdate").val(data["Address"]);
                $("#txt_StateUpdate").val(data["State"]);
                $("#txt_CityUpdate").val(data["City"]);
                $("#txt_ZipCodeUpdate").val(data["ZipCode"]);
                $("#txt_CommentUpdate").val(data["Comment"]);
                $("#txt_DesignationUpdate").val(data["Designation"]);
                userInfo.GetAdminUser();
                $("#ddl_Admin").val(adminId);
                $("label").addClass("active");
                //$("#userUpdate").show();
                //$("#userSubmit").hide();
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

    UpdateUserDetail: function (event) {
        event.preventDefault();
        var adminId = $("#ddl_Admin option:selected").val();
        var txt_StaffId = $("#txt_StaffIdUpdate").val();
        var txt_StaffName = $("#txt_StaffNameUpdate").val();
        var txt_Email = $("#txt_EmailUpdate").val();
        var txt_mobile1 = $("#txt_mobile1Update").val();
        var txt_mobile2 = $("#txt_mobile2Update").val();
        var txt_Address = $("#txt_AddressUpdate").val();
        var txt_State = $("#txt_StateUpdate").val();
        var txt_City = $("#txt_CityUpdate").val();
        var txt_ZipCode = $("#txt_ZipCodeUpdate").val();
        var txt_Comment = $("#txt_CommentUpdate").val();
        var txt_Designation = $("#txt_DesignationUpdate").val();
        var id = Ids.Id;

        var isValid = userInfo.CheckValidationUpdate();
        if (!isValid) {
            return false;
        }
        else {
            //debugger
            if (txt_Email != "") {
                var isValidEmail = userInfo.isValidEmailAddress(txt_Email);
                if (!isValidEmail) {
                    $("#lbl_Email").text("Enter valid email");
                    return false;
                }
            }
            $.ajax({
                url: "/Service/UserDetailUpdate",
                data: '{adminId:"'+adminId+'",staffId:"' + txt_StaffId + '",staffName:"' + txt_StaffName + '",email:"' + txt_Email + '",mobile1:"' + txt_mobile1 + '",mobile2:"' + txt_mobile2 + '",address:"' + txt_Address + '",state:"' + txt_State + '",city:"' + txt_City + '",zipCode:"' + txt_ZipCode + '",comment:"' + txt_Comment + '",designation:"' + txt_Designation + '",id:"' + id + '"}',
                cache: false,
                type: "POST",
                async: false,
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data == "Success") {
                        alert("Recored updated successfully");
                        window.location.replace("AddStaffUser");
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

    DeleteUserDetail: function () {
        var id = Ids.Id;
        $.ajax({
            url: "/Service/UserDetailDelete",
            data: '{id:"' + id + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data == "Success") {
                    alert("Recored Deleted successfully");
                    window.location.replace("AddStaffUser");
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

var userGroupInfo = {
    InsertUserGroup: function () {
        var txt_userGroup = $("#txt_userGroup").val();
        $.ajax({
            url: "/Service/UserGroup",
            data: '{userGroup:"' + txt_userGroup + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                //alert(data);
                if (data == "Success") {
                    alert("Record Submitted Successfully");
                } else {
                    alert("Error occured!!");
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
    },

    GetTableInPartialView: function () {
        $.ajax({
            url: "/Home/GetUserGroupPartial",
            //data: {},
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/html; charset=utf-8',
            dataType: 'html',
            success: function (data) {
                $("#tablePartial").html(data);
            },
            error: function (data) {
                alert("Error occured!!");
            }
        });
    },

    OpenUpdatePopUp: function (id) {
        var flatName = "";
        var area = "";
        $.ajax({
            url: "/Service/GetEditUserGroup",
            data: '{Id:"' + id + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#txt_updateGroup").val(data["UserGroupName"]);
                $("#txt_updateGroup").focus();
            },
            error: function () {
                alert("Error occured!!");
            }
        });

        Ids.Id = id;
    },

    OpenDeletePopUp: function (id) {
        Ids.Id = id;
    },

    UpdateUserGroup: function () {
        var userGroupName = $("#txt_updateGroup").val();
        var id = Ids.Id;
        $.ajax({
            url: "/Service/UserGroupUpdate",
            data: '{userGroupName:"' + userGroupName + '",id:"' + id + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data == "Success") {
                    alert("Recored updated successfully");
                    window.location.replace("UserGroup");
                } else {
                    alert("Error occured!!");
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
    },

    DeleteUserGroup: function () {
        var id = Ids.Id;
        $.ajax({
            url: "/Service/UserGroupDelete",
            data: '{id:"' + id + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data == "Success") {
                    alert("Recored Deleted successfully");
                    window.location.replace("UserGroup");
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
$(document).ready(function () {
    //userGroupInfo.GetTableInPartialView();
    //$("#example").dataTable();
    //userInfo.GetUserGroup();
    //userInfo.GetUserTableInPartialView();
    userInfo.GetPackage();
    //$("#userTable").dataTable();
    //$("#userUpdate").hide();
    //$("#userSubmit").show();
});

var Ids = {
    Id: 0,
    userGroupId: 0
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
                    if (i != 0) {
                        var opt = new Option(data[i].PackageName, data[i].Id);
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

}
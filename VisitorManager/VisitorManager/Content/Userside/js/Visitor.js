$(document).ready(function () {
    //userGroupInfo.GetTableInPartialView();
    //$("#example").dataTable();
    //userInfo.GetUserGroup();
    visitorInfo.GetVisitorTableInPartialView();
    //$("#userTable").dataTable();
    //$("#userUpdate").hide();
    //$("#userSubmit").show();
});

var Ids = {
    Id: 0,
    userGroupId: 0
}

var visitorInfo = {
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

    GetVisitorTableInPartialView: function () {
        $.ajax({
            url: "/Home/GetVisitorPartial",
            //data: {},
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/html; charset=utf-8',
            dataType: 'html',
            success: function (data) {
                $("#visitorTablePartial").html(data);
            },
            error: function (data) {
                alert("Error occured!!");
            }
        });
    },

    GetVisitorTableInPartialViewByFilter: function (event) {
        event.preventDefault();
        var name = $("#txt_NameFilter").val();
        var date = $("#txt_DateFilter").val();
        var vehNo = $("#txt_VehNoFilter").val();
        $.ajax({
            url: "/Home/GetVisitorPartialByFilter",
            data: { name: name, date: date, vehNo: vehNo },
            cache: false,
            type: "POST",
            async: false,
            //contentType: 'application/html; charset=utf-8',
            dataType: 'html',
            success: function (data) {
                $("#visitorTablePartial").html(data);
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
}
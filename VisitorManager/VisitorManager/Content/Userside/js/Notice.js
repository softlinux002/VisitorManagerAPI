$(document).ready(function () {
    $("#example").dataTable();
    notice.GetStaffUser();
});

var Ids = {
    Id: 0,
    userGroupId: 0
}

var notice = {
    firstBaseUrl: null,
    firstBaseUrl1: null,
    firstBaseUrl2: null,

    GetStaffUser: function () {
        var ddl_Staff = "#ddl_Staff";
        $(ddl_Staff).empty();
        var html = "<option value='0'>Staff</option>";
        $(ddl_Staff).append(html);
        $.ajax({
            url: "/Service/GetStaffUser",
            data: '',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var opt = new Option(data[i].StaffName, data[i].Id);
                    $(ddl_Staff).append(opt);
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
        $("#ddl_Staff").material_select();
    },

    GetSociety: function () {
        var ddl_Society = "#ddl_Society";
        var html = "<option value='0'>Society</option>";
        $(ddl_Society).append(html);
        $.ajax({
            url: "/Home/GetSociety",
            data: '',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var opt = new Option(data[i].SocietyName, data[i].Id);
                    $(ddl_Society).append(opt);
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
        $("#ddl_Society").material_select();
    },

    
    GetWings: function () {
        var societyId = $("#ddl_Society option:selected").val();
        var ddl_wings = "#ddl_Wings";
        $(ddl_wings).empty();
        var html = "<option value='0'>Wings</option>";
        $(ddl_wings).append(html);
        $.ajax({
            url: "/Home/GetWings",
            data: '{societyId:"' + societyId + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var opt = new Option(data[i].WingName, data[i].Id);
                    $(ddl_wings).append(opt);
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
        $("#ddl_Wings").material_select();
    },

    GetFlatUpdateDetails: function () {
        var societyId = $("#ddl_SocietyUpdate option:selected").val();
        var ddl_Wings = $("#ddl_WingsUpdate option:selected").val();
        var ddl_FlatNumber = "#ddl_FlatNumberUpdate";
        $(ddl_FlatNumber).empty();
        $.ajax({
            url: "/Home/GetFlatNumber",
            data: '{societyId:"' + societyId + '",wingId:"' + ddl_Wings + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var opt = new Option(data[i].FlatNumber, data[i].Id);
                    $(ddl_FlatNumber).append(opt);
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
        $("#ddl_FlatNumberUpdate").material_select();
    },

    GetFilterFlatDetails: function () {
        var ddl_Wings = $("#ddl_FilterWings option:selected").val();
        var ddl_FlatNumber = "#ddl_FilterFlatNo";
        $(ddl_FlatNumber).empty();
        var flatHtml = "<option value='0'>Flat No.</option>";
        $(ddl_FlatNumber).append(flatHtml);
        $.ajax({
            url: "/Service/GetFilterFlatNumber",
            data: '{wingId:"' + ddl_Wings + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    debugger;
                    var opt = new Option(data[i].FlatNumber, data[i].Id);
                    $(ddl_FlatNumber).append(opt);
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
        $("#ddl_FilterFlatNo").material_select();
    },

    GetTableInPartialView: function () {
        $.ajax({
            url: "/Home/GetFlatOwnerPartial",
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

    GetBase64: function (file) {
        var reader = new FileReader();
        reader.readAsBinaryString(file);

        reader.onload = function () {
            console.log(btoa(reader.result));
            return btoa(reader.result);
        };
        reader.onerror = function () {
            console.log('there are some problems');
        };
    },

    loadImageFileAsURL: function () {
        var filesSelected = document.getElementById("attachment1").files;
        if (filesSelected.length > 0) {
            var fileToLoad = filesSelected[0];

            var fileReader = new FileReader();

            fileReader.onload = function (fileLoadedEvent) {
                firstBaseUrl = fileLoadedEvent.target.result;
            };

            var data = fileReader.readAsDataURL(fileToLoad);
        }
    },

    loadImageFileAsURL1: function () {
        var filesSelected = document.getElementById("attachment2").files;
        if (filesSelected.length > 0) {
            var fileToLoad = filesSelected[0];

            var fileReader = new FileReader();

            fileReader.onload = function (fileLoadedEvent) {
                firstBaseUrl1 = fileLoadedEvent.target.result;
            };

            var data = fileReader.readAsDataURL(fileToLoad);
        }
    },

    loadImageFileAsURL2: function () {
        var filesSelected = document.getElementById("attachment3").files;
        if (filesSelected.length > 0) {
            var fileToLoad = filesSelected[0];

            var fileReader = new FileReader();

            fileReader.onload = function (fileLoadedEvent) {
                firstBaseUrl2 = fileLoadedEvent.target.result;
            };

            var data = fileReader.readAsDataURL(fileToLoad);
        }
    },

    InsertNotice: function () {
        var ddl_Staff = $("#ddl_Staff").val();
        var sandToType="";
        var sendTo="";
        if (ddl_Staff != undefined && ddl_Staff != "" && ddl_Staff != null) {
            sandToType = "Staff";
            sendTo=ddl_Staff;
        }
        var ddl_EmailPush = $("#ddl_EmailPush option:selected").html();
        var txt_Title = $("#txt_Title").val();
        var txt_message = $("#txt_message").val();
        var ddl_CommitteeMember = $("#ddl_CommitteeMember").val();
        var ddl_Wings = $("#ddl_Wings").val();
        var ddl_FlatNumber = $("#ddl_FlatNumber option:selected").html();
        var txt_Date = $("#txt_Date").val();
        var radioCheck = "";

        if ($("#rdo_SendNow").is(":checked")) {
            radioCheck = "Send Now";
        }
        if ($("#rdo_Schedule").is(":checked")) {
            radioCheck = "Schedule";
        }

        $.ajax({
            url: "/Service/AddNotice",
            data: '{sendTo:"' + sendTo + '",sendToType:"' + sandToType + '",flatNumber:"' + ddl_FlatNumber + '",emailPushType:"' + ddl_EmailPush + '",noticeTitle:"' + txt_Title + '",message:"' + txt_message + '",scheduleDate:"' + txt_Date + '",scheduleType:"' + radioCheck + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                //alert(data);
                if (data == "Success") {
                    alert("Record Submitted Successfully");
                    window.location.replace("Notice");
                } else {
                    alert("Error occured!!");
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
    },

    OpenUpdatePopUp: function (id) {
        flatOwner.GetSocietyUpdate();


        $.ajax({
            url: "/Service/GetEditFlatOwner",
            data: '{Id:"' + id + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#txt_OwnerIdUpdate").val(data["OwnerId"]);
                $("#txt_MobileUpdate").val(data["MobileNumber"]);
                var sId = data["SocietyId"];
                var wId = data["WingId"];
                var fNo = data["FlatNumber"];
                var pSlot = data["ParkingSlot"];

                $("#txt_FlatTypeUpdate").val(data["FlatType"]);
                $("#txt_emailUpdate").val(data["Email"]);
                $("#txt_OwnerNameUpdate").val(data["OwnerName"]);
                $("#ddl_ParkingSlotUpdate").val(pSlot);
                $("#ddl_ParkingSlotUpdate").material_select();

                $("#ddl_SocietyUpdate").val(sId);
                $("#ddl_SocietyUpdate").material_select();
                flatOwner.GetWingsUpdate();
                $("#ddl_WingsUpdate").val(wId);
                $("#ddl_WingsUpdate").material_select();
                flatOwner.GetFlatUpdateDetails();
                $("#ddl_FlatNumberUpdate option:selected").html(fNo);
                $("#ddl_FlatNumberUpdate").material_select();

                var tenant = data["Tenant"];
                if (tenant != null) {
                    $("#txt_TenantNameUpdate").val(tenant[0].Name);
                    $("#txt_TenantMobileUpdate").val(tenant[0].Mobile);
                    $("#txt_TenantEmailUpdate").val(tenant[0].Email);
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
        Ids.Id = id;
    },

    OpenShippingModeDeletePopUp: function (id) {
        Ids.Id = id;
    },

    UpdateFlatOwner: function () {
        var txt_OwnerId = $("#txt_OwnerIdUpdate").val();
        var txt_Mobile = $("#txt_MobileUpdate").val();
        var ddl_Society = $("#ddl_SocietyUpdate option:selected").val();
        var ddl_Wings = $("#ddl_WingsUpdate option:selected").val();
        var ddl_FlatNumber = $("#ddl_FlatNumberUpdate option:selected").html();
        var txt_FlatType = $("#txt_FlatTypeUpdate").val();
        var txt_email = $("#txt_emailUpdate").val();
        var txt_OwnerName = $("#txt_OwnerNameUpdate").val();
        var ddl_ParkingSlot = $("#ddl_ParkingSlotUpdate option:selected").val();
        var txt_TenantName = $("#txt_TenantNameUpdate").val();
        var txt_TenantMobile = $("#txt_TenantMobileUpdate").val();
        var txt_TenantEmail = $("#txt_TenantEmailUpdate").val();
        var id = Ids.Id;
        $.ajax({
            url: "/Service/UpdateFlatOwner",
            data: '{ownerId:"' + txt_OwnerId + '",ownerName:"' + txt_OwnerName + '",flatNumber:"' + ddl_FlatNumber + '",societyId:"' + ddl_Society + '",wingId:"' + ddl_Wings + '",mobileNumber:"' + txt_Mobile + '",flatType:"' + txt_FlatType + '",email:"' + txt_email + '",parkingSlot:"' + ddl_ParkingSlot + '",tenantName:"' + txt_TenantName + '",tenantMobile:"' + txt_TenantMobile + '",tenantEmail:"' + txt_TenantEmail + '",id:"' + id + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data == "Success") {
                    alert("Recored updated successfully");
                    window.location.replace("FlatOwner");
                } else {
                    alert("Error occured!!");
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
    },

    DeleteShippingMode: function () {
        var id = Ids.Id;
        $.ajax({
            url: "/Service/ShippingModeDelete",
            data: '{id:"' + id + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data == "Success") {
                    alert("Recored Deleted successfully");
                    window.location.replace("ShippingMode");
                } else {
                    alert("Error occured!!");
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
    },

    CancelModal: function () {
        $('#modal1').closeModal();
    },

    ExportToExcel: function () {
        $("#example").table2excel({
            filename: "Table.xls"
        });
    }
}
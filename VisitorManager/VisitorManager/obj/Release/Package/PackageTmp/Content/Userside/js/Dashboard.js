$(document).ready(function () {
    dashboard.GetUserTablePartialView();
    $("#userTable").dataTable();
    //$("#complaintTable").dataTable();
});

var Ids = {
    Id: 0,
    userGroupId: 0
}

var dashboard = {

    GetUserTablePartialView: function () {

        $.ajax({
            url: "/Home/GetDashboardUserTablePartial",
            //data: { fromDate: fromDate, toDate: toDate },
            data: {},
            cache: false,
            type: "POST",
            async: false,
            //contentType: 'application/html; charset=utf-8',
            dataType: 'html',
            success: function (data) {
                $("#userTablePartial").html(data);
            },
            error: function (data) {
                alert("Error occured!!");
            }
        });
    },

    GetEventTablePartialViewByDate: function () {
        //debugger
        var fromDate = $("#J-demo-011").val();
        var toDate = $("#J-demo-0102").val();
        if (fromDate != "" && toDate != "") {
            $.ajax({
                url: "/Home/GetEventDetailsPartialByDate",
                data: { fromDate: fromDate, toDate: toDate },
                //data: {},
                cache: false,
                type: "POST",
                async: false,
                //contentType: 'application/html; charset=utf-8',
                dataType: 'html',
                success: function (data) {
                    $("#dv_EventTablePartial").html(data);
                },
                error: function (data) {
                    alert("Error occured!!");
                }
            });
        }
    },

    ReminderEmail: function (id) {
        $.ajax({
            url: "/Service/EmailReminder",
            data: '{Id:"' + id + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data == "Success") {
                    alert("Email sent to user");
                } else {
                    alert("Error occured in email sending");
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });

    },

    GetComplaintTablePartialView: function () {
        //debugger
        var date = $("#J-demo-04").val();
        var status = $("#complaint_Status").text().trim();
        if (status == "Status") {
            status="";
        }
        $.ajax({
            url: "/Home/GetComplaintDetailsPartial",
            data: { date: date, status: status },
            //data: {},
            cache: false,
            type: "POST",
            async: false,
            //contentType: 'application/html; charset=utf-8',
            dataType: 'html',
            success: function (data) {
                $("#dv_ComplaintTablePartial").html(data);
            },
            error: function (data) {
                alert("Error occured!!");
            }
        });
    },

    GetPendingPaymentTablePartialView: function () {
        
        var date = $("#J-demo-02").val();
        var radioChecked = 'All';
        if ($("#rdb_All").is(":checked")) {
            radioChecked = "All";
            $("#rdb_All").attr("checked", "checked");
        } else if ($("#rdb_Electricity").is(":checked")) {
            radioChecked = "Electricity";
            $("#rdb_Electricity").attr("checked", "checked");
        } else if ($("#rdb_PowerBackup").is(":checked")) {
            radioChecked = "PowerBackup";
        } else if ($("#rdb_Maintenance").is(":checked")) {
            radioChecked = "Maintenance";
        } else if ($("#rdb_Gas").is(":checked")) {
            radioChecked = "Gas";
        }

        $.ajax({
            url: "/Home/GetPendingPaymentPartial",
            data: { date: date, type: radioChecked },
            //data: {},
            cache: false,
            type: "POST",
            async: false,
            //contentType: 'application/html; charset=utf-8',
            dataType: 'html',
            success: function (data) {
                $("#dv_PendingPaymentTablePartial").html(data);
            },
            error: function (data) {
                alert("Error occured!!");
            }
        });
    },

    ShowEventDetails: function (id) {
        var gid = "";
        $.ajax({
            url: "/Service/GetSingleEventDetails",
            data: '{Id:"' + id + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                $("#lbl_EventName").text(data["EventName"]);
                $("#lbl_VenueName").html(data["Venue"]);
                $("#lbl_Description").html(data["Description"]);
                $("#lbl_EventDate").html(data["EventDate"]);
                $("#lbl_EventTime").html(data["EventTime"]);
                $("#lbl_EntryFee").html(data["EntryFee"]);
                $("#lbl_LastDateOfRegistration").html(data["LastDateOfRegistration"]);
                $("#lbl_Image").attr("src", data["Image"]);
                $("#lbl_Image").attr("alt", data["Image"]);
            },
            error: function () {
                alert("Error occured!!");
            }
        });

        Ids.Id = id;

        $("#ddl_DurationUpdate").material_select();

    },

    EditAdmin: function () {

    },

    UpdateAdmin: function () {

    },

    HitFDFDetails: function () {
        $.ajax({
            url: "/Service/CreatePDFFile",
            data: '',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                
            },
            error: function () {
                alert("Error occured!!");
            }
        });
    },

    CancelModal: function () {
        $('#modal211').closeModal();
    },

    ExportToExcel: function () {
        $("#example").table2excel({
            filename: "Table.xls"
        });
    }
}
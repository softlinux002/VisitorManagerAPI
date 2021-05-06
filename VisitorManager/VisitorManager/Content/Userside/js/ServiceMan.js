$(document).ready(function () {
    $("#example").dataTable();
    $("#userUpdate").hide();
    $("#userSubmit").show();
});

var Ids = {
    Id: 0,
    userGroupId: 0
}

var serviceMan = {
    InsertServiceMan: function () {
        var txt_Id = $("#txt_Id").val();
        var ddl_Designation = $("#ddl_Designation option:selected").html();
        var txt_Name = $("#txt_Name").val();
        var txt_Mobile = $("#txt_Mobile").val();
        var txt_Address = $("#txt_Address").val();
        var ddl_Category1 = $("#ddl_Category1 option:selected").html();
        var ddl_Category2 = $("#ddl_Category2 option:selected").html();

        $.ajax({
            url: "/Service/AddServiceMan",
            data: '{serviceManId:"' + txt_Id + '",name:"' + txt_Name + '",mobile:"' + txt_Mobile + '",designation:"' + ddl_Designation + '",address:"' + txt_Address + '",serviceCategory1:"' + ddl_Category1 + '",serviceCategory2:"' + ddl_Category2 + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                //alert(data);
                if (data == "Success") {
                    alert("Record Submitted Successfully");
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
}
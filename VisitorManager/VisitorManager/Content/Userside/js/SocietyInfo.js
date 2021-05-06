$(document).ready(function () {
    $("#loginButton").click(function (event) {
        event.preventDefault();
        var username = $("#username").val();
        var password = $("#password").val();
        //alert(username + " " + password);
        $.ajax({
            url: "/Home/Index",
            data: '{username:"' + username + '",password:"' + password + '"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                //alert(data);
                if (data == "Success") {
                    window.location.replace("/Home/Dashboard");
                } else {
                    alert("Invalid Username or Password");
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
    });

    societyInfo.AddWingDetail();
});

var rowNumber = 0;


var societyInfo = {
    
    InsertSociety: function () {
        var wingArray = [];
        var societyName = $("#societyName").val();
        var address = $("#address").val();
        var city = $("#city").val();
        var state = $("#state").val();
        var zipcode = $("#zipcode").val();
        var flat = "" //$("#flat").val();
        var vat = $("#vat").val();
        var phone = $("#phone").val();
        var suppoertPhone = $("#suppoertPhone").val();
        var mail = $("#mail").val();
        var username = $("#username").val();
        var password = $("#password").val();

        $('#wingDetails tr').each(function () {
            var WingDetails = {};
            var rowClass = $(this).attr("class");
            var suffix = rowClass.match(/\d+/);
            WingDetails.WingName = $('#wingDetails tr td div #wingname' + suffix).val();
            WingDetails.TotalFlats = $('#wingDetails tr td div #noOfFlat' + suffix).val();
            WingDetails.FlatsPerFloor = $('#wingDetails tr td div #flatPerFloor' + suffix).val();
            WingDetails.StartingPrefix = $('#wingDetails tr td div #prifix' + suffix).val();
            wingArray.push(WingDetails);
        });


        var society = {
            societyName: societyName,
            address: address,
            city: city,
            state: state,
            zipcode: zipcode,
            flat: flat,
            vat: vat,
            phone: phone,
            suppoertPhone: suppoertPhone,
            mail: mail,
            username: username,
            password: password,
            wingArray: wingArray
            //Interests: ["Code", "Coffee", "Stackoverflow"]
            //public class DashboardViewModel
            //{
            //    public string Name {set;get;}
            //    public string Location {set;get;}
            //    public List<string> Interests {set;get;}
            //}
        };

        alert("Yes Working");
        $.ajax({
            url: "/Home/Society",
            data: JSON.stringify(society),  //'{societyName:"' + societyName + '",address:"' + address + '",city:"' + city + '",state:"' + state + '",zipcode:"' + zipcode + '",flat:"' + flat + '",vat:"' + vat + '",suppoertPhone:"' + suppoertPhone + '",phone:"' + phone + '",mail:"' + mail + '",username:"' + username + '",password:"' + password + '",wingArray:"'+wingArray+'"}',
            cache: false,
            type: "POST",
            async: false,
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                //alert(data);
                if (data == "Success") {
                    //window.location.replace("/Home/Dashboard");
                } else {
                    //alert("Invalid Username or Password");
                }
            },
            error: function () {
                alert("Error occured!!");
            }
        });
    },

    AddWingDetail: function () {
        rowNumber = rowNumber + 1;
        var wingDetails = $("#wingDetails");
        var winghtml = "<tr class='remove" + rowNumber + "'><td><div class='input-field col l3 m4 s12'><input id='wingname" + rowNumber + "' type='text' class='validate'><label for='phone" + rowNumber + "'>Name of Wing</label></div>";
        winghtml += "<div class='input-field col l3 m4 s12'><input id='noOfFlat" + rowNumber + "'' type='text' class='validate'><label for='noOfFla" + rowNumber + "'t'>No of Flats</label></div>";
        winghtml += "<div class='input-field col l3 m4 s12'><input id='flatPerFloor" + rowNumber + "'' type='text' class='validate'><label for='flatPerFloor" + rowNumber + "''>Flats Per floor</label></div>";
        winghtml += "<div class='input-field col l3 m4 s12'><input id='prifix" + rowNumber + "'' type='text' class='validate'><label for='prifix" + rowNumber + "''>Starting Prifix</label></div><div class='input-field col l3 m4 s12'><a id='delete" + rowNumber + "' class='waves-effect waves-light btn blue btn2 m-b-xs' onclick='societyInfo.DeleteRow(id)'>Delete</a></div></td></tr>";
        $("#wingDetails").prepend(winghtml);
    },

    DeleteRow: function (id) {
        $("#" + id).parent().parent().parent().remove();
    }
}
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

    $("#txt_lastReadingDate").datepicker({
        dateFormat: "yy-mm-dd"
    });
    //$("#txt_lastReadingDate").datepicker();
});

var serviceFee = {
    InsertElectricityBill: function () {
        var txt_flatNumber = $("#txt_flatNumber").val();
        var txt_totalUnits = $("#txt_totalUnits").val();
        var txt_lastReading = $("#txt_lastReading").val();
        var txt_lastReadingDate = $("#txt_lastReadingDate").val();
        var txt_newReading = $("#txt_newReading").val();
        var txt_newReadingDate = $("#txt_newReadingDate").val();
        var txt_PricePerUnit = $("#txt_PricePerUnit").val();
        var ddl_taxonunit = $("#ddl_taxonunit option:selected").html();
        var txt_TaxOnUnit = $("#txt_TaxOnUnit").val();
        var ddl_typeoflatefee = $("#ddl_typeoflatefee option:selected").html();
        var txt_lateFee = $("#txt_lateFee").val();
        var txt_subTotal = $("#txt_subTotal").val();
        var txt_TotalBill = $("#txt_TotalBill").val();
        var txt_fromDate = $("#txt_fromDate").val();
        var txt_toDate = $("#txt_toDate").val();
        var txt_lastDateForPayment = $("#txt_lastDateForPayment").val();
        var totalUnitPrice = parseFloat(txt_totalUnits) * parseFloat(txt_PricePerUnit);
        var totalUnitTax = parseFloat(txt_totalUnits) * parseFloat(txt_TaxOnUnit);
        var totalBill = totalUnitPrice + totalUnitTax;
        var subTotal = totalUnitPrice;
        //alert(username + " " + password);
        $.ajax({
            url: "/Home/SubmitElectricityBill",
            data: '{txt_flatNumber:"' + txt_flatNumber + '",txt_totalUnits:"' + txt_totalUnits + '",txt_lastReading:"' + txt_lastReading + '",txt_lastReadingDate:"' + txt_lastReadingDate + '",txt_newReading:"' + txt_newReading + '",txt_newReadingDate:"' + txt_newReadingDate + '",txt_PricePerUnit:"' + txt_PricePerUnit + '",ddl_taxonunit:"' + ddl_taxonunit + '",txt_TaxOnUnit:"' + txt_TaxOnUnit + '",ddl_typeoflatefee:"' + ddl_typeoflatefee + '",txt_lateFee:"' + txt_lateFee + '",txt_subTotal:"' + txt_subTotal + '",txt_TotalBill:"' + txt_TotalBill + '",txt_fromDate:"' + txt_fromDate + '",txt_toDate:"' + txt_toDate + '",txt_lastDateForPayment:"' + txt_lastDateForPayment + '"}',
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

    InsertPowerBackupBill: function () {
        var pb_FlatNumber = $("#pb_FlatNumber").val();
        var pb_TotalUnits = $("#pb_TotalUnits").val();
        var pb_UnitPrice = $("#pb_UnitPrice").val();
        var pb_ddl_Taxonunit = $("#pb_ddl_Taxonunit option:selected").html();
        var pb_Tax = $("#pb_Tax").val();
        var txt_LateFeeType = $("#pb_ddl_Typeoflatefee option:selected").html();
        var pb_TaxOnUnit = $("#pb_TaxOnUnit").val();
        var pb_LateFee = $("#pb_LateFee").val();
        var pb_Surcharge = $("#pb_Surcharge").val();
        var pb_SubTotal = $("#pb_SubTotal").val();
        var pb_TotalBill = $("#pb_TotalBill").val();
        var pb_PaymentLastDate = $("#pb_PaymentLastDate").val();
        var pb_FromDate = $("#pb_FromDate").val();
        var pb_ToDate = $("#pb_ToDate").val();

        //alert(username + " " + password);
        $.ajax({
            url: "/Home/SubmitPowerBackupBill",
            data: '{flatNumber:"' + pb_FlatNumber + '",totalUnit:"' + pb_TotalUnits + '",unitPrice:"' + pb_UnitPrice + '",subTotal:"' + pb_SubTotal + '",tax:"' + pb_Tax + '",taxType:"' + pb_ddl_Taxonunit + '",lateFeeType:"' + txt_LateFeeType + '",lateFee:"' + pb_LateFee + '",surcharge:"' + pb_Surcharge + '",netAmount:"' + pb_TotalBill + '",lastDateForPayment:"' + pb_PaymentLastDate + '",fromDate:"' + pb_FromDate + '",toDate:"' + pb_ToDate + '"}',
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

    InsertGasBill: function () {
        var gas_FlatNumber = $("#gas_FlatNumber").val();
        var gas_TotalUnit = $("#gas_TotalUnit").val();
        var gas_UnitPrice = $("#gas_UnitPrice").val();
        var gas_ddl_Taxonunit = $("#gas_ddl_Taxonunit option:selected").html();
        var gas_TaxOnUnit = $("#gas_TaxOnUnit").val();
        var gas_ddl_Typeoflatefee = $("#gas_ddl_Typeoflatefee option:selected").html();
        var gas_LateFee = $("#gas_LateFee").val();
        var gas_SubTotal = $("#gas_SubTotal").val();
        var gas_TotalBill = $("#gas_TotalBill").val();
        var gas_PaymentLastDate = $("#gas_PaymentLastDate").val();
        var gas_FromDate = $("#gas_FromDate").val();
        var gas_ToDate = $("#gas_ToDate").val();

        //alert(username + " " + password);
        $.ajax({
            url: "/Home/SubmitGasBill",
            data: '{flatNumber:"' + gas_FlatNumber + '",totalUnits:"' + gas_TotalUnit + '",pricePerUnit:"' + gas_UnitPrice + '",taxOnUnit:"' + gas_TaxOnUnit + '",taxType:"' + gas_ddl_Taxonunit + '",lateFeeType:"' + gas_ddl_Typeoflatefee + '",lateFee:"' + gas_LateFee + '",subTotal:"' + gas_SubTotal + '",netAmount:"' + gas_TotalBill + '",lastDateForPayment:"' + gas_PaymentLastDate + '",fromDate:"' + gas_FromDate + '",toDate:"' + gas_ToDate + '"}',
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

    InsertMaintenanceBill: function () {
        var mt_FlatNumber = $("#mt_FlatNumber").val();
        var ddl_MaintenanceType = $("#ddl_MaintenanceType option:selected").html();
        var mt_TotalArea = $("#mt_TotalArea").val();
        var mt_PricePerSqFoot = $("#mt_PricePerSqFoot").val();
        var mt_ddl_TypeofTax = $("#mt_ddl_TypeofTax option:selected").html();
        var mt_Tax = $("#mt_Tax").val();
        var mt_Typeoflatefee = $("#mt_ddl_Typeoflatefee option:selected").html();
        var mt_LateFee = $("#mt_LateFee").val();
        var mt_SubTotal = $("#mt_SubTotal").val();
        var mt_TotalBill = $("#mt_TotalBill").val();
        var mt_PaymentLastDate = $("#mt_PaymentLastDate").val();
        var mt_FromDate = $("#mt_FromDate").val();
        var mt_ToDate = $("#mt_ToDate").val();

        //alert(username + " " + password);
        $.ajax({
            url: "/Home/SubmitMaintenanceBill",
            data: '{flatNumber:"' + mt_FlatNumber + '",maintenanceType:"' + ddl_MaintenanceType + '",totalAreaInSqureFt:"' + mt_TotalArea + '",ratePerSquareFt:"' + mt_PricePerSqFoot + '",subTotal:"' + mt_SubTotal + '",taxType:"' + mt_ddl_TypeofTax + '",tax:"' + mt_Tax + '",lateFeeType:"' + mt_Typeoflatefee + '",lateFee:"' + mt_LateFee + '",netAmount:"' + mt_TotalBill + '",lastDateForPayment:"' + mt_PaymentLastDate + '",fromDate:"' + mt_FromDate + '",toDate:"' + mt_ToDate + '"}',
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

    UnitPriceChange: function () {
        var txt_totalUnits = $("#txt_totalUnits").val();
        var txt_PricePerUnit = $("#txt_PricePerUnit").val();
        if (txt_totalUnits != "" && txt_PricePerUnit != "") {
            var totalUnitPrice = parseFloat(txt_totalUnits) * parseFloat(txt_PricePerUnit);
            $("#txt_subTotal").val(totalUnitPrice);
            var txt_TaxOnUnit = $("#txt_TaxOnUnit").val();
            if (txt_TaxOnUnit != "") {
                var totalUnitTax = parseFloat(txt_totalUnits) * parseFloat(txt_TaxOnUnit);
                var totalBill = totalUnitPrice + totalUnitTax;
                $("#txt_TotalBill").val(totalBill);
            }
        }
    },

    UnitPricePowerBackupChange: function () {
        var txt_totalUnits = $("#txt_totalUnits").val();
        var txt_PricePerUnit = $("#txt_PricePerUnit").val();
        if (txt_totalUnits != "" && txt_PricePerUnit != "") {
            var totalUnitPrice = parseFloat(txt_totalUnits) * parseFloat(txt_PricePerUnit);
            $("#txt_subTotal").val(totalUnitPrice);
            var txt_TaxOnUnit = $("#txt_TaxOnUnit").val();
            if (txt_TaxOnUnit != "") {
                var totalUnitTax = parseFloat(txt_totalUnits) * parseFloat(txt_TaxOnUnit);
                var totalBill = totalUnitPrice + totalUnitTax;
                $("#txt_TotalBill").val(totalBill);
            }
        }
    },

    UnitPriceGasChange: function () {
        var txt_totalUnits = $("#txt_totalUnits").val();
        var txt_PricePerUnit = $("#txt_PricePerUnit").val();
        if (txt_totalUnits != "" && txt_PricePerUnit != "") {
            var totalUnitPrice = parseFloat(txt_totalUnits) * parseFloat(txt_PricePerUnit);
            $("#txt_subTotal").val(totalUnitPrice);
            var txt_TaxOnUnit = $("#txt_TaxOnUnit").val();
            if (txt_TaxOnUnit != "") {
                var totalUnitTax = parseFloat(txt_totalUnits) * parseFloat(txt_TaxOnUnit);
                var totalBill = totalUnitPrice + totalUnitTax;
                $("#txt_TotalBill").val(totalBill);
            }
        }
    },

    UnitPriceMaintenanceChange: function () {
        var txt_totalUnits = $("#txt_totalUnits").val();
        var txt_PricePerUnit = $("#txt_PricePerUnit").val();
        if (txt_totalUnits != "" && txt_PricePerUnit != "") {
            var totalUnitPrice = parseFloat(txt_totalUnits) * parseFloat(txt_PricePerUnit);
            $("#txt_subTotal").val(totalUnitPrice);
            var txt_TaxOnUnit = $("#txt_TaxOnUnit").val();
            if (txt_TaxOnUnit != "") {
                var totalUnitTax = parseFloat(txt_totalUnits) * parseFloat(txt_TaxOnUnit);
                var totalBill = totalUnitPrice + totalUnitTax;
                $("#txt_TotalBill").val(totalBill);
            }
        }
    },
}

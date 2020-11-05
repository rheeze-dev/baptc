﻿var popup, dataTable;
var entity = 'Inspector';
var apiurl = '/api/' + entity;

$(document).ready(function () {
    //alert(entity);
    //var organizationId = $('#organizationId').val();
    dataTable = $('#grid').DataTable({
        "ajax": {
            "url": apiurl + '/GetFarmersTruck',
            "type": 'GET',
            "datatype": 'json'
        },
        "order": [[0, 'desc']],
        "columns": [
            //{ "data": "commodityDate" },
            {
                "data": function (data) {
                    var d = new Date(data["timeIn"]);
                    var output = monthNames[d.getMonth()] + " " + d.getDate() + ", " + d.getFullYear() + " - " + setClockTime(d);
                    var spanData = "<span style = 'display:none;'> " + data["timeIn"] + "</span>";
                    return spanData + output;
                }
            },
            //{
            //    "data": function (data) {
            //        var d = new Date(data["timeIn"]);
            //        var output = setClockTime(d);
            //        return output;
            //    }
            //},
            { "data": "plateNumber" },
            { "data": "parkingNumber" },
            {
                "data": function (data) {
                    var d = new Date(data["dateInspected"]);
                    var dateOut = monthNames[d.getMonth()] + " " + d.getDate() + ", " + d.getFullYear() + " - " + setClockTime(d);
                    var output = dateOut;
                    if (data["dateInspected"] == null) {
                        output = "";
                    }
                    return output;
                }
            },
            //{
            //    "data": function (data) {
            //        var d = new Date(data["date"]);
            //        var output = setClockTime(d);
            //        if (data["date"] == null) {
            //            output = "";
            //        }
            //        return output;
            //    }
            //},
            { "data": "farmersName" },
            { "data": "organization" },
            { "data": "commodity" },
            { "data": "volume" },
            { "data": "barangay" },
            { "data": "remarks" },
            { "data": "controlNumber" },
            //{ "data": "municipality" },
            //{ "data": "province" },
            //{ "data": "facilitatorsName" },
            //{ "data": "accreditationChecker" },
            //{ "data": "inspector" },
            {
                "data": function (data) {
                    var status = "<span class='txt-success'>Completed</span>";
                    if (data["dateInspected"] == null && data["timeOut"] == null) {
                        status = "<label class='txt-info'>Active</label>";
                    }
                    else if (data["dateInspected"] == null && data["timeOut"] != null) {
                        status = "<label class='txt-info'>Unchecked</label>";
                    }
                    return status;
                }
            },
            {
                "data": function (data) {
                    var btnEdit = "<a class='btn btn-default btn-xs' onclick=ShowPopup('/TradingInspector/AddEditFarmersTruck?id=" + data["ticketingId"] + "')><i class='fa fa-pencil' title='Edit'></i></a>";
                    var btnView = "<a class='btn btn-default btn-xs' style='margin-left:5px' onclick=ShowPopup('/TradingInspector/ViewFarmersTruck?id=" + data["ticketingId"] + "')><i class='fa fa-external-link' title='More'></i></a>";
                    //return btnEdit;
                    var outPut = btnEdit + btnView;
                 
                    return outPut;
                }
            }
        ],
        "language": {
            "emptyTable": "no data found."
        },
        "lengthChange": false,
    });
});
const monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"];
function setClockTime(d) {
    var h = d.getHours();
    var m = d.getMinutes();
    var suffix = "AM";
    if (h > 11) { suffix = "PM"; }
    if (h > 12) { h = h - 12; }
    if (h == 0) { h = 12; }
    if (h < 10) { h = "0" + h; }
    if (m < 10) { m = "0" + m; }
    return h + ":" + m + " " + suffix;
}
function ShowPopup(url) {
    var modalId = 'modalDefault';
    var modalPlaceholder = $('#' + modalId + ' .modal-dialog .modal-content');
    $.get(url)
        .done(function (response) {
            modalPlaceholder.html(response);
            popup = $('#' + modalId + '').modal({
                keyboard: false,
                backdrop: 'static'
            });
        });
}


function SubmitAddEdit(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        var data = $(form).serializeJSON();
        //data = { priceCommodity: data };
        data = JSON.stringify(data);
        //var data = {
        //    priceCommodity: "petsay"
        //};
        //alert(data);
        //return true;
        $.ajax({
            type: 'POST',
            url: "/api/Inspector/PostFarmersTruck",
            //url: '/PriceCommodity/PostPriceCommodity',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                if (data.success) {
                    popup.modal('hide');
                    ShowMessage(data.message);
                    dataTable.ajax.reload();
                } else {
                    ShowMessageError(data.message);
                }
            }
        });

    }
    return false;
}

function Delete(id) {
    swal({
        title: "Are you sure want to Delete?",
        text: "You will not be able to restore the file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#dd4b39",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            type: 'DELETE',
            url: apiurl + '/Farmers/' + id,
            success: function (data) {
                if (data.success) {
                    ShowMessage(data.message);
                    dataTable.ajax.reload();
                } else {
                    ShowMessageError(data.message);
                }
            }
        });
    });


}





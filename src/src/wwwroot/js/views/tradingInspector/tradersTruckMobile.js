﻿var popup, dataTable;
var entity = 'Inspector';
var apiurl = '/api/' + entity;

$(document).ready(function () {
    //alert(entity);
    var organizationId = $('#organizationId').val();
    dataTable = $('#grid').DataTable({
        "ajax": {
            "url": apiurl + '/GetTradersTruck',
            "type": 'GET',
            "datatype": 'json'
        },
        "order": [[0, 'desc']],
        "columns": [
            {
                "data": function (data) {
                    var d = new Date(data["timeIn"]);
                    var output = monthNames[d.getMonth()] + " " + d.getDate() + ", " + d.getFullYear() + " - " + setClockTime(d);
                    var spanData = "<span style = 'display:none;'> " + data["timeIn"] + "</span>";
                    return spanData + output;
                }
            },
            { "data": "plateNumber" },
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
            { "data": "inspector" },
            {
                "data": function (data) {
                    var status = "<span class='txt-success'>Completed</span>";
                    if (data["dateInspected"] == null && data["timeOut"] == null) {
                        status = "<label class='txt-info'>Active</label>";
                    }
                    else if (data["dateInspected"] == null && data["timeOut"] != null) {
                        status = "<span class='txt-info'>Unchecked</span>";
                    }
                    return status;
                }
            },
            {
                "data": function (data) {
                    var unchecked = "";
                    var btnEdit = "<a class='btn btn-success btn-xs' onclick=ShowPopup('/TradingInspector/AddEditTradersTruck?id=" + data["ticketingId"] + "')>Edit</a>";
                    var btnView = "<a class='btn btn-default btn-xs' onclick=ShowPopup('/TradingInspector/ViewTradersTruck?id=" + data["ticketingId"] + "')>View</a>";

                    if (data["dateInspected"] != null) {
                        return btnView;
                    }
                    else if (data["dateInspected"] == null && data["timeOut"] == null) {
                        return btnEdit;
                    }
                    else if (data["dateInspected"] == null && data["timeOut"] != null) {
                        return unchecked;
                    }
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
            url: apiurl,
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
            url: apiurl + '/' + id,
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




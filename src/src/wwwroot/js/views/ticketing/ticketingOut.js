var popup, dataTable;
var entity = 'Ticketing';
var apiurl = '/api/' + entity;

$(document).ready(function () {
    //alert(entity);
    var organizationId = $('#organizationId').val();
    dataTable = $('#grid').DataTable({
        "ajax": {
            "url": apiurl + '/' + organizationId,
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
                    if (data["timeIn"] == null) {
                        output = "";
                    }
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
            {
                "data": function (data) {
                    var d = new Date(data["timeOut"]);
                    var dateOut = monthNames[d.getMonth()] + " " + d.getDate() + ", " + d.getFullYear() + " - " + setClockTime(d);
                    var output = dateOut;
                    if (data["timeOut"] == null) {
                        output = "";
                    }
                    return output;
                }
            },
            //{
            //    "data": function (data) {
            //        var d = new Date(data["timeOut"]);
            //        var output = setClockTime(d);
            //        if (data["timeOut"] == null) {
            //            output = "";
            //        }
            //        return output;
            //    }
            //},
            { "data": "plateNumber" },
            { "data": "typeOfTransaction" },
            { "data": "typeOfCar" },
            { "data": "driverName" },
            { "data": "parkingNumber" },
            { "data": "remarks" },
            { "data": "amount" },
            //{ "data": "issuingClerk" },
            //{ "data": "receivingClerk" },
            //{ "data": "controlNumber" },
            {
                "data": function (data) {
                    var completed = "Completed";
                    var active = "Active";
                    var availedGatePass = "Gate pass";
                    if (data["timeIn"] != null && data["timeOut"] == null) {
                        return active;
                    }
                    else if (data["timeOut"] != null) {
                        return completed;
                    }
                    else {
                        return availedGatePass;
                    }
                }
            },
            {
                "data": function (data) {
                    //var btnEdit = "<a class='btn btn-default btn-xs' onclick=ShowPopup('/Ticketing/AddEditOut?id=" + data["ticketingId"] + "')><i class='fa fa-hourglass-end' title='Completed'></i></a>";
                    var availedGatePass = "Gate pass";
                    var empty = "";
                    var btnEdit = "<a class='btn btn-success btn-xs btnComplete' data-id='" + data["ticketingId"] + "'>Finish</a>";
                    var btnView = "<a class='btn btn-default btn-xs' style='margin-left:5px' onclick=ShowPopup('/Ticketing/ViewTicketingIn?id=" + data["ticketingId"] + "')><i class='fa fa-external-link' title='More'></i></a>";
                    if (data["timeIn"] != null && data["timeOut"] != null) {
                        return btnView;
                    }
                    else if (data["timeIn"] != null && data["timeOut"] == null) {
                        return btnEdit + btnView;
                    }
                    else if (data["timeIn"] == null && data["timeOut"] == null) {
                        return availedGatePass;
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
$("#grid").on("click", ".btnComplete", function (e) {
    e.preventDefault();
    var ticketId = $(this).attr("data-id");
    var param = { id: ticketId };
    swal({
        title: "Are you sure want to complete this transaction?",
        text: "You will not be able to restore the file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#dd4b39",
        confirmButtonText: "Yes, update it!",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            type: 'POST',
            url: apiurl + '/UpdateTicketOut',
            data: param,
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
});
const monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
];
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





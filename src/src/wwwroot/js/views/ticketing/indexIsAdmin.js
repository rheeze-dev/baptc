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
            //{ "data": "accreditation" },
            { "data": "remarks" },
            { "data": "count" },
            //{ "data": "issuingClerk" },
            //{ "data": "receivingClerk" },
            //{ "data": "controlNumber" },
            //{ "data": "gatePassDate" },
            //{ "data": "priceRange" },
            //{ "data": "time" },
            //{
            //    "data": function (data) {
            //        var valid = "<span class='txt-success'>Valid</span>";
            //        var expired = "<label class='txt-info'>Expired</label>";
            //        var completed = "Completed";
            //        var active = "Active";
            //        var endDate = data["endDate"];
            //        var currentDate = new Date();
            //        endDate = new Date(endDate);

            //        if (data["timeIn"] != null && data["timeOut"] == null) {
            //            return active;
            //        }
            //        else if (data["timeOut"] != null) {
            //            return completed;
            //        }

            //        else if (endDate >= currentDate) {
            //            return valid;
            //        } else {
            //            return expired;
            //        }
            //    }
            //},
            {
                "data": function (data) {
                    var empty = "";
                    var btnEdit = "<a class='btn btn-default btn-xs' onclick=ShowPopup('/Ticketing/AddEditIn?id=" + data["ticketingId"] + "')><i class='fa fa-pencil' title='Edit'></i></a>";
                    var btnCount = "<a class='btn btn-default btn-xs btnCount' data-id='" + data["ticketingId"] + "'><i class='fa fa-plus-circle' title='Count'></i></a>";
                    var btnView = "<a class='btn btn-default btn-xs' style='margin-left:5px' onclick=ShowPopup('/Ticketing/ViewTicketingIn?id=" + data["ticketingId"] + "')><i class='fa fa-external-link' title='More'></i></a>";
                    var btnDelete = "<a class='btn btn-danger btn-xs' style='margin-left:5px' onclick=Delete('" + data["ticketingId"] + "')><i class='fa fa-trash' title='Delete'></i></a>";
                    var outPut = btnEdit + btnView + " " + btnCount + btnDelete;

                    if (data["timeIn"] != null && data["timeOut"] != null) {
                        return btnView + btnDelete;
                    }
                    else if (data["timeIn"] != null && data["timeOut"] == null) {
                        if (data["typeOfTransaction"] == "Short trip") {
                            return outPut;
                        }
                        else {
                            return btnEdit + btnView + btnDelete;
                        }
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
$("#grid").on("click", ".btnCount", function (e) {
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
            url: apiurl + '/AddCount',
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

function SubmitAddEditNewGatePass(form) {
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
            url: "/api/Ticketing/PostNewGatePass",
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





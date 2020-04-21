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
            { "data": "plateNumber" },
            { "data": "typeOfTransaction" },
            { "data": "count" },
            {
                "data": function (data) {
                    //var empty = "";
                    var btnEdit = "<a class='btn btn-success btn-xs' onclick=ShowPopup('/Ticketing/AddEditIn?id=" + data["ticketingId"] + "')>Edit</a>";
                    var btnView = "<a class='btn btn-default btn-xs' onclick=ShowPopup('/Ticketing/ViewTicketingIn?id=" + data["ticketingId"] + "')>View</a>";
                    var btnCount = "<a class='btn btn-success btn-xs btnCount' data-id='" + data["ticketingId"] + "'>Count</a>";
                    //var btnDelete = "<a class='btn btn-danger btn-xs' style='margin-left:5px' onclick=Delete('" + data["ticketingId"] + "')><i class='fa fa-trash' title='Delete'></i></a>";
                    var outPut = btnEdit + " " + btnCount + " " + btnView;

                    if (data["timeIn"] != null && data["timeOut"] != null) {
                        return btnView;
                    }
                    else if (data["timeIn"] != null && data["timeOut"] == null) {
                        if (data["typeOfTransaction"] == "Short trip") {
                            return outPut;
                        }
                        else {
                            return btnEdit + " " + btnView;
                        }
                    }
                }
            }
            //{
            //    "data": function (data) {
            //        var unchecked = "";
            //        var btnEdit = "<a class='btn btn-success btn-xs' onclick=ShowPopup('/TradingInspector/AddEditFarmersTruck?id=" + data["ticketingId"] + "')>Edit</a>";
            //        var btnView = "<a class='btn btn-default btn-xs' onclick=ShowPopup('/TradingInspector/ViewFarmersTruckMobile?id=" + data["ticketingId"] + "')>View</a>";

            //        if (data["dateInspected"] != null) {
            //            return btnView;
            //        }
            //        else if (data["dateInspected"] == null && data["timeOut"] == null) {
            //            return btnEdit;
            //        }
            //        else if (data["dateInspected"] == null && data["timeOut"] != null) {
            //            return unchecked;
            //        }
            //    }
            //}
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
                    //popup.modal('hide');
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





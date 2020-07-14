var popup, dataTable;
var entity = 'Inspector';
var apiurl = '/api/' + entity;

$(document).ready(function () {
    //alert(entity);
    //var organizationId = $('#organizationId').val();
    dataTable = $('#grid').DataTable({
        "ajax": {
            "url": apiurl + '/GetShortTrip',
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
            //{
            //    "data": function (data) {
            //        var d = new Date(data["dateInspectedIn"]);
            //        var dateOut = monthNames[d.getMonth()] + " " + d.getDate() + ", " + d.getFullYear() + " - " + setClockTime(d);
            //        var output = dateOut;
            //        if (data["dateInspectedIn"] == null) {
            //            output = "";
            //        }
            //        return output;
            //    }
            //},
            //{
            //    "data": function (data) {
            //        var d = new Date(data["dateInspectedOut"]);
            //        var dateOut = monthNames[d.getMonth()] + " " + d.getDate() + ", " + d.getFullYear() + " - " + setClockTime(d);
            //        var output = dateOut;
            //        if (data["dateInspectedOut"] == null) {
            //            output = "";
            //        }
            //        return output;
            //    }
            //},
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
            { "data": "commodityIn" },
            { "data": "remarksIn" },
            { "data": "commodityOut" },
            { "data": "remarksOut" },
            {
                "data": function (data) {
                    var status = "<span class='txt-success'>Completed</span>";
                    if (data["dateInspectedOut"] == null && data["timeOut"] == null) {
                        status = "<label class='txt-info'>Active</label>";
                    }
                    else if (data["dateInspectedIn"] == null && data["dateInspectedOut"] == null && data["timeOut"] != null) {
                        status = "<label class='txt-info'>Unchecked</label>";
                    }
                    return status;
                }
            },
            {
                "data": function (data) {
                    var btnEdit = "<a class='btn btn-default btn-xs' onclick=ShowPopup('/TradingInspector/AddEditShortTrip?id=" + data["id"] + "')><i class='fa fa-pencil' title='Edit in'></i></a>";
                    var btnEditOut = "<a class='btn btn-default btn-xs' onclick=ShowPopup('/TradingInspector/AddEditShortTripOut?id=" + data["id"] + "')><i class='fa fa-pencil' title='Edit out'></i></a>";
                    var btnView = "<a class='btn btn-default btn-xs' style='margin-left:5px' onclick=ShowPopup('/TradingInspector/ViewShortTrip?id=" + data["id"] + "')><i class='fa fa-external-link' title='More'></i></a>";
                    //return btnEdit;
                    var outPut = btnEdit + " " + btnEditOut + btnView;
                    if (data["typeOfEntry"] == "Pick-up") {
                        return btnEditOut + btnView;
                    }
                    else {
                        return outPut;
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
            url: "/api/Inspector/PostShortTrip",
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

function SubmitAddEditOut(form) {
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
            url: "/api/Inspector/PostShortTripOut",
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
            url: apiurl + '/ShortTrip/' + id,
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





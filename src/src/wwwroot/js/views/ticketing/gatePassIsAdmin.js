var popup, dataTable;
var entity = 'Ticketing';
var apiurl = '/api/' + entity;

$(document).ready(function () {
    //alert(entity);
    var organizationId = $('#organizationId').val();
    dataTable = $('#grid').DataTable({
        "ajax": {
            "url": apiurl + '/GetGatePass',
            "type": 'GET',
            "datatype": 'json'
        },
        "order": [[0, 'desc']],
        "columns": [
            //{ "data": "commodityDate" },
            {
                "data": function (data) {
                    var d = new Date(data["startDate"]);
                    var output = monthNames[d.getMonth()] + " " + d.getDate() + ", " + d.getFullYear();
                    var spanData = "<span style = 'display:none;'> " + data["startDate"] + "</span>";
                    return spanData + output;
                }
            },
            ////{
            ////    "data": function (data) {
            ////        var d = new Date(data["newStartDate"]);
            ////        var output = monthNames[d.getMonth()] + " " + d.getDate() + ", " + d.getFullYear() + " - " + setClockTime(d);
            ////        var spanData = "<span style = 'display:none;'> " + data["newStartDate"] + "</span>";
            ////        return spanData + output;
            ////    }
            ////},
            //{
            //    "data": function (data) {
            //        var d = new Date(data["endDate"]);
            //        var dateOut = monthNames[d.getMonth()] + " " + d.getDate() + ", " + d.getFullYear();
            //        var output = dateOut;
            //        if (data["endDate"] == null) {
            //            output = "";
            //        }
            //        return output;
            //    }
            //},
            //{ "data": "driverName" },
            //{ "data": "lastName" },
            //{ "data": "birthDate" },
            //{ "data": "contactNumber" },
            { "data": "plateNumber1" },
            { "data": "plateNumber2" },
            //{ "data": "idType" },
            //{ "data": "idNumber" },
            { "data": "remarks" },
            //{
            //    "data": function (data) {
            //        //var valid = "<span class='txt-success'>Valid</span>";
            //        var expired = "<label class='txt-info'>Expired</label>";
            //        var endDate = data["endDate"];
            //        var currentDate = new Date();
            //        endDate = new Date(endDate);

            //        var remainingDays = endDate - currentDate;
            //        var days = Math.floor(remainingDays / (1000 * 60 * 60 * 24));
            //        //var hours = Math.floor((remainingDays % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
            //        //var minutes = Math.floor((remainingDays % (1000 * 60 * 60)) / (1000 * 60));
            //        //var seconds = Math.floor((remainingDays % (1000 * 60)) / 1000); 
            //        if (days < 0) {
            //            return expired;
            //        } 
            //        else {
            //            return days + " days remaining";
            //        }
            //    }
            //},
            {
                "data": function (data) {
                    var valid = "<span class='txt-success'>Valid</span>";
                    var expired = "<label class='txt-info'>Expired</label>";
                    var endDate = data["endDate"];
                    var currentDate = new Date();
                    endDate = new Date(endDate);

                    if (endDate >= currentDate) {
                        return valid;
                    } else {
                        return expired;
                    }
                }
            },
            //{
            //    "data": function (data) {
            //        //var btnEdit = "<a class='btn btn-default btn-xs' onclick=ShowPopup('/Ticketing/AddEditOut?id=" + data["ticketingId"] + "')><i class='fa fa-hourglass-end' title='Completed'></i></a>";
            //        var btnEdit = "<a class='btn btn-default btn-xs btnComplete' data-id='" + data["ticketingId"] + "'>Extend</a>";
            //        if (data["endDate"]) {
            //            btnEdit = "";
            //        }
            //        return btnEdit;
            //    }
            //}
            {
                "data": function (data) {
                    //var valid = "<span class='txt-success'>Valid</span>";
                    //var expired = "<label class='txt-info'>Expired</label>";
                    var btnExtend = "<a class='btn btn-default btn-xs btnComplete' data-id='" + data["ticketingId"] + "'>Extend</a>";
                    var btnEdit = "<a class='btn btn-default btn-xs' onclick=ShowPopup('/Ticketing/AddEditGatePass?id=" + data["ticketingId"] + "')><i class='fa fa-pencil' title='Edit'></i></a>";
                    var btnDelete = "<a class='btn btn-danger btn-xs' style='margin-left:5px' onclick=Delete('" + data["ticketingId"] + "')><i class='fa fa-trash' title='Delete'></i></a>";

                    var endDate = data["endDate"];
                    var currentDate = new Date();
                    endDate = new Date(endDate);

                    var remainingDays = endDate - currentDate;
                    var days = Math.floor(remainingDays / (1000 * 60 * 60 * 24));
                    //var hours = Math.floor((remainingDays % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                    //var minutes = Math.floor((remainingDays % (1000 * 60 * 60)) / (1000 * 60));
                    //var seconds = Math.floor((remainingDays % (1000 * 60)) / 1000); 
                    if (days > 0) {
                        return btnEdit + btnDelete;
                    } 
                    else {
                        return btnExtend + btnDelete;
                    }
                }
            }
            //{
            //    "data": function (data) {
            //        var btnEdit = "<a class='btn btn-default btn-xs' onclick=ShowPopup('/Ticketing/AddEditGatePass?id=" + data["id"] + "')><i class='fa fa-pencil' title='Edit'></i></a>";
            //        var btnDelete = "<a class='btn btn-danger btn-xs' style='margin-left:5px' onclick=Delete('" + data["id"] + "')><i class='fa fa-trash' title='Delete'></i></a>";
            //        return btnEdit + btnDelete;
            //    }
            //}
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
            url: apiurl + '/ExtendGatePass',
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


function SubmitListOfStallLease(form) {
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
            url: "/api/Ticketing/PostGatePass",
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
            url: apiurl + '/DeleteGatePass/' + id,
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





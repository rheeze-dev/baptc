﻿var popup, dataTable;
var entity = 'Accreditation';
var apiurl = '/api/' + entity;

$(document).ready(function () {
    //alert(entity);
    //var organizationId = $('#organizationId').val();
    dataTable = $('#grid').DataTable({
        "ajax": {
            "url": apiurl + '/GetBuyers',
            "type": 'GET',
            "datatype": 'json'
        },
        "order": [[0, 'desc']],
        "columns": [
            { "data": "businessName" },
            { "data": "businessAddress" },
            { "data": "presentAddress" },
            { "data": "vehiclePlateNumber" },
            { "data": "contactNumber" },
            {
                "data": function (data) {
                    var btnEdit = "<a class='btn btn-success btn-xs' onclick=ShowPopup('/Accreditation/AddEditBuyers?id=" + data["id"] + "')>Edit</a>";
                    var btnView = "<a class='btn btn-default btn-xs' onclick=ShowPopup('/Accreditation/ViewBuyers?id=" + data["id"] + "')>View</a>";
                    var btnDelete = "<a class='btn btn-danger btn-xs' style='margin-left:5px' onclick=Delete('" + data["id"] + "')>Delete</a>";
                    return btnEdit + " " + btnView + btnDelete;
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
            url: "/api/Accreditation/PostBuyers",
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
            url: apiurl + '/DeleteBuyers/' + id,
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




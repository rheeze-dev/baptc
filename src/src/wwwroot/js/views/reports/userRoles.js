﻿var popup, dataTable;
var entity = 'Roles';
var apiurl = '/api/' + entity;

$(document).ready(function () {
    //alert(entity);
    var organizationId = $('#organizationId').val();
    dataTable = $('#grid').DataTable({
        "ajax": {
            "url": apiurl + '/GetUsers' ,
            "type": 'GET',
            "datatype": 'json'
        },
        "columns": [
            { "data": "userId" },
            { "data": "fullName" },
            { "data": "email" },
            { "data": "emailConfirmed" },
            {
                "data": function (data) {
                    var d = new Date(data["dateModified"]);
                    var empty = "";
                    var output = monthNames[d.getMonth()] + " " + d.getDate() + ", " + d.getFullYear();
                    if (data["dateModified"] == null) {
                        return empty;
                    }
                    else {
                        return output;
                    }
                }
            },
            { "data": "modifier" },
            {
                "data": function (data) {
                    var btnConfig = "<a class='btn btn-default btn-xs' style='margin-left:5px' onclick=ShowPopup('/Settings/ConfigUserRoles?userId=" + data["userId"] + "')><i class='fa fa-cog' title='Config'></i></a>";
                    //var btnIncative = "<a class='btn btn-default btn-xs btnInactive' data-id='" + data["userId"] + "'>Deactivate</a>";
                    //var btnActive = "<a class='btn btn-default btn-xs btnActive' data-id='" + data["userId"] + "'>Activate</a>";
                    //var btnDelete = "<a class='btn btn-danger btn-xs' style='margin-left:5px' onclick=Delete('" + data["id"] + "')><i class='fa fa-trash' title='Delete'></i></a>";
                    return btnConfig;
                }
            },
            {
                "data": function (data) {
                    //var btnConfig = "<a class='btn btn-default btn-xs' style='margin-left:5px' onclick=ShowPopup('/Settings/ConfigUserRoles?userId=" + data["userId"] + "')><i class='fa fa-cog' title='Config'></i></a>";
                    var btnActive = "Deactivated";
                    var btnInActive = "<a class='btn btn-default btn-xs btnInActive' data-id='" + data["userId"] + "'>Deactivate</a>";
                    //var btnDelete = "<a class='btn btn-danger btn-xs' style='margin-left:5px' onclick=Delete('" + data["id"] + "')><i class='fa fa-trash' title='Delete'></i></a>";
                    if (data["roleId"] != "Default") {
                        return btnInActive;
                    }
                    else if (data["roleId"] == "Default")
                    {
                        return btnActive;
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
$("#grid").on("click", ".btnInActive", function (e) {
    e.preventDefault();
    var userId = $(this).attr("data-id");
    var param = { id: userId };
    swal({
        title: "Are you sure want to deactivate this account?",
        //text: "You will not be able to restore the file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#dd4b39",
        confirmButtonText: "Yes, deactivate it!",
        closeOnConfirm: true
    }, function () {
        $.ajax({
            type: 'POST',
            url: apiurl + '/DeactivateUser',
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
    var s = d.getSeconds();
    var suffix = "AM";
    if (h > 11) { suffix = "PM"; }
    if (h > 12) { h = h - 12; }
    if (h == 0) { h = 12; }
    if (h < 10) { h = "0" + h; }
    if (m < 10) { m = "0" + m; }
    if (s < 10) { s = "0" + s; }
    return h + ":" + m + ":" + s + " " + suffix;
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
            url: apiurl + '/UserRoles/' + id,
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





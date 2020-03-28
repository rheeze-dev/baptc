var popup, dataTable;
var entity = 'Security';
var apiurl = '/api/' + entity;

$(document).ready(function () {
    //alert(entity);
    //var organizationId = $('#organizationId').val();
    dataTable = $('#grid').DataTable({
        "ajax": {
            "url": apiurl + '/GetSecurityInspectionReport',
            "type": 'GET',
            "datatype": 'json'
        },
        "order": [[0, 'desc']],
        "columns": [
            //{ "data": "commodityDate" },
            {
                "data": function (data) {
                    var d = new Date(data["date"]);
                    var output = monthNames[d.getMonth()] + " " + d.getDate() + ", " + d.getFullYear();
                    return output;
                }
            },
            //{
            //    "data": function (data) {
            //        var d = new Date(data["timeIn"]);
            //        var output = monthNames[d.getMonth()] + " " + d.getDate() + ", " + d.getFullYear() + " - " + setClockTime(d);
            //        var spanData = "<span style = 'display:none;'> " + data["timeIn"] + "</span>";
            //        if (data["timeIn"] == null) {
            //            output = "";
            //        }
            //        return spanData + output;
            //    }
            //},
            {
                "data": function (data) {
                    var d = new Date(data["date"]);
                    var output = setClockTime(d);
                    return output;
                }
            },
            { "data": "location" },
            { "data": "remarks" },
            { "data": "action" },
            { "data": "inspector" },
            {
                "data": function (data) {
                    var btnEdit = "<a class='btn btn-default btn-xs' onclick=ShowPopup('/Security/AddEditSecurityInspectionReport?id=" + data["id"] + "')><i class='fa fa-pencil' title='Edit'></i></a>";
                    var btnDelete = "<a class='btn btn-danger btn-xs' style='margin-left:5px' onclick=Delete('" + data["id"] + "')><i class='fa fa-trash' title='Delete'></i></a>";
                    return btnEdit + btnDelete;
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
            url: "/api/Security/PostSecurityInspectionReport",
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
            url: apiurl + '/SecurityInspectionReport/' + id,
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





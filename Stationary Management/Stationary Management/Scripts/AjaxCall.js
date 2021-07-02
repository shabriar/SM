
//Basic function for ajax call get data ========================================================================================================================

function ajaxCall(url, paramData, callback, method, obj) {
    // console.log(url, paramData, callback, method, obj);
    method = method == null ? "POST" : method;
    $.ajax({
        type: method,
        url: url,
        data: JSON.stringify(paramData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            //callback(data);
            //console.log(url, paramData, callback, method, obj);
            if (callback == 'renderProformaInvoiceLoad') {
                renderProformaInvoiceLoad(response, obj);
            }
            else if (callback == 'renderRemoveItem') {
                renderRemoveItem(response);
            }
            //else if (callback == 'renderDepartmentEntryLoad') {
            //    renderDepartmentEntryLoad(response);
            //}
            //else if (callback == 'renderDesignationEntryLoad') {
            //    renderDesignationEntryLoad(response);
            //}
            //else if (callback == 'renderCurrencyEntryLoad') {
            //    renderCurrencyEntryLoad(response);
            //}
            //else if (callback == 'renderCountryOfOriginEntryLoad') {
            //    renderCountryOfOriginEntryLoad(response);
            //}
            //else if (callback == 'renderCategoryEntryLoad') {
            //    renderCategoryEntryLoad(response);
            //}
            //else if (callback == 'renderMeasuremnentUnitEntryLoad') {
            //    renderMeasuremnentUnitEntryLoad(response);
            //}
            //else if (callback == 'renderTermsAndConditionsEntryLoad') {
            //    renderTermsAndConditionsEntryLoad(response);
            //}
            //else if (callback == 'renderWarehouseEntryLoad') {
            //    renderWarehouseEntryLoad(response);
            //}
            //else if (callback == 'renderProductEntryLoad') {
            //    renderProductEntryLoad(response, obj);
            //}
            //else if (callback == 'renderProductReturnLoad') {
            //    renderProductReturnLoad(response, obj);
            //}           
            //else if (callback == 'renderProformaInvoiceProductLoad') {
            //    renderProformaInvoiceProductLoad(response, obj);
            //}
            //else if (callback == 'renderAgentEntryLoad') {
            //    renderAgentEntryLoad(response, obj);
            //}
            //else if (callback == "renderStockQuantityLoad") {
            //    renderStockQuantityLoad(response, obj);
            //}
            //else if (callback == "renderResetPassword") {
            //    renderResetPassword(response);
            //}
            //else if (callback == "RenderLoadPayementTerms") {
            //    RenderLoadPayementTerms(response, obj);
            //}
            //else if (callback == "renderVisitingEventEntryLoad") {
            //    renderVisitingEventEntryLoad(response);
            //}
            //else if (callback == "renderDefaultProductLoad") {
            //    renderDefaultProductLoad(response);
            //}
            //else if (callback == 'rendeBuyerRelativeDataLoad') {
            //    rendeBuyerRelativeDataLoad(response, obj);
            //}
            //else if (callback == 'renderShowTotalPendingValue') {
            //    renderShowTotalPendingValue(response)
            //}
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(errorThrown);
            console.log(XMLHttpRequest);
            console.log(textStatus);
            alert("An error occurred whilst trying to contact the server: " + XMLHttpRequest.status + " " + textStatus + " " + errorThrown);
            //alert(textStatus + "! please try again", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
            //alert(textStatus + "! please try again", '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
        }
    });
}
//render callback functions for get data-------------------------------



function renderRemoveItem(data) {
    location.reload();
}
function renderEmployeeInfoLoad(data) {
    $("#EmpName").val(data.employee.FullName);
    $("#EmpEmail").val(data.employee.Email);

}
function getEmployeesByDept(data) {
    $('#EmployeeId').append(data.selectListString);

}
function RenderLoadPayementTerms(data, obj) {
    $('#PaymentTermsId').empty();
    $('#PaymentTermsId').append(data);
    if (obj > 0) {
        $("#PaymentTermsId option[value=" + obj + "]").prop("selected", true);
    }
    else {
        $('#PaymentTermsId').append("<option> --Select-- </option>");
    }
}

function renderResetPassword(data) {
    alert(data.msg);
}


//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : August 2018
//=======================================================================================//
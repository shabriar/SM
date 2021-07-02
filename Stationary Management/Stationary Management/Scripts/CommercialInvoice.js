// check all checkbox //
function checkAllCheckBox() {
    var checkAllValue = $("#checkAll").prop("checked");
    if (checkAllValue == true) {
        $("#piProductItemTBody").find("input[name$='.CheckValue']").each(function () {
            this.checked = checkAllValue;
            $(this).closest("tr").find("[name$='.Quantity']").removeAttr("disabled");
            $(this).closest("tr").find("[name$='.Remarks']").removeAttr("disabled");
            invoiceValueByQuantity();
        });
    }
    else {
        $("#piProductItemTBody").find("input[name$='.CheckValue']").each(function () {
            this.checked = checkAllValue;
            $(this).closest("tr").find("[name$='.Quantity']").attr("disabled", "disabled");
            $(this).closest("tr").find("[name$='.Remarks']").attr("disabled", "disabled");
            invoiceValueByQuantity();
        });
    }
}

// invoice value change by onchange & onkeyup check quantiy //
function invoiceValueByQuantity() {
    var totalInvoiceValue = 0;
    $("#piProductItemTBody").find("[name$='.CheckValue']").each(function () {
        if ($(this).prop("checked") == true) {
            var productValue = customTryParseFloat($(this).closest("tr").find("[name$='.UnitPrice']").text(), 0);
            var ciQuantity = customTryParseFloat($(this).closest("tr").find("[name$='.Quantity']").val(), 0);
            var productQuantity = customTryParseFloat($(this).closest("tr").find("[name$='.CIQuantity']").text(), 0);
            var pfiDiscount = customTryParseFloat($(this).closest("tr").find("[name$='.DiscountValue']").text(), 0);

            if (ciQuantity > productQuantity) {
                $.alert('This quantity greater than product quantity!', '<i class="fa fa-exclamation-circle" aria-hidden="true"> Alert</i>');
                var saveCIQuantity = customTryParseFloat($(this).closest("tr").find("[name$='.CIQuantity']").text(), 0);
                ciQuantity = saveCIQuantity != "" ? saveCIQuantity : productQuantity;
                $(this).closest("tr").find("[name$='.Quantity']").val(ciQuantity);
            }

            var grandProductValue = productValue * ciQuantity;
            var grandPfiValue = DiscountCalculator(pfiDiscount, "Percentage", grandProductValue).discountValue;
            var finalPfiValue = grandProductValue - grandPfiValue;
            totalInvoiceValue += finalPfiValue;

            $(this).closest("tr").find("[name$='.Quantity']").removeAttr("disabled");
            $(this).closest("tr").find("[name$='.Remarks']").removeAttr("disabled");
        }
        else {
            $(this).closest("tr").find("[name$='.Quantity']").attr("disabled", "disabled");
            $(this).closest("tr").find("[name$='.Remarks']").attr("disabled", "disabled");
        }
    });
    $("#InvoiceValue").val(totalInvoiceValue.toFixed(2));
}
// change quantiy when ci product quantity field empty //
function changeQuantity(obj) {
    $("#piProductItemTBody").find("[name$='.ProductQuantity']").each(function () {
        var productQuantity = customTryParseFloat($(obj).closest("tr").find("[name$='.ProductQuantity']").text(), 0);
        var ciQuantity = customTryParseFloat($(obj).closest("tr").find("[name$='.CIQuantity']").text(), 0);

        if ($(obj).val() == "" && ciQuantity != "") {
            $(obj).val(ciQuantity);
        }
        else if ($(obj).val() == "") {
            $(obj).val(productQuantity);
        }
    });
    invoiceValueByQuantity();
}

// form submit //
function formSubmitFunc() {
    var count = 0;
    $('#piProductItemTBody').find('tr').each(function () {
        if ($(this).find(':input').length > 0) {
            var suffix = $(this).find(':input:first').attr('name').match(/\d+/);
            $.each($(this).find(':input'), function (i, val) {
                // Replaced Name
                var oldN = $(this).attr('name');
                var newN = oldN.replace('[' + suffix + ']', '[' + count + ']');
                $(this).attr('name', newN);
            });
            count++;
        }
    });
    //$("#commercialInvoiceForm").submit();
    SubmitAndDisplayProgressMessage('#commercialInvoiceForm', '.btnSave', 'Saving..');
}
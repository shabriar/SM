// calculate rows total and return totla value in a target id
$(function () {
    assignSelect2();
   
});
$(function () {
    $('ul li a').click(function () {
        $('li a').removeClass("active");
        $(this).addClass("active");
    });
});
function assignSelect2() {
    $('.select2').select2({
    });
    $(".select2").on("select2:select", function (evt) {
        var element = evt.params.data.element;
        var $element = $(element);
        $element.detach();
        $(this).append($element);
        $(this).trigger("change");
    });
   
    
}

// Bootstrap DateTime Picker---------------------
$('.myDatepicker').datepicker({
    format: 'dd-M-yyyy',
    autoclose: true,
    todayHighlight: true,
});

$(document).on("click", ".myDatepickerIcon", function () {

    $(this).parent().find('.myDatepicker').focus();
});
function customTryParseInt(value, defaultValue) {
    defaultValue = (typeof defaultValue !== "undefined" && defaultValue !== null) ? defaultValue : 0;
    var returnValue = defaultValue;
    if (typeof value !== "undefined" && value !== null) {
        if (value.toString().length > 0) {
            value = currencyToNumber(value);
            if (!isNaN(value)) {
                returnValue = parseInt(value);
            }
        }
    }
    return returnValue;
}

function customTryParseFloat(value, defaultValue) {
    defaultValue = (typeof defaultValue !== "undefined" && defaultValue !== null) ? defaultValue : 0;
    var returnValue = defaultValue;
    if (typeof value !== "undefined" && value !== null) {
        if (value.toString().length > 0) {
            value = currencyToNumber(value);
            if (!isNaN(value)) {
                returnValue = parseFloat(value);
            }
        }
    }
    return returnValue;
}
function numberToCurrency(value) {
    var returnValue = Number(value).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
    return returnValue;
}

function currencyToNumber(value) {
    var returnValue = Number(value.toString().replace(/[^0-9.-]+/g, '')).toFixed(2);
    return returnValue;
}
function CalculateRowsTotal(rowClass, rowTextOrVal, targetId, targetTextOrVal, callback) {
    var sum = 0;
    if (rowTextOrVal == 'val') {
        $(rowClass).each(function () { sum += isNonNegativeNumber($(this).val().trim()) == false ? 0 : parseFloat($(this).val().trim()); });
    } else {
        $(rowClass).each(function () { sum += isNonNegativeNumber($(this).text().trim()) == false ? 0 : parseFloat($(this).text().trim()); });
    }
    if (targetTextOrVal == 'val') {
        $(targetId).val(parseFloat(sum).toFixed(2));
    } else {
        $(targetId).text(formatNumber(parseFloat(sum).toFixed(2)));
    }
    //callback functions

    if (callback == 'calculateSubTotalExpense') {
        calculateSubTotalExpense();
    }
    else if (callback == 'grandTotalCalculation') {
        grandTotalCalculation();
    }
}
function selectAllText(ctrl) {
    $(ctrl).select();
};
//calculating discount====================================================
function DiscountCalculator(discountAmount, discountType, amount) {
    var discountAmountWhenPercent = parseFloat(amount * (discountAmount / 100)).toFixed(2);
    var discountValue = discountType == "Flat" ? discountAmount : discountAmountWhenPercent;
    var discountValueShow = discountType == "Flat" ? discountAmount + " " : discountAmountWhenPercent + " (" + discountAmount + "%)";
    return {
        discountValue: parseFloat(discountValue).toFixed(2),
        discountValueShow: discountValueShow
    };
}

function discoutAmountValidation(discountType, discount, price) {
    if (discountType == "Percent") {
        if (parseFloat(discount) > 100) {
            return false;
        } else {
            return true;
        }
    }
    else if (discountType == "Flat") {
        if (parseFloat(discount) > price) {
            return false;
        } else {
            return true;
        }
    }

}
function formatNumber(num) {
    return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
}
//required asteric sign for input field=============================================================================
$('input[type=text]').each(function () {
    var req = $(this).attr('data-val-required');
    if (undefined != req) {
        var label = $('label[for="' + $(this).attr('id') + '"]');
        var text = label.text();
        if (text.length > 0) {
            label.append('<span style="color:red"> *</span>');
        }
    }
});


//comma Separate Number=============================================================================
function commaSeparateNumber(val) {
    while (/(\d+)(\d{3})/.test(val.toString())) {
        val = val.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
    }
    return val;
}
function isNonNegativeNumber(number) {
    if (number < 0 || isNaN(number) || number == '') {
        return false;
    }
    return true;
}
//display and hide progress popup message in submit======================================================
function SubmitAndDisplayProgressMessage(form, ctl, msg) {

    if ($(form).valid()) {
        $(form).submit();
        DisplayProgressMessage(ctl, msg);

    }
    else {
        return false;
        // not so valid code here
    }

    //$(form).submit();
    //DisplayProgressMessage(ctl, msg);
}
function DisplayProgressMessage(ctl, msg) {
    $(ctl).prop("disabled", true).html('<i class="fa fa-spinner fa-lg fa-spin"></i> ' + msg);
    $("body").addClass("submit-progress-bg");
    setTimeout(function () {
        $(".submit-progress").removeClass("hidden");
    }, 1);
    return true;
}

function HideProgressMessage(ctl, msg) {
    $(ctl).prop("disabled", false).html(msg);
    $(".submit-progress").addClass("hidden");
    $("body").removeClass("submit-progress-bg");

    return true;
}
function rearrangeNameSuffix(selector) {
    var count = 0;
    $(selector).find('tr').each(function () {

        var suffix = $(this).find(':input:first').attr('name').match(/\d+/);
        $.each($(this).find(':input'), function (i, val) {
            // Replaced Name
            var oldN = $(this).attr('name');
            var newN = oldN.replace('[' + suffix + ']', '[' + count + ']');
            $(this).attr('name', newN);

        });

        count++;
    });
}
//==================== function for converting form serializedArray data to object==================
function objectifyForm(formArray) {//serialize data function

    var returnArray = {};
    for (var i = 0; i < formArray.length; i++) {
        returnArray[formArray[i]['name']] = formArray[i]['value'];
    }
    return returnArray;
}
function RemoveRowData(obj) {
    $(obj).closest("tr").remove();
}
function seachForCurrentMonth(obj, dateFrom, dateTo, formObj) {
    formObj = formObj == null ? ".reportSearchForm" : formObj;
    var date = new Date();
    var firstDayOfCurrMonth = new Date(date.getFullYear(), date.getMonth(), 1);
    var lastDayOfCurrMonth = new Date(date.getFullYear(), date.getMonth() + 1, 0);
    $(obj).closest(formObj).find(dateFrom).val("1-" + monthNames[date.getMonth()] + "-" + date.getFullYear());
    $(obj).closest(formObj).find(dateTo).val(date.getDate() + "-" + monthNames[date.getMonth()] + "-" + date.getFullYear());
    $(obj).closest(formObj).submit();
}
function seachForPrevMonth(obj, dateFrom, dateTo, formObj) {
    formObj = formObj == null ? ".reportSearchForm" : formObj;
    var date = new Date();
    var firstDayOfCurrMonth = new Date(date.getFullYear(), date.getMonth(), 1);
   
    var lastDayofPrevMonth = new Date(firstDayOfCurrMonth - 1);
    var firstDayOfPrevMonth = new Date(lastDayofPrevMonth.getFullYear(), lastDayofPrevMonth.getMonth(), 1);
     $(obj).closest(formObj).find(dateFrom).val("1-" + monthNames[firstDayOfPrevMonth.getMonth()] + "-" + firstDayOfPrevMonth.getFullYear());
    $(obj).closest(formObj).find(dateTo).val(lastDayofPrevMonth.getDate() + "-" + monthNames[lastDayofPrevMonth.getMonth()] + "-" + lastDayofPrevMonth.getFullYear());
    $(obj).closest(formObj).submit();
}
// function for js date formate from json date string--------------------------------------
const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
    "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
];
function formatDateString(date, symbol) {
    var monthShortNames = new Array();
    monthShortNames[0] = "Jan";
    monthShortNames[1] = "Feb";
    monthShortNames[2] = "Mar";
    monthShortNames[3] = "Apr";
    monthShortNames[4] = "May";
    monthShortNames[5] = "Jun";
    monthShortNames[6] = "Jul";
    monthShortNames[7] = "Aug";
    monthShortNames[8] = "Sep";
    monthShortNames[9] = "Oct";
    monthShortNames[10] = "Nov";
    monthShortNames[11] = "Dec";
    var monthNames = new Array();
    monthNames[0] = "January";
    monthNames[1] = "February";
    monthNames[2] = "March";
    monthNames[3] = "April";
    monthNames[4] = "May";
    monthNames[5] = "June";
    monthNames[6] = "July";
    monthNames[7] = "August";
    monthNames[8] = "September";
    monthNames[9] = "October";
    monthNames[10] = "November";
    monthNames[11] = "December";

    var dateString = date.substr(6);
    var currentTime = new Date(parseInt(dateString));

    var month = '' + (currentTime.getMonth() + 1),
        //var month = monthShortNames[currentTime.getMonth()],
        day = '' + currentTime.getDate(),
        year = currentTime.getFullYear();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    return [day, month, year].join(symbol);
}

// Jquery confirm and alert setting =========================================================================
jconfirm.defaults = {
    title: ' ',
    content: 'Are you sure to continue?',
    icon: '',
    confirmButton: 'Ok',
    cancelButton: 'Cancel',
    confirmButtonClass: 'btn-default',
    cancelButtonClass: 'btn-primary',
    theme: 'white',
    animation: 'none',
    animationSpeed: 400,
    animationBounce: 1.5,
    keyboardEnabled: true,
    container: 'body',
    confirm: function () {
    },
    cancel: function () {
    },
    backgroundDismiss: true,
    autoClose: false,
    closeIcon: true,
    closeIconClass: 'fa fa-close'
};
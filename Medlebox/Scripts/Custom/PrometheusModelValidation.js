function SetValidation(action) {

    func = function (e) {
        
        field = $(this).attr('id');
        fieldname = $(this).attr('name');
        field = field.replace(/\[/g, "_")
        .replace(/\]/g, "_")
        .replace(/\./g, "_");


        FormId = $(this).closest("Form").attr("id");
        refreshID = $(this).attr("data-val-refresh-id");


        if (refreshID == undefined || refreshID=="") refreshID = ""; else refreshID = "#" + refreshID;


       if (FormId == undefined) FormId = ""; else FormId = "#" + FormId;
        e.preventDefault();
        $.ajax({
            type: $('Form' + FormId).attr('Method'),
            url: action,
            data: $('Form' + FormId).serialize(),
            success: function (data) {
                if ($(data).find('#needlogin').html() != null) {
                    return;
                }
                if ($(data).find('#error').html() != null) {
                    return;
                }
                if (refreshID == "") {
                    focused = $(':focus');
                    src = $(data).find('span[data-valmsg-for="' + fieldname + '"]').closest('div');
                    dst = $('span[data-valmsg-for="' + fieldname + '"]').closest('div');
                    dst.html(src.html());
                    setTimeout(function () { $('#' + focused.attr("id")).focus(); }, 10);
                } else {

                    focused = $(':focus');

                    src = $(data).find('span[data-valmsg-for="' + fieldname + '"]').closest('div');
                    dst = $('span[data-valmsg-for="' + fieldname + '"]').closest('div');
                    dst.html(src.html());

                    src = $(data).find('div' + refreshID);

                    dst = $('div' + refreshID);
                    dst.html(src.html());
                    $(focused).focus();
                }
                SetValidation(action);
                SetParserForField(field);
            }

        });
    };
    $('input[data-val="true"]:disabled').unbind("change");
    $('input[data-val="true"]:hidden').unbind("change");
    $('input[data-val="true"]').unbind("focusout");
    $('select[data-val="true"]').unbind("change");
    $('input[data-val="true"]').focusout(func);
    $('select[data-val="true"]').change(func);
    $('input[data-val="true"]:disabled').change(func); 
    $('input[data-val="true"]:hidden').change(func);
    $('input[data-val="true"]:hidden').attr("data-refresh-action", action);
}
function SetParser(Name, decimalCount, fracCount, allowMinus) {
    var numParser1 = new NumberParser(fracCount, ",", " ", true);
    var numMask1 = new NumberMask(numParser1, Name, decimalCount);
    numMask1.allowNegative = allowMinus;
    numMask1.leftToRight = true;
}
function SetParsers() {
    $('input[data-parser-enabled="True"]').each(function (e) {
        SetParserForField($(this).attr("id"));
    });
}

function SetParserForField(field) {
    if ($('#' + field).attr('data-parser-enabled')) {

        dec = $('#' + field).attr('data-parser-decimal');
        if (dec == undefined) dec = -1;
        float = $('#' + field).attr('data-parser-float');
        if (float == undefined) float = 0;
        minus = $('#' + field).attr('data-parser-minus');
        if (minus == undefined) minus = false;

        SetParser(field, dec, float, minus);

    }
}

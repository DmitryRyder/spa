function InitContextMenu() {
    var delayTimer;

    $("body").on("click", function (event) {
        $(".user-menu").slideUp();

        if ($(event.target).hasClass("ui-icon-circle-triangle-w") ||
            $(event.target).hasClass("ui-icon-circle-triangle-e") ||
            $(event.target).closest("#ui-datepicker-div").length > 0) {
        }
        else {
            $(".dropdown-first-level > dd > ul, .dropdown-first-level > dd > div > ul").hide();
        }

        $(".dropdown-second-level > dd > ul, .dropdown-second-level > dd > div > ul").hide();
    });

    $("body").on("click", ".contextMenu", function (event) {
        event.preventDefault();

        $(".menuContainer").hide();

        var div = $(this).next();
        div.css({
            position: "absolute",
            top: event.pageY,
            left: event.pageX,
            //         display: "block"
        });

        div.show();
        div.mouseenter(function (event) {
            if (delayTimer)
                clearTimeout(delayTimer);

        }).mouseleave(function () {
            delayTimer = setTimeout(function () {
                div.hide();
            },
                1000);
        });
        /*
        $(".contextMenu").mouseout(function () {
            delayTimer = setTimeout(function () { div.hide(); }, 1500);
        });
        */
        return false;
    });

    $(".menuContainer").menu();





    /*
    $.widget("ui.timespinner", $.ui.spinner, {
        options: {
            // seconds
            step: 60 * 1000,
            // hours
            page: 60
        },

        _parse: function (value) {
            if (typeof value === "string") {
                // already a timestamp
                if (Number(value) == value) {
                    return Number(value);
                }
                return +Globalize.parseDate(value);
            }
            return value;
        },

        _format: function (value) {
            return Globalize.format(new Date(value), "t");
        }
    });
    */

    $(".gridViewClearFilters").click(function (event) {
        var grid = ($(this)[0].getAttribute("grid"));
        if (grid != null && grid.length > 0) {
            window[grid].ClearFilter();
        } else {
            window["GridView"].ClearFilter();
        }
    });

}

function ShowSubmitWaitDialog() {
    $("#submit-wait-dialog").attr("hidden", false);
    $("#submit-wait-dialog").dialog({
        width: 600,
        modal: true,
        closeText: "",
        closeOnEscape: false
    });
}

function CloseSubmitWaitDialog() {
    $("#submit-wait-dialog").dialog("close");
}

function DisableSubmitButton(form) {
    $(form).find(":submit").each(function () {
        $(this).attr("disabled", true);
    });
}

function EnableSubmitButton(form) {
    $(form).find(":submit").each(function () {
        $(this).removeAttr("disabled");
    });
}

function checkPassword(pwd) {
    var $strength = $("#passwordStrength");
    var $strengthLevels = [
        "very weak",
        "weak",
        "good",
        "strong",
        "very strong"
    ];

    if (pwd !== "") {
        var ret = zxcvbn(pwd);

        $strength.prop("class", "passwordStrengthLevel" + (ret.score + 1));
        $strength.prop("title", "Password is " + $strengthLevels[ret.score]);
    } else {
        $strength.prop("class", "passwordStrengthLevel1");
        $strength.prop("title", "Password is undefined");
    }
}

function bindPasswordEventHandlers() {
    var $input = $(".passwordstrength");
    $input.change(function () {
        checkPassword($input.val());
    });
    $input.keyup(function () {
        $input.change();
    });
}

jQuery.expr[':'].contains = function (a, i, m) {
    return jQuery(a).text().toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
};

$(function () {
    if (navigator.userAgent.match(/iPad/i) == null) {
        $(document).tooltip({
            items: "img.tooltip",
            content: function () {
                var element = $(this);
                if (element.is("[title]")) {
                    return element.attr("title");
                }
                if (element.is("img")) {
                    return element.attr("alt");
                }
            }
        });
    }
    else {
        if ($("img.tooltip").attr("title")) {
            $("img.tooltip").attr("title", $("img.tooltip").attr("title").replace(/<b>/g, "").replace(/<\/b>/g, "").replace(/<br\/>/g, " "));
        }
    }

    $("#checkAll").change(function () {
        $("input:checkbox", $(this).parent().prev(".checkList")).not(':disabled').prop('checked', $(this).prop("checked")).not("[disabled]");
    });

    $(".showpassword").each(function (index, input) {
        var $input = $(input);
        $('<div></div>').append(
            $("<input type='checkbox' class='showpasswordcheckbox' />").click(function () {
                var change = $(this).is(":checked") ? "text" : "password";
                var rep = $("<input type='" + change + "' />")
                    .attr("id", $input.attr("id"))
                    .attr("name", $input.attr("name"))
                    .attr('class', $input.attr('class'))
                    .attr('data-val', true)
                    .attr('data-val-required', 'Password is required')
                    .attr('minlength', 14)
                    .val($input.val())
                    .insertBefore($input);
                $input.remove();
                $input = rep;
                bindPasswordEventHandlers();
            })
        ).append($("<span/>").text("Show password")).insertAfter($input.next().next());
    });

    InitContextMenu();

    $("body").on("submit", ".submit-wait-disable-button", function () {
        DisableSubmitButton(this);
        return true;
    });

    $("body").on("submit", ".submit-wait-dialog", function () {
        ShowSubmitWaitDialog();
        return true;
    });

    $(".listDelete").bind("click", function (event) {
        var href = $(this).attr("href");

        var $modal = $("<div'><div style='padding:20px 20px 15px 20px'>Are you sure to delete this item?</div></div>").appendTo("body");
        $modal.dialog({
            title: "Confirm delete action",
            width: 600,
            modal: true,
            closeText: "",
            buttons: {
                Ok: function () {
                    window.location.href = href;

                    $(this).dialog("destroy").remove();
                },
                Close: function () {
                    $(this).dialog("destroy").remove();
                }
            }
        });

        return false;
    });

    $(".listRemove").bind("click", function (event) {

        var href = $(this).attr("href");

        var $modal = $("<div><div style='padding:20px 20px 15px 20px'>Are you sure to want to remove this item?</div></div>").appendTo("body");
        $modal.dialog({
            title: "Confirm remove action",
            width: 600,
            modal: true,
            buttons: {
                Ok: function () {
                    window.location.href = href;

                    $(this).dialog("destroy").remove();
                },
                Close: function () {
                    $(this).dialog("destroy").remove();
                }
            }
        });

        return false;
    });


    $(document).ready(bindPasswordEventHandlers());

    var currentDropdown = null;
    var previousDropdown = null;

    $(".dropdown > dt > a").on('click', function () {
        previousDropdown = currentDropdown;
        var newDropDown = $(this).closest(".dropdown").find("dd ul");
        if (currentDropdown != null && currentDropdown.closest("div").attr("id") !== newDropDown.closest("div").attr("id")) {
            currentDropdown.hide();
        }

        $(this).closest(".dropdown").find("dd ul").slideToggle('fast');
        currentDropdown = newDropDown;

        return false;
    });

    $(".dropdown > dd > ul > li > a").on('click', function () {
        $(this).closest(".dropdown").find("dd ul").hide();
    });

    $(".dropdown-first-level > dt > a").on('click', function () {
        var ul = $(this).closest(".dropdown-first-level").find("> dd > ul, > dd > div > ul");

        $(".dropdown-first-level > dd > ul, .dropdown-first-level > dd > div > ul").each(function () {
            if (ul[0] != $(this)[0]) {
                $(this).hide();
            }
        });

        ul.slideToggle('fast');

        return false;
    });

    $(".dropdown-first-level > dd > ul > li > a, .dropdown-first-level > dd > div > ul > li > a").on('click', function () {
        $(this).closest("ul").hide();
    });

    $(".dropdown-second-level > dt > a").on('click', function () {
        var ul = $(this).closest(".dropdown-second-level").find("> dd > ul");

        $(".dropdown-second-level > dd > ul").each(function () {
            if (ul[0] != $(this)[0]) {
                $(this).hide();
            }
        });

        ul.slideToggle('fast');

        return false;
    });

    $(".dropdown-second-level > dd > ul > li > a").on('click', function () {
        $(this).closest("ul").hide();
    });

    function getSelectedValue(id) {
        return $("#" + id).find("dt a span.value").html();
    }

    $(document).bind('click', function (e) {
        var $clicked = $(e.target);
        if (!$clicked.parents().hasClass("dropdown") && currentDropdown != null) { $clicked.find("dl").find("dd ul").hide(); currentDropdown.hide(); }
    });

    OnFilterChange();
});

function OnFilterChange() {
    $('.mutliSelect input[type="checkbox"]').on('click', function () {

        var title = $(this).closest('.mutliSelect').find('input[type="checkbox"]').parent().text(),
            title = $(this).parent().text() + ";";
        var value = $(this).val();

        if ($(this).is(':checked')) {
            var html = '<span title="' + value + '">' + title + '</span>';
            var temp = $(this).closest("dl").find('.multiSel');
            $(this).closest("dl").find('.multiSel').append(html);
            $(this).closest("dl").find(".hida").hide();
        } else {
            $(this).closest("dl").find('span[title="' + value + '"]').remove();
            var ret = $(this).closest("dl").find(".hida");
            $(this).closest('dt').prev('dd').find("dl").find('dt a').append(ret);
        }
    });

    $(".mutliSelect input:not(.multiSelAll, .mutliSelectSearch)").change(function () {
        $(this).closest("dl").find("a").text($.map($('input:checked:not(.multiSelAll, .mutliSelectSearch)', $(this).closest("ul")),
            function (n, i) {
                return $(n).parent().text();
            }).join("; "));
    });

    $(".multiSelAll").change(function () {
        $(this).closest("ul").find("input").prop("checked", $(this).prop("checked"));
        $(this).closest("dl").find("a").text($.map($('input:checked:not(.multiSelAll, .mutliSelectSearch)', $(this).closest("ul")),
            function (n, i) {
                return $(n).parent().text();
            }).join("; "));

        $(this).closest("ul").find("input:not(.multiSelAll, .mutliSelectSearch)").last().change();
    });

    $(".multiSelSearchAll").change(function () {
        $(this).closest("ul").find("input").prop("checked", $(this).prop("checked"));
        $(this).closest("dl").find("a").text($.map($('input:checked:not(".multiSelSearchAll, .mutliSelectSearch")', $(this).closest("ul")),
            function (n, i) {
                return $(n).parent().text();
            }).join("; "));
    });
}

//Before each collapsing and expanding, check the colspan at the top left cell of the treelist and, 
//depending on it, change the colspan in the leftmost cell of the header 
//to the line above to match the first and second lines of the complex header
function OnEndCallback(s, e) {
    var $cell = $("#treeList_DX-DnD-H-0");
    var colspan = $cell.prop("colspan");

    var $celltochange = $("#treeList_colspan-2");
    if ($celltochange["0"] !== undefined) {
        $celltochange["0"].colSpan = Number(colspan) + 1;
    }

    InitContextMenu();
}

//creates temporary form, useful for simulating post requests
jQuery(function ($) {
    $.extend({
        form: function (url, data, method) {
            if (method == null) method = 'POST';
            if (data == null) data = {};

            var form = $('<form>').attr({
                method: method,
                action: url
            }).css({
                display: 'none'
            });

            var addData = function (name, data) {
                if ($.isArray(data)) {
                    for (var i = 0; i < data.length; i++) {
                        var value = data[i];
                        addData(name + '[]', value);
                    }
                } else if (typeof data === 'object') {
                    for (var key in data) {
                        if (data.hasOwnProperty(key)) {
                            addData(name + '[' + key + ']', data[key]);
                        }
                    }
                } else if (data != null) {
                    form.append($('<input>').attr({
                        type: 'hidden',
                        name: String(name),
                        value: String(data)
                    }));
                }
            };

            for (var key in data) {
                if (data.hasOwnProperty(key)) {
                    addData(key, data[key]);
                }
            }

            return form.appendTo('body');
        }
    });
});

function UpdateStatus(checked, key) {
    jQuery('input[parentKey = \'' + key + '\']').each(function () {
        this.checked = checked;
        UpdateStatus(checked, this.getAttribute('key'));
    });
}

function copyToClipboard(element, successText) {
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val($(element).text()).select();
    document.execCommand("copy");
    $temp.remove();

    $("#copyToClipboardTooltip").html(successText);
}

function uppercaseFirstLetter(string) {
    return string.replace(/\w\S*/g, function(word) {
        return word.charAt(0).toUpperCase() + word.substr(1).toLowerCase(); 
    });
}
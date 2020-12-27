$(function () {
    /*
        $.fn.getNonColSpanIndex = function () {
            if (!$(this).is('td') && !$(this).is('th'))
                return -1;
    
            var allCells = this.parent('tr').children();
            var normalIndex = allCells.index(this);
            var nonColSpanIndex = 0;
    
            allCells.each(function (i, item) {
                if (i == normalIndex)
                    return false;
    
                var colspan = $(this).attr('colspan');
                colspan = colspan ? parseInt(colspan) : 1;
                nonColSpanIndex += colspan;
            });
    
            return nonColSpanIndex;
        };
    
    $.fn.cellPos = function (rescan) {
        var $cell = this.first();
        var pos = $cell.data("cellPos");

        if (!pos || rescan) {
            var $table = $cell.closest("table, thead, tbody, tfoot");
            scanTable($table);
        }

        pos = $cell.data("cellPos");
        return pos;
    }
    */
    /*
    $.fn.getNonCallSpanIndex2 = function () {
        if (!$(this).is('td') && !$(this).is('th'))
            return -1;

        var clickedEl = $(this);
        var myCol = clickedEl.closest("td").index();
        var myRow = clickedEl.closest("tr").index();
        var rowspans = $("td[rowspan]");
        rowspans.each(function () {
            var rs = $(this);
            var rsIndex = rs.closest("tr").index();
            var rsQuantity = parseInt(rs.attr("rowspan"));
            if (myRow > rsIndex && myRow <= rsIndex + rsQuantity - 1) {
                myCol++;
            }
        });

//        alert('Row: ' + myRow + ', Column: ' + myCol);

        return myRow;
    };
    */


    /*
    function scanTable($table) {
        var m = [];
        $table.children("tr").each(function (y, row) {
            $(row).children("td, th").each(function (x, cell) {
                var $cell = $(cell),
                    cspan = $cell.attr("colspan") | 0,
                    rspan = $cell.attr("rowspan") | 0,
                    tx, ty;
                cspan = cspan ? cspan : 1;
                rspan = rspan ? rspan : 1;
                for (; m[y] && m[y][x]; ++x);
                for (tx = x; tx < x + cspan; ++tx) {
                    for (ty = y; ty < y + rspan; ++ty) {
                        if (!m[ty]) {
                            m[ty] = [];
                        }
                        m[ty][tx] = true;
                    }
                }
                var pos = { top: y, left: x };
                $cell.data("cellPos", pos);
            });
        });
    };
    */
    var $gallery = $(".controlsSidebar");

    $("li", $gallery).draggable({
        cancel: "a.ui-icon", // clicking an icon won't initiate dragging
        revert: "invalid", // when not dropped, the item will revert back to its initial position
        containment: "document",
        helper: "clone",
        cursor: "move",
        start: function (event, ui) {
            ui.helper.css('opacity', ".4");
        },
        drag: function (event, ui) {
        },
        stop: function (event, ui) {
        }
    });

    $("li", $("#toArea table")).draggable({
        cancel: "a.ui-icon", // clicking an icon won't initiate dragging
        revert: "invalid", // when not dropped, the item will revert back to its initial position
        containment: "document",
        helper: "clone",
        cursor: "move",
        start: function (event, ui) {
            ui.helper.css('opacity', ".4");
        },
        drag: function (event, ui) {
        },
        stop: function (event, ui) {
        }
    });

    addItemEvents();
    /*
    $gallery.droppable({
        accept: ".trashX li",
        classes: {
            //    "ui-droppable-active": "custom-state-active"
            "ui-droppable-hover": "ui-state-hover"
        },
        drop: function (event, ui) {
            removeControlFromTable(ui.draggable);
        }
    });

    $("td div.trashX").each(function (index) {

        var _trashx = $(this);

        _trashx.droppable({
            accept: ".controlsSidebar > li, .trashX li",
            classes: {
                //    "ui-droppable-active": "custom-state-active"
                "ui-droppable-hover": "ui-state-hover"
            },
            drop: function (event, ui) {
                addControlInTable(ui.draggable, _trashx);
            }
        });

    });
    */
    /*
    $("li", $gallery).resizable({
        grid: 40,
        maxWidth: 800,
        maxHeight: 400,
        autoHide: true,
        resize: function (event, ui) {
            $("input[name$=f_colspan]", ui.element).val(ui.size.width / 40);
            $("input[name$=f_rowspan]", ui.element).val(ui.size.height / 40);

            if (ui.element.parent().parent().parent().prop("tagName") == "TD") {
                restoreCellsInTable(ui.element.closest("td"));
                hideCellsInTable(
                    ui.element.closest("td"),
                    ui.element.find("input[name$=f_colspan]").val(),
                    ui.element.find("input[name$=f_rowspan]").val());
            }

            $(".debugInfo", ui.element).text(ui.element.find("input[name$=f_colspan]").val() + ', ' + ui.element.find("input[name$=f_rowspan]").val());
        }
    });
    */

    addResizableToControl($("li", $("#toArea table")));


    $("li", $("#toArea table")).append($("<a href='#' title='Remove control' class='ui-icon ui-icon-refresh'></a>"));


    $("ul.gallery > li").click(function (event) {
        var $item = $(this);
        var $target = $(event.target);

        if ($target.is("a.ui-icon-zoomin")) {
            showControlSettings($target);
        } else if ($target.is("a.ui-icon-refresh")) {
            removeControlFromTable($item);
        }

        return false;
    });

    $("table.grid td").each(function (index) {
        if ($(this).attr("colspan") > 1 || $(this).attr("rowspan") > 1) {
            hideCellsInTable($(this), $(this).attr("colspan"), $(this).attr("rowspan"));
        }
    });

    function addResizableToControl(controls) {
        controls.resizable({
            grid: 40,
            maxWidth: 800,
            maxHeight: 400,
            autoHide: true,
            start: function (event, ui) {
                var colspan = parseInt($("input[name$=f_colspan]", ui.element).val());
                var rowspan = parseInt($("input[name$=f_rowspan]", ui.element).val());

                for (index = 1; index < 20; index++) {
                    if (!allowControlConflictBySpan(colspan + index, rowspan, ui.element.closest("div"), 1)) {
                        break;
                    }
                }

                ui.element.resizable("option", "maxWidth", ((colspan + index) - 1) * 40);

                for (index = 0; index < 20; index++) {
                    if (!allowControlConflictBySpan(colspan, rowspan + index, ui.element.closest("div"), 1)) {
                        break;
                    }
                }

                ui.element.resizable("option", "maxHeight", ((rowspan + index) - 1) * 40);
            },
            resize: function (event, ui) {
                $("input[name$=f_colspan]", ui.element).val(ui.size.width / 40);
                $("input[name$=f_rowspan]", ui.element).val(ui.size.height / 40);

                if (ui.element.parent().parent().parent().prop("tagName") == "TD") {
                    showCellsInTable(
                        ui.element.closest("td"),
                        ui.element.closest("td").attr("colspan"),
                        ui.element.closest("td").attr("rowspan"));
                    hideCellsInTable(
                        ui.element.closest("td"),
                        ui.element.find("input[name$=f_colspan]").val(),
                        ui.element.find("input[name$=f_rowspan]").val());
                }

            //    $(".debugInfo", ui.element).text(ui.element.find("input[name$=f_colspan]").val() + ', ' + ui.element.find("input[name$=f_rowspan]").val());
            }
        });
    }

    function allowControlConflictCells(item, endContainer) {
        return allowControlConflictBySpan(
            parseInt($("input[name$=f_colspan]", item).val()),
            parseInt($("input[name$=f_rowspan]", item).val()),
            endContainer,
            0);
    }

    function allowControlConflictBySpan(colspan, rowspan, endContainer, minLength) {
        var table = endContainer.closest("table");
        var colIndex = endContainer.closest("td").index();
        if (colspan + colIndex > 21)
            return false;

        var rowIndex = endContainer.closest("tr").index();
        if (rowspan + rowIndex > 20)
            return false;

        if ($("tr:lt(" + (rowIndex + rowspan + 1) + "):gt(" + (rowIndex) + ")", table).find("td:lt(" + ((colIndex + colspan) - 1) + "):eq(" + (colIndex - 1) + "), td:lt(" + ((colIndex + colspan) - 1) + "):gt(" + (colIndex - 1) + ")").find('ul').length > minLength)
            return false;

        return true;
    }

    function addControlInTable($item, endContainer) {
        if (!allowControlConflictCells($item, endContainer))
            return false;

        if ($item.parent().parent().hasClass("trashX")) {
            if ($("ul", endContainer).length > 0)
                return false;

            var ulList = $item.parent();

            $item.closest("td").removeAttr("rowspan");
            $item.closest("td").removeAttr("colspan");
            showCellsInTable(
                $item.closest("td"),
                $item.find("input[name$=f_colspan]").val(),
                $item.find("input[name$=f_rowspan]").val());

            var $list = $("ul", endContainer).length ? $("ul", endContainer) : $("<ul class='gallery ui-helper-reset'/>").appendTo(endContainer);
            $item.appendTo($list).show();

            $("input[name$=f_table_index]", $item).val($("table").index($item.closest("table")));
            $("input[name$=f_row_index]", $item).val($item.closest("tr").index());
            $("input[name$=f_cell_index]", $item).val($item.closest("td").index());

            hideCellsInTable(
                $item.closest("td"),
                $item.find("input[name$=f_colspan]").val(),
                $item.find("input[name$=f_rowspan]").val())

            addItemEvents();

            ulList.remove();
        }
        else {
            var controlIndex = $("table .trashX ul").length;
            if ($("ul", endContainer).length > 0)
                return false;

            var $list = $("ul", endContainer).length ? $("ul", endContainer) : $("<ul class='gallery ui-helper-reset'/>").appendTo(endContainer);

            var newItem = ($item.find("input[name$=IsReusable]").val() == "1") ? $item.clone() : $item;
            newItem.find("input[name$=f_table_index]").val($("table").index(endContainer.closest("table")));
            newItem.find("input[name$=f_row_index]").val(endContainer.closest("tr").index());
            newItem.find("input[name$=f_cell_index]").val(endContainer.closest("td").index());


            newItem.find("*[name^='controls[].']").each(function (index) {
                $(this).attr("name", $(this).attr("name").replace("controls[].", "controls[" + controlIndex + "]."));
            });

            newItem.appendTo($list);

            newItem.find(".ui-resizable-handle").remove();

            addResizableToControl(newItem);

            newItem.append($("<a href='#' title='Control settings' class='ui-icon ui-icon-zoomin'>Settings</a>"));
            newItem.append($("<a href='#' title='Remove control' class='ui-icon ui-icon-refresh'></a>"));
            newItem.draggable({
                cancel: "a.ui-icon", // clicking an icon won't initiate dragging
                revert: "invalid", // when not dropped, the item will revert back to its initial position
                containment: "document",
                helper: "clone",
                cursor: "move",
                start: function (event, ui) {
                    ui.helper.css('opacity', ".4");
                },
            });

            newItem.on("click", function (event) {
                var $item = $(this),
                  $target = $(event.target);

                //      if ($target.is("a.ui-icon-trash")) {
                //          addControlInTable($item);
                if ($target.is("a.ui-icon-zoomin")) {
                    showControlSettings($("a.ui-icon-zoomin", $item));
                } else if ($target.is("a.ui-icon-refresh")) {
                    removeControlFromTable($item);
                }

                return false;
            });

            hideCellsInTable(
                newItem.parent().parent().parent(),
                newItem.find("input[name$=f_colspan]").val(),
                newItem.find("input[name$=f_rowspan]").val())
        }
    }

    function hideCellsInTable(tableCell, colspan, rownspan) {
        tableCell.attr("colspan", colspan);
        tableCell.attr("rowspan", rownspan);

        var tableRow = tableCell.parent();
        var table = tableRow.parent();

        for (rowIndex = 1; rowIndex < rownspan; rowIndex++) {
            for (cellIndex = 0; cellIndex < colspan; cellIndex++) {
                table.find("tr:eq(" + (tableRow.index() + rowIndex) + ") td:eq(" + ((tableCell.index() - 1) + cellIndex) + ")").hide();//.find("div").append("<ul>X</ul>");
            }
        }

        for (index = 0; index < colspan - 1; index++) {
            table.find("tr:eq(" + (tableRow.index()) + ") td:eq(" + (tableCell.index() + index) + ")").hide();//.find("div").append("<ul>X</ul>");
        }
    }

    function showCellsInTable(tableCell, colspan, rownspan) {
        var tableRow = tableCell.parent();
        var table = tableRow.parent();

        for (rowIndex = 1; rowIndex < rownspan; rowIndex++) {
            for (cellIndex = 0; cellIndex < colspan; cellIndex++) {
                table.find("tr:eq(" + (tableRow.index() + rowIndex) + ") td:eq(" + ((tableCell.index() - 1) + cellIndex) + ")").show();//.find("ul").remove();
            }
        }

        for (index = 0; index < colspan - 1; index++) {
            table.find("tr:eq(" + (tableRow.index()) + ") td:eq(" + (tableCell.index() + index) + ")").show();//.find("ul").remove();
        }
    }

    function addItemEvents() {
        $gallery.droppable({
            accept: ".trashX li",
            classes: {
                //    "ui-droppable-active": "custom-state-active"
                "ui-droppable-hover": "ui-state-hover"
            },
            drop: function (event, ui) {
                removeControlFromTable(ui.draggable);
            }
        });

        $("td div.trashX").each(function (index) {
            var _trashx = $(this);
            _trashx.droppable({
                accept: ".controlsSidebar > li, .trashX li",
                classes: {
                    //    "ui-droppable-active": "custom-state-active"
                    "ui-droppable-hover": "ui-state-hover"
                },
                drag: function (event, ui)  {
                },
                drop: function (event, ui) {
                    addControlInTable(ui.draggable, _trashx);
                }
        });

    });
}

function updateControlIndexForMVC($item)
{
    var controlIndex = 0;
    $(".trashX").each(function (tdIndex) {

        var foundControl = false;
        $("*[name^='controls[']", $(this)).each(function (index) {
            $(this).attr("name", $(this).attr("name").replace(/[0-9]+/g, controlIndex));
            foundControl = true;
        });

        if (foundControl) {
            controlIndex++;
        }
    });
}

function removeControlFromTable($item) {
    showCellsInTable($item.closest("td"), $item.closest("td").attr("colspan"), $item.closest("td").attr("rowspan"));
    $item.closest("td").removeAttr("rowspan");
    $item.closest("td").removeAttr("colspan");
    $item.parent().remove();

    updateControlIndexForMVC($item);
}

function showControlSettings(link) {
    $(".settings", link.parent()).dialog({
        title: "Control settings",
        width: 800,
        modal: true,
        closeText: "",
        close: function () {
            $(this).dialog("destroy");
        },
        buttons: {
            Save: function () {
                $(this).dialog("destroy");
            },
            Cancel: function () {
                $(this).dialog("destroy");
            }
        }
    });
    /*
    $(".settings", link.parent()).on('dialogclose', function (event) {
        alert('closed');
    });
    */
}

});
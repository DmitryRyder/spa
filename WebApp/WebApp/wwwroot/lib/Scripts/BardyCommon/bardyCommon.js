function checkPagerStyles() {
    function setEmptyTopPagerStyle() {
        $('#topSelectPage').attr('style', function (i, style) {
            return style && style.replace(/display[^;]+;?/g, '');
        });
        $('#topSelectPage').attr('style', 'margin-left:10px; float:right; background:transparent; margin-top: 43px; margin-bottom: 10px;')
    }

    function setEmptyBttomPagerStyle() {
        $('#bottomSelectPage').attr('style', function (i, style) {
            return style && style.replace(/display[^;]+;?/g, '');
        });
        $('#bottomSelectPage').attr('style', 'float:right; padding-bottom:20px; position: relative; margin-top: 0px;')
    }

    if ($('.custom-top-pager').is(':empty')) {
        setEmptyTopPagerStyle();
        setEmptyBttomPagerStyle();
    }
    else if ($('.custom-top-pager').find('.dx-hidden').length > 0) {
        setEmptyTopPagerStyle();
        setEmptyBttomPagerStyle();
    }
    else {
        $('#topSelectPage').attr('style', function (i, style) {
            return style && style.replace(/display[^;]+;?/g, '');
        });
        $('#topSelectPage').attr('style', 'margin-left:10px; float:right; background:transparent; position:relative; top:43px;');

        $('#bottomSelectPage').attr('style', function (i, style) {
            return style && style.replace(/display[^;]+;?/g, '');
        });
        $('#bottomSelectPage').attr('style', 'float:right; padding-bottom:20px; position: relative; margin-top: -52px;');
    }
}
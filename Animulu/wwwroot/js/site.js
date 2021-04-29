var sliderDivider = 6;
var tags = "";

new ResizeSensor(jQuery('.slider'), function () {
    $('.slider-a').height($('.slider-a').parent().height());
});

function LoginPopUp() {
    $('#LoginPop').addClass("active");
    $('#LoginPopOverlay').addClass("active");
}

function GetCommentContent() {
    return $('#CommentTextBox').text();
}

function CheckCommentBox() {
    if ($('#CommentTextBox').text().trim().length > 0) {
        $('#CommentButton').prop('disabled', false);
    }
    else {
        $('#CommentButton').prop('disabled', true);
    }
}

$(document).ready(function () {
    const urlParams = new URLSearchParams(window.location.search);
    var setOrderBtns = $('.orderBtn');
    var setTagBtns = $('.tagBtn');

    for (var i = 0; i < setTagBtns.length; i++) {
        setTagBtns[i].checked = false;
    }
    for (var i = 0; i < setOrderBtns.length; i++) {
        setOrderBtns[i].checked = false;
    }

    $('#searchInput').val(urlParams.get("quote"));

    if (urlParams.get("tag") != "" && urlParams.get("tag") != null) {
        $("#tagBox").val(urlParams.get("tag"));
        $("#tagBox").prop("checked", true);
        var parmTags = [""];
        try {
            parmTags = urlParams.get("tag").split(" ");
        }
        catch {
            parmTags[0] = urlParams.get("tag");
        }
        for (var i = 0; i < setTagBtns.length; i++) {
            for (var n = 0; n < parmTags.length; n++) {
                if (parmTags[n] == setTagBtns[i].value) {
                    setTagBtns[i].checked = true;
                    $(setTagBtns[i].parentNode).toggleClass("active", true);
                    break;
                }
            }
        }
    }

    if (urlParams.get("order") != "" && urlParams.get("order") != null) {
        $("#orderBox").val(urlParams.get("order"));
        $("#orderBox").prop("checked", true);
        var parmOrder = urlParams.get("order");
        for (var i = 0; i < setOrderBtns.length; i++) {
            if (parmOrder == setOrderBtns[i].value) {
                setOrderBtns[i].checked = true;
                $(setOrderBtns[i].parentNode).toggleClass("active", true);
                break;
            }
        }
    }

    $('#tagsDropdown').on('hide.bs.dropdown', function (e) {
        var oldtags = $("#tagBox").val();
        tags = "";
        var tagBtns = $('.tagBtn');
        for (var i = 0; i < tagBtns.length; i++) {
            if (tagBtns[i].checked && tags == "") {
                tags = tagBtns[i].value
            }
            else if (tagBtns[i].checked) {
                tags = tags + " " + tagBtns[i].value
            }
        }
        if (tags != "") {
            $("#tagBox").prop("checked", true);
        }
        $("#tagBox").val(tags);
        if (tags != oldtags) {
            $("#searchForm").submit();
        }
    });

    $(".checkbox-menu").on("change", "input[type='checkbox']", function () {
        $(this).closest("label").toggleClass("active", this.checked);
    });

    $(".radio-menu").on("change", "input[type='radio']", function () {
        $(".radio-active").toggleClass("active", false);
        $(this).closest("label").toggleClass("active", this.checked);
        var orderBtns = $('.orderBtn');
        for (var i = 0; i < orderBtns.length; i++) {
            if (orderBtns[i].checked) {
                $("#orderBox").val(orderBtns[i].value);
                $("#orderBox").prop("checked", true);
                $("#searchForm").submit();
                return;
            }
        }
    });

    $(document).on('click', '.allow-focus', function (e) {
        e.stopPropagation();
    });

    $('.slider').css("position", "inherit");

    $('.slider-ar').on('click',function () {
        $(this.parentElement).animate({
            scrollLeft: "+=" + (this.parentElement.scrollWidth / sliderDivider)
        }, "slow");
        $(this.parentElement.children[0]).fadeIn("fast");
        if ((this.parentElement.scrollLeft + (this.parentElement.scrollWidth / sliderDivider)) >= ((this.parentElement.scrollWidth - this.parentElement.clientWidth) - 2)) {
            $(this).fadeOut("fast");
        }
    });

    $('.slider-al').on('click',function () {
        $(this.parentElement).animate({
            scrollLeft: "-=" + (this.parentElement.scrollWidth / sliderDivider)
        }, "slow");
        $(this.parentElement.children[1]).fadeIn("fast");
        if ((this.parentElement.scrollLeft - (this.parentElement.scrollWidth / sliderDivider)) <= 0) {
            $(this).fadeOut("fast");
        }
    });

    $('.slider').on('mouseenter', function () {
        if (this.scrollLeft != 0) {
            $(this.children[0]).fadeIn("fast");
        } 
        if (this.scrollLeft <= (this.scrollWidth - this.clientWidth)-2) {
            $(this.children[1]).fadeIn("fast");
        }
    });

    $('.slider').on('mouseleave', function () {
        $(this.children[0]).fadeOut("fast");
        $(this.children[1]).fadeOut("fast");
    });
    
    $("#sidebar").mCustomScrollbar({
        theme: "minimal-dark",
        mouseWheelPixels: 25,
        scrollInertia: 0
    });

    $('#dismiss, #aside-overlay').on('click', function () {
        $('#sidebar').removeClass('active');
        $('#aside-overlay').removeClass('active');
    });

    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').addClass('active');
        $('#aside-overlay').addClass('active');
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });

    $('#LoginPopOverlay, #LoginPopClose').on('click', function () {
        $('#LoginPop').removeClass('active');
        $('#LoginPopOverlay').removeClass('active');
    });

    $('#searchInput').on('focusout', function () {
        $('#searchForm').removeClass("form-wide");
        $('#searchForm').removeClass("wrapper-search-shadow")
    });

    $('#searchInput').on('focus', function () {
        $('#searchForm').addClass("form-wide");
        $('#searchForm').addClass("wrapper-search-shadow")
    });

    if ($(this).scrollTop() != 0) {
        $('#topBar').addClass('navbar-sticky');
        $('#topBar').addClass('shadow');
        $('#searchInput').removeClass("search-bar-grey");
        $('#searchInput').addClass("search-bar-purple");
        $('#signIn').removeClass("btn-square-grey");
        $('#signUp').removeClass("btn-square-grey");
        $('#signIn').addClass("btn-square-purple");
        $('#signUp').addClass("btn-square-purple");
    } else {
        $('#topBar').removeClass('navbar-sticky');
        $('#topBar').removeClass('shadow');
        $('#searchInput').addClass("search-bar-grey");
        $('#searchInput').removeClass("search-bar-purple");
        $('#signIn').addClass("btn-square-grey");
        $('#signUp').addClass("btn-square-grey");
        $('#signIn').removeClass("btn-square-purple");
        $('#signUp').removeClass("btn-square-purple");
    }

    if ($(window).outerWidth() >= 1701 - 17) {
        sliderDivider = 5;
    }
    else if ($(window).outerWidth() >= 993 - 17) {
        sliderDivider = 6;
    }
    else if ($(window).outerWidth() >= 769 - 17) {
        sliderDivider = 10;
    }
    else {
        sliderDivider = 15;
    }
});

$(window).scroll(function () {
    if ($(this).scrollTop() != 0) {
        $('#topBar').addClass('navbar-sticky');
        $('#topBar').addClass('shadow');
        $('#searchInput').removeClass("search-bar-grey");
        $('#searchInput').addClass("search-bar-purple");
        $('#signIn').removeClass("btn-square-grey");
        $('#signUp').removeClass("btn-square-grey");
        $('#signIn').addClass("btn-square-purple");
        $('#signUp').addClass("btn-square-purple");
    } else {
        $('#topBar').removeClass('navbar-sticky');
        $('#topBar').removeClass('shadow');
        $('#searchInput').addClass("search-bar-grey");
        $('#searchInput').removeClass("search-bar-purple");
        $('#signIn').addClass("btn-square-grey");
        $('#signUp').addClass("btn-square-grey");
        $('#signIn').removeClass("btn-square-purple");
        $('#signUp').removeClass("btn-square-purple");
    }
});

$(window).resize(function () {
    if ($(window).outerWidth() >= 1701) {
        sliderDivider = 5;
    }
    else if ($(window).outerWidth() >= 993) {
        sliderDivider = 6;
    }
    else if ($(window).outerWidth() >= 769) {
        sliderDivider = 10;
    }
    else {
        sliderDivider = 15;
    }
});
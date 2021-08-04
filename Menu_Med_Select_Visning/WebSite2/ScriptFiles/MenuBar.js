var TopMenuBarContent = $("#TopMenuBarContent");
var MenuCurrentItemCSSClassName = "MenuCurrentItem";

var CheckForDropDownClassName = "dropdown-content";
var MenuCurrentItemParentCSSClassName = "MenuCurrentItemParent";

function MakeTopMenuBar() {
    TopMenuBarContent.load("TopMenuBar.html"); 

    var CurrentPage;

    try {
        CurrentPage = document.location.href.match(/[^\/]+$/)[0];
    }
    catch (err) {
        CurrentPage = "Index.html";
    }
    finally {
        setTimeout(function () {
            PageLoadTimeOut(CurrentPage);
        }, 200);
    }
}

function PageLoadTimeOut(CurrentPageInfo) {
    //$("#TopMenuBarContent ul li").find('a').each(function () {
    $("> ul li", TopMenuBarContent).find('a').each(function () {
        if ($(this).attr('href').toLowerCase() == CurrentPageInfo.toLowerCase()) {
            $(this).addClass(MenuCurrentItemCSSClassName);
            if ($(this).parent().hasClass(CheckForDropDownClassName)) {
                //$(this).parent().closest('a').addClass(MenuCurrentItemParentCSSClassName);
                $(this).parent().prev().addClass(MenuCurrentItemParentCSSClassName);
            }
        }
    });
}
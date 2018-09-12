//左滑菜单
$(document).ready(function(){
    $(".itemslist").click(function(){
        $(".itemslist-panel").animate({width:'toggle'},300);
        $('.itemslist-panel').css('height',document.body.clientHeight+'rem');
        $('.itemslist-bg').css('display','block');
    });

});
//选项卡
$(function () {
    $("#nav a").off("click").on("click", function () {
        var index = $(this).index();
        $("#index_contentBox .index_box").eq(index).addClass("active").siblings().removeClass("active");
    });
    $("#index_on").click(function(){
        $("#index_on").css({"color":"#366cb3"});
        $("#index_close").css({"color":"black"});
    });
    $("#index_close").click(function(){
        $("#index_on").css({"color":"black"});
        $("#index_close").css({"color":"#366cb3"});
    });

    $("#mytask_nav a").off("click").on("click", function () {
        var index = $(this).index();
        $("#mytask_contentBox .index_box").eq(index).addClass("active").siblings().removeClass("active");
    });
    $("#mytask_on").click(function(){
        $("#mytask_on").css({"color":"#366cb3"});
        $("#mytask_close").css({"color":"black"});
    });
    $("#index_close").click(function(){
        $("#index_on").css({"color":"black"});
        $("#index_close").css({"color":"#366cb3"});
    });
    $('.box-star').click(function(){
        if ($(this).find('img').attr('src') =='/Task/images/star.png'){
            $(this).find('img').attr('src','/Task/images/orstar.png');
        }else{
            $(this).find('img').attr('src','/Task/images/star.png');
        }
    });
});

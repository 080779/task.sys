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
    //显示个人中心
    // $("#index_user").click(function(){
    //     $("#index_user_content").show();
    //     $("#index_mytask_content").hide();
    //     $("#index_change").hide();
    // });
    //显示我的任务
    // $("#index_mytask").click(function(){
    //     $("#index_mytask_content").show();
    //     $("#index_user_content").hide();
    //     $("#index_change").hide();
    // });
});

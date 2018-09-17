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
    //星星收藏
    $('.box-star').click(function(){
        if($(this).find('img').attr('src')=='images/star.png'){
            $(this).find('img').attr('src','images/orstar.png');
        }else{
            $(this).find('img').attr('src','images/star.png');
        }
    });
    $("#btnHide").hide();
    $('#btnShow').click(function(){
        $("#btnShow").hide();
        $("#btnHide").show();
    });
    $('#giveUp').click(function(){
        $("#btnShow").show();
        $("#btnHide").hide();
    });
});
$(function() {
    $(".pic").click(function () {
        $(this).parent().find(".upload").click(); //隐藏了input:file样式后，点击头像就可以本地上传
        $(this).parent().find(".upload").on("change",function(){
            var objUrl = getObjectURL(this.files[0]) ; //获取图片的路径，该路径不是图片在本地的路径
            if (objUrl) {
                $(this).parent().find(".pic").attr("src", objUrl) ; //将图片路径存入src中，显示出图片
            }
            $("#img-p").hide();
        });
    });
});

function getObjectURL(file) {
    var url = null ;
    if (window.createObjectURL!=undefined) { // basic
        url = window.createObjectURL(file) ;
    } else if (window.URL!=undefined) { // mozilla(firefox)
        url = window.URL.createObjectURL(file) ;
    } else if (window.webkitURL!=undefined) { // webkit or chrome
        url = window.webkitURL.createObjectURL(file) ;
    }
    return url ;
}


$(function () {
	//左侧菜单点击操作
    $('.J_menuItem').on('click', function(){
        var dataUrl = $(this).attr('href');
        $('iframe.J_iframe').attr("src", dataUrl);
        return false;
    });
    
    //左侧菜单隐藏开启操作
    $('#site-side-tree-opc').on('click', function(){
    	var cls = $(this).attr("class");
    	if(cls == 'open'){
    		$('.layui-side').animate({left:'-200px'});
    		$('.layui-body').animate({left:'0'});
    		$('.layui-footer').animate({left:'0'});
    		$(this).attr("class", "close");
    		$(this).animate({left:"-1px"});
    		$('#site-side-tree-opc i.layui-icon').html("&#xe602;");
    	}else{
    		$('.layui-side').animate({left:'0'});
    		$('.layui-body').animate({left:'200px'});
    		$('.layui-footer').animate({left:'200px'});
    		$(this).attr("class", "open");
    		$(this).animate({left:"195px"});
    		$('#site-side-tree-opc i.layui-icon').html("&#xe603;");
    	}
        return false;
    });
    
    //开启隐藏帮助面板
    $("a.site-help").on('click', function(){
    	$("#site-right-sidebar").toggle();
    	return false;
    });
});
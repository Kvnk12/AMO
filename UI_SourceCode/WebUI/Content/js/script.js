$(document).ready(function () {
	$('.appNavClose').hide();	
	$('.appNavOpen').click(function() {				
		$('.pushContainer1Content').animate({left:240});
		$('#appNav').animate({width:240});
		$('.appNavClose').show();
		$(this).hide();
	});		
	$('.appNavClose').click(function() {
		$('.pushContainer1Content').animate({left:0});
		$('#appNav').animate({width:0});
		$('.appNavOpen').show();
		$(this).hide();
		
	});
	$('.treemenuOpen').click(function() {
		$('.treemenu').find('li').removeClass("sel");
		$('.treeMenuContent').animate({left:250});
		$('.treemenu').animate({width:250});
		$(this).hide();
	});		
	$('.treemenuClose').click(function() {
		$('.treemenu li').find('ul').css("display", "none");
		$('.treeMenuContent').animate({left:30});
		$('.treemenu').animate({width:0});
		$('.treemenuOpen').show();
	});
	
	$('.treeMenuUl li ul').parent('li').addClass('parent');
	$('.treeMenuUl .parent > a').each(function(){
		$(this).click(function(){
			$(this).next('ul').slideToggle();
			if($(this).parent().hasClass('sel') == true){
				$(this).parent().removeClass('sel');
			}
			else{
				$(this).parent().addClass('sel');
			}
		});
	});
	
	$('.appMenuUl li ul').parent('li').addClass('parent');
	$('.appMenuUl .parent > a').each(function(){
		$(this).click(function(){
			$(this).next('ul').slideToggle();
			if($(this).parent().hasClass('sel') == true){
				$(this).parent().removeClass('sel');
			}
			else{
				$(this).parent().addClass('sel');
			}
		});
	});
	
	$('.nav-tabs').find('li').on('click',function(e) {
		e.preventDefault();
		var tabli = $(this).find('a').attr('href');
		$(tabli).addClass('opened').removeClass('closed').siblings('div').addClass('closed').removeClass('opened');
	});
	
//	$('.deleteUser').on('click',function(e){
//	e.preventDefault();
//	$('#myModaldelete').modal('show')
//	});
     
	// $('.editUser').on('click',function(e){
	//e.preventDefault();
//	$('#editUser').modal('show')
	//});
});
$(document).ready(sizeContent);
$(window).resize(sizeContent);
function sizeContent() {
    //var bodyHeight = $(window).height() + $('.navbar').height()-60;
	//$('#appNav').height(bodyHeight).css('top',$('.navbar').height());
	
}


$(document).ready(function() {
    $('.warning').addClass('navbar-fixed-bottom');
	$('.closewarning').click(function(){
		$('.warning').fadeOut(1000);
		})
});
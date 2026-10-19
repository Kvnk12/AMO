$(document).ready(function () {

    $('.appNavClose').hide();
    $('.appNavOpen').click(function () {
        $(document).find('.pushContainer1').attr('id', 'contWrap');
        $('#contWrap').css({ 'width': '80%' });
        $('.pushContainer1Content').animate({ left: 270 });//240
        $('#appNav').animate({ width: 240 });
        $('.appNavClose').show();
        $(this).hide();
    });
    $('.appNavClose').click(function () {
        $(document).find('.pushContainer1').removeAttr('id style');
        $('.pushContainer1Content').animate({ left: 0 });
        $('#appNav').animate({ width: 0 });
        $('.appNavOpen').show();
        $(this).hide();

    });

    $('.treemenuOpen').click(function () {
        $(document).find('.treeMenuContent').attr('id', 'treeWrap');
        $('#treeWrap').css({ 'width': '80%' });
        $('#treeMenuArea').css({ 'width': '92%' });
        $('#metricSetupArea').css({ 'width': '92%' });
        $('#heatmapContent').css({ 'width': '92%' });//changed on 13th march for hearmap
        //$('.slarightpane').css({'width':'24%'});
        $('.treemenu').find('li').removeClass("sel");
        $('.treeMenuContent').animate({ left: 250 });
        $('.treemenu').animate({ width: 257 });
        $(this).hide();
        $('.selectFiltersClose').hide();//added on 29th may for showing the graphs correctly in the layout
        $('.selectFiltersOpen').show();//added on 29th may for showing the graphs correctly in the layout
        $(document).find('.slaleftpane').attr('id', 'slaleftpane');	//added on 29th may for showing the graphs correctly in the layout
        $(document).find('.slarightpane').attr('id', 'slarightpane');//added on 29th may for showing the graphs correctly in the layout
    });
    $('.treemenuClose').click(function () {
        $(document).find('.treeMenuContent').removeAttr('id style');
        $('.treemenu li').find('ul').css("display", "none");
        $('.treeMenuContent').animate({ left: 30 });
        $('.treemenu').animate({ width: 0 });
        $('.treemenuOpen').show();
    });

    $('.treeMenuUl li ul').parent('li').addClass('parent');
    $('.treeMenuUl .parent > a').each(function () {
        $(this).click(function () {
            $(this).next('ul').slideToggle();
            if ($(this).parent().hasClass('sel') == true) {
                $(this).parent().removeClass('sel');
            }
            else {
                $(this).parent().addClass('sel');
            }
        });
    });

    $('.bamtreeMenuUl li ul').parent('li').addClass('parent');
    $('.bamtreeMenuUl .parent > a').each(function () {
        $(this).click(function (e) {
            $(this).next('ul').slideToggle();
            e.preventDefault();
            if ($(this).parent().hasClass('sel') == true) {
                $(this).parent().removeClass('sel');
            }
            else {
                $(this).parent().addClass('sel');
            }
        });
    });

    $('.appMenuUl li ul').parent('li').addClass('parent');
    $('.appMenuUl .parent > a').each(function () {
        $(this).click(function () {
            $(this).next('ul').slideToggle();
            if ($(this).parent().hasClass('sel') == true) {
                $(this).parent().removeClass('sel');
            }
            else {
                $(this).parent().addClass('sel');
            }
        });
    });

    $('.nav-tabs').find('li').on('click', function (e) {
        e.preventDefault();
        var tabli = $(this).find('a').attr('href');
        $(tabli).addClass('opened').removeClass('closed').siblings('div').addClass('closed').removeClass('opened');
    }); //nav tab for SLA

    $('.deleteUser').on('click', function (e) {
        e.preventDefault();
        $('#myModaldelete').modal('show')
        $('#myModaldelete').modal({ keyboard: true, backdrop: 'static' })
    });

    $('#csgtrends').on('click', function (e) {
        e.preventDefault();
        $('#costSummaryTrend').modal('show')
        $('#costSummaryTrend').modal({ keyboard: true, backdrop: 'static' })
    });

    //$('.editUser').on('click',function(e){
    //e.preventDefault();
    //$('#editUser').modal('show')
    //$('#editUser').modal({keyboard: true, backdrop: 'static'}) 
    //});

    $('.deleteValueChain').on('click', function (e) {
        e.preventDefault();
        $('#myModaldelete').modal('show')
        $('#myModaldelete').modal({ keyboard: true, backdrop: 'static' })
    });

    //$('.setval').on('click',function(e){
    //e.preventDefault();
    //$('#metricsetupContent').modal('show')
    //$('#metricsetupContent').modal({keyboard: true, backdrop: 'static'}) 
    //});

    $('#showapp').on('click', function (e) {
        e.preventDefault();
        $('.metricsetupRight').css('display', 'block');
    });

    $('.applistclose').on('click', function (e) {
        e.preventDefault();
        $('.metricsetupRight').css('display', 'none');
    });

    $('#showaccdetail').on('click', function (e) {
        e.preventDefault();
        $('.accdetailArea').css('display', 'block');
    });


});

$(document).ready(function () {
    $('.metricchk').change(function () {
        if (this.checked)
            $('#appname').fadeOut('slow');
        else
            $('#appname').fadeIn('slow');
    });
});

$(document).ready(sizeContent);
$(window).resize(sizeContent);
function sizeContent() {
    //var bodyHeight = $(window).height() + $('.navbar').height()-60;
    //$('#appNav').height(bodyHeight).css('top',$('.navbar').height());

}


$(document).ready(function () {
    $('.warning').addClass('navbar-fixed-bottom');
    $('.closewarning').click(function () {
        $('.warning').fadeOut(1000);
    })
});

$(document).ready(function () {

    $('.admin-tabs').find('li').on('click', function (e) {
        e.preventDefault();
        var tabli = $(this).find('a').attr('href');
        $(tabli).addClass('opened').removeClass('closed').siblings('div').addClass('closed').removeClass('opened');
    });// function to hide and show tab nav panel

    $('.admindataeleval a[href*="#"]').on('click', function (e) {
        e.preventDefault();
        var ele = $(this).attr('href');
        $(ele).show().siblings().hide();
    });// function to hide and show the right panel metadata values in client profile - user story 4	
});

$(document).ready(function () {
    $('.dAreaOpen').hide();
    $('.rightNavoneClose').click(function () {
        $('.dependancyArea').hide();
        $('.dAreaOpen').show();
        $('.dAreaOpen').addClass('identifier');
        $(document).find('.dAreaOpen').css("margin-left", "0");
        if ($('.oAreaOpen').is('.identifier') && $('.dAreaOpen').is('.identifier')) {
            $(document).find('.amgrcontentLeft').attr('id', 'amgrcontentLeft');
            $(document).find('.amgrcontentRight').attr('id', 'amgrcontentRight');
            $(document).find('.dAreaOpen').css("margin-left", "0");
        }

        $('.dAreaOpen').click(function () {
            $('.dependancyArea').show();
            $('.dAreaOpen').removeClass('identifier');
            $('.dAreaOpen').hide()
            $(document).find('.amgrcontentLeft').removeAttr('id', 'amgrcontentLeft');
            $(document).find('.amgrcontentRight').removeAttr('id', 'amgrcontentRight');
        });
    });


    $(function () {

        // select all / deselect all
        $("#selectall").change(function () {
            $(".mycheckbox").prop('checked', $(this).prop('checked'));
        });

        // find if all the checkboxes are checked, if then select the top one too.
        $(".mycheckbox").change(function () {
            if ($(".mycheckbox").length == $(".mycheckbox:checked").length)
                $("#selectall").attr("checked", "checked");
            else
                $("#selectall").removeAttr("checked");
        });
        // select all / deselect all
        $("#selectalltwo").change(function () {
            $(".mycheckboxtwo").prop('checked', $(this).prop('checked'));
        });

        // find if all the checkboxes are checked, if then select the top one too.
        $(".mycheckboxtwo").change(function () {
            if ($(".mycheckboxtwo").length == $(".mycheckbox:checked").length)
                $("#selectalltwo").attr("checked", "checked");
            else
                $("#selectalltwo").removeAttr("checked");
        });
    });


    $(function () {
        // select all / deselect all
        $("#selectallapps").change(function () {
            $(".assigndchk").prop('checked', $(this).prop('checked'));
        });

        // find if all the checkboxes are checked, if then select the top one too.
        $(".mycheckbox").change(function () {
            if ($(".assigndchk").length == $(".assigndchk:checked").length)
                $("#selectallapps").attr("checked", "checked");
            else
                $("#selectallapps").removeAttr("checked");
        });

        $('.collapse').on('shown.bs.collapse', function () {
            $(this).parent().find(".accord-plus").removeClass("accord-plus").addClass("accord-minus");
        }).on('hidden.bs.collapse', function () {
            $(this).parent().find(".accord-minus").removeClass("accord-minus").addClass("accord-plus");
        });
    });

    $('.oAreaOpen').hide();
    $('.rightNavtwoClose').click(function () {
        $('.otherInfoArea').hide();

        $('.oAreaOpen').addClass('identifier');
        $(document).find('.oAreaOpen').css("margin-left", "0");
        if ($('.dAreaOpen').is('.identifier') && $('.oAreaOpen').is('.identifier')) {
            $(document).find('.amgrcontentLeft').attr('id', 'amgrcontentLeft');
            $(document).find('.amgrcontentRight').attr('id', 'amgrcontentRight');
            $(document).find('.oAreaOpen').css("margin-left", "0");
        }
        $('.oAreaOpen').show();
        $('.oAreaOpen').click(function () {
            $('.otherInfoArea').show();
            $('.oAreaOpen').removeClass('identifier');
            $('.oAreaOpen').hide();
            $(document).find('.amgrcontentLeft').removeAttr('id', 'amgrcontentLeft');
            $(document).find('.amgrcontentRight').removeAttr('id', 'amgrcontentRight');
        });
    });
});
/*On 19th feb for BAM 7 to refresh tabs to solve fixed header table*/
$(document).ready(function () {
    $('#sletab').click(function () {
        $("#table-xthf3").hide();
    });
    $('#bpmtab').click(function () {
        $("#table-xthf3").hide();
    });
    $('#dpndtab').click(function () {
        $("#table-xthf3").hide();
    });
    $('#appmaptab').click(function () {
        $("#table-xthf3").show();
    });
});
/*function ends here*/

/*added on 25th arp to implement right panel(select filters) for SLA Screen*/
$(document).ready(function () {
    $('.selectFiltersOpen').hide();
    $('.slarightnavClose').click(function () {
        $('.selectFiltersClose').hide();
        $('.selectFiltersOpen').show();
        $(document).find('.slaleftpane').attr('id', 'slaleftpane');
        $(document).find('.slarightpane').attr('id', 'slarightpane');
    });
    $('.selectFiltersOpen').click(function () {
        $('.selectFiltersOpen').hide();
        $('.selectFiltersClose').show();
        $(document).find('.slaleftpane').removeAttr('id', 'slaleftpane');
        $(document).find('.slarightpane').removeAttr('id', 'slarightpane');
        $(document).find('.treeMenuContent').removeAttr('id style');//added on 30th may for hiding the treemenu on opening the select filters
        $('.treemenu li').find('ul').css("display", "none");//added on 30th may for hiding the treemenu on opening the select filters
        $('.treeMenuContent').animate({ left: 30 });//added on 30th may for hiding the treemenu on opening the select filters
        $('.treemenu').animate({ width: 0 });//added on 30th may for hiding the treemenu on opening the select filters
        $('.treemenuOpen').show();//added on 30th may for hiding the treemenu on opening the select filters
    })
});

/*Added this function for SLA Service Hours function - tab 1 on 20th may*/
function disablefieldsweek() {
    if (document.getElementById('srhrsweek').checked == 1) {
        document.getElementById('monStart').disabled = 'disabled';
        document.getElementById('monEnd').disabled = 'disabled';
        document.getElementById('tueStart').disabled = 'disabled';
        document.getElementById('tueEnd').disabled = 'disabled';
        document.getElementById('wedStart').disabled = 'disabled';
        document.getElementById('wedEnd').disabled = 'disabled';
        document.getElementById('thurStart').disabled = 'disabled';
        document.getElementById('thurEnd').disabled = 'disabled';
        document.getElementById('friStart').disabled = 'disabled';
        document.getElementById('friEnd').disabled = 'disabled';
        document.getElementById('timepicker1').disabled = '';
        document.getElementById('timepicker2').disabled = '';
    } else {
        document.getElementById('timepicker1').disabled = 'disabled';
        document.getElementById('timepicker2').disabled = 'disabled';
        document.getElementById('monStart').disabled = '';
        document.getElementById('monEnd').disabled = '';
        document.getElementById('tueStart').disabled = '';
        document.getElementById('tueEnd').disabled = '';
        document.getElementById('wedStart').disabled = '';
        document.getElementById('wedEnd').disabled = '';
        document.getElementById('thurStart').disabled = '';
        document.getElementById('thurEnd').disabled = '';
        document.getElementById('friStart').disabled = '';
        document.getElementById('friEnd').disabled = '';
    }
}
/*Added this function for SLA Service Hours function - tab 1 on 20th may*/
function disablefieldsweekend() {
    if (document.getElementById('srhrsweekend').checked == 1) {
        document.getElementById('satStart').disabled = 'disabled';
        document.getElementById('satEnd').disabled = 'disabled';
        document.getElementById('sunStart').disabled = 'disabled';
        document.getElementById('sunEnd').disabled = 'disabled';
        document.getElementById('timepicker3').disabled = '';
        document.getElementById('timepicker4').disabled = '';
    } else {
        document.getElementById('timepicker3').disabled = 'disabled';
        document.getElementById('timepicker4').disabled = 'disabled';
        document.getElementById('satStart').disabled = '';
        document.getElementById('satEnd').disabled = '';
        document.getElementById('sunStart').disabled = '';
        document.getElementById('sunEnd').disabled = '';
    }
}
/*Added this function for SLA Service Hours function - tab 2 on 28th may*/
/*$(document).ready(function(){
	var i=1;
	$("#addnewholiday").click(function(){
		$("table#slaHoliday tbody:first").clone().find("input").each(function(){
			$(this).val('').attr('id', function(_, id) { return id + i });
			}).end().appendTo("table#slaHoliday");	
		i++;
	});
});*/
$(document).ready(function () {
    var pageURL = $(location).attr("href");
    pageURL = pageURL.replace('?', '/');
    pageURL = pageURL.replace('#', '/');
    var array_url = pageURL.split('/');

    //console.log(array_url);
    //"SLAManager:::collapseOne" "Controllername::: accordion id"
    var appNamesInAccordion = ["SLASetup:::collapseOne:::GetAppList:::open-ul-1",
 "IncidentDashboard:::collapseOne:::GetViewOverallSummaryConfig:::open-ul-2", "Incident:::collapseOne:::LoadDataSet:::onetwothree", "SLAMDashboard:::collapseOne:::LayoutView:::onetwothree",
"Client:::collapseTwo:::GetClientInfo:::open-ul-3", "Client:::collapseTwo:::GetEngagementInfo:::open-ul-3", "Client:::collapseTwo:::GetAccountsInfo:::open-ul-3", "ClientProfileManager:::collapseTwo:::PerformanceCompliance:::open-ul-3", "ClientProfileManager:::collapseTwo:::ClientProfileAnalysis:::open-ul-3",
"Client:::collapseTwo:::Create:::open-ul-4", "ClientProfileManager:::collapseTwo:::TicketInfo:::open-ul-4", "ClientAccountGrid:::collapseTwo:::DisplayClientAccountContracts:::open-ul-4",
 "ClientProfileManager:::collapseTwo:::Dashboard:::onetwothree",
  "ClientProfileManager:::collapseTwo:::Dashboard:::open-ul-3",

"IncidentNavigator:::collapseThree:::Dashboard:::onetwothree",
"IncidentNavigator:::collapseThree:::TrendingReport:::onetwothree",
"IncidentNavigator:::collapseThree:::ReadKey:::onetwothree",
"IncidentNavigator:::collapseThree:::Addkey:::onetwothree", "IncidentNavigator:::collapseThree:::KnownerrorDB:::onetwothree",
"IncidentNavigator:::collapseThree:::ClusterReport:::open-ul-6", "IncidentNavigator:::collapseThree:::Cluster:::open-ul-6",
"IncidentNavigator:::collapseThree:::ClusterManagement:::open-ul-6", "IncidentNavigator:::collapseThree:::ClusterAnalysis:::open-ul-6",
"IncidentNavigator:::collapseThree:::ClusterAudit:::open-ul-7",
"PortfolioAnalyzer:::collapseFour:::ApplicationInformation:::open-ul-8", "PortfolioAnalyzer:::collapseFour:::Portfolio:::open-ul-8",
"PortfolioAnalyzer:::collapseFour:::DataCollection:::open-ul-9", "PortfolioAnalyzer:::collapseFour:::TechnologyStack:::open-ul-9", "PortfolioAnalyzer:::collapseFour:::Interface:::open-ul-9", "PortfolioAnalyzer:::collapseFour:::DispositionDetails:::open-ul-9",
"PortfolioAnalyzer:::collapseFour:::AppDashboardIndex:::open-ul-10", "PortfolioAnalyzer:::collapseFour:::Landing:::open-ul-10", "PortfolioAnalyzer:::collapseFour:::Analysis:::open-ul-10", "PortfolioAnalyzer:::collapseFour:::DispositionRoadMapReport:::open-ul-10", "PortfolioAnalyzer:::collapseFour:::RatingBySubPerspectiveReport:::open-ul-10",
"PortfolioAnalyzer:::collapseFour:::MetaData:::open-ul-11", "PortfolioAnalyzer:::collapseFour:::TechnologyStackMetadata:::open-ul-11", "PortfolioAnalyzer:::collapseFour:::VendorProductVersionMetadata:::open-ul-11",
"IdeaManager:::collapseidea:::DisplayIdeas:::appMenuUl", "IdeaManager:::collapseidea:::DisplayChallenges:::onetwothree", "IdeaManager:::collapseidea:::IdeasApproved:::onetwothree", "IdeaManager:::collapseidea:::ChallengesApproval:::onetwothree",
"IdeaManager:::collapseidea:::DashboardIndex:::open-ul-12", "IdeaManager:::collapseidea:::Dashboard:::open-ul-12", "IdeaManager:::collapseidea:::PostIdea:::onetwothree", "IdeaManager:::collapseidea:::ViewIdea:::onetwothree", "IdeaManager:::collapseidea:::GetIdeaF:::open-ul-12", "IdeaManager:::collapseidea:::TrendingReport:::open-ul-12", "IdeaManager:::collapseidea:::IdeaApproval:::onetwothree",
 "IdeaManager:::collapseidea:::IdeaProgress:::open-ul-13", 

"VerticalSubVerticalsGrid:::collapseFive", "LOBandApplications:::collapseFive",
"AlignmentManager:::collapseEight",


/*"IdeaManager:::collapseCM:::DisplayIdeas:::open-ul-14",*/ "ClientProfileManager:::collapseTwo:::Survey:::open-ul-5", "IncidentNavigator:::collapseCM:::Dashboard:::open-ul-14",



 "ClientProfileManager:::collapseUM:::SetSDFSDFSDQues:::open-ul-17", "ClientProfileManager:::collapseTwo:::ClientTicketInfo:::onetwothree", "ClientProfileManager:::collapseUM:::NextGenUserReport:::open-ul-17", "ClientProfileManager:::collapseUM:::AllClientsForUser:::open-ul-17",

"IdeaManager:::collapseCM:::DisplayClassAndCategory:::open-ul-20", "IdeaManager:::collapseCM:::DisplayIdeaTypes:::open-ul-20", "IdeaManager:::collapseCM:::DisplayIdeaSources:::open-ul-20", "IdeaManager:::collapseCM:::DisplayLOB:::open-ul-20", "IdeaManager:::collapseCM:::Config:::open-ul-20", "IdeaManager:::collapseCM:::Index1:::open-ul-20",
"SLAManager:::collapseCM:::MetricLibraryView:::open-ul-21", "ServiceCalendar:::collapseCM:::GenerateServiceCalendar:::open-ul-21",
"ClientProfileManager:::collapseCM:::PerspectiveConfiguration:::open-ul-22",
"IncidentNavigator:::collapseCM:::INIncidentServicehours:::open-ul-23",
"ClientProfileManager:::collapseCM:::Configuration:::onetwothree",

"ClientProfileManager:::collapseCM:::DataMapping:::open-ul-15", "clientprofilemanager:::collapseCM:::AddNewTemplate:::open-ul-15", "ClientProfileManager:::collapseCM:::PIDataMask:::open-ul-15", "ClientProfileManager:::collapseCM:::DataFileUpload:::open-ul-15", "ClientProfileManager:::collapseCM:::JobScheduling:::open-ul-15",
"Account:::collapseCM:::Register:::open-ul-16", "Account:::collapseCM:::Users:::open-ul-16", "Account:::collapseCM:::UserProfileEdit:::open-ul-16", "Account:::collapseCM:::AssignUser:::open-ul-16", "ClientProfileManager:::collapseCM:::NextGenUserReport:::open-ul-16",

"PortfolioAnalyzer:::collapseUM:::ExAppIndex:::open-ul-17", "ClientProfileManager:::collapseUM:::ExportTicketInfo:::open-ul-17", "ClientProfileManager:::collapseUM:::DataUpload:::open-ul-17",

    ];

    for (var k = 0; k < appNamesInAccordion.length; k++) {
        var s = appNamesInAccordion[k].split(':::');
       // console.log(s);
        if (($.inArray(s[0], array_url) > -1) && ($.inArray(s[2], array_url) > -1)) {
           // console.log(s);
            //console.log('i am checking if condition');
            $('.appNavOpen').trigger('click');
            $('#'+s[1]).addClass('in').removeClass('collapse');
            // in order to open child sub menu...
            $("." + s[3]).css("display", "block");

            if (s[3] == 'open-ul-20' || s[3] == 'open-ul-21' || s[3] == 'open-ul-22' || s[3] == 'open-ul-23') {
                $("." + s[3]).parents('.ideamanager-extralevel').css("display", "block");
            }

            //return false;

        } /*else if ($.inArray(s[0], array_url) > -1) {
            $('.appNavOpen').trigger('click');
            $('#' + s[1]).addClass('in').removeClass('collapse');
        }*/
    }
       
});


// This is the blue dwarf
// more information at https://github.com/picrap/BlueDwarf

namespace BlueDwarf.Test
{
    using System.Linq;
    using System.Net;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Net.Proxy;
    using Net.Proxy.Scanner;

    [TestClass]
    public class ProxyProviderTest
    {
        #region private const string ProxynovaCountryES = ...
        private const string ProxynovaCountryES = @"


<!DOCTYPE HTML>
<html lang=""en"" dir=""ltr"" class=""no-js"">
<head>

<title>Proxy Server List - Spain Proxy - Spanish Proxies</title>

<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
<meta name=""msvalidate.01"" content=""F9BD470E5BD97F178C18C062EAF0853E"" />
<meta name=""google-site-verification"" content=""bMnUySWmhilzGnWxuBYuoKAB67o80BjSsHoM-BSoiHk"" />
<meta property=""fb:admins"" content=""1219819907"" />

<!--[if lte IE 8]>
<script src=""http://html5shiv.googlecode.com/svn/trunk/html5.js""></script>
<![endif]-->

<link rel=""shortcut icon"" href=""//www.proxynova.com/favicon.ico"">

<link rel=""stylesheet"" type=""text/css"" href=""/assets/css/master.css"">
<link rel=""stylesheet"" type=""text/css"" href=""/assets/css/nav_menu.css"">
<link rel=""stylesheet"" type=""text/css"" href=""/assets/css/country_flags.css"">
<link rel=""stylesheet"" type=""text/css"" href=""/assets/css/proxynova.css"">

<meta name=""keywords"" content=""proxy list, proxy servers 2015, proxy server list, open proxies, proxy servers, proxy servers by country, working proxy servers, proxy list 2014, fresh proxy list"" />

<script type=""text/javascript"" src=""/assets/js/proxy_list.js""></script>

<link rel=""stylesheet"" href=""http://www.proxynova.com/assets/css/proxy_list.css"">

<style>

.proxy-city {
	color:#666666;
	font-size:10px;
}

</style>



<script type=""text/javascript"" src=""//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js""></script>


</head>
<body>

<div id=""top_header""></div>

<center>
  <div id=""wrapper"">

		<div id=""social_tab"">
		  <center>

				<!-- AddThis Button BEGIN -->
				<div class=""addthis_toolbox addthis_counter_style"">
				<a class=""addthis_button_facebook_like"" fb:like:layout=""box_count""></a>
				<br>
				<br>
				<a class=""addthis_button_google_plusone"" g:plusone:size=""tall""></a>
				<br>
				<br>
				<a class=""addthis_button_tweet"" tw:count=""vertical""></a>
				</div>
				<!-- AddThis Button END -->

		  </center>
		</div>

    
    <div id=""header"" style=""margin:10px 0px;"">
    
    
		<center>
		  <div style=""width:730px; height:90px;"">
		  
				  <div id=""adz_top"">
							<script type=""text/javascript""><!--
							google_ad_client = ""ca-pub-4660819739366379"";
							/* proxy_nova_top */
							google_ad_slot = ""4582654399"";
							google_ad_width = 728;
							google_ad_height = 90;
							//-->
							</script>
							<script type=""text/javascript"" src=""//pagead2.googlesyndication.com/pagead/show_ads.js""></script>
					</div>
							
			</div>
		  
		</center>
      
    </div>
  
    
    <div id=""main_container"">
      <div style=""background-color:#4f85bb;"">
        <ul class=""navbar"">
		
          <li><a href=""//www.proxynova.com/"" title=""Proxy List"" class=""icon-home"">Home</a></li>
          
          <li class=""has-sub"">
		  
		  <a href=""//www.proxynova.com/proxy-server-list/"" title=""Free Proxy Server List"">Proxy Server List</a>
		  
            <div class=""dropdown"" style=""width:580px;"">
	
			<div style=""text-align:left;"">
				<h2>Proxy Servers by Country</h2>
			</div>
			
			  
			  <div class=""col3 clearfix""><ul><li><img src=""blank.gif"" class=""flag flag-cn"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-cn/"" title=""Chinese Proxy List""> China (36,110)</a></li><li><img src=""blank.gif"" class=""flag flag-ve"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-ve/"" title=""Venezuelan Proxy List""> Venezuela (11,850)</a></li><li><img src=""blank.gif"" class=""flag flag-tw"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-tw/"" title=""Taiwan (or Taiwanese) Proxy List""> Taiwan (4,491)</a></li><li><img src=""blank.gif"" class=""flag flag-in"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-in/"" title=""Indian Proxy List""> India (2,826)</a></li><li><img src=""blank.gif"" class=""flag flag-br"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-br/"" title=""Brazilian Proxy List""> Brazil (1,814)</a></li><li><img src=""blank.gif"" class=""flag flag-id"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-id/"" title=""Indonesian Proxy List""> Indonesia (1,616)</a></li><li><img src=""blank.gif"" class=""flag flag-us"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-us/"" title=""American Proxy List""> United States (894)</a></li><li><img src=""blank.gif"" class=""flag flag-th"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-th/"" title=""Thai Proxy List""> Thailand (786)</a></li><li><img src=""blank.gif"" class=""flag flag-ru"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-ru/"" title=""Russian Proxy List""> Russia (504)</a></li><li><img src=""blank.gif"" class=""flag flag-ar"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-ar/"" title=""Argentine Proxy List""> Argentina (390)</a></li></ul></div><div class=""col3 clearfix""><ul><li><img src=""blank.gif"" class=""flag flag-hk"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-hk/"" title=""Chinese/Hong Kong Proxy List""> Hong Kong (357)</a></li><li><img src=""blank.gif"" class=""flag flag-de"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-de/"" title=""German Proxy List""> Germany (316)</a></li><li><img src=""blank.gif"" class=""flag flag-eg"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-eg/"" title=""Egyptian Proxy List""> Egypt (293)</a></li><li><img src=""blank.gif"" class=""flag flag-jp"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-jp/"" title=""Japanese Proxy List""> Japan (286)</a></li><li><img src=""blank.gif"" class=""flag flag-cl"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-cl/"" title=""Chilean Proxy List""> Chile (227)</a></li><li><img src=""blank.gif"" class=""flag flag-ma"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-ma/"" title=""Moroccan Proxy List""> Morocco (218)</a></li><li><img src=""blank.gif"" class=""flag flag-vn"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-vn/"" title=""Vietnamese Proxy List""> Vietnam (195)</a></li><li><img src=""blank.gif"" class=""flag flag-co"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-co/"" title=""Colombian Proxy List""> Colombia (171)</a></li><li><img src=""blank.gif"" class=""flag flag-bd"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-bd/"" title=""Bangladeshi Proxy List""> Bangladesh (135)</a></li><li><img src=""blank.gif"" class=""flag flag-ua"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-ua/"" title=""Ukrainian Proxy List""> Ukraine (128)</a></li></ul></div><div class=""col3 clearfix""><ul><li><img src=""blank.gif"" class=""flag flag-gb"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-gb/"" title=""British Proxy List""> United Kingdom (116)</a></li><li><img src=""blank.gif"" class=""flag flag-pe"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-pe/"" title=""Peruvian Proxy List""> Peru (109)</a></li><li><img src=""blank.gif"" class=""flag flag-pl"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-pl/"" title=""Polish Proxy List""> Poland (101)</a></li><li><img src=""blank.gif"" class=""flag flag-my"" border=""0""><a href=""http://www.proxynova.com/proxy-server-list/country-my/"" title=""Malaysian Proxy List""> Malaysia (100)</a></li></ul></div>			  
			  <!--- the end --->
            </div>
			
          </li>
          
			<li><a href=""//www.proxynova.com/web-proxy-list/"" title=""Proxy List"">Web Proxy List</a></li>
			
			<li><a href=""//www.proxynova.com/online-proxy-checker/"" title=""Check Proxy Online"">Online Proxy Checker</a></li>
			  
			<li><a href=""//www.proxynova.com/proxy-articles/"" title=""Information about Proxy Servers"">Proxy Articles</a></li>
			
			<li><a href=""//www.proxynova.com/proxy-software/"">Proxy Software</a></li>

        </ul>
      </div>
	  
      <div id=""page_contents"">
	  

	  
<!-- content starts -->


<header>
	<h1>Spanish Proxy List - Proxies from Spain</h1>
</header>


<p>
<strong>Proxy Server List</strong> - this page provides and maintains the largest and
 the most up-to-date list of working proxy servers that are available for public use.
 Our powerful software works all day checking over a million proxies daily with most proxy servers
tested at least once every 15 minutes, thus creating one of the most reliable proxy lists on the Internet - all for free.

 </p>
 
 <p>Any <strong>proxy server</strong> listed on this page can be used with a
 software application that supports the use of proxies such as
  your web browser. The uses of proxy include hiding your real IP address,
  disguising your location, and accessing blocked websites.
  </p>
 
 <p>
This <strong>proxy list</strong> is updated once every 60 seconds from the data stored in our gigabyte-sized proxy database.
The list can be filtered by a number of attributes such as the port number of a proxy,
country of origin of a proxy, and the level of anonymity of a proxy.

</p>

<div style=""height:25px;""></div>

<center>

	<div style=""text-align:center;"">

		<select name=""proxy_country"" id=""proxy_country"" style=""width:250px;"">
			<option selected=""selected"" value="""">&lt; All Countries &gt;</option>
			<option disabled=""disabled"">---</option>
	  
					
		</select>

		&nbsp;

		<select name=""lst_proxy_level"" id=""lst_proxy_level"">
			<option value=""all"" selected=""selected"">&lt; All Proxies &gt;</option>
			<option disabled=""disabled"">---</option>
			<option value=""elite"">Elite/High Anonymous Proxies</option>
			<option value=""anonymous"">Anonymous Proxies</option>
		</select>

	</div>

<br>
<br>

	<div>
		<center>
	  
			<div class=""728x15"">
		  
			<script async src=""//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js""></script>
			<!-- proxy_nova_list -->
			<ins class=""adsbygoogle""
				 style=""display:inline-block;width:728px;height:15px""
				 data-ad-client=""ca-pub-4660819739366379""
				 data-ad-slot=""5607768202""></ins>
			<script>
			(adsbygoogle = window.adsbygoogle || []).push({});
			</script>

			</div>
			
		</center>
	</div>

  <br />
  <div id=""bla"" style=""width:950px;"">
    <table width=""950"" class=""table"" id=""tbl_proxy_list"">
      <thead>
        <tr>
          <th>Proxy IP</th>
          <th>Proxy Port</th>
          <th>Last Check</th>
          <th nowrap=""nowrap""><span title=""Proxy Speed in bytes per second"">Proxy Speed</span></th>
		  <th>Uptime</th>
          <th><span title=""The location of that particular proxy."">Proxy Country</span></th>
          <th>Anonymity </th>
        </tr>
      </thead>
      <tbody>

                <tr>
          <td align=""left"">
		  
		  <span class=""row_proxy_ip"">62.82.58.99</span></td>
		 
          <td align=""left"">
		  

		  		  

		  <a href=""http://www.proxynova.com/proxy-server-list/port-8080"">8080</a> 
		  
		  
		  
		  		  

		  
		  </td>
			
          <td align=""left"">
				<time class=""icon icon-check"">2 mins</time>
		  </td>
		  
          <td align=""left"">
			<div class=""progress-bar"" data-value=""84 "" title=""7593.2394""></div>
		  </td>
		  
		  <td style=""text-align:center !important;"">

		  			
			<span style=""color:#009900;"">61%</span>
			
		  			
		  </td>

          <td align=""left"">
		  
		  
		  
		  <img src=""//www.proxynova.com/assets/images/blank.gif"" class=""flag flag-es"" width=""15"" height=""11"" alt=""ES"" />

		  
		  
		  
		  <a href=""/proxy-server-list/country-es/"">Spain		  
		  
				
			</a>


		  
			</td>
		   
          <td align=""left"">
		  
			<span class=""proxy_transparent"" style=""font-weight:bold; font-size:10px;"">Transparent</span>
		  
		  </td>
		  
        </tr>
		
		
                <tr>
          <td align=""left"">
		  
		  <span class=""row_proxy_ip"">84.236.134.2</span></td>
		 
          <td align=""left"">
		  

		  		  

		  <a href=""http://www.proxynova.com/proxy-server-list/port-8080"">8080</a> 
		  
		  
		  
		  		  

		  
		  </td>
			
          <td align=""left"">
				<time class=""icon icon-check"">5 mins</time>
		  </td>
		  
          <td align=""left"">
			<div class=""progress-bar"" data-value=""73 "" title=""44919.1343""></div>
		  </td>
		  
		  <td style=""text-align:center !important;"">

		  			
			<span style=""color:#009900;"">63%</span>
			
		  			
		  </td>

          <td align=""left"">
		  
		  
		  
		  <img src=""//www.proxynova.com/assets/images/blank.gif"" class=""flag flag-es"" width=""15"" height=""11"" alt=""ES"" />

		  
		  
		  
		  <a href=""/proxy-server-list/country-es/"">Spain		  
		  
									<span class=""proxy-city""> - Murcia </span> 
				
			</a>


		  
			</td>
		   
          <td align=""left"">
		  
			<span class=""proxy_transparent"" style=""font-weight:bold; font-size:10px;"">Transparent</span>
		  
		  </td>
		  
        </tr>
		
		
                <tr>
          <td align=""left"">
		  
		  <span class=""row_proxy_ip"">212.231.208.224</span></td>
		 
          <td align=""left"">
		  

		  		  

		  <a href=""http://www.proxynova.com/proxy-server-list/port-80"">80</a> 
		  
		  
		  
		  		  

		  
		  </td>
			
          <td align=""left"">
				<time class=""icon icon-check"">18 mins</time>
		  </td>
		  
          <td align=""left"">
			<div class=""progress-bar"" data-value=""75 "" title=""5846.0000""></div>
		  </td>
		  
		  <td style=""text-align:center !important;"">

		  			
			<span style=""color:#009900;"">57%</span>
			
		  			
		  </td>

          <td align=""left"">
		  
		  
		  
		  <img src=""//www.proxynova.com/assets/images/blank.gif"" class=""flag flag-es"" width=""15"" height=""11"" alt=""ES"" />

		  
		  
		  
		  <a href=""/proxy-server-list/country-es/"">Spain		  
		  
				
			</a>


		  
			</td>
		   
          <td align=""left"">
		  
			<span class=""proxy_transparent"" style=""font-weight:bold; font-size:10px;"">Transparent</span>
		  
		  </td>
		  
        </tr>
		
		
                <tr>
          <td align=""left"">
		  
		  <span class=""row_proxy_ip"">88.3.200.223</span></td>
		 
          <td align=""left"">
		  

		  		  

		  <a href=""http://www.proxynova.com/proxy-server-list/port-8080"">8080</a> 
		  
		  
		  
		  		  

		  
		  </td>
			
          <td align=""left"">
				<time class=""icon icon-check"">29 mins</time>
		  </td>
		  
          <td align=""left"">
			<div class=""progress-bar"" data-value=""41 "" title=""1026.8364""></div>
		  </td>
		  
		  <td style=""text-align:center !important;"">

		  			
			<span style=""color:#009900;"">50%</span>
			
		  			
		  </td>

          <td align=""left"">
		  
		  
		  
		  <img src=""//www.proxynova.com/assets/images/blank.gif"" class=""flag flag-es"" width=""15"" height=""11"" alt=""ES"" />

		  
		  
		  
		  <a href=""/proxy-server-list/country-es/"">Spain		  
		  
									<span class=""proxy-city""> - Madrid </span> 
				
			</a>


		  
			</td>
		   
          <td align=""left"">
		  
			<span class=""proxy_transparent"" style=""font-weight:bold; font-size:10px;"">Transparent</span>
		  
		  </td>
		  
        </tr>
		
		
                <tr>
          <td align=""left"">
		  
		  <span class=""row_proxy_ip"">46.24.18.4</span></td>
		 
          <td align=""left"">
		  

		  		  

		  <a href=""http://www.proxynova.com/proxy-server-list/port-8080"">8080</a> 
		  
		  
		  
		  		  

		  
		  </td>
			
          <td align=""left"">
				<time class=""icon icon-check"">2 hrs</time>
		  </td>
		  
          <td align=""left"">
			<div class=""progress-bar"" data-value=""63 "" title=""674016.2414""></div>
		  </td>
		  
		  <td style=""text-align:center !important;"">

		  			
			<span style=""color:#CC0000;"">26%</span>
			
		  			
		  </td>

          <td align=""left"">
		  
		  
		  
		  <img src=""//www.proxynova.com/assets/images/blank.gif"" class=""flag flag-es"" width=""15"" height=""11"" alt=""ES"" />

		  
		  
		  
		  <a href=""/proxy-server-list/country-es/"">Spain		  
		  
				
			</a>


		  
			</td>
		   
          <td align=""left"">
		  
			<span class=""proxy_transparent"" style=""font-weight:bold; font-size:10px;"">Transparent</span>
		  
		  </td>
		  
        </tr>
		
		
                <tr>
          <td align=""left"">
		  
		  <span class=""row_proxy_ip"">91.223.32.9</span></td>
		 
          <td align=""left"">
		  

		  		  

		  <a href=""http://www.proxynova.com/proxy-server-list/port-8080"">8080</a> 
		  
		  
		  
		  		  

		  
		  </td>
			
          <td align=""left"">
				<time class=""icon icon-check"">4 hrs</time>
		  </td>
		  
          <td align=""left"">
			<div class=""progress-bar"" data-value=""73 "" title=""24861.7500""></div>
		  </td>
		  
		  <td style=""text-align:center !important;"">

		  			
			<span style=""color:#009900;"">59%</span>
			
		  			
		  </td>

          <td align=""left"">
		  
		  
		  
		  <img src=""//www.proxynova.com/assets/images/blank.gif"" class=""flag flag-es"" width=""15"" height=""11"" alt=""ES"" />

		  
		  
		  
		  <a href=""/proxy-server-list/country-es/"">Spain		  
		  
				
			</a>


		  
			</td>
		   
          <td align=""left"">
		  
			<span class=""proxy_transparent"" style=""font-weight:bold; font-size:10px;"">Transparent</span>
		  
		  </td>
		  
        </tr>
		
		
              </tbody>
      <tr>
        <th colspan=""10"">&nbsp;</th>
      </tr>
    </table>
	
    </center>
    
	<!--- ads on the bottom --->
	
    <center>
		<div class=""728x15"">
		
			<script async src=""//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js""></script>
			<!-- proxy_nova_list_bottom -->
			<ins class=""adsbygoogle""
				 style=""display:inline-block;width:728px;height:15px""
				 data-ad-client=""ca-pub-4660819739366379""
				 data-ad-slot=""7095001220""></ins>
			<script>
			(adsbygoogle = window.adsbygoogle || []).push({});
			</script>

		</div>
    </center>
	
	<!--- ads end --->
	
    <br />
	

	<h3>Proxy Anonymity Levels Explained</h3>
	
	<p>
	
	<ul>
	
	<li>
	<span class=""proxy_transparent""><strong>Transparent</strong></span> - target server knows your IP address and it knows that you're using a proxy.
	</li>
	
	<li>
	<span class=""proxy_anonymous""><strong>Anonymous</strong></span> - target server does not know your IP address, but it knows that you're using a proxy.
	</li>
	
	<li>
	<span class=""proxy_elite""><strong>Elite</strong></span> - target server does not know your IP address, nor does it have any clue
	that the request is coming from a proxy.
	</li>
	
	</ul>
	
	</p>
	
	<p>
	
	A more detailed explanation about the specifics can be found on our 
	<a href=""http://www.proxynova.com/proxy-articles/proxy-anonymity-levels-explained/"">proxy levels explained</a>
	article.
	
	</p>
	
	<h3>How to use Proxy Servers?</h3>
	
<p>
Almost any software can be configured to use a proxy including your web-browser which is how most people use proxies.
Configuring your browser to use a proxy server is quick and easy, and the instructions for doing that are
described for each browser in our <a href=""http://www.proxynova.com/proxy-articles/"">articles</a> section.
</p>


<p>

However, the recommended alternative to doing all those steps anytime you wish to use a different proxy, is to use our homemade <strong>proxy switcher</strong>
that can simplify the whole process to just a few mouse clicks. Our Nova Proxy Switcher can be downloaded from our 
<a href=""http://www.proxynova.com/proxy-software/"">software</a> page.


</p>

<center>
	<a href=""http://www.proxynova.com/proxy-software/"">
		<img src=""//www.proxynova.com/assets/images/software/proxy_switcher.png"" width=""180"">
	</a>
</center>
	
	
	
<!-- content ends -->
		
	
      </div>
	  
    </div>
    <div id=""footer"">
	
      <div style=""float:left;"">
	  
		<a href=""http://www.proxynova.com"" title=""Proxy Server List"">&copy; 2011-2014 ProxyNova.com</a>
		
	  </div>

	 
      <div style=""float:right;""> 
	  
		  <ul class=""list-inline"">
			  <li>
			  <a href=""https://unblockvideos.com/"" title=""Unblock Video Websites such as YouTube and DailyMotion"">Unblock YouTube using a Web Proxy</a>
			  </li>
			  
			  <li>
			  <a href=""//www.scape-xp.com"" title=""Runescape calculators and other Runescape tools"" target=""_blank"">Runescape Calculators</a> 
			  </li>
		  </ul>
	  
	  </div>
      
      <div class=""clear""></div>
      
      <div style=""position:absolute; left:-999px;"">
	 <a href=""//athlon1600.top20free.com/"" target=""_blank""><img src=""//www.proxynova.com/assets/images/top20_free.gif"" /></a>
      </div>
      
      
    </div>
  </div>

</center>












<script type=""text/javascript"">

  var _gaq = _gaq || [];
  _gaq.push(['_setAccount', 'UA-3776441-4']);
  _gaq.push(['_setDomainName', 'proxynova.com']);
  _gaq.push(['_trackPageview']);

  (function() {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
  })();

</script>


<script type=""text/javascript"">
var addthis_share = {
    url: ""http://www.proxynova.com/""
}
</script>

<script type=""text/javascript"" src=""//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-50cf8cfd69116d5a""></script>

<script type=""text/javascript"">
// adsense and analytics
window.google_analytics_uacct = ""UA-3776441-4"";

$(document).ready(function(){

	$("".progress-bar"").progressbar();
	
	$(""html"").removeClass(""no-js"");
});

</script>


<script type=""text/javascript"" src=""http://www.proxynova.com/assets/js/jquery.progressbar.js""></script>
<script type=""text/javascript"" src=""http://www.proxynova.com/assets/js/jquery.cookie.js""></script>
<script type=""text/javascript"" src=""http://www.proxynova.com/assets/js/queue.js""></script>
<script type=""text/javascript"" src=""http://www.proxynova.com/assets/js/nav_menu.js""></script>

<script type=""text/javascript"" src=""//www.php-proxy.com/feedback/feedback.js""></script>

</body>
</html>";
        #endregion

        #region private const string XroxyRSS = ...
        private const string XroxyRSS = @"
<item>
<title>Transparent only (Jan 27, 09:33 GMT)</title>
<link>http://www.xroxy.com/proxy-type-Transparent.htm</link>
<guid>http://www.xroxy.com/proxy-type-Transparent.htm</guid>
<description>Proxylists contain 847 recently checked proxies</description>
<prx:proxy>
<prx:ip>1.179.147.2</prx:ip>
<prx:port>8080</prx:port>
<prx:type>Transparent</prx:type>
<prx:ssl>false</prx:ssl>
<prx:check_timestamp>1422350188</prx:check_timestamp>
<prx:country_code>TH</prx:country_code>
<prx:latency>5242</prx:latency>
<prx:reliability>5001</prx:reliability>
</prx:proxy>
<prx:proxy>
<prx:ip>1.234.23.22</prx:ip>
<prx:port>8088</prx:port>
<prx:type>Transparent</prx:type>
<prx:ssl>true</prx:ssl>
<prx:check_timestamp>1422364267</prx:check_timestamp>
<prx:country_code>KR</prx:country_code>
<prx:latency>36872</prx:latency>
<prx:reliability>4830</prx:reliability>
</prx:proxy>
<prx:proxy>
<prx:ip>101.1.16.123</prx:ip>
<prx:port>3128</prx:port>
<prx:type>Transparent</prx:type>
<prx:ssl>true</prx:ssl>
<prx:check_timestamp>1422355147</prx:check_timestamp>
<prx:country_code>HK</prx:country_code>
<prx:latency>1355</prx:latency>
<prx:reliability>9549</prx:reliability>
</prx:proxy>
<prx:proxy>
<prx:ip>101.255.28.38</prx:ip>
<prx:port>8080</prx:port>
<prx:type>Transparent</prx:type>
<prx:ssl>true</prx:ssl>
<prx:check_timestamp>1422357729</prx:check_timestamp>
<prx:country_code>ID</prx:country_code>
<prx:latency>1284</prx:latency>
<prx:reliability>7563</prx:reliability>
</prx:proxy>
<prx:proxy>
<prx:ip>101.255.89.70</prx:ip>
<prx:port>8080</prx:port>
<prx:type>Transparent</prx:type>
<prx:ssl>true</prx:ssl>
<prx:check_timestamp>1422363206</prx:check_timestamp>
<prx:country_code>ID</prx:country_code>
<prx:latency>18734</prx:latency>
<prx:reliability>2942</prx:reliability>
</prx:proxy>
";
        #endregion

        [TestMethod]
        public void ProxynovaTest()
        {
            var hostPorts = HostScanner.CreateHostEndPoints(ProxynovaCountryES, ProxyPage.ProxynovaEx).ToArray();
            Assert.AreEqual(6, hostPorts.Length);
            Assert.IsTrue(hostPorts.Contains(new ProxyServer(ProxyProtocol.HttpConnect, IPAddress.Parse("62.82.58.99"), 8080)));
            Assert.IsTrue(hostPorts.Contains(new ProxyServer(ProxyProtocol.HttpConnect, IPAddress.Parse("84.236.134.2"), 8080)));
            Assert.IsTrue(hostPorts.Contains(new ProxyServer(ProxyProtocol.HttpConnect, IPAddress.Parse("212.231.208.224"), 80)));
            Assert.IsTrue(hostPorts.Contains(new ProxyServer(ProxyProtocol.HttpConnect, IPAddress.Parse("88.3.200.223"), 8080)));
            Assert.IsTrue(hostPorts.Contains(new ProxyServer(ProxyProtocol.HttpConnect, IPAddress.Parse("46.24.18.4"), 8080)));
            Assert.IsTrue(hostPorts.Contains(new ProxyServer(ProxyProtocol.HttpConnect, IPAddress.Parse("91.223.32.9"), 8080)));
        }

        [TestMethod]
        public void XroxyTest()
        {
            var hostPorts = HostScanner.CreateHostEndPoints(XroxyRSS, ProxyPage.XroxyRssEx).ToArray();
            Assert.AreEqual(4, hostPorts.Length);
            Assert.IsTrue(hostPorts.Contains(new ProxyServer(ProxyProtocol.HttpConnect, IPAddress.Parse("1.234.23.22"), 8088)));
            Assert.IsTrue(hostPorts.Contains(new ProxyServer(ProxyProtocol.HttpConnect, IPAddress.Parse("101.1.16.123"), 3128)));
            Assert.IsTrue(hostPorts.Contains(new ProxyServer(ProxyProtocol.HttpConnect, IPAddress.Parse("101.255.28.38"), 8080)));
            Assert.IsTrue(hostPorts.Contains(new ProxyServer(ProxyProtocol.HttpConnect, IPAddress.Parse("101.255.89.70"), 8080)));
        }
    }
}

if (!String.prototype.startsWith) {
  String.prototype.startsWith = function(searchString, position){
    position = position || 0;
    return this.substr(position, searchString.length) === searchString;
  };
}

var UrlBuilder = (function () {
  function UrlBuilder(schema, host) {
    this.schema = schema;
    this.host = host;
  }

  UrlBuilder.prototype.build = function (path) {
      return this.schema + "://" + this.host + "/" + path;
  };

  return UrlBuilder;
})();

function getPageTitle(browser) {
  try {
    return browser.Document.getElementsByTagName('title')[0].innerHTML;
  } catch(e) {}
  return '';
}

function ie_DocumentComplete(browser, url) {
  var ub = new UrlBuilder("https", WScript.Arguments.Named("hostname"));

  //WScript.Echo("DocumentComplete: " + url + ' ' + getPageTitle(browser));
  if (url === ub.build('')) {
  } else if (url.startsWith(ub.build('JMODE_ASP/faces/index.jsp?time='))) {
    onLogin(browser);
  } else if (url === ub.build('JMODE_ASP/faces/index.jsp')) {
    switch(getPageTitle(browser)) {
      case 'メインメニュー': return onMainMenu(browser);
    }
  } else if(url === ub.build('JMODE_ASP/faces/contents/index.jsp')) {
    onSerchDist(browser);
  } else if(url === ub.build('JMODE_ASP/faces/contents/X045_160_CPOP_CATALOG/X045_SELECT.jsp')) {
    browser.Document.Script.setTimeout(function() {
      //if(!browser.Document.querySelector('table.body_table tr[index="1"]')) {
      if(browser.Document.getElementById('SlipList').innerHTML.length > 0) {
        browser.Document.Script.setTimeout(arguments.callee, 100);
      }
      browser.Document.getElementById('search_button').click();
    }, 100);
  } else if (url === ub.build('JMODE_ASP/faces/contents/X045_160_CPOP_CATALOG/X045_LIST.jsp')) {
    WScript.Quit();
  }
}

function onLogin(browser) {
  var CLIENT = WScript.Arguments.Named("access-key-id"),
      PERSON = WScript.Arguments.Named("user-name"),
      CLPASS = WScript.Arguments.Named("secret-access-key"),
      PSPASS = WScript.Arguments.Named("password"),
      id_client = "form1:username",
      id_person = "form1:person_code",
      id_clpass = "form1:password",
      id_pspass = "form1:person_password";

  if(!$(id_client)) {
    id_client = "form1:client";
    id_person = "form1:person";
    id_clpass = "form1:clpass";
    id_pspass = "form1:pspass";
  }
  $(id_client).value = CLIENT;
  $(id_person).value = PERSON;
  $(id_clpass).value = CLPASS;
  $(id_pspass).value = PSPASS;
  $("form1:login").click();

  function $(id) {
    return browser.Document.getElementById(id);
  }
}

function getBytes(text) {
  var bytes,
      stream = WScript.CreateObject("ADODB.Stream");
  stream.Open();
  stream.Charset = "UTF-8";
  stream.WriteText(text);
  stream.Position = 0;
  stream.Type = 1; // binary
  stream.Position = 3; // skip BOM
  bytes = stream.Read();
  stream.Close();
  return bytes;
}

function onMainMenu(browser) {
  var ub = new UrlBuilder("https", WScript.Arguments.Named("hostname")),
      headers = 'Content-Type: application/x-www-form-urlencoded\r\n',
      data = 'form1%3Aredirect=%E5%85%A5%E5%8A%9B&form1%3Aredirectpage=X045_SELECT.jsp&form1%3Aredirectfolder=X045_160_CPOP_CATALOG&form1%3Areturnvalue=SUCCESS&form1%3Afunctype=10&form1%3AselectMenu=013&form1%3AselectSubMenu=035&form1%3AselectFunction=X045&form1%3Afop=&form1=form1';
  browser.Navigate(ub.build('JMODE_ASP/faces/contents/index.jsp'),
      '_self', null, getBytes(data), headers);
}

function onSerchDist(browser) {
  var ub = new UrlBuilder("https", WScript.Arguments.Named("hostname")),
      headers = 'Content-Type: application/x-www-form-urlencoded\r\n',
      today = new Date(),
      year = WScript.Arguments.Named("year") || today.getFullYear(),
      month = WScript.Arguments.Named("month") || today.getMonth() + 1,
      day = WScript.Arguments.Named("day") || today.getDate(),
      code = WScript.Arguments.Named("code") || '20151105_230',
      data = 'form1%3Aexecute=execute&form1%3Aaction=search&form1%3AisAjaxMode=&invest_from=&invest_to=&shop_arri_date_from=&shop_arri_date_to=&dest_cd1%3Adest=001&dest_cd1%3AdestName=&dest_cd1%3Acust=&dest_cd1%3Aarea=&dest_cd1%3AdestClass=&dest_cd1%3AdestType=&dest_cd1%3AdestGroup=&sup_cd=&category_gp_cd=&style_cd=&style_nm=&barcode=&barcode2=&regist_date_from=&regist_date_to=&update_date_from=&update_date_to=&category_cd=&season_cd=&brand_cd=&code_from='+code+'&code_to='+code+'&create_date_from=&create_date_to=&form1%3Asearch_type=1&tax_date='+year+'%E5%B9%B4'+month+'%E6%9C%88'+day+'%E6%97%A5&form1=form1'

  browser.Navigate(ub.build('JMODE_ASP/faces/contents/X045_160_CPOP_CATALOG/X045_SELECT.jsp'),
      '_self', null, getBytes(data), headers)
  //browser.Visible = true;
}

// IE起動
var ie = WScript.CreateObject("InternetExplorer.Application", "ie_");
ie.Visible = true
ie.Navigate((new UrlBuilder("https", WScript.Arguments.Named("hostname"))).build(""));
while (true) {
    WScript.Sleep(100);
}
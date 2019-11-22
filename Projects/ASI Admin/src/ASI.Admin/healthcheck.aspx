<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
<%
    Response.Write("<h3>Checking database connection strings</h3>");
    asi.asicentral.model.User user = null;
    try
    { //umbracoDbDSN, ProductContext
        var config = System.Configuration.ConfigurationManager.ConnectionStrings["umbracoDbDSN"];
        Response.Write("umbracoDbDSN Config: " + config + " <br />");

        config = System.Configuration.ConfigurationManager.ConnectionStrings["ProductContext"];
        Response.Write("ProductContext Config: " + config + " <br />");

        config = System.Configuration.ConfigurationManager.ConnectionStrings["CallContext"];
        Response.Write("CallContext Config: " + config + " <br />");  
       
        config = System.Configuration.ConfigurationManager.ConnectionStrings["VelocityContext"];
        Response.Write("VelocityContext Config: " + config + " <br />");       
        config = System.Configuration.ConfigurationManager.ConnectionStrings["Umbraco_ShowContext"];
        Response.Write("Umbraco_ShowContext Config: " + config + " <br />");       
        config = System.Configuration.ConfigurationManager.ConnectionStrings["InternetContext"];
        Response.Write("InternetContext Config: " + config + " <br />");       
        config = System.Configuration.ConfigurationManager.ConnectionStrings["ASIInternetContext"];
        Response.Write("ASIInternetContext Config: " + config + " <br />");       
    }
    catch (Exception ex)
    {
        Response.Write("Failed: " + ex.Message);
    }
%>
<%
    Response.Write("<h3>Checking MediaPath</h3>");
    var keyPath = System.Configuration.ConfigurationManager.AppSettings["MediaPath"];
    Response.Write("Config: " + keyPath + " <br />");
    try {
        string[] filePaths = System.IO.Directory.GetFiles(keyPath);
        Response.Write("Values: " + filePaths.Length + " <br />");
    }
    catch (Exception ex) {
        Response.Write("Failed: " + ex.Message);
    }
%>
<%
    Response.Write("<h3>Checking Key values</h3>");
    var keyStr = System.Configuration.ConfigurationManager.AppSettings["EsbConnectionString"];
    Response.Write("EsbConnectionString: " + keyStr + " <br />");
    
%>
<%
    Response.Write("<h3>Checking Personify</h3>");
    var url = System.Configuration.ConfigurationManager.AppSettings["svcUri"];
    Response.Write("Config: " + url + " <br />");
    using (var client = new System.Net.WebClient())
    {
        try
        {
            var response = client.DownloadString(url);
            Response.Write("Success: <pre>" + response.ToString().Substring(1,100) + "</pre> <br />");
        }
        catch (Exception ex)
        {
           Response.Write("Failed: " + ex.Message);
        }
    }
%>

<%
    Response.Write("<h3>Checking Store connection</h3>");
    try
    {
        Response.Write("<h3>Repo</h3>");
        asi.asicentral.database.StoreContext storeContext = new asi.asicentral.database.StoreContext();
        var list = storeContext.Contexts.AsEnumerable<asi.asicentral.model.store.Context>();
        foreach (var item in list)
        {
            Response.Write("Item: " + item + "<br />");
            break;
        }

    }
    catch (Exception ex)
    {
        Response.Write("Failed: " + ex.Message + "<br />");
    }
%>
</body>
</html>
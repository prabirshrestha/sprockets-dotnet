﻿<%@ Page Language="C#" %>

<%
    Bundles.Reference("assets/javascripts/vendors/jquery-1.8.2.js");
    Bundles.Reference("assets/javascripts/main.js");
    Bundles.Reference("assets/templates");
     %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Title</title>
</head>
<body>
    <form id="HtmlForm" runat="server">
    </form>

    <p>View > File Source to check the generated output</p>
    <p>Try setting debug=false in web.config to see the minified version</p>

    <%= Bundles.RenderScripts("jquery") %>
    <%= Bundles.RenderHtmlTemplates() %>
    <%= Bundles.RenderScripts("app") %>
</body>
</html>

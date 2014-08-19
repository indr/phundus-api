<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="400.aspx.cs" Inherits="Phundus.Web._400" ValidateRequest="False" %>
<% Response.StatusCode = 400; %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head>
    <meta charset="utf-8" />
    <title>400 Bad Request</title>
    
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="" />

    <link rel="stylesheet" type="text/css" href="//www.phundus.ch/Content/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="//www.phundus.ch/Content/bootstrap-responsive.min.css" />
    <!--[if lt IE 7]><link rel="stylesheet" href="//www.phundus.ch/Content/bootstrap-ie6.min.css"><![endif]-->
</head>
    <body>
        <div class="container" style="margin-top:100px">
            <div class="row">
                <div class="span3"></div>
                <div class="span6">
                    <div class="alert alert-block alert-warning">
                        <h4 class="alert-heading">400 Bad Request</h4>
                        <p>The request cannot be fulfilled due to bad syntax.</p>
                        <p>Die Anfrage-Nachricht war fehlerhaft aufgebaut.</p>
                    </div>
                </div>
                <div class="span3"></div>
            </div>
        </div>
    </body>
</html>

﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="404.aspx.cs" Inherits="Phundus.Web._404"  ValidateRequest="False" %>
<% Response.StatusCode = 404; %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head>
    <meta charset="utf-8" />
    <title>404 Page Not Found</title>
    
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
                    <div class="alert alert-block alert-info">
                        <h4 class="alert-heading">404 Page Not Found</h4>
                        <p>The requested resource could not be found but may be available again in the future. Subsequent requests by the client are permissible.</p>
                        <p>Die angeforderte Ressource wurde nicht gefunden. Dieser Statuscode kann ebenfalls verwendet werden, um eine Anfrage ohne näheren Grund abzuweisen. Links, welche auf solche Fehlerseiten verweisen, werden auch als Tote Links bezeichnet.</p>
                    </div>
                </div>
                <div class="span3"></div>
            </div>
        </div>
    </body>
</html>

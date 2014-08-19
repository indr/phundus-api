﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="500.aspx.cs" Inherits="Phundus.Web._500" ValidateRequest="False" %>
<% Response.StatusCode = 500; %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head>
    <meta charset="utf-8" />
    <title>500 Internal Server Error</title>
    
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
                    <div class="alert alert-block alert-error">
                        <h4 class="alert-heading">500 Internal Server Error</h4>
                        <p>A generic error message, given when an unexpected condition was encountered and no more specific message is suitable.</p>
                        <p>Dies ist ein „Sammel-Statuscode“ für unerwartete Serverfehler.</p>
                    </div>
                </div>
                <div class="span3"></div>
            </div>
        </div>
    </body>
</html>


function addChild(targetId) {
    var target = $('div#' + targetId);
    $.ajax({
        url: '/Article/AddChild',
        data: { prefix: targetId + '[' + target.children().length + ']' },
        dataType: 'html',
        type: 'GET',
        success: function (result) {
            target.append(result);
        }
    });
}


function addProperty(propertyDropDownListBoxId, discriminatorDropDownListBoxId, targetId, targetName) {
    var id = $('select#' + propertyDropDownListBoxId).val();
    var target = $('div#' + targetId);
    $.ajax({
        url: '/Article/AddPropertyAjax/' + id,
        data: { prefix: targetName + '[' + target.children().length + ']' },
        dataType: 'html',
        type: 'GET',
        success: function (result) {
            target.append(result);
            removePropertyFromDropDownListBox(propertyDropDownListBoxId, id);
            removePropertyFromDropDownListBox(discriminatorDropDownListBoxId, id);
        }

    });
}

function addDiscriminator(propertyDropDownListBoxId, discriminatorDropDownListBoxId, targetId, targetName) {
    var id = $('select#' + discriminatorDropDownListBoxId).val();
    var target = $('div#' + targetId);
    $.ajax({
        url: '/Article/AddDiscriminatorAjax/' + id,
        data: { prefix: targetName + '[' + target.children().length + ']' },
        dataType: 'html',
        type: 'GET',
        success: function (result) {
            target.append(result);
            removePropertyFromDropDownListBox(propertyDropDownListBoxId, id);
            removePropertyFromDropDownListBox(discriminatorDropDownListBoxId, id);
        }
    });
}


function removeChildArticle(isDeletedFieldId) {
    var isDeleted = $("input#" + isDeletedFieldId);
    var closest = isDeleted.closest(".article-editor");
    closest.hide();
    isDeleted.val('True');
}

function removeProperty(propertyContainerId, caption, propertyDefId) {
    var divToRemove = $("div#" + propertyContainerId);
    var closest = divToRemove.closest(".article-editor");
    var children = closest.find("select");
    children.each(function () {
        addPropertyToDropDownListBox(this.id, caption, propertyDefId);
    });
    divToRemove.hide();
    divToRemove.find("input#" + propertyContainerId + "_IsDeleted").val('True');
    sortDropDownListByText();
}

function removeDiscriminator(discriminatorContainerId, caption, propertyDefId) {
    var divToRemove = $("div#" + discriminatorContainerId);
    var closest = divToRemove.closest(".article-editor");
    var children = closest.find("select");
    children.each(function () {
        addPropertyToDropDownListBox(this.id, caption, propertyDefId);
    });
    divToRemove.hide();
    divToRemove.find("input#" + propertyContainerId + "_IsDeleted").val('True');
    sortDropDownListByText();
}



function addPropertyToDropDownListBox(dropDownListBoxId, caption, value) {
    $("select#" + dropDownListBoxId).append('<option value="' + value + '">' + caption + '</option>');
    
}


function removePropertyFromDropDownListBox(dropDownListBoxId, valueId) {
    $("select#" + dropDownListBoxId + " option[value='" + valueId + "']").each(function () {
        $(this).remove();
    });
}

function sortDropDownListByText() {
    // Loop for each select element on the page.
    $("select").each(function () {

        // Keep track of the selected option.
        var selectedValue = $(this).val();

        // Sort all the options by text. I could easily sort these by val.
        $(this).html($("option", $(this)).sort(function (a, b) {
            return a.text == b.text ? 0 : a.text < b.text ? -1 : 1
        }));

        // Select one option.
        $(this).val(selectedValue);
    });
}



/*
* jQuery File Upload Plugin JS Example 5.0.2
* https://github.com/blueimp/jQuery-File-Upload
*
* Copyright 2010, Sebastian Tschan
* https://blueimp.net
*
* Licensed under the MIT license:
* http://creativecommons.org/licenses/MIT/
*/

/*jslint nomen: true */
/*global $ */

$(function () {
    'use strict';

    // Initialize the jQuery File Upload widget:
    $('#fileupload').fileupload({

    });

    // Load existing files:
    //    $.getJSON($('#fileupload form').prop('action'), function (files) {
    //        var fu = $('#fileupload').data('fileupload');
    //        fu._adjustMaxNumberOfFiles(-files.length);
    //        fu._renderDownload(files)
    //            .appendTo($('#fileupload .files'))
    //            .fadeIn(function () {
    //                // Fix for IE7 and lower:
    //                $(this).show();
    //            });
    //    });
    $('#fileupload').bind('fileuploaddone', function (e, data) {
        if (data.jqXHR.responseText || data.result) {
            var fu = $('#fileupload').data('fileupload');
            var JSONjQueryObject = (data.jqXHR.responseText) ? jQuery.parseJSON(data.jqXHR.responseText) : data.result;
            fu._adjustMaxNumberOfFiles(JSONjQueryObject.files.length);
            //                debugger;
            fu._renderDownload(JSONjQueryObject.files)
                .appendTo($('#fileupload .files'))
                .fadeIn(function () {
                    // Fix for IE7 and lower:
                    $(this).show();
                });
        }
    });

    // Open download dialogs via iframes,
    // to prevent aborting current uploads:
    $('#fileupload .files a:not([target^=_blank])').live('click', function (e) {
        e.preventDefault();
        $('<iframe style="display:none;"></iframe>')
            .prop('src', this.href)
            .appendTo('body');
    });

});
function Fundus() {

}


Fundus.prototype.init = function() {
    // Meldung anzeigen, wenn ein Ajax-Request gemacht wird.
    // Evtl. ganze Webseite sperren?
    // http://stackoverflow.com/questions/5938505/asp-mvc-3-loadingelementid-element-only-shows-on-first-request
    //$('#message-box-container').ajaxStart(function () {
    //    $(this).empty().append('<div class="warning">Bitte warten...</div>');
    //});

    // Datumspicker
    $('.date-picker').datepicker({ dateFormat: "dd.mm.yy" });

    /*
    // Zebratabellen
    $('.list tr:odd').addClass('alt');
    $('.list tr').mouseover(function () {
    $(this).addClass('over');
    });
    $('.list tr').mouseout(function () {
    $(this).removeClass('over');
    });
    */

    /*
    // Feeback bei Button klick
    $("input[type='submit']").click(function () {
    $(this).fadeTo('fast', 0.1).fadeTo('slow', 1.0);
    });
    $("input[type='button']").click(function () {
    $(this).fadeTo('fast', 0.1).fadeTo('slow', 1.0);
    });

    // Icons
    $('.ui-state-default').hover(
    function () { $(this).addClass('ui-state-hover'); },
    function () { $(this).removeClass('ui-state-hover'); }
    );
    $(".ui-state-default").click(function () {
    $(this).fadeTo('fast', 0.1).fadeTo('slow', 1.0);
    });
    */

    // Table Drag'n'Drop: Position-Inputs durch Icon "ersetzen"
    $('td.dnd-idx').css('cursor', 'move')
            .append('<a class="btn" style="cursor: move;">Verschieben</a>')
            .find('input').hide();

    // Table Drag'n'Drop: Checkboxen sollen auch Post-Back auslösen.
    $('table.dnd').find('input[type="checkbox"]').click(function () {
        $(this).closest('form').submit();
    });

    // Table Drag'n'Drop: Aktivierung
    $('table.dnd').tableDnD({
        dragHandle: 'dnd-idx',
        onDrop: function (table, row) {
            $(table).find('td.dnd-idx').find('input').each(function (index) {
                $(this).val(index);

            });
            $(table).closest('form').submit();
        }

    });

    // Tabs: Active setzen bei (unobstrusiven) Tabs, die den tab-content dynamisch laden (Ajax.ActionLinks).
    $('.nav-tabs li a').click(function () {
        var li = $(this).closest('li');
        li.siblings('.active').removeClass('active');
        li.addClass('active');
    });


    $('span.field-validation-valid, span.field-validation-error').each(function () {
        $(this).addClass('help-inline');
    });

    $('form').submit(function () {
        if ($(this).valid()) {
            $(this).find('div.control-group').each(function () {
                if ($(this).find('span.field-validation-error').length == 0) {
                    $(this).removeClass('error');
                }
            });
        }
        else {
            $(this).find('div.control-group').each(function () {
                if ($(this).find('span.field-validation-error').length > 0) {
                    $(this).addClass('error');
                }
            });
        }
    });

    $('form').each(function () {
        $(this).find('div.control-group').each(function () {
            if ($(this).find('span.field-validation-error').length > 0) {
                $(this).addClass('error');
            }
        });
    });

    $('.unobtrusive-remove').remove();


    /*
    * jQuery File Upload Plugin JS Example 6.5
    * https://github.com/blueimp/jQuery-File-Upload
    *
    * Copyright 2010, Sebastian Tschan
    * https://blueimp.net
    *
    * Licensed under the MIT license:
    * http://www.opensource.org/licenses/MIT
    */

    /*jslint nomen: true, unparam: true, regexp: true */
    /*global $, window, document */

    $(function () {
        'use strict';

        // Initialize the jQuery File Upload widget:
        $('#fileupload').fileupload();

        // Enable iframe cross-domain access via redirect option:
        $('#fileupload').fileupload(
        'option',
        'redirect',
        window.location.href.replace(
            /\/[^\/]*$/,
            '/cors/result.html?%s'
        )
    );

        if (window.location.hostname === 'blueimp.github.com') {
            // Demo settings:
            $('#fileupload').fileupload('option', {
                url: '//jquery-file-upload.appspot.com/',
                maxFileSize: 5000000,
                acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
                resizeMaxWidth: 1920,
                resizeMaxHeight: 1200
            });
            // Upload server status check for browsers with CORS support:
            if ($.ajaxSettings.xhr().withCredentials !== undefined) {
                $.ajax({
                    url: '//jquery-file-upload.appspot.com/',
                    type: 'HEAD'
                }).fail(function () {
                    $('<span class="alert alert-error"/>')
                    .text('Upload server currently unavailable - ' +
                            new Date())
                    .appendTo('#fileupload');
                });
            }
        } else {
            // Load existing files:
            $('#fileupload').each(function () {
                var that = this;
                $.getJSON(this.action, function (result) {
                    if (result && result.length) {
                        $(that).fileupload('option', 'done')
                        .call(that, null, { result: result });
                    }
                });
            });
        }

    });


}
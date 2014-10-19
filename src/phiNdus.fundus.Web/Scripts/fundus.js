﻿
var fundus;
!function ($) {
    "use strict";

    $(function () {
        fundus = {
            baseUri: undefined,

            init: function () {
                $(document).ajaxSuccess(function (e, xhr, opts) {
                    fundus.reinit();
                });

                $(document).ajaxError(function (event, request, settings) {
                    if (request.status == '500') {
                        var regex = new RegExp("<title>(.*?)</title>");
                        var matches = regex.exec(request.responseText);
                        if (matches.length > 0)
                            fundus.messageBox(request.status + ': ' + request.statusText, matches[1]);
                        else
                            fundus.messageBox(request.status + ': ' + request.statusText, '');
                    }
                    else if (request.status == '400') {
                        fundus.reinit();
                    }
                });

                // http://www.braindonor.net/blog/integrating-bootstrap-error-styling-with-mvcs-unobtrusive-error-validation/381/
                $('form').on('submit', function () {
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

                // Tabs: Active setzen bei (unobstrusiven) Tabs, die den tab-content dynamisch laden (Ajax.ActionLinks).
                $('.nav-tabs li a').on('click', function () {
                    var $li = $(this).closest('li');
                    $li.siblings('.active').removeClass('active');
                    $li.addClass('active');
                });

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

                $('form').on('submit', function () {
                    if (typeof window.triggerSave == 'function') {
                        $('textarea.tinymce').triggerSave();
                    }
                });

                this.reinit();
            },

            reinit: function () {
                $('span.field-validation-valid, span.field-validation-error').each(function () {
                    $(this).addClass('help-inline');
                });

                // http://www.braindonor.net/blog/integrating-bootstrap-error-styling-with-mvcs-unobtrusive-error-validation/381/
                $('form').each(function () {
                    $(this).find('div.control-group').each(function () {
                        if ($(this).find('span.field-validation-error').length > 0) {
                            $(this).addClass('error');
                        }
                    });
                });

                $('a.fundus-shop-showArticle').each(function (elem) {
                    var id = $(this).attr('article-id');
                    var href = $(this).attr('href');
                    $(this).click(function (e) { fundus.shop.showArticle(id, href); }).attr('href', '#').removeClass('fundus-shop-showArticle');
                });

                $('.unobtrusive-remove').remove();

                // http://www.tinymce.com/wiki.php/jQuery_Plugin
                // Initializes all textareas with the tinymce class
                $('textarea.tinymce-init').removeClass('tinymce-init').addClass('tinymce').tinymce({
                    //script_url: this.baseUri + 'Scripts/tinymce/tiny_mce.js',
                    theme: "simple",
                    entity_encoding: "raw"
                });

                $('.add-datepicker').datepicker();

                $('input.add-inc-dec').removeClass('add-inc-dec').each(function () {
                    $(this).wrap('<div class="input-append">');
                    $(this).after('<button class="btn add-inc" type="button"><i class="icon-plus"></i></button>',
                                  '<button class="btn add-dec" type="button"><i class="icon-minus"></i></button>');
                });

                $('.add-inc').removeClass('add-inc').click(function () {
                    var input = $(this).siblings('input');
                    var value = parseFloat(input.val()) + 1;
                    if (isNaN(value))
                        value = 1;
                    input.val(value);
                });

                $('.add-dec').removeClass('add-dec').click(function () {
                    var input = $(this).siblings('input');
                    var value = parseFloat(input.val()) - 1;
                    if (isNaN(value) || (value < 0))
                        value = 0;
                    input.val(value);
                });
            },

            messageBox: function (title, body) {
                var $div = $('#modal-show-error');

                $div.find('.modal-header h3').html(title);

                $div.find('.modal-body').html(body);
                $div.modal();
            }


        };

        fundus.users = {
            locked: function (id) {
                alert('Der Benutzer wurde erfolgreich gesperrt.')

                $('tr#user-' + id).find('a[data-ajax-success="fundus.users.locked"]').hide();
                $('tr#user-' + id).find('a[data-ajax-success="fundus.users.unlocked"]').show();
                $('tr#user-' + id).find('td[name="isLockedOut"] input').prop('checked', true);
            },

            unlocked: function (id) {
                alert('Der Benutzer wurde erfolgreich entsperrt.');

                $('tr#user-' + id).find('a[data-ajax-success="fundus.users.unlocked"]').hide();
                $('tr#user-' + id).find('a[data-ajax-success="fundus.users.locked"]').show();
                $('tr#user-' + id).find('td[name="isLockedOut"] input').prop('checked', false);
            }
        };

        fundus.shop = {
            showArticle: function (id, url) {
                var $div = $('#selected-articles');

                //$.scrollTo($div);

                // Artikel bereits angezeigt?
                if ($div.find('a[href="#' + id + '"]').length > 0) {
                    $.smoothScroll({
                        scrollTarget: '#' + id
                    });
                }
                else {

                    $.ajax({
                        url: url,
                        success: function (data, textStatus, jqXHR) {
                            $div.find('.nav-tabs').first().append('<li><a href="#' + id + '" data-toggle="tab">' + data.caption + ' <span class="close" style="margin:-2px -4px 0 8px;" onclick="fundus.shop.closeArticle(' + id + ')">×</span></a></li>');
                            $div.find('.tab-content').first().append('<div class="tab-pane" id="' + id + '">' + data.content + '</div>');

                            var $tab = $div.find('a[href="#' + id + '"]');
                            $tab.tab('show');

                            $.smoothScroll({
                                scrollTarget: '#' + id
                            });
                        }
                    });
                }
            },

            closeArticle: function (id) {
                var $div = $('#selected-articles');

                var $li = $div.find('a[href="#' + id + '"]').parent('li');
                if ($li.hasClass('active')) {
                    var $next = $li.next();
                    if ($next.length == 0)
                        $next = $li.prev();
                    $next.find('a').tab('show');
                }

                $li.remove();
                $div.find('div#' + id).remove();



            },

            orderConfirmed: function (orderDto) {
                alert('Die Bestellung wurde erfolgreich bestätigt und ein E-Mail wurde an ' + orderDto.ReserverName + ' gesendet.');
                $('tr#order-' + orderDto.Id).fadeOut(300, function () { $(this).remove; });
            },

            orderRejected: function (orderDto) {
                alert('Die Bestellung wurde abgelehnt und ein E-Mail wurde an ' + orderDto.ReserverName + ' gesendet.');
                $('tr#order-' + orderDto.Id).fadeOut(300, function () { $(this).remove; });
            },

            sayHello: function () {
                alert('hellas fellas!');
            }
        };
    });
} (window.jQuery);


if (typeof XDate != 'undefined') {
    !function () {
        "use strict";
        function parseDayMonthYear(str) {
            var parts = str.split('.');
            if (parts.length == 3) {
                return new XDate(
                    parseInt(parts[2]), // year
                    parseInt(parts[1]), // month
                    parseInt(parts[0]) // date
                );
            }
        }
        XDate.parsers.push(parseDayMonthYear);
    } ();
}


jQuery.extend(jQuery.validator.methods, {
    date: function (value, element) {
        return this.optional(element) || /^\d\d?\.\d\d?\.\d\d\d?\d?$/.test(value);
    },
    number: function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:,\d+)?$/.test(value);
    }
});

var fundus;
!function ($) {
    "use strict";

    $(function () {
        fundus = {
            baseUri: undefined,

            init: function () {
                $('body').ajaxSuccess(function (e, xhr, opts) {
                    fundus.reinit();
                });

                $('body').ajaxError(function (e, xhr, opts) {
                    fundus.showError(e, xhr, opts);
                });

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
                        //tinyMCE.triggerSave();
                    }
                });

                this.reinit();
            },

            reinit: function () {
                $('span.field-validation-valid, span.field-validation-error').each(function () {
                    $(this).addClass('help-inline');
                });

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
                    script_url: this.baseUri + 'Scripts/tinymce/tiny_mce.js',
                    theme: "simple",
                    entity_encoding: "raw"
                });
            },

            showError: function (e, xhr, opts) {
                if (this.showError === true) {
                    alert('hmm');
                    return;
                }
                this.showingError = true;
                var $div = $('#modal-show-error');

                $div.find('.modal-header h3').html(xhr.status + ': ' + xhr.statusText);

                $div.find('.modal-body').html('<p>Oops! Das hätte nicht passieren dürfen!</p>');
                $div.modal();
                this.showingError = false;
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

            sayHello: function () {
                alert('hellas fellas!');
            }
        };
    });
} (window.jQuery);

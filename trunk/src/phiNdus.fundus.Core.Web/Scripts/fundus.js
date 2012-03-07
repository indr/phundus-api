
var fundus = {
    baseUri: undefined,

    init: function () {
        $('body').ajaxSuccess(function (e, xhr, opts) {
            fundus.reinit();
        });

        $('body').ajaxError(function (e, xhr, opts) {
            fundus.showError(e, xhr, opts);
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

        this.reinit();
    },

    reinit: function () {
        $('.unobtrusive-remove').remove();

        // http://www.tinymce.com/wiki.php/jQuery_Plugin
        // Initializes all textareas with the tinymce class
        $('textarea.tinymce').tinymce({
            script_url: this.baseUri + 'Scripts/tinymce/tiny_mce.js',
            theme: "simple"
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
}

fundus.shop = {
    sayHello: function () {
        alert('hellas fellas!');
    }
}












//Fundus.prototype.reinit = function () {

//    //alert('reinit');
//    
//    // Datumspicker
//    $('.date-picker').datepicker({ dateFormat: "dd.mm.yy" });

//    

//    


//    $('span.field-validation-valid, span.field-validation-error').each(function () {
//        $(this).addClass('help-inline');
//    });

//    $('form').submit(function () {
//        if ($(this).valid()) {
//            $(this).find('div.control-group').each(function () {
//                if ($(this).find('span.field-validation-error').length == 0) {
//                    $(this).removeClass('error');
//                }
//            });
//        }
//        else {
//            $(this).find('div.control-group').each(function () {
//                if ($(this).find('span.field-validation-error').length > 0) {
//                    $(this).addClass('error');
//                }
//            });
//        }
//    });

//    $('form').each(function () {
//        $(this).find('div.control-group').each(function () {
//            if ($(this).find('span.field-validation-error').length > 0) {
//                $(this).addClass('error');
//            }
//        });
//    });



//    
//}


// Meldung anzeigen, wenn ein Ajax-Request gemacht wird.
// Evtl. ganze Webseite sperren?
// http://stackoverflow.com/questions/5938505/asp-mvc-3-loadingelementid-element-only-shows-on-first-request
//$('#message-box-container').ajaxStart(function () {
//    $(this).empty().append('<div class="warning">Bitte warten...</div>');
//});



function addProperty(propertyDropDownListBoxId, discriminatorDropDownListBoxId, targetId) {
    var id = $('select#' + propertyDropDownListBoxId).val();
    $.ajax({
        url: '/Article/AddPropertyAjax/' + id,
        dataType: 'html',
        type: 'GET',
        success: function (result) {
            $('div#' + targetId).append(result);
            removePropertyFromDropDownListBox(propertyDropDownListBoxId, id);
            removePropertyFromDropDownListBox(discriminatorDropDownListBoxId, id);
        }

    });
}

function addDiscriminator(propertyDropDownListBoxId, discriminatorDropDownListBoxId, targetId) {
    var id = $('select#' + discriminatorDropDownListBoxId).val();
    $.ajax({
        url: '/Article/AddDiscriminatorAjax/' + id,
        dataType: 'html',
        type: 'GET',
        success: function (result) {
            $('div#' + targetId).append(result);
            removePropertyFromDropDownListBox(propertyDropDownListBoxId, id);
            removePropertyFromDropDownListBox(discriminatorDropDownListBoxId, id);
        }
    });
}


function removeProperty(propertyContainerId, caption, propertyDefId) {
    var divToRemove = $("div#" + propertyContainerId);
    var closest = divToRemove.closest(".article-editor");
    var children = closest.find("select");
    children.each(function () {
        addPropertyToDropDownListBox(this.id, caption, propertyDefId);
    });
    divToRemove.remove();
    sortDropDownListByText();
}

function removeDiscriminator(discriminatorContainerId, caption, propertyDefId) {
    var divToRemove = $("div#" + discriminatorContainerId);
    var closest = divToRemove.closest(".article-editor");
    var children = closest.find("select");
    children.each(function () {
        addPropertyToDropDownListBox(this.id, caption, propertyDefId);
    });
    divToRemove.remove();
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

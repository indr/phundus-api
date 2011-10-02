
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


var utils = {};

utils.objectifyFormData = function(formId) {
    var data = $(formId).serializeArray();
    var dataObj = {};
    data.forEach(function(val) {
        dataObj[val.name] = val.value;
    });
    return dataObj;
};
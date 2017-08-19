$(function () {
    $.extend($.fn.validatebox.defaults.rules, {
        DateMonth: {
            validator: function (value) {
                var objRegExp = /^(\d{4})\-(\d{2})$/;
                if (!objRegExp.test(value)) {
                    return false;
                }
                var arrayDate = value.split('-');
                var intYear = parseInt(arrayDate[0], 10);
                var intMonth = parseInt(arrayDate[1], 10);

                if (intMonth < 1 || intMonth > 12) {
                    return false;
                }
                return true;
            },
            message: '请输入正确的日期格式，格式：yyyy-MM'
        }
    });

    $.extend($.fn.validatebox.defaults.rules, {
        CustomDate: {
            validator: function (value) {
                var objRegExp = /^(\d{4})\-(\d{2})\-(\d{2})$/;
                if (!objRegExp.test(value)) {
                    return false;
                }
                var arrayDate = value.split('-');
                var intDay = parseInt(arrayDate[2], 10);
                var intYear = parseInt(arrayDate[0], 10);
                var intMonth = parseInt(arrayDate[1], 10);

                if (intMonth < 1 || intMonth > 12) {
                    return false;
                }

                if (intDay < 1 || intDay > 31) {
                    return false;
                }

                return true;
            },
            message: '请输入正确的日期格式，格式：yyyy-MM-dd'
        }
    });

    $.extend($.fn.validatebox.defaults.rules, {
        CustomTime: {
            validator: function (value) {
                var objRegExp = /^(\d{2}):(\d{2})$/;
                if (!objRegExp.test(value)) {
                    return false;
                }

                var arrayDate = value.split(':');
                var intMM = parseInt(arrayDate[1], 10);
                var intHH = parseInt(arrayDate[0], 10);

                if (intHH < 0 || intHH > 24) {
                    return false;
                }

                if (intMM < 0 || intMM > 60) {
                    return false;
                }

                return true;
            },
            message: '请输入正确的时间格式，格式：HH:mm'
        }
    });

    $.extend($.fn.validatebox.defaults.rules, {
        CustomLongDateTime: {
            validator: function (value) {
                var objRegExp = /^(?:19|20)[0-9][0-9]-(?:(?:0[1-9])|(?:1[0-2]))-(?:(?:[0-2][1-9])|(?:[1-3][0-1])) (?:(?:[0-2][0-3])|(?:[0-1][0-9])):[0-5][0-9]$/;
                if (!objRegExp.test(value)) {
                    return false;
                }

                return true;
            },
            message: '请输入正确的时间格式，格式：yyyy-MM-dd HH:mm'
        }
    });

    $.extend($.fn.validatebox.defaults.rules, {
        CustomInteger: {
            validator: function (value) {
                var objRegExp = /^[0-9]*[1-9][0-9]*$/;
                if (!objRegExp.test(value)) {
                    return false;
                }

                return true;
            },
            message: '输入正整数'
        }
    });

    $.extend($.fn.validatebox.defaults.rules, {
        CustomIntegerWithZero: {
            validator: function (value) {
                var objRegExp = /^[0-9]*$/;
                if (!objRegExp.test(value)) {
                    return false;
                }

                return true;
            },
            message: '输入正整数'
        }
    });

    $.extend($.fn.validatebox.defaults.rules, {
        CustomIncludeZeroInt: {
            validator: function (value) {
                var objRegExp = /^[0-9]+\d*$/;
                if (!objRegExp.test(value)) {
                    return false;
                }

                var len = $.trim(value).length;
                if (len > 6) {
                    return false;
                }

                return true;
            },
            message: '输入大于等于零的整数,长度不能超过6位'
        }
    });

    $.extend($.fn.validatebox.defaults.rules, {
        CustomIntOrFloat: {
            validator: function (value) {
                var objRegExp = /^\d+(\.\d{1,4})?$/;
                if (!objRegExp.test(value)) {
                    return false;
                }

                var len = $.trim(value).length;

                if ($.trim(value).indexOf('.') == -1) {
                    if (len > 11) {
                        return false;
                    }
                } else {
                    if (len > 16) {
                        return false;
                    }
                }

                return true;
            },
            message: '输入整数或者小数(保留四位),长度整数不能超过11位,小数不能超过16位'
        }
    });

});
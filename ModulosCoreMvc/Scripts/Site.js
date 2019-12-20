var $optionActive = null;
var $formActive = null;
$body = $("#area-body");

var constantes = {
    numericBool: {
        TRUE: 1,
        FALSE: 0
    },
    actions: {
        ADD: 1,
        EDIT: 2,
        DELETE: 3
    },
    nivelUbigeo: {
        DEPARTAMENTO: 1,
        PROVINCIA: 2,
        DISTRITO: 3
    },
    tipoAlerta: {
        SUCCESS: 1,
        WARNING: 2,
        DANGER: 3,
        INFO: 4
    },
    errorCodes: {
        ERROR_MODEL_INVALID: 1,
        ERROR_FORMAT_KEY: 2,
        ERROR_ENTITY_NO_EXISTS: 3,
        ERROR_SERVER: 4,
        ERROR_DUPLICATE_ENTITY: 5,
        ERROR_ENTITY_INTEGRITY: 6
    },
    defaultValidationResult: {
        valid: true,
        message: 'Hay datos que no cumplen con la validación requerida, por favor corrija'
    }
}

$.ajaxSetup({
    error: function (x, e) {
        if (x.status == 403) {
            window.location.href = x.responseJSON.returnUrl;
        }
    }
});

$(document).on({
    ajaxStart: function () { $body.addClass("loading"); },
    ajaxStop: function () { $body.removeClass("loading"); }
});

//JQUERY CUSTOM PLUGINS
(function ($) {
    /**
     * Plugin JQuery para crear combobox editables
     * @returns {} 
     */
    $.fn.myEditableCombo = function () {
        var me = this;
        $(me).parent().addClass('my-editable-combo');
        if ($(me)[0].nextElementSibling == null) {
            $(me).parent().append('<input type="text"/>');
        }
        $(me).click(function () {
            if ($(me).val() != 0 && $(me).val() != undefined) {
                $(me)[0].nextElementSibling.value = $(me).find('option[value=' + $(me).val() + ']').html();
            }
            else {
                $(me)[0].nextElementSibling.value = "";
            }
        });

        $($(me)[0].nextElementSibling).change(function () {
            $(me).val(null);
        });
    };

    /**
     * Plugin JQuery para crear tablas boostrap 3 (POWER BY BOOSTRAP TABLE 3)
     * @returns {} 
     */
    $.fn.convertBoostrap3Table = function (height, extendOnClear, searchFunction, onClickRow, crudController) {
        var divTable = $(this);
        var table = divTable.find('table[name=table]');
        var toolbar = divTable.find('div[id="toolbar"]');
        var remove = $(this).find('button[id=remove]');
        var arraySelections = [];

        table.bootstrapTable({
            height: height
        });

        setTimeout(function () {
            table.bootstrapTable('resetView');
        }, 200);

        table.on('check.bs.table uncheck.bs.table ' +
        'check-all.bs.table uncheck-all.bs.table', function () {
            remove.prop('disabled', !table.bootstrapTable('getSelections').length);
            arraySelections = getIdSelections(table);
        });

        table.on('expand-row.bs.table', function (e, index, row, $detail) {
            if (index % 2 == 1) {
                $detail.html('Loading from ajax request...');
                $.get('LICENSE', function (res) {
                    $detail.html(res.replace(/\n/g, '<br>'));
                });
            }
        });

        table.on('all.bs.table', function (e, name, args) {
        });

        if (onClickRow != undefined && onClickRow != null) {
            table.on('click-row.bs.table', onClickRow);
        }

        if (remove != undefined) {
            remove.click(function () {
                var ids = getIdSelections();
                table.bootstrapTable('remove', {
                    field: 'id',
                    values: ids
                });
                remove.prop('disabled', true);
            });
        }

        $(window).resize(function () {
            table.bootstrapTable('resetView', {
                height: height
            });
        });

        if (toolbar != null) {
            toolbar.find('button[id=aceptar]').click(function () {
                if (searchFunction == undefined || searchFunction == null) {
                    table.bootstrapTable('refresh');
                } else {
                    searchFunction();
                }
            });

            toolbar.find('button[id=limpiar]').click(function () {
                clearFilters();
                if (extendOnClear != undefined && extendOnClear != null) {
                    extendOnClear();
                }
                table.bootstrapTable('refresh');
            });
        }
        return table;
    };


    $.fn.ubigeoPeru = function (defaultRegion, urlData, isEdit, codigo, id) {
        var div = $(this);
        var cboDepartamento = div.find('[name="CodigoDepartamento"]');
        var cboProvincia = div.find('[name="CodigoProvincia"]');
        var cboDistrito = div.find('[name="UbigeoId"]');

        cboDepartamento.change(function () {
            var codigoDepartamento = $(this).val();
            if (codigoDepartamento != 0) {
                $.ajax({
                    url: urlData,
                    type: 'POST',
                    data: { nivel: constantes.nivelUbigeo.PROVINCIA, codigoDepartamento: codigoDepartamento },
                    success: function (response) {
                        if (!isEdit) {
                            setDataOnCombobox(cboProvincia, 'CodigoProvincia', 'NombreProvincia', response.data);
                        }
                        else {
                            setDataOnComboboxSelect(cboProvincia, 'CodigoProvincia', 'NombreProvincia', response.data, codigo.substring(2, 4));
                            cboProvincia.change();
                        }
                    }
                });
            }
            else {
                cboProvincia.html('');
                cboDistrito.html('');
                cboProvincia.val(0);
                cboDistrito.val(0);
            }
        });

        cboProvincia.change(function () {
            var codigoDepartamento = cboDepartamento.val();
            var codigoProvincia = $(this).val();
            if (codigoProvincia != 0) {
                $.ajax({
                    url: urlData,
                    type: 'POST',
                    data: {
                        nivel: constantes.nivelUbigeo.DISTRITO, codigoDepartamento: codigoDepartamento,
                        codigoProvincia: codigoProvincia
                    },
                    success: function (response) {
                        if (!isEdit) {
                            setDataOnCombobox(cboDistrito, 'Id', 'NombreDistrito', response.data);
                        }
                        else {
                            setDataOnComboboxSelect(cboDistrito, 'Id', 'NombreDistrito', response.data, id);
                        }
                    }
                });
            }
            else {
                cboDistrito.html('');
                cboDistrito.val(0);
            }
        });

        $.ajax({
            url: urlData,
            type: 'POST',
            data: { nivel: constantes.nivelUbigeo.DEPARTAMENTO },
            success: function (response) {
                if (!isEdit) {
                    setDataOnCombobox(cboDepartamento, 'CodigoDepartamento', 'NombreDepartamento', response.data);
                }
                else {
                    setDataOnComboboxSelect(cboDepartamento, 'CodigoDepartamento', 'NombreDepartamento', response.data, codigo.substring(0, 2));
                    cboDepartamento.change();
                }
            }
        });
    };

    $.fn.TipoDocumento = function (defaultRegion, urlData, isEdit, id) {
        var cboTipoDocumento = $(this);

        $.ajax({
            url: urlData,
            type: 'GET',
            data: {},
            success: function (response) {
                if (!isEdit) {
                    setDataOnCombobox(cboTipoDocumento, 'Id', 'Acronimo', response.data);
                }
                else {
                    setDataOnComboboxSelect(cboTipoDocumento, 'Id', 'Acronimo', response.data, id);
                }
            }
        });
    };


    $.fn.serializeJSON = function () {
        var data = {};
        var arrayData = $(this).serializeArray();
        $.each(arrayData, function (index, element) {
            data[element.name] = element.value;
        });
        return data;
    };

    $.fn.intNumber = function () {
        $(this).keypress(function (event) {
            return event.charCode >= 48 && event.charCode <= 57;
        });
    }
})(jQuery);

function getUrl(area, controller, action) {
    var url = '/';
    if (area != undefined && area != null) {
        url += area.trim() + '/' + controller + '/' + action;
    } else {
        url += controller + '/' + action;
    }
    return url;
}

function onOptionClick(area, controller, action, data, evt) {
    freeMemory();
    var requestFunc = function () {
        $.ajax({
            url: getUrl(area, controller, action),
            type: 'GET',
            data: data,
            success: function (response) {
                removeModals(false);
                if ($optionActive != null) {
                    desActiveOption($optionActive);
                }
                activeOption(evt);
                $('#app-body').html(response);
                $formActive = null;
            }
        });
    };
    if ($formActive != null) {
        showConfirmMessage('Confirmación', 'Actualemente tiene un formulario activo, ¿Esta seguro que desea salir del formulario actual, para ir a esta opción?',
            function () {
                requestFunc();
                $('.modalMessageConfirm').modal('toggle');
            });
    }
    else {
        requestFunc();
    }
}

function freeMemory() {
    var controllersnames = Object.keys($appContext);
    $.each(controllersnames, function (index, name) {
        $appContext[name] = null;
    });
}

function activeOption(option) {
    $optionActive = option;
    $(option).find('a').addClass('activo');
}

function desActiveOption(option) {
    $(option).find('a').removeClass('activo');
}

//FUNCTION FOR BOOSTRAP TABLE 3:
function getWindowHeight() {
    return $(window).height();
}

function getIdSelections(table) {
    return $.map(table.bootstrapTable('getSelections'), function (row) {
        return row.id;
    });
}

$responseHandler = function (res) {
    if (res.rows == null) {
        res.rows = [];
    }
    $.each(res.rows, function (i, row) {
        row.state = $.inArray(row.id, []) !== -1;
    });
    return res;
}

function detailFormatter(index, row) {
    var html = [];
    $.each(row, function (key, value) {
        html.push('<p><b>' + key + ':</b> ' + value + '</p>');
    });
    return html.join('');
}

function operateFormatter(value, row, index) {
    return [
        '<a class="like" href="javascript:void(0)" title="Like">',
        '<i class="glyphicon glyphicon-heart"></i>',
        '</a>  ',
        '<a class="remove" href="javascript:void(0)" title="Remove">',
        '<i class="glyphicon glyphicon-remove"></i>',
        '</a>'
    ].join('');
}

window.operateEvents = {
    'click .like': function (e, value, row, index) {
        alert('You click like action, row: ' + JSON.stringify(row));
    },
    'click .remove': function (e, value, row, index) {
        $table.bootstrapTable('remove', {
            field: 'id',
            values: [row.id]
        });
    }
};

function totalTextFormatter(data) {
    return 'Total';
}

function totalNameFormatter(data) {
    return data.length;
}

function totalPriceFormatter(data) {
    var total = 0;
    $.each(data, function (i, row) {
        total += +(row.price.substring(1));
    });
    return '$' + total;
}

function showNotification(tipo, title, message) {
  
    var icono = 'glyphicon glyphicon-info-sign';
    var tipoMsg = 'info';
    switch (tipo) {
        case constantes.tipoAlerta.SUCCESS:
            icono = 'glyphicon glyphicon-ok-sign';
            tipoMsg = 'success';
            break;
        case constantes.tipoAlerta.WARNING:
            icono = 'glyphicon glyphicon-alert';
            tipoMsg = 'warning';
            break;
        case constantes.tipoAlerta.DANGER:
            icono = 'glyphicon glyphicon-exclamation-sign';
            tipoMsg = 'danger';
            break;
    };

    $.notify({
        icon: icono,
        title: title,
        message: message
        //url: 'https://github.com/mouse0270/bootstrap-notify',
        //target: '_blank'
    }, {
        element: 'body',
        position: null,
        type: tipoMsg,
        allow_dismiss: false,
        newest_on_top: true,
        showProgressbar: false,
        placement: {
            from: "bottom",
            align: "right"
        },
        offset: 20,
        spacing: 10,
        z_index: 1031,
        delay: 3000,
        timer: 1000,
        //url_target: '_blank',
        mouse_over: 'pause',
        animate: {
            enter: 'animated fadeInDown',
            exit: 'animated fadeOutUp'
        },
        //onShow: null,
        //onShown: null,
        //onClose: null,
        //onClosed: null,
        icon_type: 'class',
        template: ' <div class="alert alert-{0} alert-dismissible fade in" role="alert">' +
                    '<button type="button" class="close" data-dismiss="alert" aria-label="Close">' +
                    '<span aria-hidden="true">×</span>' +
                    '</button>' +
                    ' <h4><span data-notify="icon"></span>&nbsp;{1}</h4><p>{2}</p>' +
                      '<div class="progress" data-notify="progressbar">' +
                            '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                        '</div>' +
                        '<a href="{3}" target="{4}" data-notify="url"></a>' +
                    '</div>'
    });

}



function prevenDefaultFormAdd(form, controller) {
    form.submit(function (event) {
        event.preventDefault();
        var resultValidate = controller.formValidateAdd(form);
        if (resultValidate.valid) {
            controller.saveFormAdd(form);
        }
        else {
            showNotification(constantes.tipoAlerta.WARNING, "" ,resultValidate.message);
        }
    });
}

function prevenDefaultFormEdit(form, controller) {
    form.submit(function (event) {
        event.preventDefault();
        var resultValidate = controller.formValidateEdit(form);
        if (resultValidate.valid) {
            controller.saveFormEdit(form);
        }
        else {
            showNotification(constantes.tipoAlerta.WARNING,"", resultValidate.message);
        }
    });
}

function handleErrorCodes(code, message, data, controller) {
    switch (code) {
        case constantes.errorCodes.ERROR_SERVER:
            showNotification(constantes.tipoAlerta.DANGER, "¡Algo salió mal!", message);
            break;
        case constantes.errorCodes.ERROR_MODEL_INVALID:
        case constantes.errorCodes.ERROR_FORMAT_KEY:
        case constantes.errorCodes.ERROR_ENTITY_NO_EXISTS:
            showNotification(constantes.tipoAlerta.WARNING,"¡Sea responsable con la información!", message);
            break;
        case constantes.errorCodes.ERROR_DUPLICATE_ENTITY:
            showNotification(constantes.tipoAlerta.WARNING, "¡Sea responsable con la información!", controller.messageDuplicated(data));
            break;
        case constantes.errorCodes.ERROR_ENTITY_INTEGRITY:
            showNotification(constantes.tipoAlerta.WARNING, "¡Sea responsable con la información!", controller.messageIntegrity(data));
            break;
    }
}

function removeModals(isReusable) {
    if ((isReusable != undefined && !isReusable) || isReusable == undefined) {
        var modals = $('.modalForm');
        $.each(modals, function (index, element) {
            $(element).remove();
        });
    }
}
function onCancelForm(form) {
    var btn = form.find('.btnCancelar');
    var cancelUrl = form.find('#cancelUrl').val();
    btn.click(function () {
        showConfirmMessage(null, '¿Esta seguro que desea cancelar el formulario?', function () {
            freeMemory();
            $.ajax({
                url: cancelUrl,
                type: 'GET',
                success: function (response) {
                    removeModals(false);
                    $('#app-body').html(response);
                    $('.modalMessageConfirm').modal('toggle');
                }
            });
        });
    });
}

//Base controller for actions:
$CrudController = {
    currentForm: null,
    onAdd: function (area, controller, action, modal, isReusable) {
        var me = this;
        removeModals(isReusable);
        $.ajax({
            url: getUrl(area, controller, action),
            type: 'GET',
            success: function (response) {
                if (modal) {
                    var tmpl = $('#template-modal').tmpl();
                    tmpl.find('.modal-body').html(response);
                    var form = tmpl.find('form');
                    me.currentForm = form;
                    tmpl.on('shown.bs.modal', function () {
                        me.onAfterShowAdd(form);
                    });
                    if (form != undefined) {
                        prevenDefaultFormAdd(form, me);
                        tmpl.modal({
                            cache: false,
                            backdrop: 'static'
                        }, 'show');
                    }
                }
                else {
                    $('#app-body').html(response);
                    var form = $('#app-body').find('form');
                    me.currentForm = form;
                    $formActive = form;
                    if (form != undefined) {
                        prevenDefaultFormAdd(form, me);
                        me.onAfterShowAdd(form);
                        onCancelForm(form);
                    }
                }
            }
        });
    },
    onEdit: function (area, controller, action, modal, isReusable) {
        var me = this;
        var selections = $table.bootstrapTable('getSelections');
        if (selections.length > 0) {
            var data = selections[0];
            removeModals(isReusable);
            $.ajax({
                url: getUrl(area, controller, action),
                type: 'GET',
                data: { id: data.Id },
                success: function (response) {
                    if (modal) {
                        var tmpl = $('#template-modal').tmpl();
                        tmpl.find('.modal-body').html(response);
                        var form = tmpl.find('form');
                        me.currentForm = form;
                        tmpl.on('shown.bs.modal', function () {
                            me.onAfterShowEdit(form);
                        });
                        if (form != undefined) {
                            prevenDefaultFormEdit(form, me);
                            tmpl.modal({
                                cache: false,
                                backdrop: 'static'
                            }, 'show');
                        }
                    }
                    else {
                        $('#app-body').html(response);
                        var form = $('#app-body').find('form');
                        me.currentForm = form;
                        $formActive = form;
                        if (form != undefined) {
                            prevenDefaultFormEdit(form, me);
                            me.onAfterShowEdit(form);
                        }
                    }
                }
            });
        }
        else {
            showMessage(null, 'Primero seleccione un registro');
        }
    },
    onDelete: function (area, controller, action) {
        var me = this;
        var selections = $table.bootstrapTable('getSelections');
        if (selections.length > 0) {
            var data = selections[0];
            removeModals(false);
            showConfirmMessage(null, me.messageConfirmDelete(data), function () {
                $.ajax({
                    url: getUrl(area, controller, action),
                    type: 'POST',
                    data: { id: data.Id, __RequestVerificationToken: $('input[name=__RequestVerificationToken]').val() },
                    success: function (response) {
                        if (response.success) {
                            $table.bootstrapTable('refresh');
                            $('.modalMessageConfirm').modal('toggle');
                            showNotification(constantes.tipoAlerta.SUCCESS, "¡En hora Buena!", 'El registro se ha eliminado');
                        }
                        else {
                            handleErrorCodes(response.errorCode, response.message, data, me);
                        }
                    }
                });
            });
        }
        else {
            showMessage(null, 'Primero seleccione un registro');
        }
    },
    saveFormAdd: function (form) {
        var me = this;
        var data = form.serializeJSON();
        me.customDataAdd(data, form);
        $.ajax({
            url: form.attr('action'),
            type: 'POST',
            data: data,
            success: function (response) {
                if (response.success) {
                    $table.bootstrapTable('refresh');
                    $('.modalForm').modal('toggle');
                    showNotification(constantes.tipoAlerta.SUCCESS, "¡En hora Buena!", 'El registro se ha completado de forma exitosa');
                }
                else {
                    handleErrorCodes(response.errorCode, response.message, data, me);
                }
            }
        });
    },
    saveFormEdit: function (form) {
        var me = this;
        var data = form.serializeJSON();
        me.customDataEdit(data, form);
        $.ajax({
            url: form.attr('action'),
            type: 'POST',
            data: data,
            success: function (response) {
                if (response.success) {
                    $table.bootstrapTable('refresh');
                    $('.modalForm').modal('toggle');
                    showNotification(constantes.tipoAlerta.SUCCESS, "¡En hora Buena!", 'La edición se ha completado de forma exitosa');
                }
                else {
                    handleErrorCodes(response.errorCode, response.message, data, me);
                }
            }
        });
    },
    customDataAdd: function (data, form) { },
    customDataEdit: function (data, form) { },
    formValidateAdd: function (form) { return constantes.defaultValidationResult; },
    formValidateEdit: function (form) { return constantes.defaultValidationResult; },
    onAfterShowAdd: function (form) { },
    onAfterShowEdit: function (form) { },
    messageDuplicated: function (data) { return 'Ya existe un registro duplicado'; },
    messageIntegrity: function (data) { return 'El registro esta siendo utilizado'; },
    messageConfirmDelete: function (data) { return '¿Esta seguro que desea eliminar el registro?'; },
    showModal: function (url, widthModal) {
        $.ajax({
            url: url,
            type: 'GET',
            data: {},
            success: function (response) {
                var tmpl = $('#template-modal-auto').tmpl({ widthModal: widthModal });
                tmpl.find('.modal-body').html(response);
                tmpl.modal({
                    cache: false
                });
            }
        });
    },
    formatterOptionsCrud: function (value, obj, index) {
        var tmpl = $('#template-formatter-options-crud').tmpl(obj);
        return tmpl[0].innerHTML;
    },
    formatterFlag: function (value, obj, index) {
        if (value != null) {
            if (value) {
                return '<center><img alt="" src="/Content/Images/check.png" /></center>';
            }
            else {
                return '<center><img alt="" src="/Content/Images/warning.png" /></center>';
            }
        }
        return '';
    }
};


$appContext = {

}

//SHOW DIALOG AND MESSAGE FUNCTIONS

/**
 * Muestra un dialogo de información
 * @param {} title 
 * @param {} mensaje 
 * @param {} callBack 
 * @returns {} 
 */
function showMessage(title, message, callBack) {
    $('.modalMessageInfo').remove();
    var tmpl = $('#template-message').tmpl({
        title: title,
        message: message
    });

    if (callBack != undefined && callBack != null) {
        tmpl.find('[type=button]').click(callBack);
    }

    tmpl.modal('show');
}

/**
 * Muestra un dialogo de confirmación o  pregunta,
 * ejecuta una acción despues de terminada la ejecución
 * @param {} title 
 * @param {} mensaje 
 * @param {} callBack 
 * @returns {} 
 */
function showConfirmMessage(title, message, callBack) {
    $('.modalMessageConfirm').remove();
    var tmpl = $('#template-confirm').tmpl({
        title: title,
        message: message
    });

    if (callBack != undefined && callBack != null) {
        tmpl.find('.btn-ok').click(callBack);
    }

    tmpl.modal('show');
}
//DINAMIC FILTERS FUNCTIONS: 
/**
 * Construye un arreglo de filtros dinámicos
 * @param {} params 
 * @returns {} 
 */
function addCustomFilters(params) {
    var arrayFiltros = [];
    var filtros = $('.filtro');

    $.each(filtros, function (index, item) {
        var value = null;
        var tipo = $(item).attr('tipo-filtro');
        if (tipo == 'date') {
            value = $(item).find('input').val();
        }
        else if (tipo == 'list') {
            var list = $(item).find('input[type=checkbox]:checked');
            value = "";
            for (var i = 0; i < list.length; i++) {
                if (i == 0) {
                    value += $(list[i]).val();
                } else {
                    value += ',' + $(list[i]).val();
                }
            }
        }
        else if (tipo == 'checked') {
            if ($(item).prop('checked')) {
                value = $(item).val();
            }
        }
        else if (tipo == 'bool') {
            if ($(item).prop('checked')) {
                value = $(item).val();
            }
        }
        else {
            value = $(item).val();
        }

        if (tipo != 'checked' && tipo != 'list' && tipo != 'bool') {
            if (value != undefined && value.trim() !== '' && value != 0 && value != null) {
                arrayFiltros.push({
                    NombreCampo: $(item).attr('name'),
                    Valor: value,
                    Tipo: (tipo == 'bool') ? 'bit' : tipo,
                    Expresion: $(item).attr('filtro-exp')
                });
            }
        } else if (tipo == 'list') {
            arrayFiltros.push({
                NombreCampo: $(item).attr('name'),
                Valor: value,
                Tipo: $(item).attr('tipo-filtro'),
                Expresion: $(item).attr('filtro-exp')
            });
        }
        else if (tipo == 'bool') {
            if (value != undefined && value.trim() !== '' && value != null) {
                arrayFiltros.push({
                    NombreCampo: $(item).attr('name'),
                    Valor: value,
                    Tipo: (tipo == 'bool') ? 'bit' : tipo,
                    Expresion: $(item).attr('filtro-exp')
                });
            }
        }
        else {
            if (value == true) {
                arrayFiltros.push({
                    NombreCampo: $(item).attr('name'),
                    Valor: value,
                    Tipo: (tipo == 'checked') ? 'integer' : tipo,
                    Expresion: $(item).attr('filtro-exp')
                });
            }
        }
    });

    params.filters = JSON.stringify(arrayFiltros);

    if (params.sort != null && params.sort != '') {
        params.sort = parseSort(params.sort);
    }
    else {
        params.sort = $('#defaultSort').val();
    }
    var defaultSearch = $('#defaultSearch').val();
    if (defaultSearch != undefined && defaultSearch != null) {
        params.fieldSearch = defaultSearch;
    }

    params.customSearch = $('#customSearch').val() != undefined ? $('#customSearch').val() : false;
    return params;
}

/**
 * Parsea el nombre del campo por el cual se quiere ordenar
 * @param {} sort 
 * @returns {} 
 */
function parseSort(sort) {
    var control = $('#' + sort);
    var value = null;
    if (control != undefined && control != null && control != '') {
        value = $(control).val();
    }
    if (value != undefined && value != null && value != '') {
        return value;
    }
    return false;
}

/**
 * Limpia los filtros dinámicos
 * @returns {} 
 */
function clearFilters() {
    var filtros = $('.filtro');
    $.each(filtros, function (index, item) {
        var estable = $(item).attr('filter-estable');
        if (estable == undefined || !estable) {
            if ($(item).attr('tipo-filtro') == 'date') {
                value = $(item).find('input').val('');
            } else if ($(item).attr('tipo-filtro') == 'list') {
                var list = $(item).find('input[type=checkbox]');
                $.each(list, function (idx, le) {
                    $(le).prop('checked', true);
                });
                list.change();
            } else if ($(item).attr('tipo-filtro') == 'integer') {
                value = $(item).val(0);
            } else if ($(item).attr('tipo-filtro') == 'string') {
                value = $(item).val('');
            } else if ($(item).attr('tipo-filtro') == 'checked') {
                if ($(item).prop('checked')) {
                    $(item).prop('checked', false);
                    $(item).change();
                }
            } else if ($(item).attr('tipo-filtro') == 'bool') {
                if ($(item).prop('checked')) {
                    $(item).prop('checked', false);
                    $(item).change();
                }
            }
        }
    });
}

/**
 * Obtiene un objeto con todos losa datos de un registro de la grilla de bandeja de fichas
 * @param {} index 
 * @param {} table 
 * @returns {} 
 */
function getObjectAtIndexPage(index, table) {
    var data = table.bootstrapTable('getData');
    return data[index];
}

//GENERAL GUI FUNCTIONS
/**
 * Poner Data en combobox
 * @param {} comboId 
 * @param {} fielId 
 * @param {} fieldLabel 
 * @param {} arrayData 
 * @returns {} 
 */
function setDataOnCombobox(combo, fielId, fieldLabel, arrayData) {
    combo.html('');
    combo.append($('<option></option>').val(null).html('...'));
    $.each(arrayData, function (index, item) {
        combo.append($('<option></option>').val(item[fielId]).html(item[fieldLabel]));
    });
    if (arrayData.length > 1) {
        combo.prop('disabled', false);
    } else {
        if (arrayData.length == 1) {
            combo.val(arrayData[0][fielId]);
            combo.prop('disabled', true);
            combo.change();
        }
    }
}

function setDataOnComboboxSelect(combo, fielId, fieldLabel, arrayData, valueSelect) {
    combo.html('');
    combo.append($('<option></option>').val(null).html('...'));
    $.each(arrayData, function (index, item) {
        if (item[fielId] == valueSelect) {
            combo.append($('<option selected="selected"></option>').val(item[fielId]).html(item[fieldLabel]));
        }
        else {
            combo.append($('<option></option>').val(item[fielId]).html(item[fieldLabel]));
        }
    });
    if (arrayData.length > 1) {
        combo.prop('disabled', false);
    } else {
        if (arrayData.length == 1) {
            combo.val(arrayData[0][fielId]);
            combo.prop('disabled', true);
            combo.change();
        }
    }
}

function formatBolean(value, obj, index) {
    if (value == 1) {
        return '<center><img alt="" src="/Content/Images/accept.png" /></center>';
    } else {
        return '';
    }
}

function formatPrint(value, obj, index) {
    var urlReport = $('#urlReport').val();
    var acronimoFicha = $('#FichaAcronimo').val();
    if (value != null) {
        return '<center><a href="' + urlReport + "?registroFichaId=" + value + '&acronimoFicha=' + acronimoFicha + '" target="_blank"><img alt="" src="/Content/Images/search.png"/></a></center>';
    } else {
        return '';
    }
}

function formatDelete(value, obj, index) {
    var acronimoFicha = $('#FichaAcronimo').val();
    var callFunction = "onClickDelete(" + obj.Id + "," + index + ",'" + acronimoFicha + "')";
    if (value != null) {
        return '<center><img alt="" src="/Content/Images/erase.gif" style="cursor:pointer" onclick="' + callFunction + '"/></center>';
    } else {
        return '';
    }
}

function formatUpdate(value, obj, index) {
    var acronimoFicha = $('#FichaAcronimo').val();
    var callFunction = "onClickUpdate(" + obj.Id + "," + index + ",'" + acronimoFicha + "')";
    if (value != null) {
        return '<center><img alt="" src="/Content/Images/edit.png" style="cursor:pointer" onclick="' + callFunction + '"/></center>';
    } else {
        return '';
    }
}

function onClickDelete(registroFichaId, index, acronimo) {
    var viewMode = $('#deleteViewMode').val();
    if (viewMode === 'modal') {
        var obj = getObjectAtIndexPage(index, $table);
        var tmpl = $('#template-delete').tmpl(obj);
        showConfirmMessage("Confirmación", tmpl, function () {
            eliminarRegistroFichaAjax(acronimo, registroFichaId);
        });
    }
    else if (viewMode === 'window') {

    }
}

function onClickUpdate(registroFichaId, index, acronimo) {
    var viewMode = $('#updateViewMode').val();
    if (viewMode === 'modal') {
        var obj = getObjectAtIndexPage(index, $table);
        var tmpl = $('#template-update').tmpl(obj);
        showModalForm("Editar Registro Ficha", tmpl, function () {
            if (validateEditModalForm()) {
                var data = getDataByEditForm();
                updateRegistroFichaAjax(acronimo, registroFichaId, data);
            }
        });
        $('#dateTimeFechaVisitaEdit').datetimepicker({
            format: 'DD/MM/YYYY'
        });
    }
    else if (viewMode === 'window') {

    }
}

function defaultValidityForm(form) {
    if (form != undefined && form.length > 0) {
        if (form[0].checkValidity()) {
            return true;
        } else {
            $.each(form.find('input'), function (index, element) {
                element.reportValidity();
            });
            return false;
        }
    }
    return false;
}

$('.count').each(function () {
    $(this).prop('Counter', 0).animate({
        Counter: $(this).text()
    }, {
        duration: 3000,
        easing: 'swing',
        step: function (now) {
            $(this).text(Math.ceil(now));
        }
    });
});

//BOOTSTRAP-TABLE ROW FORMATTERS 
function dateFormatter(value, row) {
    var fecha = '-';
    if (value != null && value != undefined) {
        fecha = moment(value).format('DD/MM/YYYY');
    }
    return fecha;
}

function checkFormatter(value) {
    var icon = Boolean(value) ? 'glyphicon-check' : 'glyphicon-unchecked'
    return '<i class="glyphicon ' + icon + '"></i>';
}

function decimalFormatter(value, row) {
    return value.toLocaleString("en-US");
}

function numberFormatter(value, row) {
    return value.toLocaleString("en-US");
}
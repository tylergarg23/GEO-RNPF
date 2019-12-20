$('#modalNuevaPersona').on('show.bs.modal', function () {
    $(this).find('form')[0].reset();
    $("#errorRUC").text("");
    $('#datosNatural').show();
    $('#datosJuridica').hide();
    $('#cargando').hide();
})
$('#modalNuevaPersona').on('hidden.bs.modal', function () {

    //$("#DepartamentoPredio").val("");   



    //$("#DepartamentoPredio option").prop('selected', true);
});
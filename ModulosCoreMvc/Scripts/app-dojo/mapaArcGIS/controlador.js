define(
    //requires:
    ["dojo/_base/declare",
        "dojo/dom",
        "dojo/dom-construct",
        "dijit/_WidgetBase",
        "dijit/_TemplatedMixin",
        "dojo/text!./vista.html",
        "dojo/text!./vistaEvaluar.html",
        "dojo/on",
        'dojo/dom-attr',
        "dojo/dom-style",
        "dojo/query",
        "dijit/registry",
        "esri/widgets/Legend",
        "esri/geometry/Polygon",
        "esri/geometry/geometryEngine",
    ],
    function (declare, dom, domConstruct, _WidgetBase, _TemplatedMixin, vista, vistaEvaluar, on, domAttr, domStyle, query, registry,
        Legend, Polygon, geometryEngine) {

        // PRIVADOS
        var html = null;
        var map = null;
        var canvas = null;
        var capaPredio = null;
        var capaPlantacion = null;
        var capasConsulta = [];
        var capaResultado = null;
        var buffer = null;
        var extentGeometria = null;
        var primeraVez = 0;
        var prediosSuperpuestos = [];
        var estadoGeo = null;
        var servWeb = {
            bp: "http://geo.serfor.gob.pe/geoservicios/rest/services/Visor/Ordenamiento_Forestal/MapServer/0",
            bpp: "http://geo.serfor.gob.pe/geoservicios/rest/services/Visor/Ordenamiento_Forestal/MapServer/1",
            anp: "http://geo.serfor.gob.pe/geoservicios/rest/services/Visor/Ordenamiento_Forestal/MapServer/3",
            cf: "http://geo.serfor.gob.pe/geoservicios/rest/services/Visor/Modalidad_Acceso/MapServer/0",
            bl: "http://geo.serfor.gob.pe/geoservicios/rest/services/Visor/Modalidad_Acceso/MapServer/1",
            cu: "http://geo.serfor.gob.pe/geoservicios/rest/services/Visor/Modalidad_Acceso/MapServer/4",
            br: "http://geo.serfor.gob.pe/geoservicios/rest/services/Visor/Tematico1_Forestal/MapServer/0",
            ef: "http://geo.serfor.gob.pe/geoservicios/rest/services/Visor/Inventario_Ecosistemas_Fragiles/MapServer/0",
            pr: "http://georural.minagri.gob.pe/geoservicios/rest/services/servicios_ogc/Catastro_Rural/MapServer/0"
        };
        var simboloVertice = {
            type: "simple-marker",
            color: [68, 157, 68],
            style: "diamond",
            size: "10px"
        };

        var simboloCentroid = {
            type: "simple-marker",
            color: [68, 157, 68],
            style: "x",
            size: "15px"
        };

        var simboloPlantacion = {
            type: "simple-fill",
            color: [68, 157, 68, 0.35],
            outline: {
                color: [68, 157, 68],
                width: 1
            }
        };


        function creaMapa() {
            var deferred = $.Deferred();
            require(["esri/Map", "esri/views/MapView"], function (Map, MapView) {
                //CREA UN OBJETO MAPA CON UN MAPA BASE POR DEFECTO
                map = new Map({
                    basemap: "hybrid"
                });
                //CREA UNA VISTA DEL MAPA, IDENTIFICA SU LUGAR EN PANTALLA Y CONFIGURACIÓN INICIAL
                canvas = new MapView({
                    container: "canvas",
                    map: map,
                    center: [-75, -10],
                    zoom: 8 //TYLER 12.12.19
                });
                canvas.when(function () {
                    deferred.resolve();
                });

            });
            return deferred.promise();
        };
        function creaCapas() {
            var deferred = $.Deferred();
            capasConsulta = [];
            require(["esri/layers/FeatureLayer", "esri/layers/GraphicsLayer"], function (FeatureLayer, GraphicsLayer) {
                capaPredio = new GraphicsLayer();
                capaPlantacion = new GraphicsLayer();
                capasConsulta.push(new FeatureLayer({
                    url: servWeb.cf,
                    outFields: ["CONTRA", "FECONT", "NOMTIT", "NUMRUC", "FUENTE"],
                    visible: false
                }));
                capasConsulta.push(new FeatureLayer({
                    url: servWeb.anp,
                    visible: true
                    //outFields: ["*"],
                }));

                capasConsulta.push(new FeatureLayer({
                    url: servWeb.bpp,
                    visible: false
                    //outFields: ["*"],
                }));

                capasConsulta.push(new FeatureLayer({
                    url: servWeb.bp,
                    visible: false, //TYLER 12.12.19
                    outFields: ["ZONA"] //TYLER 12.12.19
                }));

                capasConsulta.push(new FeatureLayer({
                    url: servWeb.bl,
                    visible: false
                    //outFields: ["*"],
                }));

                capasConsulta.push(new FeatureLayer({
                    url: servWeb.cu,
                    visible: false
                    //outFields: ["*"],
                }));

                capasConsulta.push(new FeatureLayer({
                    url: servWeb.br,
                    visible: false
                    //outFields: ["*"],
                }));

                capasConsulta.push(new FeatureLayer({
                    url: servWeb.ef,
                    visible: false
                    //outFields: ["*"],
                }));

                capasConsulta.push(new FeatureLayer({
                    url: servWeb.pr,
                    visible: true,
                    outFields: ["cod_predio"]
                    //outFields: ["*"],                   
                }));
     
                capaResultado = new GraphicsLayer();
                deferred.resolve();
            });
            return deferred.promise();
        };
        function botonesCapas() {
            var opciones = domConstruct.toDom("<div class='row'><div class='col-md-12'><input type='checkbox' id='btnPlantacion' checked> <a href='#' id='zoomPlantacion' >Plantación</a> </input></div><div class='col-md-12'><input type='checkbox' id='btnModAcceso'>Concesiones Forestales</input></div><div class='col-md-12'><input type='checkbox' id='btnANP'>Áreas Naturales Protegidas</input></div><div class='col-md-12'><input type='checkbox' id='btnCM'>Catastro Mínero</input><div class='col-md-12'><input type='checkbox' id='btnBPP'>Bosques de Producción Permanente</input><div class='col-md-12'><input type='checkbox' id='btnBP'>Bosques Protectores</input></div><div class='col-md-12'><input type='checkbox' id='btnBL'>Bosques Locales</input></div><div class='col-md-12'><input type='checkbox' id='btnCU'>Cesiones en uso</input></div><div class='col-md-12'><input type='checkbox' id='btnBR'>Bosques de Reserva</input></div><div class='col-md-12'><input type='checkbox' id='btnEF'>Ecosistemas Frágiles</input></div><div class='col-md-12'><input type='checkbox' id='btnPR'>Predios Rurales</input></div></div></div>");

            domConstruct.place(opciones, "layerToggle");

            var btnModAcceso = dom.byId("btnModAcceso");
            var btnANP = dom.byId("btnANP");
            var btnBPP = dom.byId("btnBPP");
            var btnBP = dom.byId("btnBP");
            var btnBL = dom.byId("btnBL");
            var btnCU = dom.byId("btnCU");
            var btnBR = dom.byId("btnBR");
            var btnEF = dom.byId("btnEF");
            var btnPR = dom.byId("btnPR");
            on(btnModAcceso, "change", function () {
                capasConsulta[0].visible = btnModAcceso.checked;
            });
            on(btnANP, "change", function () {
                capasConsulta[1].visible = btnANP.checked;
            });
            on(btnBPP, "change", function () {
                capasConsulta[3].visible = btnBPP.checked;
            });
            on(btnBP, "change", function () {
                capasConsulta[4].visible = btnBP.checked;
            });
            on(btnBL, "change", function () {
                capasConsulta[5].visible = btnBL.checked;
            });
            on(btnCU, "change", function () {
                capasConsulta[6].visible = btnCU.checked;
            });
            on(btnBR, "change", function () {
                capasConsulta[7].visible = btnBR.checked;
            });
            on(btnEF, "change", function () {
                capasConsulta[8].visible = btnEF.checked;
            });
            on(btnPR, "change", function () {
                capasConsulta[9].visible = btnPR.checked;
            });
        }
        function cargaToolBar(div) {
            console.log(capaPredio.count);
            var toolbar = domConstruct.toDom("<span id='layerToggle'><div id='toolbar' class='row'><div class='col-md-12'><input type='checkbox' id='btnPlantacion' checked> <a href='#' id='zoomPlantacion' >Plantación</a> </input></div></div></span>");
            domConstruct.place(toolbar, div);
            if (capaPredio.graphics.items.length !== 0) {
                let botonCapaPredio = domConstruct.toDom("<div class='row'><div class='col-md-12'><input type='checkbox' id='btnPredio' checked> Centroide Predio</input></div></div>");
                domConstruct.place(botonCapaPredio, "layerToggle");
                var btnPredio = dom.byId("btnPredio");
                on(btnPredio, "change", function () {
                    capaPredio.visible = btnPredio.checked;
                });
            }

            var btnPlantacion = dom.byId("btnPlantacion");

            var zoomPlantacion = dom.byId("zoomPlantacion");
            on(btnPlantacion, "change", function () {
                capaPlantacion.visible = btnPlantacion.checked;
            });
            on(zoomPlantacion, "click", function () {
                canvas.goTo(extentGeometria);
            });
        }
        function addResultadoToolBar() {

            if (dom.byId("btnResultado") === undefined || dom.byId("btnResultado") === null) {
                var botonCapa = domConstruct.toDom("<div class='row'><div class='col-md-12'><input type='checkbox' id='btnResultado' checked> Resultado</input></div></div>");
                domConstruct.place(botonCapa, "layerToggle");
                var btnResultado = dom.byId("btnResultado");
                on(btnResultado, "change", function () {
                    capaResultado.visible = btnResultado.checked;
                });
            }
        }
        function cargaCapas() {
            map.addMany(capasConsulta);
            botonesCapas();
            canvas.whenLayerView(capasConsulta[0]).then(function (capa) {
                capa.watch("updating", function (val) {
                    if (val === undefined) {
                        console.log('cargo concesiones!!!!');
                    }
                });
            }).otherwise(function (error) {

            });
            canvas.whenLayerView(capasConsulta[1]).then(function (capa) {
                capa.watch("updating", function (val) {
                    if (val === undefined) {
                        console.log('cargo areas!!!!');
                    }
                });
            }).otherwise(function (error) {

            });
            canvas.whenLayerView(capasConsulta[2]).then(function (capa) {
                capa.watch("updating", function (val) {
                    if (val === undefined) {
                        console.log('cargo mineras!!!!');
                    }
                });
            }).otherwise(function (error) {

            });
        };
        function cargaLeyenda() {
            var legend = new Legend({
                view: canvas,
                layerInfos: [
                    {
                        layer: capasConsulta[0],
                        title: 'Áreas Naturales Protegidas'
                    },
                    {
                        layer: capasConsulta[1],
                        title: 'Concesiones Forestales'
                    },
                    {
                        layer: capasConsulta[2],
                        title: 'Concesiones Míneras'
                    },
                    {
                        layer: capasConsulta[3],
                        title: 'Bosques de Producción Permanente'
                    },
                    {
                        layer: capasConsulta[4],
                        title: 'Bosques Protectores'
                    },
                    {
                        layer: capasConsulta[5],
                        title: 'Bosques Locales'
                    },
                    {
                        layer: capasConsulta[6],
                        title: 'Cesiones en uso'
                    },
                    {
                        layer: capasConsulta[7],
                        title: 'Bosques de Reserva'
                    },
                    {
                        layer: capasConsulta[8],
                        title: 'Ecosistemas Frágiles'
                    },
                    {
                        layer: capasConsulta[9],
                        title: 'Predios Rurales'
                    }
                ]
            });
            canvas.ui.add(legend, "bottom-right");
        }
        function cargaGraficoCapa() {
            var deferred = $.Deferred();
            if (tipoVent !== "bloque") {
                getBloques().then(addBloquesGrafico).then(function () {
                    getCentroidePredio().then(addPredioGrafico).then(function () {
                        deferred.resolve();
                    });

                });
            }
            else {
                getCoordenadas().then(addGrafico).then(function () {
                    deferred.resolve();
                });
            }
            return deferred.promise();
        }
        function getBloques() {
            return $.ajax({
                url: url_serweb,
                data: { plantacionId: $('#PlantacionId').val() },
                type: "GET"
            });
        }
        function getCentroidePredio() {
            return $.ajax({
                url: url_serweb_predio,
                data: { plantacionId: $('#PlantacionId').val() },
                type: "GET"
            });
        }
        function getCoordenadas() {
            return $.ajax({
                url: url_serweb,
                data: { bloqueId: $('#BloqueId').val() },
                type: "GET"
            });
        }
        function addBloquesGrafico(lista_bloques) {
            var deferred = $.Deferred();
            $("#numBloquesLabel").html(lista_bloques.length);
            extentGeometria = null;
            //geometrias = [];
            lista_bloques.map(function (bloque, i) {

                require(["esri/Graphic"], function (Graphic) {

                    var forma = [];
                    let nombreBloque = '';
                    var pieza = domConstruct.toDom('<tr><td colspan="2"><strong>' + bloque.Nombre.toUpperCase() + '</strong> (' + bloque.Coordenadas.length + ' vértices)</td></tr>');
                    domConstruct.place(pieza, "listBloquesBody");

                    bloque.Coordenadas.map(function (coordenada, i) {

                        pieza = domConstruct.toDom('<tr><th rowspan="2"><strong>' + coordenada.CoordenadaGeografica.NombreLocation + '</strong></th>' +
                            '<td>' + coordenada.CoordenadaEsteUTM + ' E - ' + coordenada.CoordenadaNorteUTM + 'N</td>' +
                            '</tr><tr><td>' + coordenada.CoordenadaGeografica.Latitud + ' lat. ' + coordenada.CoordenadaGeografica.Longitud + ' lng.</td></tr>');
                        domConstruct.place(pieza, "listBloquesBody");
                        //Construye Poligono
                        var punto = [coordenada.CoordenadaGeografica.Longitud, coordenada.CoordenadaGeografica.Latitud];
                        forma.push(punto);

                        //Construye los puntos y coloca en capa
                        var point = {
                            type: "point",
                            x: coordenada.CoordenadaGeografica.Longitud,
                            y: coordenada.CoordenadaGeografica.Latitud,
                        };
                        var pointGraphic = new Graphic({
                            geometry: point,
                            symbol: simboloVertice,
                            attributes: {
                                Nombre: coordenada.CoordenadaGeografica.Nombre,
                                Punto: coordenada.CoordenadaGeografica.NombreLocation,
                                UTM_X: coordenada.CoordenadaEsteUTM,
                                UTM_Y: coordenada.CoordenadaNorteUTM,
                                GEO_X: coordenada.CoordenadaGeografica.Latitud,
                                GEO_Y: coordenada.CoordenadaGeografica.Longitud
                            },
                            popupTemplate: {
                                title: "{Nombre} - {Punto}",
                                content: '<div class="infoDiv">' +
                                    '<table class="table table-condensed"><tbody>' +
                                    '<tr><td><span class="glyphicon glyphicon-globe" aria-hidden="true"></span>&nbsp;UTM wgs84:</td><td colspan="2">{UTM_X} E -' +
                                    ' {UTM_Y} N</td></tr>' +
                                    '<tr><td rowspan="2"><span class="glyphicon glyphicon-map-marker" aria-hidden="true"></span>&nbsp;Geográficas:</td><td>{GEO_X} lat.</td></tr>' +
                                    '<tr><td>{GEO_Y} lng.</td></tr>' +
                                    '</tbody></table>' +
                                    '</div>'
                            }
                        });

                        nombreBloque = coordenada.CoordenadaGeografica.Nombre;
                        capaPlantacion.add(pointGraphic);
                    });

                    var poligono = {
                        type: "polygon", // autocasts as new Polygon()
                        rings: forma
                    };
                    //geometrias.push(poligono);
                    var grafico = new Graphic({
                        geometry: poligono,
                        symbol: simboloPlantacion
                    });
                    capaPlantacion.add(grafico);
                    //Construye los puntos y coloca en capa
                    let centroide = {
                        type: "point",
                        x: grafico.geometry.centroid.x,
                        y: grafico.geometry.centroid.y
                    };
                    let centroideGraphic = new Graphic({
                        geometry: centroide,
                        symbol: simboloVertice,
                        attributes: {
                            Nombre: 'Centroide de Bloque: ' + nombreBloque,
                            GEO_X: centroide.y,
                            GEO_Y: centroide.x
                        },
                        popupTemplate: {
                            title: "{Nombre}",
                            content: '<div class="infoDiv">' +
                                '<table class="table table-condensed"><tbody>' +
                                '<tr><td rowspan="2"><span class="glyphicon glyphicon-map-marker" aria-hidden="true"></span>&nbsp;Geográficas:</td><td>{GEO_X} lat.</td></tr>' +
                                '<tr><td>{GEO_Y} lng.</td></tr>' +
                                '</tbody></table>' +
                                '</div>'
                        }
                    });
                    capaPlantacion.add(centroideGraphic);

                    if (extentGeometria) {
                        extentGeometria.union(grafico.geometry.extent.expand(1.5));
                    }
                    else
                        extentGeometria = grafico.geometry.extent.expand(1.2); //TYLER 19.12.19

                });
            });
            capaPlantacion.when(function () {
                //canvas.extent = extentGeometria;
                canvas.goTo(extentGeometria);
                deferred.resolve();
            });
            return deferred.promise();
        }
        function addPredioGrafico(coordenada) {
            var deferred = $.Deferred();
            require(["esri/Graphic"], function (Graphic) {

                if (coordenada.CoordenadaGeografica !== null) {

                    //Construye los puntos y coloca en capa
                    var point = {
                        type: "point",
                        x: coordenada.CoordenadaGeografica.Longitud,
                        y: coordenada.CoordenadaGeografica.Latitud
                    };
                    var pointGraphic = new Graphic({
                        geometry: point,
                        symbol: simboloCentroid,
                        attributes: {
                            Nombre: coordenada.CoordenadaGeografica.Nombre,
                            Punto: coordenada.CoordenadaGeografica.NombreLocation,
                            UTM_X: coordenada.CoordenadaEsteUTM,
                            UTM_Y: coordenada.CoordenadaNorteUTM,
                            GEO_X: coordenada.CoordenadaGeografica.Latitud,
                            GEO_Y: coordenada.CoordenadaGeografica.Longitud
                        },
                        popupTemplate: {
                            title: "{Nombre} - {Punto}",
                            content: '<div class="infoDiv">' +
                                '<table class="table table-condensed"><tbody>' +
                                '<tr><td><span class="glyphicon glyphicon-globe" aria-hidden="true"></span>&nbsp;UTM wgs84:</td><td colspan="2">{UTM_X} E -' +
                                ' {UTM_Y} N</td></tr>' +
                                '<tr><td rowspan="2"><span class="glyphicon glyphicon-map-marker" aria-hidden="true"></span>&nbsp;Geográficas:</td><td>{GEO_X} lat.</td></tr>' +
                                '<tr><td>{GEO_Y} lng.</td></tr>' +
                                '</tbody></table>' +
                                '</div>'
                        }
                    });
                    capaPredio.add(pointGraphic);
                }


                deferred.resolve();

            });

            return deferred.promise();
        }
        function addGrafico(coordenadas) {
            var deferred = $.Deferred();
            var cantidadPuntos = coordenadas.length;
            var forma = [];
            //geometrias = [];
            if (cantidadPuntos > 0) {
                require(["esri/Graphic"], function (Graphic) {
                    $("#numVerticesLabel").html(cantidadPuntos);

                    var pieza = domConstruct.toDom('<tr><th><strong>Nombre</strong></th><td><strong>Coordenadas UTM/Geog.</strong></td></tr>');
                    domConstruct.place(pieza, "listVerticesBody");


                    coordenadas.map(function (coordenada, i) {

                        pieza = domConstruct.toDom('<tr><th rowspan="2"><strong>' + coordenada.CoordenadaGeografica.NombreLocation + '</strong></th>' +
                            '<td>' + coordenada.CoordenadaEsteUTM + ' E - ' + coordenada.CoordenadaNorteUTM + 'N</td>' +
                            '</tr><tr><td>' + coordenada.CoordenadaGeografica.Latitud + ' lat. ' + coordenada.CoordenadaGeografica.Longitud + ' lng.</td></tr>');
                        domConstruct.place(pieza, "listVerticesBody");
                        //Construye Poligono
                        var punto = [coordenada.CoordenadaGeografica.Longitud, coordenada.CoordenadaGeografica.Latitud];
                        forma.push(punto);

                        //Construye los puntos y coloca en capa
                        var point = {
                            type: "point",
                            x: coordenada.CoordenadaGeografica.Longitud,
                            y: coordenada.CoordenadaGeografica.Latitud,
                        };
                        var pointGraphic = new Graphic({
                            geometry: point,
                            symbol: simboloVertice,
                            attributes: {
                                Nombre: coordenada.CoordenadaGeografica.Nombre,
                                Punto: coordenada.CoordenadaGeografica.NombreLocation,
                                UTM_X: coordenada.CoordenadaEsteUTM,
                                UTM_Y: coordenada.CoordenadaNorteUTM,
                                GEO_X: coordenada.CoordenadaGeografica.Latitud,
                                GEO_Y: coordenada.CoordenadaGeografica.Longitud
                            },
                            popupTemplate: {
                                title: "{Nombre} - {Punto}",
                                content: '<div class="infoDiv">' +
                                    '<table class="table table-condensed"><tbody>' +
                                    '<tr><td><span class="glyphicon glyphicon-globe" aria-hidden="true"></span>&nbsp;UTM wgs84:</td><td colspan="2">{UTM_X} E -' +
                                    ' {UTM_Y} N</td></tr>' +
                                    '<tr><td rowspan="2"><span class="glyphicon glyphicon-map-marker" aria-hidden="true"></span>&nbsp;Geográficas:</td><td>{GEO_X} lat.</td></tr>' +
                                    '<tr><td>{GEO_Y} lng.</td></tr>' +
                                    '</tbody></table>' +
                                    '</div>'
                            }
                        });


                        capaPlantacion.add(pointGraphic);
                    });

                    var poligono = {
                        type: "polygon", // autocasts as new Polygon()
                        rings: forma
                    };
                    //geometrias.push(poligono);
                    var grafico = new Graphic({
                        geometry: poligono,
                        symbol: simboloPlantacion
                    });
                    capaPlantacion.add(grafico);
                    capaPlantacion.when(function () {
                        extentGeometria = grafico.geometry.extent.expand(1.5);
                        canvas.goTo(extentGeometria);
                        deferred.resolve();
                    });
                });

            } else {
                showNotification(constantes.tipoAlerta.INFO, "Mensaje", "No hay resultados para mostrar");
            }
            return deferred.promise();
        }
        function addEventoEvaluar() {
            var btnEvaluar = dom.byId('evaluar');
            var oProgress = dom.byId("progress");
            on(btnEvaluar, "click", function () {
                domAttr.set(btnEvaluar, 'disabled', true);
                domAttr.set(oProgress, 'hidden', false);
                evaluaSuperposicion().then(function () {
                    domAttr.set(btnEvaluar, 'disabled', false);
                    domAttr.set(oProgress, 'hidden', true);                 
                });
              

            });

            if (primeraVez == 0) {
                primeraVez++;
                btnEvaluar.click();

            }
            
        }


        function validarPredio(predio) {

            var cantidadPredios = prediosSuperpuestos.length;
            var estado = '';
            predioCorrecto = false;

            if (cantidadPredios == 1) {
                var predioSuperpuesto = prediosSuperpuestos[0];
                predioCorrecto = predio === predioSuperpuesto;
            }

            else {
                predioCorrecto = !cantidadPredios;
            }

            estado = predioCorrecto ? "OK" : "FAIL";

            return estado;
        }


        var respuestaCapas = []; //agrega un elemento cuando la promesa se resuelva
        function verificarEvaluacionCapas() {

            // si el numero de respuestas es igual al numero de capas 
            // ya finalizaron todas las promesas para poder anzar la validacion del predio
            var cantCapas = capasConsulta.length;
            var cantRespuesta = respuestaCapas.length;

            if (cantCapas === cantRespuesta) {

                if (tipoVent === 'plantacion') {
                    estadoGeo = validarPredio(predioPlantacion);
                    $.ajax({

                        type: 'POST',
                        url: url_update_geo_status,
                        data: { plantacionId: $('#PlantacionId').val(), estadoGeo: estadoGeo},
                        dataType: 'json',
                        success: function (result) {

                            console.log('UPDATE GEO STATUS');

                        },
                        error: function(ex){

                        }
                    });
                }

            }
        }

        function evaluaSuperposicion() {
            var deferred = $.Deferred();
            getGeometrias().then(function (geometrias) {

                getBuffer(geometrias).then(function () {

                    clearAcciones();
                    capasConsulta.forEach(function (capa, i) {


                        consultaEspacial(capa, buffer).then(muestraResultados).then(function (contador) {
                            writeAccion(capa.title, contador);
                            if (contador > 0) addResultadoToolBar();
                            respuestaCapas.push(1);
                            verificarEvaluacionCapas();

                        }, function (error) {
                                writeAccion(capa.title, 0);
                                respuestaCapas.push(0)
                            // This function will execute if the promise is rejected due to an error
                        });
                    });
                    deferred.resolve();
                });
            });
            //capaPlantacion.graphics.items[4].geometry.type                                 
            return deferred.promise();
        }
        function clearAcciones() {
            var divResultado = dom.byId('resulEvaluacion');
            var pieza = domConstruct.toDom('<div id="resulEvaluacion"></div >');
            domConstruct.place(pieza, divResultado, "replace");
        }
        function writeAccion(nom_capa, numero) {
            var divResultado = dom.byId('resulEvaluacion');
            var html = '<div class="botton16"><div class="row"><div class="col-md-12">Se evaluó la capa <b>' + nom_capa + '</b></div></div>'
            //var pieza = domConstruct.toDom();
            if (numero > 0) {
                html = html + '<div class="row"><div class="col-md-12 rojo">Existe superposición con ' + numero + ' poligonos de esta capa</div></div></div>';
            }
            else {
                html = html + '<div class="row"><div class="col-md-12 verde">No existe ninguna superposición con esta capa</div></div></div>';
            }
            var pieza = domConstruct.toDom(html);
            domConstruct.place(pieza, divResultado);
        }

        function consultaEspacial(capa, buffer) {
            var query = capa.createQuery();
            query.geometry = buffer;
            query.spatialRelationship = "intersects";

            return capa.queryFeatures(query);
        }
        function muestraResultados(results) {
            var deferred = $.Deferred();
            //var num = (results.displayField === "Fuente" ? 0 : (results.displayField === "CODIGO" ? 1 : 2));
            var num = (results.displayField === "cod_predio" ? 0 : results.displayField === "CODIGO" ? 1 : 2);
            //capaResultado.removeAll();
            var features = results.features.map(function (graphic) {

                graphic.popupTemplate = templateCapa(num);

                graphic.symbol = symbolCapa(num);

                prediosSuperpuestos.push(graphic.attributes.num_predio); 
                return graphic;
            });
            //var numGraficos = features.length;
            //dom.byId("results").innerHTML = numQuakes + " earthquakes found";           
            capaResultado.addMany(features);
            deferred.resolve(features.length);
            return deferred.promise();
        }
        function templateCapa(num) {
            var template = null;
            switch (num) {
                /*case 0:
                    template = {
                        title: "Detalle de Concesión",
                        content: '<div class="row"><div class="col-md-3">Contrato</div><div class="col-md-9">{CONTRA}</div></div>' +
                        '<div class="row"><div class="col-md-3">Fecha</div><div class="col-md-9">{FECONT}</div></div>' +
                        '<div class="row"><div class="col-md-3">Títular</div><div class="col-md-9">{NOMTIT}</div></div>' +
                        '<div class="row"><div class="col-md-3">RUC</div><div class="col-md-9">{NUMRUC}</div></div>' +
                        '<div class="row"><div class="col-md-3">Fuente</div><div class="col-md-9">{FUENTE}</div></div>',
                            fieldInfos: [{
                                fieldName: "FECONT",
                                format: {
                                    //dateString: { local: true, systemLocale:true},
                                    dateFormat: "short-date"
                                }
                            }]                        
                    };
                    break;*/
                case 0:
                    template = {
                        title: "Puesto Privado",
                        content: '<div class="row"><div class="col-md-3">Codigo privad</div><div class="col-md-9">{cod_predio}</div></div>',

                    };
                    break;

                case 1:
                    template = {
                        title: "Área Natural Protegida: ",
                        content: '<div class="row"><div class="col-md-3">Titular</div><div class="col-md-9"></div></div>'
                    };
                    break;
                /*case 2:
                    template = {
                        title: "Catastro Minero: {CODIGOU}",
                        content: '<div class="row"><div class="col-md-3">Titular</div><div class="col-md-9">{TIT_CONCES}</div></div>' +
                        '<div class="row"><div class="col-md-3">Concesión</div><div class="col-md-9">{CONCESION}</div></div>' +
                        '<div class="row"><div class="col-md-3">Hectareas</div><div class="col-md-9">{HECTAGIS}</div></div>' +
                        '<div class="row"><div class="col-md-3">Estado</div><div class="col-md-9">{LEYENDA}</div></div>'
                    };
                    break;*/
                case 2:
                    template = {
                        title: "Predio Rural",
                        content: '<div class="row"><div class="col-md-3">Número del predio</div><div class="col-md-9"></div></div>' +
                            '<div class="row"><div class="col-md-3">num_predio</div><div class="col-md-9">{num_predio}</div></div>' +
                            '<div class="row"><div class="col-md-3">cod_predio</div><div class="col-md-9">{cod_predio}</div></div>' +
                            '<div class="row"><div class="col-md-3">area_ha</div><div class="col-md-9">{area_ha}</div></div>' +
                            '<div class="row"><div class="col-md-3">perimetro</div><div class="col-md-9">{perimetro}</div></div>' //TYLER 12.12.19
                    };
                    break;
            }
            return template;
        }
        function symbolCapa(num) {
            var symbolo = null;
            switch (num) {
                case 0:
                    symbolo = {
                        type: "simple-fill",
                        color: [157, 68, 100, 0.35],
                        outline: {
                            color: [157, 68, 100],
                            width: 1
                        }
                    };
                    break;
                case 1:
                    symbolo = {
                        type: "simple-fill",
                        color: [100, 68, 157, 0.35],
                        outline: {
                            color: [100, 68, 157],
                            width: 1
                        }
                    };
                    break;
                case 2:
                    symbolo = {
                        type: "simple-fill",
                        color: [157, 68, 157, 0.35],
                        outline: {
                            color: [157, 68, 157],
                            width: 1
                        }
                    };
                    break;
            }
            return symbolo;
        }
        function getGeometrias() {
            var deferred = $.Deferred();
            var geometrias = [];
            var count = capaPlantacion.graphics.items.length;
            capaPlantacion.graphics.items.forEach(function (grafico, i) {
                if (grafico.geometry.type === "polygon") {
                    geometrias.push(grafico.geometry);
                }
                if (i === count - 1) deferred.resolve(geometrias);
            });
            return deferred.promise();
        }
        function getBuffer(geometrias) {
            var deferred = $.Deferred();
            var buffers = geometryEngine.geodesicBuffer(geometrias, [
                1
            ], "meters",
                true);
            buffer = buffers[0];
            deferred.resolve();
            return deferred.promise();
        }

        function cargaTabEvaluar() {
            var deferred = $.Deferred();
            var contenido = domConstruct.toDom(vistaEvaluar);
            var lugarContenido = query(".bhoechie-tab")[0];
            domConstruct.place(contenido, lugarContenido);
            var menu3 = query(".list-group-item")[2];
            domStyle.set(menu3, "display", "block");
            deferred.resolve();
            return deferred.promise();
        }
        return declare("app-dojo/mapaArcGIS/controlador", null, {

            constructor: function (div, boton) {
                //Construye Vista
                html = domConstruct.toDom(vista);
                domConstruct.place(html, div, "replace");
                //if (boton != undefined) addEventoEvaluar(boton);
                $.when(creaMapa(), creaCapas()).then(function () {
                    map.add(capaPredio);
                    map.add(capaPlantacion);
                    map.addMany(capasConsulta);
                    map.add(capaResultado);
                    cargaGraficoCapa().then(function () {
                        cargaToolBar(div);
                        cargaTabEvaluar().then(function () {
                            addEventoEvaluar();
                        });
                    });
                    //cargaCapas();
                    //cargaLeyenda();                                        
                });

                //$.when(creaMapa, creaCapas).then(function () {
                //        cargaCapas();
                //});                        
            },
            destroy: function () {

            }
        });
    });
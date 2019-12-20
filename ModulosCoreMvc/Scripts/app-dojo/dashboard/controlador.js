define(
    //requires:
    [   "dojo/_base/declare",
        "dojo/dom",
        "dojo/dom-construct",
        //"dijit/_WidgetBase",
        //"dijit/_TemplatedMixin",
        //"dojo/text!./vista.html",                
        "dojo/on",
        'dojo/dom-attr',
        "dojo/dom-style",
        "dojo/query",
        //"dijit/registry",                        
    ],
    function (declare, dom, domConstruct,
        //_WidgetBase, _TemplatedMixin, vista,
        on, domAttr, domStyle, query
    //    registry
    ) {

       
        return declare("app-dojo/dashboard/controlador", null, {

            constructor: function (div) {
                var greetingNode = dom.byId('greeting');
                domConstruct.place('<em> Dojo!</em>', greetingNode);
                //Construye Vista
                //html = domConstruct.toDom(vista);
                //domConstruct.place(html, div, "replace");

                                                   
            }      
        });
    });
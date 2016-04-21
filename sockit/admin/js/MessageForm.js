/**
 * @param socket
 * @param container
 * @param elementTemplate
 * @constructor
 */
var MessageForm = function ( socket, container, elementTemplate ) {

    this._socket = socket;
    this._container = container;
    this._elementTemplate = elementTemplate;
};

MessageForm.prototype = {


    _bindInputHandlers: function () {

        var container = this._container;

        Array.prototype.forEach.call( this._container.querySelectorAll('input[type=range]'), function ( range ) {

            var updateValue = function ( to ) {
                container.querySelector( '#'+ range.id +'-value' ).innerHTML = to;
            };

            range.addEventListener('input', function () {
                updateValue( this.value );
            });

            updateValue( range.value );
        });
    },

    _bindSendHandlers: function () {

        var socket = this._socket;
        var container = this._container;

        Array.prototype.forEach.call( this._container.querySelectorAll('button'), function ( button ) {

            button.addEventListener('click', function () {

                var args = [];
                var argInputs = container.querySelectorAll('input[id^='+ button.id +'], select[id^='+ button.id +']' );

                if ( /has-arguments/.test( button.className ) ) {

                    Array.prototype.forEach.call( argInputs
                        ,
                        function ( formField ) {

                            if ( formField.tagName.toLowerCase() == 'select' ) {

                                args.push( formField.options[ formField.selectedIndex ].value );

                            } else if ( /json/.test( formField.className ) ) {

                                var getObject = eval("(function () { return "+ formField.value +"; })");
                                args.push(  getObject() );

                            } else {
                                args.push( formField.value );
                            }

                            if ( argInputs.length === 1 ) {
                                args = args[0];
                            }
                        }
                    );

                    socket.emit( button.id, args );
                    
                } else {
                    socket.emit( button.id );
                }

            });
        });
    },

    _createFormElement: function ( message, messageDetails ) {

        var parent = document.createElement('div');
        var viewModel = {
            name: message
        };

        if ( messageDetails.arguments ) {

            viewModel.hasArguments = true;
            viewModel.arguments = [];

            for ( var a= 0, al = messageDetails.arguments.length; a<al; a++ ) {
                viewModel.arguments.push( this._getInputViewModel( messageDetails.arguments[ a ] ) );
            }
        }

        parent.innerHTML = this._elementTemplate( viewModel );

        for ( var i = 0, il = parent.childNodes.length; i < il; i++ ) {

            this._container.appendChild( parent.childNodes[ i ].cloneNode(true) );
        }
    },

    _getInputViewModel: function ( inputConfig ) {

        var viewModel = {};

        for ( var prop in inputConfig ) {
            if ( inputConfig.hasOwnProperty( prop ) ) {
                viewModel[ prop ] = inputConfig[ prop ];
            }
        }

        switch ( inputConfig.type ) {

            case 'range':
                viewModel.isRange = true;
                break;

            case 'select':
                viewModel.isSelect = true;
                viewModel.options = inputConfig.options.map( function ( option ) {

                    return {
                        name: option,
                        selected: inputConfig.default == option
                    };
                });
                break;
        }

        return viewModel;
    },

    parse: function ( messages ) {

        for ( var message in messages ) {
            this._createFormElement( message, messages[ message ] );
        }

        this._bindInputHandlers();
        this._bindSendHandlers();
    }
};
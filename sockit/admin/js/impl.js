(function ( MessageForm, messageConfig ) {

    var socket = io( document.location.origin );
    var messages = messageConfig.messages;
    var elementTemplate = Handlebars.compile( document.getElementById('form-element' ).innerHTML );

    var messageForm = new MessageForm( socket, document.querySelector('.messages'), elementTemplate );

    messageForm.parse( messages );

})( MessageForm, config );
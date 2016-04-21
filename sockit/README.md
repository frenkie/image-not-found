# sockit
When you want clients to communicate with each other through messages
with optionally handled arguments, use sockit. It saves you the hassle of
copy pasting all that socket.io juice.

It even provides you with an admin page to quickly pass on every 
available message, with arguments.

## install & run
Copy `config.example.json` to `config.json` and adjust the messages
to your needs.

Run `npm start` and your set! Any socket.io client can now make
use of this easy configured server.

### admin
You automagically get an admin page to fake a client and tinker with
the arguments and their values, just visit [localhost:8080/admin](http://localhost:8080/admin)

### ip & port
By default sockit uses `process.env.IP` and `process.env.PORT` with
a fallback to ip '0.0.0.0' and port 8080. 

## message configuration
Message configuration is done in `config.json` through a JSON object with the following
properties:

```javascript

{
    "messages": {
        
        // message-name will be the name you can listen for on the socket
        "message-name": {
            
            // optional array of arguments, will generate input fields on the admin page,
            // and an array (or just the argument value in case of 1 argument) will be 
            // passed on as the data of the message.
            // Note that sockit actually doesn't care what you send and won't parse it.
            // The type is only needed for the admin page to render the proper input fields
            // adn in case of a 'json' type eval'ing the input value on that page.
            "arguments" : [                
                {
                  "name": "argument1",
                  "type": "string"
                },
                {
                  "name": "argument2",
                  "type": "number"
                },
                {
                  "name": "argument3",
                  "type": "range",
                  "min": 0,
                  "max": 100,
                  "step": 1,
                  "default": 50
                },
                {
                  "name": "argument4",
                  "type": "select",
                  "options": ["one", "two", "three", "four"],
                  "default": "two"
                },
                {
                  "name": "argument5",
                  "type": "json" // will eval the input string on the admin page as a JS object
                                 // So you can input {key: 'value'} or {"key":"value"}, that's up to you
                },                
            ],
            
            // optional function to handle the above arguments serverside 
            // before broadcasting them. Function should return the
            // resulting data to send, which can be different from the input
            "handleArguments": "(function handle () { return Array.prototype.join.apply( arguments || [], [':'] ); })"            
        }
    }
}

```


## developing & debugging
You can also run a debug version of the server with debug output and
live reloading of the server when file contents change.

Just do `npm run debug` and tada!

## client example
Need a quick copy pastable client example? Here's an HTML/JavaScript one
based on ip and port configurations of sockit, adjust where needed:
 
```html

<script src="http://localhost:8080/socket.io/socket.io.js"></script>
<script>

    var socket = io( 'http://localhost:8080' );
    
    socket.on('message-name', function ( data ) {
    
    });

</script>
 
``` 

## versions

### 1.0.0
Added json type arguments. Mostly useful in the admin page.
When dealing with a message that just has 1 argument, the value of that argument
will be passed on, instead of an array with 1 value like in version 0.1.

### 0.1.0
First draft

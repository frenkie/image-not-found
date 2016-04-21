# Hackastory - Image Not Found

Image Not Found is a location based audio play (hoorspel in Dutch), where you
experience a short news story blindfolded with only audio cues.

# Contents

1.  `image-not-found/` - The main Unity game
2.  `deviceorientation/` - The (iOs) mobile app to deliver head orientation to the game
3.  `sockit/` - Socket message server for communication between game and iOS app.

# Image Not Found
The 'location based' audio play game, which triggers an audio story by hitting audio
cues. You can experience the game physically or in the browser.

## installation
You need to have Unity installed in order to open the project. All necessary plugins 
are included.

## deployment for a desktop/physical game
There are 2 scenes in Image Not Found:

1.  room
2.  browser-version

The room scene is a standalone version you can use just by pressing 'play' in
Unity. You can let somebody walk in a physical space with a blindfold and headphones
on.
You can use your mouse to move the red player on the floor (press the left mouse), 
which is the fake GPS way currently possible. For head orientation which helps with the 3D
Unity audio, you do need the deviceorientation app and the sockit server.


## deployment for the browser
You can generate a browser verson which mimicks what the physical game does.
In the build settings of the project, add the browser-version scene to export a
version of the game you can play in the browser with just your headphones and
keyboard+mouse.



# Device orientation
It's actually nothing more than a 360 mobile app, that communicates it's y-axis 
rotation to a socket server, broadcasting it to any one who's listening.

## installation
You need to have Unity installed in order to open the project. All necessary device
orientation plugins are included.

## development
The script sending the rotation socket event is a component of the VideoSphere object.
The script where the GUI is popped up from is connected to the Socket.io object. 

## deployment
You can deploy it as an iOs app, or and Android app, whatever you prefer.
When running the app, you have to set the right IP address of the socket server
in the input field visible in the GUI, and press 'save' to connect to the server
and start communicating the y rotation.



# Sockit

## installation
You need to have [Node.js](https://nodejs.org/en/) and [NPM](https://github.com/npm/npm) 
(comes with the latest Node.js) installed. Navigate with the command line to `sockit/` 
and run:

```bash
npm install
```

## deployment

Run:

```bash
npm start
```

After that, it will be running on [localhost:8080](http://localhost:8080/admin)
by default with an admin page on '/admin' where you can send y-rotation data
yourself without the need for the device orientation app, e.g.

```javascript
{rotation:90.0}
```
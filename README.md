# controls
1. corner analog stick
2. drag
3. mix? some players corner analog, some drag.

#TODO
DONE - Fix: allow using Hold to restart button after a player wins
Fix: allow using Hold to restart button after it's already been used to regen a maze
- Shadow on text so it can be read ontop of black maze
- Rotate or Animate Cube size from huge to small to show where the "green pad" is.
DONE - Change analog stick to not require drag, to just require tap inside it's clamp
- Resize walls and camera bounds to match x,y size of Maze
- Keep score. keep track of how often a color has won.

#TODO web version
- use WASD, arrow keys, mouse and a gamepad to control 4 players
- if no game pads are detected, delete yellow ball

### player trails using debug lines
create a cube line behind the player
to insure cubes aren't made when player is idle:
only draw cube if last distance - current distance > some number
### player handicap
set movement speed for player so different players move at different speeds

#
This game is at "prototype" fidelity level, it doesn't have the fidelity of your typical $0.99 game. Right now it only has fun gameplay for up to 4 people on a single device.

# How-To: get the Free version!
We know everyone can't afford the $0.99 paid version. So we have options so you can play the full game for free! Getting the free version entails you either:
1. Get a pre-built copy from a pirate. Arrr! (may contain viruses)
or
2. Compile a copy using the source code here.

Here are the steps to building a complete version. Takes about 4 hours to do.
1. [1/2 hr] Download and install the free version of Unity 5.x
2. [2 hr] Setup Unity to export to your device of choice. (Android requires installing the Android SDK)
3. [1/2 hr] Download this source code from GitHub and make sure it runs in Unity
4. (optional) fix code in the game for parts you think suck.
5. [1/2 hr] Build for your device of choice.
6. [1/2 hr] Send and install the build on your device. For Android the easiest way is to email the *.apk to an email account you have on the device.


# Unity Webplayer PSA
PSA: As of Chrome 42 (Released April 14th, 2015), NPAPI plugins are disabled by default! That means Unity Player, Java, and Silverlight games will not work in the Chrome browser! That said, Flash in the Chrome browser continues to work fine (PPAPI).
To Developers: Consider using HTML5 and WebGL instead. Having troubles with the export?
To Players: A workaround is to enable NPAPI plugins in your Chrome settings. Click or Copy+Paste this URL in to a tab:
chrome://flags/#enable-npapi
As of September 2015 however, this option will no longer be available.
Other browsers (Firefox, Internet Explorer, Safari) and Flash are unaffected, but FWIW there is no Unity Player or Silverlight for Linux.
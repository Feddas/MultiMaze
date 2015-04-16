# controls
1. corner analog stick
2. drag
3. mix? some players corner analog, some drag.

#TODO
DONE - Fade text "First to get to the green pad wins"
- If not on win screen, hold middle button for 3 seconds restart
- Way to exit, Hold restart button for 6 seconds (3 more seconds after restart) with countdown
- Animate Cube size from huge to small to show where the "green pad" is.
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

# How-To: get the Free version!
We know everyone can't afford the $0.99 paid version. So we have options so you can play the full game for free! Getting the free version entails you either:
1. Get a pre-built copy from a pirate. Arrr! (may contain viruses)
or
2. Compile a copy using the source code here.

Here are the steps to building a complete version. Takes about 4 hr to do.
1. [1/2 hr] Download and install the free version of Unity 5.x
2. [2 hr] Setup Unity to export to your device of choice. (Android requires installing the Android SDK)
3. [1/2 hr] Download this source code from GitHub and make sure it runs in Unity
4. (optional) fix code in the game for parts you think suck.
5. [1/2 hr] Build for your device of choice.
6. [1/2 hr] Send and install the build on your device. For Android the easiest way is to email the *.apk to an email account you have on the device.
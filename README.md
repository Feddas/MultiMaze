# controls
1. corner analog stick
2. drag
3. mix? some players corner analog, some drag.

#TODO
DONE - Place player at maze start
DONE - find longest path during maze generation
DONE - use longest path to place end
- factor RollerBall out of standard assets
- remove RollerBall jump
- replace RollerBall mesh with basic sphere
- Create 4 Color materials for RollerBall

#TODO multiplayer positioning
- find 4 paths that are all the same distance from the start
- set start cell as the finish cell
- Make sure those 4 paths are at least 3 cells apart, so fingers don't jam against other players if using touch movement.

### player trails using debug lines
create a cube line behind the player
to insure cubes aren't made when player is idle:
only draw cube if last distance - current distance > some number
### player handicap
set movement speed for player so different players move at different speeds

# Free version
We know everyone can't afford the paid version. So we have options so you can play the full game for free! Getting the free version entails you either:
1. Get a pre-built copy from a pirate. Arrr! (may contain viruses)
or
2. Compile a copy using the source code here.

Here are the steps to building a complete version. Takes about 6 hr to do.
1. [1 hr] Download and install the free version of Unity 5.x
2. [2 hr] Setup Unity to export to your device of choice. (Android requires installing the Android SDK)
3. [1 hr] Download this source code and make sure it runs in Unity
4. (optional) fix code in the game for parts you think suck.
5. [1 hr] Build for your device of choice.
6. [1 hr] Send and install the build on your device. For Android the easist way is to email the *.apk to an email account you have on the device.
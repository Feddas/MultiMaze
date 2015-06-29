
# to do AKA stuff you'll see happen with support
- Tilting the device moves all 4 balls at the same
- Resize walls and camera bounds to match x,y size of Maze.
- Settings page radio boxes for maze size; short, normal, long, custom
- Handicap: AKA let the unskilled have a chance against the skilled.
   - set movement speed for player so different players move at different speeds
- Keep score. keep track of how often a color has won.

# to do web version
- use WASD, arrow keys, mouse and a gamepad to control 4 players
- if no game pads are detected, delete yellow ball
- tune webGL version http://docs.unity3d.com/Manual/webgl-building.html

# bugs to fix
Hard to repoduce: allow using hold to restart button after it's already been used to regen a maze

# WebGL
As of Unity 5.1, WebGL does not support use of:
- PlayerPrefs
- Multi-Dimensional arrays
- Setting page doesn't work "unity3d webgl uncaught rangeerror maximum call stack size exceeded"

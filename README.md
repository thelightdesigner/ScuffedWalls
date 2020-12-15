# ScuffedWalls
A simple tool for making ne 1.2 beat saber maps easier. (Heavilly inspired off of BeatWalls)

Features:
 - Noodle extensions custom json data
 - Simple 3d model to wall support with animation & color
 - Image to wall support
 - Custom events support
 - Importing map objects from other map files
 - Appending custom data to map objects
 
 Usage:
  - Drag the map folder onto the program
  - Input the number of the map file to generate to (Will overwrite anything in this map file)
  - Type in the generated SW file, saving refreshes the program automatically. Or hitting R in the console window.

3d modeling for wall conversion

Because beatsaber walls do not support meshes nativley, And because scuffedwalls does not have an internal mesh converter; All modeling done for wall conversion must be made up of cuboids with a global transformation type. The model must be exported in the Collada (.dae) format with +Y up +Z forwards global orientation. *At the time of writing this scuffedwalls does not have color animation support.*

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/text%20examlpe.gif)
![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/global%20or.jpg)

To line up the origin points of the deafault cube in blender, tab into edit mode and snap the cube to where the origin is front bottom center.

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/cube.jpg)

Only changes to transformation and color values will affect the converted walls. Animation is supported.

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/transformation.jpg)

ScuffedWalls support for decomposed transformation types in collada has been dropped so to ensure proper parsing check that 'Matrix' is the selected transformation type.

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/animation.jpg)

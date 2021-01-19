# ScuffedWalls
A simple tool for making ne 1.2 beat saber maps easier. (Heavilly inspired off of BeatWalls)

Features:
 - Noodle extensions custom json data
 - Simple 3d model to wall support with animation & color
 - Image to wall support w/ compression
 - Custom events support
 - Importing map objects from other map files
 - Appending custom data to map objects

- ~~Text to wall support~~ v0.7.0 <-
 
 Usage:
  - Drag the map folder onto the program
  - Input the number of the map file to generate to (Will overwrite anything in this map file)
  - Type in the generated SW file, saving refreshes the program automatically. Or hitting R in the console window.
Windows will probably bother you about this being malware. If you dont trust it clone the repo and build it yourself.
  
More info on scuffed functions can be found [`here`](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Functions.md)

3d modeling for wall conversion

Because beatsaber walls do not support meshes nativley, And because scuffedwalls does not have an internal mesh converter; All modeling done for wall conversion must be made up exclusivly of cubes with only transformations. Editing the mesh in any way wont affect the converted walls. The model must be exported in the Collada (.dae) format with +Y up +Z forwards global orientation. *At the time of writing this scuffedwalls does not have color animation support.*

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/text%20examlpe.gif)
![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/text%20examp.gif)

When going to export to collade hitting 'n' will bring up a collada settings menu. this is where you will choose the Y up Z forwards and check the global orientation box.

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/global%20or.jpg)

To line up the origin points of the default cube in blender, tab into edit mode and snap the cube to where the origin is front bottom center with y facing forwards. The converter wont work unless this is done exactly correct.

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/cube.jpg)

Only changes to transformation and color values will affect the converted walls. Animation is supported.

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/transformation.jpg)

To ensure proper parsing check that 'Matrix' is the selected transformation type.

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/animation.jpg)

Sampling rate can be adjusted if the map file size becomes an issue.

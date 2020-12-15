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

Because beatsaber walls do not support meshes nativley, And because scuffedwalls does not have an internal mesh converter; All modeling for done for wall conversion must be made up of cuboids with a global transformation type. The model must be exported in the Collada (.dae) format with +Y up +Z forwards global orientation. *At the time of writing this scuffedwalls does not have color animation support.*

![](text examlpe.gif)

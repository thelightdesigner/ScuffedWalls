# Setting Up A Blender Project for Wall Conversion

Because beatsaber walls do not support meshes nativley, And because scuffedwalls does not have an internal mesh converter; All modeling done for wall conversion must be made up exclusivly of 3d cubes (default cubes) with only changes to rotation, scale, position and color. Editing the mesh in any way wont affect the converted walls. The model must be exported in the Collada (.dae) format with +Y up +Z forwards global orientation.

It is important to note that changes are still being made to the model parser and this section will change in future updates.

## Animating
Any animation on position, rotation or scale to a cube will show up in beatsaber. Shape Keys and other mesh deformations wont work.

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/transformation.jpg)

##  Parenting and Baking

One of the benefits of making a 3d model in blender is that complex 3d movement is all calculated internally. Parenting is a good way to get a collection of cubes in blender to all move togethor just by moving the reference object. In order to get scuffedwalls to pick up on the child cubes in your scene you must bake the childeren.

While selecting the childeren objects go to object, animation, bake action. Select visual keying and clear parents. This will automatically keyframe every child object to its visual position in the scene. It also clears all parents that the child objects may have had.

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/bake.png)
![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/bake2.png)

## Exporting!!!!!!

When going to export to collada hitting 'n' will bring up a collada settings menu. this is where you will choose the Y up Z forwards. check the global orientation box.


![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/global%20or.jpg)

To ensure proper parsing check that 'Matrix' is the selected transformation type.

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/animation.jpg)

Sampling rate can be adjusted if the map file size becomes an issue.

## Importing

To make a 3d model in your map with scuffedwalls. Call the ModelToWall function. More info on the parameters can be found in [`here`](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Functions.md)

HasAnimation: bool; this tells the model parser to read the model file as if it has an animation attached to it. It is disabled by default.

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/text%20examlpe.gif)

Normal: bool; makes the walls jump in and despawn as normal ne1.0 walls would. the proportions are preserved so overriding njs will change the amount of time it takes for the model finish being visible. Setting normal as true will ignore animations.

Normal:true will result in this from a static model with no animations

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/normal.gif)


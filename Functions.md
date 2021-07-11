## Functions
Functions are referenced in the \_SW.sw file.

example:
```
0:Wall
  color:[0,1,1,1]
```
makes a cyan wall

All the available functions are listed below

- [`AppendWalls`](#AppendWalls)
- [`AppendNotes`](#AppendNotes)
- [`AppendEvents`](#AppendEvents)
- [`TextToWall`](#TextToWall)
- [`ModelToWall`](#ModelToWall)
- [`ImageToWall`](#ImageToWall)
- [`Environment`](#Environment)
- [`CloneFromWorkspace`](#CloneFromWorkspace)
- [`Blackout`](#Blackout)
- [`Import`](#Import)
- [`Run`](#Run)
- [`Wall`](#Wall)
- [`Note`](#Note)
- [`AnimateTrack`](#AnimateTrack)
- [`AssignPathAnimation`](#AssignPathAnimation)
- [`AssignPlayerToTrack`](#AssignPlayerToTrack)
- [`ParentTrack`](#ParentTrack)
- [`PointDefinition`](#PointDefinition)


## Noodle Extensions/Chroma Properties Syntax
Noodle Extensions/Chroma/Other properties that can be used on most functions 

Most of these properties are directly connected to their corresponding Noodle/Chroma property. 
[`Noodle documentation`](https://github.com/Aeroluna/NoodleExtensions) 
[`Noodle Animation documentation`](https://github.com/Aeroluna/NoodleExtensions/blob/master/Documentation/AnimationDocs.md)
[`Chroma documentation`](https://github.com/Aeroluna/Chroma)

"" = put in quotes, ? = optional

- AnimateDefinitePosition: \[x,y,z,t,"e"?]
- DefineAnimateDefinitePosition:string
- AnimatePosition: \[x,y,z,t,"e"?]
- DefineAnimatePosition:string
- Scale: \[x,y?,z?]
- Track: string
- NJSOffset: float
- NJS: float
- AnimateDissolve: \[d,t,"e"?]
- DefineAnimateDissolve:string
- AnimateColor: \[r,g,b,a,t,"e"?]
- DefineAnimateColor:string
- AnimateRotation: \[x,y,z,t,"e"?]
- DefineAnimateRotation:string
- AnimateLocalRotation: \[x,y,z,t,"e"?]
- DefineAnimateLocalRotation:string
- AnimateScale: \[x,y,z,t,"e"?]
- DefineAnimateScale:string
- AnimateInteractable:\[i,t]
- DefineAnimateInteractable:string
- Interactable: bool
- Fake: bool
- Position: \[x,y]
- Rotation: \[x,y,z] or float
- LocalRotation: \[x,y,z]
- CutDirection: float
- DisableSpawnEffect: bool
 - CPropID: int
 - CLightID: int
 - CGradientDuration: float
 - CgradientStartColor: \[r,g,b,a?]
 - CgradientEndColor: \[r,g,b,a?]
 - CgradientEasing: string
 - CLockPosition: bool
 - CPreciseSpeed: float
 - CDirection: int
 - CNameFilter: string
 - CReset: bool
 - CStep: float
 - CProp: float
 - CSpeed: float
 - CCounterSpin: bool
 - Color: \[r,g,b,a] (0-1)
 - RGBColor:\[r,g,b,a] (0-255)

## CustomEvent Properties Syntax
Noodle Extensions properties that are used on custom event functions (AnimateTrack, AssignPathAnimation...) "" = put in quotes, ? = optional

- AnimateDefinitePosition: \[x,y,z,t,"e"?]
- DefineAnimateDefinitePosition:string
- AnimatePosition: \[x,y,z,t,"e"?]
- DefineAnimatePosition:string
- Track: string
- AnimateDissolve: \[d,t,"e"?]
- DefineAnimateDissolve:string
- AnimateColor: \[r,g,b,a,t,"e"?]
- DefineAnimateColor:string
- AnimateRotation: \[x,y,z,t,"e"?]
- DefineAnimateRotation:string
- AnimateLocalRotation: \[x,y,z,t,"e"?]
- DefineAnimateLocalRotation:string
- AnimateScale: \[x,y,z,t,"e"?]
- DefineAnimateScale:string
- childTracks:\["str","str"...]
- parentTrack: string
- easing: string

## Math & LineFunctions
Math expressions are computed inside of { } symbols. A random floating point number is yielded from the line function `Random(val1,val2)`. A random integer is yielded from the line function `RandomInt(val1,val2)`.

```
0:Wall
  position:[{ 5+6 }, Random(1,5), { 5+6+Random(2,10) }]
  scale:[RandomInt(5,0),RandomInt(5,0),RandomInt(5,0)]
  ```

```
0:Wall
  repeat:100
  position:[{repeat/10},0,0]
  scale:[0.1,1,1]
  color:HSLtoRGB({repeat/100},1,0.5)
  ```
  
![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/rainbow.png)
  
  
The example above uses the HSLtoRGB line function to create a rainbow.

The available line functions are:
 - Random(Val1,Val2) => returns a number
 - RandomInt(Val1,Val2) => returns a number
 - HSLtoRGB(Hue,Saturation?,Lightness?,Alpha?,Any extra values like easings or whatever?) => returns a point definition
 - MultPointDefinition(PointDefinition,value to multiply) => returns a point definition
 - OrderPointDefinitions(PointDefinitions) => returns point definitions

todo: add more string functions, dm me for suggestions or clone the repo and navigate to ScuffedWalls -> Program -> Parser -> StringFunc.cs and add a new StringFunction() to the array in void Populate(). When you're finished and it works feel free to make a pull request.



 ## Variables
Variables are containers for string/numerical data.

```
Workspace

var:SomeVariableName
  data:5
  recompute:0

0:Wall
  NJS:SomeVariableName
  ```

  ```
Workspace

  var:Grey
  data:Random(0,1)
  recompute:1

  #Creates walls with random shades of grey
0:Wall
  color:[Grey,Grey,Grey,1]
  repeat:15
  ```
  
Keep in mind: Variables are only accessable from the workspace they are defined in.

recompute:
0 = recompute math, variables and random() for all references of the variable, 
1 = recompute every repeat/function, 
2 = compute once on creation
defaults to 2

# Internal Variables
Variables that are auto created and changed internally. All repeatable functions will have at least 2 internal variables called "repeat" and "time". The append function populates all the properties of each wall/note/event as a variable.

```
0:Wall
  repeat:60
  repeataddtime:0.05
  scale:[0.25,0.25,0.25]
  position:[{repeat/8},{Sin(repeat/2)}]
  ```

![](https://github.com/thelightdesigner/ScuffedWalls/blob/1.0/Readme/sine.png)


# AppendEvents
adds on custom chroma data to events/lights between the function time and endtime (toBeat)

 - toBeat: float
 - appendTechnique: int(0-2)
 - chroma customdata
 - lighttype: 0, 1, 2, 3; the type of the light
  
 ~special things~
 - converttorainbow
 - rainbowfactor

 Example
```
 5:AppendToAllEventsBetween
   toBeat:10
   appendTechnique:2
   lightType:1,3,0
   converttorainbow: true
   rainbowfactor:1
 ```

# AppendWalls
adds on custom noodle data to walls between the function time and endtime (toBeat)

 - toBeat: float
 - appendTechnique: int(0-2)
 - onTrack: string, only appends to notes on this track
 - generic custom data
 
  Example
 ```
 5:AppendToAllWallsBetween
   toBeat:10
   track: FurryTrack
   appendTechnique:1
 ```


multiplies all the wall times by 2
```
0:AppendWalls
   time:{_time * 2}
   appendtechnique:1
   ```

multiplies all the definitepositions by 3 except for the time value
```
0:AppendWalls
   animateDefinitePosition:[{_animation._definitePosition(0)(0) * 3},{_animation._definitePosition(0)(1) * 3},{_animation._definitePosition(0)(2) * 3},_animation._definitePosition(0)(3)]
   appendtechnique:1
   ```
   
a very scuffed way to make a rainbow


![](https://github.com/thelightdesigner/ScuffedWalls/blob/1.0/Readme/color.png)


[`a less scuffed way to make a rainbow`](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Functions.md#string-functions)

# AppendNotes
adds on custom noodle data to notes between the function time and endtime (toBeat)

 - toBeat: float
 - notetype: int,int,int (defaults to 0,1,2,3), only appends to notes with the specified type(s), see [`here`](https://bsmg.wiki/mapping/map-format.html#notes-2) for info on \_type
 - appendTechnique: int(0-2)
 - onTrack: string, only appends to notes on this track
 - generic custom data
 
  Example
  ```
 5:AppendToAllNotesBetween
   toBeat:10
   track: FurryTrack
   appendTechnique:2

60:AppendToAllNotesBetween
tobeat:63
Njsoffset:Random(1,3)
AnimatePosition:[Random(-7,6),Random(-6,6),0,0],[0,0,0,0.35,"easeOutCubic"],[0,0,0,1]
AnimateDissolve:[0,0],[1,0.1],[1,1]
DisableSpawnEffect:true

66:AppendToAllNotesBetween
tobeat:99
NJS:10
DisableSpawnEffect:true
AnimateDissolveArrow: [0,0],[0,1]
track:CameraMoveNotes
 ```

multiplies all the note times by 2

```
0:AppendNotes
   time:{_time * 2}
   appendtechnique:1
   ```

multiplies all the definitepositions by 3 except for the time value
```
0:AppendNotes
   animateDefinitePosition:[{_animation._definitePosition(0)(0) * 3},{_animation._definitePosition(0)(1) * 3},{_animation._definitePosition(0)(2) * 3},_animation._definitePosition(0)(3)]
   appendtechnique:1
   ```


 ## AppendTechnique
The merge priority of the values being appended
 - 0 = Low Priority (Will not overwrite any property but can still append to nulled properties)
 - 1 = High Priority (Can overwrite any property)
 - 2~4 = ??? (Dont use these)

**default is 0**

## Append Function Internal Variables
The append function runs through each object in a workspace and changes its data.

In the example, `animateDefinitePosition:[{_animation._definitePosition(0)(0) * 3}...]`

`_animation._definitePosition(0)(0)` is a reference to an "internal" variable created by the append function. in the case of SW, indexers are represented with parenthesis.


this example takes the value from the already existing  `_definitePosition` array, indexes in to the first array of the point definition, then indexes in again into the first value of the array, multiplies the resulting floating point number by 3, and then sets that.

granted this only works if every object has a  `_definitePosition` with a value at the specified index

confusing right?


# TextToWall
Constructs text out of walls

Rizthesnuggies [`Intro to TextToWall`](https://www.youtube.com/watch?v=g49gfMtzETY) function

see [here](https://github.com/thelightdesigner/ScuffedWalls/blob/main/TextToWall.md) for how the program reads font images/models.

 - path: string
 - fullpath string
 - line: string, the text you want to convert to walls. [this can be repeated](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/linetext.jpg) to add more lines of text.
 - letting: float, the relative space between letters. default: 1
 - leading: float, the relative space between lines. default: 1
 - size: float, scales the text. default: 1 (gigantic)
 - thicc: float, makes the edges of the walls fill more of the center
 - duration: float
 - definitedurationbeats: float, makes the walls stay around for exactly this long in beats
 - definitedurationseconds: float, makes the walls stay around for exactly this long in seconds
 - definitetime: beats/seconds, makes the walls jump in at exactly the function time in seconds or beats
 - Position => moves the text by this amount, defaults to \[0,0]
 - all the other imagetowall params if your really interested
 - all the other modeltowall params if your really interested
 - generic custom data
 
 Example
 ```
 5:TextToWall
   Path:font.dae
   line:a line of text!
   line:another line of text?
   letting:2
   leading:-1
   thicc:12
   spreadspawntime:1
   size:0.1
   position:[0,2]
   animatedefiniteposition:[0,0,0,0]

#makes the text jump in at beat 5 and exist for 7 beats exactly
   definitedurationbeats:7
   definitetime:beats
 ```

# ModelToWall 
(repeatable)
constructs a model out of walls. see [here](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Blender%20Project.md) for more info

Rizthesnuggies [`Intro to ModelToWall`](https://youtu.be/FfHGRbUdV_k) function


 - path: string
 - fullpath: string
 - hasAnimation: bool, tells the model parser to read animation. definite only
 - duration: float, controls the duration of the model. this affects the length of time it takes to play the model animation.
 - definitedurationbeats: float, makes the walls stay around for exactly this long in beats
 - definitedurationseconds: float, makes the walls stay around for exactly this long in seconds
 - definitetime: beats/seconds, makes the walls jump in at exactly the function time in seconds or beats
 - spreadspawntime: float
 - Normal: bool, makes the walls jump in and fly out as normal. essentially 1.0 model to wall when set to true. default: false
 - createtracks: bool
 - colormult: float, multiplies all the model color values by this amount
 - preservetime: bool
 - cameratoplayer: bool
 - createnotes: bool
 - spline: bool
 - spreadspawntime: float
 - type: 0, 1, 2 or 3
 - alpha: float
 - thicc: float
 - deltaposition: \[x,y,z] offsets the model in 3d by this position vector
 - deltarotation: \[x,y,z] rotates the model around the center of its bounding box
 - deltascale: float, scales the model around the center of its bounding box
 - setdeltaposition: bool
 - setdeltascale: bool
 - generic custom data
 - repeat: int
 - repeataddtime: float
 
 
  Example
  ```
 5:ModelToWall
   Path:model.dae
   hasAnimation:false
   spreadspawntime:1
   normal:true
   track:FurryTrack

#makes the model jump in at beat 5 and last for 5.1276 seconds exactly
   definitetime:beats
   definitedurationseconds:5.1276
 ```

# ImageToWall
constructs an image out of walls as pixels

Rizthesnuggies [`Intro to ImageToWall`](https://youtu.be/Cxbc4llIq3k) function

 - path: string
 - fullpath string
 - isBlackEmpty: bool, doesn't add pixel if the pixel color is black. default: false
 - size: float, scales the image. default: 1
 - thicc: float, makes the edges of the walls fill more of the center
 - centered: bool, centers the x position. default: false
 - duration: float  
 - definitedurationbeats: float, makes the walls stay around for exactly this long in beats
 - definitedurationseconds: float, makes the walls stay around for exactly this long in seconds
 - definitetime: beats/seconds, makes the walls jump in at exactly the function time in seconds or beats
 - spreadspawntime: float. default: 0
 - maxlinelength: int, the max line length. default: +infinity
 - shift: float, the difference in compression priorities between the inverted compression. default: 1
 - compression: float, how much to compress the wall image, Not linear in the slightest. recommended value(0-0.1) default: 0
 - Position => moves each pixel by this amount, defaults to \[0,0]
 - Alpha: the alpha value
 - generic custom data
 
  Example
  ```
 5:ImageToWall
   Path:image.png
   thicc:12
   size:0.5
   isBlackEmpty: true
   centered: true
   maxlinelength:8
   compression:0.1
   spreadspawntime:1
   position:[0,2]
   duration:12
   animatedefiniteposition:[0,0,0,0]
 ```
 
# Environment
makes a chroma environment enhancement, idk what this does but i heard [`its pretty cool`](https://github.com/Aeroluna/Chroma#environment-enhancement)

- id: string
- track: string
- lookupmethod: string
- duplicate: int
- active: bool
- scale: \[x,y,z]
- localposition: \[x,y,z]
- localrotation: \[x,y,z]
- position: \[x,y,z]
- rotation: \[x,y,z]
 
 
# CloneFromWorkspace
clones mapobjects from a different workspace by the index or by the name. the time of the function is the beat that starts cloning from.

- Type: int,int,int (defaults to 0,1,2,3) 0 being walls, 1 being notes, 2 being lights, 3 being custom events & NOT point definitions
- Index: int, the index of the workspace you want to clone from. It's either one or the other.
- Name:string, the name of the workspace you want to clone from. It's either one or the other.
- addTime: float, shifts the cloned things by this amount.
- toBeat: float, the beat where to stop cloning from.

 Example
```
Workspace:wtf workspace
64:wall

Workspace:hahaball

Workspace

	#adds in one wall at beat 97, a copy of "wtf workspace" shifted up by 32 beats
	#now in the map there will be a wall at 64 and a wall at 96
 25:CloneFromWorkspace
   Name:wtf workspace
   Type:0,1,2
   toBeat:125
   addTime:32
   
   
 ```
 
# Blackout
adds a single light off event at the beat number. why? because why not.

 Example
  ```
 5:Blackout
 ```
 
 
# Run
calls the terminal/command prompt and runs the specified args after or before the programs runtime.

also can run javascript files

 - args: string, this is what will be put into the terminal
 - runbefore: bool, (if true) will execute this function before SW begins to parse the .sw file, when false this function runs after SW finishes writing to the map file
 - javascript: string, path to the .js file, will execute this file using the node command

```
0:Run
  Javascript:CoolMapScript.js
  RunBefore: false
```
note that in the above example, CoolMapScript.js is in the map folder

```
0:Run
  Args:Start Notepad.exe
  RunBefore: false
```
 
# Import
adds in map objects from other map.dat files

 - path: string
 - fullpath string
 - type:int,int,int (defaults to 0,1,2,3) what to import where 0 = walls, 1 = notes, 2 = lights, 3 = customevents & point definitions
 - addtime:float
 - toBeat: float
 
  Example: adds lights from EasyStandard.dat from beat 15 to beat 180
  ```
 15:Import
   fullpath:E:\New folder\steamapps\common\Beat Saber\Beat Saber_Data\CustomWIPLevels\scuffed walls test\EasyStandard.dat
   type:2
   toBeat:180
 ```



# Wall
(repeatable)
makes a wall

Rizthesnuggies [`Intro to Wall & Note`](https://youtu.be/hojmJ1UZcb8) function

- duration: float
- definitedurationbeats: float, makes the walls stay around for exactly this long in beats
 - definitedurationseconds: float, makes the walls stay around for exactly this long in seconds
 - definitetime: beats/seconds, makes the walls jump in at exactly the function time in seconds or beats
- repeat: int, amount of times to repeat
- repeatAddTime: float
- generic custom data

 Example
```
	#blue fire
5:Wall
  repeat:160
  repeataddtime:0.2
  NJSOffset:-10
  animatedefiniteposition:[Random(0,2),Random(8,12),Random(28,31),0],[Random(-7,-4),Random(10,14),Random(28,31),1,"easeInSine"]
  animatescale:[1,1,1,0],[0.01,0.01,0.01,1,"easeInSine"]
  scale:[0.8,0.8,0.8]
  color:[0,Random(1.6,1.7),Random(1.9,2),2]
  localrotation:[Random(0,360),Random(0,360),Random(0,360)]
  rotation:[0,0,-5]
  track:flowerfloat
  animatedissolve:[0,0],[1,0],[1,0.9],[0,1]

	#shooting star
164:Wall
  repeat:50
  repeataddtime:0.4
  scale:[0.2,0.2,18]
  Njs:10
  NjsOffset:20
  position:[Random(0,80),Random(-100,100)]
  color:[Random(0,20),Random(0,20),Random(0,20),20]
  rotation:[Random(0,360),90,0]

```

# Note
(repeatable)
makes a note

Rizthesnuggies [`Intro to Wall & Note`](https://youtu.be/hojmJ1UZcb8) function

- repeat: int, amount of times to repeat
- repeatAddTime: float
- generic custom data
- type:int
- cutDirection:int



these properties use \_noteJumpStartBeatOffset to adjust the notes duration

- definitedurationbeats: float, makes the note stay around for exactly this long in beats
 - definitedurationseconds: float, makes the note stay around for exactly this long in seconds
 - definitetime: beats/seconds, makes the note jump in at exactly the function time in seconds or beats

 Example
```
	#Note fire
100:Note
  repeat:66
  repeatAddTime:0.3
  localRotation:[Random(0,360),Random(0,360),Random(0,360)]
  Rotation:[90,0,0]
  Position:[Random(-12,-6),Random(8,18)]
  AnimatePosition:[0,0,-10,0],[0,0,-10,1]
  AnimateDissolve:[0,0],[1,0.1],[1,1]
  AnimateScale:[2,2,2,0],[2,2,2,1]
  AnimateColor:[1, 0, 0, 1,0], [0, 1, 0, 0.5, 0.0832], [0, 0, 1, 1, 0.15], [1, 0, 0, 1, 0.23], [0, 1, 0, 1, 0.30], [0, 0, 1, 1, 0.38], [1, 0, 0, 1, 0.45], [0, 1, 0, 1, 0.52],     [0, 0, 1, 1, 0.60], [1, 0, 0, 1, 0.68], [0, 1, 0, 1, 0.75], [0, 0, 1, 1, 0.84],[1,1,1,1,1]
  NJS:10
  NJSOffset:4
  fake:true
  isInteractable: false
  track: RandomShit
```

# AnimateTrack
(repeatable)
makes a custom event

 - customevent data
 - repeat: int, amount of times to repeat
 - repeatAddTime: float
 
  Example
 ```
 70:AnimateTrack
  track:RandomShit
  duration:1
  animatedissolve:[0,0],[0,1]
  animatedissolvearrow:[0,0],[0,1]
  
  100:AnimateTrack
  track:RandomShit
  duration:1
  animatedissolve:[0,0],[1,1]
  animatedissolvearrow:[0,0],[1,1]
```

# AssignPathAnimation
(repeatable)
makes a custom event

 - customevent data
 - repeat: int, amount of times to repeat
 - repeatAddTime: float
 
  Example
 ```
42:AssignPathAnimation
  track:BeginningStretch
  duration:8
  AnimateRotation:[0,0,0,0],[0,0,0,0.5,"easeOutQuad"],[0,0,0,1]
  easing:easeInOutSine
```

# AssignPlayerToTrack
makes a custom event
 - customevent data
 
  Example
 ```
 3:AssignPlayerToTrack
    track:BigMovement
 ```

# ParentTrack
makes a custom event
 - customevent data
 
  Example
  ```
 3:ParentTrack
    ParentTrack:BigMovement
    ChildTracks:["rightnotes","leftnotes"]
 ```

# PointDefinition
makes a point definition
  - name:string
  - points:points

  Example
```
  5:PointDefinition
    name:UpDownPoints
    points:[0,0,0,0],[0,15,0,0.5,"easeInOutSine"],[0,0,0,1,"easeInOutSine"]

  15:Wall
    DefineAnimateDefinitePosition:UpDownPoints
```

# Script
use this function by downloading the repo and navigating to ScuffedWalls>Program>Functions>Script.cs

is this stupid? yes.


# Uwu
don't ever call this

:)

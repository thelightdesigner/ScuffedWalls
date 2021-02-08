## Functions

Functions are defined by a number followed with a colon and the function name (i.e  5:function). Every line beneath the function will apply to that function. All the available functions that can be used are the following.

- [`TextToWall`](#TextToWall)
- [`ModelToWall`](#ModelToWall)
- [`CloneFromWorkspaceByIndex`](#CloneFromWorkspaceByIndex)
- [`Blackout`](#Blackout)
- [`ImageToWall`](#ImageToWall)
- [`AppendToAllWallsBetween`](#AppendToAllWallsBetween)
- [`AppendToAllNotesBetween`](#AppendToAllNotesBetween)
- [`AppendToAllEventsBetween`](#AppendToAllEventsBetween)
- [`Import`](#Import)
- [`Wall`](#Wall)
- [`Note`](#Note)
- [`AnimateTrack`](#AnimateTrack)
- [`AssignPathAnimation`](#AssignPathAnimation)
- [`AssignPlayerToTrack`](#AssignPlayerToTrack)
- [`ParentTrack`](#ParentTrack)
- [`PointDefinition`](#PointDefinition)

## CustomData
generic customdata that can be parsed as a parameter on most functions
"" = put in quotes, ? = optional
- AnimateDefinitePosition: \[x,y,z,t,"e"?]
- AnimatePosition: \[x,y,z,t,"e"?]
- Scale: \[x,y?,z?]
- Track: string
- NJSOffset: float
- NJS: float
- AnimateDissolve: \[d,t,"e"?]
- AnimateColor: \[r,g,b,t,"e"?]
- AnimateRotation: \[x,y,z,t,"e"?]
- AnimateLocalRotation: \[x,y,z,t,"e"?]
- AnimateScale: \[x,y,z,t,"e"?]
- isInteractable: bool
- Fake: bool
- Position: \[x,y]
- Rotation: \[x,y,z] or float
- LocalRotation: \[x,y,z]
- CutDirection: float
- NoSpawnEffect: bool

## CustomEvent Data
generic customdata for customevents
"" = put in quotes, ? = optional
- AnimateDefinitePosition: \[x,y,z,t,"e"?]
- AnimatePosition: \[x,y,z,t,"e"?]
- Track: string
- AnimateDissolve: \[d,t,"e"?]
- AnimateColor: \[r,g,b,t,"e"?]
- AnimateRotation: \[x,y,z,t,"e"?]
- AnimateLocalRotation: \[x,y,z,t,"e"?]
- AnimateScale: \[x,y,z,t,"e"?]
- childTracks:\["str","str"...]
- parentTrack: string
- easing: string

## Chroma CustomData : CustomData
 - CPropID: int
 - CLightID: int
 - CGradientDuration: float
 - CgradientStartColor: [r,g,b,a?]
 - CgradientEndColor: [r,g,b,a?]
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
 - Color: [r,g,b,a?]

# TextToWall
uses imagetowall to procedurally create walltext from text (Constructs text out of walls)

Rizthesnuggies [`Intro to TextToWall`](https://www.youtube.com/watch?v=g49gfMtzETY) functions

see [here](https://github.com/thelightdesigner/ScuffedWalls/blob/main/TextToWall.md) for how the program reads font images.

 - path: string
 - fullpath string
 - line: string, the text you want to convert to walls. [this can be repeated](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/linetext.jpg) to add more lines of text.
 - letting: float, the relative space between letters. default: 1
 - leading: float, the relative space between lines. default: 1
 - size: float, scales the text. default: 1 (gigantic)
 - thicc: float, makes the edges of the walls fill more of the center
 - duration: float
 - Position => moves the text by this amount, defaults to \[0,0]
 - all the other imagetowall params if your really interested
 - generic custom data
 
 Example
 ```
 5:TextToWall
   Path:font.png
   line:a line of text!
   line:another line of text?
   letting:2
   leading:-1
   thicc:12
   spreadspawntime:1
   size:0.1
   position:[0,2]
   duration:12
   animatedefiniteposition:[0,0,0,0]
 ```

# ModelToWall
constructs a model out of walls. see [here](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Blender%20Project.md) for more info

 - path: string
 - fullpath string
 - hasAnimation: bool, tells the model parser to read animation. doesnt work with normal yet.
 - duration: float
 - spreadspawntime: float
 - Normal: bool, makes the walls jump in and fly out as normal. essentially 1.0 model to wall when set to true. default: false
 - generic custom data
 
  Example
  ```
 5:ModelToWall
   Path:model.dae
   hasAnimation:false
   spreadspawntime:1
   normal:true
   track:FurryTrack
 ```

# ImageToWall
constructs an image out of walls as pixels

 - path: string
 - fullpath string
 - isBlackEmpty: bool, doesn't add pixel if the pixel color is black. default: false
 - size: float, scales the image. default: 1
 - thicc: float, makes the edges of the walls fill more of the center
 - centered: bool, centers the x position. default: false
 - duration: float
 - spreadspawntime: float. default: 0
 - maxlinelength: int, the max line length. default: +infinity
 - shift: float, the difference in compression priorities between the inversed compression. default: 1
 - compression: float, how much to compress the wall image, Not linear in the slightest. reccomended value(0-0.1) default: 0
 - Position => moves each pixel by this amount, defaults to \[0,0]
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
 
# CloneFromWorkspaceByIndex
clones mapobjects from a different workspace by the index

- Type: int,int,int (defaults to 0,1,2,3) 0 being walls, 1 being notes, 2 being lights, 3 being custom events
- Index: int
- fromBeat: float
- addTime: float, shifts the cloned things by this amount.
- toBeat: float

 Example
```
 5:CloneFromWorkspaceByIndex
   Index:2
   Type:0,1,2
   fromBeat:25
   toBeat:125
   addTime:32
 ```
 
# Blackout
adds a single light off event

 Example
  ```
 5:Blackout
 ```

# AppendToAllEventsBetween
adds on custom chroma data to events/lights between the function time and endtime

 - toBeat: float
 - appendTechnique: int(0-2)
 - chroma customdata
  lighttype: 0, 1, 2, 3; the type of the light
  
 ~special things~
 - converttoprops
 - propfactor
 - converttorainbow
 - rainbowfactor

 Example
```
 5:AppendToAllEventsBetween
   toBeat:10
   appendTechnique:2
   lightType:1,3,0
   converttoprops: true
   propfactor: 1
   converttorainbow: true
   rainbowfactor:1
 ```

# AppendToAllWallsBetween
adds on custom noodle data to walls between the function time and endtime

 - toBeat: float
 - appendTechnique: int(0-2)
 - generic custom data
 
  Example
 ```
 5:AppendToAllWallsBetween
   toBeat:10
   track: FurryTrack
 ```

# AppendToAllNotesBetween
adds on custom noodle data to notes between the function time and endtime

 - toBeat: float
 - type: int,int,int (defaults to 0,1,2,3,4,5,6,7,8)
 - appendTechnique: int(0-2)
 - generic custom data
 
  Example
  ```
 5:AppendToAllNotesBetween
   toBeat:10
   track: FurryTrack
 ```
 
# Import
adds in map objects from other map.dat files

 - path: string
 - fullpath string
 - type:int(0-3), what to import
 - fromBeat: float
 - toBeat: float
 
  Example
  ```
 5:Import
   fullpath:E:\New folder\steamapps\common\Beat Saber\Beat Saber_Data\CustomWIPLevels\scuffed walls test\EasyStandard.dat
   type:2
   fromBeat:15
   toBeat:180
 ```

# Wall
makes a wall

- duration: float
- repeat: int, amount of times to repeat
- repeatAddTime: float
- generic custom data

 Example
```
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
```

# Note
makes a note

- duration: float
- repeat: int, amount of times to repeat
- repeatAddTime: float
- generic custom data

 Example
```
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
    track:BigMovement
    ChildTracks:["rightnotes","leftnotes"]
 ```



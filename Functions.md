## Functions

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
generic customdata that can be parsed on most functions
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

# ModelToWall
constructs a definite model out of walls. see [README.md](https://github.com/thelightdesigner/ScuffedWalls/blob/main/README.md) for more info

 - path: string
 - fullpath string
 - hasAnimation: bool, tells the model parser to read animation
 - duration: float
 - spreadspawntime: float
 - generic custom data

# ImageToWall
constructs an image out of walls as pixels

 - path: string
 - fullpath string
 - isBlackEmpty: bool, doesn't add pixel if the pixel color is black
 - size: float, scales the image
 - thicc: float, makes the edges of the walls fill more of the center
 - centered: bool, centers the x position
 - duration: float
 - spreadspawntime: float
 - maxlinelength: int, the max line length
 - shift: float, the difference in compression priorities between the inversed compression
 - compression: float, how much to compress the wall image, Not linear in the slightest. reccomended value(0-0.1)
 - AnimateDefinitePosition => moves each pixel by this amount, defaults to \[0,0,0,0],\[0,0,0,1] (only if normal = false)
 - Position => moves each pixel by this amount, defaults to \[0,0] (only if normal = true)
 - generic custom data
 
# CloneFromWorkspaceByIndex
clones mapobjects from a different workspace by the index

- Type: int(0-3), what to clone
- Index: int
- fromBeat: float
- toBeat: float


# Blackout
adds a single light off event

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

# AppendToAllWallsBetween
adds on custom noodle data to walls between the function time and endtime

 - toBeat: float
 - appendTechnique: int(0-2)
 - generic custom data

# AppendToAllNotesBetween
adds on custom noodle data to notes between the function time and endtime

 - toBeat: float
 - NoteColor: red, blue, bomb, 1, 2, 3, 4; the type of the note
 - appendTechnique: int(0-2)
 - generic custom data
 
# Import
adds in map objects from other map.dat files

 - path: string
 - fullpath string
 - type:int(0-3)
 - fromBeat: float
 - toBeat: float

# Wall
makes a wall

- duration: float
- repeat: int, amount of times to repeat
- repeatAddTime: float
- generic custom data

# Note
makes a note

- duration: float
- repeat: int, amount of times to repeat
- repeatAddTime: float
- generic custom data

# AnimateTrack
makes a custom event

 - customevent data
 - repeat: int, amount of times to repeat
 - repeatAddTime: float

# AssignPathAnimation
makes a custom event

 - customevent data
 - repeat: int, amount of times to repeat
 - repeatAddTime: float

# AssignPlayerToTrack
makes a custom event
 - customevent data

# ParentTrack
makes a custom event
 - customevent data

# PointDefinition
defines a set of points
- name: string
- points: \[],\[],\[]



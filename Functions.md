## Functions

- [`ModelToWall`](#ModelToWall)
- [`CloneFromWorkspaceByIndex`](#CloneFromWorkspaceByIndex)
- [`Blackout`](#Blackout)
- [`ImageToWall`](#ImageToWall)
- [`AppendToAllWallsBetween`](#AppendToAllWallsBetween)
- [`AppendToAllNotesBetween`](#AppendToAllNotesBetween)
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

# ModelToWall
constructs a definite model out of walls. see [README.md](https://github.com/thelightdesigner/ScuffedWalls/blob/main/README.md) for more info
parameters
 - path: string
 - fullpath string
 - hasAnimation: bool, tells the model parser to read animation
 - duration: float
 - spreadspawntime: float

# ImageToWall
constructs an image out of walls as pixels
parameters
 - path: string
 - fullpath string
 - isBlackEmpty: bool, doesn't add pixel if the pixel color is black
 - size: float, scales the image
 - thicc: float, makes the edges of the walls fill more of the center
 - normal: bool, makes the image use position
 - centered: bool, centers the x position
 - duration: float
 - spreadspawntime: float
 - AnimateDefinitePosition => moves each pixel by this amount, defaults to \[0,0,0,0],\[0,0,0,1] (only if normal = false)
 - Position => moves each pixel by this amount, defaults to \[0,0] (only if normal = true)
 
# CloneFromWorkspaceByIndex

# Blackout

# ImageToWall

# AppendToAllWallsBetween

# AppendToAllNotesBetween

# Import

# Wall

# Note

# AnimateTrack

# AssignPathAnimation

# AssignPlayerToTrack

# ParentTrack

# PointDefinition



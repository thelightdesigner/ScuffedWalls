# ScuffedWalls v1.3.5

# Documentation on functions can be found at
# https://github.com/thelightdesigner/ScuffedWalls/blob/main/Functions.md
            
# DM @thelightdesigner#1337 for more help?

# Using this tool requires an understanding of Noodle Extensions.
# https://github.com/Aeroluna/NoodleExtensions/blob/master/Documentation/AnimationDocs.md

# Playtest your maps

Workspace:Default

0: Import
   Path:ExpertPlusStandard_Old.dat

0:Import
path:retrx.dat
tobeat: 76.5

Workspace: Workspace 1

1:Note

Workspace: A Different Workspace

2:Note

0:AppendNotes

0:Wall
  position:[{ 5+6 }, Random(1,5), { 5+6+Random(2,10) }]
  scale:[RandomInt(5,0),RandomInt(5,0),RandomInt(5,0)]
  
0:Wall
  repeat:100
  position:[{repeat/10},0,0]
  scale:[0.1,1,1]
  color:HSLtoRGB({repeat/100},1,0.5)
  
var:SomeVariableName
  data:5
  recompute:0

0:Wall
  NJS:SomeVariableName
  
var:Grey
data:Random(0,1)
recompute:1

#Creates walls with random shades of grey
0:Wall
color:[Grey,Grey,Grey,1]
repeat:15

0:Wall
  repeat:60
  repeataddtime:0.05
  scale:[0.25,0.25,0.25]
  position:[{repeat/8},{Sin(repeat/2)}]
  
5:AppendToAllWallsBetween
  toBeat:10
  track: FurryTrack
  appendTechnique:1


0:AppendWalls
   time:{_time * 2}
   appendtechnique:1
   
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

0:AppendNotes
   time:{_time * 2}
   appendtechnique:1
   
5:AppendToAllEventsBetween
   toBeat:10
   appendTechnique:2
   lightType:1,3,0
   converttorainbow: true
   rainbowfactor:1
   
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
  animatedefiniteposition:[0,0,0,0]
  
5:ModelToWall
 Path:model.dae
 hasAnimation:false
 spreadspawntime:1
 normal:true
 track:FurryTrack
 duration: 30
 
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
  
  0:AnimateTrack
 track:RandomShit
 duration:1
 animatedissolve:[0,0],[0,1]
 animatedissolvearrow:[0,0],[0,1]
 
 0:AnimateTrack
 track:RandomShit
 duration:1
 animatedissolve:[0,0],[1,1]
 animatedissolvearrow:[0,0],[1,1]
 
 0:AssignPathAnimation
 track:RandomShit
 duration:8
 AnimateRotation:[0,0,0,0],[0,0,0,0.5,"easeOutQuad"],[0,0,0,1]
 easing:easeInOutSine
 
 3:AssignPlayerToTrack
   track:BigMovement
   
0:AnimateTrack
track:BigMovement
animateposition: [-2,5,6,0],[1,1,1,1]

3:ParentTrack
  ParentTrack:BigMovement
  ChildTracks:["rightnotes","leftnotes"]
  
5:PointDefinition
    name:UpDownPoints
    points:[0,0,0,0],[0,15,0,0.5,"easeInOutSine"],[0,0,0,1,"easeInOutSine"]

15:Wall
    DefineAnimateDefinitePosition:UpDownPoints
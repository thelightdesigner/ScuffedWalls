
Workspace

0: Import
 Path:Eplus.dat
 type: 2

0:Blackout




Workspace

0: Import
 Path:HardStandard.dat
	#pathway
40:AppendToAllWallsBetween
  toBeat: 73
  NJSOffset: 3
  NJS: 14
  track: RainbowTrack
  AnimateDissolve:[0,0],[1,0.1],[1,1]

0:AnimateTrack
track:RainbowTrack
duration: 1
AnimateDissolve:[0,0],[0,1]
40:AnimateTrack
track:RainbowTrack
duration: 1
AnimateDissolve:[0,0],[1,1]
100:AnimateTrack
track:RainbowTrack
duration: 0
AnimateDissolve:[0,0]

	#thin pathway
73:AppendToAllWallsBetween
  toBeat: 87.9
  NJSOffset: 3
  NJS: 14
  track: SideNotesDissolve
  AnimateDissolve:[0,0],[1,0.1],[1,0.5],[0,0.51]


	#noodle helix
87.9:AppendToAllWallsBetween
  toBeat: 101
  NJSOffset: 3
  NJS: 10
  track: RainbowHelixDissolve
  AnimateDissolve:[0,0],[1,0.1],[1,0.5],[0,0.51]

0:AnimateTrack
track:RainbowHelixDissolve
duration: 1
AnimateDissolve:[0,0],[0,1]
87:AnimateTrack
track:RainbowHelixDissolve
duration: 1
AnimateDissolve:[0,0],[1,1]
99:AnimateTrack
track:RainbowHelixDissolve
duration: 1
AnimateDissolve:[1,0],[0,1]




	#twist lines
136:AppendToAllWallsBetween
  toBeat: 168
  NJSOffset: 4
  NJS: 11
  track: WindingPath
  AnimateDissolve:[0,0],[1,0.1],[1,0.5],[0,0.51]

130:AssignPathAnimation
  track:WindingPath
  duration:0
  easing:easeInOutSine
  AnimateRotation:[0,20,0,0],[0,-10,-20,0.3,"easeInOutSine"],[0,10,20,0.4,"easeInOutSine"],[0,0,0,0.5,"easeInOutSine"]
136:AssignPathAnimation
  track:WindingPath
  duration:16
  easing:easeInOutSine
  AnimateRotation:[0,-30,0,0],[0,10,20,0.3,"easeInOutSine"],[0,-10,-20,0.4,"easeInOutSine"],[0,0,0,0.5,"easeInOutSine"]
152:AssignPathAnimation
  track:WindingPath
  duration:16
  easing:easeInOutSine
  AnimateRotation:[0,10,0,0],[0,-30,-30,0.3,"easeInOutSine"],[0,10,10,0.4,"easeInOutSine"],[0,0,0,0.5,"easeInOutSine"]
  


	#mountians path
168:AppendToAllWallsBetween
  toBeat:199.9
  NJSOffset: 1
  NJS: 15
  AnimateDissolve:[0,0],[1,0.1],[1,0.5],[0,0.51]
  track: MountainTrack


0:AppendToAllWallsBetween
toBeat:500
Njs:18
NjsOffset:-0.8
AppendTechnique: 0






Workspace

	#second planet
39:ModelToWall
  path: planet2.dae
  spreadspawntime:1
 AnimateDissolve:[0,0],[1,0.1],[1,0.9],[0,1]
  duration:  32
  track: MusicPlanets

8:AnimateTrack
  track: MusicPlanets
  duration: 8
  repeat:7
  repeataddtime:8
  AnimatePosition:[0,0,0,0,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.33,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.66,"easeInOutSine"],[0,0,0,1,"easeInOutSine"]

	#second stars
38: Wall
  AnimateDefinitePosition:[Random(-25,25),Random(-10,25),Random(20,60),0]
 repeat:60
 repeataddtime: 0.02
 spreadspawntime:1
 duration:32
 scale:[Random(0.1,0.2),Random(0.1,0.2),Random(0.1,0.2)]
 AnimateDissolve:[0,0],[1,0.1],[1,0.9],[0,1]
 track:MusicStars


69:AnimateTrack
  track: MusicPlanets
  duration: 3
  AnimateRotation:[0,0,0,0],[0,0,120,1,"easeInCubic"]

69:AnimateTrack
  track: MusicStars
  duration: 3
  AnimateRotation:[0,0,0,0],[0,0,120,1,"easeInCubic"]


100:AnimateTrack
  track: MusicPlanets
  duration: 0
  AnimateRotation:[0,0,0,0]

100:AnimateTrack
  track: MusicStars
  duration: 0
  AnimateRotation:[0,0,0,0]


	#side briks
72:ModelToWall
  path: morebricks.dae
  normal:true
  track:SideNotesDissolve
  NJS: 10
  color:[0,0,0,-50]
  rotation:[10,0,0]
 AnimateDissolve:[0,0],[1,0.1],[1,0.9],[0,1]
  NJSOffset:2

	#side notes
72:ModelToWall
  path: notes.dae
  normal:true
  track:SideNotesDissolve
  NJS: 10
  rotation:[10,0,0]
 AnimateDissolve:[0,0],[1,0.1],[1,0.5],[0,0.51]
  NJSOffset:2

#side track
0:AnimateTrack
track:SideNotesDissolve
duration: 1
AnimateDissolve:[0,0],[0,1]
71:AnimateTrack
track:SideNotesDissolve
duration: 1
AnimateDissolve:[0,0],[1,1]
99:AnimateTrack
track:SideNotesDissolve
duration: 1
AnimateDissolve:[1,0],[0,1]


	#stream rainbow music pulse thing
104:ModelToWall
  path: stream.dae
  normal:true
  track: WaveformBounce
  Njsoffset:2
  AnimateDissolve:[0,0],[1,0.1],[1,0.5],[0,0.51]
AnimateColor:[2,0,0,2,0],[2,2,0,2,0.1],[0,2,2,2,0.2],[0,0,2,2,0.3],[2,0,2,2,0.4],[2,0,0,2,0.5]



#wave
103.8:AssignPathAnimation
track: WaveformBounce
duration:0.2
repeat:8
repeataddtime:2
easing:easeInExpo
AnimateScale:[1,0.01,1,0],[1,2,1,0.1],[1,0.02,1,0.15,"easeInOutSine"],[1,0.5,1,0.23,"easeInOutSine"],[1,0.01,1,0.33,"easeInOutSine"],[1,1,1,0.45,"easeInOutSine"],[1,0.01,1,0.5,"easeInOutSine"]

104.8:AssignPathAnimation
track: WaveformBounce
duration:0.2
repeat:8
repeataddtime:2
easing:easeInExpo
AnimateScale:[1,0.01,1,0],[1,4,1,0.1],[1,0.02,1,0.15,"easeInOutSine"],[1,0.2,1,0.23,"easeInOutSine"],[1,0.01,1,0.33,"easeInOutSine"],[1,2,1,0.45,"easeInOutSine"],[1,0.01,1,0.5,"easeInOutSine"]

104:AssignPathAnimation
track: WaveformBounce
duration:0.8
repeat:16
repeataddtime:1
easing:easeOutElastic
AnimateColor:[0,0,0,1,0]
AnimateScale:[1,0.01,1,0],[1,0.01,1,1]

120.8:AssignPathAnimation
track: WaveformBounce
duration:0.2
repeat:8
repeataddtime:2
easing:easeInExpo
AnimateScale:[1,0.01,1,0],[1,2,1,0.1],[1,0.02,1,0.15,"easeInOutSine"],[1,0.5,1,0.23,"easeInOutSine"],[1,0.01,1,0.33,"easeInOutSine"],[1,1,1,0.45,"easeInOutSine"],[1,0.01,1,0.5,"easeInOutSine"]

121.8:AssignPathAnimation
track: WaveformBounce
duration:0.2
repeat:7
repeataddtime:2
easing:easeInExpo
AnimateScale:[1,0.01,1,0],[1,4,1,0.1],[1,0.02,1,0.15,"easeInOutSine"],[1,0.2,1,0.23,"easeInOutSine"],[1,0.01,1,0.33,"easeInOutSine"],[1,2,1,0.45,"easeInOutSine"],[1,0.01,1,0.5,"easeInOutSine"]

121:AssignPathAnimation
track: WaveformBounce
duration:0.8
repeat:16
repeataddtime:1
easing:easeOutElastic
AnimateColor:[0,0,0,1,0]
AnimateScale:[1,0.01,1,0],[1,0.01,1,1]


#rot
104:AssignPathAnimation
track: WaveformBounce
duration:0
 AnimateRotation:[0,0,0,0],[0,0,25,0.13],[0,0,50,0.25],[0,0,75,0.38],[0,0,100,0.5]
108:AssignPathAnimation
track: WaveformBounce
duration:0
 AnimateRotation:[0,0,0,0],[0,0,-25,0.13],[0,0,-50,0.25],[0,0,-75,0.38],[0,0,-100,0.5]
  repeat:2
  repeataddtime:16
112:AssignPathAnimation
track: WaveformBounce
duration:0
 AnimateRotation:[0,0,0,0]
  repeat:2
  repeataddtime:16
116:AssignPathAnimation
track: WaveformBounce
duration:0
 AnimateRotation:[0,0,90,0]
  repeat:2
  repeataddtime:16

120.5:AssignPathAnimation
track: WaveformBounce
duration:0
  AnimateRotation:[0,0,0,0],[0,0,25,0.13],[0,0,50,0.25],[0,0,75,0.38],[0,0,100,0.5]
121:AssignPathAnimation
track: WaveformBounce
duration:0
  AnimateRotation:[0,0,0,0],[0,0,-25,0.13],[0,0,-50,0.25],[0,0,-75,0.38],[0,0,-100,0.5]
121.5:AssignPathAnimation
track: WaveformBounce
duration:0
 AnimateRotation:[0,0,0,0],[0,0,25,0.13],[0,0,50,0.25],[0,0,75,0.38],[0,0,100,0.5]

#dissolve
0:AnimateTrack
track:WaveformBounce
duration: 1
AnimateDissolve:[0,0],[0,1]
104:AnimateTrack
track:WaveformBounce
duration: 0.2
AnimateDissolve:[0,0],[1,1]
135:AnimateTrack
track:WaveformBounce
duration: 1
AnimateDissolve:[1,0],[0,1]


	#rainbow note avalanche
136:Note
repeat:124
repeatAddTime:0.26
localRotation:[Random(0,360),Random(0,360),Random(0,360)]
Rotation:[120,0,0]
Position:[Random(-12,-6),Random(8,18)]
NJS:10
NJSOffset:4
track: RainbowNoteAvalanche
fake:true
isInteractable: false

136:Note
repeat:124
repeatAddTime:0.26
localRotation:[Random(0,360),Random(0,360),Random(0,360)]
Rotation:[120,0,0]
Position:[Random(6,12),Random(8,18)]
track: RainbowNoteAvalanche

NJS:10
NJSOffset:4
fake:true
isInteractable: false

130:AssignpathAnimation
duration:0
track: RainbowNoteAvalanche
AnimatePosition:[0,0,-10,0],[0,0,-10,1]
AnimateDissolve:[0,0],[1,0.1],[1,0.3],[0,0.8]
AnimateDissolveArrow:[0,0],[1,0.1],[1,0.3],[0,0.8]
AnimateScale:[2,2,2,0],[2,2,2,1]
AnimateColor:[1, 0, 0, 1,0], [0, 1, 0, 0.5, 0.0832], [0, 0, 1, 1, 0.15], [1, 0, 0, 1, 0.23], [0, 1, 0, 1, 0.30], [0, 0, 1, 1, 0.38], [1, 0, 0, 1, 0.45], [0, 1, 0, 1, 0.52], [0, 0, 1, 1, 0.60], [1, 0, 0, 1, 0.68], [0, 1, 0, 1, 0.75], [0, 0, 1, 1, 0.84],[1,1,1,1,1]

0:AnimateTrack
track:RainbowNoteAvalanche
duration: 1
AnimateDissolve:[0,0],[0,1]
AnimateDissolveArrow:[0,0],[0,1]
136:AnimateTrack
track:RainbowNoteAvalanche
duration: 1
AnimateDissolve:[0,0],[1,1]
AnimateDissolveArrow:[0,0],[1,1]


	#music notes
136:ModelToWall
  path: masnotes.dae
  normal:true
  NJS: 13
  NJSOffset:1
  AnimateDissolve:[0,0],[1,0.1],[1,0.5],[0,0.51]
  track:NoteBounce

0:AnimateTrack
track:NoteBounce
duration: 1
AnimateDissolve:[0,0],[0,1]
136:AnimateTrack
track:NoteBounce
duration: 1
AnimateDissolve:[0,0],[1,1]

#bounce
136:AnimateTrack



0:AppendToAllWallsBetween
toBeat:500
Njs:18
NjsOffset:-0.8
AppendTechnique: 0






	#other effects
Workspace

#jam jam star
40:AnimateTrack
  track: MusicStars
  duration: 0.33
  AnimateColor:[10,10,10,10,0],[10,10,10,10,0.5],[0,0,0,0,0.5],[0,0,0,0,1]
41:AnimateTrack
  track: MusicStars
  duration: 0.25
  repeat: 3
  repeataddtime: 0.25
  AnimateColor:[10,10,10,10,0],[10,10,10,10,0.5],[0,0,0,0,0.5],[0,0,0,0,1]
42:AnimateTrack
  track: MusicStars
  duration: 0.25
  repeat: 3
  repeataddtime: 0.25
  AnimateColor:[10,10,10,10,0],[10,10,10,10,0.5],[0,0,0,0,0.5],[0,0,0,0,1]
43:AnimateTrack
  track: MusicStars
  duration: 0.33
  repeat:2
  repeataddtime:0.5
  AnimateColor:[10,10,10,10,0],[10,10,10,10,0.5],[0,0,0,0,0.5],[0,0,0,0,1]

#jam jam plant

40:AnimateTrack
  track: MusicPlanets
  duration: 0.8
  AnimateColor:[Random(0,10),Random(0,10),Random(0,10),10,0],[0,0,0,0,1,"easeOutSine"]
  repeat: 5
  repeataddtime:0.8


Workspace
40:CloneFromWorkspaceByIndex
index: 3
type:3
addtime:4
toBeat:10000
44:CloneFromWorkspaceByIndex
index: 3
type:3
addtime:8
toBeat:10000
48:CloneFromWorkspaceByIndex
index: 3
type:3
addtime:12
toBeat:10000
52:CloneFromWorkspaceByIndex
index: 3
type:3
addtime:16
toBeat:10000
56:CloneFromWorkspaceByIndex
index: 3
type:3
addtime:20
toBeat:10000
60:CloneFromWorkspaceByIndex
index: 3
type:3
addtime:24
toBeat:10000
64:CloneFromWorkspaceByIndex
index: 3
type:3
addtime:28
toBeat:10000


Workspace
	#first bricks
3:ModelToWall
  path: firstbricks.dae
  duration: 35
  spreadspawntime: 0.2
  AnimateDissolve: [0,0],[1,0.2],[1,1]
 color:[0.2,0,0,-50]
 hasAnimation: true
  track: FirstBricks

0:AnimateTrack
track:FirstBricks
AnimatePosition:[0,0,0,0],[0,0,0,1]
duration: 1

34:AnimateTrack
track:FirstBricks
duration:  8
AnimatePosition:[0,0,0,0],[0,0,-40,1,"easeInSine"]

	#first trees
3:ModelToWall
  path: firsttres.dae
  duration: 35
  spreadspawntime: 0.2
  AnimateDissolve: [0,0],[1,0.2],[1,1]
 hasAnimation: true
  track: FirstBricks


	#next bricks
34:ModelToWall
  path: nextbricks.dae
  duration: 6
  color:[0.2,0,0,-50]
  AnimateDissolve: [0,0],[1,0.4],[1,1]
  track: FirstBricks



	#real or lie
35:ModelToWall
  path: realorlie.dae
  duration:  5.5
  AnimateDissolve: [0,0],[1,0.4],[1,1]
  AnimatePosition:[Random(-1,1),Random(-1,1),Random(-1,1),0,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.33,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.66,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),1,"easeInOutSine"]
  color: [1.8,0,3.2,10]
  track: FirstBricks


	#planet
1:ModelToWall
  path: planet.dae
  spreadspawntime:1
  duration:  37
 AnimateDissolve:[0,0],[1,0.1],[1,0.9],[0,1]
  track: MusicPlanets

	#stars
0: Wall
  AnimateDefinitePosition:[Random(-30,30),Random(10,30),Random(20,60),0]
 repeat:80
 repeataddtime: 0.02
 rotation:[10,0,0]
 duration:36
 scale:[Random(0.1,0.2),Random(0.1,0.2),Random(0.1,0.2)]
 AnimateDissolve:[0,0],[1,0.1],[1,0.9],[0,1]
 track:MusicStars



0:AnimateTrack
  track: MusicPlanets
  duration: 8
  AnimateColor:[0,0,0,0,0],[0,0,0,0,1]
 AnimateDissolve:[0,0],[1,1]



8:AnimateTrack
  track: MusicPlanets
  duration: 4
  repeat:4
  repeataddtime:8
  AnimateColor:[10,0,10,10,0],[0,0,0,0,1,"easeOutSine"]
12:AnimateTrack
  track: MusicPlanets
  duration: 4
  repeat:4
  repeataddtime:8
  AnimateColor:[0,5,10,10,0],[0,0,0,0,1,"easeOutSine"]



0:AnimateTrack
  track: MusicStars
  duration: 8
  AnimateColor:[0,0,0,0,0],[0,0,0,0,1]
 AnimateDissolve:[0,0],[1,1]

8:AnimateTrack
  track: MusicStars
  duration: 4
  repeat:8
  repeataddtime:4
  AnimateColor:[10,10,10,10,0],[0,0,0,0,1,"easeOutSine"]



0:AppendToAllWallsBetween
toBeat:500
Njs:18
NjsOffset:-0.8
AppendTechnique: 0


Workspace


	#copy paste

	#stars, planets
0:CloneFromWorkspaceByIndex
  index:2
  fromBeat:35
  toBeat:72
  addTime:160
	#floor
0:CloneFromWorkspaceByIndex
  index:1
  fromBeat:35
  toBeat:72
 addTime:160
	#customevents
0:CloneFromWorkspaceByIndex
  index:4
  fromBeat:30
  toBeat:72
 addTime:160
0:CloneFromWorkspaceByIndex
  index:3
  fromBeat:30
  toBeat:72
 addTime:160

180:AnimateTrack
track:RainbowNoteAvalanche
duration: 0
AnimateDissolve:[0,0]
AnimateDissolveArrow:[0,0]

	#beat drop
0:CloneFromWorkspaceByIndex
  index:2
  fromBeat:100
  toBeat:168
  addTime:160
0:CloneFromWorkspaceByIndex
  index:1
  fromBeat:100
  toBeat:168
  addTime:160

	#last stars
328: Wall
  AnimateDefinitePosition:[Random(-25,25),Random(-25,25),Random(20,60),0]
 repeat:30
 repeataddtime: 0.02
 duration:10
  color:[10,10,10,10]
 scale:[Random(0.1,0.2),Random(0.1,0.2),Random(0.1,0.2)]
 AnimateDissolve:[0,0],[1,0.3],[1,0.6],[0,1]
  track:SpinStars

328:AnimateTrack
  track:SpinStars
  duration:10
  AnimateRotation:[0,0,180,0],[0,0,0,1,"easeOutQuint"]



0:AppendToAllWallsBetween
toBeat:500
Njs:18
NjsOffset:-0.8
AppendTechnique: 0

Workspace

	#mountains, trees, stars
168:ModelToWall
  path: mountain.dae
  normal:true
  NJS: 15
  track:MountainTrack
 AnimateDissolve:[0,0],[1,0.1],[1,0.5],[0,0.51]
  NJSOffset:10

0:AnimateTrack
  track:MountainTrack
  duration:1
  AnimateDissolve:[0,0]

167:AnimateTrack
  track:MountainTrack
  duration:1
  AnimateDissolve:[0,0],[1,1]

173: Wall
  AnimateDefinitePosition:[Random(-40,40),60,Random(10,70),0]
  repeat:80
  repeataddtime: 0.02
  color:[10,10,10,10]
  rotation:[40,0,0]
  duration:24
  scale:[Random(0.1,0.2),Random(0.1,0.2),Random(0.1,0.2)]
  AnimateDissolve:[0,0],[1,0.1],[1,0.9],[0,1]

	#piano
232:ModelToWall
  path: paino.dae
  normal:true
  NJS: 10
  AnimateDissolve:[0,0],[1,0.1],[1,0.5],[0,0.51]
  NJSOffset: 10
  track: PianoZig


220:AssignPathAnimation
  track:PianoZig
  duration:0
  AnimateRotation:[0,20,0,0],[0,-10,-20,0.3,"easeInOutSine"],[0,10,20,0.4,"easeInOutSine"],[0,0,0,0.5,"easeInOutSine"]
232:AssignPathAnimation
  track:PianoZig
  duration:8
  easing:easeInOutSine
  AnimateRotation:[0,-30,0,0],[0,10,20,0.3,"easeInOutSine"],[0,-10,-20,0.4,"easeInOutSine"],[0,0,0,0.5,"easeInOutSine"]
240:AssignPathAnimation
  track:PianoZig
  duration:8
  easing:easeInOutSine
  AnimateRotation:[0,10,0,0],[0,-30,-30,0.3,"easeInOutSine"],[0,10,10,0.4,"easeInOutSine"],[0,0,0,0.5,"easeInOutSine"]
248:AssignPathAnimation
  track:PianoZig
  duration:8
  easing:easeInOutSine
  AnimateRotation:[0,-30,0,0],[0,10,20,0.3,"easeInOutSine"],[0,-10,-20,0.4,"easeInOutSine"],[0,0,0,0.5,"easeInOutSine"]

101:ModelToWall
  path: lie.dae
  duration:2
  NJSOffset: -10
   AnimateDissolve:[0,0],[1,0.3],[1,0.8],[0,1]
  AnimateRotation:[0,0,-180,0],[0,0,0,0.8,"easeOutQuint"]
  AnimatePosition:[0,2,0,0],[0,2,20,0.7,"easeOutQuint"],[0,2,0,1,"easeInQuint"]
261:ModelToWall
  path: lie.dae
  duration:2
  NJSOffset: -10
   AnimateDissolve:[0,0],[1,0.3],[1,0.8],[0,1]
  AnimateRotation:[0,0,-180,0],[0,0,0,0.8,"easeOutQuint"]
  AnimatePosition:[0,2,0,0],[0,2,20,0.7,"easeOutQuint"],[0,2,0,1,"easeInQuint"]




0:AppendToAllWallsBetween
toBeat:500
Njs:18
NjsOffset:-0.8
AppendTechnique: 0

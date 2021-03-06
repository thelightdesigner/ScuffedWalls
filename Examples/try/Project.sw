
workspace:point definitions
0:PointDefinition
  name:FadeInOut
  Points:[0,0],[1,0.1],[1,0.9],[0,1]




Workspace:main
	#intro scene
1: ModelToWall
  path: models\flower.dae
  duration:31
  animatedissolve:[1,0.9],[0,1]
  spreadspawntime:1
  track:intro
1: ModelToWall
  path: models\firstbranch.dae
  duration:31
  track:intro
  animatedissolve:[1,0.9],[0,1]
  animateposition:[0,0,0,0.6],[5,0,0,0.9,"easeInSine"]
1: ModelToWall
  path: models\firstbranch.dae
  duration:32
  animatedissolve:[1,0.9],[0,1]
  track:intro
  animateposition:[-10,0,0,0.6],[-15,0,0,0.9,"easeInSine"]

0:AnimateTrack
  track:intro
  duration:0
  animatedissolve:[0,0]
3:AnimateTrack
  track:intro
  duration:7
  animatedissolve:[0,0],[1,1]

22:AnimateTrack
  track:intro
  duration:14
  animateposition:[0,0,0,0],[0,0,-65,1,"easeInSine"]
  animatedissolve:[1,0],[1,0.9],[0,1]




36: ModelToWall
  path:models\viralspiral.dae
  normal:true
  definitetime:beats
  NJSOffset:3
  NJS:10
  AnimateDissolve:[0,0],[1,0.1]
  track:hugewalls

36: ModelToWall
  path:models\hugewalls1.dae
  AnimatePosition:[0,0,60,0]
  normal:true
  NJSOffset:6
  track:hugewalls
  NJS:8
  AnimateDissolve:[0,0],[1,0.1]

0:animatetrack
  track:hugewalls
  duration:0
  animatedissolve:[0,0]

30:animatetrack
  track:hugewalls
  duration:6
  animatedissolve:[0,0],[1,1]


	#speed particles
97:Wall
  definitetime:Beats
  repeat:40
  repeataddtime:0.08
  scale:[0.08,0.08]
  duration:-1.8
  position:[Random(-25,25),Random(-15,18)]
  color:[1,1,2,1]
98:Wall
  definitetime:Beats
  repeat:40
  repeataddtime:0.055
  scale:[0.08,0.08]
  duration:-1.8
  position:[Random(-15,15),Random(-12,15)]
  color:[1,1,2,1]
99:Wall
  definitetime:Beats
  repeat:60
  repeataddtime:0.02
  scale:[0.08,0.08]
  duration:-1.8
  position:[Random(-10,10),Random(-7,13)]
  color:[1,1,2,1]
  
	#river
99:ModelToWall
  path:models\riverwater.dae
  NjsOffset:5
  AnimatePosition:[0,0,20,0]
  Njs:15.5
  Normal:true
  color:[0.6,1,3,15]
  AnimateDissolve:[0,0],[1,0.05]
  track:river

100:ModelToWall
  path:models\riverrocks.dae
  NjsOffset:5
  AnimatePosition:[0,0,20,0]
  Njs:9
  Normal:true
  color:[-15,-15,-15,2]
  AnimateDissolve:[0,0],[1,0.05],[1,0.7],[0,0.8]
  track:river

100:ModelToWall
  path:models\river.dae
  NjsOffset:5
  AnimatePosition:[0,0,20,0]
  Njs:9
  AnimateDissolve:[0,0],[1,0.05],[1,0.7],[0,0.8]
  track:river
  Normal:true

0:AnimateTrack
  track:river
  animatedissolve:[0,0]
  duration:1
99.75:AnimateTrack
  track:river
  animatedissolve:[0,0],[1,1]
  duration:1

	#things
100:Wall
  repeat:200
  repeataddtime:0.3
  scale:[0.2,0.2]
  position:[Random(-15,15),Random(-3,1)]
  animateposition:[0,0,20,0]
  color:[Random(0,2),Random(0,2),Random(0,2),1]
  NJS:15
  duration:1.5
  AnimateDissolve:[0,0],[1,0.05],[1,0.7],[0,0.8]
  NJSOffset:6
  track:river



	#clouds
166:ModelToWall
  path:models\cloudsleft.dae
  normal:true
  Njs:20
  Njsoffset:15
  animateposition:[0,0,160,0]
  color:[5,5,5,1]
  rotation:[5,5,0]
  defineanimatedissolve:FadeInOut
  track:clouds
166:ModelToWall
  path:models\cloudsright.dae
  normal:true
  Njs:20
  Njsoffset:15
  color:[5,5,5,1]
  defineanimatedissolve:FadeInOut
  animateposition:[0,0,160,0]
  rotation:[5,-5,0]
  track:clouds

0:animatetrack
  track:clouds
  duration:0
  animatedissolve:[0,0]
164:animatetrack
  track:clouds
  duration:1
  animatedissolve:[0,0],[1,1]


	#shooting stars
164:Wall
  repeat:50
  repeataddtime:0.4
  scale:[0.2,0.2,18]
  Njs:10
  NjsOffset:20
  position:[Random(0,80),Random(-100,100)]
  color:[Random(0,20),Random(0,20),Random(0,20),20]
  rotation:[Random(0,360),90,0]
  defineanimatedissolve:FadeInOut
  track:stars

0:ParentTrack
  parenttrack:starsmove
  childtracks:["stars"]
0:AnimateTrack
  track:starsmove
  duration:1
  animaterotation:[-10,0,0,0]
  animateposition:[0,0,220,0]

0:animatetrack
  track:stars
  duration:0
  animatedissolve:[0,0]
164:animatetrack
  track:stars
  duration:1
  animatedissolve:[0,0],[1,1]

	#moon
164:ModelToWall
  definiteTime:beats
  definiteduration:28
  color:[50,50,50,50]
  path:models\moon.dae
  track:clouds

	




	#big passage 1
196: ModelToWall
  path:models\bigpassage.dae
  normal:true
  NJSOffset:4
  animatedissolve:[0,0],[1,0.05],[1,0.7],[0,0.7]
  NJS:50
  color:[0.2,0.2,0.2,-1]
  animateposition:[0,-1000,120,0],[0,0,120,0]
  animaterotation:[0,0,0,0],[0,0,20,0.7]
  track:passage1

196: ModelToWall
  path:models\bigpassagelarger.dae
  normal:true
  NJSOffset:4
  NJS:50
  color:[21,21,25,25]
  animatedissolve:[0,0],[0.1,0.05],[0.1,0.7],[0,0.7]
  animateposition:[0,-1000,120,0],[0,0,120,0]
  #animatescale:[1.02,1.02,1.02,0]
  animaterotation:[0,0,0,0],[0,0,20,0.7]
  track:passage2

0:AnimateTrack
  track:passage2
  animatedissolve:[0,0]
  duration:1
196:AnimateTrack
  track:passage2
  animatedissolve:[0,0],[1,1]
  duration:0.5
0:AnimateTrack
  track:passage1
  animatedissolve:[0,0]
  duration:1
196:AnimateTrack
  track:passage1
  animatedissolve:[0,0],[1,1]
  duration:0.5

196:AnimateTrack
  repeat:24
  repeataddtime:1
  track:passage1
  duration:1
  animatecolor:[4,4,8,4,0],[0.2,0.2,0.2,-1,1,"easeOutSine"]
220:AnimateTrack
  repeat:8
  repeataddtime:0.5
  track:passage1
  duration:0.5
  animatecolor:[4,4,8,4,0],[0.2,0.2,0.2,-1,1,"easeOutSine"]

195.5:AnimateTrack
  repeat:25
  repeataddtime:1
  track:passage2
  duration:1
  animatecolor:[21,21,25,25,0],[0.2,0.2,0.2,-1,1,"easeOutExpo"]
220.25:AnimateTrack
  repeat:8
  repeataddtime:0.5
  track:passage2
  duration:0.5
  animatecolor:[21,21,25,25,0],[0.2,0.2,0.2,-1,1,"easeOutExpo"]

	#drop
	#things
260:Wall
  repeat:600
  repeataddtime:0.1
  scale:[0.1,0.1,5]
  duration:1
  NJSOffset:2
  NJS:20
  animateposition:[0,0,-20,0]
  color:[11,11,15,1]
  animatedissolve:[0,0],[1,0.1],[1,0.7],[0,0.8]
  position:[Random(10,120),Random(-30,30)]
  track:coolness1

0:AnimateTrack
  track:coolness1
  duration:1
  AnimateDissolve:[0,0]
260:AnimateTrack
  track:coolness1
  duration:1
  AnimateDissolve:[0,0],[1,1]

260:AnimateTrack
  track:coolness1
  duration:16
  repeat:1
  repeataddtime:32
  animaterotation: [120,-90,0,0],[75,-90,0,1]
  AnimateColor:[1,1,5,1,0],[1,5,2,1,0.33],[1,1,5,1,0.66],[1,5,2,1,1]
276:AnimateTrack
  track:coolness1
  duration:16
  repeat:1
  repeataddtime:32
  animaterotation: [75,-90,0,0],[120,-90,0,1]
  AnimateColor:[1,1,5,1,0],[1,5,2,1,0.33],[1,1,5,1,0.66],[1,5,2,1,1]



	#dont look back

324: ModelToWall
  path:models\hugewalls2.dae
  AnimatePosition:[0,0,-90,0]
  rotation:[0,180,0]
  normal:true
  NJSOffset:6
  track:hugewalls
  NJS:8
  AnimateDissolve:[0,0],[1,0.1]

200:animatetrack
  track:hugewalls
  duration:0
  animatedissolve:[0,0]

318:animatetrack
  track:hugewalls
  duration:6
  animatedissolve:[0,0],[1,1]

416:animatetrack
  track:hugewalls
  duration:5
  animatedissolve:[1,0],[0,1]


	#clouds
422:ModelToWall
  path:models\cloudsleft.dae
  normal:true
  Njs:20
  Njsoffset:15
  animateposition:[0,0,160,0]
  color:[5,5,5,1]
  rotation:[5,5,0]
  defineanimatedissolve:FadeInOut
  track:clouds
422:ModelToWall
  path:models\cloudsright.dae
  normal:true
  Njs:20
  Njsoffset:15
  color:[5,5,5,1]
  defineanimatedissolve:FadeInOut
  animateposition:[0,0,160,0]
  rotation:[5,-5,0]
  track:clouds

300:animatetrack
  track:clouds
  duration:0
  animatedissolve:[0,0]
420:animatetrack
  track:clouds
  duration:1
  animatedissolve:[0,0],[1,1]


	#shooting stars
420:Wall
  repeat:50
  repeataddtime:0.4
  scale:[0.2,0.2,18]
  Njs:10
  NjsOffset:20
  position:[Random(0,80),Random(-100,100)]
  color:[Random(0,20),Random(0,20),Random(0,20),20]
  rotation:[Random(0,360),90,0]
  defineanimatedissolve:FadeInOut
  track:stars

300:animatetrack
  track:stars
  duration:0
  animatedissolve:[0,0]
420:animatetrack
  track:stars
  duration:1
  animatedissolve:[0,0],[1,1]

	#moon
420:ModelToWall
  definiteTime:beats
  definiteduration:28
  color:[50,50,50,50]
  path:models\moon.dae
  track:clouds


	#big passage 2
453: ModelToWall
  path:models\bigpassage.dae
  normal:true
  NJSOffset:4
  NJS:50
  track:passage1
  #color:[0.2,0.2,0.2,-1]
  animateposition:[0,-1000,120,0],[0,0,120,0]
  #animatecolor:[2.1,2.7,3,2,0],[0.1,0.1,0.1,1,0.4],[-1,-1,-1,-20,0.5]
  animatelocalrotation:[0,0,0,0],[0,180,0,0.7]
  #animaterotation:[20,0,0,0],[0,0,0,0.5]

453: ModelToWall
  path:models\bigpassage.dae
  normal:true
  NJSOffset:4
  NJS:50
  track:passage2
  #color:[21,21,25,25]
  animatedissolve:[0.1,0]
  animateposition:[0,-1000,120,0],[0,0,120,0]
  animatescale:[1.02,1.02,1.02,0]
  animatelocalrotation:[0,0,0,0],[0,180,0,0.7]

452:AnimateTrack
  repeat:24
  repeataddtime:1
  track:passage1
  duration:1
  animatecolor:[4,4,8,4,0],[0.2,0.2,0.2,-1,1,"easeOutSine"]
476:AnimateTrack
  repeat:8
  repeataddtime:0.5
  track:passage1
  duration:0.5
  animatecolor:[4,4,8,4,0],[0.2,0.2,0.2,-1,1,"easeOutSine"]

451.5:AnimateTrack
  repeat:25
  repeataddtime:1
  track:passage2
  duration:1
  animatecolor:[21,21,25,25,0],[0.2,0.2,0.2,-1,1,"easeOutExpo"]
476.25:AnimateTrack
  repeat:8
  repeataddtime:0.5
  track:passage2
  duration:0.5
  animatecolor:[21,21,25,25,0],[0.2,0.2,0.2,-1,1,"easeOutExpo"]









	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8

Workspace:ending BRRRRR walls random helper
484:AnimateTrack
  duration:4
  track:VroooooomOffset
  animateposition:[0,3,0,0]

0:ParentTrack
  parentTrack:VroooooomOffset
  childTracks:["Vroooooom"]

497:AnimateTrack
 duration:1
  track:Vroooooom
 animatedissolve:[1,0],[0,1]

511:AnimateTrack
 duration:1
  track:Vroooooom
 animatedissolve:[1,0],[0,1]

 	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8

Workspace:ending BRRRRR walls random backwards
484:Wall
  repeat:100
  repeataddtime:0.06
  scale:[Random(8,10),Random(18,25),Random(8,8)]
  animatescale:[2,2,2,0]
  position:[0,Random(40,120)]
  NJS:300
  animateDissolve:[0,0],[1,0]
  NJSOffset:0
  animateposition:[0,0,-100,0]
  rotation:[0,180,Random(0,360)]
  animaterotation:[0,0,0,0],[0,0,170,1,"easeInQuad"]
  localRotation:[Random(0,360),Random(0,360),Random(0,360)]
  track:Vroooooom
animatecolor:[23.5, 5.2, 15.5,10,0.2],[15.0, 5.2, 23.5,10,0.35],[5.2, 7.6, 23.5,10,0.5],[23.5, 5.2, 15.5,10,0.67],[15.0, 5.2, 23.5,10,0.88],[5.2, 21.4, 23.5,10,1]
484:Wall
  repeat:100
  repeataddtime:0.06
  scale:[Random(8,10),Random(18,25),Random(8,8)]
  position:[0,Random(40,120)]
  NJS:300
  animatescale:[2,2,2,0]
  NJSOffset:0
  animateDissolve:[0,0],[1,0]
  animateposition:[0,0,-100,0]
  rotation:[0,180,Random(0,360)]
  animaterotation:[0,0,0,0],[0,0,-170,1,"easeInQuad"]
  localRotation:[Random(0,360),Random(0,360),Random(0,360)]
  track:Vroooooom
animatecolor:[23.5, 5.2, 15.5,10,0.2],[15.0, 5.2, 23.5,10,0.35],[5.2, 7.6, 23.5,10,0.5],[23.5, 5.2, 15.5,10,0.67],[15.0, 5.2, 23.5,10,0.88],[5.2, 21.4, 23.5,10,1]

484:Wall
  repeat:400
  repeataddtime:0.015
  scale:[1,10,1]
  animateDissolve:[0,0],[1,0]
  color:[Random(0,20),Random(0,20),Random(0,20),20]
  position:[0,Random(10,120)]
  NJS:300
  NJSOffset:0
  rotation:[0,180,Random(0,360)]
  animateposition:[0,0,-100,0]
  track:Vroooooom

483:AnimateTrack
 duration:0
  track:Vroooooom
 animatedissolve:[0,0]

484:AnimateTrack
 duration:1
  track:Vroooooom
 animatedissolve:[0,0],[1,1]

489:AnimateTrack
 duration:1
  track:Vroooooom
 animatedissolve:[1,0],[0,1]

 	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8

Workspace:ending BRRRRR walls random forwards
492:Wall
  repeat:100
  repeataddtime:0.06
  scale:[Random(8,10),Random(18,25),Random(8,8)]
  animatescale:[2,2,2,0]
  position:[0,Random(40,120)]
  NJS:300
  NJSOffset:0
  duration:1
  animateDissolve:[0,0],[1,0]
  animateposition:[0,0,250,0]
  rotation:[0,0,Random(0,360)]
  animaterotation:[0,0,0,0],[0,0,170,1,"easeOutQuad"]
  localRotation:[Random(0,360),Random(0,360),Random(0,360)]
  track:Vroooooom
animatecolor:[23.5, 5.2, 15.5,10,0.2],[15.0, 5.2, 23.5,10,0.35],[5.2, 7.6, 23.5,10,0.5],[23.5, 5.2, 15.5,10,0.67],[15.0, 5.2, 23.5,10,0.88],[5.2, 21.4, 23.5,10,1]


492:Wall
  repeat:100
  repeataddtime:0.06
  scale:[Random(8,10),Random(18,25),Random(8,8)]
  position:[0,Random(60,120)]
  NJS:300
  duration:1
  animatescale:[2,2,2,0]
  NJSOffset:0
  animateposition:[0,0,250,0]
  animateDissolve:[0,0],[1,0]
  rotation:[0,0,Random(0,360)]
  animaterotation:[0,0,0,0],[0,0,-170,1,"easeOutQuad"]
  localRotation:[Random(0,360),Random(0,360),Random(0,360)]
  track:Vroooooom
animatecolor:[23.5, 5.2, 15.5,10,0.2],[15.0, 5.2, 23.5,10,0.35],[5.2, 7.6, 23.5,10,0.5],[23.5, 5.2, 15.5,10,0.67],[15.0, 5.2, 23.5,10,0.88],[5.2, 21.4, 23.5,10,1]

492:Wall
  repeat:400
  repeataddtime:0.015
  scale:[1,10,1]
  duration:1
  color:[Random(0,20),Random(0,20),Random(0,20),20]
 position:[0,Random(10,120)]
  animateDissolve:[0,0],[1,0]
  NJS:300
  NJSOffset:0
  rotation:[0,0,Random(0,360)]
  animateposition:[0,0,250,0]
  track:Vroooooom

491:AnimateTrack
 duration:0
  track:Vroooooom
 animatedissolve:[0,0]

492:AnimateTrack
 duration:1
  track:Vroooooom
 animatedissolve:[0,0],[1,1]
492:AnimateTrack
 duration:1
  track:Vroooooom
 animatedissolve:[0,0],[1,1]

	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8

Workspace:drop cloner
0:CloneFromWorkspace
  toBeat:10000
  addTime:16
  name:ending BRRRRR walls random forwards
0:CloneFromWorkspace
  toBeat:10000
  addTime:16
  name:ending BRRRRR walls random backwards


	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8

Workspace:ending BRRRRR MORE WALLS LETS GOOOOOO
516:Wall
  repeat:450
  repeataddtime:0.06
  scale:[Random(8,10),Random(18,25),Random(8,8)]
  animatescale:[2,2,2,0]
  position:[0,Random(40,120)]
  NJS:300
  animateDissolve:[0,0],[1,0]
  NJSOffset:0
  animateposition:[0,0,-100,0]
  rotation:[0,180,Random(0,360)]
  animaterotation:[0,0,0,0],[0,0,170,1,"easeInQuad"]
  localRotation:[Random(0,360),Random(0,360),Random(0,360)]
  track:Vroooooom
animatecolor:[23.5, 5.2, 15.5,10,0.2],[15.0, 5.2, 23.5,10,0.35],[5.2, 7.6, 23.5,10,0.5],[23.5, 5.2, 15.5,10,0.67],[15.0, 5.2, 23.5,10,0.88],[5.2, 21.4, 23.5,10,1]
516:Wall
  repeat:450
  repeataddtime:0.06
  scale:[Random(8,10),Random(18,25),Random(8,8)]
  position:[0,Random(40,120)]
  NJS:300
  animatescale:[2,2,2,0]
  NJSOffset:0
  animateDissolve:[0,0],[1,0]
  animateposition:[0,0,-100,0]
  rotation:[0,180,Random(0,360)]
  animaterotation:[0,0,0,0],[0,0,-170,1,"easeInQuad"]
  localRotation:[Random(0,360),Random(0,360),Random(0,360)]
  track:Vroooooom
animatecolor:[23.5, 5.2, 15.5,10,0.2],[15.0, 5.2, 23.5,10,0.35],[5.2, 7.6, 23.5,10,0.5],[23.5, 5.2, 15.5,10,0.67],[15.0, 5.2, 23.5,10,0.88],[5.2, 21.4, 23.5,10,1]

516:Wall
  repeat:2000
  repeataddtime:0.015
  scale:[1,10,1]
  animateDissolve:[0,0],[1,0]
  color:[Random(0,20),Random(0,20),Random(0,20),20]
  position:[0,Random(10,120)]
  NJS:300
  NJSOffset:0
  rotation:[0,180,Random(0,360)]
  animateposition:[0,0,-100,0]
  track:Vroooooom

515:AnimateTrack
 duration:0
  track:Vroooooom
 animatedissolve:[0,0]

516:AnimateTrack
 duration:1
  track:Vroooooom
 animatedissolve:[0,0],[1,1]


	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8

Workspace:flowerpower
	#flower
2.2: ModelToWall
  path: models\flowermodel.dae
  duration:36
  hasanimation: true
  track:flowerfloat
  animatedissolve:[1,0.94],[0,1]

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

292:AnimateTrack
track:flowerfloat
duration:32
repeat:16
repeataddtime:32
AnimatePosition:[0,0,0,0,"easeInOutSine"],[Random(-3,3),Random(-0.5,0),0,0.25,"easeInOutSine"],[Random(-3,3),Random(-0.5,0),0,0.5,"easeInOutSine"],[Random(-3,3),Random(-0.5,0.5),0,0.75,"easeInOutSine"],[0,0,0,1,"easeInOutSine"]
AnimateRotation:[0,0,0,0,"easeInOutSine"],[0,0,Random(-5,5),0.25,"easeInOutSine"],[0,0,Random(-5,0),0.5,"easeInOutSine"],[0,0,Random(-5,0),0.75,"easeInOutSine"],[0,0,0,1,"easeInOutSine"]
4:AnimateTrack
track:flowerfloat
duration:32
repeat:4
repeataddtime:32
AnimatePosition:[0,0,0,0,"easeInOutSine"],[Random(-3,3),Random(-0.5,0),0,0.25,"easeInOutSine"],[Random(-3,3),Random(-0.5,0),0,0.5,"easeInOutSine"],[Random(-3,3),Random(-0.5,0.5),0,0.75,"easeInOutSine"],[0,0,0,1,"easeInOutSine"]
AnimateRotation:[0,0,0,0,"easeInOutSine"],[0,0,Random(-5,5),0.25,"easeInOutSine"],[0,0,Random(-5,0),0.5,"easeInOutSine"],[0,0,Random(-5,0),0.75,"easeInOutSine"],[0,0,0,1,"easeInOutSine"]


0:AnimateTrack
  track:flowerfloat
  duration:0
  animatedissolve:[0,0]
3:AnimateTrack
  track:flowerfloat
  duration:7
  animatedissolve:[0,0],[1,1]

	#flower 2
290.2: ModelToWall
  path: models\flowermodel.dae
  duration:36
  hasanimation: true
  animateposition:[0,-1000,0,0],[0,0,0,0]
  track:flowerfloat
  animatedissolve:[1,0.94],[0,1]

293:Wall
  repeat:160
  repeataddtime:0.2
  NJSOffset:-10
  animatedefiniteposition:[Random(0,2),Random(8,12),Random(28,31),0],[Random(4,7),Random(10,14),Random(28,31),1,"easeInSine"]
  animatescale:[1,1,1,0],[0.01,0.01,0.01,1,"easeInSine"]
  scale:[0.8,0.8,0.8]
  color:[0,Random(1.6,1.7),Random(1.9,2),2]
  localrotation:[Random(0,360),Random(0,360),Random(0,360)]
  rotation:[0,0,-5]
  track:flowerfloat
  animatedissolve:[0,0],[1,0],[1,0.9],[0,1]

292:AnimateTrack
track:flowerfloat
duration:32
repeat:4
repeataddtime:32
AnimatePosition:[0,0,0,0,"easeInOutSine"],[Random(-3,3),Random(-0.5,0),0,0.25,"easeInOutSine"],[Random(-3,3),Random(-0.5,0),0,0.5,"easeInOutSine"],[Random(-3,3),Random(-0.5,0.5),0,0.75,"easeInOutSine"],[0,0,0,1,"easeInOutSine"]
AnimateRotation:[0,0,0,0,"easeInOutSine"],[0,0,Random(-5,5),0.25,"easeInOutSine"],[0,0,Random(-5,0),0.5,"easeInOutSine"],[0,0,Random(-5,0),0.75,"easeInOutSine"],[0,0,0,1,"easeInOutSine"]


288:AnimateTrack
  track:flowerfloat
  duration:0
  animatedissolve:[0,0]
292:AnimateTrack
  track:flowerfloat
  duration:8
  animatedissolve:[0,0],[1,1]

0:ParentTrack
  parenttrack:flowerfloatoffset
  childtracks:["flowerfloat"]

292:AnimateTrack
  track:flowerfloatoffset
 duration:8
 AnimatePosition:[0,0,50,0],[0,0,0,1,"easeOutSine"]


	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8

Workspace:we gon gon try 1 model helper

226.8:AnimateTrack
 track:wetry
 duration:1
 animateposition:[0,0,0,0],[0,0,-30,1,"easeInQuart"]

250:AnimateTrack
 track:wetry
 duration:1
 animateposition:[0,0,0,0]

0:AnimateTrack
  duration:1
  track:wegon
  animatedissolve:[0,0]
0:AnimateTrack
  duration:1
  track:wetry
  AnimateDissolve:[0,0]




	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8

Workspace:we gon gon try 1 model



255.5:AnimateTrack
  duration:1
  track:wegon
  AnimateDissolve:[0,0],[1,1]


244:AnimateTrack
  duration:1
  track:wegon
  animatedissolve:[0,0]

115.526:ModelToWall
  definitetime: seconds
  definitedurationseconds:2.783
  NJSOffset:-10
  hasAnimation:true
  path:models\wegontry1.dae
  animatedissolve:[0,0],[1,0.1],[1,0.6],[0,1] 
  color:[10,10,10,1]
  track:wegon



	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8

Workspace:we gon gon try 2 model

115.56:ModelToWall
  definitetime: seconds
  definitedurationseconds:2.783
  NJSOffset:-10
  hasAnimation:true
  animatedissolve:[0,0],[1,0.1],[1,0.8],[0,1] 
  path:models\wetry1.dae
  track:holyFUCK
color:[10,10,10,1]




248:AnimateTrack
  duration:0
 track:holyFUCK
  AnimateDissolve:[0,0]

258:AnimateTrack
  duration:0.5
 track:holyFUCK
  AnimateDissolve:[0,0],[1,1]



	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8

Workspace:we gon try 2 cloner
0:CloneFromWorkspace
  name:we gon gon try 2 model
  toBeat:100000
  addTime: 256
0:CloneFromWorkspace
  name:we gon gon try 1 model
  toBeat:100000
  addTime: 256

0:CloneFromWorkspace
  name:we gon gon try 2 model
  toBeat:100000
  addTime: 288
0:CloneFromWorkspace
  name:we gon gon try 1 model
  toBeat:100000
  addTime: 288

530:AppendToAllWallsBetween
  toBeat:550
  AnimateColor:[10,10,10,1,0],[10,10,10,1,0.75],[0.79,0.5,1,5,0.8],[0.5,0.76,1,5,0.9]
  AppendTechnique:1




	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8

Workspace:We gon try text wall helper
155:AnimateTrack
  track:wegonrot
  duration:0
  animaterotation:[0,0,20,0]
191:AnimateTrack
  track:wegonrot
  duration:0
  animaterotation:[0,0,-20,0]
222:AnimateTrack
  track:wegonrot
animaterotation:[0,0,0,0]
  duration:0
400:AnimateTrack
  track:wegonrot
  duration:0
  animaterotation:[0,0,20,0]
430:AnimateTrack
  track:wegonrot
  duration:0
  animaterotation:[0,0,-20,0]
470:AnimateTrack
  track:wegonrot
  duration:0
  animaterotation:[0,0,0,0]

227:TextToWall
  definitetime:beats
  definiteduration:1
  NJSOffset:-10
  line:TRY
  thicc:12
  path:litefont.png
  size:0.4
  AnimateDissolve:[0,0],[1,0],[1,0.8],[0,1]
  AnimateDefinitePosition:[0,0,15,0],[0,0,40,1]

195:TextToWall
  definitetime:beats
  definiteduration:1
  NJSOffset:-10
  line:TRY
  thicc:12
  path:litefont.png
  size:0.4
 AnimateDissolve:[0,0],[1,0],[1,0.5],[0,1]
  AnimateDissolve:[0,0],[1,0],[1,0.8],[0,1]
  AnimateDefinitePosition:[0,0,30,0]

451:TextToWall
  definitetime:beats
  definiteduration:1
  NJSOffset:-10
  line:TRY
  thicc:12
  path:litefont.png
  size:0.4
 AnimateDissolve:[0,0],[1,0],[1,0.5],[0,1]
  AnimateDissolve:[0,0],[1,0],[1,0.8],[0,1]
  AnimateDefinitePosition:[0,0,30,0]

483:TextToWall
  definitetime:beats
  definiteduration:1.5
  NJSOffset:-10
  line:TRY
  thicc:12
  path:litefont.png
  size:0.4
  AnimateDissolve:[0,0],[1,0]
  AnimateDefinitePosition:[0,0,40,0],[0,0,0,1]

	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8

Workspace:We gon try text wall
192:TextToWall
  definitetime:beats
  definiteduration:1.5
  thicc:12
  NJSOffset:-10
  track:wegonrot
  line:WE GON
  size:0.3
  AnimateDissolve:[0,0],[1,0],[1,0.5],[0,1]
  path:litefont.png
  AnimateDefinitePosition:[-10,6,30,0],[7,6,30,1,"easeOutQuad"]
193:TextToWall
  definitetime:beats
  definiteduration:1.5
  NJSOffset:-10
  line:WE GON
  thicc:12
  track:wegonrot
  path:litefont.png
  AnimateDissolve:[0,0],[1,0],[1,0.5],[0,1]
  size:0.3
  AnimateDefinitePosition:[6,4,30,0],[-10,4,30,1,"easeOutQuad"]
194:TextToWall
  definitetime:beats
  definiteduration:1.5
  NJSOffset:-10
  line:WE GON
  thicc:12
  path:litefont.png
  track:wegonrot
  size:0.3
  AnimateDissolve:[0,0],[1,0],[1,0.5],[0,1]
  AnimateDefinitePosition:[-7,0,30,0],[10,0,30,1,"easeOutQuad"]


	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8

Workspace:we gon try cloner 1
0:CloneFromWorkspace
  name:We gon try text wall
  toBeat:100000
  addTime:-32
0:CloneFromWorkspace
  name:We gon try text wall
  toBeat:100000
  addTime:32
0:CloneFromWorkspace
  name:We gon try text wall
  toBeat:100000
  addTime:224
0:CloneFromWorkspace
  name:We gon try text wall
  toBeat:100000
  addTime:256
0:CloneFromWorkspace
  name:We gon try text wall
  toBeat:100000
  addTime:288



	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8



Workspace:drop explosion 1 helper

257:AnimateTrack
  track:wallshake1parent
  duration:2
  animateposition:[0,0,0,0],[0,0,-30,1,"easeInQuad"]
259.9:AnimateTrack
  track:wallshake1parent
  duration:0
  animateposition:[0,0,0,0]

0:parenttrack
  parenttrack:wallshake1parent
  childtracks:["wallshake1","wallshake2"]




  	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8

Workspace:drop explosion 1
228:Wall
  repeat:25
  definitetime:beats
  definiteduration:8
  scale:[1,2.5,1]
  AnimateScale:[0.1,0.1,0.1,0],[2,2,2,0.125,"easeOutQuart"]
  AnimateLocalRotation:[0,0,0,0],[Random(0,360),Random(0,360),Random(0,360),0.125,"easeOutQuart"]
  AnimateDefinitePosition:[0,0,60,0],[Random(-30,30),Random(-32,38),Random(10,70),0.125,"easeOutExpo"]
  AnimateDissolve:[0,0],[0.8,0.01],[0.5,0.4],[0,0.8]
  AnimateRotation:[0,0,0,0.125],[0,0,-179,0.75,"easeInCubic"]
AnimateColor:[1,1,5,1,0],[1,5,2,1,0.35],[5,1,5,1,0.4],[0,0,5,1,0.54]
  interactable:false
  track:wallshake1

228:AnimateTrack
  track:wallshake1
  duration:1
  repeat:8
  repeataddtime:1
  color:[1,1,1,1]
  AnimatePosition:[0,0,0,0,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.1,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.2,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.3,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.4,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.5,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.6,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.7,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.8,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.9],[0,0,0,1,"easeInOutSine"]
interactable:false

228:Wall
  repeat:25
  definitetime:beats
  definiteduration:8
  scale:[1,2.5,1]
  AnimateDissolve:[0,0],[0.8,0.1],[0.5,0.4],[0,0.8]
  AnimateScale:[0.1,0.1,0.1,0],[2,2,2,0.125,"easeOutQuart"]
  AnimateRotation:[0,0,0,0.125],[0,0,179,0.75,"easeInCubic"]
  AnimateLocalRotation:[0,0,0,0],[Random(0,360),Random(0,360),Random(0,360),0.125,"easeOutQuart"]
  AnimateDefinitePosition:[0,0,60,0],[Random(-30,30),Random(-32,38),Random(10,70),0.125,"easeOutExpo"]
  AnimateColor:[1,5,2,1,0],[1,5,2,1,0.35],[4,0,5,1,0.4],[0,5,5,1,0.54]
  track:wallshake2

228:AnimateTrack
  track:wallshake2
  duration:1
  repeat:8
  repeataddtime:1
  AnimatePosition:[0,0,0,0,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.1,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.2,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.3,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.4,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.5,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.6,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.7,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.8,"easeInOutSine"],[Random(-1,1),Random(-1,1),Random(-1,1),0.9],[0,0,0,1,"easeInOutSine"]


	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8


Workspace:drop explosion 1 cloner
0:CloneFromWorkspace
  name:drop explosion 1
  toBeat:100000
  addTime:8
0:CloneFromWorkspace
  name:drop explosion 1
  toBeat:100000
  addTime:16
0:CloneFromWorkspace
  name:drop explosion 1
  toBeat:100000
  addTime:24
0:CloneFromWorkspace
  name:drop explosion 1
  toBeat:100000
  addTime:32
0:CloneFromWorkspace
  name:drop explosion 1
  toBeat:100000
  addTime:40
0:CloneFromWorkspace
  name:drop explosion 1
  toBeat:100000
  addTime:48
0:CloneFromWorkspace
  name:drop explosion 1
  toBeat:100000
  addTime:56


	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8


Workspace:try drop 1 helper

228:AnimateTrack
track:try
duration:8
repeat:4
repeataddtime:16
AnimateRotation:[0,0,0,0],[Random(-10,10),Random(-10,10),Random(-10,10),0.125,"easeOutQuart"],[Random(-10,10),Random(-10,10),Random(-10,10),0.25,"easeInOutSine"],[Random(-10,10),Random(-10,10),Random(-10,10),1,"easeOutQuart"]
236:AnimateTrack
track:try2
duration:8
repeat:4
repeataddtime:16
AnimateRotation:[0,0,0,0],[Random(-10,10),Random(-10,10),Random(-10,10),0.125,"easeOutQuart"],[Random(-10,10),Random(-10,10),Random(-10,10),0.25,"easeInOutSine"],[Random(-10,10),Random(-10,10),Random(-10,10),1,"easeOutQuart"]

516:AnimateTrack
track:try
duration:8
repeat:2
repeataddtime:16
AnimateRotation:[0,0,0,0],[Random(-10,10),Random(-10,10),Random(-10,10),0.125,"easeOutQuart"],[Random(-10,10),Random(-10,10),Random(-10,10),0.25,"easeInOutSine"],[Random(-10,10),Random(-10,10),Random(-10,10),1,"easeOutQuart"]
524:AnimateTrack
track:try2
duration:8
repeat:2
repeataddtime:16
AnimateRotation:[0,0,0,0],[Random(-10,10),Random(-10,10),Random(-10,10),0.125,"easeOutQuart"],[Random(-10,10),Random(-10,10),Random(-10,10),0.25,"easeInOutSine"],[Random(-10,10),Random(-10,10),Random(-10,10),1,"easeOutQuart"]




	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8

Workspace:try drop 1
#Try

225:AnimateTrack
  track:try
  animatedissolve:[0,0]
  duration:1

228:AnimateTrack
  animatedissolve:[0,0],[1,1]
  duration:0.5
  track:try

232:AnimateTrack
  track:try2
  animatedissolve:[0,0]
  duration:1

236:AnimateTrack
  animatedissolve:[0,0],[1,1]
  duration:0.5
  track:try2

256:AnimateTrack
  animatedissolve:[1,0],[0,1]
  duration:1
  track:try2



	#3.636; 8 beats length seconds

102.639:ModelToWall
  definiteTime:seconds
  definitedurationseconds:4.234
  path:models\try1.dae
  animatedissolve:[0,0],[1,0.1],[1,0.8],[0,1] 
  hasAnimation:true
  track:try

106.275:ModelToWall
  definiteTime:seconds
  definitedurationseconds:4.234
  path:models\try3.dae
  animatedissolve:[0,0],[1,0.1],[1,0.8],[0,1] 
  hasAnimation:true
  track:try2



	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8


Workspace:try drop 1 cloner
0:CloneFromWorkspace
  name:try drop 1
  addtime:16
  toBeat:100000
0:CloneFromWorkspace
  name:try drop 1
  addtime:32
  toBeat:100000
0:CloneFromWorkspace
  name:try drop 1
  addtime:48
  toBeat:100000
0:CloneFromWorkspace
  name:try drop 1
  addtime:288
  toBeat:100000
0:CloneFromWorkspace
  name:try drop 1
  addtime:304
  toBeat:100000

	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8



Workspace:try drop 1 you gotta try text

233.5:TextToWall
  definitetime:beats
  definiteduration:1
  NJSOffset:-10
  path:litefont.png
  animatedissolve:[0,0],[1,0],[1,0.5],[0,1]
  line:YOU
  size:0.3
  thicc:12
  track:drop1text
  animatedefiniteposition:[-9,3,30,0]
234:TextToWall
  definitetime:beats
  definiteduration:1
  NJSOffset:-10
  path:litefont.png
  animatedissolve:[0,0],[1,0],[1,0.5],[0,1]
  line:KNOW
  track:drop1text
  size:0.3
  thicc:12
  animatedefiniteposition:[-5,5,30,0]
234.5:TextToWall
  definitetime:beats
  definiteduration:1
  NJSOffset:-10
  path:litefont.png
  track:drop1text
  animatedissolve:[0,0],[1,0],[1,0.5],[0,1]
  line:YOU
  size:0.3
  thicc:12
  animatedefiniteposition:[3,2,30,0]
235:TextToWall
  definitetime:beats
  definiteduration:3
  track:drop1text
  NJSOffset:-10
  animatedissolve:[0,0],[1,0],[1,0.5],[0,1]
  path:litefont.png
  line:GOTTA
  size:0.3
  thicc:12
  animatedefiniteposition:[9,5,30,0]



	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8

Workspace:try drop 1 good in good bye text

240:TextToWall
  definitetime:beats
  definiteduration:1
  NJSOffset:-10
  path:litefont.png
  animatedissolve:[0,0],[1,0],[1,0.5],[0,1]
  line:GOOD
  size:0.3
  thicc:12
  track:drop1text
  animatedefiniteposition:[0,5,30,0]
240.5:TextToWall
  definitetime:beats
  definiteduration:1
  NJSOffset:-10
  path:litefont.png
  animatedissolve:[0,0],[1,0],[1,0.5],[0,1]
  line:IN
  track:drop1text
  size:0.3
  thicc:12
  animatedefiniteposition:[0,10,30,0]
241:TextToWall
  definitetime:beats
  definiteduration:1
  NJSOffset:-10
  path:litefont.png
  track:drop1text
  animatedissolve:[0,0],[1,0],[1,0.5],[0,1]
  line:GOOD
  size:0.3
  thicc:12
  animatedefiniteposition:[-4,5,30,0]
241.5:TextToWall
  definitetime:beats
  definiteduration:3
  track:drop1text
  NJSOffset:-10
  animatedissolve:[0,0],[1,0],[1,0.5],[0,1]
  path:litefont.png
  line:BYE
  size:0.3
  thicc:12
  animatedefiniteposition:[5,5,30,0]



	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8

Workspace::try drop 1 other text cloner

0:CloneFromWorkspace
  name:try drop 1 you gotta try text
  addtime:16
  tobeat:1000000
0:CloneFromWorkspace
  name:try drop 1 you gotta try text
  addtime:32
  tobeat:1000000
0:CloneFromWorkspace
  name:try drop 1 good in good bye text
  addtime:32
  tobeat:1000000
0:CloneFromWorkspace
  name:try drop 1 you gotta try text
  addtime:48
  tobeat:1000000

0:CloneFromWorkspace
  name:try drop 1 good in good bye text
  addtime:288
  tobeat:1000000

0:CloneFromWorkspace
  name:try drop 1 you gotta try text
  addtime:288
0:CloneFromWorkspace
  name:try drop 1 you gotta try text
  addtime:304
  tobeat:1000000



	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8

Workspace:text

	#lyrics
0:AssignPathAnimation
  track:textpath
  animatedissolve:[0,0],[1,0.2],[1,0.5],[0,1]

0:ParentTrack
  parentTrack:flowerfloat
  childtracks:["textpath"]

	#closing time
37:TextToWall
  path:litefont.png
  NJSOffset:-10
  duration:0.2
  line:closing
  line: 
  track:textpath
  rotation:[5,-10,0]
  animatedefiniteposition:[0,2,22,0]
  thicc:12
  leading:-5
  color:[0.8,0.9,1,2]
  size:0.1
38.75:TextToWall
  path:litefont.png
  NJSOffset:-10
  duration:1
  line:        time
  track:textpath
  rotation:[10,-10,0]
  animatedefiniteposition:[0,2,22,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.1

	#still in the same
41.5:TextToWall
  path:litefont.png
  NJSOffset:-10
  duration:0
  line:still in the same bar
  line:
  track:textpath
  rotation:[-10,10,0]
  spreadspawntime:0.2
  animatedefiniteposition:[0,2,22,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.1
43:TextToWall
  path:litefont.png
  NJSOffset:-10
  duration:3
  line:    where you said goodbye
  track:textpath
  rotation:[-10,10,0]
  spreadspawntime:0.2
  animatedefiniteposition:[0,2,22,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.1


49.5:TextToWall
  path:litefont.png
  NJSOffset:-10
  duration:0.7
  line:if i could i would've
  line: 
  line: 
  line: 
  track:textpath
  rotation:[10,-10,0]
  spreadspawntime:0.2
  animatedefiniteposition:[0,2,22,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.1
52:TextToWall
  path:litefont.png
  NJSOffset:-10
  duration:1.5
  line:pressed rewind
  line: 
  line: 
  track:textpath
  rotation:[10,-10,0]
  spreadspawntime:0.2
  animatedefiniteposition:[0,2,22,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.1
55.5:TextToWall
  path:litefont.png
  NJSOffset:-10
  duration:1.5
  line:but you and i
  line: 
  track:textpath
  rotation:[10,-10,0]
  spreadspawntime:0.2
  animatedefiniteposition:[0,2,22,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.1
60:TextToWall
  path:litefont.png
  NJSOffset:-10
  duration:3
  line:it just went right
  track:textpath
  rotation:[10,-10,0]
  spreadspawntime:0.2
  animatedefiniteposition:[0,2,22,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.1

67.5:TextToWall
  path:litefont.png
  NJSOffset:-10
  duration:3
  line:so i built these walls
  line: 
  line: 
  track:textpath
  rotation:[-8,10,0]
  spreadspawntime:0.2
  animatedefiniteposition:[0,2,22,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.1
73.5:TextToWall
  path:litefont.png
  NJSOffset:-10
  duration:0
  line:cause after losing
  line: 
  track:textpath
  rotation:[-8,10,0]
  spreadspawntime:0.2
  animatedefiniteposition:[0,2,22,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.1
75.5:TextToWall
  path:litefont.png
  NJSOffset:-10
  duration:2
  line:i was scared to fall
  track:textpath
  rotation:[-8,10,0]
  spreadspawntime:0.2
  animatedefiniteposition:[0,2,22,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.1

80.5:TextToWall
  path:litefont.png
  NJSOffset:-10
  definitetime:beats
  definiteduration:3
  line:but i will climb
  line: 
  line: 
  track:textpath
  rotation:[-8,-10,0]
  spreadspawntime:0.2
  animatedefiniteposition:[0,2,22,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.1
83.2:TextToWall
  path:litefont.png
  NJSOffset:-10
  definitetime:beats
  definiteduration:3.5
  line:even though they're tall
  line: 
  track:textpath
  rotation:[-8,-10,0]
  spreadspawntime:0.2
  animatedefiniteposition:[0,2,22,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.1

87:TextToWall
  path:litefont.png
  NJSOffset:-10
  definitetime:beats
  definiteduration:5.5
  line:been here before
  track:textpath
  rotation:[-8,-10,0]
  spreadspawntime:0.2
  animatedefiniteposition:[0,2,22,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.1



	#second hugewalls section
	#dont look back
324:TextToWall
  definitetime:beats
  path:litefont.png
  line:dont look
  line: 
  track:textpath
  animatedefiniteposition:[Random(-5,-2),Random(3,5),10,0],[Random(-5,-2),Random(3,5),50,1]
  thicc:12
  leading:-5
  color:[0.8,0.9,1,2]
  size:0.1
325.5:TextToWall
  definitetime:beats
  path:litefont.png
  line:        back
  track:textpath
animatedefiniteposition:[Random(-5,-2),Random(3,5),10,0],[Random(-5,-2),Random(3,5),40,1]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.1

	#theres nothing therre but everything we had
328.5:TextToWall
  path:litefont.png
definitetime:beats
  line:theres nothing there
  line:
  track:textpath
  animatedefiniteposition:[Random(2,5),Random(3,5),10,0],[Random(-5,-2),Random(3,5),50,1]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.1
330.75:TextToWall
  path:litefont.png
definitetime:beats
  line:    but everything we had
  track:textpath
  animatedefiniteposition:[Random(2,5),Random(3,5),10,0],[Random(-5,-2),Random(3,5),50,1]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.1


	#we found love and the love went bad
337:TextToWall
  path:litefont.png
definitetime:beats
  line:we found love
  line: 
  line: 
  line: 
  track:textpath
  animatedefiniteposition:[Random(-5,-2),Random(-5,-3),10,0],[Random(-5,-2),Random(-5,-3),40,1]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.1
339.35:TextToWall
  path:litefont.png
definitetime:beats
  line: and the love went bad
  line: 
  line: 
  track:textpath
animatedefiniteposition:[Random(-5,-2),Random(-5,-3),10,0],[Random(-5,-2),Random(-5,-3),50,1]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.1


	#i wont look back
343.5:TextToWall
  path:litefont.png
definitetime:beats
  line:i wont look back
  line: 
  track:textpath
animatedefiniteposition:[Random(-5,-2),Random(3,5),10,0],[Random(-5,-2),Random(-5,-3),40,1]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.1
	#no no
347.5:TextToWall
  path:litefont.png
definitetime:beats
  line:no no....
  track:textpath
animatedefiniteposition:[Random(-5,-2),Random(-5,-3),10,0],[Random(2,5),Random(3,5),50,1]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.1


355:TextToWall
  path:litefont.png
definitetime:beats
definiteduration:3
  line:WHEN YOU GAVE
  track:textpath
 animatedefiniteposition:[0,6,35,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.2
357:TextToWall
  path:litefont.png
definitetime:beats
definiteduration:3
  line:ALL YOUR LOVE
  track:textpath
 animatedefiniteposition:[0,9,35,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.2
359:TextToWall
  path:litefont.png
definitetime:beats
definiteduration:5
  line:BUT IT WASN'T ENOUGH...
  track:textpath
 animatedefiniteposition:[0,3,35,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.2

363:TextToWall
  path:litefont.png
definitetime:beats
definiteduration:3
  line:WHEN YOU SWEAR
  track:textpath
 animatedefiniteposition:[0,9,35,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.2
365:TextToWall
  path:litefont.png
definitetime:beats
definiteduration:3
  line:THIS TIME
  track:textpath
 animatedefiniteposition:[0,6,35,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.2
367:TextToWall
  path:litefont.png
definitetime:beats
definiteduration:5
  line:YOU'RE GIVING UP
  track:textpath
 animatedefiniteposition:[0,3,35,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.2

370:TextToWall
  path:litefont.png
definitetime:beats
definiteduration:2.5
  line:YOU STILL GOTTA
  track:textpath
 animatedefiniteposition:[0,6,35,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.2
372:TextToWall
  path:litefont.png
definitetime:beats
definiteduration:5
  line:Try
  track:textpath
 animatedefiniteposition:[0,3,35,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.5

377.5:TextToWall
  path:litefont.png
definitetime:beats
definiteduration:2.5
  line:YOU KNOW YOU GOTTA
  track:textpath
 animatedefiniteposition:[0,9,35,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.2
380:TextToWall
  path:litefont.png
definitetime:beats
definiteduration:6
  line:Try
  track:textpath
 animatedefiniteposition:[0,3,35,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.5


	#repeatbut imlazy af this works
387:TextToWall
  path:litefont.png
definitetime:beats
definiteduration:3
  line:WHEN YOU GAVE
  track:textpath
 animatedefiniteposition:[0,6,35,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.2
389:TextToWall
  path:litefont.png
definitetime:beats
definiteduration:3
  line:ALL YOU GOT
  track:textpath
 animatedefiniteposition:[0,9,35,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.2
391:TextToWall
  path:litefont.png
definitetime:beats
definiteduration:5
  line:LIKE A MILLION TIMES...
  track:textpath
 animatedefiniteposition:[0,3,35,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.2

394.5:TextToWall
  path:litefont.png
definitetime:beats
definiteduration:3
  line:WE PUT THE HELL
  track:textpath
 animatedefiniteposition:[0,9,35,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.2
397:TextToWall
  path:litefont.png
definitetime:beats
definiteduration:3
  line:IN HELLO
  track:textpath
 animatedefiniteposition:[0,6,35,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.2
399:TextToWall
  path:litefont.png
definitetime:beats
definiteduration:5
  line:AND THE GOOD IN GOODBYE...
  track:textpath
 animatedefiniteposition:[0,3,35,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.2

402.5:TextToWall
  path:litefont.png
definitetime:beats
definiteduration:2.5
  line:YOU GOTTA
  track:textpath
 animatedefiniteposition:[0,9,35,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.2
404:TextToWall
  path:litefont.png
definitetime:beats
definiteduration:5
  line:Try
  track:textpath
 animatedefiniteposition:[0,3,35,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.5

409.5:TextToWall
  path:litefont.png
definitetime:beats
definiteduration:2.5
  line:YOU KNOW YOU GOTTA
  track:textpath
 animatedefiniteposition:[0,9,35,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.2
412:TextToWall
  path:litefont.png
definitetime:beats
definiteduration:4
  line:Try
  track:textpath
 animatedefiniteposition:[0,3,35,0]
  thicc:12
  color:[0.8,0.9,1,2]
  size:0.5


	#house keeping
0:AppendToAllWallsBetween
  toBeat:100000
  NJS:18
  interactable:false
  NJSOffset:-0.8

Workspace:lights import

	#Lonely
0:Import
  fullpath:E:\New folder\steamapps\common\Beat Saber\Beat Saber_Data\CustomWIPLevels\Try - mitiS ft RORY\Light_V0.2\ExpertPlusStandard.dat
  type:2

	#Jam
0:Import
  fullpath:E:\New folder\steamapps\common\Beat Saber\Beat Saber_Data\CustomWIPLevels\autoWip - Try.zip\ExpertPlusStandard.dat
  type:2



Workspace:notes import

0:Import
 fullpath:E:\New folder\steamapps\common\Beat Saber\Beat Saber_Data\CustomWIPLevels\Try - mitiS ft RORY\MitiS_Final_Full_Map\ExpertPlusStandard.dat
 type:1

Workspace:message for remie
560:TextToWall
  path:litefont.png
  definiteTime:beats
  definiteDurationSeconds:3
  NJSOffset:-10
  track:message
  thicc:12
  Line:We know it has been hard
  size:0.12
  AnimateDissolve:[0,0],[1,0.35],[1,0.65],[0,1]
  AnimateDefinitePosition:[0,0,0,0]

566:TextToWall
  path:litefont.png
  definiteTime:beats
  definiteDurationSeconds:2.5
  NJSOffset:-10
  track:message
  Line:But we are all here for you
  size:0.1
  thicc:12
  AnimateDissolve:[0,0],[1,0.35],[1,0.65],[0,1]
  AnimateDefinitePosition:[0,0,0,0]

571:TextToWall
  path:litefont.png
  definiteTime:beats
  definiteDurationSeconds:6
  NJSOffset:-10
  track:message
  thicc:12
  Line:We all love you Remie
  size:0.085
  AnimateDissolve:[0,0],[1,0.35],[1,0.65],[0,1]
  AnimateDefinitePosition:[0,0,0,0]

556:AnimateTrack
  track:message
  duration:20
  AnimatePosition:[0,2,45,0],[0,2,21,1,"easeOutSine"]


	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8

Workspace:credits
582:TextToWall
  path:litefont.png
  definiteTime:beats
  definiteduration:18
  NJSOffset:-10
  track:message
  centered:false
  thicc:12
  Line:Mapped by
  Line:
  Line:
  Line:
  Line:
  size:0.08
  AnimateDissolve:[0,0],[1,0.15],[1,0.85],[0,1]
  AnimateDefinitePosition:[0,0,0,0]


584:TextToWall
  path:litefont.png
  definiteTime:beats
  definiteduration:16
  NJSOffset:-10
  track:message
  centered:false
  thicc:12
  Line:Lone
  Line:
  Line:
Line:
  size:0.08
  rgbcolor:[255, 0, 247,255]
  AnimateDissolve:[0,0],[1,0.15],[1,0.85],[0,1]
  AnimateDefinitePosition:[0,0,0,0]
586:TextToWall
  path:litefont.png
  definiteduration:14
  definiteTime:beats
  NJSOffset:-10
  track:message
  thicc:12
  Line:Jamman
  Line:
Line:
  rgbcolor:[136, 0, 255,255]
  size:0.08
  centered:false
  AnimateDissolve:[0,0],[1,0.15],[1,0.85],[0,1]
  AnimateDefinitePosition:[0,0,0,0]
588:TextToWall
  path:litefont.png
  definiteTime:beats
  definiteduration:12
  NJSOffset:-10
  track:message
  thicc:12
  centered:false
  Line:thelightdesigner
Line:
  size:0.08
rgbcolor:[0, 221, 255,255]
  AnimateDissolve:[0,0],[1,0.15],[1,0.85],[0,1]
  AnimateDefinitePosition:[0,0,0,0]
590:TextToWall
  path:litefont.png
  definiteTime:beats
  definiteduration:10
  NJSOffset:-10
  track:message
  centered:false
  thicc:12
  Line:Remie
  size:0.08
  rgbcolor:[0, 143, 10,255]
  AnimateDissolve:[0,0],[1,0.15],[1,0.85],[0,1]
  AnimateDefinitePosition:[0,0,0,0]





	#house keeping
0:AppendToAllWallsBetween
  NJS:18
  interactable:false
  toBeat:1000000
  NJSOffset:-0.8


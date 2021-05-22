# ScuffedWalls v1.1.0-unreleased

# Documentation on functions can be found at
# https://github.com/thelightdesigner/ScuffedWalls/blob/main/Functions.md
            
# DM @thelightdesigner#1337 for more help?

# Using this tool requires an understanding of Noodle Extensions.
# https://github.com/Aeroluna/NoodleExtensions/blob/master/Documentation/AnimationDocs.md

# Playtest your maps

Workspace:Default

0: Import
   Path:ExpertStandard_Old.dat

0: AnimateTrack
track:sun
duration:24
animaterotation:[0,0,90,0]
animatescale:[0,0,0,0],[1,0.5,1,0]
animateposition:[0,-6,40,0],[0,3,40,1,"easeOutSine"]

88:AnimateTrack
duration:1
track:lowClouds
repeat:64
repeataddtime:1
animatescale:[0,0,0,0],[1,1,1,0],[1,1,1,1],[0,0,0,1]
animateposition:[0,-2,0,0],[0,-7,0,1,"easeInQuad"],[0,0,0,1]

67.95:AnimateTrack
duration:0
track:backLasers
animaterotation:[0,180,-30,0]

88:AnimateTrack
duration:0
track:backLasers
animaterotation:[0,180,80,0]
animateposition:[0,80,140,0]

91:AnimateTrack
duration:0
track:backLasers
animaterotation:[0,180,-80,0]
animateposition:[0,40,190,0]

120:AnimateTrack
duration:0
track:backLasers
animaterotation:[0,135,-60,0]
animateposition:[0,40,170,0]

123:AnimateTrack
duration:0
track:backLasers
animaterotation:[6,175,-90,0]
animateposition:[0,70,130,0]

123.45:AnimateTrack
duration:0
track:backLasers
animaterotation:[20,175,-120,0]
animateposition:[-10,60,145,0]

167.75:AnimateTrack
duration:0
track:highClouds
animatescale:[0,0,0,0],[1,1,1,0]

167.75:AnimateTrack
duration:1
track:backLasers
repeat:32
repeataddtime:1
animaterotation:[Random(-20,20),Random(170,210),Random(120,-120),0]
animateposition:[Random(-20,20),Random(50,70),Random(90,140),0]

199.75:AnimateTrack
duration:0
track:backLasers
animatescale:[0,0,0,0]

199.75:AnimateTrack
duration:0
track:leftLasers
animatescale:[1,1,1,0]
animaterotation:[0,0,0,0]

199.75:AnimateTrack
duration:0
track:rightLasers
animatescale:[1,1,1,0]
animaterotation:[0,0,0,0]

199.75:AnimateTrack
duration:0
track:topLasers
animatescale:[1,1,1,0]

209.5:AnimateTrack
duration:3
track:leftLasers
animatelocalposition:[-20,-40,-30,0],[-20,-40,-40,1]

211.5:AnimateTrack
duration:3
track:rightLasers
animatelocalposition:[20,-40,-30,0],[20,-40,-40,1]

213.5:AnimateTrack
duration:0
track:leftLasers
animatelocalposition:[-20,-40,-30,0]

214.5:AnimateTrack
duration:0
track:rightLasers
animatelocalposition:[20,-40,-30,0]

264:AnimateTrack
duration:1
track:lowClouds
repeat:64
repeataddtime:1
animatescale:[0,0,0,0],[1,1,1,0],[1,1,1,1],[0,0,0,1]
animateposition:[0,-2,0,0],[0,-7,0,1,"easeInQuad"],[0,0,0,1]

264:AnimateTrack
duration:1
track:highClouds
repeat:64
repeataddtime:1
animatescale:[0,0,0,0],[1.05,1.1,1.1,0],[1,1,1,1],[0,0,0,1]

278:AnimateTrack
duration:0
track:leftLasers
animatelocalposition:[-20,-40,-30,0]
animaterotation:[0,0,-10,0]
animatescale:[0.1,0.1,0.1,0]

278:AnimateTrack
duration:0
track:rightLasers
animatelocalposition:[20,-40,-30,0]
animaterotation:[0,0,10,0]
animatescale:[0.1,0.1,0.1,0]

278.9:AnimateTrack
duration:0
track:leftLasers
animaterotation:[0,90,-20,0]

290.8:AnimateTrack
duration:0
track:leftLasers
animaterotation:[0,0,-10,0]

291.45:AnimateTrack
duration:0
track:leftLasers
animaterotation:[0,0,-30,0]

294:AnimateTrack
duration:0
track:leftLasers
animaterotation:[0,0,-10,0]

294.8:AnimateTrack
duration:0
track:rightLasers
animaterotation:[0,-90,20,0]

309:AnimateTrack
duration:0
track:rightLasers
animaterotation:[0,0,10,0]

310.75:AnimateTrack
duration:0
track:leftLasers
animaterotation:[0,90,-20,0]

326:AnimateTrack
duration:0
track:leftLasers
animaterotation:[0,0,-10,0]

326.9:AnimateTrack
duration:4
track:rightLasers
animaterotation:[0,0,-10,0],[0,0,-40,1]

327.4:AnimateTrack
duration:4
track:leftLasers
animaterotation:[0,0,10,0],[0,0,40,1]

328:AnimateTrack
duration:10
track:highClouds
animatescale:[1,1,1,0],[0,0,0,1]

var:leftRandomRotation
data:[Random(80,-80),Random(80,-80),Random(40,-80),0]
recompute:0

var:leftRandomPosition
data:[Random(40,-10),Random(60,-60),Random(-40,100),0]
recompute:0

var:rightRandomRotation
data:[Random(80,-80),Random(80,-80),Random(40,-80),0]
recompute:0

var:rightRandomPosition
data:[Random(40,-10),Random(60,-60),Random(100,40),0]
recompute:0

330:AnimateTrack
duration:0
track:rightLasers
animatelocalrotation:rightRandomRotation
animatelocalposition:rightRandomPosition
animatescale:[0.001,0.001,0.001,0]

331:AnimateTrack
duration:0
track:leftLasers
animatelocalrotation:leftRandomRotation
animatelocalposition:leftRandomPosition
animatescale:[0.001,0.001,0.001,0]

332.75:AnimateTrack
duration:0
track:rightLasers
animatelocalrotation:rightRandomRotation
animatelocalposition:rightRandomPosition

334.2:AnimateTrack
duration:0
track:leftLasers
animatelocalrotation:leftRandomRotation
animatelocalposition:leftRandomPosition

335.2:AnimateTrack
duration:0
track:rightLasers
animatelocalrotation:rightRandomRotation
animatelocalposition:rightRandomPosition

337:AnimateTrack
duration:0
track:leftLasers
animatelocalrotation:leftRandomRotation
animatelocalposition:leftRandomPosition

339:AnimateTrack
duration:0
track:rightLasers
animatelocalrotation:rightRandomRotation
animatelocalposition:rightRandomPosition

340:AnimateTrack
duration:0
track:leftLasers
animatelocalrotation:leftRandomRotation
animatelocalposition:leftRandomPosition

342:AnimateTrack
duration:0
track:rightLasers
animatelocalrotation:rightRandomRotation
animatelocalposition:rightRandomPosition

343.45:AnimateTrack
duration:0
track:leftLasers
animatelocalrotation:leftRandomRotation
animatelocalposition:leftRandomPosition

346:AnimateTrack
duration:0
track:rightLasers
animatelocalrotation:rightRandomRotation
animatelocalposition:rightRandomPosition

347:AnimateTrack
duration:0
track:leftLasers
animatelocalrotation:leftRandomRotation
animatelocalposition:leftRandomPosition

351:AnimateTrack
duration:0
track:rightLasers
animatelocalrotation:rightRandomRotation
animatelocalposition:rightRandomPosition

352:AnimateTrack
duration:0
track:leftLasers
animatelocalrotation:leftRandomRotation
animatelocalposition:leftRandomPosition

354:AnimateTrack
duration:0
track:rightLasers
animatelocalrotation:rightRandomRotation
animatelocalposition:rightRandomPosition

355:AnimateTrack
duration:0
track:leftLasers
animatelocalrotation:leftRandomRotation
animatelocalposition:leftRandomPosition

357:AnimateTrack
duration:0
track:rightLasers
animatelocalrotation:rightRandomRotation
animatelocalposition:rightRandomPosition

358:AnimateTrack
duration:0
track:leftLasers
animatelocalrotation:leftRandomRotation
animatelocalposition:leftRandomPosition

362:AnimateTrack
duration:0
track:rightLasers
animatelocalrotation:rightRandomRotation
animatelocalposition:rightRandomPosition
animatescale:[0.001,0.001,0.001,0]

363:AnimateTrack
duration:0
track:leftLasers
animatelocalrotation:leftRandomRotation
animatelocalposition:leftRandomPosition
animatescale:[0.001,0.001,0.001,0]

364.75:AnimateTrack
duration:0
track:rightLasers
animatelocalrotation:rightRandomRotation
animatelocalposition:rightRandomPosition

366.2:AnimateTrack
duration:0
track:leftLasers
animatelocalrotation:leftRandomRotation
animatelocalposition:leftRandomPosition

367.2:AnimateTrack
duration:0
track:rightLasers
animatelocalrotation:rightRandomRotation
animatelocalposition:rightRandomPosition

369:AnimateTrack
duration:0
track:leftLasers
animatelocalrotation:leftRandomRotation
animatelocalposition:leftRandomPosition

370:AnimateTrack
duration:0
track:rightLasers
animatelocalrotation:rightRandomRotation
animatelocalposition:rightRandomPosition

372:AnimateTrack
duration:0
track:leftLasers
animatelocalrotation:leftRandomRotation
animatelocalposition:leftRandomPosition

374:AnimateTrack
duration:0
track:rightLasers
animatelocalrotation:rightRandomRotation
animatelocalposition:rightRandomPosition

375.45:AnimateTrack
duration:0
track:leftLasers
animatelocalrotation:leftRandomRotation
animatelocalposition:leftRandomPosition

378:AnimateTrack
duration:0
track:rightLasers
animatelocalrotation:rightRandomRotation
animatelocalposition:rightRandomPosition

379:AnimateTrack
duration:0
track:leftLasers
animatelocalrotation:leftRandomRotation
animatelocalposition:leftRandomPosition

383:AnimateTrack
duration:0
track:rightLasers
animatelocalrotation:rightRandomRotation
animatelocalposition:rightRandomPosition

383:AnimateTrack
duration:0
track:leftLasers
animatelocalrotation:leftRandomRotation
animatelocalposition:leftRandomPosition

386:AnimateTrack
duration:0
track:rightLasers
animatelocalrotation:rightRandomRotation
animatelocalposition:rightRandomPosition

387:AnimateTrack
duration:0
track:leftLasers
animatelocalrotation:leftRandomRotation
animatelocalposition:leftRandomPosition

388:AnimateTrack
duration:0
track:rightLasers
animatelocalrotation:rightRandomRotation
animatelocalposition:rightRandomPosition

390:AnimateTrack
duration:0
track:leftLasers
animatelocalrotation:leftRandomRotation
animatelocalposition:leftRandomPosition

395:AnimateTrack
duration:0
track:leftLasers
animatescale:[0,0,0,0]

395:AnimateTrack
duration:0
track:rightLasers
animatescale:[0,0,0,0]

199.5:AnimateTrack
duration:0.5
track:hills
animatedissolve:[0,0,0,0],[1,1,1,1]

0:AnimateTrack
duration:0
track:hills
animatedissolve:[0,0]

200:AnimateTrack
duration:0
track:hills
animatedissolve:[1,0]

#264:AnimateTrack
#duration:0
#track:hills
#animatedissolve:[0,0]

0:AnimateTrack
duration:0
track:spires
animatedissolve:[0,0]

264:AnimateTrack
duration:0
track:spires
animatedissolve:[1,0]

327:AnimateTrack
duration:1
track:spires
animatedissolve:[1,0],[0,1]

264:AnimateTrack
duration:1
track:spires
repeat:64
repeataddtime:1
animatecolor:[4,4,4,4,0],[0.4,0.4,0.4,0,1,"easeOutSine"]

391.25:AnimateTrack
duration:0.75
track:outroLeft
animatedissolve:[1,0],[0,1,"easeInExpo"]
animateposition:[0,0,0,0],[-20,-20,0,1,"easeInExpo"]
animaterotation:[0,0,0,0],[0,0,30,1,"easeInExpo"]

391.25:AnimateTrack
duration:0.75
track:outroRight
animatedissolve:[1,0],[0,1,"easeInExpo"]
animateposition:[0,0,0,0],[20,-20,0,1,"easeInExpo"]
animaterotation:[0,0,0,0],[0,0,-30,1,"easeInExpo"]

391.5:AnimateTrack
duration:4
track:sun
animateposition:[0,3,2,0],[0,0,40,1,"easeOutExpo"]

var:particleX
data:Random(-40,40)
recompute:1

78:Wall
repeat:592
repeataddtime:0.125
color:[0.4,0.4,0.4,2]
duration:10
position:[{particleX * -2},Random(15,100)]
rotation:[0,0,particleX]
animatedefiniteposition:[0,0,200,0],[0,0,-20,1]
scale:[10,10,10]
animatescale:[{0.2 / 10},{0.2 / 10},{Random(1,20) / 10},0]
localrotation:[Random(0,360),Random(0,360),Random(0,360)]
animatedissolve:[0,0],[1,0.2]
track:particles

88:AnimateTrack
repeat:64
repeataddtime:1
duration:1
animatecolor:[10,10,10,10,0],[0.4,0.4,0.4,2,1]
track:particles

0:AnimateTrack
duration:0
track:particles
animatedissolve:[0,0]

88:AnimateTrack
duration:0
track:particles
animatedissolve:[1,0]

151:AnimateTrack
duration:1
track:particles
animatedissolve:[1,0],[0,1]


# LYRICS

3:TextToWall
path:font.png
size:0.05
line: THIS SHOULD BE HIDDEN   PLEASE FOLLOW THE WARNINGS
position:[0,0]
animatedefiniteposition:[150000,6,20,0]
spreadspawntime:0.5
duration:3
color:[0,0,0,20]
track:lyrics
thicc:0.00001

0:AnimateTrack
duration:7
track:lyrics
animatedissolve:[0,-0.2],[1,0.3],[1,0.8],[0,1]

28:TextToWall
definitedurationbeats:16
path:font.dae
line: Duumu. Slyleaf
thicc:12
size:0.5
position:[-1.5,10]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.5,"easeInOutCirc"]
animatedefiniteposition:[0,0,20,0],[0,0,70,0.5,"easeInOutCirc"]
animatedissolve:[0,0],[1,0.3],[1,0.8],[0,1]
color:[0.964,0.333,0.117]
track:title

30:TextToWall
definitedurationbeats:16
path:font.dae
line: Illuminate
thicc:12
size:0.6
position:[-2,5.5]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.5,"easeInOutCirc"]
animatedefiniteposition:[0,0,20,0],[0,0,60,0.5,"easeInOutCirc"]
animatedissolve:[0,0],[1,0.3],[1,0.8],[0,1]
track:title

53.5:TextToWall
definitedurationbeats:16
path:font.dae
line:I know its easy
thicc:12
size:0.2
position:[-3,8]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.2,"easeOutExpo"]
animatedefiniteposition:[0,0,0,0],[0,0,40,0.4,"easeOutExpo"],[0,0,100,1,"easeInSine"]
animaterotation:[0,0,-10,0],[0,0,0,0.4,"easeOutExpo"],[0,0,20,1,"easeInSine"]
animatedissolve:[0,0],[1,0.1],[1,0.5],[0,1]
track:calmLyrics

59:TextToWall
definitedurationbeats:16
path:font.dae
line:To get lost
thicc:12
size:0.2
position:[10,5]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.2,"easeOutExpo"]
animatedefiniteposition:[0,0,0,0],[0,0,30,0.2,"easeOutExpo"],[0,0,100,1,"easeInSine"]
animaterotation:[0,0,14,0],[0,0,4,0.2,"easeOutExpo"],[0,0,-20,1,"easeInSine"]
animatedissolve:[0,0],[1,0.1],[1,0.5],[0,1]
track:calmLyrics

63.5:TextToWall
definitedurationbeats:16
path:font.dae
line:in yesterday
thicc:12
size:0.2
position:[-13,2]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.2,"easeOutExpo"]
animatedefiniteposition:[0,0,0,0],[0,0,30,0.2,"easeOutExpo"],[0,0,100,1,"easeInSine"]
animaterotation:[0,0,10,0],[0,0,-4,0.2,"easeOutExpo"],[0,0,-10,1,"easeInSine"]
animatedissolve:[0,0],[1,0.1],[1,0.5],[0,1]
track:calmLyrics

70:TextToWall
definitedurationbeats:16
path:font.dae
line:To bring back all these things
thicc:12
size:0.15
position:[8,8]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.2,"easeOutExpo"]
animatedefiniteposition:[0,0,0,0],[0,0,30,0.4,"easeOutExpo"],[0,0,100,1,"easeInSine"]
animaterotation:[0,0,14,0],[0,0,-5,0.4,"easeOutExpo"],[0,0,-15,1,"easeInSine"]
animatedissolve:[0,0],[1,0.1],[1,0.5],[0,1]
track:calmLyrics

78.5:TextToWall
definitedurationbeats:8
path:font.dae
line:I know you loved them
thicc:12
size:0.2
position:[-12,10]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.2,"easeOutExpo"]
animatedefiniteposition:[0,0,0,0],[0,0,30,0.4,"easeOutQuint"],[0,0,100,1,"easeInCirc"]
animaterotation:[0,0,-14,0],[0,0,0,0.4,"easeOutQuint"],[0,0,15,1,"easeInCirc"]
animateposition:[0,0,0,0],[0,0,10,1]
animatedissolve:[0,0],[1,0.1],[1,0.5],[0,1]
track:calmLyrics

82.5:TextToWall
definitedurationbeats:8
path:font.dae
line:I know you loved them
thicc:12
size:0.2
position:[-5,6]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.2,"easeOutExpo"]
animatedefiniteposition:[0,0,0,0],[0,0,30,0.4,"easeOutQuint"],[0,0,100,1,"easeInCirc"]
animaterotation:[0,0,14,0],[0,0,0,0.4,"easeOutQuint"],[0,0,-17,1,"easeInCirc"]
animateposition:[0,0,0,0],[0,0,10,1]
animatedissolve:[0,0],[1,0.1],[1,0.5],[0,1]
track:calmLyrics

89:TextToWall
definitedurationbeats:1
path:font.dae
line: But I
thicc:12
size:0.5
position:[-1,6]
animatedefiniteposition:[20,0,30,0],[0,0,50,1,"easeOutExpo"]
animaterotation:[0,0,0,0],[0,0,20,1,"easeInExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.5],[0,1]

90:TextToWall
definitedurationbeats:2
path:font.dae
line: know 
thicc:12
size:0.5
position:[4,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,50,0],[0,0,60,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

90.75:TextToWall
definitedurationbeats:2
path:font.dae
line:that I can
thicc:12
size:0.5
position:[2,7]
color:[10,10,10,10]
animatedefiniteposition:[0,0,50,0],[0,0,60,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

88:AnimateTrack
duration:2
animaterotation:[0,0,-30,0],[0,0,0,1,"easeOutExpo"]
animateposition:[0,0,0,0],[20,0,0,1,"easeInExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics

92:TextToWall
definitedurationbeats:2
path:font.dae
line: try again 
thicc:12
size:0.5
position:[-9,16]
color:[10,10,10,10]
animatedefiniteposition:[0,0,60,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

93.5:TextToWall
definitedurationbeats:2
path:font.dae
line:to
thicc:12
size:0.5
position:[-12,11]
color:[10,10,10,10]
animatedefiniteposition:[0,0,60,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

90:AnimateTrack
duration:2
animaterotation:[0,0,0,0],[0,0,-30,1,"easeInExpo"]
animateposition:[-20,0,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics2

94:TextToWall
definitedurationbeats:2
path:font.dae
line:warm your heart
thicc:12
size:0.5
position:[6,13]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,0,60,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

95.5:TextToWall
definitedurationbeats:2
path:font.dae
line:please
thicc:12
size:0.5
position:[-8,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,0,60,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

92:AnimateTrack
duration:2
animaterotation:[0,0,30,0],[0,0,0,1,"easeOutExpo"]
animateposition:[0,0,0,0],[-20,0,0,1,"easeInExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics

96:TextToWall
definitedurationbeats:2
path:font.dae
line:understand
thicc:12
size:0.5
position:[4,16]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,0,60,1]
animateposition:[20,0,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

98:TextToWall
definitedurationbeats:1
path:font.dae
line:THINGS
thicc:12
size:0.7
position:[9,10]
rotation:[-6,4,-10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,0,60,1]
animateposition:[-5,0,0,0],[0,0,0,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

99:TextToWall
definitedurationbeats:1
path:font.dae
line:DONT
thicc:12
size:0.7
position:[-8,10]
rotation:[4,6,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,40,0],[0,0,50,1]
animateposition:[5,0,0,0],[0,0,0,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

100:TextToWall
definitedurationbeats:1
path:font.dae
line:STAY
thicc:12
size:0.7
position:[-4,7]
rotation:[-9,8,-4]
color:[10,10,10,10]
animatedefiniteposition:[0,0,60,0],[0,0,50,1]
animateposition:[-5,0,0,0],[0,0,0,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

101:TextToWall
definitedurationbeats:3
path:font.dae
line:FOREVER
thicc:12
size:0.7
position:[-9,0]
rotation:[-16,6,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animateposition:[5,0,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

105.5:TextToWall
definitedurationbeats:0.5
path:font.dae
line:We
thicc:12
size:0.5
position:[10,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animaterotation:[0,0,0,0],[-10,0,0,1,"easeInQuart"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

106:TextToWall
definitedurationbeats:1
path:font.dae
line:say
thicc:12
size:0.6
rotation:[-4,7,3]
position:[-20,9]
color:[10,10,10,10]
animatedefiniteposition:[0,0,60,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

106.5:TextToWall
definitedurationbeats:1
path:font.dae
line:we
thicc:12
size:0.8
rotation:[-4,7,3]
position:[-19,5]
color:[10,10,10,10]
animatedefiniteposition:[0,0,60,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

104:AnimateTrack
duration:1
animaterotation:[10,0,0,0],[0,0,0,1,"easeOutExpo"]
animateposition:[0,0,0,0],[-10,10,20,1,"easeInExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics

107:TextToWall
definitedurationbeats:1
path:font.dae
line:want
thicc:12
size:0.6
rotation:[4,-7,4]
position:[27,14]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

107.5:TextToWall
definitedurationbeats:1
path:font.dae
line:to
thicc:12
size:0.8
rotation:[4,-7,4]
position:[25,11]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

105:AnimateTrack
duration:1
animaterotation:[0,0,0,0],[0,20,0,1,"easeInExpo"]
animateposition:[0,-10,-10,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics2

108:TextToWall
definitedurationbeats:2
path:font.dae
line:make a change
thicc:12
size:0.6
rotation:[0,0,-3]
position:[-11,20]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,90,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

109.5:TextToWall
definitedurationbeats:1
path:font.dae
line:I
thicc:12
size:1
rotation:[0,0,3]
position:[-16,16]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,90,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

106:AnimateTrack
duration:2
animaterotation:[20,0,0,0],[0,0,0,1,"easeOutExpo"]
animateposition:[0,0,0,0],[-10,10,20,1,"easeInExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics

110:TextToWall
definitedurationbeats:1
path:font.dae
line:must
thicc:12
size:0.6
position:[-20,11]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

110.5:TextToWall
definitedurationbeats:1
path:font.dae
line:ad
thicc:12
size:0.8
position:[-19,7]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

108:AnimateTrack
duration:1
animaterotation:[0,0,0,0],[-10,0,-20,1,"easeInExpo"]
animateposition:[10,0,-10,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics2

111:TextToWall
definitedurationbeats:1
path:font.dae
line:mit
thicc:12
size:0.6
position:[18,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

111.5:TextToWall
definitedurationbeats:1
path:font.dae
line:it
thicc:12
size:0.8
position:[20,6]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

109:AnimateTrack
duration:1
animaterotation:[10,0,-20,0],[0,0,0,1,"easeOutExpo"]
animateposition:[0,0,0,0],[15,20,20,1,"easeInExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics

112:TextToWall
definitedurationbeats:2
path:font.dae
line:feels
thicc:12
size:0.6
position:[-20,14]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

112.75:TextToWall
definitedurationbeats:2
path:font.dae
line:a little
thicc:12
size:0.8
position:[-19,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

110:AnimateTrack
duration:2
animaterotation:[0,0,0,0],[0,0,60,1,"easeInExpo"]
animateposition:[-20,0,-40,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics2

114:TextToWall
definitedurationbeats:3
path:font.dae
line:strange
size:0.8
position:[0,11]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,1,"easeOutExpo"]
animatecolor:[10,0,10,10,0],[10,10,10,10,1,"easeInExpo"]
animatedefiniteposition:[0,0,60,0],[0,0,70,1]
animaterotation:[0,0,-60,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.7],[0,1]

117.5:TextToWall
definitedurationbeats:2.5
path:font.dae
line:To
thicc:12
size:0.8
position:[-30,16]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

118:TextToWall
definitedurationbeats:2
path:font.dae
line:see it in a
thicc:12
size:0.6
position:[-25,12]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

115.5:AnimateTrack
duration:2.5
animaterotation:[0,0,0,0],[15,0,0,1,"easeInExpo"]
animateposition:[-40,0,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics

120:TextToWall
definitedurationbeats:2
path:font.dae
line:different way
thicc:12
size:0.8
position:[16,45]
rotation:[20,0,-20]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,-10,70,1]
animaterotation:[-15,0,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.4],[0,1,"easeInExpo"]

121.5:TextToWall
definitedurationbeats:0.5
path:font.dae
line:And
thicc:12
size:0.5
position:[12,20]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animaterotation:[0,0,0,0],[-10,0,0,1,"easeInQuart"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

122:TextToWall
definitedurationbeats:1
path:font.dae
line:even
thicc:12
size:0.6
rotation:[-4,7,3]
position:[-20,9]
color:[10,10,10,10]
animatedefiniteposition:[0,0,60,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

120:AnimateTrack
duration:1
animaterotation:[10,0,0,0],[0,0,0,1,"easeOutExpo"]
animateposition:[0,0,0,0],[-10,10,20,1,"easeInExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics

123:TextToWall
definitedurationbeats:1
path:font.dae
line:if
thicc:12
size:0.6
rotation:[4,-7,4]
position:[27,14]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

123.5:TextToWall
definitedurationbeats:1
path:font.dae
line:it
thicc:12
size:0.6
rotation:[4,-7,4]
position:[27,18]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

121:AnimateTrack
duration:1
animaterotation:[0,0,0,0],[0,20,0,1,"easeInExpo"]
animateposition:[0,-10,-10,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics2

124:TextToWall
definitedurationbeats:2
path:font.dae
line:passes by
thicc:12
size:0.6
rotation:[0,0,-3]
position:[-11,20]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,90,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

125.5:TextToWall
definitedurationbeats:1
path:font.dae
line:this
thicc:12
size:1
rotation:[0,0,3]
position:[-16,16]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,90,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

122:AnimateTrack
duration:2
animaterotation:[20,0,0,0],[0,0,0,1,"easeOutExpo"]
animateposition:[0,0,0,0],[-10,10,20,1,"easeInExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics

126:TextToWall
definitedurationbeats:2
path:font.dae
line:isnt how
thicc:12
size:0.6
position:[-20,11]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

127.5:TextToWall
definitedurationbeats:1
path:font.dae
line:we
thicc:12
size:0.8
position:[-19,7]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

124:AnimateTrack
duration:2
animaterotation:[0,0,0,0],[-10,0,-20,1,"easeInExpo"]
animateposition:[10,0,-10,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics2

128:TextToWall
definitedurationbeats:2
path:font.dae
line:say goodbye
thicc:12
size:0.5
position:[20,16]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,-3,60,1]
animaterotation:[0,0,30,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

130:TextToWall
definitedurationbeats:1
path:font.dae
line:JUST
thicc:12
size:0.7
position:[9,10]
rotation:[-6,4,-10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,0,60,1]
animateposition:[-5,0,0,0],[0,0,0,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

131:TextToWall
definitedurationbeats:1
path:font.dae
line:UN
thicc:12
size:0.7
position:[-15,10]
rotation:[4,6,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,40,0],[0,0,50,1]
animateposition:[5,0,0,0],[0,0,0,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

132:TextToWall
definitedurationbeats:1.5
path:font.dae
line:TIL
thicc:12
size:0.7
position:[5,7]
rotation:[-9,8,-4]
color:[10,10,10,10]
animatedefiniteposition:[0,0,60,0],[0,0,50,1]
animateposition:[-5,0,0,0],[0,0,0,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

133.5:TextToWall
definitedurationbeats:4
path:font.dae
line:NEXT
thicc:12
size:0.7
position:[-9,0]
rotation:[-16,6,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animateposition:[5,0,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

134.5:TextToWall
definitedurationbeats:3
path:font.dae
line:TIME
thicc:12
size:0.7
position:[-9,-4]
rotation:[-16,6,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animateposition:[5,0,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

137.5:TextToWall
definitedurationbeats:0.5
path:font.dae
line:Theres
thicc:12
size:0.5
position:[-15,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animateposition:[0,0,0,0],[0,-10,0,1,"easeInQuart"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

138:TextToWall
definitedurationbeats:2
path:font.dae
line:nothing wrong
thicc:12
size:0.5
position:[4,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,50,0],[0,0,60,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

139.5:TextToWall
definitedurationbeats:1
path:font.dae
line:with
thicc:12
size:0.5
position:[2,6]
color:[10,10,10,10]
animatedefiniteposition:[0,0,50,0],[0,0,60,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

136:AnimateTrack
duration:2
animaterotation:[0,0,0,0],[0,0,-30,1,"easeInExpo"]
animateposition:[0,10,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics

140:TextToWall
definitedurationbeats:2
path:font.dae
line:shedding tears
thicc:12
size:0.5
position:[-9,16]
color:[10,10,10,10]
animatedefiniteposition:[0,0,60,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

141.5:TextToWall
definitedurationbeats:1
path:font.dae
line:to
thicc:12
size:0.5
position:[-12,11]
color:[10,10,10,10]
animatedefiniteposition:[0,0,60,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

138:AnimateTrack
duration:2
animaterotation:[0,0,30,0],[0,0,0,1,"easeOutExpo"]
animateposition:[0,0,0,0],[-10,0,0,1,"easeInExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics2

142:TextToWall
definitedurationbeats:1
path:font.dae
line:make
thicc:12
size:0.6
rotation:[4,-7,4]
position:[27,14]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

142.5:TextToWall
definitedurationbeats:1
path:font.dae
line:sure
thicc:12
size:0.6
rotation:[4,-7,4]
position:[27,18]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

140:AnimateTrack
duration:1
animaterotation:[0,0,0,0],[0,20,0,1,"easeInExpo"]
animateposition:[0,0,-10,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics

143:TextToWall
definitedurationbeats:1
path:font.dae
line:that
thicc:12
size:0.6
position:[-19,14]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,0,60,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

143.5:TextToWall
definitedurationbeats:1
path:font.dae
line:your
thicc:12
size:0.6
position:[-19,18]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,0,60,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

141:AnimateTrack
duration:1
animaterotation:[-10,0,0,0],[0,0,0,1,"easeOutExpo"]
animateposition:[0,0,0,0],[-20,0,0,1,"easeInExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics2

144:TextToWall
definitedurationbeats:2
path:font.dae
line:sight is clear
thicc:12
size:0.5
position:[4,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,50,0],[0,0,60,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

145.75:TextToWall
definitedurationbeats:1
path:font.dae
line:and
thicc:12
size:0.5
position:[2,7]
color:[10,10,10,10]
animatedefiniteposition:[0,0,50,0],[0,0,60,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

142:AnimateTrack
duration:2
animateposition:[10,0,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics

146:TextToWall
definitedurationbeats:1
path:font.dae
line:BRING
thicc:12
size:0.7
position:[9,10]
rotation:[-6,4,-10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,0,60,1]
animateposition:[-5,0,0,0],[0,0,0,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

147:TextToWall
definitedurationbeats:2
path:font.dae
line:TO
thicc:12
size:0.7
position:[-15,10]
rotation:[4,6,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,40,0],[0,0,50,1]
animateposition:[5,0,0,0],[0,0,0,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

147.5:TextToWall
definitedurationbeats:1.5
path:font.dae
line:LIGHT
thicc:12
size:0.7
position:[-15,7]
rotation:[4,6,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,40,0],[0,0,50,1]
animateposition:[5,0,0,0],[0,0,0,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

149:TextToWall
definitedurationbeats:1
path:font.dae
line:OUR
thicc:12
size:0.7
position:[5,7]
rotation:[-9,8,-4]
color:[10,10,10,10]
animatedefiniteposition:[0,0,60,0],[0,0,50,1]
animateposition:[-5,0,0,0],[0,0,0,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

150:TextToWall
definitedurationbeats:4
path:font.dae
line:SILLY 
thicc:12
size:0.7
position:[-40,0]
rotation:[-16,6,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animateposition:[5,0,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1]
track:lyrics

151.5:TextToWall
definitedurationbeats:4
path:font.dae
line:FEARS
thicc:12
size:0.7
position:[-40,-4]
rotation:[-16,6,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animateposition:[5,0,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1]
track:lyrics

148:AnimateTrack
track:lyrics
duration:4
animatedissolve:[1,0.8],[0,1]
animateposition:[0,0,0,0],[10,0,0,1,"easeInExpo"]

164:TextToWall
path:font.png
line:its terrific
size:0.1
position:[0,7]
color:[0,0,0,1]
definitedurationbeats:6
animatedefiniteposition:[0,0,40,0],[0,0,40,1]
animatedissolve:[0,0],[0.8,0.5,"easeOutSine"],[0.8,1],[0,1]

165:TextToWall
path:font.png
line:im so proud of you
size:0.1
position:[0,4]
color:[0,0,0,1]
definitedurationbeats:5
animatedefiniteposition:[0,0,30,0],[0,0,30,1]
animatedissolve:[0,0],[0.8,1,"easeOutSine"],[0,1]

231.5:TextToWall
definitedurationbeats:4
path:font.dae
line:And
thicc:12
size:0.2
position:[0,7]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.75,"easeOutExpo"]
animatedefiniteposition:[0,0,0,0],[0,0,40,0.75,"easeOutExpo"]
animaterotation:[0,0,-10,0],[0,0,0,0.75,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.75],[0,1]
track:calmLyrics

232.5:TextToWall
definitedurationbeats:4
path:font.dae
line:just when you forgot
thicc:12
size:0.2
position:[0,5]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.75,"easeOutExpo"]
animatedefiniteposition:[0,0,0,0],[0,0,40,0.75,"easeOutExpo"]
animaterotation:[0,0,-10,0],[0,0,0,0.75,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.75],[0,1]
track:calmLyrics

235:TextToWall
definitedurationbeats:4
path:font.dae
line:maybe
thicc:12
size:0.2
position:[10,9]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.75,"easeOutExpo"]
animatedefiniteposition:[0,0,0,0],[0,0,40,0.75,"easeOutExpo"]
animaterotation:[0,0,-10,0],[0,0,0,0.75,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.75],[0,1]
track:calmLyrics

236:TextToWall
definitedurationbeats:4
path:font.dae
line:take the time to stop
thicc:12
size:0.2
position:[10,6]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.75,"easeOutExpo"]
animatedefiniteposition:[0,0,0,0],[0,0,30,0.75,"easeOutExpo"]
animaterotation:[0,0,14,0],[0,0,4,0.75,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.75],[0,1]
track:calmLyrics

239.5:TextToWall
definitedurationbeats:4
path:font.dae
line:and
thicc:12
size:0.2
position:[-10,6]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.75,"easeOutExpo"]
animatedefiniteposition:[0,0,0,0],[0,0,30,0.75,"easeOutExpo"]
animaterotation:[0,0,14,0],[0,0,4,0.75,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.75],[0,1]
track:calmLyrics

240:TextToWall
definitedurationbeats:4
path:font.dae
line:cherish what we have
thicc:12
size:0.2
position:[-13,2]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.75,"easeOutExpo"]
animatedefiniteposition:[0,0,0,0],[0,0,30,0.75,"easeOutExpo"]
animaterotation:[0,0,10,0],[0,0,-4,0.75,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.75],[0,1]
track:calmLyrics

242.75:TextToWall
definitedurationbeats:4
path:font.dae
line:instead of
thicc:12
size:0.2
position:[-3,11]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.75,"easeOutExpo"]
animatedefiniteposition:[0,0,0,0],[0,0,30,0.75,"easeOutExpo"]
animaterotation:[0,0,10,0],[0,0,-4,0.75,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.75],[0,1]
track:calmLyrics

244:TextToWall
definitedurationbeats:4
path:font.dae
line:missing what we had
thicc:12
size:0.2
position:[7,8]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.75,"easeOutExpo"]
animatedefiniteposition:[0,0,0,0],[0,0,40,0.75,"easeOutExpo"]
animaterotation:[0,0,14,0],[0,0,4,0.75,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.75],[0,1]
track:calmLyrics

247.5:TextToWall
definitedurationbeats:4
path:font.dae
line:you
thicc:12
size:0.2
position:[7,10]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.75,"easeOutExpo"]
animatedefiniteposition:[0,0,0,0],[0,0,40,0.75,"easeOutExpo"]
animaterotation:[0,0,14,0],[0,0,4,0.75,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.75],[0,1]
track:calmLyrics

248:TextToWall
definitedurationbeats:4
path:font.dae
line:only notice when its gone
thicc:12
size:0.2
position:[-3,8]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.75,"easeOutExpo"]
animatedefiniteposition:[0,0,0,0],[0,0,40,0.75,"easeOutExpo"]
animaterotation:[0,0,-10,0],[0,0,0,0.75,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.75],[0,1]
track:calmLyrics

251.5:TextToWall
definitedurationbeats:4
path:font.dae
line:be
thicc:12
size:0.2
position:[-13,6]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.75,"easeOutExpo"]
animatedefiniteposition:[0,0,0,0],[0,0,40,0.75,"easeOutExpo"]
animaterotation:[0,0,-10,0],[0,0,0,0.75,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.75],[0,1]
track:calmLyrics

252:TextToWall
definitedurationbeats:4
path:font.dae
line:cause to satisfy
thicc:12
size:0.2
position:[-7,4]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.75,"easeOutExpo"]
animatedefiniteposition:[0,0,0,0],[0,0,40,0.75,"easeOutExpo"]
animaterotation:[0,0,10,0],[0,0,0,0.75,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.75],[0,1]
track:calmLyrics

255:TextToWall
definitedurationbeats:6
path:font.dae
line:WHAT YOU LONG FOR
thicc:12
size:0.7
position:[0,8]
color:[10,10,10,10]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.75,"easeOutExpo"]
animatedefiniteposition:[0,30,20,0],[0,0,60,0.75,"easeOutExpo"]
animaterotation:[-20,0,0,0],[0,0,0,0.75,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.75],[0,1]
track:calmLyrics

266:TextToWall
definitedurationbeats:2
path:font.dae
line:You will
thicc:12
size:0.5
position:[0,11]
color:[10,10,10,10]
animatedefiniteposition:[0,0,50,0],[0,0,60,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

267:TextToWall
definitedurationbeats:2
path:font.dae
line:have to
thicc:12
size:0.5
position:[0,7]
color:[10,10,10,10]
animatedefiniteposition:[0,0,50,0],[0,0,60,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

264:AnimateTrack
duration:2
animaterotation:[0,0,-30,0],[0,0,0,1,"easeOutExpo"]
animateposition:[0,0,0,0],[20,0,0,1,"easeInExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics

268:TextToWall
definitedurationbeats:2
path:font.dae
line:try again
thicc:12
size:0.5
position:[-9,19]
color:[10,10,10,10]
animatedefiniteposition:[0,0,60,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

269.5:TextToWall
definitedurationbeats:2
path:font.dae
line:to
thicc:12
size:0.5
position:[-3,14]
color:[10,10,10,10]
animatedefiniteposition:[0,0,60,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

266:AnimateTrack
duration:2
animaterotation:[0,0,0,0],[0,0,-30,1,"easeInExpo"]
animateposition:[-20,0,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics2

270:TextToWall
definitedurationbeats:2
path:font.dae
line:fill your heart
thicc:12
size:0.4
position:[6,13]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,0,60,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

271.5:TextToWall
definitedurationbeats:2
path:font.dae
line:please
thicc:12
size:0.5
position:[-8,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,0,60,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

268:AnimateTrack
duration:2
animaterotation:[0,0,30,0],[0,0,0,1,"easeOutExpo"]
animateposition:[0,0,0,0],[-20,0,0,1,"easeInExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics

272:TextToWall
definitedurationbeats:2
path:font.dae
line:understand
thicc:12
size:0.5
position:[4,16]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,0,60,1]
animateposition:[20,0,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

274:TextToWall
definitedurationbeats:1
path:font.dae
line:THINGS
thicc:12
size:0.7
position:[9,10]
rotation:[-6,4,-10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,0,60,1]
animateposition:[-5,0,0,0],[0,0,0,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

275:TextToWall
definitedurationbeats:1
path:font.dae
line:DONT
thicc:12
size:0.7
position:[-8,10]
rotation:[4,6,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,40,0],[0,0,50,1]
animateposition:[5,0,0,0],[0,0,0,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

276:TextToWall
definitedurationbeats:1
path:font.dae
line:STAY
thicc:12
size:0.7
position:[-4,7]
rotation:[-9,8,-4]
color:[10,10,10,10]
animatedefiniteposition:[0,0,60,0],[0,0,50,1]
animateposition:[-5,0,0,0],[0,0,0,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

277:TextToWall
definitedurationbeats:3
path:font.dae
line:FOREVER
thicc:12
size:0.7
position:[-9,0]
rotation:[-16,6,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animateposition:[5,0,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

281.5:TextToWall
definitedurationbeats:0.5
path:font.dae
line:You
thicc:12
size:0.5
position:[10,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animaterotation:[0,0,0,0],[0,0,-10,1,"easeInQuart"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

282:TextToWall
definitedurationbeats:2
path:font.dae
line:know youve got
thicc:12
size:0.3
position:[1,13]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,0,60,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

283.5:TextToWall
definitedurationbeats:2
path:font.dae
line:to
thicc:12
size:0.5
position:[0,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,0,60,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

280:AnimateTrack
duration:2
animaterotation:[0,0,30,0],[0,0,0,1,"easeOutExpo"]
animateposition:[0,0,0,0],[-20,0,0,1,"easeInExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics

284:TextToWall
definitedurationbeats:2
path:font.dae
line:make a change
thicc:12
size:0.5
rotation:[0,0,-3]
position:[0,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,90,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

285.5:TextToWall
definitedurationbeats:1
path:font.dae
line:you
thicc:12
size:0.7
rotation:[0,0,-3]
position:[-20,3]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,90,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

282:AnimateTrack
duration:2
animaterotation:[0,0,-20,0],[0,0,0,1,"easeOutExpo"]
animateposition:[0,0,0,0],[10,10,20,1,"easeInExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics2

286:TextToWall
definitedurationbeats:2
path:font.dae
line:must admit
thicc:12
size:0.5
position:[6,11]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,0,60,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

287.5:TextToWall
definitedurationbeats:2
path:font.dae
line:it
thicc:12
size:0.7
position:[10,8]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,0,60,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

284:AnimateTrack
duration:2
animaterotation:[0,0,0,0],[-20,0,-20,1,"easeInExpo"]
animateposition:[-20,0,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics

288:TextToWall
definitedurationbeats:2
path:font.dae
line:feels
thicc:12
size:0.4
position:[0,14]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

288.75:TextToWall
definitedurationbeats:2
path:font.dae
line:a little
thicc:12
size:0.5
position:[0,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

286:AnimateTrack
duration:2
animaterotation:[0,0,0,0],[0,0,60,1,"easeInExpo"]
animateposition:[-20,0,-40,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics2

290:TextToWall
definitedurationbeats:3
path:font.dae
line:strange
size:0.7
position:[0,8]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,1,"easeOutExpo"]
animatecolor:[10,0,10,10,0],[10,10,10,10,1,"easeInExpo"]
animatedefiniteposition:[0,0,60,0],[0,0,70,1]
animaterotation:[0,0,-60,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.7],[0,1]

293.5:TextToWall
definitedurationbeats:2.5
path:font.dae
line:To
thicc:12
size:0.8
position:[-5,16]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

294:TextToWall
definitedurationbeats:2
path:font.dae
line:see it in a
thicc:12
size:0.4
position:[-7,12]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

291.5:AnimateTrack
duration:2.5
animaterotation:[0,0,0,0],[15,0,0,1,"easeInExpo"]
animateposition:[-10,0,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics

296:TextToWall
definitedurationbeats:2
path:font.dae
line:different way
thicc:12
size:0.4
position:[-10,45]
rotation:[20,0,-20]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,-10,70,1]
animaterotation:[-15,0,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.4],[0,1,"easeInExpo"]

297.5:TextToWall
definitedurationbeats:0.5
path:font.dae
line:And
thicc:12
size:0.5
position:[10,15]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animaterotation:[0,0,0,0],[-10,0,0,1,"easeInQuart"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

298:TextToWall
definitedurationbeats:2
path:font.dae
line:even when
thicc:12
size:0.5
rotation:[0,0,-3]
position:[0,18]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,90,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

299.5:TextToWall
definitedurationbeats:1
path:font.dae
line:it
thicc:12
size:0.7
rotation:[0,0,-3]
position:[0,14]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,90,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

296:AnimateTrack
duration:2
animaterotation:[0,0,-20,0],[0,0,0,1,"easeOutExpo"]
animateposition:[0,0,0,0],[10,10,20,1,"easeInExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics2

300:TextToWall
definitedurationbeats:2
path:font.dae
line:passes by
thicc:12
size:0.6
rotation:[0,0,-3]
position:[0,20]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,90,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

301.5:TextToWall
definitedurationbeats:1
path:font.dae
line:that
thicc:12
size:1
rotation:[0,0,3]
position:[4,14]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,90,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

298:AnimateTrack
duration:2
animaterotation:[20,0,0,0],[0,0,0,1,"easeOutExpo"]
animateposition:[0,0,0,0],[-10,10,20,1,"easeInExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics

302:TextToWall
definitedurationbeats:2
path:font.dae
line:wasnt how
thicc:12
size:0.6
position:[4,15]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

303.5:TextToWall
definitedurationbeats:1
path:font.dae
line:you
thicc:12
size:0.8
position:[0,11]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

300:AnimateTrack
duration:2
animaterotation:[0,0,0,0],[-10,0,-20,1,"easeInExpo"]
animateposition:[10,0,-10,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics2

304:TextToWall
definitedurationbeats:2
path:font.dae
line:said goodbye
thicc:12
size:0.5
position:[1,16]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,-3,60,1]
animaterotation:[0,0,30,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

306:TextToWall
definitedurationbeats:1
path:font.dae
line:JUST
thicc:12
size:0.7
position:[9,10]
rotation:[-6,4,-10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,0,60,1]
animateposition:[-5,0,0,0],[0,0,0,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

307:TextToWall
definitedurationbeats:1
path:font.dae
line:UN
thicc:12
size:0.7
position:[-15,10]
rotation:[4,6,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,40,0],[0,0,50,1]
animateposition:[5,0,0,0],[0,0,0,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

308:TextToWall
definitedurationbeats:1.5
path:font.dae
line:TIL
thicc:12
size:0.7
position:[5,7]
rotation:[-9,8,-4]
color:[10,10,10,10]
animatedefiniteposition:[0,0,60,0],[0,0,50,1]
animateposition:[-5,0,0,0],[0,0,0,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

309.5:TextToWall
definitedurationbeats:4
path:font.dae
line:NEXT
thicc:12
size:0.7
position:[-9,0]
rotation:[-16,6,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animateposition:[5,0,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

310.5:TextToWall
definitedurationbeats:3
path:font.dae
line:TIME
thicc:12
size:0.7
position:[-9,-4]
rotation:[-16,6,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animateposition:[5,0,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

313.5:TextToWall
definitedurationbeats:0.5
path:font.dae
line:Youll
thicc:12
size:0.5
position:[0,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animateposition:[0,0,0,0],[0,-10,0,1,"easeInQuart"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

314:TextToWall
definitedurationbeats:2
path:font.dae
line:still get through
thicc:12
size:0.3
position:[4,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,50,0],[0,0,60,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

315.5:TextToWall
definitedurationbeats:1
path:font.dae
line:the
thicc:12
size:0.5
position:[5,6]
color:[10,10,10,10]
animatedefiniteposition:[0,0,50,0],[0,0,60,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

312:AnimateTrack
duration:2
animaterotation:[0,0,0,0],[0,0,-30,1,"easeInExpo"]
animateposition:[0,10,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics

316:TextToWall
definitedurationbeats:2
path:font.dae
line:falling tears
thicc:12
size:0.4
position:[1,16]
color:[10,10,10,10]
animatedefiniteposition:[0,0,60,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

317.5:TextToWall
definitedurationbeats:1
path:font.dae
line:and
thicc:12
size:0.5
position:[-2,11]
color:[10,10,10,10]
animatedefiniteposition:[0,0,60,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics2

314:AnimateTrack
duration:2
animaterotation:[0,0,30,0],[0,0,0,1,"easeOutExpo"]
animateposition:[0,0,0,0],[-10,0,0,1,"easeInExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics2

318:TextToWall
definitedurationbeats:2
path:font.dae
line:now you know
thicc:12
size:0.4
position:[0,11]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

319.5:TextToWall
definitedurationbeats:1
path:font.dae
line:your
thicc:12
size:0.8
position:[-2,7]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animatedissolve:[0,0],[1,0.1]
track:lyrics

316:AnimateTrack
duration:2
animaterotation:[0,0,0,0],[-10,0,-20,1,"easeInExpo"]
animateposition:[10,0,-10,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]
track:lyrics

320:TextToWall
definitedurationbeats:2
path:font.dae
line:sight is clear
thicc:12
size:0.5
position:[1,16]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,-3,60,1]
animaterotation:[0,0,30,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

322:TextToWall
definitedurationbeats:1
path:font.dae
line:BRING
thicc:12
size:0.7
position:[9,10]
rotation:[-6,4,-10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,70,0],[0,0,60,1]
animateposition:[-5,0,0,0],[0,0,0,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

323:TextToWall
definitedurationbeats:2
path:font.dae
line:TO
thicc:12
size:0.7
position:[-15,12]
rotation:[4,6,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,40,0],[0,0,50,1]
animateposition:[5,0,0,0],[0,0,0,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

323.75:TextToWall
definitedurationbeats:1.5
path:font.dae
line:LIGHT
thicc:12
size:0.7
position:[-15,7]
rotation:[4,6,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,40,0],[0,0,50,1]
animateposition:[5,0,0,0],[0,0,0,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

325:TextToWall
definitedurationbeats:1
path:font.dae
line:OUR
thicc:12
size:0.7
position:[5,7]
rotation:[-9,8,-4]
color:[10,10,10,10]
animatedefiniteposition:[0,0,60,0],[0,0,50,1]
animateposition:[-5,0,0,0],[0,0,0,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[1,0.9],[0,1]

326:TextToWall
definitedurationbeats:4
path:font.dae
line:SILLY 
thicc:12
size:0.7
position:[-10,-10]
rotation:[-16,6,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animateposition:[5,0,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1]
track:lyrics3

327.5:TextToWall
definitedurationbeats:2.5
path:font.dae
line:FEARS
thicc:12
size:0.7
position:[-10,-14]
rotation:[-16,6,10]
color:[10,10,10,10]
animatedefiniteposition:[0,0,80,0],[0,0,70,1]
animateposition:[5,0,0,0],[0,0,0,1,"easeOutExpo"]
animatedissolve:[0,0],[1,0.1]
track:lyrics3

324:AnimateTrack
track:lyrics3
duration:4
animateposition:[0,0,0,0],[10,0,0,1,"easeInExpo"]

330:TextToWall
definitedurationbeats:2
path:font.dae
line:SILLY
size:0.7
position:[0,-10]
rotation:[-16,6,10]
animatecolor:[10,10,10,1,0],[0,0,0,10,0.3]
animatedefiniteposition:[0,0,70,0],[0,0,70,1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[0,1]
track:fearsExplode

330: TextToWall
definitedurationbeats:2
path:font.dae
line:FEARS
size:0.7
position:[0,-14]
rotation:[-16,6,10]
animatecolor:[10,10,10,1,0],[0,0,0,10,0.3]
animatedefiniteposition:[0,0,70,0],[0,0,70,1,"easeOutSine"]
animatelocalrotation:[0,0,0,0],[Random(0,360),Random(0,360),Random(0,360),1,"easeOutSine"]
animatedissolve:[0,0],[1,0.1],[0,1]
track:fearsExplode

328:AnimateTrack
duration:2
track:fearsExplode
animateposition:[0,0,0,0],[10,0,0,1,"easeOutExpo"]

382.5:TextToWall
definitedurationbeats:8
path:font.dae
line:I know you loved them
thicc:12
size:0.2
position:[12,10]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.2,"easeOutExpo"]
animatedefiniteposition:[0,0,0,0],[0,0,30,0.4,"easeOutQuint"],[0,0,100,1,"easeInCirc"]
animaterotation:[0,0,14,0],[0,0,0,0.4,"easeOutQuint"],[0,0,-15,1,"easeInCirc"]
animateposition:[0,0,0,0],[0,0,10,1]
animatedissolve:[0,0],[1,0.1]
track:calmLyrics

386.5:TextToWall
definitedurationbeats:8
path:font.dae
line:I know you loved them
thicc:12
size:0.2
position:[5,6]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.2,"easeOutExpo"]
animatedefiniteposition:[0,0,0,0],[0,0,30,0.4,"easeOutQuint"],[0,0,100,1,"easeInCirc"]
animaterotation:[0,0,-14,0],[0,0,0,0.4,"easeOutQuint"],[0,0,17,1,"easeInCirc"]
animateposition:[0,0,0,0],[0,0,10,1]
animatedissolve:[0,0],[1,0.1]
track:calmLyrics

391.5:AnimateTrack
track:calmLyrics
duration:0.5
animatedissolve:[1,0],[0,1]

394:TextToWall
definitedurationbeats:8
path:font.dae
line: Mapped by Swifter
thicc:12
size:0.5
position:[-1.5,10]
color:[20,20,20,20]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.25,"easeOutExpo"]
animatedefiniteposition:[0,0,20,0],[0,0,70,0.25,"easeOutExpo"]
animatedissolve:[0,0],[1,0.125],[0,1]
track:title2

394:TextToWall
definitedurationbeats:8
path:font.dae
line:Thank you for playing
thicc:12
size:0.4
position:[0,6]
animatelocalrotation:[Random(0,360),Random(0,360),Random(0,360),0],[0,0,0,0.25,"easeOutExpo"]
animatedefiniteposition:[0,0,20,0],[0,0,70,0.25,"easeOutExpo"]
animatedissolve:[0,0],[1,0.125],[0,1]
color:[0,20,0,20]
track:title2

0:AppendToAllNotesBetween
tobeat:87.5
disablenotegravity:true
disablespawneffect:true
njsoffset:3
animatedissolve:[0,0],[1,0.4,"easeOutSine"]
animatedissolvearrow:[0,0],[1,0.4,"easeOutSine"]
animatelocalrotation:[Random(-20,20),Random(-20,20),Random(-20,20),0],[0,0,0,0.4,"easeOutSine"]
animateposition:[Random(-3,3),Random(0,4),Random(0,-6),0],[0,0,0,0.4,"easeOutSine"]

88:AppendToAllNotesBetween
tobeat:152
disablenotegravity:true
disablespawneffect:true
njsoffset:2
njs:20
animatedissolve:[0,0.125],[1,0.125,"easeOutSine"]
animatedissolvearrow:[0,0.125],[1,0.125,"easeOutSine"]
animateposition:[0,0,20,0.125],[0,0,0,0.375,"easeOutCubic"]
animatelocalrotation:[Random(90,270),Random(90,270),Random(90,270),0.125],[0,0,0,0.375,"easeOutCubic"]
animaterotation:[Random(10,0),Random(-20,20),0,0.125],[0,0,0,0.375,"easeOutCubic"]

153:AppendToAllNotesBetween
tobeat:199
disablenotegravity:true
disablespawneffect:true
njsoffset:3
animatedissolve:[0,0],[1,0.4,"easeOutSine"]
animatedissolvearrow:[0,0],[1,0.4,"easeOutSine"]
animatelocalrotation:[Random(-20,20),Random(-20,20),Random(-20,20),0],[0,0,0,0.4,"easeOutSine"]
animateposition:[Random(-3,3),Random(0,4),Random(0,-6),0],[0,0,0,0.4,"easeOutSine"]

200:AppendToAllNotesBetween
tobeat:231
disablenotegravity:true
disablespawneffect:true
njsoffset:5
animatedissolve:[0,0],[1,0.7,"easeOutSine"]
animatedissolvearrow:[0,0],[1,0.7,"easeOutSine"]
animatelocalrotation:[Random(-20,20),Random(-20,20),Random(-20,20),0],[0,0,0,0.4,"easeOutSine"]
animateposition:[Random(-3,3),Random(0,4),Random(0,-6),0],[0,0,0,0.4,"easeOutSine"]

231.5:AppendToAllNotesBetween
tobeat:263
disablenotegravity:true
disablespawneffect:true
njsoffset:3
animatedissolve:[0,0],[1,0.4,"easeOutSine"]
animatedissolvearrow:[0,0],[1,0.4,"easeOutSine"]
animatelocalrotation:[Random(-20,20),Random(-20,20),Random(-20,20),0],[0,0,0,0.4,"easeOutSine"]
animateposition:[Random(-3,3),Random(0,4),Random(0,-6),0],[0,0,0,0.4,"easeOutSine"]

264:AppendToAllNotesBetween
tobeat:327.75
disablenotegravity:true
disablespawneffect:true
njsoffset:2
njs:20
animatedissolve:[0,0.125],[1,0.125,"easeOutSine"]
animatedissolvearrow:[0,0.125],[1,0.125,"easeOutSine"]
animateposition:[0,20,20,0.125],[0,0,0,0.375,"easeOutCubic"]
animatelocalrotation:[Random(90,270),Random(90,270),Random(90,270),0.125],[0,0,0,0.375,"easeOutCubic"]
animaterotation:[Random(10,-10),Random(-20,20),0,0.125],[0,0,0,0.375,"easeOutCubic"]

328:AppendToAllNotesBetween
tobeat:391.5
disablenotegravity:true
disablespawneffect:true
njsoffset:5
animatedissolve:[0,0],[1,0.7,"easeOutSine"]
animatedissolvearrow:[0,0],[1,0.7,"easeOutSine"]
animatelocalrotation:[Random(-20,20),Random(-20,20),Random(-20,20),0],[0,0,0,0.4,"easeOutSine"]
animateposition:[Random(-3,3),Random(0,4),Random(0,-6),0],[0,0,0,0.4,"easeOutSine"]

0:AnimateTrack
track:leftMidLasers
animatelocalposition:[0,-200000,0,0]

0:AnimateTrack
track:rightMidLasers
animatelocalposition:[0,-200000,0,0]

23.75:AnimateTrack
track:leftMidLasers
animatelocalposition:[-0.5,-7.3,-7,0]

23.75:AnimateTrack
track:rightMidLasers
animatelocalposition:[0.5,-7.3,-7,0]

152:AnimateTrack
track:leftMidLasers
animatelocalposition:[0,-200000,0,0]

152:AnimateTrack
track:rightMidLasers
animatelocalposition:[0,-200000,0,0]

168:AnimateTrack
track:leftMidLasers
animatelocalposition:[-0.5,-7.3,-7,0]

168:AnimateTrack
track:rightMidLasers
animatelocalposition:[0.5,-7.3,-7,0]

328:AnimateTrack
track:leftMidLasers
animatelocalposition:[0,-200000,0,0]

328:AnimateTrack
track:rightMidLasers
animatelocalposition:[0,-200000,0,0]
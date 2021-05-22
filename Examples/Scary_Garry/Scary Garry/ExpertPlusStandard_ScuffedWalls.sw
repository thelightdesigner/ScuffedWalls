# ScuffedWalls v1.1.3

# Documentation on functions can be found at
# https://github.com/thelightdesigner/ScuffedWalls/blob/main/Functions.md
            
# DM @thelightdesigner#1337 for more help?

# Using this tool requires an understanding of Noodle Extensions.
# https://github.com/Aeroluna/NoodleExtensions/blob/master/Documentation/AnimationDocs.md

# Playtest your maps

Workspace:Default

0: Import
   Path:ExpertPlusStandard_Old.dat

22:appendtoallnotesbetween
tobeat:117.5
AnimatePosition:[0,-2,3,0],[0,0,0,0.1]
animatedissolve: [0,0],[1,0.15]

118:appendtoallnotesbetween
AnimateDissolveArrow: [0,0],[1,0.3]
AnimatePosition:[Random(-7,6),Random(-6,6),0,0],[0,0,0,0.35,"easeOutCubic"],[0,0,0,1]
njs:20
njsoffset:0.1
animatedissolve: [0,0],[1,0.15]

118:note
position: [-6,2,2]
repeat:126
repeataddtime: 0.5
type:3
animatescale: [0,0,0,0],[7,7,7,0.15]
AnimatePosition:[-10,4,3,0],[0,0,0,0.15]
animaterotation: [5,-360,0,0], [60,360,0,0.2]
fake:true
animatedissolve: [0,0],[1,0.5]
rgbcolor: [255,0,0,1]

118:note
position: [6,2,2]
repeat:126
repeataddtime: 0.5
type:3
animatescale: [0,0,0,0],[7,7,7,0.15]
AnimatePosition:[10,4,3,0],[0,0,0,0.15]
animaterotation: [5,-360,0,0], [-60,360,0,0.2]
fake:true
animatedissolve: [0,0],[1,0.5]
rgbcolor: [255,0,0,1]

118:note
position: [-6,2,2]
repeat:126
repeataddtime: 0.5
type:3
animatescale: [0,0,0,0],[7,7,7,0.15]
AnimatePosition:[-10,4,3,0],[0,0,0,0.15]
animaterotation: [5,-360,0,0], [-60,360,0,0.2]
fake:true
animatedissolve: [0,0],[1,0.5]
rgbcolor: [0,0,255,1]

118:note
position: [6,2,2]
repeat:126
repeataddtime: 0.5
type:3
animatescale: [0,0,0,0],[7,7,7,0.15]
AnimatePosition:[10,4,3,0],[0,0,0,0.15]
animaterotation: [5,-360,0,0], [60,360,0,0.2]
fake:true
animatedissolve: [0,0],[1,0.5]
rgbcolor: [0,0,255,1]
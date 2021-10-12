# TextToWall

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/geico.png)

We have a beginners tutorial! [see here](https://www.youtube.com/watch?v=g49gfMtzETY)

## Font Model
Scuffedwalls can use a 3d model to create wall text, see [`here`](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Blender%20Project.md) for more information on 3d modeling for wall conversion. Groups of cubes can be assigned as a letter by having a material named one of the letters from the character list below. example: Letter_a, Letter_b...Letter_A... Letter_quotationmark, Letter_apostrophe...

a b c d e f g h i j k l m n o p q r s t u v w x y z
A B C D E F G H I J K L M N O P Q R S T U V W X Y Z
questionmark period exclamation apostrophe dash quotationmark greaterthan lessthan paranthesisleft paranthesisright curleybraceleft curleybraceright

All the letters should be roughly one blender unit tall in a line on the X axis.

![](https://github.com/thelightdesigner/ScuffedWalls/blob/1.0/Readme/litefont.jpg)

Community pre-made font models:


- [`TzurS11Font.blend`](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Examples/fonts/TzurS11Font.blend)      [`TzurS11Font.dae`](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Examples/fonts/TzurS11Font.dae)

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/TzurS11Font.png)

- [`litefont.blend`](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Examples/fonts/litefont.blend)      [`litefont.dae`](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Examples/fonts/litefont.dae)

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/litefont.png)

- [`swifterfont.blend`](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Examples/fonts/swifterfont.blend)      [`swifterfont.dae`](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Examples/fonts/swifterfont.dae)

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/swifterfont.png)

- [`AirscrachFont.blend`](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Examples/fonts/AirscrachFont.blend)  [`AirscrachFont.dae`](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Examples/fonts/AirscrachFont.dae)

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/AirscrachFont.png)

```
5:TextToWall
   line:oh god
   line:oh fuck
   line:Aero trapped me in beat saber
   path:litefont.dae
   size:0.1
   thicc:12
   animatedefiniteposition:[0,0,0,0]
  definitedurationbeats:10
   definitetime:beats
   alpha:0.5
   letting:0.25
   leading:1.1
   ```

![](https://github.com/thelightdesigner/ScuffedWalls/blob/1.0/Readme/fuck.jpg)

## Font Image
Example: 
![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Examples/All%20Functions%20in%20Docs/font.png)

Scuffedwalls can use a special image to get the font for text to wall conversion. The image consists of a black or transparent background with the all characters on the same line.

Individual characters must be seperated by at least one vertical line of uninterupted black/transparent pixels.  (i.e just keep the characters apart)

The characters are read in the order 
a b c d e f g h i j k l m n o p q r s t u v w x y z
A B C D E F G H I J K L M N O P Q R S T U V W X Y Z
questionmark period exclamation apostrophe dash quotationmark greaterthan lessthan paranthesisleft paranthesisright curleybraceleft curleybraceright

If you generate and all the text is the wrong character, check your font.png file! there is probably character overlap.
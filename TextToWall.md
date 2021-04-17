# TextToWall

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/geico.png)


We have a beginners tutorial! [see here](https://www.youtube.com/watch?v=g49gfMtzETY)

## Font Image
Scuffedwalls can use a special image to get the font for text to wall conversion. The image consists of a black or transparent background with the all characters on the same line.

example

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Examples/fonts/litefont.png)

![](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Examples/fonts/COMICSANS.png)

individual characters must be seperated by at least one vertical line of uninterupted black/transparent pixels.  (i.e just keep the characters apart)

the characters are read in the order 
a b c d e f g h i j k l m n o p q r s t u v w x y z
A B C D E F G H I J K L M N O P Q R S T U V W X Y Z
questionmark period exclamation apostrophe dash quotationmark greaterthan lessthan paranthesisleft paranthesisright curleybraceleft curleybraceright

if you generate and all the text is the wrong character, check your font.png file! there is probably character overlap.


## Font Model
Scuffedwalls can use a 3d model to create wall text, see here for more information on 3d modeling for wall conversion. Groups of cubes can be assigned as a letter by having a material named one of the letters from the character list below. example: Letter_a, Letter_b...Letter_A... Letter_quotationmark, Letter_apostrophe...

a b c d e f g h i j k l m n o p q r s t u v w x y z
A B C D E F G H I J K L M N O P Q R S T U V W X Y Z
questionmark period exclamation apostrophe dash quotationmark greaterthan lessthan paranthesisleft paranthesisright curleybraceleft curleybraceright

All the letters should be roughly one blender unit tall in a line on the X axis.

[example moment]

```5:TextToWall
   line:oh fuck
   line:oh no
   line:Aero trapped me in beat saber
   path:font.dae
   size:0.1
   thicc:12
   animatedefiniteposition:[0,0,0,0]
   position:[0,5]
  definitedurationbeats:10
   definitetime:beats
   alpha:0.5
   letting:1
   leading:1
   ```

[example]
   





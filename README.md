# ScuffedWalls
A command line tool for making Noodle Extensions 2.0 beat saber maps & modcharts.

This tool does not do the same thing as beatwalls.

Features:
 - Create custom events
 - Import/Combine map objects from other map files
 - Add noodle/chroma data to map objects
 - Work without code
 - 3d model converter
 - Image converter
 - Text converter
 
 Usage:
  - Place the program in the map folder
  - Open the program
  - Input the number of the map file to write to (Will overwrite anything in this map file)
  - Write in the generated \_ScuffedWalls.sw file. 
  - Saving the \_ScuffedWalls.sw file causes the program to write to the map files.

Get Started:

 - Intro and Setup video tutorial by #Rizthesnuggie2634 -> [`right here`](https://youtu.be/RrcQRQfaXAI)
 - Functions + Explanations -> [`here`](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Functions.md)
 - 3d modeling for wall conversion -> [`here`](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Blender%20Project.md)
 - TextToWall -> [`here`](https://github.com/thelightdesigner/ScuffedWalls/blob/main/TextToWall.md)

 Rizthesnuggie's full intro documentation can be found [`here`](https://drive.google.com/drive/folders/1aAUuv8Ycmf2LdSRvKYhfThY2tQhZxFYS?usp=sharing)
 
 
*Windows will probably bother you about this being malware. If you dont trust it, clone the repo and build it yourself.*

If everything doesnt work and your in a country that uses , as the decimal symbol, [`changing regional settings`](https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/regional.png) is a common fix.

## Examples

 - [`Shelter`](https://www.youtube.com/watch?v=a4zSCDYRwAQ)
 - [`Illuminate`](https://www.youtube.com/watch?v=lFL3Gjy15oc&t=1s)
 - [`Homesick`](https://www.youtube.com/watch?v=St3fSqj8SXc)
 - [`Gamecube Intro`](https://www.youtube.com/watch?v=0SVRM0cmUVE)
 - [`Try`](https://www.youtube.com/watch?v=fO4Z6OG5w_I)
 - [`Real or Lie`](https://www.youtube.com/watch?v=59X3Qb78-Es)
 - [`Exosphere`](https://www.youtube.com/watch?v=698L0vSI0no)
 - [`Rare`](https://www.youtube.com/watch?v=fQpDYL0If7U)
 - [`Wait`](https://www.youtube.com/watch?v=FLstEOwle08)
 - [`Giant Enemy Spider`](https://www.youtube.com/watch?v=SntUgEmF9UQ)
 - [`0108ROCKET`](https://www.youtube.com/watch?v=YtHnIqrLW1s)
 - [`Dadadada`](https://www.youtube.com/watch?v=vJlGANqWn2U)
 - [`Scary Garry`](https://www.youtube.com/watch?v=Pw5GfyzEWNU)
 - [`Industry Baby`](https://www.youtube.com/watch?v=vtNGsJnHRD4)
 - [`Last Christmas`](https://www.youtube.com/watch?v=kcKMgOnlyis)
 - [`Crystallized`](https://youtu.be/TnvvoApz4zg)
 - [`Wasted`](https://www.youtube.com/watch?v=TTqqOfxKLEI)
 - `Roth Bartlett - Off the World (wobbleorange)`
 - `Amanda Marshall - Let it Rain (wobbleorange)`

<img src="https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/Try.jpg" alt="Try" width="500"/>

<img src="https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/rocket.jpg" alt="Try" width="500"/>

<img src="https://github.com/thelightdesigner/ScuffedWalls/blob/main/Readme/shelter.jpg" alt="Try" width="500"/>

## For Developers

To create a function, clone the repo and navigate to ScuffedWalls -> Program -> Functions. Create a new .cs file. All classes under the namespace `ScuffedWalls.Functions` that are decorated with the `ScuffedFunction` attribute will be populated as a function. The params string constructor is used to define the name or names of the function. Your class must inherit from `SFunction` which contains an array of parameters, the `InstanceWorkspace`, the `GetParam` method, and the virtual method `Run`. The starting point for your code must be in an override of the virtual method `Run`. If you use a parameter without calling `GetParam` you must mark the parameter as used by setting `WasUsed` to true. `InstanceWorkspace` contains the lists of mapobjects that are present in the workspace that the function was called from.

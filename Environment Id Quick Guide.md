# Environment Enhancement

## How To Get Chroma Logs
- 1 - Beat Saber\UserData\Beat Saber IPA.json
```json
"CondenseModLogs": false,
"CreateModLogs": true,
"HideMessagesForPerformance": false
```
- 2 - Beat Saber\UserData\Chroma.json
```json
"PrintEnvironmentEnhancementDebug": true
```
- 3 - Play A Map
- 4 - Beat Saber\Logs\Chroma\\_latest.log

This will now contain the logs of all map elements in any maps you play

## ID Conversion
The Chroma logs will give you IDs like this
```ruby
[DEBUG @ 16:32:17 | Chroma] BTSEnvironment.[0]Environment.[11]Clouds
```
You only need the part after the Chroma word
```ruby
BTSEnvironment.[0]Environment.[11]Clouds
```
### Notes
The suggested lookup method for Environment Enhancement is **Regex** (I highly recommend doing your own research on Regex.)

The **\\** character is called an escape character and tells code to ignore the intended functionality of the next character

When scripting you escape twice but if you are using ScuffedWalls to edit environment you only escape once.

I highly recommend to use a [REGEX WEBSITE](https://regexr.com/) it will help you check if your Regex statement is correct (since the webiste is pure Regex you only escape once)

<hr>

To convert a normal Chroma ID into Regex you must first escape all `.` (periods)

```ruby
BTSEnvironment\.[0]Environment\.[11]Clouds
```
Then escape all square brackets

```ruby
BTSEnvironment\.\[0\]Environment\.\[11\]Clouds
```

If the Regex statement that you use is being highlighted in the website it means that it works! good job you did Regex!
there is much more with Regex and I suggest [THIS TUTORIAL](https://youtu.be/sa-TUpSx1JA) that explains how to do Regex

This can now be used by ScuffedWalls or custom scripts

ScuffedWalls:
```js
0:Environment
  Id: IDSTATEMENT
  LookUpMethod:Regex/Contains/Exact
  Rotation:[90,0,0]
  //Here you can put more custom data!
```
JavaScript:
```js
_environment.push(
{
_id: "IDSTATEMENT",
_lookupMethod: "Regex/Contains/Exact",
_rotation:[x,y,z]
//Here you can put more custom data!
}
)
```
## [`Back To Functions`](Functions.md#Environment)

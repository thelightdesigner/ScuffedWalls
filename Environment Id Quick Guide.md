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

This Will Now Contain The Logs Of All Map Elements In Any Maps You Play

## id Conversion
The Chroma Logs Will Give You Lots Of Ids like this
```ruby
BTSEnvironment.[0]Environment.[11]Clouds
```
### Notes
The reccomended lookup method for Environment Enhancement is **Regex** (I Highly recommend doing your own research on Regex.)

The **\\** character is called an escape character and tells code to ignore the intended functionality of the next character

When scripting you escape twice but if you are using scuffed walls to edit environment you only escape once.

I highly recommend to use a [REGEX WEBSITE](https://regexr.com/) it will help you check if your regex statement is correct(since the webiste is pure regex you only escape once)
<hr>
To Convert A Normal Chroma Id Into Regex You Must First Escape All . (periods)

```ruby
BTSEnvironment\.[0]Environment\.[11]Clouds
```
Then Escape All Square Brackets

```ruby
BTSEnvironment\.\[0\]Environment\.\[11\]Clouds
```

If the regex statement that you use is being highlighted in the website it means that it works! good job you did regex!
there is much more with regex and I recommend [THIS TUTORIAL](https://youtu.be/sa-TUpSx1JA) that explains how to do regex

This Can Now Be Used by Scuffed Walls/scripts

Scuffed walls:
```js
0:Environment
  Id: IDSTATEMENT
  LookUpMethod:Regex/Contains/Exact
  Rotation:[90,0,0]
  //Here you can put more custom data!
```
Script:
```js
_environment.push(
{
_id: "IDSTATEMENT",
_lookupMethod: "Regex/Contains/Exact",
_rotation:[x,y,z]
//Here you can put more custom data!
)
```
## [`Back To Functions`](Functions.md#Environment)

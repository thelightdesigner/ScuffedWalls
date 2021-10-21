# Environment Enhancement

## How To Get Chroma Logs
- 1 - Beat Saber\UserData\Beat Saber IPA.json
```json
"CondenseModLogs": false,
"CreateModLogs": true,
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
<hr>
To Convert A Normal Chroma Id Into Regex You Must First Escape All . (periods)

```ruby
BTSEnvironment\.[0]Environment\.[11]Clouds
```
Then Escape All Square Brackets

```ruby
BTSEnvironment\.\[0\]Environment\.\[11\]Clouds
```
Add **^** and **$** To Define The Start And End Of The ID

```ruby
^BTSEnvironment\.\[0\]Environment\.\[11\]Clouds$
```
and Finally Replace all non-specific numbers with a number wildcard **d\*** (Don't forget to include an escape character)

```ruby
^BTSEnvironment\.\[\d*\]Environment\.\[\d*\]Clouds$
```
This Can Now Be Used by Scuffed Walls

```ruby
0:Environment
  Id:^BTSEnvironment\.\[\d*\]Environment\.\[\d*\]Clouds$
  LookUpMethod:Regex
  Rotation:[90,0,0]
```
## [`Back To Functions`](Functions.md#Environment)

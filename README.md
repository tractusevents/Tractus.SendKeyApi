# Tractus.SendKeyApi
A basic ASP.NET Core web API to send a keystroke to Windows.

## How it Was Made - Video
https://www.youtube.com/watch?v=LYhNc0x7oF0

This version can send ALT+1 thru ALT+9 by POSTing localhost:5000/keyboard.

Example POST request:
```
{"numberKey": 4,"lAlt": true}
```

# DreamKing

This is a mod for the game Hollow Knight

## ToDo

* Space in path underneath thorns maybe too small
* Layering issues on every single thorn
* Entrance to small cave with credits
* HornetPickupPoints very broken with nail arts

## Credits

look at the credits in Credits.md

## Ideas

look at the ideas in Ideas.md

## How to Setup

1. Download the source code or clone the repository

2. Open the project in visual studio 2017 or 2019 (i used 2019)

3. Now you could try to build it (99% not possible)

4. Right click "References" and go to "Add References"

5. On the left side, select the last item

6. On the bottom, click the button next to OK and select the following files

    1. All the files here should be in the directory ``%Hollow Knight Installation Directory%\hollow_knight_Data\Managed\``

```
Assembly-CSharp.dll
System.dll
UnityEngine.dll
UnityEngine.CoreModule.dll
UnityEngine.UI.dll
```

7. Open the project properties

8. Go to Build Events

9. In the text area "Commandline post build" (or similar), remove the 5th line, or change the path at the end to your corresponding hollow knight installation path

10. Now you should be able to build the project (in the debug configuration)

<h1>CeLeRy</h1>
<h4>A (Work-In-Progress) Lightweight .NET Universal Mod Loader</h4>
<img src="https://github.com/user-attachments/assets/b01cb42c-8929-4ada-b71a-a4128ce38d55" width="200">

<h1>How It Works?</h1>
As shown below, You take the contents of a release zip and extract them into your game directory.
The game directory is whatever folder contains the main .exe of your game.

version.dll gets automatically loaded by the game .exe and starts the process. It loads
clrhost.dll (after loading its dependency, Ijwhost.dll). clrhost.dll is a C++/CLR project
configured to run .NET 8.0 . This is what allows it to run CeLeRy.dll which kicks off the
mod loading.

Also worth noting that the console is opened by clrhost.dll, right before loading CeLeRy.dll .
Future plans include making the console configurable. Along with this, plans for a better log
system that saves logs to a file. But for now console window will remain open for debugging.

Each mod gets its own thread for now. May change in the future.

Winform mods CAN work. Easiest way is to just make a Winform project and use the dll. Just ignore the .exe after compile.
Changing to Class Library may cause issues with Winform.

Feel free to make a Pull Request, Or submit an Issue Ticket for Feature Requests, Issues, and anything else!
My Discord is 'Glatrix#0001' or just 'glatrix'. Reach out there for questions. Might make a server later on.

```
ExampleGame
│   ExampleGame.exe
│   version.dll                      (CeLeRy Proxy Loader)
├───ExampleGameData
│   │   (Blah Blah)
│
├───CeLeRy
│   │   CeLeRy.dll                   (.NET CeLeRy Core DLL)
│   │   clrhost.dll                  (CeLeRy Proxy-Core Middleman) ie. [version.dll] -> [Ijwhost.dll + clrhost.dll] -> [CeLeRy.dll])
│   │   clrhost.runtimeconfig.json   (Required because clrhost.dll is a C++/CLR dll.
│   │   Ijwhost.dll                  (clrhost.dll dependency)
│   │
│   └───runtime
│           (Any System and Microsoft Dependencies)
└───mods
        (Any DLLs here will get loaded, checked for CeLeRyMod Attribute on any Class, and Execute accordingly.
        Hello World.txt
```

<h1>CeLeRy</h1>

<p>
A (Work-In-Progress) Lightweight .NET Universal Mod Loader     
</p>

<p>  
<img src="https://github.com/user-attachments/assets/26fc7d5b-9255-4145-84fe-1ea0ff9b0597" width="200">   
</p>

- Only Tested On Windows 10
- No ARM Support
- x86/32-bit games get x32. 
- x64/64-bit games get x64
- Downloads currently contain FULL .NET Deps. Plan to remove soon.
  
| Current Releases (v0.1) |
| ---------------- |
| [x32 (STILL WIP)](https://github.com/Glatrix/CeLeRy/releases/latest/download/CeLeRy32.zip) |
| [x64 (STILL WIP)](https://github.com/Glatrix/CeLeRy/releases/latest/download/CeLeRy64.zip) |

| Works With | Status |
| ------------- | - |
| .NET 8.0 | [✅] |
| Winforms | [✅] |
| DLL Imports | [✅] |
| Unsafe Context | [✅] (Still Unsafe) |
| Nuget Packages | [❔] (Not Yet Tested) |
| .NET 8.x | [❔] (Not Yet Tested) |
| Native AOT (and/or) Single-File | [❔] (Not Planned Feature) |

![image](https://github.com/user-attachments/assets/62523817-2fee-4aeb-b397-948238a99620)


| To-Do | Status |
| ------------- | - |
| Better Logging | Working On |
| Nuget Package Handling | In Research |
| Better / More Examples | Backlog |
| Docs, Webpage, and FAQ | Backlog |
| Configuration Options | Backlog |
| Plugins & More Mod Options | Backlog |
| .NET 8.x | Backlog |

** Note About To-Dos and Roadmap
```
All of these planned features and TO-DOs still center around keeping
this project lightweight. Meaning The code base should be kept as small
and easy to read as possible. 

Plugins will hopfully be the main form
of expansion on the base mod loader. 

Plugins are currently planned to load
AFTER System and Microsoft DLLs but BEFORE mods are loaded allowing for
ease of use. 

Plugins ARE NOT currently planned to be the same as, or include
nuget packages. I'm still looking at how to impliment nuget packages
in such a way that can be easily debugged, changed, etc.
```

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
My Discord is `Glatrix#0001` or just `glatrix`. Reach out there for questions. Might make a server later on.

```
ExampleGame
│   ExampleGame.exe
│   version.dll                      (CeLeRy Proxy Loader)
├───ExampleGameData
│   │   (Blah Blah)
├───CeLeRy
│   │   CeLeRy.dll                   (.NET CeLeRy Core DLL)
│   │   clrhost.dll                  (CeLeRy Proxy-Core Middleman) ie. [version.dll] -> [Ijwhost.dll + clrhost.dll] -> [CeLeRy.dll])
│   │   clrhost.runtimeconfig.json   (Required because clrhost.dll is a C++/CLR dll.
│   │   Ijwhost.dll                  (clrhost.dll dependency)
│   │   latest_logs.txt              (Logs Now Saved)
└───mods
        (Any DLLs here will get loaded, checked for CeLeRyMod Attribute on any Class, and Execute accordingly.
        Hello World.txt
```


<h1>Thanks!</h1>
Thanks to Bitcrackers for https://github.com/BitCrackers/version-proxy

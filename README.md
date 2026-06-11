# Flamingo
command-line zero-trust game launcher for Microsoft Windows operating system written in C# .NET 10. 
# How it works?
Launch a game in low integrity mode. The game get launched as low integrity process which is very restricted, it cannot write to the registry and it’s limited from writing to most locations in the current user’s profile.
# instruction
Build the project, put the solution in the same folder as the game to sandbox.
# usage
ZTALauncher.exe [program.exe] [mode] [optional verbose flag]
# example
ZTALauncher.exe "notepad.exe" "vanilla" "verbose"

# note
ZTALauncher will generate a base 64 signature of the executable and print it in the console

# Download the latest release: Cyanide
https://github.com/PigeonWilson/Flamingo/releases/
# Localization
More language can be added to the embedded Resources.rsx file. The system handle local culture and load the appropriate string from resources. Currently support English. Don't forget to add the iso language code to this file https://github.com/PigeonWilson/Flamingo/blob/main/ZTALauncher/Enums.cs

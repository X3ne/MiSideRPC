@echo off
set buildPath=bin\Release\net6.0\
set zipName=MiSideRPC.zip
set zipPath=%buildPath%%zipName%

mkdir "%buildPath%BepInEx\plugins\MiSideRPC\Assets"

copy "%buildPath%MiSideRPC.dll" "%buildPath%BepInEx\plugins\MiSideRPC\"
copy "%buildPath%MiSideRPC.pdb" "%buildPath%BepInEx\plugins\MiSideRPC\"
copy "Assets\game_hints.json" "%buildPath%BepInEx\plugins\MiSideRPC\Assets\"
copy "Dependencies\discord_game_sdk.dll" "%buildPath%BepInEx\plugins\MiSideRPC\"
copy "Dependencies\Newtonsoft.Json.dll" "%buildPath%BepInEx\plugins\MiSideRPC\"
copy "Dependencies\Newtonsoft.Json.pdb" "%buildPath%BepInEx\plugins\MiSideRPC\"

powershell -ExecutionPolicy Bypass -Command "& { Compress-Archive -Path \"%buildPath%BepInEx\*\" -DestinationPath \"%zipPath%\" -Force }"

echo ZIP file created at: %zipPath%

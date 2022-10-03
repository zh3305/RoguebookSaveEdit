rem  正常编译
dotnet publish -c release  
move RoguebookSaveEdit\bin\release\net5.0-windows\publish\lib\TextAsset RoguebookSaveEdit\bin\release\net5.0-windows\publish\TextAsset
rem  包含依赖框架
dotnet publish -r win-x64 -c release --self-contained 
move RoguebookSaveEdit\bin\release\net5.0-windows\win-x64\publish\lib\TextAsset RoguebookSaveEdit\bin\release\net5.0-windows\win-x64\publish\TextAsset
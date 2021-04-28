Set Parsing Test Project

## Системные требования:  
- Одна из поддерживаемых платформ: 
    - Wndows 7+ (Any CPU)
    - Linux (Arm32 | Arm64 | x64 | x64 Alpine)
    - macOS (x64)
- .NET Core 3.1.x SDK (https://dotnet.microsoft.com/download/dotnet/3.1)

---    
Выполните следующую комманду чтобы убедиться, что указанные пакеты установленны корректно.
```bash
> dotnet --list-sdks
```

---
## Инструкции по запуску
Для запуска тестов выполните:

```bash
> cd ./src
> dotnet tool install -g dotnet-reportgenerator-globaltool
> dotnet test -l:"console;verbosity=normal" /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
> reportgenerator.exe "-reports:SetParsing.Tests/coverage.cobertura.xml" "-targetdir:report" -reporttypes:Html
> ./report/index.html
```

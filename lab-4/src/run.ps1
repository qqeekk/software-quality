dotnet test -l:"console;verbosity=normal" /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
#reportgenerator.exe "-reports:SetCalculations.Tests/coverage.cobertura.xml" "-targetdir:report" -reporttypes:Html
reportgenerator.exe "-reports:../../**/coverage.cobertura.xml" "-targetdir:report" -reporttypes:Html
./report/index.html

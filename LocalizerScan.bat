@echo off
REM Finds the phrases the code asks to have localized, Localize("..."), adds the new ones to FinderSeeker\Translations\Translations.db,
REM and translates the empty cells. Needs the Einkr key in C:\EinkrKey.txt.
dotnet run --project "%~dp0LocalizerScan" -c Release -- "%~dp0." 8

pause

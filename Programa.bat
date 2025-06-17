@echo off

REM Paleidžiamas Master su argumentais agent1 ir agent2
start "Master" cmd /k "Master\bin\Debug\net8.0\Master.exe agent1 agent2"

REM Paleidžiamas ScannerA su savo katalogo keliu
start "ScannerA" cmd /k "ScannerA\bin\Debug\net8.0\ScannerA.exe C:\Users\Dovmar\Desktop\csharp\ScannerA\TextFiles"

REM Paleidžiamas ScannerB su savo katalogo keliu
start "ScannerB" cmd /k "ScannerB\bin\Debug\net8.0\ScannerB.exe C:\Users\Dovmar\Desktop\csharp\ScannerB\TextFiles"


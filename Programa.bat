@echo off


start "Master" cmd /k "Master\bin\Debug\net8.0\Master.exe agent1 agent2"

start "ScannerA" cmd /k "ScannerA\bin\Debug\net8.0\ScannerA.exe C:\Users\Dovmar\Desktop\csharp\ScannerA\TxtFailai"

start "ScannerB" cmd /k "ScannerB\bin\Debug\net8.0\ScannerB.exe C:\Users\Dovmar\Desktop\csharp\ScannerB\TxtFailai"


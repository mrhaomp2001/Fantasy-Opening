@echo off
title Startup

for /f "tokens=2 delims==" %%a in ('wmic OS Get localdatetime /value') do set "dt=%%a"
set "YY=%dt:~2,2%" & set "YYYY=%dt:~0,4%" & set "MM=%dt:~4,2%" & set "DD=%dt:~6,2%"
set "HH=%dt:~8,2%" & set "Min=%dt:~10,2%" & set "Sec=%dt:~12,2%"

set "fullstamp=%HH%:%Min%:%Sec% %DD%/%MM%/%YYYY%"

(
    echo:
    echo # [ %fullstamp% ]
    echo:
    echo ^<details^>^<summary^> ^<p align="center"^>^<h2^>Chi tiết công việc:^</h2^>^</p^> ^</summary^>
    echo:
    echo ### Kết quả hôm nay:
    echo:
    echo ### Công việc: 
    echo:
    echo ^</details^>
    echo:
) > temp.txt

if exist readme.md (
    type readme.md >> temp.txt
)

move /Y temp.txt readme.md >nul

start readme.md

:: Start auto push

echo:
echo Is your internet available, my dear?
set /p input="(Y/n)"

if /i "%input%"=="y" (
    git add .
    git commit -m "auto push %fullstamp%"
    git push origin master
) else (
    goto :exit
)

:exit
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
    echo ^<details^>
    echo ^<summary^>^<h2^>Chi tiết công việc:^</h2^> ^</summary^>
    echo:
    echo ### Kết quả hôm nay:
    echo:
    echo ### Công việc trong kế hoạch: 
    echo:
    echo ^</details^>
    echo:
) > temp.txt

if exist readme.md (
    type readme.md >> temp.txt
)

move /Y temp.txt readme.md >nul

start "" "readme.md"
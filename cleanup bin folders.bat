set fldr=%my%\LIBRARIES\VS\VI\CORE\VI
call %my%\SCRIPTS\bat\misc\RemoveDir.bat %fldr% debug
pause
call %my%\SCRIPTS\bat\misc\RemoveDir.bat %fldr% release
pause
set fldr=
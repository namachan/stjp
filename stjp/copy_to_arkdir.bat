setlocal
set ARK_DIR_PATH=P:\SteamLibrary\steamapps\common\ARK\
set ARK_DST_LNG=ja
copy /B /V /Y "%~dp0ShooterGame.json" "%ARK_DIR_PATH%ShooterGame\Content\Localization\Game\%ARK_DST_LNG%\ShooterGame.archive"
endlocal
WinWait($CMDLINE[1] , $CMDLINE[2] , 3 )
;WinActive($CMDLINE[1])
WinSetState($CMDLINE[1], "", @SW_RESTORE);
ControlSetText($CMDLINE[1], "", "[CLASS:Edit; INSTANCE:1]", $CMDLINE[2] )
ControlClick($CMDLINE[1], "", "[CLASS:Button; INSTANCE:1]")
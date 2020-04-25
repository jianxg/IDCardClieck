@echo 开始注册
copy gsdll32.dll%windir%\system32\
regsvr32 %windir%\system32\GSDll.dll /s
@echo GSDll.dll注册成功
@pause
@echo ��ʼע��
copy gsdll32.dll%windir%\system32\
regsvr32 %windir%\system32\GSDll.dll /s
@echo GSDll.dllע��ɹ�
@pause
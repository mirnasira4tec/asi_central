SETLOCAL
call "%VS110COMNTOOLS%\vsvars32.bat"
msbuild asi_central.xml /t:BuildSolutions

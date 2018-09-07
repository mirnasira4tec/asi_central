SETLOCAL
call "%VS2017COMNTOOLS%\VsDevCmd.bat"
msbuild asi_central.xml /property:Local="true" /property:CREATEZIP=true /property:JOB_NAME=ASICentral /property:BUILD_NUMBER=001 /property:ExtensionTasksPath="D:\\Program Files\\MSBuild\\ExtensionPack\\4.0\\" /property:BUILDTARGETDIR=..\Packaging /t:Package

SETLOCAL
call "%VS110COMNTOOLS%\vsvars32.bat"
msbuild asi_central.xml /property:Local="true" /property:NuGetExe=".nuget\\NuGet.exe" /property:CREATEZIP=true /property:JOB_NAME=ASICentral /property:BUILD_NUMBER=001 /property:ExtensionTasksPath="C:\\Program Files\\MSBuild\\ExtensionPack\\4.0\\" /property:BUILDTARGETDIR=..\Packaging /t:Package

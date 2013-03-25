SETLOCAL
call "%VS110COMNTOOLS%\vsvars32.bat"
msbuild asi_central.xml /property:NuGetExe="D:\\Data\\Packaging\\NuGet.exe" /property:CREATEZIP=true /property:JOB_NAME=ASICentral /property:BUILD_NUMBER=001 /property:ExtensionTasksPath="C:\\Program Files\\MSBuild\\ExtensionPack\\4.0\\" /property:BUILDTARGETDIR=D:\Data\Packaging\ASIAdmin /t:Package


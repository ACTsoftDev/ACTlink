REM Download and install https://github.com/SonarSource-VisualStudio/sonar-scanner-msbuild/releases/download/2.2/sonar-scanner-msbuild-2.2.0.24.zip
REM Change the path of the SonarQube.Scanner.MSBuild.exe below to the path where the above zip file has been unzipped.
REM Once the below commands are executed, the results will be uploaded to the SonarQube Server 
REM the URL Location for this project is  http://tools.innominds.com/sonar/dashboard/index/com.actchargers.actlink
C:\Dev\Installs\sonar-scanner-msbuild-2.2.0.24\SonarQube.Scanner.MSBuild.exe begin /k:"com.actchargers.actlink" /n:"ACT-Link" /v:"1.0.0"
"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" /t:Rebuild
C:\Dev\Installs\sonar-scanner-msbuild-2.2.0.24\SonarQube.Scanner.MSBuild.exe end
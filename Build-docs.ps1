function Find-OnPath
{
    [CmdletBinding()]
    Param( [Parameter(Mandatory=$True,Position=0)][string]$exeName)
    $path = where.exe $exeName 2>$null
    if(!$path)
    {
        Write-Verbose "'$exeName' not found on PATH"
    }
    else
    {
        Write-Verbose "Found: '$path'"
    }
    return $path
}

# invokes NuGet.exe, handles downloading it to the script root if it isn't already on the path
function Invoke-NuGet
{
    $NuGetPath = Find-OnPath NuGet.exe -ErrorAction Continue
    if( !$NuGetPath )
    {
        $nugetToolsPath = "$PSScriptRoot\Tools\NuGet.exe"
        if( !(Test-Path $nugetToolsPath))
        {
            # Download it from official NuGet release location
            Write-Verbose "Downloading Nuget.exe to $nugetToolsPath"
            Invoke-WebRequest -UseBasicParsing -Uri https://dist.NuGet.org/win-x86-commandline/latest/NuGet.exe -OutFile $nugetToolsPath
            Write-Verbose "Adding tools folder to path fro Nuget.exe"
        }
        $env:Path = "$env:Path;$(Join-Path $PSSCriptRoot 'Tools')"
    }
    Write-Information "$NugetPath"
    Write-Information "NuGet $args"
    NuGet.exe $args
    $err = $LASTEXITCODE
    if($err -ne 0)
    {
        throw "Error running NuGet: $err"
    }
}

function Get-CmdEnvironment ($cmd, $Arguments)
{
    $retVal = @{}
    Write-Verbose "Running [`"$cmd`" $Arguments >nul & set] to get environment variables"
    $envOut =  cmd /c "`"$cmd`" $Arguments >nul & set"
    foreach( $line in $envOut )
    {
        $name, $value = $line.split('=');
        $retVal.Add($name, $value)
    }
    return $retVal
}

function Merge-Environment( [hashtable]$OtherEnv, [string[]]$IgnoreNames )
{
<#
.SYNOPSIS
    Merges the name value pairs of a hash table into the current environment

.PARAMETER OtherEnv
    Hash table containing name value pairs to add to the environment

.PARAMETER IgnoreNames
    Names of properties in OtherEnv to ignore
.NOTES
    Standard system variables are always ignored and are blocked from merging
#>
    $SystemVars = @('COMPUTERNAME',
                    'USERPROFILE',
                    'HOMEPATH',
                    'LOCALAPPDATA',
                    'PSModulePath',
                    'PROCESSOR_ARCHITECTURE',
                    'CommonProgramFiles(x86)',
                    'ProgramFiles(x86)',
                    'PROCESSOR_LEVEL',
                    'LOGONSERVER',
                    'SystemRoot',
                    'SESSIONNAME',
                    'ALLUSERSPROFILE',
                    'PUBLIC',
                    'APPDATA',
                    'PROCESSOR_REVISION',
                    'USERNAME',
                    'CommonProgramW6432',
                    'CommonProgramFiles',
                    'OS',
                    'USERDOMAIN_ROAMINGPROFILE',
                    'PROCESSOR_IDENTIFIER',
                    'ComSpec',
                    'SystemDrive',
                    'ProgramFiles',
                    'NUMBER_OF_PROCESSORS',
                    'ProgramData',
                    'ProgramW6432',
                    'windir',
                    'USERDOMAIN'
                   )
    $IgnoreNames += $SystemVars
    $otherEnv.GetEnumerator() | ?{ !($ignoreNames -icontains $_.Name) } | %{ Set-Item -Path "env:$($_.Name)" -value $_.Value; Write-Verbose "env:$($_.Name)=$($_.Value)" }
}

function Find-VSInstance([switch]$PreRelease)
{
    Install-Module VSSetup -Scope CurrentUser | Out-Null
    Get-VSSetupInstance -Prerelease:$PreRelease |
        Select-VSSetupInstance -Require 'Microsoft.Component.MSBuild', 'Microsoft.VisualStudio.Component.VC.Tools.x86.x64', 'Microsoft.VisualStudio.Component.VC.CMake.Project' |
        select -First 1
}

function Initialize-VCVars($vsInstance = (Find-VSInstance))
{
    if($vsInstance)
    {
        $vcEnv = Get-CmdEnvironment (Join-Path $vsInstance.InstallationPath 'VC\Auxiliary\Build\vcvarsall.bat') 'x86_amd64'
        Merge-Environment $vcEnv @('Prompt')
    }
    else
    {
        Write-Error "VisualStudio instance not found"
    }
}

if( !( Test-Path -PathType Container tools ) )
{
    md tools | out-null
}

# Unfortunately, just getting docfx to succesfully build is a lot of undocumented
# workarounds for alot of bugs from it's use of msbuild and Roslyn, dotnet and VS
# build conflicts. It is beyond me why a doc generator tool needs to build a project
# let alone why it needs to include the Roslyn Compiler too... [sigh]...
#
# This script will manually install the packages needed to run docfx and the memperpage
# plugin. Then creates a VCVARS environment to run docfx to work around the dependency
# and conflict problems with docfx.
Initialize-VCVars

Invoke-NuGet install docfx.console -ExcludeVersion -OutputDirectory tools
$env:Path = "$env:Path;$(Join-Path $PSScriptRoot 'tools\docfx.console\tools')"
Invoke-NuGet install memberpage -ExcludeVersion -OutputDirectory tools
docfx -t statictoc,tools\memberpage\content,templates\Ubiquity
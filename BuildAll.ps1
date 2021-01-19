Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases,
    [switch]$NoClean
)

. .\buildutils.ps1

# Main Script entry point -----------
pushd $PSScriptRoot
$oldPath = $env:Path
$ErrorActionPreference = "Stop"
$InformationPreference = "Continue"
try
{
    dotnet --list-sdks
    $msbuild = Find-MSBuild -AllowVsPrereleases:$AllowVsPreReleases
    if( !$msbuild )
    {
        throw "MSBuild not found"
    }

    if( !$msbuild.FoundOnPath )
    {
        $env:Path = "$env:Path;$($msbuild.BinPath)"
    }

    # setup standard MSBuild logging for this build
    $msbuildLoggerArgs = @('/clp:Verbosity=Minimal')

    if (Test-Path "C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll")
    {
        $msbuildLoggerArgs = $msbuildLoggerArgs + @("/logger:`"C:\Program Files\AppVeyor\BuildAgent\Appveyor.MSBuildLogger.dll`"")
    }

    $buildPaths = Get-BuildPaths $PSScriptRoot

    Write-Information "Build Paths:"
    Write-Information ($buildPaths | Format-Table | Out-String)

    if( (Test-Path -PathType Container $buildPaths.BuildOutputPath) -and !$NoClean )
    {
        Write-Information "Cleaning output folder from previous builds"
        rd -Recurse -Force -Path $buildPaths.BuildOutputPath
    }

    md $buildPaths.NuGetOutputPath -ErrorAction SilentlyContinue| Out-Null

    $BuildInfo = Get-BuildInformation $buildPaths
    if($env:APPVEYOR)
    {
        Update-AppVeyorBuild -Version $BuildInfo.FullBuildNumber
    }

    $packProperties = @{ version=$($BuildInfo.PackageVersion)
                         llvmversion=$($BuildInfo.LlvmVersion)
                         buildbinoutput=(normalize-path (Join-path $($buildPaths.BuildOutputPath) 'bin'))
                         configuration=$Configuration
                       }

    $msBuildProperties = @{ Configuration = $Configuration
                            FullBuildNumber = $BuildInfo.FullBuildNumber
                            PackageVersion = $BuildInfo.PackageVersion
                            FileVersionMajor = $BuildInfo.FileVersionMajor
                            FileVersionMinor = $BuildInfo.FileVersionMinor
                            FileVersionBuild = $BuildInfo.FileVersionBuild
                            FileVersionRevision = $BuildInfo.FileVersionRevision
                            FileVersion = $BuildInfo.FileVersion
                            LlvmVersion = $BuildInfo.LlvmVersion
                          }

    Write-Information "Build Parameters:"
    Write-Information ($BuildInfo | Format-Table | Out-String)

    # clone docs output location so it is available as a destination for the rest of the build
    if( !(Test-Path (Join-Path $buildPaths.DocsOutput '.git') -PathType Container))
    {
        Write-Information "Cloning Docs repository"
        pushd BuildOutput -ErrorAction Stop
        try
        {
            git clone https://github.com/UbiquityDotNET/Argument.Validators.git -b gh-pages docs -q

            # purge all current files so that they don't remain if the newly generated content doesn't need it.
            Get-ChildItem * -exclude .git | Remove-Item -recurse -force
        }
        finally
        {
            popd
        }
    }

    Write-Information "Restoring NuGet Packages"
    Invoke-MSBuild -Targets Restore -Project src\Ubiquity.ArgValidators.sln -Properties $msBuildProperties -LoggerArgs $msbuildLoggerArgs ($msbuildLoggerArgs + @("/bl:Ubiquity.ArgValidators-restore.binlog") )

    Write-Information "Building Solution"
    Invoke-MSBuild -Targets Build -Project src\Ubiquity.ArgValidators.sln -Properties $msBuildProperties -LoggerArgs $msbuildLoggerArgs ($msbuildLoggerArgs + @("/bl:Ubiquity.ArgValidators-build.binlog") )

    if( $env:APPVEYOR_PULL_REQUEST_NUMBER )
    {
        foreach( $item in Get-ChildItem *.binlog )
        {
            Push-AppveyorArtifact $item.FullName
        }
    }
}
finally
{
    popd
    $env:Path = $oldPath
}

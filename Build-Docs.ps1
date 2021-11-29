<#
.SYNOPSIS
    Builds the docs for this repository

.PARAMETER Configuration
    This sets the build configuration to use, default is "Release" though for inner loop development this may be set to "Debug"

.PARAMETER AllowVsPreReleases
    Switch to enable use of Visual Studio Pre-Release versions. This is NEVER enabled for official production builds, however it is
    useful when adding support for new versions during the pre-release stages.

.PARAMETER FullInit
    Performs a full initialization. A full initialization includes forcing a re-capture of the time stamp for local builds
    as well as writes details of the initialization to the information and verbose streams.

.PARAMETER NoClone
    Skip cloning of the docs repository. Useful for inner loop development where you only do the clone once when iterating on
    doc updates.
#>
Param(
    [string]$Configuration="Release",
    [switch]$AllowVsPreReleases,
    [switch]$FullInit,
    [switch]$NoClone
)

Push-Location $PSScriptRoot
$oldPath = $env:Path
try
{
    . .\repo-buildutils.ps1
    $buildInfo = Initialize-BuildEnvironment -FullInit:$FullInit -AllowVsPreReleases:$AllowVsPreReleases

    $msBuildProperties = @{
        Configuration = $Configuration
    }
    $msBuildPropertyList = ConvertTo-PropertyList $msBuildProperties

    $docsOutputPath = $buildInfo['DocsOutputPath']

    # Clone docs output location so it is available as a destination for the Generated docs content
    # and the versioned docs links can function correctly for locally generated docs
    if(!$NoClone -and !(Test-Path (Join-Path $docsOutputPath '.git') -PathType Container))
    {
        if(Test-Path -PathType Container $docsOutputPath)
        {
            Remove-Item -Path $docsOutputPath -Recurse -Force
        }

        Write-Information "Cloning Docs repository"
        git clone https://github.com/UbiquityDotNET/Argument.Validators.git -b gh-pages $docsOutputPath -q
    }

    # remove all contents from docs generation to ensure clean generated docs for this release
    if(Test-Path -PathType Container $docsOutputPath)
    {
        Remove-Item -Path $docsOutputPath -Recurse -Force
    }

    dotnet build 'docfx\Docfx.csproj' -p:$msBuildPropertyList
}
catch
{
    # everything from the official docs to the various articles in the blog-sphere says this isn't needed
    # and in fact it is redundant - They're all WRONG! By re-throwing the exception the original location
    # information is retained and the error reported will include the correct source file and line number
    # data for the error. Without this, only the error message is retained and the location information is
    # Line 1, Column 1, of the outer most script file, which is, of course, completely useless.
    throw
}
finally
{
    Push-Location
    $env:Path = $oldPath
}

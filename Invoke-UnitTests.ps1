. .\buildutils.ps1

# Main Script entry point -----------
if( $env:APPVEYOR )
{
    $loggerArgs = '/logger:Appveyor'
}

$testsFailed = $false
$vsInstance = Find-VSInstance
$vstest = [System.IO.Path]::Combine($vsInstance.InstallationPath, 'Common7','IDE','CommonExtensions','Microsoft','TestWindow','vstest.console.exe')

<#
TODO: Insert call(s) to run tests
#>

if($testsFailed)
{
    Write-Error "One or more tests failed - Build should fail"
}

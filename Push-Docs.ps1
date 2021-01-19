if($env:APPVEYOR_PULL_REQUEST_NUMBER -or ($env:build_reason -ieq ""PullRequest") )
{
    return;
}
pushd .\BuildOutput\docs -ErrorAction Stop
try
{
    # Docs must only be updated from a build with the official repository as the default remote.
    # This ensures that the links to source in the generated docs will have the correct URLs
    # (e.g. docs pushed to the official repository MUST not have links to source in some private fork)
    $remoteUrl = git ls-remote --get-url
    if($remoteUrl -ne "https://github.com/UbiquityDotNET/Argument.Validators.git")
    {
        throw "Pushing docs is only allowed when the origin remote is the official source release. Current remote: $remoteUrl"
    }

    if($env:CI)
    {
        git config --global credential.helper store
        Add-Content "$env:USERPROFILE\.git-credentials" "https://$($env:docspush_access_token):x-oauth-basic@github.com`n"
        git config --global user.email "$env:docspush_email"
        git config --global user.name "$env:docspush_username"
    }

    Write-Information "Adding files to git"
    git add * | Out-File -Append docs-commit.log

    Write-Information "Committing changes to git"
    git commit -m "CI Docs Update" | Out-File -Append docs-commit.log

    Write-Information "pushing changes to git"
    git push -q
}
finally
{
    popd
}

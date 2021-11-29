<#
.SYNOPSIS
    Publishes the current release as a new branch to the upstream repository

.DESCRIPTION
    Generally, this function will finalize the changes for the release and create a new "merge-back"
    branch to manage any conflicts to prevent commits "AFTER" the tag is applied to the origin repository.
    After this completes it is still required to create a PR for the resulting branch to the origin's "develop"
    branch to merge any changes in this branch - including the release tag.

    Completing the PR with a release tag should trigger the start the official build via a GitHub action
    or other such automated build processes. These, normally, also include publication of the resulting
    binaries as appropriate. This function only pushes the tag, the rest is up to the back-end configuration
    of the repository.

.NOTE
    For the gory details of this process see: https://www.endoflineblog.com/implementing-oneflow-on-github-bitbucket-and-gitlab
#>

Param([Parameter(Mandatory=$True)]$commit)
$repoRoot = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, ".."))
. (join-path $repoRoot repo-buildutils.ps1)
$buildInfo = Initialize-BuildEnvironment

# create script scoped alias for git that throws a PowerShell exception if the command fails
Set-Alias git Invoke-git -scope Script -option Private

# pushing the tag to develop branch on the official repository triggers the official build and release of the Nuget Packages
$tagName = Get-BuildVersionTag $buildInfo
git co develop
git pull upstream develop
git push
git tag -a $tagName $commit -m"Release $tagName"
git push upstream $tagName

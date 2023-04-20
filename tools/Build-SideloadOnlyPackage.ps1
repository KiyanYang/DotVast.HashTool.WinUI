[CmdletBinding()]
param (
    [ValidateSet('Debug', 'Release')]
    [string]
    $Configuration = 'Release',

    [ValidateSet('x64', 'arm64')]
    [string]
    $Platform = 'x64',

    [string]
    $AppxPackageDir = 'AppxPackage'
)

Push-Location (Split-Path $PSScriptRoot -Parent)

# 创建临时目录
$tmpAppxPackageDir = New-Item -Path (New-Guid).Guid -ItemType Directory

# Restore
# https://learn.microsoft.com/en-us/nuget/reference/msbuild-targets#restoring-packagereference-and-packagesconfig-projects-with-msbuild
& msbuild '.\src\DotVast.HashTool.WinUI.sln' `
    -t:Restore `
    -p:RestorePackagesConfig=true `
    -p:Configuration=$Configuration `
    -p:Platform=$Platform

# 构建 MSIX 侧载包
& msbuild '.\src\DotVast.HashTool.WinUI\DotVast.HashTool.WinUI.csproj' `
    -p:Configuration=$Configuration `
    -p:Platform=$Platform `
    -p:AppxBundle=Never `
    -p:UapAppxPackageBuildMode=SideloadOnly `
    -p:AppxPackageDir="$($tmpAppxPackageDir.FullName)\" `
    -p:GenerateAppxPackageOnBuild=true

# 移除不必要的依赖
$tmpPackageFolder = (Get-ChildItem $tmpAppxPackageDir)[0]
Get-ChildItem "$($tmpPackageFolder.FullName)/Dependencies" | Where-Object Name -NotLike $Platform | Remove-Item -Recurse

# 重命名包目录名
$gitDatetime = Get-Date -UnixTimeSeconds (git show -s --format=%ct HEAD) -Format 'yyMMdd'
$gitShortHash = git show -s --format=%h HEAD
$newPackageFolderName = "$($tmpPackageFolder.Name)-$gitDatetime-$gitShortHash"
$tmpPackageFolder = Rename-Item -Path $tmpPackageFolder -NewName $newPackageFolderName -PassThru

# 将包目录移动至目标位置
New-Item -Path $AppxPackageDir -ItemType Directory -Force | Out-Null
$packageFolder = "$AppxPackageDir\$newPackageFolderName"
if (Test-Path $packageFolder) {
    Remove-Item "$packageFolder" -Recurse
}
Move-Item -Path $tmpPackageFolder -Destination $packageFolder
Remove-Item $tmpAppxPackageDir

# 设置生成包的目录
$Global:OutputPackage = Get-Item $packageFolder

Pop-Location

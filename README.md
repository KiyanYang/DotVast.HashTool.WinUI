<p align="center">
  <img src="DotVast.HashTool.WinUI/Assets/Logo.png" width = "128" height = "128" alt="图标"/>
</p>

<div align="center">

# HashTool

#### 用于计算和校验文件、文件夹或文本哈希值的工具！

#### A Tool for Calculating and Verifying the Hash Value of Any File, Folder, or Text!

[![MIT License](https://img.shields.io/github/license/KiyanYang/DotVast.HashTool.WinUI)](https://github.com/KiyanYang/DotVast.HashTool.WinUI/blob/main/LICENSE.txt)
[![GitHub release (latest by date including pre-releases)](https://img.shields.io/github/v/release/KiyanYang/DotVast.HashTool.WinUI?include_prereleases)](https://github.com/KiyanYang/DotVast.HashTool.WinUI/releases)
[![GitHub issues](https://img.shields.io/github/issues/KiyanYang/DotVast.HashTool.WinUI)](https://github.com/KiyanYang/DotVast.HashTool.WinUI/issues)
[![GitHub all releases](https://img.shields.io/github/downloads/KiyanYang/DotVast.HashTool.WinUI/total)](https://github.com/KiyanYang/DotVast.HashTool.WinUI/releases)

[![CodeQL](https://github.com/KiyanYang/DotVast.HashTool.WinUI/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/KiyanYang/DotVast.HashTool.WinUI/actions/workflows/codeql-analysis.yml)
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FKiyanYang%2FDotVast.HashTool.WinUI.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2FKiyanYang%2FDotVast.HashTool.WinUI?ref=badge_shield)

</div>

---

HashTool 支持文件、文件夹或文本的哈希计算。

## 使用

打开 [系统设置 > 隐私和安全性 > 开发者选项](ms-settings:developers)，启用 `开发人员模式`，并展开下方的 `PowerShell`，启用`更改执行策略，以允许本地 PowerShell 脚本在未签名的情况下运行。远程脚本需要签名。`。

之后打开右侧的 [Release](https://github.com/KiyanYang/DotVast.HashTool.WinUI/releases) 页面，找到最新版本，并选择适用于当前系统的安装包下载。下载完成后，解压压缩包，右击 `Install.ps1` 脚本，选择“使用 PowerShell 运行”，根据提示进行安装。

## 界面

![主页](./Assets/HomePage.png)

## 感谢

- 工具
  - [Visual Studio Community 2022](https://visualstudio.microsoft.com/zh-hans/vs/community/)
  - [.NET 6](https://docs.microsoft.com/zh-cn/dotnet/api/?view=net-6.0)
  - [Template Studio](https://github.com/microsoft/TemplateStudio)

- 库
  - [CommunityToolkit.Mvvm](https://www.nuget.org/packages/CommunityToolkit.Mvvm)
  - [HashLib4CSharp](https://www.nuget.org/packages/HashLib4CSharp)
  - [Microsoft.Extensions.Hosting](https://www.nuget.org/packages/Microsoft.Extensions.Hosting)
  - [Microsoft.WindowsAppSDK](https://www.nuget.org/packages/Microsoft.WindowsAppSDK)
  - [Microsoft.Xaml.Behaviors.WinUI.Managed](https://www.nuget.org/packages/Microsoft.Xaml.Behaviors.WinUI.Managed)
  - [Serilog.Extensions.Hosting](https://www.nuget.org/packages/Serilog.Extensions.Hosting)
  - [Serilog.Sinks.File](https://www.nuget.org/packages/Serilog.Sinks.File)
  - [WinUIEx](https://www.nuget.org/packages/WinUIEx)

- 代码片段
  - [QuickXorHash.cs](https://gist.github.com/rgregg/c07a91964300315c6c3e77f7b5b861e4)
  

<p align="center">
  <img src="../src/DotVast.HashTool.WinUI/Assets/Logo.png" width = "128" height = "128" alt="图标"/>
</p>

<div align="center">

# HashTool

**用于计算文件、文件夹或文本哈希值的工具！**

[![MIT License](https://img.shields.io/github/license/KiyanYang/DotVast.HashTool.WinUI)](./LICENSE.txt)
[![GitHub release (latest by date)](https://img.shields.io/github/v/release/KiyanYang/DotVast.HashTool.WinUI)](https://github.com/KiyanYang/DotVast.HashTool.WinUI/releases)
[![GitHub issues](https://img.shields.io/github/issues/KiyanYang/DotVast.HashTool.WinUI)](https://github.com/KiyanYang/DotVast.HashTool.WinUI/issues)
[![GitHub all releases](https://img.shields.io/github/downloads/KiyanYang/DotVast.HashTool.WinUI/total)](https://github.com/KiyanYang/DotVast.HashTool.WinUI/releases)

[![Build](https://github.com/KiyanYang/DotVast.HashTool.WinUI/actions/workflows/build.yml/badge.svg)](https://github.com/KiyanYang/DotVast.HashTool.WinUI/actions/workflows/build.yml)
[![CodeQL](https://github.com/KiyanYang/DotVast.HashTool.WinUI/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/KiyanYang/DotVast.HashTool.WinUI/actions/workflows/codeql-analysis.yml)
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FKiyanYang%2FDotVast.HashTool.WinUI.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2FKiyanYang%2FDotVast.HashTool.WinUI?ref=badge_shield)

</div>

---

DotVast.HashTool.WinUI 允许你计算文件，文件夹或文本的哈希值。它是验证数据完整性和真实性的有用工具。

## 特性

- 可以计算文件，文件夹或文本的哈希值。
- 支持 MD5, SHA1, SHA2, SHA3 等哈希算法，详见 [wiki](https://github.com/KiyanYang/DotVast.HashTool.WinUI/wiki/功能#哈希算法)。
- 支持拖放文件和文件夹。
- 现代化且友好的界面。
- 支持多种语言。

## 安装

1. 打开 [系统设置 > 隐私和安全性 > 开发者选项](ms-settings:developers)，启用 `开发人员模式`，并展开下方的 `PowerShell`，启用`更改执行策略，以允许本地 PowerShell 脚本在未签名的情况下运行。远程脚本需要签名。`。
2. 在右侧的 [Release](https://github.com/KiyanYang/DotVast.HashTool.WinUI/releases) 中，找到最新版本，并下载适用于当前系统的压缩包。
3. 解压压缩包，右击 `Install.ps1` 脚本，选择“使用 PowerShell 运行”，根据提示进行安装。
4. 安装完毕后，注意恢复第 1 步启用的功能。

## 界面

![主页](./images/HomePage-0.3.0.webp)

## 贡献

如果你发现错误或者有反馈建议，请在阅读[贡献指南](./CONTRIBUTING.md)后，随时在 GitHub 上打开 issue 或 PR。你也可以 fork 这个存储库并进行自己的修改。如果你喜欢这个项目，请考虑 Star 它，以表示你的支持。

## 许可

DotVast.HashTool.WinUI 采用 MIT License，详见 [LICENSE](./LICENSE.txt).

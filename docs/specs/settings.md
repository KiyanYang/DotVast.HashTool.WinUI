# Settings

软件绝大部分设置保存至 `Windows.Storage.ApplicationData.Current.LocalSettings`，其余设置保存至软件自身。

## LocalSettings

在 `LocalSettings` 储存的内容均为 JSON 序列化后的字符串，其结构及运行时类型如下：

```yaml
LocalSettings:
  IsAlwaysOnTop: bool # 是否置顶.
  HashFontFamilyName: string # 显示哈希值位置的字体.
  Theme: Enums.AppTheme # 主题.
  FileAttributesToSkip: System.IO.FileAttributes # 要跳过的文件的属性.
  IncludePreRelease: bool # 检查更新时是否包括预发行版本.
  CheckForUpdatesOnStartup: bool # 是否启动时检查更新.
  StartingWhenCreateHashTask: bool # 创建任务时是否开始计算.

LocalSettings.Containers:
  DataOptions:Hashes:
    <Enums.HashKind>: Models.HashSetting # 哈希设置.
  ContextMenu:
    IsEnabled: bool # 是否启用资源管理器注册的上下文菜单.
    HashNames: [ string ] # 启用的哈希算法名称.
```

## Others

其余设置的位置及类型如下（注意此处未标明类型转换方式）：

```yaml
Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride: Enums.AppLanguage
```

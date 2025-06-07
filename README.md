# FFmpeg WinForm GUI 视频处理工具

![C#](https://img.shields.io/badge/C%23-100%25-239120.svg)
![.NET](https://img.shields.io/badge/.NET%20Framework-4.7.2+-512BD4.svg)
![FFmpeg](https://img.shields.io/badge/FFmpeg-Required-red.svg)
![License](https://img.shields.io/badge/License-MIT-green.svg)

一个基于 C# WinForms 和 FFMpegCore 库开发的现代化视频音频处理工具，提供直观的图形界面和强大的批处理功能。

## ✨ 主要特性

### 🎬 视频处理
- **智能视频合并** - 将多个视频文件无缝合并为一个完整视频
- **纯视频提取** - 从视频中移除音频轨道，保留纯视频内容
- **批量处理** - 支持同时处理多个视频文件

### 🎵 音频处理  
- **音频文件合并** - 将多个音频文件合并为单一音频文件
- **音频格式转换** - 从视频批量提取音频，自动转换为 MP3 格式
- **高质量输出** - 保持原始音频质量进行处理

### 🎞️ 音视频同步合并
- **智能音视频合并** - 将独立的视频和音频文件精准合并
- **音轨替换** - 为现有视频替换新的音频轨道
- **同步控制** - 支持音视频长度不匹配时的处理策略

### 📁 现代化文件管理
- **拖拽支持** - 支持直接拖拽文件到程序界面
- **批量文件操作** - 一次性选择和处理多个文件
- **可视化列表** - 清晰显示文件名、路径和状态
- **选择性处理** - 通过复选框精确控制要处理的文件
- **智能列表管理** - 支持删除选中项和一键清空

### 🔧 智能环境配置
- **自动 FFmpeg 检测** - 启动时自动检测 FFmpeg 可用性
- **一键安装** - 提供自动下载和配置 FFmpeg 的选项
- **手动配置指导** - 详细的手动安装指导和路径配置

### ⚡ 用户体验优化
- **异步处理** - 所有操作均为异步，不阻塞界面响应
- **实时进度显示** - 处理过程中显示详细进度和状态信息
- **操作可取消** - 支持中途取消长时间的处理操作
- **智能错误处理** - 详细的错误信息和处理建议

## 🎯 支持格式

### 视频格式
```
MP4, MKV, AVI, MOV, WMV, FLV, WebM, MPEG, MPG
```

### 音频格式
```
MP3, WAV, AAC, M4A, OGG, WMA, FLAC
```

## 🛠️ 环境要求

- **操作系统**: Windows 10/11 (推荐) 或 Windows 7 SP1+
- **运行时**: .NET Framework 4.7.2 或更高版本
- **依赖**: FFmpeg (程序可自动下载安装)
- **硬件**: 建议 4GB+ 内存用于处理大文件

## 🚀 快速开始

### 方式一：自动安装（推荐）
1. 直接运行程序
2. 程序会自动检测 FFmpeg 是否可用
3. 如未找到，选择"是"进行自动下载安装
4. 等待安装完成即可使用

### 方式二：手动安装
1. **下载 FFmpeg**
   ```
   访问：https://ffmpeg.org/download.html
   下载 Windows 版本的 FFmpeg
   ```

2. **配置 FFmpeg**
   ```
   解压下载的 FFmpeg 文件
   将 bin 目录下的所有文件复制到：
   [程序目录]\ffmpegShared\bin\
   ```

3. **验证安装**
   ```
   重新启动程序
   如果不再提示缺少 FFmpeg，说明配置成功
   ```

## 📖 使用指南

### 基础操作流程

#### 1️⃣ 添加文件
- **方法一**: 点击菜单栏 "添加文件"
- **方法二**: 直接拖拽文件到程序界面
- **支持**: 多文件同时选择和添加

#### 2️⃣ 选择目标文件
- 在文件列表中勾选需要处理的文件
- 不同操作对文件数量和类型有特定要求
- 程序会自动验证文件格式和数量

#### 3️⃣ 执行处理操作

##### 🎬 合并视频
```
要求：至少勾选 2 个视频文件
操作：菜单 → 合并视频
输出：选择保存位置和文件名
```

##### 🎵 合并音频  
```
要求：至少勾选 2 个音频文件
操作：菜单 → 合并音频
输出：选择保存位置和文件名
```

##### 🎞️ 音视频合并
```
要求：勾选 1 个视频文件 + 1 个音频文件
操作：菜单 → 合并音视频
输出：选择保存位置和文件名
特性：支持音视频长度不匹配处理
```

##### 🎵 批量提取音频
```
要求：勾选任意数量的视频文件
操作：菜单 → 提取音频
输出：选择输出文件夹
结果：自动生成同名的 MP3 文件
```

##### 🎬 批量提取视频
```
要求：勾选任意数量的视频文件  
操作：菜单 → 提取视频
输出：选择输出文件夹
结果：生成去除音频的纯视频文件
```

## 🏗️ 项目架构

```
FFmpeg-WinForm-GUI/
├── 核心文件
│   ├── Form1.cs                 # 主窗体逻辑和用户交互
│   ├── Form1.Designer.cs        # 窗体 UI 设计器代码
│   ├── FFmpegExtensions.cs      # FFmpeg 操作扩展方法库
│   ├── FFmpegDownloader.cs      # FFmpeg 自动下载管理器
│   ├── FFmpegDownloadForm.cs    # FFmpeg 下载进度窗体
│   ├── ProgressForm.cs          # 操作进度显示窗体
│   └── Program.cs               # 程序入口点
├── 依赖文件
│   └── ffmpegShared/            # FFmpeg 可执行文件目录
│       └── bin/
│           ├── ffmpeg.exe       # 核心转换工具
│           ├── ffprobe.exe      # 媒体信息分析工具
│           └── ffplay.exe       # 媒体播放工具
└── 配置文件
    ├── app.config               # 应用程序配置
    └── README.md                # 项目文档
```

## 🔧 核心技术栈

### 主要框架
- **FFMpegCore 5.1.0** - .NET 平台的 FFmpeg 现代化封装库
- **Windows Forms** - 成熟稳定的桌面应用程序框架
- **C# 8.0+** - 使用最新语言特性提升开发效率

### 关键技术特性
- **异步编程模型** - 基于 async/await 的非阻塞操作
- **进度报告机制** - IProgress<T> 接口实现实时状态更新
- **取消令牌支持** - CancellationToken 支持操作中断
- **扩展方法设计** - 优雅的 FFmpeg 操作封装

## 🔌 扩展 API

`FFmpegExtensions` 类提供了丰富的异步扩展方法：

### 基础合并操作
```csharp
// 异步视频合并
public static Task<bool> MergeVideosAsync(string output, string[] videoFiles, 
    IProgress<string> progress = null, CancellationToken cancellationToken = default)

// 异步音频合并
public static Task<bool> MergeAudiosAsync(string output, string[] audioFiles,
    IProgress<string> progress = null, CancellationToken cancellationToken = default)

// 异步音视频合并
public static Task<bool> MergeAudioVideoAsync(string videoFile, string audioFile, 
    string output, bool stopAtShortest = false, IProgress<string> progress = null, 
    CancellationToken cancellationToken = default)
```

### 批量提取操作
```csharp
// 批量音频提取
public static Task<(int success, int failed)> BatchExtractAudioAsync(
    string[] videoFiles, string outputDirectory, IProgress<string> progress = null, 
    CancellationToken cancellationToken = default)

// 批量视频提取（去除音频）
public static Task<(int success, int failed)> BatchExtractVideoAsync(
    string[] videoFiles, string outputDirectory, IProgress<string> progress = null, 
    CancellationToken cancellationToken = default)
```

### 高级提取选项
```csharp
// 高质量音频提取
public static Task<bool> ExtractAudioAsync(string videoFile, string audioOutput, 
    string audioCodec = null, AudioQuality? quality = null, int? sampleRate = null,
    IProgress<string> progress = null, CancellationToken cancellationToken = default)

// 自定义视频提取
public static Task<bool> ExtractVideoAsync(string videoFile, string videoOutput, 
    string videoCodec = null, VideoSize? size = null, int? fps = null,
    IProgress<string> progress = null, CancellationToken cancellationToken = default)
```

## ⚠️ 重要注意事项

### 🔴 必读警告
- **FFmpeg 依赖**: 必须确保 FFmpeg 已正确安装并可访问
- **文件备份**: 建议在处理重要文件前进行备份
- **性能考虑**: 处理大文件时需要充足的磁盘空间和处理时间
- **格式兼容**: 部分特殊编码的文件可能需要预处理

### 💡 性能优化建议
- **内存使用**: 同时处理的大文件数量不宜过多
- **磁盘空间**: 确保输出目录有足够的可用空间
- **系统资源**: 处理过程中避免运行其他资源密集型程序

## 🆘 故障排除

### 常见问题及解决方案

**Q: 程序启动时提示"FFmpeg 未找到"？**
```
解决方案：
1. 选择自动下载安装（推荐）
2. 手动下载 FFmpeg 并放置到 ffmpegShared\bin\ 目录
3. 检查文件权限，确保程序有读取权限
```

**Q: 处理过程中出现"操作失败"？**  
```
可能原因：
1. 文件格式不兼容 → 尝试转换文件格式
2. 文件已损坏 → 使用其他工具验证文件完整性
3. 磁盘空间不足 → 清理磁盘空间
4. 文件被占用 → 关闭可能占用文件的程序
```

**Q: 处理大文件时程序无响应？**
```
这是正常现象：
1. 程序使用异步处理，界面不会冻结
2. 查看进度窗口了解处理状态
3. 可以随时点击取消按钮中止操作
4. 耐心等待处理完成
```

**Q: 输出文件质量不理想？**
```
优化建议：
1. 检查源文件质量
2. 尝试不同的输出格式
3. 考虑使用专业参数调整（需代码修改）
```

## 🎯 开发信息

### 技术规格
- **开发语言**: C# 8.0+
- **目标框架**: .NET Framework 4.7.2
- **UI 框架**: Windows Forms
- **核心依赖**: FFMpegCore 5.1.0
- **支持平台**: Windows 7 SP1+ / Windows 10/11

### 项目统计
- **创建时间**: 2025年5月29日
- **最后更新**: 2025年6月7日
- **代码语言**: 100% C#
- **开源协议**: MIT License

## 📄 许可证

本项目基于 [MIT License](LICENSE) 开源协议发布。

```
Copyright (c) 2025 qq7995516

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
```

## 🤝 贡献与支持

### 如何贡献
- 🐛 **Bug 报告**: 在 Issues 中详细描述问题
- 💡 **功能建议**: 提出新功能想法和改进建议  
- 🔧 **代码贡献**: 提交 Pull Request 参与开发
- 📖 **文档改进**: 帮助完善项目文档

### 技术支持
- **GitHub Issues**: 主要的问题讨论和 Bug 报告平台
- **代码审查**: 欢迎同行开发者进行代码审查和建议

---

<div align="center">

**💝 如果这个项目对你有帮助，请考虑给它一个 ⭐ Star 支持！**

[![GitHub stars](https://img.shields.io/github/stars/qq7995516/FFmpeg-WinForm-GUI.svg?style=social&label=Star)](https://github.com/qq7995516/FFmpeg-WinForm-GUI/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/qq7995516/FFmpeg-WinForm-GUI.svg?style=social&label=Fork)](https://github.com/qq7995516/FFmpeg-WinForm-GUI/network)

</div>
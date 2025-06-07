# FFmpeg 视频处理工具

一个基于 C# WinForms 和 FFMpegCore 库开发的简单易用的视频音频处理工具。

## 功能特性

### 🎬 视频处理
- **合并多个视频文件** - 将多个视频文件合并为一个完整的视频
- **提取纯视频流** - 从视频中去除音频轨道，保留纯视频内容

### 🎵 音频处理  
- **合并多个音频文件** - 将多个音频文件合并为一个音频文件
- **从视频提取音频** - 批量从视频文件中提取音频，输出为 MP3 格式

### 🎞️ 音视频合并
- **视频+音频合并** - 将一个视频文件与一个音频文件合并，可用于替换视频的音轨

### 📁 文件管理
- **批量文件添加** - 支持一次选择多个文件进行处理
- **可视化文件列表** - 清晰显示文件名和完整路径
- **勾选式操作** - 通过勾选框选择要处理的文件
- **列表管理** - 支持删除勾选项和清空列表

## 支持格式

### 视频格式
- MP4, MKV, AVI, MOV, WMV, FLV, WebM, MPEG, MPG

### 音频格式
- MP3, WAV, AAC, M4A, OGG, WMA, FLAC

## 环境要求

- .NET Framework 4.7.2 或更高版本
- Windows 操作系统
- FFmpeg 可执行文件（需放置在 `ffmpegShared\bin\` 目录下）

## 安装说明

1. **下载 FFmpeg**
   ```
   访问 https://ffmpeg.org/download.html
   下载适合 Windows 的 FFmpeg 版本
   ```

2. **配置 FFmpeg**
   ```
   解压 FFmpeg 文件
   将 ffmpeg.exe 等可执行文件复制到项目的 ffmpegShared\bin\ 目录下
   ```

3. **编译项目**
   ```bash
   # 使用 Visual Studio 打开项目
   # 或使用 dotnet CLI
   dotnet build
   ```

## 使用方法

### 1. 添加文件
- 点击菜单栏的"添加文件"选项
- 在对话框中选择要处理的视频或音频文件
- 支持多选，一次添加多个文件

### 2. 选择操作文件
- 在文件列表中勾选需要处理的文件
- 不同操作对文件数量和类型有不同要求

### 3. 执行操作

#### 合并视频
- 勾选至少 2 个视频文件
- 点击"合并视频"菜单项
- 选择输出位置和文件名

#### 合并音频  
- 勾选至少 2 个音频文件
- 点击"合并音频"菜单项
- 选择输出位置和文件名

#### 音视频合并
- 勾选 1 个视频文件和 1 个音频文件
- 点击"合并音视频"菜单项
- 选择输出位置和文件名

#### 提取音频
- 勾选任意数量的视频文件
- 点击"提取音频"菜单项
- 选择输出文件夹，程序会批量提取音频为 MP3 格式

#### 提取视频
- 勾选任意数量的视频文件  
- 点击"提取视频"菜单项
- 选择输出文件夹，程序会生成去除音频的纯视频文件

## 项目结构

```
ffmpeg视频处理/
├── FFmpegExtensions.cs     # FFmpeg 操作扩展方法
├── Form1.cs               # 主窗体逻辑
├── Form1.Designer.cs      # 窗体设计器代码
├── Program.cs             # 程序入口点
├── ffmpegShared/          # FFmpeg 可执行文件目录
│   └── bin/
│       ├── ffmpeg.exe
│       ├── ffprobe.exe
│       └── ffplay.exe
└── README.md              # 项目说明文档
```

## 核心技术

- **FFMpegCore** - .NET 下的 FFmpeg 封装库
- **WinForms** - Windows 桌面应用程序框架
- **C# 扩展方法** - 自定义的 FFmpeg 操作扩展

## 扩展功能

`FFmpegExtensions` 类提供了以下扩展方法：

```csharp
// 基础方法
public static bool MergeVideos(string output, params string[] videoFiles)
public static bool MergeAudios(string output, params string[] audioFiles)  
public static bool MergeAudioVideo(string videoFile, string audioFile, string output, bool stopAtShortest = false)

// 提取方法
public static bool ExtractAudio(string videoFile, string audioOutput)
public static bool ExtractAudio(string videoFile, string audioOutput, string audioCodec = null, AudioQuality? quality = null, int? sampleRate = null)
public static bool ExtractVideo(string videoFile, string videoOutput)
public static bool ExtractVideo(string videoFile, string videoOutput, string videoCodec = null, VideoSize? size = null, int? fps = null)
```

## 注意事项

⚠️ **重要提醒**
- 确保 FFmpeg 可执行文件已正确配置在 `ffmpegShared\bin\` 目录
- 处理大文件时可能需要较长时间，请耐心等待
- 建议在处理前备份原始文件
- 某些特殊格式的文件可能无法正常处理

## 常见问题

**Q: 提示找不到 FFmpeg？**
A: 检查 `ffmpegShared\bin\` 目录下是否包含 `ffmpeg.exe` 文件

**Q: 处理失败怎么办？**  
A: 确认文件格式是否支持，尝试转换文件格式后再处理

**Q: 可以处理哪些文件大小？**
A: 理论上没有限制，但处理大文件需要更多时间和系统资源

## 开发者信息

- **开发语言**: C#
- **框架**: .NET Framework / .NET Core
- **UI框架**: Windows Forms
- **依赖库**: FFMpegCore

## 许可证

本项目仅供学习和个人使用。

---

💡 **提示**: 如果你觉得这个工具有用，欢迎 Star 支持！

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Diagnostics;
using System.Windows.Forms;

namespace ffmpeg视频处理
{
    /// <summary>
    /// 下载和安装 FFmpeg 的工具类
    /// </summary>
    public static class FFmpegDownloader
    {
        private static readonly string FFmpegFolder = Path.Combine(Application.StartupPath, "ffmpegShared");
        private static readonly string FFmpegBinFolder = Path.Combine(FFmpegFolder, "bin");
        private static readonly string FFmpegExePath = Path.Combine(FFmpegBinFolder, "ffmpeg.exe");

        // FFmpeg 官方下载地址 (Windows 64位版本)
        private const string FFmpegDownloadUrl = "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip";

        /// <summary>
        /// 检查 FFmpeg 是否已安装且可用
        /// </summary>
        public static bool IsFFmpegInstalled()
        {
            try
            {
                if (!File.Exists(FFmpegExePath))
                    return false;

                // 测试 FFmpeg 是否能正常运行
                using (var process = new Process())
                {
                    process.StartInfo.FileName = FFmpegExePath;
                    process.StartInfo.Arguments = "-version";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    process.WaitForExit(5000); // 5秒超时
                    return process.ExitCode == 0;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 自动下载并安装 FFmpeg
        /// </summary>
        public static async Task<bool> DownloadAndInstallFFmpegAsync(IProgress<string> progress = null)
        {
            try
            {
                progress?.Report("开始下载 FFmpeg...");

                // 创建目录
                Directory.CreateDirectory(FFmpegFolder);
                Directory.CreateDirectory(FFmpegBinFolder);

                string tempZipPath = Path.Combine(Path.GetTempPath(), "ffmpeg-temp.zip");
                string tempExtractPath = Path.Combine(Path.GetTempPath(), "ffmpeg-extract");

                try
                {
                    // 下载 FFmpeg
                    progress?.Report("正在下载 FFmpeg 压缩包...");
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.Timeout = TimeSpan.FromMinutes(10); // 10分钟超时
                        var response = await httpClient.GetAsync(FFmpegDownloadUrl);
                        response.EnsureSuccessStatusCode();

                        await using var fileStream = File.Create(tempZipPath);
                        await response.Content.CopyToAsync(fileStream);
                    }

                    progress?.Report("下载完成，正在解压...");

                    // 解压文件
                    if (Directory.Exists(tempExtractPath))
                        Directory.Delete(tempExtractPath, true);

                    ZipFile.ExtractToDirectory(tempZipPath, tempExtractPath);

                    progress?.Report("正在安装 FFmpeg...");

                    // 查找解压后的 bin 目录
                    string[] binDirs = Directory.GetDirectories(tempExtractPath, "bin", SearchOption.AllDirectories);
                    if (binDirs.Length == 0)
                    {
                        progress?.Report("错误：解压后未找到 bin 目录");
                        return false;
                    }

                    string sourceBinDir = binDirs[0];

                    // 复制必要的可执行文件
                    string[] requiredFiles = { "ffmpeg.exe", "ffprobe.exe", "ffplay.exe" };
                    foreach (string fileName in requiredFiles)
                    {
                        string sourceFile = Path.Combine(sourceBinDir, fileName);
                        string destFile = Path.Combine(FFmpegBinFolder, fileName);

                        if (File.Exists(sourceFile))
                        {
                            File.Copy(sourceFile, destFile, true);
                            progress?.Report($"已安装: {fileName}");
                        }
                    }

                    progress?.Report("FFmpeg 安装完成！");
                    return IsFFmpegInstalled();
                }
                finally
                {
                    // 清理临时文件
                    try
                    {
                        if (File.Exists(tempZipPath))
                            File.Delete(tempZipPath);
                        if (Directory.Exists(tempExtractPath))
                            Directory.Delete(tempExtractPath, true);
                    }
                    catch { } // 忽略清理错误
                }
            }
            catch (Exception ex)
            {
                progress?.Report($"下载失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 获取 FFmpeg 版本信息
        /// </summary>
        public static string GetFFmpegVersion()
        {
            try
            {
                if (!File.Exists(FFmpegExePath))
                    return "未安装";

                using (var process = new Process())
                {
                    process.StartInfo.FileName = FFmpegExePath;
                    process.StartInfo.Arguments = "-version";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();

                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    // 提取版本号 (格式: ffmpeg version 4.4.0-...)
                    var lines = output.Split('\n');
                    if (lines.Length > 0 && lines[0].StartsWith("ffmpeg version"))
                    {
                        return lines[0].Replace("ffmpeg version ", "").Split(' ')[0];
                    }

                    return "版本未知";
                }
            }
            catch
            {
                return "获取失败";
            }
        }
    }
}
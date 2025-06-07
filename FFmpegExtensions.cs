using FFMpegCore;
using FFMpegCore.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace ffmpeg视频处理
{
    public static class FFmpegExtensions
    {
        /// <summary>
        /// 异步合并多个视频文件
        /// </summary>
        public static async Task<bool> MergeVideosAsync(string output, string[] videoFiles,
            IProgress<string> progress = null, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() =>
            {
                try
                {
                    progress?.Report("开始合并视频文件...");
                    cancellationToken.ThrowIfCancellationRequested();

                    bool result = FFMpeg.Join(output, videoFiles);

                    progress?.Report(result ? "视频合并完成" : "视频合并失败");
                    return result;
                }
                catch (OperationCanceledException)
                {
                    progress?.Report("操作已取消");
                    throw;
                }
                catch (Exception ex)
                {
                    progress?.Report($"合并失败: {ex.Message}");
                    return false;
                }
            }, cancellationToken);
        }

        /// <summary>
        /// 异步合并多个音频文件
        /// </summary>
        public static async Task<bool> MergeAudiosAsync(string output, string[] audioFiles,
            IProgress<string> progress = null, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() =>
            {
                try
                {
                    progress?.Report("开始合并音频文件...");
                    cancellationToken.ThrowIfCancellationRequested();

                    bool result = FFMpegArguments
                        .FromConcatInput(audioFiles)
                        .OutputToFile(output, true, opt => opt.CopyChannel())
                        .ProcessSynchronously();

                    progress?.Report(result ? "音频合并完成" : "音频合并失败");
                    return result;
                }
                catch (OperationCanceledException)
                {
                    progress?.Report("操作已取消");
                    throw;
                }
                catch (Exception ex)
                {
                    progress?.Report($"合并失败: {ex.Message}");
                    return false;
                }
            }, cancellationToken);
        }

        /// <summary>
        /// 异步合并视频和音频
        /// </summary>
        public static async Task<bool> MergeAudioVideoAsync(string videoFile, string audioFile, string output,
            bool stopAtShortest = false, IProgress<string> progress = null, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() =>
            {
                try
                {
                    progress?.Report("开始合并音视频文件...");
                    cancellationToken.ThrowIfCancellationRequested();

                    bool result = FFMpeg.ReplaceAudio(videoFile, audioFile, output, stopAtShortest);

                    progress?.Report(result ? "音视频合并完成" : "音视频合并失败");
                    return result;
                }
                catch (OperationCanceledException)
                {
                    progress?.Report("操作已取消");
                    throw;
                }
                catch (Exception ex)
                {
                    progress?.Report($"合并失败: {ex.Message}");
                    return false;
                }
            }, cancellationToken);
        }

        /// <summary>
        /// 异步提取音频
        /// </summary>
        public static async Task<bool> ExtractAudioAsync(string videoFile, string audioOutput,
            IProgress<string> progress = null, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() =>
            {
                try
                {
                    progress?.Report($"开始提取音频: {Path.GetFileName(videoFile)}");
                    cancellationToken.ThrowIfCancellationRequested();

                    bool result = FFMpegArguments
                        .FromFileInput(videoFile)
                        .OutputToFile(audioOutput, true, opt => opt
                            .WithVideoCodec("none")
                            .WithAudioCodec("copy"))
                        .ProcessSynchronously();

                    progress?.Report(result ? "音频提取完成" : "音频提取失败");
                    return result;
                }
                catch (OperationCanceledException)
                {
                    progress?.Report("操作已取消");
                    throw;
                }
                catch (Exception ex)
                {
                    progress?.Report($"提取失败: {ex.Message}");
                    return false;
                }
            }, cancellationToken);
        }

        /// <summary>
        /// 异步提取视频
        /// </summary>
        public static async Task<bool> ExtractVideoAsync(string videoFile, string videoOutput,
            IProgress<string> progress = null, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() =>
            {
                try
                {
                    progress?.Report($"开始提取视频: {Path.GetFileName(videoFile)}");
                    cancellationToken.ThrowIfCancellationRequested();

                    bool result = FFMpegArguments
                        .FromFileInput(videoFile)
                        .OutputToFile(videoOutput, true, opt => opt
                            .WithVideoCodec("copy")
                            .WithAudioCodec("none"))
                        .ProcessSynchronously();

                    progress?.Report(result ? "视频提取完成" : "视频提取失败");
                    return result;
                }
                catch (OperationCanceledException)
                {
                    progress?.Report("操作已取消");
                    throw;
                }
                catch (Exception ex)
                {
                    progress?.Report($"提取失败: {ex.Message}");
                    return false;
                }
            }, cancellationToken);
        }

        /// <summary>
        /// 批量异步提取音频
        /// </summary>
        public static async Task<(int success, int failed)> BatchExtractAudioAsync(
            string[] videoFiles, string outputFolder,
            IProgress<string> progress = null, CancellationToken cancellationToken = default)
        {
            int successCount = 0;
            int failCount = 0;

            for (int i = 0; i < videoFiles.Length; i++)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    string videoFile = videoFiles[i];
                    string fileName = Path.GetFileNameWithoutExtension(videoFile);
                    string audioOutput = Path.Combine(outputFolder, $"{fileName}.mp3");

                    progress?.Report($"处理 {i + 1}/{videoFiles.Length}: {Path.GetFileName(videoFile)}");

                    bool result = await ExtractAudioAsync(videoFile, audioOutput, null, cancellationToken);

                    if (result)
                        successCount++;
                    else
                        failCount++;
                }
                catch (OperationCanceledException)
                {
                    progress?.Report("批量处理已取消");
                    throw;
                }
                catch (Exception ex)
                {
                    progress?.Report($"处理 {Path.GetFileName(videoFiles[i])} 时出错: {ex.Message}");
                    failCount++;
                }
            }

            return (successCount, failCount);
        }

        /// <summary>
        /// 批量异步提取视频
        /// </summary>
        public static async Task<(int success, int failed)> BatchExtractVideoAsync(
            string[] videoFiles, string outputFolder,
            IProgress<string> progress = null, CancellationToken cancellationToken = default)
        {
            int successCount = 0;
            int failCount = 0;

            for (int i = 0; i < videoFiles.Length; i++)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    string videoFile = videoFiles[i];
                    string fileName = Path.GetFileNameWithoutExtension(videoFile);
                    string extension = Path.GetExtension(videoFile);
                    string videoOutput = Path.Combine(outputFolder, $"{fileName}_no_audio{extension}");

                    progress?.Report($"处理 {i + 1}/{videoFiles.Length}: {Path.GetFileName(videoFile)}");

                    bool result = await ExtractVideoAsync(videoFile, videoOutput, null, cancellationToken);

                    if (result)
                        successCount++;
                    else
                        failCount++;
                }
                catch (OperationCanceledException)
                {
                    progress?.Report("批量处理已取消");
                    throw;
                }
                catch (Exception ex)
                {
                    progress?.Report($"处理 {Path.GetFileName(videoFiles[i])} 时出错: {ex.Message}");
                    failCount++;
                }
            }

            return (successCount, failCount);
        }
    }
}
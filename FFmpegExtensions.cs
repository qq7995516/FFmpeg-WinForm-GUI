using FFMpegCore;
using FFMpegCore.Enums;

namespace ffmpeg视频处理
{
    public static class FFmpegExtensions
    {
        /// <summary>
        /// 合并多个视频文件
        /// </summary>
        /// <param name="output">输出文件路径</param>
        /// <param name="videoFiles">要合并的视频文件路径数组</param>
        /// <returns>合并是否成功</returns>
        public static bool MergeVideos(string output, params string[] videoFiles)
        {
            // 直接调用内置 Join 方法即可
            return FFMpeg.Join(output, videoFiles);
        }

        /// <summary>
        /// 合并多个音频文件
        /// </summary>
        /// <param name="output">输出音频文件路径</param>
        /// <param name="audioFiles">要合并的音频文件路径数组</param>
        /// <returns>合并是否成功</returns>
        public static bool MergeAudios(string output, params string[] audioFiles)
        {
            // 这里实际上可以利用 FFMpeg.Join 实现，但需要保证输入都是音频文件
            // FFMpeg.Join 主要为视频设计，音频可用 ffmpeg concat demuxer 达成
            // 以下是简单的方案
            return FFMpegArguments
                .FromConcatInput(audioFiles)
                .OutputToFile(output, true, opt => opt.CopyChannel())
                .ProcessSynchronously();
        }

        /// <summary>
        /// 合并视频和音频为一个文件（如视频去原音轨后加上新音轨）
        /// </summary>
        /// <param name="videoFile">原始视频文件路径</param>
        /// <param name="audioFile">音频文件路径</param>
        /// <param name="output">输出文件路径</param>
        /// <param name="stopAtShortest">是否按最短时长合并</param>
        /// <returns>合并是否成功</returns>
        public static bool MergeAudioVideo(string videoFile, string audioFile, string output, bool stopAtShortest = false)
        {
            // 直接调用 ReplaceAudio 完成视频音轨替换
            return FFMpeg.ReplaceAudio(videoFile, audioFile, output, stopAtShortest);
        }

        /// <summary>
        /// 从视频文件中提取音频
        /// </summary>
        /// <param name="videoFile">输入视频文件路径</param>
        /// <param name="audioOutput">输出音频文件路径</param>
        /// <returns>提取是否成功</returns>
        public static bool ExtractAudio(string videoFile, string audioOutput)
        {
            return FFMpegArguments
                .FromFileInput(videoFile)
                .OutputToFile(audioOutput, true, opt => opt
                    .WithVideoCodec("none")
                    .WithAudioCodec("copy")
                )
                .ProcessSynchronously();
        }

        /// <summary>
        /// 从视频文件中提取视频流（去除音频）
        /// </summary>
        /// <param name="videoFile">输入视频文件路径</param>
        /// <param name="videoOutput">输出视频文件路径</param>
        /// <returns>提取是否成功</returns>
        public static bool ExtractVideo(string videoFile, string videoOutput)
        {
            return FFMpegArguments
                .FromFileInput(videoFile)
                .OutputToFile(videoOutput, true, opt => opt
                    .WithVideoCodec("copy")   // 直接复制视频流
                    .WithAudioCodec("none")   // 不包含音频流
                )
                .ProcessSynchronously();
        }
    }
}

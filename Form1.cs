using System.Threading.Tasks;
using System.Threading;

namespace ffmpeg视频处理
{
    public partial class Form1 : Form
    {
        private readonly HashSet<string> _videoExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        { ".mp4", ".mkv", ".avi", ".mov", ".wmv", ".flv", ".webm", ".mpeg", ".mpg" };

        private readonly HashSet<string> _audioExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        { ".mp3", ".wav", ".aac", ".m4a", ".ogg", ".wma", ".flac" };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!FFmpegDownloader.IsFFmpegInstalled())
            {
                var result = MessageBox.Show(
                    "检测到 FFmpeg 未安装或无法使用。\n\n是否现在下载安装 FFmpeg？\n\n点击\"是\"自动下载，点击\"否\"手动配置。",
                    "FFmpeg 未找到",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using var downloadForm = new FFmpegDownloadForm();
                    downloadForm.ShowDialog(this);
                }
                else if (result == DialogResult.No)
                {
                    MessageBox.Show(
                        "请手动下载 FFmpeg 并放置到以下目录：\n\n" +
                        Path.Combine(Application.StartupPath, "ffmpegShared", "bin") +
                        "\n\n下载地址：https://ffmpeg.org/download.html",
                        "手动安装说明",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }

            FFMpegCore.GlobalFFOptions.Configure(options =>
            {
                options.BinaryFolder = Path.Combine(Application.StartupPath, "ffmpegShared", "bin");
            });

            InitializeListViewSettings();
        }

        private async void 合并视频ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await ProcessFiles("合并视频", async (files, progress, token) =>
            {
                if (files.Length < 2)
                {
                    MessageBox.Show("请至少选择两个视频文件进行合并", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!ValidateFileTypes(files, _videoExtensions, "视频"))
                    return;

                using var saveDialog = new SaveFileDialog();
                saveDialog.Filter = "MP4 文件|*.mp4|所有文件|*.*";
                saveDialog.Title = "保存合并后的视频";
                saveDialog.DefaultExt = "mp4";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    bool success = await FFmpegExtensions.MergeVideosAsync(
                        saveDialog.FileName, files, progress, token);

                    ShowResult(success, "视频合并");
                }
            });
        }

        private async void 合并音频ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await ProcessFiles("合并音频", async (files, progress, token) =>
            {
                if (files.Length < 2)
                {
                    MessageBox.Show("请至少选择两个音频文件进行合并", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!ValidateFileTypes(files, _audioExtensions, "音频"))
                    return;

                using var saveDialog = new SaveFileDialog();
                saveDialog.Filter = "MP3 文件|*.mp3|WAV 文件|*.wav|所有文件|*.*";
                saveDialog.Title = "保存合并后的音频";
                saveDialog.DefaultExt = "mp3";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    bool success = await FFmpegExtensions.MergeAudiosAsync(
                        saveDialog.FileName, files, progress, token);

                    ShowResult(success, "音频合并");
                }
            });
        }

        private async void 合并音视频ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await ProcessFiles("合并音视频", async (files, progress, token) =>
            {
                if (files.Length != 2)
                {
                    MessageBox.Show("请选择一个视频文件和一个音频文件进行合并", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string videoFile = null;
                string audioFile = null;

                foreach (var filePath in files)
                {
                    string extension = Path.GetExtension(filePath);
                    if (_videoExtensions.Contains(extension))
                    {
                        if (videoFile == null)
                            videoFile = filePath;
                        else
                        {
                            MessageBox.Show("只能选择一个视频文件", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else if (_audioExtensions.Contains(extension))
                    {
                        if (audioFile == null)
                            audioFile = filePath;
                        else
                        {
                            MessageBox.Show("只能选择一个音频文件", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"文件 {Path.GetFileName(filePath)} 不是支持的视频或音频格式", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (videoFile == null || audioFile == null)
                {
                    MessageBox.Show("请确保选择了一个视频文件和一个音频文件", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using var saveDialog = new SaveFileDialog();
                saveDialog.Filter = "MP4 文件|*.mp4|所有文件|*.*";
                saveDialog.Title = "保存合并后的视频";
                saveDialog.DefaultExt = "mp4";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    bool success = await FFmpegExtensions.MergeAudioVideoAsync(
                        videoFile, audioFile, saveDialog.FileName, true, progress, token);

                    ShowResult(success, "音视频合并");
                }
            });
        }

        private async void 提取音频ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await ProcessFiles("批量提取音频", async (files, progress, token) =>
            {
                if (files.Length == 0)
                {
                    MessageBox.Show("请选择至少一个视频文件进行音频提取", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!ValidateFileTypes(files, _videoExtensions, "视频"))
                    return;

                using var folderDialog = new FolderBrowserDialog();
                folderDialog.Description = "选择音频文件保存位置";

                if (folderDialog.ShowDialog() != DialogResult.OK)
                    return;

                var result = await FFmpegExtensions.BatchExtractAudioAsync(
                    files, folderDialog.SelectedPath, progress, token);

                string message = $"音频提取完成！\n成功: {result.success} 个\n失败: {result.failed} 个";
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show(message, "完成", MessageBoxButtons.OK,
                        result.failed == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                }));
            });
        }

        private async void 提取视频ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await ProcessFiles("批量提取视频", async (files, progress, token) =>
            {
                if (files.Length == 0)
                {
                    MessageBox.Show("请选择至少一个视频文件进行视频提取（去除音频）", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!ValidateFileTypes(files, _videoExtensions, "视频"))
                    return;

                using var folderDialog = new FolderBrowserDialog();
                folderDialog.Description = "选择视频文件保存位置";

                if (folderDialog.ShowDialog() != DialogResult.OK)
                    return;

                var result = await FFmpegExtensions.BatchExtractVideoAsync(
                    files, folderDialog.SelectedPath, progress, token);

                string message = $"视频提取完成！\n成功: {result.success} 个\n失败: {result.failed} 个";
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show(message, "完成", MessageBoxButtons.OK,
                        result.failed == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                }));
            });
        }

        private void 删除勾选项ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = listViewFiles.Items.Count - 1; i >= 0; i--)
                {
                    if (listViewFiles.Items[i].Checked)
                    {
                        listViewFiles.Items.RemoveAt(i);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除勾选项时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show("确定要清空所有文件吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    listViewFiles.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"清空列表时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 添加文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using var openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "选择要添加的文件";
                openFileDialog.Filter = "媒体文件|*.mp4;*.mkv;*.avi;*.mov;*.wmv;*.flv;*.webm;*.mpeg;*.mpg;*.mp3;*.wav;*.aac;*.m4a;*.ogg;*.wma;*.flac|视频文件|*.mp4;*.mkv;*.avi;*.mov;*.wmv;*.flv;*.webm;*.mpeg;*.mpg|音频文件|*.mp3;*.wav;*.aac;*.m4a;*.ogg;*.wma;*.flac|所有文件|*.*";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filePath in openFileDialog.FileNames)
                    {
                        string fileName = Path.GetFileName(filePath);
                        ListViewItem item = new ListViewItem(fileName);
                        item.SubItems.Add(filePath);
                        item.Tag = filePath;
                        listViewFiles.Items.Add(item);
                    }

                    if (openFileDialog.FileNames.Length > 0)
                    {
                        MessageBox.Show($"已成功添加 {openFileDialog.FileNames.Length} 个文件",
                            "添加成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加文件时发生错误: {ex.Message}",
                    "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<string> GetCheckedItems()
        {
            List<string> checkedItems = new List<string>();
            foreach (ListViewItem item in listViewFiles.Items)
            {
                if (item.Checked && item.Tag != null)
                {
                    checkedItems.Add(item.Tag.ToString());
                }
            }
            return checkedItems;
        }

        private void InitializeListViewSettings()
        {
            listViewFiles.View = View.Details;

            if (listViewFiles.Columns.Count == 0)
            {
                listViewFiles.Columns.Add("文件名", 200, HorizontalAlignment.Left);
                listViewFiles.Columns.Add("文件路径", 400, HorizontalAlignment.Left);
            }

            listViewFiles.MultiSelect = true;
            listViewFiles.GridLines = true;
            listViewFiles.FullRowSelect = true;
            listViewFiles.AllowDrop = true;
        }

        private async Task ProcessFiles(string operationName,
            Func<string[], IProgress<string>, CancellationToken, Task> operation)
        {
            var checkedItems = GetCheckedItems();
            if (checkedItems.Count == 0)
            {
                MessageBox.Show($"请选择要进行{operationName}的文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var progressForm = new ProgressForm(operationName);

            var progress = new Progress<string>(message =>
            {
                progressForm.UpdateStatus(message);
            });

            var progressTask = Task.Run(async () =>
            {
                try
                {
                    await operation(checkedItems.ToArray(), progress, progressForm.CancellationToken);
                    progressForm.SetCompleted(true);
                }
                catch (OperationCanceledException)
                {
                    progressForm.UpdateStatus("操作已取消");
                    progressForm.SetCompleted(false);
                }
                catch (Exception ex)
                {
                    progressForm.UpdateStatus($"错误: {ex.Message}");
                    progressForm.SetCompleted(false);
                }
            });

            progressForm.ShowDialog(this);
            await progressTask;
        }

        private bool ValidateFileTypes(string[] files, HashSet<string> allowedExtensions, string fileType)
        {
            foreach (var filePath in files)
            {
                string extension = Path.GetExtension(filePath);
                if (!allowedExtensions.Contains(extension))
                {
                    MessageBox.Show($"文件 {Path.GetFileName(filePath)} 不是{fileType}文件", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private void ShowResult(bool success, string operationName)
        {
            this.Invoke(new Action(() =>
            {
                if (success)
                {
                    MessageBox.Show($"{operationName}成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"{operationName}失败，请检查文件格式是否兼容", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }));
        }
    }
}
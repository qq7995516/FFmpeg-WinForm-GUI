using System.Threading.Tasks;

namespace ffmpeg视频处理
{
    public partial class Form1 : Form
    {
        // 常用视频和音频文件扩展名列表
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
            // 检查 FFmpeg 是否可用
            if (!FFmpegDownloader.IsFFmpegInstalled())
            {
                var result = MessageBox.Show(
                    "检测到 FFmpeg 未安装或无法使用。\n\n是否现在下载安装 FFmpeg？\n\n点击\"是\"自动下载，点击\"否\"手动配置。",
                    "FFmpeg 未找到",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // 显示下载窗口
                    using (var downloadForm = new FFmpegDownloadForm())
                    {
                        downloadForm.ShowDialog(this);
                    }
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

            // 配置 FFMpegCore
            FFMpegCore.GlobalFFOptions.Configure(options =>
            {
                options.BinaryFolder = Path.Combine(Application.StartupPath, "ffmpegShared", "bin");
            });

            InitializeListViewSettings();
        }

        private void 合并视频ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取所有勾选的项目
                var checkedItems = GetCheckedItems();
                if (checkedItems.Count < 2)
                {
                    MessageBox.Show("请至少选择两个视频文件进行合并", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 检查是否都是视频文件
                foreach (var filePath in checkedItems)
                {
                    string extension = Path.GetExtension(filePath);
                    if (!_videoExtensions.Contains(extension))
                    {
                        MessageBox.Show($"文件 {Path.GetFileName(filePath)} 不是视频文件，无法进行视频合并", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // 选择保存位置
                using var saveDialog = new SaveFileDialog();

                saveDialog.Filter = "MP4 文件|*.mp4|所有文件|*.*";
                saveDialog.Title = "保存合并后的视频";
                saveDialog.DefaultExt = "mp4";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string outputPath = saveDialog.FileName;

                    // 显示进度提示
                    MessageBox.Show("开始合并视频，请稍候...", "处理中", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 执行合并操作
                    bool success = FFmpegExtensions.MergeVideos(outputPath, checkedItems.ToArray());

                    if (success)
                    {
                        MessageBox.Show("视频合并成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("视频合并失败，请检查文件格式是否兼容", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"合并视频时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 合并音频ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取所有勾选的项目
                var checkedItems = GetCheckedItems();
                if (checkedItems.Count < 2)
                {
                    MessageBox.Show("请至少选择两个音频文件进行合并", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 检查是否都是音频文件
                foreach (var filePath in checkedItems)
                {
                    string extension = Path.GetExtension(filePath);
                    if (!_audioExtensions.Contains(extension))
                    {
                        MessageBox.Show($"文件 {Path.GetFileName(filePath)} 不是音频文件，无法进行音频合并", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // 选择保存位置
                using var saveDialog = new SaveFileDialog();

                saveDialog.Filter = "MP3 文件|*.mp3|WAV 文件|*.wav|所有文件|*.*";
                saveDialog.Title = "保存合并后的音频";
                saveDialog.DefaultExt = "mp3";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string outputPath = saveDialog.FileName;

                    // 显示进度提示
                    MessageBox.Show("开始合并音频，请稍候...", "处理中", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 执行合并操作
                    bool success = FFmpegExtensions.MergeAudios(outputPath, checkedItems.ToArray());

                    if (success)
                    {
                        MessageBox.Show("音频合并成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("音频合并失败，请检查文件格式是否兼容", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"合并音频时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 合并音视频ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取所有勾选的项目
                var checkedItems = GetCheckedItems();
                if (checkedItems.Count != 2)
                {
                    MessageBox.Show("请选择一个视频文件和一个音频文件进行合并", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string videoFile = null;
                string audioFile = null;

                // 确定哪个是视频文件，哪个是音频文件
                foreach (var filePath in checkedItems)
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

                // 选择保存位置
                using var saveDialog = new SaveFileDialog();

                saveDialog.Filter = "MP4 文件|*.mp4|所有文件|*.*";
                saveDialog.Title = "保存合并后的视频";
                saveDialog.DefaultExt = "mp4";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string outputPath = saveDialog.FileName;

                    // 显示进度提示
                    MessageBox.Show("开始合并音视频，请稍候...", "处理中", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 执行合并操作
                    bool success = FFmpegExtensions.MergeAudioVideo(videoFile, audioFile, outputPath, true);

                    if (success)
                    {
                        MessageBox.Show("音视频合并成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("音视频合并失败，请检查文件格式是否兼容", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"合并音视频时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 删除勾选项ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 从后向前删除选中项，避免索引变化问题
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
                // 确认是否清空
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
                // 创建打开文件对话框
                using var openFileDialog = new OpenFileDialog();

                // 设置对话框属性
                openFileDialog.Title = "选择要添加的文件";
                openFileDialog.Filter = "媒体文件|*.mp4;*.mkv;*.avi;*.mov;*.wmv;*.flv;*.webm;*.mpeg;*.mpg;*.mp3;*.wav;*.aac;*.m4a;*.ogg;*.wma;*.flac|视频文件|*.mp4;*.mkv;*.avi;*.mov;*.wmv;*.flv;*.webm;*.mpeg;*.mpg|音频文件|*.mp3;*.wav;*.aac;*.m4a;*.ogg;*.wma;*.flac|所有文件|*.*";
                openFileDialog.Multiselect = true; // 允许选择多个文件

                // 显示对话框并检查用户是否点击了"确定"
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // 处理选择的文件
                    foreach (string filePath in openFileDialog.FileNames)
                    {
                        // 获取文件名
                        string fileName = Path.GetFileName(filePath);

                        // 创建新的列表项
                        ListViewItem item = new ListViewItem(fileName);
                        item.SubItems.Add(filePath); // 添加文件路径作为第二列
                        item.Tag = filePath; // 存储完整路径在Tag中以便后续使用

                        // 添加到ListView
                        listViewFiles.Items.Add(item);
                    }

                    // 如果添加了文件，显示成功消息
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

        // 辅助方法：获取所有勾选的文件路径
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
            // 1. 设置 ListView 的 View 属性为 Details 以显示列
            listViewFiles.View = View.Details;

            // 2. 清除可能已存在的旧列（可选，如果您想确保从头开始）
            // listViewFiles.Columns.Clear();
            // 3. 添加列标题 (如果尚未在设计器中添加)
            //    检查以避免重复添加（如果此方法可能被多次调用或设计器已添加列）
            if (listViewFiles.Columns.Count == 0)
            {
                listViewFiles.Columns.Add("文件名", 200, HorizontalAlignment.Left); // 参数：文本，宽度，对齐方式
                listViewFiles.Columns.Add("文件路径", 400, HorizontalAlignment.Left);
            }
            else
            {
                // 如果列已存在，可以考虑更新它们的文本（如果需要）
                // listViewFiles.Columns[0].Text = "文件名";
                // listViewFiles.Columns[1].Text = "文件路径";
            }

            // 4. 允许多选（可选）
            listViewFiles.MultiSelect = true;

            // 5. 显示网格线（可选，更易读）
            listViewFiles.GridLines = true;

            // 6. 整行选择（可选，更美观）
            listViewFiles.FullRowSelect = true;

            // 允许拖拽
            listViewFiles.AllowDrop = true;
        }

        private void 提取音频ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取所有勾选的项目
                var checkedItems = GetCheckedItems();
                if (checkedItems.Count == 0)
                {
                    MessageBox.Show("请选择至少一个视频文件进行音频提取", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 检查是否都是视频文件
                foreach (var filePath in checkedItems)
                {
                    string extension = Path.GetExtension(filePath);
                    if (!_videoExtensions.Contains(extension))
                    {
                        MessageBox.Show($"文件 {Path.GetFileName(filePath)} 不是视频文件，无法提取音频", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // 选择保存文件夹
                using var folderDialog = new FolderBrowserDialog();
                folderDialog.Description = "选择音频文件保存位置";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string outputFolder = folderDialog.SelectedPath;
                    int successCount = 0;
                    int failCount = 0;

                    // 显示进度提示
                    MessageBox.Show($"开始提取 {checkedItems.Count} 个视频的音频，请稍候...", "处理中", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 逐个提取音频
                    foreach (var videoFile in checkedItems)
                    {
                        try
                        {
                            string fileName = Path.GetFileNameWithoutExtension(videoFile);
                            string audioOutput = Path.Combine(outputFolder, $"{fileName}.mp3");

                            // 执行提取操作
                            bool success = FFmpegExtensions.ExtractAudio(videoFile, audioOutput);

                            if (success)
                                successCount++;
                            else
                                failCount++;
                        }
                        catch
                        {
                            failCount++;
                        }
                    }

                    // 显示结果
                    string message = $"音频提取完成！\n成功: {successCount} 个\n失败: {failCount} 个";
                    MessageBox.Show(message, "完成", MessageBoxButtons.OK,
                        failCount == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"提取音频时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 提取视频ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取所有勾选的项目
                var checkedItems = GetCheckedItems();
                if (checkedItems.Count == 0)
                {
                    MessageBox.Show("请选择至少一个视频文件进行视频提取（去除音频）", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 检查是否都是视频文件
                foreach (var filePath in checkedItems)
                {
                    string extension = Path.GetExtension(filePath);
                    if (!_videoExtensions.Contains(extension))
                    {
                        MessageBox.Show($"文件 {Path.GetFileName(filePath)} 不是视频文件，无法提取视频", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // 选择保存文件夹
                using var folderDialog = new FolderBrowserDialog();
                folderDialog.Description = "选择视频文件保存位置";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string outputFolder = folderDialog.SelectedPath;
                    int successCount = 0;
                    int failCount = 0;

                    // 显示进度提示
                    MessageBox.Show($"开始提取 {checkedItems.Count} 个视频（去除音频），请稍候...", "处理中", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 逐个提取视频
                    foreach (var videoFile in checkedItems)
                    {
                        try
                        {
                            string fileName = Path.GetFileNameWithoutExtension(videoFile);
                            string extension = Path.GetExtension(videoFile);
                            string videoOutput = Path.Combine(outputFolder, $"{fileName}_no_audio{extension}");

                            // 执行提取操作
                            bool success = FFmpegExtensions.ExtractVideo(videoFile, videoOutput);

                            if (success)
                                successCount++;
                            else
                                failCount++;
                        }
                        catch
                        {
                            failCount++;
                        }
                    }

                    // 显示结果
                    string message = $"视频提取完成！\n成功: {successCount} 个\n失败: {failCount} 个";
                    MessageBox.Show(message, "完成", MessageBoxButtons.OK,
                        failCount == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"提取视频时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}

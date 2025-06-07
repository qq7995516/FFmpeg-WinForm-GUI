using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ffmpeg视频处理
{
    /// <summary>
    /// 下载 FFmpeg 的 Windows 窗体界面
    /// </summary>
    public partial class FFmpegDownloadForm : Form
    {
        private ProgressBar progressBar;
        private Label statusLabel;
        private Button downloadButton;
        private Button cancelButton;
        private bool isDownloading = false;

        public FFmpegDownloadForm()
        {
            InitializeComponent();
            CheckFFmpegStatus();
        }

        private void InitializeComponent()
        {
            this.Text = "FFmpeg 下载器";
            this.Size = new System.Drawing.Size(500, 200);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // 状态标签
            statusLabel = new Label();
            statusLabel.Text = "检查 FFmpeg 状态...";
            statusLabel.Location = new System.Drawing.Point(20, 20);
            statusLabel.Size = new System.Drawing.Size(460, 40);
            statusLabel.AutoSize = false;
            this.Controls.Add(statusLabel);

            // 进度条
            progressBar = new ProgressBar();
            progressBar.Location = new System.Drawing.Point(20, 70);
            progressBar.Size = new System.Drawing.Size(460, 23);
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.Visible = false;
            this.Controls.Add(progressBar);

            // 下载按钮
            downloadButton = new Button();
            downloadButton.Text = "下载 FFmpeg";
            downloadButton.Location = new System.Drawing.Point(300, 120);
            downloadButton.Size = new System.Drawing.Size(80, 30);
            downloadButton.Click += DownloadButton_Click;
            this.Controls.Add(downloadButton);

            // 取消按钮
            cancelButton = new Button();
            cancelButton.Text = "取消";
            cancelButton.Location = new System.Drawing.Point(390, 120);
            cancelButton.Size = new System.Drawing.Size(80, 30);
            cancelButton.Click += (s, e) => this.Close();
            this.Controls.Add(cancelButton);
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckFFmpegStatus()
        {
            if (FFmpegDownloader.IsFFmpegInstalled())
            {
                string version = FFmpegDownloader.GetFFmpegVersion();
                statusLabel.Text = $"FFmpeg 已安装 (版本: {version})\n可以正常使用视频处理功能。";
                downloadButton.Text = "重新下载";
            }
            else
            {
                statusLabel.Text = "FFmpeg 未安装或无法使用\n需要下载 FFmpeg 才能使用视频处理功能。";
                downloadButton.Text = "下载 FFmpeg";
            }
        }

        private async void DownloadButton_Click(object sender, EventArgs e)
        {
            if (isDownloading) return;

            isDownloading = true;
            downloadButton.Enabled = false;
            progressBar.Visible = true;

            var progress = new Progress<string>(message =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => statusLabel.Text = message));
                }
                else
                {
                    statusLabel.Text = message;
                }
            });

            try
            {
                bool success = await FFmpegDownloader.DownloadAndInstallFFmpegAsync(progress);

                if (success)
                {
                    MessageBox.Show("FFmpeg 下载安装成功！", "成功",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CheckFFmpegStatus();
                }
                else
                {
                    MessageBox.Show("FFmpeg 下载安装失败，请检查网络连接或手动下载。", "失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"下载过程中发生错误: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isDownloading = false;
                downloadButton.Enabled = true;
                progressBar.Visible = false;
            }
        }
    }
}
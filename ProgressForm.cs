using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ffmpeg视频处理
{
    public partial class ProgressForm : Form
    {
        private ProgressBar progressBar;
        private Label statusLabel;
        private Label titleLabel;
        private Button cancelButton;
        private CancellationTokenSource _cancellationTokenSource;

        public CancellationToken CancellationToken => _cancellationTokenSource.Token;

        public ProgressForm(string title)
        {
            InitializeComponent();
            titleLabel.Text = title;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(450, 180);
            this.Text = "处理进度";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;

            // 标题
            titleLabel = new Label();
            titleLabel.Text = "正在处理...";
            titleLabel.Location = new Point(20, 20);
            titleLabel.Size = new Size(400, 25);
            titleLabel.Font = new Font("微软雅黑", 10F, FontStyle.Bold);
            this.Controls.Add(titleLabel);

            // 状态标签
            statusLabel = new Label();
            statusLabel.Text = "准备开始...";
            statusLabel.Location = new Point(20, 50);
            statusLabel.Size = new Size(400, 40);
            statusLabel.AutoSize = false;
            this.Controls.Add(statusLabel);

            // 进度条
            progressBar = new ProgressBar();
            progressBar.Location = new Point(20, 95);
            progressBar.Size = new Size(400, 23);
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.MarqueeAnimationSpeed = 50;
            this.Controls.Add(progressBar);

            // 取消按钮
            cancelButton = new Button();
            cancelButton.Text = "取消";
            cancelButton.Location = new Point(345, 125);
            cancelButton.Size = new Size(75, 30);
            cancelButton.Click += CancelButton_Click;
            this.Controls.Add(cancelButton);
        }

        public void UpdateStatus(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateStatus), message);
                return;
            }

            statusLabel.Text = message;
        }

        public void SetProgress(int current, int total)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<int, int>(SetProgress), current, total);
                return;
            }

            if (total > 0)
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Maximum = total;
                progressBar.Value = Math.Min(current, total);
            }
        }

        public void SetCompleted(bool success)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<bool>(SetCompleted), success);
                return;
            }

            cancelButton.Text = "关闭";
            progressBar.Value = progressBar.Maximum;

            if (success)
            {
                statusLabel.Text = "处理完成！";
                progressBar.Style = ProgressBarStyle.Continuous;
            }
            else
            {
                statusLabel.Text = "处理失败！";
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (cancelButton.Text == "取消")
            {
                _cancellationTokenSource.Cancel();
                cancelButton.Enabled = false;
                statusLabel.Text = "正在取消...";
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (cancelButton.Text == "取消")
            {
                e.Cancel = true;
                CancelButton_Click(null, null);
            }
            else
            {
                _cancellationTokenSource?.Dispose();
            }
            base.OnFormClosing(e);
        }
    }
}
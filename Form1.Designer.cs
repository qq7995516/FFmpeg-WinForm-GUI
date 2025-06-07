namespace ffmpeg视频处理
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            listViewFiles = new ListView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            合并视频ToolStripMenuItem = new ToolStripMenuItem();
            合并音频ToolStripMenuItem = new ToolStripMenuItem();
            合并音视频ToolStripMenuItem = new ToolStripMenuItem();
            删除勾选项ToolStripMenuItem = new ToolStripMenuItem();
            清空ToolStripMenuItem = new ToolStripMenuItem();
            添加文件ToolStripMenuItem = new ToolStripMenuItem();
            提取ToolStripMenuItem = new ToolStripMenuItem();
            音频ToolStripMenuItem = new ToolStripMenuItem();
            视频ToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // listViewFiles
            // 
            listViewFiles.CheckBoxes = true;
            listViewFiles.ContextMenuStrip = contextMenuStrip1;
            listViewFiles.Location = new Point(12, 12);
            listViewFiles.Name = "listViewFiles";
            listViewFiles.Size = new Size(776, 362);
            listViewFiles.TabIndex = 0;
            listViewFiles.UseCompatibleStateImageBehavior = false;
            listViewFiles.View = View.Details;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { 合并视频ToolStripMenuItem, 合并音频ToolStripMenuItem, 合并音视频ToolStripMenuItem, 删除勾选项ToolStripMenuItem, 清空ToolStripMenuItem, 添加文件ToolStripMenuItem, 提取ToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(181, 194);
            // 
            // 合并视频ToolStripMenuItem
            // 
            合并视频ToolStripMenuItem.Name = "合并视频ToolStripMenuItem";
            合并视频ToolStripMenuItem.Size = new Size(180, 24);
            合并视频ToolStripMenuItem.Text = "合并视频";
            合并视频ToolStripMenuItem.Click += 合并视频ToolStripMenuItem_Click;
            // 
            // 合并音频ToolStripMenuItem
            // 
            合并音频ToolStripMenuItem.Name = "合并音频ToolStripMenuItem";
            合并音频ToolStripMenuItem.Size = new Size(180, 24);
            合并音频ToolStripMenuItem.Text = "合并音频";
            合并音频ToolStripMenuItem.Click += 合并音频ToolStripMenuItem_Click;
            // 
            // 合并音视频ToolStripMenuItem
            // 
            合并音视频ToolStripMenuItem.Name = "合并音视频ToolStripMenuItem";
            合并音视频ToolStripMenuItem.Size = new Size(180, 24);
            合并音视频ToolStripMenuItem.Text = "合并音视频";
            合并音视频ToolStripMenuItem.Click += 合并音视频ToolStripMenuItem_Click;
            // 
            // 删除勾选项ToolStripMenuItem
            // 
            删除勾选项ToolStripMenuItem.Name = "删除勾选项ToolStripMenuItem";
            删除勾选项ToolStripMenuItem.Size = new Size(180, 24);
            删除勾选项ToolStripMenuItem.Text = "删除勾选项";
            删除勾选项ToolStripMenuItem.Click += 删除勾选项ToolStripMenuItem_Click;
            // 
            // 清空ToolStripMenuItem
            // 
            清空ToolStripMenuItem.Name = "清空ToolStripMenuItem";
            清空ToolStripMenuItem.Size = new Size(180, 24);
            清空ToolStripMenuItem.Text = "清空";
            清空ToolStripMenuItem.Click += 清空ToolStripMenuItem_Click;
            // 
            // 添加文件ToolStripMenuItem
            // 
            添加文件ToolStripMenuItem.Name = "添加文件ToolStripMenuItem";
            添加文件ToolStripMenuItem.Size = new Size(180, 24);
            添加文件ToolStripMenuItem.Text = "添加文件";
            添加文件ToolStripMenuItem.Click += 添加文件ToolStripMenuItem_Click;
            // 
            // 提取ToolStripMenuItem
            // 
            提取ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 音频ToolStripMenuItem, 视频ToolStripMenuItem });
            提取ToolStripMenuItem.Name = "提取ToolStripMenuItem";
            提取ToolStripMenuItem.Size = new Size(180, 24);
            提取ToolStripMenuItem.Text = "提取";
            // 
            // 音频ToolStripMenuItem
            // 
            音频ToolStripMenuItem.Name = "音频ToolStripMenuItem";
            音频ToolStripMenuItem.Size = new Size(180, 24);
            音频ToolStripMenuItem.Text = "提取音频";
            音频ToolStripMenuItem.Click += 提取音频ToolStripMenuItem_Click;
            // 
            // 视频ToolStripMenuItem
            // 
            视频ToolStripMenuItem.Name = "视频ToolStripMenuItem";
            视频ToolStripMenuItem.Size = new Size(180, 24);
            视频ToolStripMenuItem.Text = "提取视频";
            视频ToolStripMenuItem.Click += 提取视频ToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 386);
            Controls.Add(listViewFiles);
            Name = "Form1";
            Text = "ffmpeg视频处理";
            Load += Form1_Load;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ListView listViewFiles;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem 合并视频ToolStripMenuItem;
        private ToolStripMenuItem 合并音频ToolStripMenuItem;
        private ToolStripMenuItem 合并音视频ToolStripMenuItem;
        private ToolStripMenuItem 删除勾选项ToolStripMenuItem;
        private ToolStripMenuItem 清空ToolStripMenuItem;
        private ToolStripMenuItem 添加文件ToolStripMenuItem;
        private ToolStripMenuItem 提取ToolStripMenuItem;
        private ToolStripMenuItem 音频ToolStripMenuItem;
        private ToolStripMenuItem 视频ToolStripMenuItem;
    }
}

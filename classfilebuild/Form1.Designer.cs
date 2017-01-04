namespace classfilebuild
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.mcuhead = new System.Windows.Forms.CheckBox();
            this.ListImage = new System.Windows.Forms.ImageList(this.components);
            this.FileNameBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.AllowDrop = true;
            this.button1.Location = new System.Drawing.Point(108, 106);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(67, 24);
            this.button1.TabIndex = 0;
            this.button1.Text = "生成";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.DragDrop += new System.Windows.Forms.DragEventHandler(this.button1_DragDrop);
            this.button1.DragEnter += new System.Windows.Forms.DragEventHandler(this.button1_DragEnter);
            this.button1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.exit);
            // 
            // mcuhead
            // 
            this.mcuhead.AutoSize = true;
            this.mcuhead.Checked = true;
            this.mcuhead.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mcuhead.Location = new System.Drawing.Point(12, 111);
            this.mcuhead.Name = "mcuhead";
            this.mcuhead.Size = new System.Drawing.Size(90, 16);
            this.mcuhead.TabIndex = 1;
            this.mcuhead.Text = "生成mcuhead";
            this.mcuhead.UseVisualStyleBackColor = true;
            // 
            // ListImage
            // 
            this.ListImage.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ListImage.ImageStream")));
            this.ListImage.TransparentColor = System.Drawing.Color.Transparent;
            this.ListImage.Images.SetKeyName(0, "QQ截图20160418160518.jpg");
            // 
            // FileNameBox
            // 
            this.FileNameBox.Location = new System.Drawing.Point(75, 6);
            this.FileNameBox.Name = "FileNameBox";
            this.FileNameBox.Size = new System.Drawing.Size(100, 21);
            this.FileNameBox.TabIndex = 3;
            this.FileNameBox.TextChanged += new System.EventHandler(this.FileNameBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "文件名称:";
            // 
            // treeView1
            // 
            this.treeView1.Font = new System.Drawing.Font("宋体", 24F);
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.ListImage;
            this.treeView1.Location = new System.Drawing.Point(12, 33);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.ShowLines = false;
            this.treeView1.ShowPlusMinus = false;
            this.treeView1.ShowRootLines = false;
            this.treeView1.Size = new System.Drawing.Size(163, 67);
            this.treeView1.TabIndex = 5;
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listView1_ItemDrag);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.button1_DragDrop);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(182, 135);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FileNameBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mcuhead);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "C语言文件生成器";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.exit);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox mcuhead;
        private System.Windows.Forms.ImageList ListImage;
        private System.Windows.Forms.TextBox FileNameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView treeView1;
    }
}


namespace Project
{
    partial class FrmGame
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGame));
            this.label1 = new System.Windows.Forms.Label();
            this.txtLevel = new System.Windows.Forms.TextBox();
            this.btnTaoMap = new System.Windows.Forms.Button();
            this.tmDiChuyen = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(404, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nhập Level";
            // 
            // txtLevel
            // 
            this.txtLevel.Location = new System.Drawing.Point(516, 18);
            this.txtLevel.Name = "txtLevel";
            this.txtLevel.Size = new System.Drawing.Size(100, 22);
            this.txtLevel.TabIndex = 1;
            this.txtLevel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLevel_KeyDown);
            // 
            // btnTaoMap
            // 
            this.btnTaoMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTaoMap.ForeColor = System.Drawing.Color.Maroon;
            this.btnTaoMap.Location = new System.Drawing.Point(639, 12);
            this.btnTaoMap.Name = "btnTaoMap";
            this.btnTaoMap.Size = new System.Drawing.Size(112, 31);
            this.btnTaoMap.TabIndex = 2;
            this.btnTaoMap.Text = "Tạo Map";
            this.btnTaoMap.UseVisualStyleBackColor = true;
            this.btnTaoMap.Click += new System.EventHandler(this.btnTaoMap_Click);
            // 
            // tmDiChuyen
            // 
            this.tmDiChuyen.Interval = 500;
            this.tmDiChuyen.Tick += new System.EventHandler(this.tmDiChuyen_Tick);
            // 
            // FrmGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 753);
            this.Controls.Add(this.btnTaoMap);
            this.Controls.Add(this.txtLevel);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmGame";
            this.Text = "GameRobotLoop";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLevel;
        private System.Windows.Forms.Button btnTaoMap;
        private System.Windows.Forms.Timer tmDiChuyen;
    }
}


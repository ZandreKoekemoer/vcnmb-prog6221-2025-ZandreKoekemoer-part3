namespace CyberBotGUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            txtInput = new TextBox();
            btnSend = new Button();
            rtbChat = new RichTextBox();
            lblTitle = new Label();
            SuspendLayout();
            // 
            // txtInput
            // 
            txtInput.Location = new Point(50, 400);
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(520, 23);
            txtInput.TabIndex = 0;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(590, 400);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(75, 23);
            btnSend.TabIndex = 1;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // rtbChat
            // 
            rtbChat.Location = new Point(50, 120);
            rtbChat.Name = "rtbChat";
            rtbChat.ReadOnly = true;
            rtbChat.Size = new Size(615, 260);
            rtbChat.TabIndex = 2;
            rtbChat.Text = "";
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 18F);
            lblTitle.Location = new Point(50, 30);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(615, 60);
            lblTitle.TabIndex = 3;
            lblTitle.Text = "Cybersecurity Awareness Bot";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Click += lblTitle_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(720, 460);
            Controls.Add(lblTitle);
            Controls.Add(rtbChat);
            Controls.Add(btnSend);
            Controls.Add(txtInput);
            Name = "Form1";
            Text = "CyberBot GUI";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RichTextBox rtbChat;
        private System.Windows.Forms.Label lblTitle;
    }
}

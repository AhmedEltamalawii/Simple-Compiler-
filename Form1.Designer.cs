namespace compiler
{
    partial class Form1
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
            this.lexerButton = new System.Windows.Forms.Button();
            this.parserButton = new System.Windows.Forms.Button();
            this.codeTextBox = new System.Windows.Forms.TextBox();
            this.textLabel = new System.Windows.Forms.Label();
            this.tokensListBox = new System.Windows.Forms.ListBox();
            this.IdentifiersListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lexerButton
            // 
            this.lexerButton.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lexerButton.Location = new System.Drawing.Point(158, 248);
            this.lexerButton.Name = "lexerButton";
            this.lexerButton.Size = new System.Drawing.Size(126, 35);
            this.lexerButton.TabIndex = 0;
            this.lexerButton.Text = "Start Scan";
            this.lexerButton.UseVisualStyleBackColor = true;
            this.lexerButton.Click += new System.EventHandler(this.lexerButton_Click);
            // 
            // parserButton
            // 
            this.parserButton.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.parserButton.Location = new System.Drawing.Point(373, 294);
            this.parserButton.Name = "parserButton";
            this.parserButton.Size = new System.Drawing.Size(126, 35);
            this.parserButton.TabIndex = 1;
            this.parserButton.Text = "Start parse";
            this.parserButton.UseVisualStyleBackColor = true;
            this.parserButton.Click += new System.EventHandler(this.parserButton_Click);
            // 
            // codeTextBox
            // 
            this.codeTextBox.Location = new System.Drawing.Point(196, 12);
            this.codeTextBox.Multiline = true;
            this.codeTextBox.Name = "codeTextBox";
            this.codeTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.codeTextBox.Size = new System.Drawing.Size(303, 184);
            this.codeTextBox.TabIndex = 2;
            // 
            // textLabel
            // 
            this.textLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textLabel.Location = new System.Drawing.Point(30, 82);
            this.textLabel.Name = "textLabel";
            this.textLabel.Size = new System.Drawing.Size(149, 48);
            this.textLabel.TabIndex = 3;
            this.textLabel.Text = "Enter Your Code";
            this.textLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tokensListBox
            // 
            this.tokensListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tokensListBox.FormattingEnabled = true;
            this.tokensListBox.ItemHeight = 18;
            this.tokensListBox.Location = new System.Drawing.Point(573, 12);
            this.tokensListBox.Name = "tokensListBox";
            this.tokensListBox.Size = new System.Drawing.Size(353, 184);
            this.tokensListBox.TabIndex = 4;
            // 
            // IdentifiersListBox
            // 
            this.IdentifiersListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IdentifiersListBox.FormattingEnabled = true;
            this.IdentifiersListBox.ItemHeight = 18;
            this.IdentifiersListBox.Location = new System.Drawing.Point(573, 236);
            this.IdentifiersListBox.Name = "IdentifiersListBox";
            this.IdentifiersListBox.Size = new System.Drawing.Size(215, 130);
            this.IdentifiersListBox.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1010, 470);
            this.Controls.Add(this.IdentifiersListBox);
            this.Controls.Add(this.tokensListBox);
            this.Controls.Add(this.textLabel);
            this.Controls.Add(this.codeTextBox);
            this.Controls.Add(this.parserButton);
            this.Controls.Add(this.lexerButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button lexerButton;
        private System.Windows.Forms.Button parserButton;
        private System.Windows.Forms.TextBox codeTextBox;
        private System.Windows.Forms.Label textLabel;
        private System.Windows.Forms.ListBox tokensListBox;
        private System.Windows.Forms.ListBox IdentifiersListBox;
    }
}


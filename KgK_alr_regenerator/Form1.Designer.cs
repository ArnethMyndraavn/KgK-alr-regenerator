namespace KgK_alr_regenerator
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
            input_button = new Button();
            output_button = new Button();
            regen_button = new Button();
            input_path = new RichTextBox();
            output_path = new RichTextBox();
            progress_bar = new ProgressBar();
            title = new Label();
            label1 = new Label();
            input_dialog = new FolderBrowserDialog();
            output_dialog = new FolderBrowserDialog();
            SuspendLayout();
            // 
            // input_button
            // 
            input_button.Location = new Point(588, 58);
            input_button.Name = "input_button";
            input_button.Size = new Size(200, 50);
            input_button.TabIndex = 0;
            input_button.Text = "Script and alr directory";
            input_button.UseVisualStyleBackColor = true;
            input_button.Click += input_button_Click;
            // 
            // output_button
            // 
            output_button.Location = new Point(588, 114);
            output_button.Name = "output_button";
            output_button.Size = new Size(200, 50);
            output_button.TabIndex = 1;
            output_button.Text = "Output directory";
            output_button.UseVisualStyleBackColor = true;
            output_button.Click += output_button_Click;
            // 
            // regen_button
            // 
            regen_button.Enabled = false;
            regen_button.Location = new Point(588, 239);
            regen_button.Name = "regen_button";
            regen_button.Size = new Size(200, 75);
            regen_button.TabIndex = 2;
            regen_button.Text = "Regenerate alr files";
            regen_button.UseVisualStyleBackColor = true;
            regen_button.Click += regen_button_Click;
            // 
            // input_path
            // 
            input_path.Enabled = false;
            input_path.Location = new Point(12, 58);
            input_path.Name = "input_path";
            input_path.Size = new Size(570, 50);
            input_path.TabIndex = 3;
            input_path.Text = "";
            // 
            // output_path
            // 
            output_path.Enabled = false;
            output_path.Location = new Point(12, 114);
            output_path.Name = "output_path";
            output_path.Size = new Size(570, 50);
            output_path.TabIndex = 4;
            output_path.Text = "";
            // 
            // progress_bar
            // 
            progress_bar.Location = new Point(12, 239);
            progress_bar.Name = "progress_bar";
            progress_bar.Size = new Size(570, 75);
            progress_bar.TabIndex = 5;
            // 
            // title
            // 
            title.AutoSize = true;
            title.BackColor = Color.Transparent;
            title.Font = new Font("Segoe UI", 30F);
            title.ForeColor = Color.Black;
            title.Location = new Point(119, 1);
            title.Name = "title";
            title.Size = new Size(507, 54);
            title.TabIndex = 6;
            title.Text = "Ken ga Kimi alr regenerator";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 20F);
            label1.Location = new Point(190, 199);
            label1.Name = "label1";
            label1.Size = new Size(284, 37);
            label1.TabIndex = 7;
            label1.Text = "Regeneration progress";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 326);
            Controls.Add(label1);
            Controls.Add(title);
            Controls.Add(progress_bar);
            Controls.Add(output_path);
            Controls.Add(input_path);
            Controls.Add(regen_button);
            Controls.Add(output_button);
            Controls.Add(input_button);
            Name = "Form1";
            Text = "Ken ga Kimi alr regenerator";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button input_button;
        private Button output_button;
        private Button regen_button;
        private RichTextBox input_path;
        private RichTextBox output_path;
        private ProgressBar progress_bar;
        private Label title;
        private Label label1;
        private FolderBrowserDialog input_dialog;
        private FolderBrowserDialog output_dialog;
    }
}

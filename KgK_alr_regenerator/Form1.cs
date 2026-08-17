using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace KgK_alr_regenerator
{
    public partial class Form1 : Form
    {
        bool hasInputDirectory = false;
        bool hasOutputDirectory = false;

        string inputDirectory = string.Empty;
        string outputDirectory = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void input_button_Click(object sender, EventArgs e)
        {
            input_dialog.ShowDialog();
            string inputFolder = input_dialog.SelectedPath;
            if (inputFolder != null && Directory.Exists(inputFolder))
            {
                inputDirectory = inputFolder;
                input_path.Text = inputFolder;
                hasInputDirectory = true;
                enable_regen_button();
            }
        }

        private void output_button_Click(object sender, EventArgs e)
        {
            output_dialog.ShowDialog();
            string outputFolder = output_dialog.SelectedPath;
            if (outputFolder != null)
            {
                if (!Directory.Exists(outputFolder))
                {
                    
                    Directory.CreateDirectory(outputFolder);
                }
                outputDirectory = outputFolder;
                output_path.Text = outputFolder;
                hasOutputDirectory = true;
                enable_regen_button();
            }
        }

        private void regen_button_Click(object sender, EventArgs e)
        {
            
            AlrRegen.AlrRegenerate(inputDirectory, outputDirectory, progress_bar);
            progress_bar.Value = 100;
        }

        private void enable_regen_button()
        {
            if (hasInputDirectory && hasOutputDirectory)
            {
                regen_button.Enabled = true;
            }
            else
            {
                regen_button.Enabled = false;
            }
        }
    }
}

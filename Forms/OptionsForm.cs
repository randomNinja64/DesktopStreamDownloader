using System;
using System.Windows.Forms;

namespace DesktopStreamDownloader
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            qualityComboBox.Text = Properties.Settings.Default.DefaultQuality;
            concurrentNum.Value = ClampConcurrent(Properties.Settings.Default.MaxConcurrentDownloads);
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DefaultQuality = qualityComboBox.Text;
            Properties.Settings.Default.MaxConcurrentDownloads = (int)concurrentNum.Value;
            Properties.Settings.Default.Save();

            this.Close();
        }

        private static decimal ClampConcurrent(int value)
        {
            if (value < 1)
            {
                return 1;
            }
            if (value > 5)
            {
                return 5;
            }
            return value;
        }
    }
}

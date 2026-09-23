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
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DefaultQuality = qualityComboBox.Text;
            Properties.Settings.Default.Save();

            this.Close();
        }
    }
}

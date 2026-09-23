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
            // Load settings from user settings into the dropdown and text box
            instanceTxtBox.Text = Properties.Settings.Default.InvidiousInstance;
            qualityComboBox.Text = Properties.Settings.Default.DefaultQuality;

            // If TLS12 is true, check the checkbox
            if (Properties.Settings.Default.TLS12 == true)
            {
                tlsToggle.Checked = true;
            }
            else
            {
                tlsToggle.Checked = false;
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            // If there is a '/' at the end of the Invidious instance, remove it
            while (instanceTxtBox.Text.EndsWith("/"))
            {
                instanceTxtBox.Text = instanceTxtBox.Text.Remove(instanceTxtBox.Text.Length - 1);
            }

            // Save items in dropdown and text box to user settings
            Properties.Settings.Default.InvidiousInstance = instanceTxtBox.Text;
            Properties.Settings.Default.DefaultQuality = qualityComboBox.Text;
            // Save value of tlsToggle to TLS12
            Properties.Settings.Default.TLS12 = tlsToggle.Checked;
            Properties.Settings.Default.Save();

            // Close form
            this.Close();
        }

        private void instanceTxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            //If enter key is pressed, run okBtn_Click
            if (e.KeyCode == Keys.Enter)
            {
                okBtn_Click(sender, e);
            }
        }
    }
}

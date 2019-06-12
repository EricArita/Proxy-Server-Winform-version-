using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1712169
{
    public partial class FormBlackList : Form
    {
        public FormBlackList()
        {
            InitializeComponent();
        }

        private void FormBlackList_Load(object sender, EventArgs e)
        {
            dgrvBlackList.Columns.Add("Index", "ID");
            dgrvBlackList.Columns.Add("DomainName", "Domain name to block");
            dgrvBlackList.Columns["Index"].Width = 80;
            dgrvBlackList.Columns["DomainName"].Width = dgrvBlackList.Width - 80;
            dgrvBlackList.EnableHeadersVisualStyles = false;
            dgrvBlackList.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            int index = 0;
            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                index++;
                dgrvBlackList.Rows.Add(index, ConfigurationManager.AppSettings[key].ToString());
            }
        }

        private static int index = 0;

        private void btnAdd_Click(object sender, EventArgs e)
        {            
            string value = txtDomainName.Text;
            string key;
            if (value.Contains("www"))
            {
                value = value.Substring(value.IndexOf('.') + 1);
                key = value;
            }
            else
            {
                key = value;
            }
           
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            index++;
            dgrvBlackList.Rows.Add(index, txtDomainName.Text);                     
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string value = dgrvBlackList.CurrentRow.Cells[1].Value.ToString();
            string key = value;

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(key);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            dgrvBlackList.Rows.Remove(dgrvBlackList.CurrentRow);
            index--;

            int cnt = 0;
            foreach(DataGridViewRow row in dgrvBlackList.Rows)
            {
                cnt++;
                row.Cells[0].Value = cnt.ToString();
            }
        }
    }
}

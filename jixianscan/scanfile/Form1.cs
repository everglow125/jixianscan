using scanlogic;
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

namespace scanfile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitControl();
        }
        private void InitControl()
        {
            this.lbl_sourcePath.Text = ConfigurationManager.AppSettings["sourcePath"];
            this.lbl_targetPath.Text = ConfigurationManager.AppSettings["targetPath"];
        }

        private void btn_scan_Click(object sender, EventArgs e)
        {
            var scanDate = this.dtp_scanDate.Value;
            var sourcePath = ConfigurationManager.AppSettings["sourcePath"];
            var list = new ScanFileLogic().Excute(sourcePath, scanDate);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            var targetPath = ConfigurationManager.AppSettings["targetPath"];
            new SaveFileLogic().Excute(targetPath, new DataTable());
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            var targetPath = ConfigurationManager.AppSettings["targetPath"];
            new OpenFolderLogic().OpenFolder(targetPath);
        }

    }
}

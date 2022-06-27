using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;

namespace CFS_Latam_cashApplicationTool
{
    public partial class FrmSearchCustomerName : Form
    {
        public FrmSearchCustomerName()
        {
            InitializeComponent();
        }

        private void SearchCustomerName_Load(object sender, EventArgs e)
        {
            txtLettersFind.Text = "Please type here customer name...";
        }
    }
}

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
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace CFS_Latam_cashApplicationTool
{
    public partial class FrmSearchCustomerName : Form
    {
        //Conexión GLOBAL a Data Set
        DsFbl5n ds = new DsFbl5n();
        public FrmSearchCustomerName()
        {
            InitializeComponent();

        }
      
        private void FrmSearchCustomerName_Load(object sender, EventArgs e)
        {
            //Carga combobox de company codes
            fillCompanyCodes();
            fillPlaceholderTxtBox();
        }

        private void txtLettersFind_Enter(object sender, EventArgs e)
        {
            if (txtLettersFind.Text == "Type here customer name...")
            {
                txtLettersFind.Text = "";
                txtLettersFind.ForeColor = Color.Black;
            }
        }

        // Método que deja placeholder en textbox para filtro por customer name
        private void txtLettersFind_Leave(object sender, EventArgs e)
        {
            fillPlaceholderTxtBox();
        }

        // Método que llena combobox de compay codes
        void fillCompanyCodes()
        {
            DsFbl5nTableAdapters.SP_COMPANYCODESTableAdapter daCoCd = new DsFbl5nTableAdapters.SP_COMPANYCODESTableAdapter();
            daCoCd.Fill(ds.SP_COMPANYCODES);
            cboCoCdSearch.DataSource = ds.SP_COMPANYCODES;
            cboCoCdSearch.DisplayMember = "Company Code";
            cboCoCdSearch.ValueMember = "Company Code";
            cboCoCdSearch.Text = "-- Select Company --";       
            cboCoCdSearch.ForeColor = Color.Black;            
        }

        // Método que permite que el usuario ingrese caracteres que muestren los customers name
        private void txtLettersFind_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtLettersFind.Text != String.Empty && cboCoCdSearch.SelectedIndex != -1)
                {                    
                    DsFbl5nTableAdapters.SP_FINDCUSTOMERBYTEXTTableAdapter daSearch = new DsFbl5nTableAdapters.SP_FINDCUSTOMERBYTEXTTableAdapter();

                    daSearch.Fill(ds.SP_FINDCUSTOMERBYTEXT, Convert.ToString(cboCoCdSearch.Text), Convert.ToString(txtLettersFind.Text));
                    AdtvgSearchCustomer.DataSource = daSearch;
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        // Método que borra información de datagridview cuando usuario cambia company code
        private void cboCoCdSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtLettersFind.Text = string.Empty;

            if (AdtvgSearchCustomer.Rows.Count != 0)
            {
                 AdtvgSearchCustomer.DataSource = null;
            }

            fillPlaceholderTxtBox();
            
            FrmMain frmDestine = Owner as FrmMain;

            frmDestine.TxtCustNumberMain.Text = string.Empty;
            frmDestine.TxtAltNumberMain.Text = string.Empty;
            frmDestine.LblCustNameMain.Text = string.Empty;
            frmDestine.CboCoCdMain.Text = string.Empty;
        }
         
        // Valida que se ingrese solo letras en textbox de busqueda
        private void txtLettersFind_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!(char.IsLetter(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            //{
            //    MessageBox.Show("Only letters are allowed", "Cash Application Tool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    e.Handled = true;
            //    return;
            //}
        }

        void fillPlaceholderTxtBox()
        {
            if (txtLettersFind.Text == "")
            {
                txtLettersFind.Text = "Type here customer name...";
                txtLettersFind.ForeColor = Color.DimGray;
            }
        }

        private void cboCoCdSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!(char.IsLetter(e.KeyChar)) || (e.KeyChar != (char)Keys.Back))
            //{
            //    MessageBox.Show("Locked for manual entry, please select from the list", "Cash Application Tool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    e.Handled = true;
            //    return;
            //}
        }

        // Método para pasar información de celda seleccionada a Formulario Main
        public void AdtvgSearchCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                int colSelect = AdtvgSearchCustomer.CurrentRow.Cells[e.ColumnIndex].ColumnIndex;

                FrmMain frmDestine = Owner as FrmMain;

                if (colSelect == 0)
                {                    
                    frmDestine.TxtCustNumberMain.Text = AdtvgSearchCustomer.CurrentRow.Cells[e.ColumnIndex].Value.ToString();
                    frmDestine.TxtAltNumberMain.Text = "";
                    frmDestine.LblCustNameMain.Text = AdtvgSearchCustomer.CurrentRow.Cells[2].Value.ToString();
                    frmDestine.CboCoCdMain.Text = AdtvgSearchCustomer.CurrentRow.Cells[3].Value.ToString();
                }
                else if (colSelect == 1)
                {
                    frmDestine.TxtAltNumberMain.Text = AdtvgSearchCustomer.CurrentRow.Cells[e.ColumnIndex].Value.ToString();
                    frmDestine.TxtCustNumberMain.Text = "";
                    frmDestine.LblCustNameMain.Text = AdtvgSearchCustomer.CurrentRow.Cells[2].Value.ToString();
                    frmDestine.CboCoCdMain.Text = AdtvgSearchCustomer.CurrentRow.Cells[3].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void cboCoCdSearch_Click(object sender, EventArgs e)
        {
            if (cboCoCdSearch.Text == "-- Select Company --")
            {
                cboCoCdSearch.Text = "";
                cboCoCdSearch.ForeColor = Color.Black;
            }
        }

        private void cboCoCdSearch_Leave(object sender, EventArgs e)
        {
            fillPlaceHolder();
        }
        void fillPlaceHolder()
        {
            if (cboCoCdSearch.Text == "")
            {
                cboCoCdSearch.Text = "-- Select Company --";
                cboCoCdSearch.ForeColor = Color.DimGray;
            }
        }

        private void FrmSearchCustomerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
        }

    }
}

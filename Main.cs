using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.Entity;
// Libreria ADO NET
using System.Data.SqlClient;
using System.Threading;

namespace CFS_Latam_cashApplicationTool
{
    public partial class FrmMain : Form
    {       
        public FrmMain()
        {
            InitializeComponent();
            this.TtMessage.SetToolTip(this.pictLeft, "Delete selected rows");
            this.TtMessage.SetToolTip(this.PictureRight, "Selected Items");
            this.TtMessage.SetToolTip(this.pictSearch, "Search customer information");
            this.TtMessage.SetToolTip(this.pictSubmit, "Send customer conciliation");
            this.TtMessage.SetToolTip(this.pictUnselectAll, "Remove all rows");
            this.TtMessage.SetToolTip(this.pictExcel, "Export    Shift+F1");

        }

        //Conexión GLOBAL a Data Set
        DsFbl5n ds = new DsFbl5n();
        //Creamos objeto de Clase MainInput
        MainInput objMain = new MainInput();

        //Arrays de company codes
        private static readonly string[] company = { "AR1", "BR1", "CH4", "CL1", "CO1", "CR1", "EC1", "MX3", "PE1", "PY1", "UY1", "VE5" };
        //Crea objeto arrays de company codes
        private static readonly ArrayList cocd = new ArrayList(company);
        // Obtenemos ID de usuario (Ex: B082193) 
        private readonly string userName = Environment.UserName;

        private void FrmMain_Load(object sender, EventArgs e)
        {           
            //Carga User ID en header
            lblUserId.Text = "User: " + userName;
            
            //Carga combobox de company codes en base a objeto "cocd" (Linea 19)
            foreach (string c in cocd)
            {
                cboCompanyCode.Items.Add(c);
                cboCompanyCode.Text = "Select Company";
            }            

        }
            
        //Cierra ventana Main desde el menu PROGRAM - EXIT
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PictSearch_Click(object sender, EventArgs e)
        {            
            try
            {
                objMain.CompanyCode = cboCompanyCode.Text.ToString();
                objMain.CustomerNumber = txtCustomerNumber.Text;
                objMain.AltNumber = TxtAltNumber.Text;
                objMain.DocDZ = "DZ";

                // Valida que los campos de busqueda esten completos ********************************************************************
                //
                if (string.IsNullOrEmpty(objMain.CompanyCode) || cboCompanyCode.Text == "Select Company")
                {
                    MessageBox.Show("Please select Company Code", "Cash Application Tool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else if (string.IsNullOrEmpty(objMain.CustomerNumber) && string.IsNullOrEmpty(objMain.AltNumber))
                {
                    MessageBox.Show("Please enter a customer number or alternative number", "Cash Application Tool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                // Creamos objetos TablaAdapter con procedimientos almacenados en SQL **********************************************************
                //
                // Completa Tab CUSTOMER PAYMENT ---------------------------------- 01
                //               
                DsFbl5nTableAdapters.SP_SELECTPAYMENTSTableAdapter daPay = new DsFbl5nTableAdapters.SP_SELECTPAYMENTSTableAdapter();
                // Completa Tab INVOICES ------------------------------------------ 02
                //
                DsFbl5nTableAdapters.SP_SELECTINVOICESTableAdapter daInv = new DsFbl5nTableAdapters.SP_SELECTINVOICESTableAdapter();
                // Completa Tab CREDIT NOTES -------------------------------------- 03
                //
                DsFbl5nTableAdapters.SP_SELECTCREDITNOTESTableAdapter daCN = new DsFbl5nTableAdapters.SP_SELECTCREDITNOTESTableAdapter();
                // Completa Tab CREDIT BALANCE ------------------------------------ 04
                //
                DsFbl5nTableAdapters.SP_SELECTCREDITBALANCETableAdapter daCB = new DsFbl5nTableAdapters.SP_SELECTCREDITBALANCETableAdapter();
                // Completa Tab ALL DOCUMENTS ------------------------------------- 05
                //
                DsFbl5nTableAdapters.SP_SELECTFBL5NTableAdapter daAll = new DsFbl5nTableAdapters.SP_SELECTFBL5NTableAdapter();

                // Pasa variables Customer Number o AltNumber a null si corresponde 
                //
                if (objMain.CustomerNumber == string.Empty && objMain.AltNumber != string.Empty)
                {
                    // Completa Tab CUSTOMER PAYMENT ---------------------------------- 01
                    //               
                    daPay.Fill(ds.SP_SELECTPAYMENTS, objMain.CompanyCode, null, Convert.ToDecimal(objMain.AltNumber), "DZ");
                    AdtvgCustomerPay.DataSource = ds.SP_SELECTPAYMENTS;

                    // Completa Tab INVOICES ------------------------------------------ 02
                    //
                    daInv.Fill(ds.SP_SELECTINVOICES, objMain.CompanyCode, null, Convert.ToDecimal(objMain.AltNumber));
                    AdtvgInvoices.DataSource = ds.SP_SELECTINVOICES;

                    // Completa Tab CREDIT NOTES -------------------------------------- 03
                    //
                    daCN.Fill(ds.SP_SELECTCREDITNOTES, objMain.CompanyCode, null, Convert.ToDecimal(objMain.AltNumber));
                    AdtvgCreditNotes.DataSource = ds.SP_SELECTCREDITNOTES;

                    // Completa Tab CREDIT BALANCE ------------------------------------ 04
                    //
                    daCB.Fill(ds.SP_SELECTCREDITBALANCE, objMain.CompanyCode, null, Convert.ToDecimal(objMain.AltNumber));
                    AdtvgCreditBalance.DataSource = ds.SP_SELECTCREDITBALANCE;

                    // Completa Tab ALL DOCUMENTS ------------------------------------- 05
                    //
                    daAll.Fill(ds.SP_SELECTFBL5N, objMain.CompanyCode, null, Convert.ToDecimal(objMain.AltNumber));
                    AdtvgAllDoc.DataSource = ds.SP_SELECTFBL5N;
                }
                else if (objMain.CustomerNumber != string.Empty && objMain.AltNumber == string.Empty)
                {
                    // Completa Tab CUSTOMER PAYMENT ---------------------------------- 01
                    //
                    daPay.Fill(ds.SP_SELECTPAYMENTS, objMain.CompanyCode, Convert.ToDecimal(objMain.CustomerNumber), null, "DZ");
                    AdtvgCustomerPay.DataSource = ds.SP_SELECTPAYMENTS;

                    // Completa Tab INVOICES ------------------------------------------ 02
                    //
                    daInv.Fill(ds.SP_SELECTINVOICES, objMain.CompanyCode, Convert.ToDecimal(objMain.CustomerNumber), null);
                    AdtvgInvoices.DataSource = ds.SP_SELECTINVOICES;

                    // Completa Tab CREDIT NOTES -------------------------------------- 03
                    //
                    daCN.Fill(ds.SP_SELECTCREDITNOTES, objMain.CompanyCode, Convert.ToDecimal(objMain.CustomerNumber), null);
                    AdtvgCreditNotes.DataSource = ds.SP_SELECTCREDITNOTES;

                    // Completa Tab CREDIT BALANCE ------------------------------------ 04
                    //
                    daCB.Fill(ds.SP_SELECTCREDITBALANCE, objMain.CompanyCode, Convert.ToDecimal(objMain.CustomerNumber), null);
                    AdtvgCreditBalance.DataSource = ds.SP_SELECTCREDITBALANCE;

                    // Completa Tab ALL DOCUMENTS ------------------------------------- 05
                    //
                    daAll.Fill(ds.SP_SELECTFBL5N, objMain.CompanyCode, Convert.ToDecimal(objMain.CustomerNumber), null);
                    AdtvgAllDoc.DataSource = ds.SP_SELECTFBL5N;
                }
                else
                {
                    // Completa Tab CUSTOMER PAYMENT ---------------------------------- 01
                    //
                    daPay.Fill(ds.SP_SELECTPAYMENTS, objMain.CompanyCode, Convert.ToDecimal(objMain.CustomerNumber), Convert.ToDecimal(objMain.AltNumber), "DZ");
                    AdtvgCustomerPay.DataSource = ds.SP_SELECTPAYMENTS;

                    // Completa Tab INVOICES ------------------------------------------ 02
                    //
                    daInv.Fill(ds.SP_SELECTINVOICES, objMain.CompanyCode, Convert.ToDecimal(objMain.CustomerNumber), Convert.ToDecimal(objMain.AltNumber));
                    AdtvgInvoices.DataSource = ds.SP_SELECTINVOICES;

                    // Completa Tab CREDIT NOTES -------------------------------------- 03
                    //
                    daCN.Fill(ds.SP_SELECTCREDITNOTES, objMain.CompanyCode, Convert.ToDecimal(objMain.CustomerNumber), Convert.ToDecimal(objMain.AltNumber));
                    AdtvgCreditNotes.DataSource = ds.SP_SELECTCREDITNOTES;

                    // Completa Tab CREDIT BALANCE ------------------------------------ 04
                    //
                    daCB.Fill(ds.SP_SELECTCREDITBALANCE, objMain.CompanyCode, Convert.ToDecimal(objMain.CustomerNumber), Convert.ToDecimal(objMain.AltNumber));
                    AdtvgCreditBalance.DataSource = ds.SP_SELECTCREDITBALANCE;

                    // Completa Tab ALL DOCUMENTS ------------------------------------- 05
                    //
                    daAll.Fill(ds.SP_SELECTFBL5N, objMain.CompanyCode, Convert.ToDecimal(objMain.CustomerNumber), Convert.ToDecimal(objMain.AltNumber));
                    AdtvgAllDoc.DataSource = ds.SP_SELECTFBL5N;
                }

                // Completamos lbl de Company Code - Customer Nuumber  o Alt Number - Customer Name
                //

                string nameSelect = AdtvgAllDoc.CurrentRow.Cells[3].Value.ToString();

                if (nameSelect != string.Empty && objMain.CustomerNumber != string.Empty)
                {
                    lblCustomerSearch.Text = objMain.CompanyCode + " - " + objMain.CustomerNumber + " - " + nameSelect;
                }
                else if (nameSelect != string.Empty && objMain.CustomerNumber == string.Empty && objMain.AltNumber != string.Empty)
                {
                    lblCustomerSearch.Text = objMain.CompanyCode + " - Alternative " + objMain.AltNumber;
                }
                else if (nameSelect != string.Empty && objMain.CustomerNumber != string.Empty && objMain.AltNumber != string.Empty)
                {
                    lblCustomerSearch.Text = objMain.CompanyCode + " - " + objMain.CustomerNumber + " - " + nameSelect;
                }
                else if (nameSelect == string.Empty && objMain.CustomerNumber != string.Empty)
                {
                    lblCustomerSearch.Text = objMain.CompanyCode + " - " + objMain.CustomerNumber;
                }                 

            }
            catch(Exception)
            {                
                MessageBox.Show("Customer number " + objMain.CustomerNumber + " not found in database.", "Cash Application Tool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
           
        }

        private void TxtCustomerNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("This field only supports numbers", "Cash Application Tool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private void TxtReceiptNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("This field only supports numbers", "Cash Application Tool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }
        private void TxtAltNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("This field only supports numbers", "Cash Application Tool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }
        private void DtgvConciliation_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
  
        private void PanelTitleFilter_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PictSubmit_Click(object sender, EventArgs e)
        {
         
            MessageBox.Show("Your conciliation was sent successfully", "Cash Application Tool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        private void AdtgvCustPay_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictLeft_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow rowPrincipal in adtvgConciliation.SelectedRows)
            {
                if (!rowPrincipal.IsNewRow)
                   adtvgConciliation.Rows.Remove(rowPrincipal);
            }

            adtvgConciliation.ClearSelection();
            // Suma valores de columna Amount in Doc Curre (Col 6) de DataGridView Conciliation
            //            
            // Total Payments
            double totalPayments = adtvgConciliation.Rows.OfType<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "DZ").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            txtCustomerPayTotal.Text = Convert.ToString(totalPayments);

            // Total Invoices
            double totalRV = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "RV").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalDV = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "DV").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalDR = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "DR").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalInvoce = totalRV + totalDV + totalDR;
            txtInvoicesTotal.Text = Convert.ToString(totalInvoce);

            // Total Credit Notes
            double totalRG = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "RG").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalDG = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "DG").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalCreditNote = totalRG + totalDG;
            txtCreditNotesTotal.Text = Convert.ToString(totalCreditNote);

            // Total Credit Balance
            double totalAB = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "AB").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalRC = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "RC").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalCreditBalance = totalAB + totalRC;
            txtCreditBalanceTotal.Text = Convert.ToString(totalCreditBalance);

            double subtotal = totalRV + totalDV + totalDR + totalRG + totalDG + totalAB + totalRC;
            txtSubtotal.Text = Convert.ToString(subtotal);

        }
        private void PictureRight_Click(object sender, EventArgs e)
        {
            // Recorremos la colección de filas seleccionadas en el control --------------- Tab Customer Payment
            // DataGridView principal.  
            foreach (DataGridViewRow rowPrincipal in AdtvgCustomerPay.SelectedRows)
            {
                // Creamos un array con los valores que vamos a insertar en el segundo control DataGridView
                // 
                object[] values = {
                            rowPrincipal.Cells[0].Value, rowPrincipal.Cells[1].Value,
                            rowPrincipal.Cells[2].Value, rowPrincipal.Cells[3].Value,
                            rowPrincipal.Cells[4].Value, rowPrincipal.Cells[5].Value,
                            rowPrincipal.Cells[6].Value, rowPrincipal.Cells[7].Value,
                            rowPrincipal.Cells[8].Value, rowPrincipal.Cells[9].Value,
                            rowPrincipal.Cells[10].Value, rowPrincipal.Cells[11].Value,
                            rowPrincipal.Cells[12].Value, rowPrincipal.Cells[13].Value,
                            rowPrincipal.Cells[14].Value, rowPrincipal.Cells[15].Value,
                            rowPrincipal.Cells[16].Value};
                 
                // Creamos un nuevo objeto DataGridViewRow.
                //
                DataGridViewRow row = new DataGridViewRow();
                // Creamos las celdas y las rellenamos con los valores existentes en el array.
                //
                row.CreateCells(adtvgConciliation, values);

                // Añadimos la nueva fila al segundo control DataGridView.
                //
                adtvgConciliation.Rows.Add(row);
                                    
            }


            // Recorremos la colección de filas seleccionadas en el control --------------- Tab Invoices
            // DataGridView principal.  
            foreach (DataGridViewRow rowPrincipal in AdtvgInvoices.SelectedRows)
            {
                // Creamos un array con los valores que vamos a insertar
                // en el segundo control DataGridView.
                object[] values = {
                                    rowPrincipal.Cells[0].Value, rowPrincipal.Cells[1].Value,
                                    rowPrincipal.Cells[2].Value, rowPrincipal.Cells[3].Value,
                                    rowPrincipal.Cells[4].Value, rowPrincipal.Cells[5].Value,
                                    rowPrincipal.Cells[6].Value, rowPrincipal.Cells[7].Value,
                                    rowPrincipal.Cells[8].Value, rowPrincipal.Cells[9].Value,
                                    rowPrincipal.Cells[10].Value, rowPrincipal.Cells[11].Value,
                                    rowPrincipal.Cells[12].Value, rowPrincipal.Cells[13].Value,
                                    rowPrincipal.Cells[14].Value, rowPrincipal.Cells[15].Value,
                                    rowPrincipal.Cells[16].Value};

                // Creamos un nuevo objeto DataGridViewRow.
                //
                DataGridViewRow row = new DataGridViewRow();

                // Creamos las celdas y las rellenamos con los valores existentes en el array.
                //
                row.CreateCells(adtvgConciliation, values);

                // Añadimos la nueva fila al segundo control DataGridView.
                //
                adtvgConciliation.Rows.Add(row);
            }

            // Recorremos la colección de filas seleccionadas en el control --------------- Tab Credit Note
            // DataGridView principal.  
            foreach (DataGridViewRow rowPrincipal in AdtvgCreditNotes.SelectedRows)
            {
                // Creamos un array con los valores que vamos a insertar en el segundo control DataGridView.
                // 
                object[] values = {
                                    rowPrincipal.Cells[0].Value, rowPrincipal.Cells[1].Value,
                                    rowPrincipal.Cells[2].Value, rowPrincipal.Cells[3].Value,
                                    rowPrincipal.Cells[4].Value, rowPrincipal.Cells[5].Value,
                                    rowPrincipal.Cells[6].Value, rowPrincipal.Cells[7].Value,
                                    rowPrincipal.Cells[8].Value, rowPrincipal.Cells[9].Value,
                                    rowPrincipal.Cells[10].Value, rowPrincipal.Cells[11].Value,
                                    rowPrincipal.Cells[12].Value, rowPrincipal.Cells[13].Value,
                                    rowPrincipal.Cells[14].Value, rowPrincipal.Cells[15].Value,
                                    rowPrincipal.Cells[16].Value};

                // Creamos un nuevo objeto DataGridViewRow.
                //
                DataGridViewRow row = new DataGridViewRow();

                // Creamos las celdas y las rellenamos con los valores existentes en el array.
                //
                row.CreateCells(adtvgConciliation, values);

                // Añadimos la nueva fila al segundo control DataGridView.
                //
                adtvgConciliation.Rows.Add(row);
            }

            // Recorremos la colección de filas seleccionadas en el control --------------- Tab Credit Balance
            // DataGridView principal.  
            foreach (DataGridViewRow rowPrincipal in AdtvgCreditBalance.SelectedRows)
            {
                // Creamos un array con los valores que vamos a insertar
                // en el segundo control DataGridView.
                object[] values = {
                                    rowPrincipal.Cells[0].Value, rowPrincipal.Cells[1].Value,
                                    rowPrincipal.Cells[2].Value, rowPrincipal.Cells[3].Value,
                                    rowPrincipal.Cells[4].Value, rowPrincipal.Cells[5].Value,
                                    rowPrincipal.Cells[6].Value, rowPrincipal.Cells[7].Value,
                                    rowPrincipal.Cells[8].Value, rowPrincipal.Cells[9].Value,
                                    rowPrincipal.Cells[10].Value, rowPrincipal.Cells[11].Value,
                                    rowPrincipal.Cells[12].Value, rowPrincipal.Cells[13].Value,
                                    rowPrincipal.Cells[14].Value, rowPrincipal.Cells[15].Value,
                                    rowPrincipal.Cells[16].Value};

                // Creamos un nuevo objeto DataGridViewRow.
                //
                DataGridViewRow row = new DataGridViewRow();

                // Creamos las celdas y las rellenamos con los valores existentes en el array.
                //
                row.CreateCells(adtvgConciliation, values);

                // Añadimos la nueva fila al segundo control DataGridView.
                //
                adtvgConciliation.Rows.Add(row);                
            }

            // Recorremos la colección de filas seleccionadas en el control --------------- Tab All Documents
            // DataGridView principal.            
            foreach (DataGridViewRow rowPrincipal in AdtvgAllDoc.SelectedRows)
            {
                // Creamos un array con los valores que vamos a insertar
                // en el segundo control DataGridView.
                object[] values = {
                                    rowPrincipal.Cells[0].Value, rowPrincipal.Cells[1].Value,
                                    rowPrincipal.Cells[2].Value, rowPrincipal.Cells[3].Value,
                                    rowPrincipal.Cells[4].Value, rowPrincipal.Cells[5].Value,
                                    rowPrincipal.Cells[6].Value, rowPrincipal.Cells[7].Value,
                                    rowPrincipal.Cells[8].Value, rowPrincipal.Cells[9].Value,
                                    rowPrincipal.Cells[10].Value, rowPrincipal.Cells[11].Value,
                                    rowPrincipal.Cells[12].Value, rowPrincipal.Cells[13].Value,
                                    rowPrincipal.Cells[14].Value, rowPrincipal.Cells[15].Value,
                                    rowPrincipal.Cells[16].Value};

                // Creamos un nuevo objeto DataGridViewRow.
                //
                DataGridViewRow row = new DataGridViewRow();

                // Creamos las celdas y las rellenamos con los valores existentes en el array.
                //
                row.CreateCells(adtvgConciliation, values);

                // Añadimos la nueva fila al segundo control DataGridView.
                //
                adtvgConciliation.Rows.Add(row);    
            }

            AdtvgCustomerPay.ClearSelection();
            AdtvgInvoices.ClearSelection();
            AdtvgCreditNotes.ClearSelection();
            AdtvgCreditBalance.ClearSelection();
            AdtvgAllDoc.ClearSelection();

            // Suma valores de columna Amount in Doc Curre (Col 6) de DataGridView Conciliation
            //            
            // Total Payments
            double totalPayments = adtvgConciliation.Rows.OfType<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "DZ").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            txtCustomerPayTotal.Text = Convert.ToString(totalPayments);
            
            // Total Invoices
            double totalRV = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "RV").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalDV = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "DV").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalDR = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "DR").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalInvoce = totalRV + totalDV + totalDR;
            txtInvoicesTotal.Text = Convert.ToString(totalInvoce);

            // Total Credit Notes
            double totalRG = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "RG").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalDG = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "DG").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalCreditNote = totalRG + totalDG;
            txtCreditNotesTotal.Text = Convert.ToString(totalCreditNote);

            // Total Credit Balance
            double totalAB = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "AB").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalRC = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "RC").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalCreditBalance = totalAB + totalRC;
            txtCreditBalanceTotal.Text = Convert.ToString(totalCreditBalance);

            double subtotal = totalRV + totalDV + totalDR + totalRG + totalDG + totalAB + totalRC;
            txtSubtotal.Text= Convert.ToString(subtotal);

        }

        private void AdtvgAllDoc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void PanelBar_Paint(object sender, PaintEventArgs e)
        {
            
        }
        private void pictUnselectAll_Click(object sender, EventArgs e)
        {
            adtvgConciliation.Rows.Clear();
        }
        private void advancedDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictExcel_Click(object sender, EventArgs e)
        {

            try
            {
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                app.Visible = true;
                worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                worksheet.Name = "Records";

                try
                {
                    for (int i = 0; i < adtvgConciliation.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1] = adtvgConciliation.Columns[i].HeaderText;
                    }
                    for (int i = 0; i < adtvgConciliation.Rows.Count; i++)
                    {
                        for (int j = 0; j < adtvgConciliation.Columns.Count; j++)
                        {
                            if (adtvgConciliation.Rows[i].Cells[j].Value != null)
                            {
                                worksheet.Cells[i + 2, j + 1] = adtvgConciliation.Rows[i].Cells[j].Value.ToString();
                            }
                            else
                            {
                                worksheet.Cells[i + 2, j + 1] = "";
                            }
                        }
                    }

                    //Getting the location and file name of the excel to save from user. 
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    saveDialog.FilterIndex = 2;

                    if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        workbook.SaveAs(saveDialog.FileName);
                        MessageBox.Show("Export Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                finally
                {
                    app.Quit();
                    workbook = null;
                    worksheet = null;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); }

        }


}
}

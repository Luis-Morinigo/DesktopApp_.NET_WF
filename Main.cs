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
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using System.Windows.Controls;
using System.Drawing.Design;
using System.Globalization;
using DocumentFormat.OpenXml.Vml;
using TextBox = System.Windows.Forms.TextBox;
using DocumentFormat.OpenXml.Spreadsheet;
using Color = System.Drawing.Color;
using Microsoft.Office.Tools.Excel.Controls;
using ComboBox = System.Windows.Forms.ComboBox;
using Label = System.Windows.Forms.Label;

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

        // Obtenemos ID de usuario (Ex: B082193) 
        private readonly string userName = Environment.UserName;

        private void FrmMain_Load(object sender, EventArgs e)
        {           
            //Carga User ID en header
            lblUserId.Text = "User: " + userName;
            //Carga combobox de company codes
            fillCompanyCodes();
            objetos();
            //Color a header de columnas Disputes
            adtvgConciliation.Columns[18].HeaderCell.Style.BackColor = Color.LightGreen; adtvgConciliation.Columns[19].HeaderCell.Style.BackColor = Color.LightGreen;
            adtvgConciliation.Columns[20].HeaderCell.Style.BackColor = Color.LightGreen; adtvgConciliation.Columns[21].HeaderCell.Style.BackColor = Color.LightGreen;
            //Convierte a formato 1,000.00 números de columna de Dispute Amount
            //adtvgConciliation.Columns[18].ValueType = typeof(double);
        }

        // Variables para obtener valores de Formulario Search Customer
        private TextBox _txtCustNumberMain;
        public TextBox TxtCustNumberMain { get => _txtCustNumberMain; set => _txtCustNumberMain = value; }

        private TextBox _txtAltNumberMain;
        public TextBox TxtAltNumberMain { get => _txtAltNumberMain; set => _txtAltNumberMain = value; }
        
        private ComboBox _cboCoCdMain;
        public ComboBox CboCoCdMain { get => _cboCoCdMain; set => _cboCoCdMain = value; }

        private Label _lblCustNameMain;
        public Label LblCustNameMain { get => _lblCustNameMain; set => _lblCustNameMain = value; }

        private void objetos()
        {
            _txtCustNumberMain = txtCustomerNumber;
            _txtAltNumberMain = TxtAltNumber;
            _cboCoCdMain = cboCompanyCode;
            _lblCustNameMain = lblCustomerSearch;

        }

        // Método para completar combobox de company codes
        void fillCompanyCodes()
        {
            DsFbl5nTableAdapters.SP_COMPANYCODESTableAdapter daCoCd = new DsFbl5nTableAdapters.SP_COMPANYCODESTableAdapter();
            daCoCd.Fill(ds.SP_COMPANYCODES);
            cboCompanyCode.DataSource = ds.SP_COMPANYCODES;
            cboCompanyCode.DisplayMember = "Company Code";
            cboCompanyCode.ValueMember = "Company Code";
            cboCompanyCode.Text = "-- Select Company --";
            cboCompanyCode.ForeColor = Color.Black;
        }

        //Cierra ventana Main desde el menu PROGRAM - EXIT
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PictSearch_Click(object sender, EventArgs e)
        {
            clearConciliation(); 
            clearSubtotales();

            try
            {
                objMain.CompanyCode = cboCompanyCode.Text.ToString();
                objMain.CustomerNumber = txtCustomerNumber.Text;
                objMain.AltNumber = TxtAltNumber.Text;
                objMain.DocDZ = "DZ";

                // Step 1: Valida que los campos de busqueda esten completos ********************************************************************
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

                // Step 2: Creamos objetos TableAdapter con procedimientos almacenados en SQL **********************************************************
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
                // Completa Filas AGREEMENTS ------------------------------------- 05
                //
                DsFbl5nTableAdapters.SP_AGREEMENTTableAdapter daAg = new DsFbl5nTableAdapters.SP_AGREEMENTTableAdapter();                
                // Completa Filas REASON CODES ------------------------------------- 07
                //
                DsFbl5nTableAdapters.SP_REASONCODETableAdapter daRc = new DsFbl5nTableAdapters.SP_REASONCODETableAdapter();

                // Step 3: Pasa variables Customer Number o AltNumber a null si corresponde 
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

                    // Completa combobox de columna AGREEMENTS ------------------------------------- 06
                    //
                    daAg.Fill(ds.SP_AGREEMENT, objMain.CompanyCode, Convert.ToDecimal(objMain.CustomerNumber));

                    DataGridViewComboBoxColumn comboAgreement = adtvgConciliation.Columns[20] as DataGridViewComboBoxColumn;

                    comboAgreement.DataSource = ds.SP_AGREEMENT;
                    comboAgreement.DisplayMember = "Agreement";
                    comboAgreement.ValueMember = "Agreement";

                    //Carga Reason codes ------------------------------------- 07
                    //
                    daRc.Fill(ds.SP_REASONCODE, objMain.CompanyCode);

                    DataGridViewComboBoxColumn comboReason = adtvgConciliation.Columns[19] as DataGridViewComboBoxColumn;

                    comboReason.DataSource = ds.SP_REASONCODE;
                    comboReason.DisplayMember = "Reason Code";
                    comboReason.ValueMember = "Reason Code";
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

                // Step 4: Completamos lbl de Company Code - Customer Nuumber  o Alt Number - Customer Name
                //
                string nameSelect = AdtvgAllDoc.CurrentRow.Cells[3].Value.ToString();

                lblCustomerLoadName(nameSelect);          
            }
            catch(Exception)
            {                
                MessageBox.Show("Customer number " + objMain.CustomerNumber + " not found in database.", "Cash Application Tool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
           
        }
        // Carga nombre de cliente
        //
        public void lblCustomerLoadName(string nameSelect)
        {

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

        // Valida que solo se ingresen numeros en textbox Customer Number
        //
        private void TxtCustomerNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("This field only supports numbers", "Cash Application Tool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        // Valida que solo se ingresen numeros en textbox Receip Number
        //
        private void TxtReceiptNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("This field only supports numbers", "Cash Application Tool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        // Valida que solo se ingresen numeros en textbox Alternative Number
        //
        private void TxtAltNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("This field only supports numbers", "Cash Application Tool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
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
            sumTotal();
        }

        void sumTotal()
        {
            double totalPayments = adtvgConciliation.Rows.OfType<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "DZ").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            //txtCustomerPayTotal.Text = Convert.ToString(totalPayments);
            txtCustomerPayTotal.Text = (totalPayments.ToString("#,#0.00", CultureInfo.InvariantCulture));
                
            // Total Invoices
            double totalRV = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "RV").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalDV = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "DV").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalDR = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "DR").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalInvoce = totalRV + totalDV + totalDR;
            //txtInvoicesTotal.Text = Convert.ToString(totalInvoce);
            txtInvoicesTotal.Text = (totalInvoce.ToString("#,#0.00", CultureInfo.InvariantCulture));

            // Total Credit Notes
            double totalRG = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "RG").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalDG = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "DG").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalCreditNote = totalRG + totalDG;
            //txtCreditNotesTotal.Text = Convert.ToString(totalCreditNote);
            txtCreditNotesTotal.Text = (totalCreditNote.ToString("#,#0.00", CultureInfo.InvariantCulture));

            // Total Credit Balance
            double totalAB = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "AB").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalRC = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "RC").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalCreditBalance = totalAB + totalRC;
            //txtCreditBalanceTotal.Text = Convert.ToString(totalCreditBalance);
            txtCreditBalanceTotal.Text = (totalCreditBalance.ToString("#,#0.00", CultureInfo.InvariantCulture));

            // Subtotal
            double subtotal = totalRV + totalDV + totalDR + totalRG + totalDG + totalAB + totalRC + totalPayments;

            //Convierto a formato coma y punto
            //txtSubtotal.Text = Convert.ToString(Math.Round(subtotal, 2));
            txtSubtotal.Text = (subtotal.ToString("#,#0.00", CultureInfo.InvariantCulture));
        }

        private void PictSubmit_Click(object sender, EventArgs e)
        {
            double totalsub = Convert.ToDouble(txtSubtotal.Text);

            if (totalsub <= 10 && totalsub >= -10)
            {
                MessageBox.Show("Your conciliation was sent successfully", "Cash Application Tool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("Please review your reconciliation, the subtotal must be between the ranges 10 and -10", "Cash Application Tool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
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
                                    rowPrincipal.Cells[16].Value
                };

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
                                    rowPrincipal.Cells[16].Value
                };

                //AdtvgAllDoc.Rows.RemoveAt(rowPrincipal.Cells[0].RowIndex);

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
            sumTotal();
        }
       
        private void pictUnselectAll_Click(object sender, EventArgs e)
        {
            //adtvgConciliation.Rows.Clear();
            clearConciliation();
            sumTotal();
        }

        void clearConciliation()
        {
            adtvgConciliation.Rows.Clear();
            txtReceiptNumber.Clear();   
        }

        private void pictExcel_Click(object sender, EventArgs e)
        {
            exportExcel();
        }

        private void exportExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportExcel();
        }

        // Función para exportar DataGrid Conciliation a Excel
        //
        void exportExcel()
        {
            try
            {
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                app.Visible = true;
                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Sheets["Sheet1"];
                worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.ActiveSheet;
                //worksheet = workbook.Sheets["Sheet1"];
                //worksheet = workbook.ActiveSheet;

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
                        MessageBox.Show("Export Successful", "Cash Application Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        void clearSubtotales()
        {
            txtInvoicesTotal.Clear();
            txtCustomerPayTotal.Clear();
            txtCreditNotesTotal.Clear();
            txtCreditBalanceTotal.Clear();
            txtSubtotal.Clear();
        }

        private void pictFormSearch_Click(object sender, EventArgs e)
        {
            FrmSearchCustomerName frmSearch = new FrmSearchCustomerName();
            AddOwnedForm(frmSearch);
            frmSearch.ShowDialog();
        }
        //Hover & Leave
        private void pictSearch_MouseHover(object sender, EventArgs e)
        {
            this.pictSearch.Size = new System.Drawing.Size(46, 40);
            this.pictSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictSearch.Cursor = System.Windows.Forms.Cursors.Hand;
        }
        private void pictSearch_MouseLeave(object sender, EventArgs e)
        {
            this.pictSearch.Size = new System.Drawing.Size(43, 37);
            this.pictSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }
        private void pictFormSearch_MouseHover(object sender, EventArgs e)
        {
            this.pictFormSearch.Size = new System.Drawing.Size(46, 40);
            this.pictFormSearch.BorderStyle = BorderStyle.FixedSingle;
            this.pictFormSearch.Cursor = System.Windows.Forms.Cursors.Hand;
        }
        private void pictFormSearch_MouseLeave(object sender, EventArgs e)
        {
            this.pictFormSearch.Size = new System.Drawing.Size(43, 37);
            this.pictFormSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }
        private void pictExcel_MouseHover(object sender, EventArgs e)
        {            
            this.pictExcel.Size = new System.Drawing.Size(46, 40);
            this.pictExcel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictExcel.Cursor = System.Windows.Forms.Cursors.Hand;
        }
        private void pictExcel_MouseLeave(object sender, EventArgs e)
        {
            this.pictExcel.Size = new System.Drawing.Size(43, 37);
            this.pictExcel.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }
        private void PictureRight_MouseHover(object sender, EventArgs e)
        {
            this.PictureRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PictureRight.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void PictureRight_MouseLeave(object sender, EventArgs e)
        {
            this.PictureRight.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        private void pictLeft_MouseHover(object sender, EventArgs e)
        {
            this.pictLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictLeft.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void pictLeft_MouseLeave(object sender, EventArgs e)
        {
            this.pictLeft.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        private void pictUnselectAll_MouseHover(object sender, EventArgs e)
        {
            this.pictUnselectAll.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictUnselectAll.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void pictUnselectAll_MouseLeave(object sender, EventArgs e)
        {
            this.pictUnselectAll.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }
        private void pictSubmit_MouseHover(object sender, EventArgs e)
        {
            this.pictSubmit.Size = new System.Drawing.Size(110, 52);
            this.pictSubmit.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void pictSubmit_MouseLeave(object sender, EventArgs e)
        {
            this.pictSubmit.Size = new System.Drawing.Size(104, 46);
        }

        // Ingresa número de recibo en campo Receipt number
        private void txtReceiptNumber_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i <= adtvgConciliation.Rows.Count; i++)
            {
                if (i != adtvgConciliation.Rows.Count)
                {
                    adtvgConciliation.Rows[i].Cells[17].Value = txtReceiptNumber.Text;
                }
            }
        }

        private void adtvgConciliation_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int columnNumber = adtvgConciliation.CurrentCell.ColumnIndex;
            try
            {
                if(columnNumber == 20)
                {
                    cboDescriptionAgreement.Visible = true;

                    objMain.CompanyCode = Convert.ToString(adtvgConciliation.Rows[e.RowIndex].Cells[0].Value);
                    objMain.CustomerNumber = Convert.ToString(adtvgConciliation.Rows[e.RowIndex].Cells[2].Value);
                    string agreementNumber = Convert.ToString(adtvgConciliation.Rows[e.RowIndex].Cells[20].Value);
                    
                    //Carga números de acuerdo
                    DsFbl5nTableAdapters.SP_DESCRIPTIONAGREEMENTTableAdapter daDs = new DsFbl5nTableAdapters.SP_DESCRIPTIONAGREEMENTTableAdapter();

                    daDs.Fill(ds.SP_DESCRIPTIONAGREEMENT, objMain.CompanyCode, Convert.ToDecimal(objMain.CustomerNumber), Convert.ToDecimal(agreementNumber));

                    cboDescriptionAgreement.DataSource = ds.SP_DESCRIPTIONAGREEMENT;
                    cboDescriptionAgreement.DisplayMember = "Descript_";

                    adtvgConciliation.Rows[e.RowIndex].Cells[21].Value = cboDescriptionAgreement.Text;                    
                }
            }
            catch (Exception)
            {

            }
            cboDescriptionAgreement.Visible = false;
        }

        private void adtvgConciliation_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if(e.Exception.Message == "DataGridViewComboBoxCell value is not valid.")
            {
                object value = adtvgConciliation.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (!((DataGridViewComboBoxColumn)adtvgConciliation.Columns[e.ColumnIndex]).Items.Contains(value))
                {
                    e.ThrowException = false;
                }
            }
        }

        void fillPlaceHolder()
        {
            if (cboCompanyCode.Text == "")
            {
                cboCompanyCode.Text = "-- Select Company --";
                cboCompanyCode.ForeColor = Color.DimGray;
            }
        }

        private void cboCompanyCode_Click(object sender, EventArgs e)
        {
            if (cboCompanyCode.Text == "-- Select Company --")
            {
                cboCompanyCode.Text = "";
                cboCompanyCode.ForeColor = Color.Black;
            }                        
        }

        private void cboCompanyCode_Leave(object sender, EventArgs e)
        {
            fillPlaceHolder();
        }

        private void validOnlyNumber(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("This field only supports numbers", "Cash Application Tool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        private bool ValidNumber(string value)
        {
            //Obtenemos la longitud de la celda
            int len = value.Length;
            for (int i = 0; i != len; ++i)
            {
                //Detectamos si la celda tiene únicamente números
                if (!(value[i] >= '0' && value[i] <= '9') || value[i] <= '.')
                    return true;
            }
            return false;
        }

        private void adtvgConciliation_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Columns_KeyPress);

            if (adtvgConciliation.CurrentCell.ColumnIndex == 18)
            {
                TextBox tb = e.Control as TextBox;

                if (tb != null)
                {
                     tb.KeyPress += new KeyPressEventHandler(Columns_KeyPress);
                     adtvgConciliation.Columns[18].ValueType = typeof(double);                    
                }
            }
        }

        private void Columns_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 45) || (e.KeyChar >= 58 && e.KeyChar <= 255) || (e.KeyChar == 47))
            {
                e.Handled = true;
            }
        }
    }
}

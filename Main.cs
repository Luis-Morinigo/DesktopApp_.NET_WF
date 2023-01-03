﻿using System;
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
using System.Data.Entity.Infrastructure;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing;
using CFS_Latam_cashApplicationTool.Models.DBUsersActivity;
//using CFS_Latam_cashApplicationTool.Models.DBConctact;

namespace CFS_Latam_cashApplicationTool
{
    public partial class FrmMain : Form
    {       
        public FrmMain()
        {
            InitializeComponent();
            this.TtMessage.SetToolTip(this.pictLeft, "Delete selected rows in Customer Conciliation");
            this.TtMessage.SetToolTip(this.PictureRight, "Selected Items in Customer Line Item Display");
            this.TtMessage.SetToolTip(this.pictSearch, "Search customer information    F8");
            this.TtMessage.SetToolTip(this.pictSubmitDb, "Send Customer conciliation    F10");
            this.TtMessage.SetToolTip(this.pictUnselectAll, "Remove all rows in Customer conciliation");
            this.TtMessage.SetToolTip(this.pictExcel, "Export    Shift+F1");
            this.TtMessage.SetToolTip(this.pictFormSearch, "Search customers by text    Shift+F2");
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.DoubleBuffered = true;
        }

        //Conexión GLOBAL a Data Set
        DsFbl5n ds = new DsFbl5n();
        //Creamos objeto de Clase MainInput
        MainInput objMain = new MainInput();
        FrmLoginError objFrmError = new FrmLoginError();        

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

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.DoubleBuffered = true;
        }

        // Variables para obtener valores de Formulario Search Customer **********************************************************************************
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

        // Método para completar combobox de company codes ****************************************************************************************
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

        // Cierra ventana Main desde el menu PROGRAM - EXIT ****************************************************************************************
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Busca información del cliente seleccionado en SQL ***************************************************************************************
        public void PictSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //Limpia DataGrid Conciliation y subtotales
                clearConciliation();
                clearSubtotales();
                
                objMain.CompanyCode = cboCompanyCode.Text.ToString();
                objMain.CustomerNumber = txtCustomerNumber.Text;
                objMain.AltNumber = TxtAltNumber.Text;
                objMain.DocDZ = "DZ";                

                // Step 1: Valida que los campos de busqueda esten completos
                //
                if (string.IsNullOrEmpty(objMain.CompanyCode) || cboCompanyCode.Text.Contains("Select Company"))
                {
                    //MessageBox.Show("Please select Company Code", "Cash Application Tool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    MessageBox.Show("Please select Company Code", MainInput.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (string.IsNullOrEmpty(objMain.CustomerNumber) && string.IsNullOrEmpty(objMain.AltNumber))
                {
                    MessageBox.Show("Please enter a customer number or alternative number", MainInput.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    // Step 2: Creamos objetos TableAdapter con procedimientos almacenados en SQL
                    //
                    // Tab CUSTOMER PAYMENT ---------------------------------- 01
                    //               
                    DsFbl5nTableAdapters.SP_SELECTPAYMENTSTableAdapter daPay = new DsFbl5nTableAdapters.SP_SELECTPAYMENTSTableAdapter();
                    // Tab INVOICES ------------------------------------------ 02
                    //
                    DsFbl5nTableAdapters.SP_SELECTINVOICESTableAdapter daInv = new DsFbl5nTableAdapters.SP_SELECTINVOICESTableAdapter();
                    // Tab CREDIT NOTES -------------------------------------- 03
                    //
                    DsFbl5nTableAdapters.SP_SELECTCREDITNOTESTableAdapter daCN = new DsFbl5nTableAdapters.SP_SELECTCREDITNOTESTableAdapter();
                    // Tab CREDIT BALANCE ------------------------------------ 04
                    //
                    DsFbl5nTableAdapters.SP_SELECTCREDITBALANCETableAdapter daCB = new DsFbl5nTableAdapters.SP_SELECTCREDITBALANCETableAdapter();
                    // Tab ALL DOCUMENTS ------------------------------------- 05
                    //
                    DsFbl5nTableAdapters.SP_SELECTFBL5NTableAdapter daAll = new DsFbl5nTableAdapters.SP_SELECTFBL5NTableAdapter();
                    // Filas AGREEMENTS ------------------------------------- 05
                    //
                    DsFbl5nTableAdapters.SP_AGREEMENTTableAdapter daAg = new DsFbl5nTableAdapters.SP_AGREEMENTTableAdapter();
                    // Filas REASON CODES ------------------------------------- 07
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

                        //Completa combobox de columna AGREEMENTS------------------------------------ 06
                        //
                        daAg.Fill(ds.SP_AGREEMENT, objMain.CompanyCode, Convert.ToDecimal(objMain.AltNumber));
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

                        //Completa combobox de columna AGREEMENTS------------------------------------ 06
                        //
                        daAg.Fill(ds.SP_AGREEMENT, objMain.CompanyCode, Convert.ToDecimal(objMain.CustomerNumber));
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

                        //Completa combobox de columna AGREEMENTS------------------------------------ 06
                        //
                        daAg.Fill(ds.SP_AGREEMENT, objMain.CompanyCode, Convert.ToDecimal(objMain.CustomerNumber));
                    }

                    DataGridViewComboBoxColumn comboAgreement = adtvgConciliation.Columns[20] as DataGridViewComboBoxColumn;

                    comboAgreement.DataSource = ds.SP_AGREEMENT;
                    comboAgreement.DisplayMember = "Agreement";
                    comboAgreement.ValueMember = "Agreement";

                    // Carga Reason codes ------------------------------------- 07
                    //
                    daRc.Fill(ds.SP_REASONCODE, objMain.CompanyCode);

                    DataGridViewComboBoxColumn comboReason = adtvgConciliation.Columns[19] as DataGridViewComboBoxColumn;

                    comboReason.DataSource = ds.SP_REASONCODE;
                    comboReason.DisplayMember = "Reason Code";
                    comboReason.ValueMember = "Reason Code";

                    // Step 4: Completamos lbl de Company Code - Customer Nuumber  o Alt Number - Customer Name
                    //
                    string nameSelect = AdtvgAllDoc.CurrentRow.Cells[3].Value.ToString();

                    lblCustomerLoadName(nameSelect);
                }
            }
            catch(Exception)
            {                
                MessageBox.Show("Customer number " + objMain.CustomerNumber + " not found in database.", MainInput.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }           
        }

        // Carga nombre de cliente ********************************************************************************************************************
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
            validOnlyNumber(Owner, e);
        }
        // Valida que solo se ingresen numeros en textbox Receip Number
        //
        private void TxtReceiptNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            validOnlyNumber(Owner, e);
        }
        // Valida que solo se ingresen numeros en textbox Alternative Number
        //
        private void TxtAltNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            validOnlyNumber(Owner, e);
        }

        //Suma subtotales por tipo de documento ******************************************************************************************************
        void sumTotal()
        {
            // Total DZ
            double totalPayments = adtvgConciliation.Rows.OfType<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "DZ").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            txtCustomerPayTotal.Text = (totalPayments.ToString("#,#0.00", CultureInfo.InvariantCulture));
                
            // Total Invoices
            double totalRV = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "RV").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalDV = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "DV").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalDR = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "DR").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalInvoce = totalRV + totalDV + totalDR;
            txtInvoicesTotal.Text = (totalInvoce.ToString("#,#0.00", CultureInfo.InvariantCulture));

            // Total Credit Notes
            double totalRG = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "RG").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalDG = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "DG").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalCreditNote = totalRG + totalDG;
            txtCreditNotesTotal.Text = (totalCreditNote.ToString("#,#0.00", CultureInfo.InvariantCulture));

            // Total Credit Balance
            double totalAB = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "AB").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalRC = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[6].Value != null && x.Cells[4].Value.ToString() == "RC").ToList().Sum(y => Convert.ToDouble(y.Cells[6].Value));
            double totalCreditBalance = totalAB + totalRC;
            txtCreditBalanceTotal.Text = (totalCreditBalance.ToString("#,#0.00", CultureInfo.InvariantCulture));

            // Total Disputes
            double totalDP = adtvgConciliation.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[18].Value != null).ToList().Sum(y => Convert.ToDouble(y.Cells[18].Value));
            double totalDisputes = totalDP * -1;
            txtDisputesTotal.Text = (totalDisputes.ToString("#,#0.00", CultureInfo.InvariantCulture));

            // Subtotal
            double subtotal = totalRV + totalDV + totalDR + totalRG + totalDG + totalAB + totalRC + totalPayments - totalDP;

            // Convierto a formato coma y punto
            txtSubtotal.Text = (subtotal.ToString("#,#0.00", CultureInfo.InvariantCulture));

            // Cambia color de fondo según importe
            double totalsub = Convert.ToDouble(txtSubtotal.Text);

            if (totalsub > 10 || totalsub < -10)
            {
                txtSubtotal.BackColor = System.Drawing.Color.OrangeRed;
                txtSubtotal.ForeColor = Color.White;
            }
            else if (totalsub <= 10 && totalsub >= -10)
            {
                txtSubtotal.BackColor = System.Drawing.Color.LightGreen;
                txtSubtotal.ForeColor = Color.Black;
            }
        }

        // Envia información de Datagrid Conciliation a SQL ****************************************************************************************************
        private void pictSubmitDb_Click(object sender, EventArgs e)
        {
            try
            {
                if (adtvgConciliation.RowCount > 0)
                {
                    double totalsub = Convert.ToDouble(txtSubtotal.Text);
                    bool checkDisputes = false;

                    // Validamos los campos de Disputes Amount, Reason Code y Description
                    for (int i = 0; i < adtvgConciliation.RowCount; i++)
                    {
                        double colDisputeAmount = Convert.ToDouble(adtvgConciliation.Rows[i].Cells[18].Value);
                        string colReasonCode = Convert.ToString(adtvgConciliation.Rows[i].Cells[19].Value);
                        string colAgreement = Convert.ToString(adtvgConciliation.Rows[i].Cells[20].Value);
                        string colDescription = Convert.ToString(adtvgConciliation.Rows[i].Cells[21].Value);

                        if (colDisputeAmount != 0 && (colReasonCode == string.Empty || colDescription == string.Empty))
                        {
                            adtvgConciliation.Rows[i].Cells[19].Style.BackColor = Color.DarkSalmon; adtvgConciliation.Rows[i].Cells[21].Style.BackColor = Color.DarkSalmon;
                            checkDisputes = true;
                        }
                        else if (colReasonCode != string.Empty && (colDisputeAmount == 0 || colDescription == string.Empty))
                        {
                            adtvgConciliation.Rows[i].Cells[18].Style.BackColor = Color.DarkSalmon; adtvgConciliation.Rows[i].Cells[21].Style.BackColor = Color.DarkSalmon;
                            checkDisputes = true;
                        }
                        else if (colAgreement != string.Empty && (colDisputeAmount == 0 || colReasonCode == string.Empty || colDescription == string.Empty))
                        {
                            adtvgConciliation.Rows[i].Cells[18].Style.BackColor = Color.DarkSalmon; adtvgConciliation.Rows[i].Cells[19].Style.BackColor = Color.DarkSalmon;
                            adtvgConciliation.Rows[i].Cells[21].Style.BackColor = Color.DarkSalmon;
                            checkDisputes = true;
                        }
                        else if (colDescription != string.Empty && (colDisputeAmount == 0 || colReasonCode == string.Empty))
                        {
                            adtvgConciliation.Rows[i].Cells[18].Style.BackColor = Color.DarkSalmon; adtvgConciliation.Rows[i].Cells[19].Style.BackColor = Color.DarkSalmon;
                            checkDisputes = true;
                        }
                        else
                        {
                            adtvgConciliation.Rows[i].Cells[18].Style.BackColor = Color.Empty; adtvgConciliation.Rows[i].Cells[19].Style.BackColor = Color.Empty;
                            adtvgConciliation.Rows[i].Cells[20].Style.BackColor = Color.Empty; adtvgConciliation.Rows[i].Cells[21].Style.BackColor = Color.Empty;
                        }
                    }

                    if (checkDisputes)
                    {
                        MessageBox.Show("There are mandatory fields pending completion in your Customer Conciliation.", MainInput.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (totalsub > 10 || totalsub < -10)
                    {
                        MessageBox.Show("Please review your reconciliation, the subtotal must be between the ranges 10 and -10", MainInput.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (totalsub <= 10 && totalsub >= -10)
                    {
                        // Agregar columnas a Datagrid Conciliation                      
                        addColumns(Owner,e);

                        MessageBox.Show("Your conciliation was sent successfully", MainInput.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                // No existe información para enviar en Customer Conciliation
                else if (adtvgConciliation.RowCount == 0)
                {
                    MessageBox.Show("There isn't information to send in Customer Conciliation.", MainInput.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Método para agregar columnas a datagrid de Customer Conciliation
        private void addColumns(object sender, EventArgs e)
        {
            try
            {
                DataGridViewTextBoxColumn Col_IdDocNumber; DataGridViewTextBoxColumn Col_EntruDate; DataGridViewTextBoxColumn Col_Guid;
                DataGridViewTextBoxColumn Col_UserName; DataGridViewTextBoxColumn Col_DocNumberCAT; DataGridViewTextBoxColumn Col_Tool;
                DataGridViewTextBoxColumn Col_Status; DataGridViewTextBoxColumn Col_PostingDateSAP; DataGridViewTextBoxColumn Col_DocumentNumberSAP;

                Col_IdDocNumber = new DataGridViewTextBoxColumn() { Name = "ID_Doc_Number", HeaderText = "ID Doc Number", Width = 150 };
                Col_EntruDate = new DataGridViewTextBoxColumn() { Name = "Entry_date_CAT", HeaderText = "Entry date CAT", Width = 150 };
                Col_Guid = new DataGridViewTextBoxColumn() { Name = "Guid", HeaderText = "GUID", Width = 50 };
                Col_UserName = new DataGridViewTextBoxColumn() { Name = "User_Name", HeaderText = "User Name", Width = 150 };
                Col_DocNumberCAT = new DataGridViewTextBoxColumn() { Name = "Doc_Number_CAT", HeaderText = "Doc Number CAT", Width = 150 };
                Col_Tool = new DataGridViewTextBoxColumn() { Name = "Tool", HeaderText = "Tool", Width = 150 };
                Col_Status = new DataGridViewTextBoxColumn() { Name = "Status", HeaderText = "Status", Width = 150 };
                Col_PostingDateSAP = new DataGridViewTextBoxColumn() { Name = "Posting_Date_SAP", HeaderText = "Posting Date SAP", Width = 150 };
                Col_DocumentNumberSAP = new DataGridViewTextBoxColumn() { Name = "Document_number_SAP", HeaderText = "Document number SAP", Width = 150 };

                this.adtvgConciliation.Columns.Add(Col_IdDocNumber); this.adtvgConciliation.Columns.Add(Col_EntruDate); this.adtvgConciliation.Columns.Add(Col_Guid);
                this.adtvgConciliation.Columns.Add(Col_UserName); this.adtvgConciliation.Columns.Add(Col_DocNumberCAT); this.adtvgConciliation.Columns.Add(Col_Tool);
                this.adtvgConciliation.Columns.Add(Col_Status); this.adtvgConciliation.Columns.Add(Col_PostingDateSAP); this.adtvgConciliation.Columns.Add(Col_DocumentNumberSAP);

                // Elimina última fila de Datagrid Customer Conciliation
                adtvgConciliation.AllowUserToAddRows = false;

                //Genero conexión a la base de datos
                using (Models.SSC_Finance_DWEntities DataBaseUser = new Models.SSC_Finance_DWEntities())
                {
                    // Busca nombre de tabla USER DESKTOP APP *******************************************
                    var userNames = (from d in DataBaseUser.CASH_APPLICATION___Users_Desktop_App
                                     where (d.user_id == Environment.UserName)
                                     select d.user_names).ToList().FirstOrDefault();
               
                    // Completamos información de las columnas agregadas
                    for (int i = 0; i < adtvgConciliation.RowCount; i++)
                    {
                        string userLogin = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

                        if ((string)adtvgConciliation.Rows[i].Cells[0].Value != string.Empty && i < adtvgConciliation.RowCount)
                        {
                            adtvgConciliation.Rows[i].Cells[22].Value = string.Concat((string)adtvgConciliation.Rows[i].Cells[0].Value + (string)adtvgConciliation.Rows[i].Cells[9].Value);
                            adtvgConciliation.Rows[i].Cells[23].Value = userLogin;
                            adtvgConciliation.Rows[i].Cells[24].Value = Environment.UserName;
                            adtvgConciliation.Rows[i].Cells[25].Value = userNames;
                            adtvgConciliation.Rows[i].Cells[27].Value = MainInput.APPNAME;
                            adtvgConciliation.Rows[i].Cells[28].Value = "Pending F-32";
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Quita filas seleccionadas de Grid Conciliation
        private void pictLeft_Click(object sender, EventArgs e)
        {
            try
            {
                if (adtvgConciliation.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow rowPrincipal in adtvgConciliation.SelectedRows)
                    {
                        if (!rowPrincipal.IsNewRow)
                            adtvgConciliation.Rows.Remove(rowPrincipal);
                    }

                    adtvgConciliation.ClearSelection();
                    // Suma valores de columna Amount in Doc Curr (Col 6) de DataGridView Conciliation
                    //            
                    sumTotal();
                }
            }
            catch (Exception)
            {
                //Capurar error y enviar a base de datos ************************************************
                //***************************************************************************************
            }

        }


        // Pasar fila seleccionada a Grid Conciliation ***************************************************************************************       
        public void PictureRight_Click(object sender, EventArgs e)
        {
            if (AdtvgCustomerPay.SelectedRows.Count > 0 || AdtvgInvoices.SelectedRows.Count > 0 || AdtvgCreditNotes.SelectedRows.Count > 0 || AdtvgCreditBalance.SelectedRows.Count > 0 || AdtvgAllDoc.SelectedRows.Count > 0)
            {
                Stack<string> listDocuments = new Stack<string>();
                bool check = false;

                // Recorremos la colección de filas seleccionadas en el control ------------------------------------------ Tab 01 Customer Payment 
                foreach (DataGridViewRow rowPrincipal in AdtvgCustomerPay.SelectedRows)
                {
                    // Creamos un array con los valores que vamos a insertar en el segundo control DataGridView
                    // 
                    object[] values =
                    {
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

                    if (adtvgConciliation.RowCount >= 0)
                    {
                        for (int i = 0; i < adtvgConciliation.RowCount; i++)
                        {
                            if (Convert.ToString(adtvgConciliation.Rows[i].Cells[8].Value) == Convert.ToString(rowPrincipal.Cells[8].Value))
                            {
                                listDocuments.Push(Convert.ToString(rowPrincipal.Cells[8].Value));
                                check = true;
                            }
                        }

                        if (!check)
                        {
                            // Creamos un nuevo objeto DataGridViewRow.
                            //
                            DataGridViewRow row = new DataGridViewRow();
                            // Creamos las celdas y las rellenamos con los valores existentes en el array.
                            //
                            row.CreateCells(adtvgConciliation, values);
                            // Añadimos la nueva fila al segundo control DataGridView.
                            //
                            adtvgConciliation.Rows.Add(row);
                            adtvgConciliation.AllowUserToAddRows = true;
                        }
                    }
                }

                // Recorremos la colección de filas seleccionadas en el control -------------------------------------------------- Tab 02 Invoices   
                foreach (DataGridViewRow rowPrincipal in AdtvgInvoices.SelectedRows)
                {
                    // Creamos un array con los valores que vamos a insertar en el segundo control DataGridView.
                    object[] values =
                    {
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

                    if (adtvgConciliation.RowCount >= 0)
                    {
                        for (int i = 0; i < adtvgConciliation.RowCount; i++)
                        {
                            if (Convert.ToString(adtvgConciliation.Rows[i].Cells[8].Value) == Convert.ToString(rowPrincipal.Cells[8].Value))
                            {
                                listDocuments.Push(Convert.ToString(rowPrincipal.Cells[8].Value));
                                check = true;
                            }
                        }

                        if (!check)
                        {
                            // Creamos un nuevo objeto DataGridViewRow.
                            //
                            DataGridViewRow row = new DataGridViewRow();
                            // Creamos las celdas y las rellenamos con los valores existentes en el array.
                            //
                            row.CreateCells(adtvgConciliation, values);
                            // Añadimos la nueva fila al segundo control DataGridView.
                            //
                            adtvgConciliation.Rows.Add(row);
                            adtvgConciliation.AllowUserToAddRows = true;
                        }
                    }
                }

                // Recorremos la colección de filas seleccionadas en el control --------------------------------------------------- Tab 03 Credit Note   
                foreach (DataGridViewRow rowPrincipal in AdtvgCreditNotes.SelectedRows)
                {
                    // Creamos un array con los valores que vamos a insertar en el segundo control DataGridView.
                    object[] values =
                    {
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

                    if (adtvgConciliation.RowCount >= 0)
                    {
                        for (int i = 0; i < adtvgConciliation.RowCount; i++)
                        {
                            if (Convert.ToString(adtvgConciliation.Rows[i].Cells[8].Value) == Convert.ToString(rowPrincipal.Cells[8].Value))
                            {
                                listDocuments.Push(Convert.ToString(rowPrincipal.Cells[8].Value));
                                check = true;
                            }
                        }

                        if (!check)
                        {
                            // Creamos un nuevo objeto DataGridViewRow.
                            //
                            DataGridViewRow row = new DataGridViewRow();
                            // Creamos las celdas y las rellenamos con los valores existentes en el array.
                            //
                            row.CreateCells(adtvgConciliation, values);
                            // Añadimos la nueva fila al segundo control DataGridView.
                            //
                            adtvgConciliation.Rows.Add(row);
                            adtvgConciliation.AllowUserToAddRows = true;
                        }
                    }
                }

                // Recorremos la colección de filas seleccionadas en el control --------------------------------------------------- Tab 04 Credit Balance
                // DataGridView principal.  
                foreach (DataGridViewRow rowPrincipal in AdtvgCreditBalance.SelectedRows)
                {
                    // Creamos un array con los valores que vamos a insertar en el segundo control DataGridView.
                    object[] values =
                    {
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

                    if (adtvgConciliation.RowCount >= 0)
                    {
                        for (int i = 0; i < adtvgConciliation.RowCount; i++)
                        {
                            if (Convert.ToString(adtvgConciliation.Rows[i].Cells[8].Value) == Convert.ToString(rowPrincipal.Cells[8].Value))
                            {
                                listDocuments.Push(Convert.ToString(rowPrincipal.Cells[8].Value));
                                check = true;
                            }
                        }

                        if (!check)
                        {
                            // Creamos un nuevo objeto DataGridViewRow.
                            //
                            DataGridViewRow row = new DataGridViewRow();
                            // Creamos las celdas y las rellenamos con los valores existentes en el array.
                            //
                            row.CreateCells(adtvgConciliation, values);
                            // Añadimos la nueva fila al segundo control DataGridView.
                            //
                            adtvgConciliation.Rows.Add(row);
                            adtvgConciliation.AllowUserToAddRows = true;
                        }
                    }
                }

                // Recorremos la colección de filas seleccionadas en el control ------------------------------------------------ Tab 05 All Documents
                // DataGridView principal.            
                foreach (DataGridViewRow rowPrincipal in AdtvgAllDoc.SelectedRows)
                {
                    // Creamos un array con los valores que vamos a insertar en el segundo control DataGridView.
                    object[] values =
                    {
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

                    if (adtvgConciliation.RowCount >= 0)
                    {
                        for (int i = 0; i < adtvgConciliation.RowCount; i++)
                        {
                            if (Convert.ToString(adtvgConciliation.Rows[i].Cells[8].Value) == Convert.ToString(rowPrincipal.Cells[8].Value))
                            {
                                listDocuments.Push(Convert.ToString(rowPrincipal.Cells[8].Value));
                                check = true;
                            }
                        }

                        if (!check)
                        {
                            // Creamos un nuevo objeto DataGridViewRow.
                            //
                            DataGridViewRow row = new DataGridViewRow();
                            // Creamos las celdas y las rellenamos con los valores existentes en el array.
                            //
                            row.CreateCells(adtvgConciliation, values);
                            // Añadimos la nueva fila al segundo control DataGridView.
                            //
                            adtvgConciliation.Rows.Add(row);
                            adtvgConciliation.AllowUserToAddRows = true;
                        }
                    }
                }

                if (check)
                {
                    MessageBox.Show("The following documents have already been selected: " + "\n\n" + string.Join(", ", listDocuments.ToArray()) + "\n\n" + "Please review your conciliation (Column name: Document number). Thanks", MainInput.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listDocuments.Clear();
                }

                //Limpia selección
                //
                AdtvgCustomerPay.ClearSelection();
                AdtvgInvoices.ClearSelection();
                AdtvgCreditNotes.ClearSelection();
                AdtvgCreditBalance.ClearSelection();
                AdtvgAllDoc.ClearSelection();

                // Suma valores de columna Amount in Doc Curre (Col 6) de DataGridView Conciliation
                //            
                sumTotal();
            }
        }
       
        // Quita todas las filas seleccionadas en Datagrid Conciliation ***************************************************************************************
        private void pictUnselectAll_Click(object sender, EventArgs e)
        {
            if (adtvgConciliation.RowCount > 0)
            {
                DialogResult dialogRemoveAll = MessageBox.Show("Are you sure to remove all documents from your conciliation?", MainInput.APPNAME, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dialogRemoveAll == DialogResult.Yes)
                {
                    clearConciliation();
                    sumTotal();               
                }
            }
        }

        void clearConciliation()
        {
            adtvgConciliation.Rows.Clear();
            txtReceiptNumber.Clear();
            adtvgConciliation.AllowUserToAddRows = false;
        }

        // Llama a método para Exportar Datagrid Conciliation a Excel ***************************************************************************************
        private void pictExcel_Click(object sender, EventArgs e)
        {
            exportExcel();
        }

        // Llama a método para Exportar Datagrid Conciliation a Excel ****************************************************************************************
        private void exportExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportExcel();
        }

        // Función para exportar DataGrid Conciliation a Excel ***********************************************************************************************
        void exportExcel()
        {
            try
            {
                if (adtvgConciliation.RowCount > 0)
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

                        //Obtener ubicación y el nombre del archivo de Excel para guardar según Usuario. 
                        SaveFileDialog saveDialog = new SaveFileDialog();
                        saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                        saveDialog.FilterIndex = 2;

                        if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            workbook.SaveAs(saveDialog.FileName);
                            MessageBox.Show("Export Successful", MainInput.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                else if (adtvgConciliation.RowCount == 0)
                {
                    MessageBox.Show("There isn´t information to export in your customer conciliation", MainInput.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        // Borra subtotales ***************************************************************************************************************************
        void clearSubtotales()
        {
            txtInvoicesTotal.Clear();
            txtCustomerPayTotal.Clear();
            txtCreditNotesTotal.Clear();
            txtCreditBalanceTotal.Clear();
            txtDisputesTotal.Clear();
            txtSubtotal.Clear();
        }

        // Abre ventana de busqueda de cliente por texto **********************************************************************************************
        private void pictFormSearch_Click(object sender, EventArgs e)
        {
            callFormSearch();
        }

        void callFormSearch()
        {
            FrmSearchCustomerName frmSearch = new FrmSearchCustomerName();
            AddOwnedForm(frmSearch);
            frmSearch.ShowDialog();
        }

        // Hover & Leave *******************************************************************************************************************************
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
        private void pictSubmitDb_MouseHover(object sender, EventArgs e)
        {
            this.pictSubmitDb.Size = new System.Drawing.Size(46, 40);
            this.pictSubmitDb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictSubmitDb.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void pictSubmitDb_MouseLeave(object sender, EventArgs e)
        {
            this.pictSubmitDb.Size = new System.Drawing.Size(43, 37);
            this.pictSubmitDb.BorderStyle = System.Windows.Forms.BorderStyle.None;
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

        // Ingresa número de recibo en campo Receipt number ******************************************************************************************
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

        // Carga Descripción de números de acuerdo ****************************************************************************************************
        private void adtvgConciliation_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int columnNumber = adtvgConciliation.CurrentCell.ColumnIndex;
            try
            {
                if(columnNumber == 20)
                {
                    cboDescriptionAgreement.Visible = true;
                    
                    //objMain.CompanyCode = Convert.ToString(adtvgConciliation.Rows[e.RowIndex].Cells[0].Value);
                    objMain.CustomerNumber = Convert.ToString(adtvgConciliation.Rows[0].Cells[1].Value);
                    //objMain.CustomerNumber = Convert.ToString(adtvgConciliation.Rows[0].Cells[2].Value);
                    string agreementNumber = Convert.ToString(adtvgConciliation.Rows[e.RowIndex].Cells[20].Value);
                    objMain.CompanyCode = cboCompanyCode.Text.ToString();

                    DsFbl5nTableAdapters.SP_DESCRIPTIONAGREEMENTTableAdapter daDs = new DsFbl5nTableAdapters.SP_DESCRIPTIONAGREEMENTTableAdapter();

                    daDs.Fill(ds.SP_DESCRIPTIONAGREEMENT, Convert.ToString(objMain.CompanyCode), Convert.ToDecimal(objMain.CustomerNumber), Convert.ToDecimal(agreementNumber));

                    cboDescriptionAgreement.DataSource = ds.SP_DESCRIPTIONAGREEMENT;
                    cboDescriptionAgreement.DisplayMember = "Descript_";

                    adtvgConciliation.Rows[e.RowIndex].Cells[21].Value = cboDescriptionAgreement.Text;                    
                }
                else if(columnNumber == 18)
                {
                    sumTotal();
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

        // Función para validar ingreso de números enteros ******************************************************************************************
        void validOnlyNumber(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                MessageBox.Show("This field only supports numbers", MainInput.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
            }
        }

        // Función para validar ingreso de números con decimales en columna Dispute Amount **********************************************************
        private void adtvgConciliation_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Columns_KeyPress);

            if (adtvgConciliation.CurrentCell.ColumnIndex == 18)
            {
                TextBox tb = e.Control as TextBox;

                if (tb != null)
                {
                     tb.KeyPress += new KeyPressEventHandler(Columns_KeyPress);
                    //Convierte a formato 1,000.00 números de columna de Dispute Amount
                    adtvgConciliation.Columns[18].ValueType = typeof(double);                    
                }
            }
        }

        //Función para validar ingreso de números y punto en columna de Dispute Amount
        private void Columns_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 45) || (e.KeyChar >= 58 && e.KeyChar <= 255) || (e.KeyChar == 47))
            {
                e.Handled = true;
            }
        }       
            
        private void adtvgConciliation_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {   
            // Validamos los campos de Disputes Amount, Reason Code y Description
            for (int i = 0; i < adtvgConciliation.RowCount; i++)
            {
                double colDisputeAmount = Convert.ToDouble(adtvgConciliation.Rows[i].Cells[18].Value);
                string colReasonCode = Convert.ToString(adtvgConciliation.Rows[i].Cells[19].Value);
                string colAgreement = Convert.ToString(adtvgConciliation.Rows[i].Cells[20].Value);
                string colDescription = Convert.ToString(adtvgConciliation.Rows[i].Cells[21].Value);

                if (colDisputeAmount != 0 && (colReasonCode == string.Empty || colDescription == string.Empty))
                {
                    adtvgConciliation.Rows[i].Cells[19].Style.BackColor = Color.DarkSalmon; adtvgConciliation.Rows[i].Cells[21].Style.BackColor = Color.DarkSalmon;
                }
                else if (colReasonCode != string.Empty && (colDisputeAmount == 0 || colDescription == string.Empty))
                {
                    adtvgConciliation.Rows[i].Cells[18].Style.BackColor = Color.DarkSalmon; adtvgConciliation.Rows[i].Cells[21].Style.BackColor = Color.DarkSalmon;
                    
                    // Carga company code y habilita columna Reference
                    if(Convert.ToString(adtvgConciliation.Rows[i].Cells[0].Value) == string.Empty)
                    {
                        adtvgConciliation.Rows[i].Cells[0].Value = adtvgConciliation.Rows[0].Cells[0].Value;
                        adtvgConciliation.Rows[i].Cells[1].Value = adtvgConciliation.Rows[0].Cells[1].Value;
                        adtvgConciliation.Rows[i].Cells[2].Value = adtvgConciliation.Rows[0].Cells[2].Value;
                        adtvgConciliation.Rows[i].Cells[3].Value = adtvgConciliation.Rows[0].Cells[3].Value;
                        adtvgConciliation.Rows[i].Cells[15].Value = adtvgConciliation.Rows[0].Cells[15].Value;
                        adtvgConciliation.Columns[9].ReadOnly = false;
                    }
                }
                else if (colAgreement != string.Empty && (colDisputeAmount == 0 || colReasonCode == string.Empty || colDescription == string.Empty))
                {
                    adtvgConciliation.Rows[i].Cells[18].Style.BackColor = Color.DarkSalmon; adtvgConciliation.Rows[i].Cells[19].Style.BackColor = Color.DarkSalmon;
                    adtvgConciliation.Rows[i].Cells[21].Style.BackColor = Color.DarkSalmon;
                }
                else if (colDescription != string.Empty && (colDisputeAmount == 0 || colReasonCode == string.Empty))
                {
                    adtvgConciliation.Rows[i].Cells[18].Style.BackColor = Color.DarkSalmon; adtvgConciliation.Rows[i].Cells[19].Style.BackColor = Color.DarkSalmon;
                }
                else
                {
                    adtvgConciliation.Rows[i].Cells[18].Style.BackColor = Color.Empty; adtvgConciliation.Rows[i].Cells[19].Style.BackColor = Color.Empty;
                    adtvgConciliation.Rows[i].Cells[20].Style.BackColor = Color.Empty; adtvgConciliation.Rows[i].Cells[21].Style.BackColor = Color.Empty;
                }
            }
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictSearch_Click(Owner, e);
        }

        private void txtCustomerNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F8)
            {
                PictSearch_Click(Owner, e);
            }
        }

        private void findCustomersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            callFormSearch();
        }

        private void cboDescriptionAgreement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F10)
            {
                pictSubmitDb_Click(Owner, e);
            }
        }

        private void submitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictSubmitDb_Click(Owner, e);
        }

        private void emailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            objFrmError.sendEmail();
        }

        private void whatsappToolStripMenuItem_Click(object sender, EventArgs e)
        {
            objFrmError.sendWhatsApp();
        }
    }
}

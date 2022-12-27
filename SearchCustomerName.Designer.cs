
namespace CFS_Latam_cashApplicationTool
{
    partial class FrmSearchCustomerName
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSearchCustomerName));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cboCoCdSearch = new System.Windows.Forms.ComboBox();
            this.lblCompanyCode = new System.Windows.Forms.Label();
            this.txtLettersFind = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblTitleFilter = new System.Windows.Forms.Label();
            this.AdtvgSearchCustomer = new Zuby.ADGV.AdvancedDataGridView();
            this.customerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.altPayerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.companyCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sPFINDCUSTOMERBYTEXTBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsFbl5n = new CFS_Latam_cashApplicationTool.DsFbl5n();
            this.sP_FINDCUSTOMERBYTEXTTableAdapter = new CFS_Latam_cashApplicationTool.DsFbl5nTableAdapters.SP_FINDCUSTOMERBYTEXTTableAdapter();
            this.panelGridView = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AdtvgSearchCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPFINDCUSTOMERBYTEXTBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsFbl5n)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Highlight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(675, 49);
            this.panel1.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblTitle.Location = new System.Drawing.Point(223, 11);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(229, 25);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "Search Customer by text";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.tableLayoutPanel1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 49);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(675, 84);
            this.panel2.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.10345F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.72414F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.586207F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.58621F));
            this.tableLayoutPanel1.Controls.Add(this.cboCoCdSearch, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCompanyCode, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtLettersFind, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(39, 37);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(580, 32);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // cboCoCdSearch
            // 
            this.cboCoCdSearch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cboCoCdSearch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cboCoCdSearch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboCoCdSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCoCdSearch.FormattingEnabled = true;
            this.cboCoCdSearch.Location = new System.Drawing.Point(108, 4);
            this.cboCoCdSearch.Name = "cboCoCdSearch";
            this.cboCoCdSearch.Size = new System.Drawing.Size(139, 23);
            this.cboCoCdSearch.TabIndex = 2;
            this.cboCoCdSearch.SelectedIndexChanged += new System.EventHandler(this.cboCoCdSearch_SelectedIndexChanged);
            this.cboCoCdSearch.Click += new System.EventHandler(this.cboCoCdSearch_Click);
            this.cboCoCdSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboCoCdSearch_KeyPress);
            this.cboCoCdSearch.Leave += new System.EventHandler(this.cboCoCdSearch_Leave);
            // 
            // lblCompanyCode
            // 
            this.lblCompanyCode.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCompanyCode.AutoSize = true;
            this.lblCompanyCode.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblCompanyCode.Location = new System.Drawing.Point(17, 9);
            this.lblCompanyCode.Name = "lblCompanyCode";
            this.lblCompanyCode.Size = new System.Drawing.Size(85, 13);
            this.lblCompanyCode.TabIndex = 5;
            this.lblCompanyCode.Text = "Company Code";
            // 
            // txtLettersFind
            // 
            this.txtLettersFind.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLettersFind.Location = new System.Drawing.Point(335, 3);
            this.txtLettersFind.Name = "txtLettersFind";
            this.txtLettersFind.Size = new System.Drawing.Size(223, 23);
            this.txtLettersFind.TabIndex = 7;
            this.txtLettersFind.TextChanged += new System.EventHandler(this.txtLettersFind_TextChanged);
            this.txtLettersFind.Enter += new System.EventHandler(this.txtLettersFind_Enter);
            this.txtLettersFind.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLettersFind_KeyPress);
            this.txtLettersFind.Leave += new System.EventHandler(this.txtLettersFind_Leave);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel3.Controls.Add(this.lblTitleFilter);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(673, 25);
            this.panel3.TabIndex = 0;
            // 
            // lblTitleFilter
            // 
            this.lblTitleFilter.AutoSize = true;
            this.lblTitleFilter.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitleFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTitleFilter.Location = new System.Drawing.Point(10, 4);
            this.lblTitleFilter.Name = "lblTitleFilter";
            this.lblTitleFilter.Size = new System.Drawing.Size(46, 17);
            this.lblTitleFilter.TabIndex = 4;
            this.lblTitleFilter.Text = "Filters";
            // 
            // AdtvgSearchCustomer
            // 
            this.AdtvgSearchCustomer.AllowUserToDeleteRows = false;
            this.AdtvgSearchCustomer.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.AdtvgSearchCustomer.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.AdtvgSearchCustomer.AutoGenerateColumns = false;
            this.AdtvgSearchCustomer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.AdtvgSearchCustomer.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            this.AdtvgSearchCustomer.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.AdtvgSearchCustomer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.AdtvgSearchCustomer.ColumnHeadersHeight = 34;
            this.AdtvgSearchCustomer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.AdtvgSearchCustomer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.customerDataGridViewTextBoxColumn,
            this.altPayerDataGridViewTextBoxColumn,
            this.customerNameDataGridViewTextBoxColumn,
            this.companyCodeDataGridViewTextBoxColumn});
            this.AdtvgSearchCustomer.DataSource = this.sPFINDCUSTOMERBYTEXTBindingSource;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.AdtvgSearchCustomer.DefaultCellStyle = dataGridViewCellStyle3;
            this.AdtvgSearchCustomer.EnableHeadersVisualStyles = false;
            this.AdtvgSearchCustomer.FilterAndSortEnabled = true;
            this.AdtvgSearchCustomer.FilterStringChangedInvokeBeforeDatasourceUpdate = true;
            this.AdtvgSearchCustomer.Location = new System.Drawing.Point(40, 156);
            this.AdtvgSearchCustomer.Margin = new System.Windows.Forms.Padding(15);
            this.AdtvgSearchCustomer.Name = "AdtvgSearchCustomer";
            this.AdtvgSearchCustomer.ReadOnly = true;
            this.AdtvgSearchCustomer.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.AdtvgSearchCustomer.Size = new System.Drawing.Size(580, 294);
            this.AdtvgSearchCustomer.SortStringChangedInvokeBeforeDatasourceUpdate = true;
            this.AdtvgSearchCustomer.TabIndex = 2;
            this.AdtvgSearchCustomer.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.AdtvgSearchCustomer_CellDoubleClick);
            // 
            // customerDataGridViewTextBoxColumn
            // 
            this.customerDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.customerDataGridViewTextBoxColumn.DataPropertyName = "Customer";
            this.customerDataGridViewTextBoxColumn.HeaderText = "Customer";
            this.customerDataGridViewTextBoxColumn.MinimumWidth = 22;
            this.customerDataGridViewTextBoxColumn.Name = "customerDataGridViewTextBoxColumn";
            this.customerDataGridViewTextBoxColumn.ReadOnly = true;
            this.customerDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.customerDataGridViewTextBoxColumn.Width = 83;
            // 
            // altPayerDataGridViewTextBoxColumn
            // 
            this.altPayerDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.altPayerDataGridViewTextBoxColumn.DataPropertyName = "Alt Payer";
            this.altPayerDataGridViewTextBoxColumn.HeaderText = "Alt Payer";
            this.altPayerDataGridViewTextBoxColumn.MinimumWidth = 22;
            this.altPayerDataGridViewTextBoxColumn.Name = "altPayerDataGridViewTextBoxColumn";
            this.altPayerDataGridViewTextBoxColumn.ReadOnly = true;
            this.altPayerDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.altPayerDataGridViewTextBoxColumn.Width = 72;
            // 
            // customerNameDataGridViewTextBoxColumn
            // 
            this.customerNameDataGridViewTextBoxColumn.DataPropertyName = "Customer Name";
            this.customerNameDataGridViewTextBoxColumn.HeaderText = "Customer Name";
            this.customerNameDataGridViewTextBoxColumn.MinimumWidth = 22;
            this.customerNameDataGridViewTextBoxColumn.Name = "customerNameDataGridViewTextBoxColumn";
            this.customerNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.customerNameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // companyCodeDataGridViewTextBoxColumn
            // 
            this.companyCodeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.companyCodeDataGridViewTextBoxColumn.DataPropertyName = "Company Code";
            this.companyCodeDataGridViewTextBoxColumn.HeaderText = "Company Code";
            this.companyCodeDataGridViewTextBoxColumn.MinimumWidth = 22;
            this.companyCodeDataGridViewTextBoxColumn.Name = "companyCodeDataGridViewTextBoxColumn";
            this.companyCodeDataGridViewTextBoxColumn.ReadOnly = true;
            this.companyCodeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.companyCodeDataGridViewTextBoxColumn.Width = 105;
            // 
            // sPFINDCUSTOMERBYTEXTBindingSource
            // 
            this.sPFINDCUSTOMERBYTEXTBindingSource.DataMember = "SP_FINDCUSTOMERBYTEXT";
            this.sPFINDCUSTOMERBYTEXTBindingSource.DataSource = this.dsFbl5n;
            // 
            // dsFbl5n
            // 
            this.dsFbl5n.DataSetName = "DsFbl5n";
            this.dsFbl5n.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // sP_FINDCUSTOMERBYTEXTTableAdapter
            // 
            this.sP_FINDCUSTOMERBYTEXTTableAdapter.ClearBeforeFill = true;
            // 
            // panelGridView
            // 
            this.panelGridView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGridView.Location = new System.Drawing.Point(0, 0);
            this.panelGridView.Name = "panelGridView";
            this.panelGridView.Size = new System.Drawing.Size(675, 472);
            this.panelGridView.TabIndex = 3;
            // 
            // FrmSearchCustomerName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(675, 472);
            this.Controls.Add(this.AdtvgSearchCustomer);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(691, 511);
            this.MinimumSize = new System.Drawing.Size(691, 511);
            this.Name = "FrmSearchCustomerName";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Search Customer - Cash Application Tool CFS Latam";
            this.Load += new System.EventHandler(this.FrmSearchCustomerName_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSearchCustomerName_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AdtvgSearchCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sPFINDCUSTOMERBYTEXTBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsFbl5n)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblTitleFilter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblCompanyCode;
        private System.Windows.Forms.ComboBox cboCoCdSearch;
        private System.Windows.Forms.TextBox txtLettersFind;
        private Zuby.ADGV.AdvancedDataGridView AdtvgSearchCustomer;
        private System.Windows.Forms.BindingSource sPFINDCUSTOMERBYTEXTBindingSource;
        private DsFbl5n dsFbl5n;
        private DsFbl5nTableAdapters.SP_FINDCUSTOMERBYTEXTTableAdapter sP_FINDCUSTOMERBYTEXTTableAdapter;
        private System.Windows.Forms.Panel panelGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn altPayerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn companyCodeDataGridViewTextBoxColumn;
    }
}
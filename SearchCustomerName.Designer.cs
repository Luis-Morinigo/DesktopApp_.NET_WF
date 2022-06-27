
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSearchCustomerName));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lblCompanyCode = new System.Windows.Forms.Label();
            this.txtLettersFind = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblTitleFilter = new System.Windows.Forms.Label();
            this.advancedDataGridView1 = new Zuby.ADGV.AdvancedDataGridView();
            this.customerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.altPayerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.companyCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sPFINDCUSTOMERBYTEXTBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsFbl5n = new CFS_Latam_cashApplicationTool.DsFbl5n();
            this.sP_FINDCUSTOMERBYTEXTTableAdapter = new CFS_Latam_cashApplicationTool.DsFbl5nTableAdapters.SP_FINDCUSTOMERBYTEXTTableAdapter();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advancedDataGridView1)).BeginInit();
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
            this.panel1.Size = new System.Drawing.Size(653, 49);
            this.panel1.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblTitle.Location = new System.Drawing.Point(212, 11);
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
            this.panel2.Size = new System.Drawing.Size(653, 84);
            this.panel2.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.19355F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.80645F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 275F));
            this.tableLayoutPanel1.Controls.Add(this.comboBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCompanyCode, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtLettersFind, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(22, 31);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(545, 37);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(105, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(123, 21);
            this.comboBox1.TabIndex = 2;
            // 
            // lblCompanyCode
            // 
            this.lblCompanyCode.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCompanyCode.AutoSize = true;
            this.lblCompanyCode.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblCompanyCode.Location = new System.Drawing.Point(14, 12);
            this.lblCompanyCode.Name = "lblCompanyCode";
            this.lblCompanyCode.Size = new System.Drawing.Size(85, 13);
            this.lblCompanyCode.TabIndex = 5;
            this.lblCompanyCode.Text = "Company Code";
            // 
            // txtLettersFind
            // 
            this.txtLettersFind.Location = new System.Drawing.Point(272, 3);
            this.txtLettersFind.Name = "txtLettersFind";
            this.txtLettersFind.Size = new System.Drawing.Size(223, 20);
            this.txtLettersFind.TabIndex = 7;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panel3.Controls.Add(this.lblTitleFilter);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(651, 25);
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
            // advancedDataGridView1
            // 
            this.advancedDataGridView1.AllowUserToDeleteRows = false;
            this.advancedDataGridView1.AutoGenerateColumns = false;
            this.advancedDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.advancedDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.customerDataGridViewTextBoxColumn,
            this.altPayerDataGridViewTextBoxColumn,
            this.customerNameDataGridViewTextBoxColumn,
            this.companyCodeDataGridViewTextBoxColumn});
            this.advancedDataGridView1.DataSource = this.sPFINDCUSTOMERBYTEXTBindingSource;
            this.advancedDataGridView1.FilterAndSortEnabled = true;
            this.advancedDataGridView1.FilterStringChangedInvokeBeforeDatasourceUpdate = true;
            this.advancedDataGridView1.Location = new System.Drawing.Point(40, 156);
            this.advancedDataGridView1.Name = "advancedDataGridView1";
            this.advancedDataGridView1.ReadOnly = true;
            this.advancedDataGridView1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.advancedDataGridView1.Size = new System.Drawing.Size(580, 284);
            this.advancedDataGridView1.SortStringChangedInvokeBeforeDatasourceUpdate = true;
            this.advancedDataGridView1.TabIndex = 2;
            // 
            // customerDataGridViewTextBoxColumn
            // 
            this.customerDataGridViewTextBoxColumn.DataPropertyName = "Customer";
            this.customerDataGridViewTextBoxColumn.HeaderText = "Customer";
            this.customerDataGridViewTextBoxColumn.MinimumWidth = 22;
            this.customerDataGridViewTextBoxColumn.Name = "customerDataGridViewTextBoxColumn";
            this.customerDataGridViewTextBoxColumn.ReadOnly = true;
            this.customerDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.customerDataGridViewTextBoxColumn.Width = 80;
            // 
            // altPayerDataGridViewTextBoxColumn
            // 
            this.altPayerDataGridViewTextBoxColumn.DataPropertyName = "Alt Payer";
            this.altPayerDataGridViewTextBoxColumn.HeaderText = "Alt Payer";
            this.altPayerDataGridViewTextBoxColumn.MinimumWidth = 22;
            this.altPayerDataGridViewTextBoxColumn.Name = "altPayerDataGridViewTextBoxColumn";
            this.altPayerDataGridViewTextBoxColumn.ReadOnly = true;
            this.altPayerDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.altPayerDataGridViewTextBoxColumn.Width = 80;
            // 
            // customerNameDataGridViewTextBoxColumn
            // 
            this.customerNameDataGridViewTextBoxColumn.DataPropertyName = "Customer Name";
            this.customerNameDataGridViewTextBoxColumn.HeaderText = "Customer Name";
            this.customerNameDataGridViewTextBoxColumn.MinimumWidth = 22;
            this.customerNameDataGridViewTextBoxColumn.Name = "customerNameDataGridViewTextBoxColumn";
            this.customerNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.customerNameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.customerNameDataGridViewTextBoxColumn.Width = 300;
            // 
            // companyCodeDataGridViewTextBoxColumn
            // 
            this.companyCodeDataGridViewTextBoxColumn.DataPropertyName = "Company Code";
            this.companyCodeDataGridViewTextBoxColumn.HeaderText = "Company Code";
            this.companyCodeDataGridViewTextBoxColumn.MinimumWidth = 22;
            this.companyCodeDataGridViewTextBoxColumn.Name = "companyCodeDataGridViewTextBoxColumn";
            this.companyCodeDataGridViewTextBoxColumn.ReadOnly = true;
            this.companyCodeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.companyCodeDataGridViewTextBoxColumn.Width = 80;
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
            // FrmSearchCustomerName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(653, 452);
            this.Controls.Add(this.advancedDataGridView1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmSearchCustomerName";
            this.Text = "Search Customer - Cash Application Tool CFS Latam";
            this.Load += new System.EventHandler(this.SearchCustomerName_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advancedDataGridView1)).EndInit();
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
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox txtLettersFind;
        private Zuby.ADGV.AdvancedDataGridView advancedDataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn altPayerDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn companyCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource sPFINDCUSTOMERBYTEXTBindingSource;
        private DsFbl5n dsFbl5n;
        private DsFbl5nTableAdapters.SP_FINDCUSTOMERBYTEXTTableAdapter sP_FINDCUSTOMERBYTEXTTableAdapter;
    }
}
using CFS_Latam_cashApplicationTool.DsFbl5nTableAdapters;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CFS_Latam_cashApplicationTool
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        //Obtenemos ID de usuario
        private readonly string userName = Environment.UserName;
        //private readonly string userName = "A15470";
        public void pictLogin_Click(object sender, EventArgs e)
        {
            try
            {
                //Valida que exista información en base de datos FBL5N
                //
                using (var countDb = new SP_SELECTFBL5NTableAdapter())
                {
                    int DbRows = countDb.CountDbFBL5N().Value;
                    //int DbRows = 0;

                    //Genero conexión a la base de datos
                    using (Models.SSC_Finance_DWEntities DataBaseUser = new Models.SSC_Finance_DWEntities())
                    {
                        // Buscar datos del usuario en la tabla USER DESKTOP APP *******************************************
                        //var lstName = (from d in DataBaseUser.CASH_APPLICATION___Users_Desktop_App
                        //          where d.user_id == userName
                        //          select d.user_names).ToList();
                        //string primero = lstName.First();

                        //Consulta Linq si el Usuario se encuentra activo en la base de datos *******************************************
                        //var lst = from d in DataBaseUser.CASH_APPLICATION___Users_Desktop_App
                        //          where d.user_id == userName
                        //          select d;

                        //Consulta Linq si el Usuario se encuentra activo en la base de datos
                        var userStatus = (from d in DataBaseUser.CASH_APPLICATION___Users_Desktop_App
                                          where (d.user_id == userName)
                                          select d.user_status).ToList().FirstOrDefault();

                        if (userStatus == "Active" && DbRows != 0) // Usuario dado de alta && status Active && DB con info == LOGIN OK
                        {
                            //Oculta ventana Login
                            this.Hide();
                            //Creo objteo del formulario Main
                            FrmMain frmIn = new FrmMain();
                            //Evento lambda para cerrar ventana Login
                            frmIn.FormClosed += (s, args) => this.Close();
                            //Open Form frmMain
                            frmIn.Show();
                        }
                        else if (DbRows == 0) // DB no tiene info
                        {
                            //Oculta ventana Login
                            this.Hide();
                            //Creo objteo del formulario LoginError
                            FrmLoginError frmErr = new FrmLoginError();
                            //Evento lambda para cerrar ventana Login
                            frmErr.FormClosed += (s, args) => this.Close();
                            //Open Form LoginError                  
                            frmErr.Show();
                        }
                        else if (userStatus == "Inactive") // Usuario con status INACTIVE
                        {
                            MessageBox.Show($"User {userName} is registered but with status INACTIVE, please contact the CUSTOMER FINANCIAL SERVICES team to change the status to ACTIVE.", MainInput.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else if (string.IsNullOrEmpty(userStatus)) // Usuario no registrado en sistema
                        {
                            userStatus = "not registered";
                            MessageBox.Show($"User {userName} is not registered, please contact the CUSTOMER FINANCIAL SERVICES team to start the registration process in the system.", MainInput.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }

                        //MessageBox.Show(DateTime.Now.ToString("dd-MM-yyyy HH:m:ss"));

                        // Guardar login de usuario en DB User Activity *************************************************************
                        //***********************************************************************************************************

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void pictLogin_MouseHover(object sender, EventArgs e)
        {
            this.pictLogin.Size = new System.Drawing.Size(162, 59);
            this.pictLogin.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void pictLogin_MouseLeave(object sender, EventArgs e)
        {
            this.pictLogin.Size = new System.Drawing.Size(156, 53);
        }
    }
}

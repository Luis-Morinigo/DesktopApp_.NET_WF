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
using CFS_Latam_cashApplicationTool.Models.DBUsersActivity;
using System.Reflection;
using DocumentFormat.OpenXml.Office2013.Word;
using System.Threading;
using System.Runtime.Remoting.Lifetime;

namespace CFS_Latam_cashApplicationTool
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        //Obtenemos ID de usuario
        public readonly string userID = Environment.UserName;
        public int IDCreated { get; private set; }
        //private readonly string userID = "A15477";
        public void pictLogin_Click(object sender, EventArgs e)
        {
            try
            {  
                // Verifica que el componente este libre
                if(backgroundWorkerLogin.IsBusy != true)
                {                    
                    backgroundWorkerLogin.RunWorkerAsync();
                    this.pictureGifLogin.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            this.pictureGifLogin.Visible = false;
            this.lblVersion.Text = "Version " + Application.ProductVersion;
        }

        private void backgroundWorkerLogin_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(2000);
        }

        private void backgroundWorkerLogin_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
                        // Busca nombre de tabla USER DESKTOP APP *******************************************
                        var userNames = (from d in DataBaseUser.CASH_APPLICATION___Users_Desktop_App
                                         where (d.user_id == userID)
                                         select d.user_names).ToList().FirstOrDefault();

                        // Busca área de tabla USER DESKTOP APP *********************************************
                        var userArea = (from d in DataBaseUser.CASH_APPLICATION___Users_Desktop_App
                                        where (d.user_id == userID)
                                        select d.user_area).ToList().FirstOrDefault();

                        // Busca si el Usuario se encuentra activo en la base de datos **********************
                        var userStatus = (from d in DataBaseUser.CASH_APPLICATION___Users_Desktop_App
                                          where (d.user_id == userID)
                                          select d.user_status).ToList().FirstOrDefault();

                        // Usuario dado de alta && status Active && DB con info == LOGIN OK
                        if (userStatus == "Active" && DbRows != 0) 
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
                        // DB no tiene info
                        else if (DbRows == 0) 
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
                        // Usuario con status INACTIVE
                        else if (userStatus == "Inactive") 
                        {
                            MessageBox.Show($"User {userID} is registered but with status INACTIVE, please contact the CUSTOMER FINANCIAL SERVICES team to change the status to ACTIVE.", MainInput.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        // Usuario no registrado en sistema
                        else if (string.IsNullOrEmpty(userStatus)) 
                        {
                            userStatus = "Not registered";
                            MessageBox.Show($"User {userID} is not registered, please contact the CUSTOMER FINANCIAL SERVICES team to start the registration process in the system.", MainInput.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }

                        //dateHourLogin.ToString("dd-MM-yyyy HH:m:ss");
                        string userLogin = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

                        // Guardar login de usuario en DB User Activity *************************************************************
                        using (Models.DBUsersActivity.EntitiesUsersActivity dbLogin = new Models.DBUsersActivity.EntitiesUsersActivity())
                        {
                            CASH_APPLICATION___Users_Activity oUserActivity = new CASH_APPLICATION___Users_Activity();
                            oUserActivity.users_id = userID;
                            oUserActivity.users_names = userNames;
                            oUserActivity.users_area = userArea;
                            oUserActivity.users_status = userStatus;
                            oUserActivity.usersDatetime_login = userLogin;
                            oUserActivity.usersDatetime_logout = "";
                            oUserActivity.usersClose_window = "";

                            dbLogin.CASH_APPLICATION___Users_Activity.Add(oUserActivity);
                            dbLogin.SaveChanges();

                            // Obtenemos ID del registro
                            int IDCreated = oUserActivity.id;
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("error occurred while establishing a connection to SQL Server"))
                {
                    MessageBox.Show("Could not establish connection to the server." + "\n\n" + "Please check your VPN connection.", MainInput.APPNAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
        }

        // Hover & Leave ********************************************************************************
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

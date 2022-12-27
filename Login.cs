using CFS_Latam_cashApplicationTool.DsFbl5nTableAdapters;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
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
 
        public void pictLogin_Click(object sender, EventArgs e)
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
                    //Consulta mediante Linq si el Usuario se encuentra activo en la base de datos
                    var lst = from d in DataBaseUser.CASH_APPLICATION___Users_Desktop_App
                              where d.user_id == userName
                              select d;

                    if (lst.Count() > 0 && DbRows != 0) // Si usuario se encuentra y la DB tiene info
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
                    else if (DbRows == 0) // Si la DB no tiene info
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
                    else if (lst.Count() == 0) // Si el usuario no esta dado de alta
                    {
                        MessageBox.Show("Unregistered user", "Cash Application Tool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
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

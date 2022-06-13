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
        private void FrmLogin_Load(object sender, EventArgs e)
        {
            txtUserId.Text = userName;            
        }

        private void panelLogin_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictLogin_Click(object sender, EventArgs e)
        {
            //Obtenemos password ingresada por usuario
            string sPass = txtPasword.Text.Trim();
            //Obtenemos ID de usuario
            string sUser = txtUserId.Text.Trim();

            if (sPass == string.Empty)
            {
                MessageBox.Show("Please enter Password..!!", "Cash Application Tool");
            }
            else
            {
                //Genero conexión a la base de datos
                using (Models.SSC_Finance_DWEntities DataBaseUser = new Models.SSC_Finance_DWEntities())
                {
                    //Consulta mediante Linq
                    var lst = from d in DataBaseUser.CASH_APPLICATION___Users_Desktop_App
                              where d.user_id == txtUserId.Text
                              && d.password == sPass
                              select d;

                    if (lst.Count() > 0)
                    {
                        MessageBox.Show("Welcome to Cash Application Tool..!!", "Cash Application Tool");

                        //Oculta ventana Login
                        this.Hide();
                        //Creo objteo del formulario Main
                        FrmMain frmIn = new FrmMain();

                        //Evento lambda para cerrar ventana Login
                        frmIn.FormClosed += (s, args) => this.Close();
                        //Open Form frmMain
                        frmIn.Show();
                    }
                    else
                    {
                        MessageBox.Show("Unregistered user or wrong password", "Cash Application Tool");
                    }

                }

            }

        }

        private void panelLogo_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

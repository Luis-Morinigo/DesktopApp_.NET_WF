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
 
        private void pictLogin_Click(object sender, EventArgs e)
        {
            //Genero conexión a la base de datos
            using (Models.SSC_Finance_DWEntities DataBaseUser = new Models.SSC_Finance_DWEntities())
            {
                //Consulta mediante Linq
                var lst = from d in DataBaseUser.CASH_APPLICATION___Users_Desktop_App
                            where d.user_id == userName
                            select d;

                if (lst.Count() > 0)
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
                else
                {
                    MessageBox.Show("Unregistered user", "Cash Application Tool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }

            }

        }

    }
}

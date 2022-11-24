using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CFS_Latam_cashApplicationTool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmLogin());
            //Application.Run(new FrmMain());
            //Application.Run(new FrmDisputes()); 
            //Application.Run(new FrmLoading()); 
            //Application.Run(new FrmSearchCustomerName()); 
        }
    }
}

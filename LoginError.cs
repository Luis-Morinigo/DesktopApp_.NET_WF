using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Web.UI.WebControls;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.Office.Interop.Outlook;
using Exception = System.Exception;

namespace CFS_Latam_cashApplicationTool
{
    public partial class FrmLoginError : Form
    {
        public FrmLoginError()
        {
            InitializeComponent();
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Abrir Whatsapp ******************************************************************************************************
        public void sendWhatsApp()
        {
            try
            {
                //Genero conexión a la base de datos
                using (Models.DBConctact.SSC_Finance_DWEntities2 whatsapplDhl = new Models.DBConctact.SSC_Finance_DWEntities2())
                {
                    // Traer número de Whatsapp de DB a lista
                    var lst = (from d in whatsapplDhl.CASH_APPLICATION___Contacto_DHL
                               where d.Whatsapp.Contains("54")
                               select d).ToList();

                    foreach (var e in lst)
                    {
                        List<string> lstAllRecipients = new List<string>();
                        lstAllRecipients.Add(e.Whatsapp);
                        // Traer NUMERO DE WHATSAPP de DB
                        System.Diagnostics.Process.Start("https://web.whatsapp.com/" + "send?phone=" + e.Whatsapp + "&text=" + "Hola team DHL, me ayudan por favor");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void pictWhatsapp_Click(object sender, EventArgs e)
        {
            sendWhatsApp();
        }

        // Abrir Outlook ******************************************************************************************************
        public void sendEmail()
        {
            try
            {
                // Abre Outlook en caso de que no este abierto
                Outlook.Application outlookObj = null;

                if (Process.GetProcessesByName("OUTLOOK").Count().Equals(0))
                {
                    Process.Start("outlook.exe");
                }
                var process = Process.GetProcessesByName("OUTLOOK").First();
                while (!process.HasExited)
                {
                    try
                    {
                        outlookObj = (Outlook.Application)Marshal.GetActiveObject("Outlook.Application");
                        break;
                    }
                    catch
                    {
                        outlookObj = null;
                    }
                }

                //Genero conexión a la base de datos
                using (Models.DBConctact.SSC_Finance_DWEntities2 mailDhl = new Models.DBConctact.SSC_Finance_DWEntities2())
                {
                    // Traer mail de DB a lista
                    var lst = (from d in mailDhl.CASH_APPLICATION___Contacto_DHL
                              where d.Mail_HelpDesk.Contains("scj")
                              select d).ToList();

                    foreach (var e in lst)
                    {
                        List<string> lstAllRecipients = new List<string>();                        
                        lstAllRecipients.Add(e.Mail_HelpDesk);

                        Outlook.Application outlookApp = new Outlook.Application();
                        Outlook._MailItem oMailItem = (Outlook._MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);
                        Outlook.Inspector oInspector = oMailItem.GetInspector;

                        // Recipient
                        Outlook.Recipients oRecips = (Outlook.Recipients)oMailItem.Recipients;
                        foreach (String recipient in lstAllRecipients)
                        {
                            Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(recipient);
                            oRecip.Resolve();
                        }

                        // Add CC
                        //Outlook.Recipient oCCRecip = oRecips.Add("test@testmail.com");
                        //oCCRecip.Type = (int)Outlook.OlMailRecipientType.olCC;
                        //oCCRecip.Resolve();

                        // Agrega Subject 
                        oMailItem.Subject = ".NET: Cash Application Tool - Consulta";
                        oMailItem.Body = "Hola team DHL," + "\n\n" + "Podrian ayudarme con...";

                        // Display mail
                        oMailItem.Display(true);
                    }
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Hover & Leave ******************************************************************************************************
        private void lblClose_MouseHover(object sender, EventArgs e)
        {
            this.lblClose.Size = new System.Drawing.Size(36, 35);
            this.lblClose.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblClose.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void lblClose_MouseLeave(object sender, EventArgs e)
        {
            this.lblClose.Size = new System.Drawing.Size(34, 33);
            this.lblClose.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        private void pictWhatsapp_MouseHover(object sender, EventArgs e)
        {
            this.pictWhatsapp.Size = new System.Drawing.Size(133, 123);
            this.pictWhatsapp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictWhatsapp.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void pictWhatsapp_MouseLeave(object sender, EventArgs e)
        {
            this.pictWhatsapp.Size = new System.Drawing.Size(129, 120);
            this.pictWhatsapp.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        private void pictEmail_MouseHover(object sender, EventArgs e)
        {
            this.pictEmail.Size = new System.Drawing.Size(133, 123);
            this.pictEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictEmail.Cursor = System.Windows.Forms.Cursors.Hand;
        }

        private void pictEmail_MouseLeave(object sender, EventArgs e)
        {
            this.pictEmail.Size = new System.Drawing.Size(129, 120);
            this.pictEmail.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        private void pictEmail_Click(object sender, EventArgs e)
        {
            sendEmail();
        }
    }
}

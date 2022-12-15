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

        // Abre Whatsapp ******************************************************************************************************
        private void sendWhatsApp()
        {
            try
            {
                // Traer NUMERO DE WHATSAPP de DB
                System.Diagnostics.Process.Start("https://web.whatsapp.com/" + "send?phone=" + "5491131352843" + "&text=" + "Hola team DHL, me ayudan por favor");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictWhatsapp_Click(object sender, EventArgs e)
        {
            sendWhatsApp();
        }

        // Abre Outlook ******************************************************************************************************
        private void sendEmail()
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

                List<string> lstAllRecipients = new List<string>();
                // Traer mail de DB
                lstAllRecipients.Add("digitalhublatam@scj.com");

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

                //Add CC
                //Outlook.Recipient oCCRecip = oRecips.Add("test@testmail.com");
                //oCCRecip.Type = (int)Outlook.OlMailRecipientType.olCC;
                //oCCRecip.Resolve();

                //Add Subject
                oMailItem.Subject = ".NET: Cash Application Tool - Consulta";
                oMailItem.Body = "Hola team DHL," + "\n\n" + "Podrian ayudarme con...";

                //Display the mailbox
                oMailItem.Display(true);
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

using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CFS_Latam_cashApplicationTool
{
    public class MainInput
    {
        // Name
        static public string APPNAME = "Cash Application Tool";
        // Atributos Públicos de la Clase
        private string _CompanyCode;
        public string CompanyCode
        {
            get { return _CompanyCode; }
            set { _CompanyCode = value; }
        }

        private string _CustomerNumber;
        public string CustomerNumber
        {
            get { return _CustomerNumber; }
            set { _CustomerNumber = value; }
        }

        private string _AltNumber;
        public string AltNumber
        {
            get { return _AltNumber; }
            set { _AltNumber = value; }
        }
        private string _DocDZ;
        public string DocDZ
        {
            get { return _DocDZ; }
            set { _DocDZ = value; }
        }

        private string _DocRV;
        public string DocRV
        {
            get { return _DocRV; }
            set { _DocRV = value; }
        }

        private string _DocDV;
        public string DocDV
        {
            get { return _DocDV; }
            set { _DocDV = value; }
        }

        private string _DocDR;
        public string DocDR
        {
            get { return _DocDR; }
            set { _DocDR = value; }
        }

        private string _DocRG;
        public string DocRG
        {
            get { return _DocRG; }
            set { _DocRG = value; }
        }

        private string _DocDG;
        public string DocDG
        {
            get { return _DocDG; }
            set { _DocDG = value; }
        }

        private string _DocAB;
        public string DocAB
        {
            get { return _DocAB; }
            set { _DocAB = value; }
        }

        private string _DocRC;
        public string DocRC
        {
            get { return _DocRC; }
            set { _DocRC = value; }
        }
    }
}

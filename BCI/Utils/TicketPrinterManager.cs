using System;
using System.ComponentModel;

namespace BCI
    {
    public class TicketPrinterManager : INotifyPropertyChanged
        {
        public event PropertyChangedEventHandler PropertyChanged;
        
        string TicketPrinterName { get; set; }

        public TicketPrinterManager()
            {
            TicketPrinterName = Properties.Settings.Default.TicketPrinter;

            }

    public void ImprimirTicket(string sID, string sFecha, string sChapa, string sEmp, string sProd, string sBruto, string sTara, string sNeto)
        {                        

        string str = "";

        // header-----------------------------------------------------------------------

        str += (char)(27) + (char)116 + (char)(16); // Code PAge WPC1252

        str += (char)(27) + (char)(114) + (char)(0); // color negro
        str += (char)(27) + (char)(97) + (char)(1); // alineacion centrada
        str += (char)(27) + (char)(33) + (char)(5) + "COOPERATIVA CHORTITZER LTDA." + Environment.NewLine;
        str += (char)(27) + (char)(33) + (char)(0) + "Complejo Industrial" + Environment.NewLine;
        str += (char)(27) + (char)(33) + (char)(12) + "BASCULA" + Environment.NewLine + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(5) + "Avda. Central e/ Industrial" + Environment.NewLine + "Loma Plata, Chaco, Paraguay" + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(5) + "Tel. (0492) 418 600" + Environment.NewLine + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(92) + "Certificado de Pesaje" + Environment.NewLine + Environment.NewLine;

        // ------------------------------------------------------------------------------



        str += (char)(27) + (char)(97) + (char)(0); // alineacion izq

        // str += Chr(27) + Chr(33) + Chr(4) + "N° Pesada: " + Chr(27) + Chr(33) + Chr(12) + sID + vbCrLf + vbCrLf

        str += (char)(27) + (char)(33) + (char)(5) + "Fecha:  " + (char)(27) + (char)(33) + (char)(13) + sFecha + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(5) + "Chapa N°: " + (char)(27) + (char)(33) + (char)(13) + sChapa + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(5) + "Empresa/Productor: ";
        if (sEmp.Length > 31)
            str += (char)(27) + (char)(100) + (char)(1);
        str += (char)(27) + (char)(33) + (char)(13) + sEmp + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(5) + "Producto: ";
        if (sProd.Length > 23)
            str += (char)(27) + (char)(100) + (char)(1);
        str += (char)(27) + (char)(33) + (char)(13) + sProd + Environment.NewLine;


        str += (char)(27) + (char)(97) + (char)(1); // alineacion centrada

        str += (char)(27) + (char)(33) + (char)(5) + new string('-', 40);

        str += (char)(27) + (char)(97) + (char)(0); // alineacion izq

        str += (char)(27) + (char)(33) + (char)(24) + "PESO BRUTO:";
        str += (char)(27) + (char)(33) + (char)(56) + AlignRight(sBruto) + Environment.NewLine; // valores en negrita

        str += (char)(27) + (char)(33) + (char)(24) + "PESO TARA: ";
        str += (char)(27) + (char)(33) + (char)(56) + AlignRight(sTara) + Environment.NewLine; // valores en negrita

        str += (char)(27) + (char)(33) + (char)(24) + "PESO NETO: ";
        str += (char)(27) + (char)(33) + (char)(56) + AlignRight(sNeto) + Environment.NewLine; // valores en negrita



        str += (char)(27) + (char)(97) + (char)(1); // alineacion centrada

        str += (char)(27) + (char)(33) + (char)(5) + new string('-', 40);

        if (sBruto == "N/D" | sTara == "N/D" | sNeto == "N/D")
            str += (char)(27) + (char)(97) + (char)(1) + (char)(27) + (char)(33) + (char)(5) + "N/D: Peso no determinado." + Environment.NewLine;

        // str += Chr(27) + Chr(97) + Chr(0) 'alineacion izq

        str += (char)(27) + (char)(100) + (char)(1); // FEED 1
        str += (char)(27) + (char)(97) + (char)(0); // alineacion izq
        str += (char)(27) + (char)(33) + (char)(5) + "Obs.:";

        str += (char)(27) + (char)(100) + (char)(1); // FEED 1

        str += (char)(27) + (char)(97) + (char)(1); // alineacion centrada

        str += (char)(27) + (char)(33) + (char)(5) + new string('-', 40);

        str += (char)(27) + (char)(100) + (char)(6); // FEED 6

        str += (char)(27) + (char)(97) + (char)(1) + (char)(27) + (char)(33) + (char)(5) + "Pesado por:__________________" + Environment.NewLine;
        // str += Chr(27) + Chr(33) + Chr(5) + Now.ToString + vbCrLf

        str += (char)(27) + (char)(100) + (char)(6) + " "; // FEED 6

        printpos(str);
        }
    public void ImprimirTicketRecAlgodon(string sID, string sFecha, string sChapa, string sEmp, string sLote)
        {
        
        string str = "";

        // header-----------------------------------------------------------------------

        str += (char)(27) + (char)(116) + (char)(16); // Code PAge WPC1252

        str += (char)(27) + (char)(114) + (char)(0); // color negro
        str += (char)(27) + (char)(97) + (char)(1); // alineacion centrada
        str += (char)(27) + (char)(33) + (char)(5) + "COOPERATIVA CHORTITZER LTDA." + Environment.NewLine;
        str += (char)(27) + (char)(33) + (char)(0) + "Complejo Industrial" + Environment.NewLine;
        str += (char)(27) + (char)(33) + (char)(12) + "BASCULA" + Environment.NewLine + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(92) + "RECEPCIÓN DE ALGODÓN" + Environment.NewLine + Environment.NewLine;

        // ------------------------------------------------------------------------------



        str += (char)(27) + (char)(97) + (char)(0); // alineacion izq

        str += (char)(27) + (char)(33) + (char)(5) + "N° Pesada: " + (char)(27) + (char)(33) + (char)(13) + sID + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(5) + "Fecha:  " + (char)(27) + (char)(33) + (char)(13) + sFecha + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(5) + "Chapa N°: " + (char)(27) + (char)(33) + (char)(13) + sChapa + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(5) + "Empresa/Productor: " + Environment.NewLine;
        if (sEmp.Length > 31)
            str += (char)(27) + (char)(100) + (char)(1);
        str += (char)(27) + (char)(33) + (char)(56) + sEmp + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(5) + "Lote: ";
        str += (char)(27) + (char)(33) + (char)(56) + sLote + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(56) + "Humedad: ___________" + Environment.NewLine;

        str += (char)(27) + (char)(97) + (char)(1); // alineacion centrada

        str += (char)(27) + (char)(33) + (char)(5) + new string('-', 40);

        str += (char)(27) + (char)(97) + (char)(0) + Environment.NewLine + Environment.NewLine + Environment.NewLine; // alineacion izq

        str += (char)(27) + (char)(97) + (char)(1) + (char)(27) + (char)(33) + (char)(5) + "Pesado por:__________________" + Environment.NewLine;
        // str += Chr(27) + Chr(33) + Chr(5) + Now.ToString + vbCrLf

        str += (char)(27) + (char)(100) + (char)(6) + " "; // FEED 6

        printpos(str);
        }
    public void ImprimirTicketRecMuestra(string sID, string sFecha, string sChapa, string sEmp, string sProd)
        {

        string str = "";

        // header-----------------------------------------------------------------------

        str += (char)(27) + (char)(116) + (char)(16); // Code PAge WPC1252

        str += (char)(27) + (char)(114) + (char)(0); // color negro
        str += (char)(27) + (char)(97) + (char)(1); // alineacion centrada
        str += (char)(27) + (char)(33) + (char)(5) + "COOPERATIVA CHORTITZER LTDA." + Environment.NewLine;
        str += (char)(27) + (char)(33) + (char)(0) + "Complejo Industrial" + Environment.NewLine;
        str += (char)(27) + (char)(33) + (char)(12) + "BASCULA" + Environment.NewLine + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(92) + "MUESTRA DE MATERIA PRIMA" + Environment.NewLine + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(56) + "N°: " + sID + Environment.NewLine + Environment.NewLine;

        // ------------------------------------------------------------------------------



        str += (char)(27) + (char)(97) + (char)(0); // alineacion izq

        str += (char)(27) + (char)(33) + (char)(5) + "Fecha:  " + (char)(27) + (char)(33) + (char)(13) + sFecha + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(5) + "Chapa N°: " + (char)(27) + (char)(33) + (char)(13) + sChapa + Environment.NewLine;

        // str += Chr(27) + Chr(33) + Chr(5) + "Empresa/Productor: " + vbCrLf
        // If sEmp.Length > 31 Then
        // str += Chr(27) + Chr(100) + Chr(1)
        // End If
        // str += Chr(27) + Chr(33) + Chr(5) + sEmp + vbCrLf

        str += (char)(27) + (char)(33) + (char)(5) + "Producto: " + Environment.NewLine + Environment.NewLine;
        str += (char)(27) + (char)(33) + (char)(56) + sProd + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(56) + "Camión OK?" + Environment.NewLine + "SI_____  NO_____" + Environment.NewLine + Environment.NewLine;
        str += (char)(27) + (char)(33) + (char)(56) + "Carga OK?" + Environment.NewLine + "SI_____  NO_____" + Environment.NewLine + Environment.NewLine;

        str += (char)(27) + (char)(97) + (char)(1); // alineacion centrada

        str += (char)(27) + (char)(33) + (char)(5) + new string('-', 40);

        str += (char)(27) + (char)(97) + (char)(0) + Environment.NewLine + Environment.NewLine + Environment.NewLine; // alineacion izq

        str += (char)(27) + (char)(97) + (char)(1) + (char)(27) + (char)(33) + (char)(5) + "Firma Bascula:__________________" + Environment.NewLine;
        // str += Chr(27) + Chr(33) + Chr(5) + Now.ToString + vbCrLf

        str += (char)(27) + (char)(100) + (char)(6) + " "; // FEED 6

        printpos(str);
        }

    public void ImprimirTicketRecOrdenDescarga(string sID, string sFecha, string sChapa, string sEmp, string sProd, string sVerVehiculo, string sVerCarga)
        {

        string str = "";

        // header-----------------------------------------------------------------------

        str += (char)(27) + (char)(116) + (char)(16); // Code PAge WPC1252

        str += (char)(27) + (char)(114) + (char)(0); // color negro
        str += (char)(27) + (char)(97) + (char)(1); // alineacion centrada
        str += (char)(27) + (char)(33) + (char)(5) + "COOPERATIVA CHORTITZER LTDA." + Environment.NewLine;
        str += (char)(27) + (char)(33) + (char)(0) + "Complejo Industrial" + Environment.NewLine;
        str += (char)(27) + (char)(33) + (char)(12) + "BASCULA" + Environment.NewLine + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(92) + "ORDEN DE DESCARGA" + Environment.NewLine + Environment.NewLine;

        // ------------------------------------------------------------------------------

        str += (char)(27) + (char)(97) + (char)(0); // alineacion izq

        str += (char)(27) + (char)(56) + (char)(5) + "N°: " + (char)(27) + (char)(33) + (char)(13) + sID + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(5) + "Fecha:  " + (char)(27) + (char)(33) + (char)(13) + sFecha + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(5) + "Chapa N°: " + (char)(27) + (char)(33) + (char)(13) + sChapa + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(5) + "Establecimiento: " + Environment.NewLine;
        if (sEmp.Length > 31)
            str += (char)(27) + (char)(100) + (char)(1);
        str += (char)(27) + (char)(33) + (char)(56) + sEmp + Environment.NewLine;

        str += (char)(27) + (char)(33) + (char)(56) + sProd + Environment.NewLine;

        //str += (char)(27) + (char)(33) + (char)(56) + "Camión OK?   " + sVerVehiculo + Environment.NewLine;
        //str += (char)(27) + (char)(33) + (char)(56) + "Carga OK?    " + sVerCarga + Environment.NewLine;

        str += (char)(27) + (char)(97) + (char)(1); // alineacion centrada

        str += (char)(27) + (char)(33) + (char)(5) + new string('-', 40);

        str += (char)(27) + (char)(97) + (char)(0) + Environment.NewLine + Environment.NewLine + Environment.NewLine; // alineacion izq

        str += (char)(27) + (char)(97) + (char)(1) + (char)(27) + (char)(33) + (char)(5) + "Firma Bascula:__________________" + Environment.NewLine;
        // str += Chr(27) + Chr(33) + Chr(5) + Now.ToString + vbCrLf

        str += (char)(27) + (char)(100) + (char)(6) + " "; // FEED 6

        printpos(str);
        }

    public string AlignRight(string str)
        {
        return new string(' ', 16 - 6 - str.Length) + str;
        }


    public void printpos(string str)
        {
        RawPrinterHelper.SendStringToPrinter(TicketPrinterName, str);
        }
    public string setLRtwocolumn(string a, string b)
        {
            if ((40 - a.Length - b.Length) > 0)
                {
                string blank = new string(' ', 40 - a.Length - b.Length);
                return a + blank;
                }
            else
                //return Strings.Left(a, 39 - b.Length) + " ";
                return a.Substring(0, 39 - b.Length) + " ";
        }
    }


    //public class RawPrinterHelper
    //    {
    //    // Structure and API declarions:
    //    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    //    struct DOCINFOW
    //        {
    //        [MarshalAs(UnmanagedType.LPWStr)]
    //        public string pDocName;
    //        [MarshalAs(UnmanagedType.LPWStr)]
    //        public string pOutputFile;
    //        [MarshalAs(UnmanagedType.LPWStr)]
    //        public string pDataType;
    //        }

    //    [DllImport("winspool.Drv", EntryPoint = "OpenPrinterW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    //    public static bool OpenPrinter(string src, ref IntPtr hPrinter, Int32 pd)
    //        {
    //        }
    //    [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    //    public static bool ClosePrinter(IntPtr hPrinter)
    //        {
    //        }
    //    [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    //    public static bool StartDocPrinter(IntPtr hPrinter, Int32 level, ref DOCINFOW pDI)
    //        {
    //        }
    //    [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    //    public static bool EndDocPrinter(IntPtr hPrinter)
    //        {
    //        }
    //    [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    //    public static bool StartPagePrinter(IntPtr hPrinter)
    //        {
    //        }
    //    [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    //    public static bool EndPagePrinter(IntPtr hPrinter)
    //        {
    //        }
    //    [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    //    public static bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, ref Int32 dwWritten)
    //        {
    //        }

    //    // SendBytesToPrinter()
    //    // When the function is given a printer name and an unmanaged array of  
    //    // bytes, the function sends those bytes to the print queue.
    //    // Returns True on success or False on failure.
    //    public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
    //        {
    //        IntPtr hPrinter;      // The printer handle.
    //        Int32 dwError;        // Last error - in case there was trouble.
    //        DOCINFOW di = new DOCINFOW();          // Describes your document (name, port, data type).
    //        Int32 dwWritten;      // The number of bytes written by WritePrinter().
    //        bool bSuccess;     // Your success code.

    //        // Set up the DOCINFO structure.
    //            {
    //            var withBlock = di;
    //            withBlock.pDocName = "My Visual Basic .NET RAW Document";
    //            withBlock.pDataType = "RAW";
    //            }
    //        // Assume failure unless you specifically succeed.
    //        bSuccess = false;
    //        if (OpenPrinter(szPrinterName, ref hPrinter, 0))
    //            {
    //            if (StartDocPrinter(hPrinter, 1, ref di))
    //                {
    //                if (StartPagePrinter(hPrinter))
    //                    {
    //                    // Write your printer-specific bytes to the printer.
    //                    bSuccess = WritePrinter(hPrinter, pBytes, dwCount, ref dwWritten);
    //                    EndPagePrinter(hPrinter);
    //                    }
    //                EndDocPrinter(hPrinter);
    //                }
    //            ClosePrinter(hPrinter);
    //            }
    //        // If you did not succeed, GetLastError may give more information
    //        // about why not.
    //        if (bSuccess == false)
    //            dwError = Marshal.GetLastWin32Error();
    //        return bSuccess;
    //        } // SendBytesToPrinter()

    //    // SendFileToPrinter()
    //    // When the function is given a file name and a printer name, 
    //    // the function reads the contents of the file and sends the
    //    // contents to the printer.
    //    // Presumes that the file contains printer-ready data.
    //    // Shows how to use the SendBytesToPrinter function.
    //    // Returns True on success or False on failure.
    //    public static bool SendFileToPrinter(string szPrinterName, string szFileName)
    //        {
    //        // Open the file.
    //        FileStream fs = new FileStream(szFileName, FileMode.Open);
    //        // Create a BinaryReader on the file.
    //        BinaryReader br = new BinaryReader(fs);
    //        // Dim an array of bytes large enough to hold the file's contents.
    //        byte[] bytes = new byte[fs.Length + 1];
    //        bool bSuccess;
    //        // Your unmanaged pointer.
    //        IntPtr pUnmanagedBytes;

    //        // Read the contents of the file into the array.
    //        bytes = br.ReadBytes(fs.Length);
    //        // Allocate some unmanaged memory for those bytes.
    //        pUnmanagedBytes = Marshal.AllocCoTaskMem(fs.Length);
    //        // Copy the managed byte array into the unmanaged array.
    //        Marshal.Copy(bytes, 0, pUnmanagedBytes, fs.Length);
    //        // Send the unmanaged bytes to the printer.
    //        bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, fs.Length);
    //        // Free the unmanaged memory that you allocated earlier.
    //        Marshal.FreeCoTaskMem(pUnmanagedBytes);
    //        return bSuccess;
    //        } // SendFileToPrinter()

    //    // When the function is given a string and a printer name,
    //    // the function sends the string to the printer as raw bytes.
    //    public static void SendStringToPrinter(string szPrinterName, string szString)
    //        {
    //        IntPtr pBytes;
    //        Int32 dwCount;
    //        // How many characters are in the string?
    //        dwCount = szString.Length();
    //        // Assume that the printer is expecting ANSI text, and then convert
    //        // the string to ANSI text.

    //        pBytes = Marshal.StringToCoTaskMemAnsi(szString);
    //        // Send the converted ANSI string to the printer.
    //        SendBytesToPrinter(szPrinterName, pBytes, dwCount);
    //        Marshal.FreeCoTaskMem(pBytes);
    //        }

    
    }

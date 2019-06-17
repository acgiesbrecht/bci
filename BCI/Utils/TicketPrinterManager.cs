using BCI.Models;
using BCI.Utils;
using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

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

        public void imprimirTicket(XX_OPM_BCI_PESADAS_ALL pesada)
        {
           // Task.Run(() =>
           // {
                string str = "";

                // header-----------------------------------------------------------------------

                str += Chr(27) + Chr(116) + Chr(16); // Code PAge WPC1252

                str += Chr(27) + Chr(114) + Chr(0); // color negro
                str += Chr(27) + Chr(97) + Chr(1); // alineacion centrada
                str += Chr(27) + Chr(33) + Chr(5) + "COOPERATIVA CHORTITZER LTDA." + Environment.NewLine;
                str += Chr(27) + Chr(33) + Chr(0) + "Complejo Industrial" + Environment.NewLine;
                str += Chr(27) + Chr(33) + Chr(12) + "BASCULA" + Environment.NewLine + Environment.NewLine;

                str += Chr(27) + Chr(33) + Chr(5) + "Avda. Central e/ Industrial" + Environment.NewLine + "Loma Plata, Chaco, Paraguay" + Environment.NewLine;

                str += Chr(27) + Chr(33) + Chr(5) + "Tel. (0492) 418 600" + Environment.NewLine + Environment.NewLine;

                str += Chr(27) + Chr(33) + Chr(92) + "Certificado de Pesaje" + Environment.NewLine + Environment.NewLine;

                // ------------------------------------------------------------------------------

                str += Chr(27) + Chr(97) + Chr(0); // alineacion izq

                str += Chr(27) + Chr(33) + Chr(4) + "N° Pesada: " + Chr(27) + Chr(33) + Chr(12) + pesada.PESADA_ID.ToString() + Environment.NewLine + Environment.NewLine;

                str += Chr(27) + Chr(33) + Chr(5) + "Fecha:  " + Chr(27) + Chr(33) + Chr(13) + pesada.ExitDate + Environment.NewLine;

                str += Chr(27) + Chr(33) + Chr(5) + "Chapa N°: " + Chr(27) + Chr(33) + Chr(13) + pesada.MATRICULA + Environment.NewLine;

            if (pesada.Establecimiento != null) { 
                str += Chr(27) + Chr(33) + Chr(5) + "Establecimiento: ";
                if (pesada.Establecimiento.RazonSocial.Length > 31)
                    str += Chr(27) + Chr(100) + Chr(1);
                str += Chr(27) + Chr(33) + Chr(13) + pesada.Establecimiento.Significado + Environment.NewLine;
            }
            str += Chr(27) + Chr(33) + Chr(5) + "Articulo: ";
                if (pesada.InventoryItem.DESCRIPCION_ITEM.Length > 23)
                    str += Chr(27) + Chr(100) + Chr(1);
                str += Chr(27) + Chr(33) + Chr(13) + pesada.InventoryItem.DESCRIPCION_ITEM + Environment.NewLine;

                if (pesada.LOTE != null)
                {
                    str += Chr(27) + Chr(33) + Chr(5) + "Lote: ";
                    if (pesada.LOTE.Length == 11)
                    {
                        str += Chr(27) + Chr(33) + Chr(13) + pesada.LOTE.Substring(3, 3) + Environment.NewLine;
                    }
                    else
                    {
                        str += Chr(27) + Chr(33) + Chr(13) + pesada.LOTE + Environment.NewLine;
                    }
                }

                if (pesada.OBSERVACIONES != null) {
                    str += Chr(27) + Chr(33) + Chr(5) + "Obs.: " + Chr(27) + Chr(33) + Chr(13) + pesada.OBSERVACIONES + Environment.NewLine;
                }

                str += Chr(27) + Chr(97) + Chr(1); // alineacion centrada

                str += Chr(27) + Chr(33) + Chr(5) + new string('-', 40);

                str += Chr(27) + Chr(97) + Chr(0); // alineacion izq

                str += Chr(27) + Chr(33) + Chr(24) + "PESO BRUTO:";
                string bruto = pesada.PESO_BRUTO != null ? pesada.PESO_BRUTO.ToString() : "S/D";
                str += Chr(27) + Chr(33) + Chr(56) + AlignRight(pesada.PESO_BRUTO.ToString()) + Environment.NewLine; // valores en negrita

                str += Chr(27) + Chr(33) + Chr(24) + "PESO TARA: ";
                string tara = pesada.PESO_BRUTO != null ? pesada.PESO_BRUTO.ToString() : "S/D";
                str += Chr(27) + Chr(33) + Chr(56) + AlignRight(pesada.PESO_TARA.ToString()) + Environment.NewLine; // valores en negrita

                str += Chr(27) + Chr(33) + Chr(24) + "PESO NETO: ";
                string neto = (pesada.PESO_BRUTO != null && pesada.PESO_TARA != null) ? (pesada.PESO_BRUTO - pesada.PESO_TARA).ToString() : "S/D";
                str += Chr(27) + Chr(33) + Chr(56) + AlignRight(neto) + Environment.NewLine; // valores en negrita



                str += Chr(27) + Chr(97) + Chr(1); // alineacion centrada

                str += Chr(27) + Chr(33) + Chr(5) + new string('-', 40);

                if (bruto == "N/D" | tara == "N/D" | neto == "N/D")
                    str += Chr(27) + Chr(97) + Chr(1) + Chr(27) + Chr(33) + Chr(5) + "N/D: Peso no determinado." + Environment.NewLine;

                // str += Chr(27) + Chr(97) + Chr(0) 'alineacion izq

                str += Chr(27) + Chr(100) + Chr(1); // FEED 1
                str += Chr(27) + Chr(97) + Chr(0); // alineacion izq
                str += Chr(27) + Chr(33) + Chr(5) + "Obs.:";

                str += Chr(27) + Chr(100) + Chr(1); // FEED 1

                str += Chr(27) + Chr(97) + Chr(1); // alineacion centrada

                str += Chr(27) + Chr(33) + Chr(5) + new string('-', 40);

                str += Chr(27) + Chr(100) + Chr(6); // FEED 6

                str += Chr(27) + Chr(97) + Chr(1) + Chr(27) + Chr(33) + Chr(5) + "Pesado por:__________________" + Environment.NewLine;
                // str += Chr(27) + Chr(33) + Chr(5) + Now.ToString + vbCrLf

                str += Chr(27) + Chr(100) + Chr(6) + " "; // FEED 6

                printpos(str);
            //});
        }
        public void imprimirTicketRecAlgodon(XX_OPM_BCI_PESADAS_ALL pesada)
        {
            //Task.Run(() =>
            //{
                string str = "";

                // header-----------------------------------------------------------------------

                str += Chr(27) + Chr(116) + Chr(16); // Code PAge WPC1252

                str += Chr(27) + Chr(114) + Chr(0); // color negro
                str += Chr(27) + Chr(97) + Chr(1); // alineacion centrada
                str += Chr(27) + Chr(33) + Chr(5) + "COOPERATIVA CHORTITZER LTDA." + Environment.NewLine;
                str += Chr(27) + Chr(33) + Chr(0) + "Complejo Industrial" + Environment.NewLine;
                str += Chr(27) + Chr(33) + Chr(12) + "BASCULA" + Environment.NewLine + Environment.NewLine;

                str += Chr(27) + Chr(33) + Chr(92) + "RECEPCIÓN DE ALGODÓN" + Environment.NewLine + Environment.NewLine;

                // ------------------------------------------------------------------------------

                str += Chr(27) + Chr(97) + Chr(0); // alineacion izq

                str += Chr(27) + Chr(33) + Chr(5) + "N° Pesada: " + Chr(27) + Chr(33) + Chr(13) + pesada.PESADA_ID.ToString() + Environment.NewLine;

                str += Chr(27) + Chr(33) + Chr(5) + "Fecha:  " + Chr(27) + Chr(33) + Chr(13) + pesada.EntryDate + Environment.NewLine;

                str += Chr(27) + Chr(33) + Chr(5) + "Chapa N°: " + Chr(27) + Chr(33) + Chr(13) + pesada.MATRICULA + Environment.NewLine;

                str += Chr(27) + Chr(33) + Chr(5) + "Empresa/Productor: " + Environment.NewLine;
                if (pesada.Establecimiento.RazonSocial.Length > 31)
                    str += Chr(27) + Chr(100) + Chr(1);
                str += Chr(27) + Chr(33) + Chr(56) + pesada.Establecimiento.RazonSocial + Environment.NewLine;

                str += Chr(27) + Chr(33) + Chr(5) + "Lote: ";
                str += Chr(27) + Chr(33) + Chr(56) + pesada.LOTE + Environment.NewLine;

                str += Chr(27) + Chr(33) + Chr(56) + "Humedad: ___________" + Environment.NewLine;

                str += Chr(27) + Chr(97) + Chr(1); // alineacion centrada

                str += Chr(27) + Chr(33) + Chr(5) + new string('-', 40);

                str += Chr(27) + Chr(97) + Chr(0) + Environment.NewLine + Environment.NewLine + Environment.NewLine; // alineacion izq

                str += Chr(27) + Chr(97) + Chr(1) + Chr(27) + Chr(33) + Chr(5) + "Pesado por:__________________" + Environment.NewLine;
                // str += Chr(27) + Chr(33) + Chr(5) + Now.ToString + vbCrLf

                str += Chr(27) + Chr(100) + Chr(6) + " "; // FEED 6

                printpos(str);
            //});
        }
        public void imprimirTicketRecMuestra(XX_OPM_BCI_PESADAS_ALL pesada)
        {
            //Task.Run(() =>
            //{
                StringBuilder st = new StringBuilder();
                string str = "";
            
                // header-----------------------------------------------------------------------

                st.Append(Chr(27) + Chr(116) + Chr(16)); // Code PAge WPC1252

                st.Append(Chr(27) + Chr(114) + Chr(0)); // color negro
                st.Append(Chr(27) + Chr(97) + Chr(1)); // alineacion centrada
                st.Append(Chr(27) + Chr(33) + Chr(5) + "COOPERATIVA CHORTITZER LTDA." + Environment.NewLine);
                st.Append(Chr(27) + Chr(33) + Chr(0) + "Complejo Industrial" + Environment.NewLine);
                st.Append(Chr(27) + Chr(33) + Chr(12) + "BASCULA" + Environment.NewLine + Environment.NewLine);

                st.Append(Chr(27) + Chr(33) + Chr(92) + "MUESTRA DE MATERIA PRIMA" + Environment.NewLine + Environment.NewLine);

                st.Append(Chr(27) + Chr(33) + Chr(56) + "N°: " + pesada.PESADA_ID.ToString() + Environment.NewLine + Environment.NewLine);

                // ------------------------------------------------------------------------------



                st.Append(Chr(27) + Chr(97) + Chr(0)); // alineacion izq

                st.Append(Chr(27) + Chr(33) + Chr(5) + "Fecha:  " + Chr(27) + Chr(33) + Chr(13) + pesada.EntryDate + Environment.NewLine);

                st.Append(Chr(27) + Chr(33) + Chr(5) + "Chapa N°: " + Chr(27) + Chr(33) + Chr(13) + pesada.MATRICULA + Environment.NewLine);

                st.Append(Chr(27) + Chr(33) + Chr(5) + "Tipo de Actividad°: " + Chr(27) + Chr(33) + Chr(13) + pesada.TipoActividad.Description + Environment.NewLine);

                st.Append(Chr(27) + Chr(33) + Chr(5) + "RUC: " + Chr(27) + Chr(33) + Chr(13) + pesada.Establecimiento.RUC + Environment.NewLine);

            // str += Chr(27) + Chr(33) + Chr(5) + "Empresa/Productor: " + vbCrLf
            // If sEmp.Length > 31 Then
            // str += Chr(27) + Chr(100) + Chr(1)
            // End If
            // str += Chr(27) + Chr(33) + Chr(5) + sEmp + vbCrLf

            //st.Append(Chr(27) + Chr(33) + Chr(5) + "Producto: " + Environment.NewLine + Environment.NewLine);
            st.Append(Chr(27) + Chr(33) + Chr(5) + Environment.NewLine + Environment.NewLine);
                st.Append(Chr(27) + Chr(33) + Chr(56) + pesada.InventoryItem.DESCRIPCION_ITEM + Environment.NewLine);

                if (pesada.LOTE != null)
                {
                    st.Append(Chr(27) + Chr(33) + Chr(56) + "Lote: ");
                    if (pesada.LOTE.Length == 11)
                    {
                        st.Append(Chr(27) + Chr(33) + Chr(56) + pesada.LOTE.Substring(3, 3) + Environment.NewLine);
                    }
                    else
                    {
                        st.Append(Chr(27) + Chr(33) + Chr(56) + pesada.LOTE + Environment.NewLine);
                    }
                }

                //st.Append(Chr(27) + Chr(33) + Chr(56) + "Camión OK?" + Environment.NewLine + "SI_____  NO_____" + Environment.NewLine + Environment.NewLine);
                //st.Append(Chr(27) + Chr(33) + Chr(56) + "Carga OK?" + Environment.NewLine + "SI_____  NO_____" + Environment.NewLine + Environment.NewLine);

                st.Append(Chr(27) + Chr(97) + Chr(1)); // alineacion centrada

                st.Append(Chr(27) + Chr(33) + Chr(5) + new string('-', 40));

                st.Append(Chr(27) + Chr(97) + Chr(0) + Environment.NewLine + Environment.NewLine + Environment.NewLine); // alineacion izq

                st.Append(Chr(27) + Chr(97) + Chr(1) + Chr(27) + Chr(33) + Chr(5) + "Firma Bascula:__________________" + Environment.NewLine);
                // str += Chr(27) + Chr(33) + Chr(5) + Now.ToString + vbCrLf

                st.Append(Chr(27) + Chr(100) + Chr(6) + " "); // FEED 6

                printpos(st.ToString());
            //});

        }

        private string Chr(int c)
        {
            return Char.ConvertFromUtf32(c);
        }

        /*public void ImprimirTicketRecOrdenDescarga(string sID, string sFecha, string sChapa, string sEmp, string sProd, string sVerVehiculo, string sVerCarga)
            {

            string str = "";

            // header-----------------------------------------------------------------------

            str += Chr(27) + Chr(116) + Chr(16); // Code PAge WPC1252

            str += Chr(27) + Chr(114) + Chr(0); // color negro
            str += Chr(27) + Chr(97) + Chr(1); // alineacion centrada
            str += Chr(27) + Chr(33) + Chr(5) + "COOPERATIVA CHORTITZER LTDA." + Environment.NewLine;
            str += Chr(27) + Chr(33) + Chr(0) + "Complejo Industrial" + Environment.NewLine;
            str += Chr(27) + Chr(33) + Chr(12) + "BASCULA" + Environment.NewLine + Environment.NewLine;

            str += Chr(27) + Chr(33) + Chr(92) + "ORDEN DE DESCARGA" + Environment.NewLine + Environment.NewLine;

            // ------------------------------------------------------------------------------

            str += Chr(27) + Chr(97) + Chr(0); // alineacion izq

            str += Chr(27) + Chr(56) + Chr(5) + "N°: " + Chr(27) + Chr(33) + Chr(13) + sID + Environment.NewLine;

            str += Chr(27) + Chr(33) + Chr(5) + "Fecha:  " + Chr(27) + Chr(33) + Chr(13) + sFecha + Environment.NewLine;

            str += Chr(27) + Chr(33) + Chr(5) + "Chapa N°: " + Chr(27) + Chr(33) + Chr(13) + sChapa + Environment.NewLine;

            str += Chr(27) + Chr(33) + Chr(5) + "Establecimiento: " + Environment.NewLine;
            if (sEmp.Length > 31)
                str += Chr(27) + Chr(100) + Chr(1);
            str += Chr(27) + Chr(33) + Chr(56) + sEmp + Environment.NewLine;

            str += Chr(27) + Chr(33) + Chr(56) + sProd + Environment.NewLine;

            //str += Chr(27) + Chr(33) + Chr(56) + "Camión OK?   " + sVerVehiculo + Environment.NewLine;
            //str += Chr(27) + Chr(33) + Chr(56) + "Carga OK?    " + sVerCarga + Environment.NewLine;

            str += Chr(27) + Chr(97) + Chr(1); // alineacion centrada

            str += Chr(27) + Chr(33) + Chr(5) + new string('-', 40);

            str += Chr(27) + Chr(97) + Chr(0) + Environment.NewLine + Environment.NewLine + Environment.NewLine; // alineacion izq

            str += Chr(27) + Chr(97) + Chr(1) + Chr(27) + Chr(33) + Chr(5) + "Firma Bascula:__________________" + Environment.NewLine;
            // str += Chr(27) + Chr(33) + Chr(5) + Now.ToString + vbCrLf

            str += Chr(27) + Chr(100) + Chr(6) + " "; // FEED 6

            printpos(str);
            }
*/
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

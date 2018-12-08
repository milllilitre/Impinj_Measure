using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Impinj.OctaneSdk;
namespace rfid
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        ///   // Create an instance of the ImpinjReader class.
       public static ImpinjReader reader = new ImpinjReader();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
             Form1 form1=new Form1();
            Application.Run(form1);
           
        
        }
    }
}

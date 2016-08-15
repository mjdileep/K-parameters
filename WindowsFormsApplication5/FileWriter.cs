using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    class FileWriter
    {

        public void Csvwriter(string deviceId,string calibration,string A1st,string A2nd,string Kpara,string B1st,string B2nd,string KparaB,string C1st,string C2nd,string KparaC)
        {
            string filePathForTests = null;
            String filepath2;
            if (Form1.CallibrationDataFolderPath == "None")
            {
                string workingDir = Directory.GetCurrentDirectory().Replace("WindowsFormsApplication5\\bin\\Release", "").Replace("WindowsFormsApplication5\\bin\\Debug", "");
                filepath2 = workingDir + "Kparameters\\";
            }
            else filepath2 = Form1.CallibrationDataFolderPath + "\\Kparameters\\";
            switch (calibration)
            {
                case "Current": filePathForTests = filepath2+"current\\" + deviceId + ".csv"; break;
                case "Voltage": filePathForTests = filepath2+"voltage\\" + deviceId + ".csv"; break;
                case "ActiveEnergy": filePathForTests = filepath2+"ActiveEnergy\\" + deviceId + ".csv"; break;
                case "ReactiveEnergy": filePathForTests = filepath2+"ReactiveEnergy\\" + deviceId + ".csv"; break;
                case "ApperentEnergy": filePathForTests = filepath2+"ApperentEnergy\\" + deviceId + ".csv"; break;
                case "ActivePower": filePathForTests = filepath2+"ActivePower\\" + deviceId + ".csv"; break;
                case "ReactivePower": filePathForTests = filepath2+"ReactivePower\\" + deviceId + ".csv"; break;
                case "ApperentPower": filePathForTests = filepath2+"ApperentPower\\" + deviceId + ".csv"; break;
            }
            try
            {
                String delimiter = ",";

                String filePath = filePathForTests;
                if (!File.Exists(filePath))
                {
                   File.Create(filePath).Close();
                   String[][] output2 = new String[][]{
                   new String[]{"\n","PhaseA 1st","PhaseA 2nd","PhaseA k"," ","PhaseB 1st","PhaseB 2nd","PhaseB k"," ","PhaseC 1st","PhaseC 2nd","PhaseC k\n"} /*add the values that you want inside a csv file. Mostly this function can be used in a foreach loop.*/
                };
                   int length2 = output2.GetLength(0);
                   StringBuilder sb2 = new StringBuilder();
                   for (int index = 0; index < length2; index++)
                       sb2.AppendLine(String.Join(delimiter, output2[index]));
                   File.AppendAllText(filePath, sb2.ToString());
                }
                

           
                String[][] output = new String[][]{
            new String[]{" ",A1st,A2nd,Kpara," ",B1st,B2nd,KparaB," ",C1st,C2nd,KparaC} /*add the values that you want inside a csv file. Mostly this function can be used in a foreach loop.*/
            };
                int length = output.GetLength(0);
                StringBuilder sb = new StringBuilder();
                for (int index = 0; index < length; index++)
                    sb.AppendLine(String.Join(delimiter, output[index]));
                File.AppendAllText(filePath, sb.ToString());
                // File.
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
    }
}

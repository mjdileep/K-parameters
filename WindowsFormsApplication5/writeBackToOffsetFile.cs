using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    class writeBackToOffsetFile
    {



    public void writeBack(string deviceId,string calibration,string Rs1st,string Rs2nd,string Rs3rd,string slopeA,string slopeB,string slopeC)
        {
            string filePathForTests = null;
            String filepath2;
            if (Form1.CallibrationDataFolderPath == "None")
            {
                string workingDir = Directory.GetCurrentDirectory().Replace("WindowsFormsApplication5\\bin\\Release", "").Replace("WindowsFormsApplication5\\bin\\Debug", "");
                filepath2 = workingDir + "Kparameters\\";
            }
            else filepath2 = Form1.CallibrationDataFolderPath+ "\\Kparameters\\";

            switch (calibration)
            {
                case "Current": filePathForTests =filepath2+"current\\" + deviceId + ".csv"; break;
                case "Voltage": filePathForTests = filepath2 + "current\\" + deviceId + ".csv"; break;
                case "ActiveEnergy": filePathForTests = filepath2 + "current\\" + deviceId + ".csv"; break;
                case "ReactiveEnergy": filePathForTests = filepath2 + "current\\" + deviceId + ".csv"; break;
                case "ApperentEnergy": filePathForTests = filepath2 + "current\\" + deviceId + ".csv"; break;
                case "ActivePower": filePathForTests = filepath2 + "current\\" + deviceId + ".csv"; break;
                case "ReactivePower": filePathForTests = filepath2 + "current\\" + deviceId + ".csv"; break;
                case "ApperentPower": filePathForTests = filepath2 + "current\\" + deviceId + ".csv"; break;
            }
                try
               {
                String delimiter = ",";

                String filePath = filePathForTests;
                if (!File.Exists(filePath))
                {
                   File.Create(filePath).Close();
                };

        
                String[][] output = new String[][]{
            new String[]{" ","rsquard",Rs1st," "," "," ",Rs2nd," "," "," ",Rs3rd," "," ","\n","Gradient",slopeA," "," "," ",slopeB," "," "," ",slopeC," "," "," ",} /*add the values that you want inside a csv file. Mostly this function can be used in a foreach loop.*/
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



using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication5
{
    class RewriteOffsetsToDevice
    {

        public void rewrites(List<WindowsFormsApplication5.Form1.EnableListDeviceInformation> devicesData)
        {

            List<WindowsFormsApplication5.Form1.EnableListDeviceInformation> deviceDatalist = devicesData;

            for (int i = 0; i < deviceDatalist.Count; i++)
            {
                try
                {

                    //do whatever need to do to these ports one by one
                    SerialPort port = new SerialPort(deviceDatalist[i].Port);
                

                    // Set the properties.
                    port.BaudRate = 115200;
                    port.Parity = Parity.None;
                    port.ReadTimeout = 10;
                    port.StopBits = StopBits.One;
                     
                    //upload the firmares oneby one
                    port.PortName = deviceDatalist[i].Port;

                    port.Open();
                    /////////
                    string filepath;
                    SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
                    if (Form1.CallibrationDataFolderPath == "None")
                    {
                        string workingDir = Directory.GetCurrentDirectory().Replace("WindowsFormsApplication5\\bin\\Release", "").Replace("WindowsFormsApplication5\\bin\\Debug", "");
                        filepath = workingDir + "OffsetValues\\" + devicesData[i].DeviceId + ".csv";
                    }
                    else filepath = Form1.CallibrationDataFolderPath + "\\" + devicesData[i].DeviceId + ".csv";
                   // filepath = @"F:\calibrationFiles\OffsetValues\" + devicesData[i].DeviceId + ".csv";
                    var workbook=new GemBox.Spreadsheet.ExcelFile();
                    GemBox.Spreadsheet.ExcelWorksheet worksheet =null;
                    try
                    {
                        workbook = ExcelFile.Load(filepath);

                        // Select active worksheet.
                        worksheet = workbook.Worksheets.ActiveWorksheet;
                    }
                    catch (Exception ex) { this.excecutionErrorSummery(deviceDatalist[i].DeviceId, "some"); continue; }
                    StringBuilder ep1 = new StringBuilder();
                    StringBuilder ep2 = new StringBuilder();
                    StringBuilder ep3 = new StringBuilder();
                    StringBuilder ep4 = new StringBuilder();
                    StringBuilder ep5 = new StringBuilder();
                    try
                    {
                        
                        ep1.Append("OFFSET_C,");
                        ep2.Append("OFFSET_V,");
                        ep2.Append("OFFSET_Ph,");
                        ep2.Append("OFFSET_AP,");
                        ep2.Append("OFFSET_RP,");

                        ep1.Append(Math.Round(Convert.ToDouble(worksheet.Cells["C9"].GetFormattedValue())) + ",");
                        ep1.Append(Math.Round(Convert.ToDouble(worksheet.Cells["D9"].GetFormattedValue())) + ",");
                        ep1.Append(Math.Round(Convert.ToDouble(worksheet.Cells["E9"].GetFormattedValue())) + ",");
                        ep2.Append(Math.Round(Convert.ToDouble(worksheet.Cells["C17"].GetFormattedValue())) + ",");
                        ep2.Append(Math.Round(Convert.ToDouble(worksheet.Cells["D17"].GetFormattedValue())) + ",");
                        ep2.Append(Math.Round(Convert.ToDouble(worksheet.Cells["E17"].GetFormattedValue())) + ",");
                        ep3.Append(Math.Round(Convert.ToDouble(worksheet.Cells["C26"].GetFormattedValue())) + ",");
                        ep3.Append(Math.Round(Convert.ToDouble(worksheet.Cells["D26"].GetFormattedValue())) + ",");
                        ep3.Append(Math.Round(Convert.ToDouble(worksheet.Cells["E26"].GetFormattedValue())) + ",");
                        ep4.Append(Math.Round(Convert.ToDouble(worksheet.Cells["C37"].GetFormattedValue())) + ",");
                        ep4.Append(Math.Round(Convert.ToDouble(worksheet.Cells["D37"].GetFormattedValue())) + ",");
                        ep4.Append(Math.Round(Convert.ToDouble(worksheet.Cells["E37"].GetFormattedValue())) + ",");
                        ep5.Append(Math.Round(Convert.ToDouble(worksheet.Cells["C48"].GetFormattedValue())) + ",");
                        ep5.Append(Math.Round(Convert.ToDouble(worksheet.Cells["D48"].GetFormattedValue())) + ",");
                        ep5.Append(Math.Round(Convert.ToDouble(worksheet.Cells["E48"].GetFormattedValue())) + ",");
                    }
                    catch (Exception ex) { this.excecutionErrorSummery(deviceDatalist[i].DeviceId); continue; }
                    port.Write(ep1.ToString()); port.Write(ep2.ToString());
                    port.Write(ep3.ToString()); port.Write(ep4.ToString());
                    port.Write(ep5.ToString());
                    /////////
                    port.Close();

                }

                catch { this.excecutionErrorSummery(deviceDatalist[i].Port, 0); continue; }

            }
        }
        public void excecutionErrorSummery(string id, string field)
        {
            String filePath;
            if (Form1.CallibrationDataFolderPath == "None")
            {
                string workingDir = Directory.GetCurrentDirectory().Replace("WindowsFormsApplication5\\bin\\Release", "").Replace("WindowsFormsApplication5\\bin\\Debug", "");
                filePath = workingDir + "Kparameters\\excecutionErrorSummery.txt";
            }
            else filePath = Form1.CallibrationDataFolderPath + "\\Kparameters\\excecutionErrorSummery.txt";
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine("Couldn't load the K-parameter file for device ID: " + id);
            }
        }
        public void excecutionErrorSummery(string id)
        {
            String filePath;
            if (Form1.CallibrationDataFolderPath == "None")
            {
                string workingDir = Directory.GetCurrentDirectory().Replace("WindowsFormsApplication5\\bin\\Release", "").Replace("WindowsFormsApplication5\\bin\\Debug", "");
                filePath = workingDir + "Kparameters\\excecutionErrorSummery.txt";
            }
            else filePath = Form1.CallibrationDataFolderPath + "\\Kparameters\\excecutionErrorSummery.txt";
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine("There are problems with the data in  " + id+".csv file");
            }
        }
        public void excecutionErrorSummery(string id,int i)
        {
            String filePath;
            if (Form1.CallibrationDataFolderPath == "None")
            {
                string workingDir = Directory.GetCurrentDirectory().Replace("WindowsFormsApplication5\\bin\\Release", "").Replace("WindowsFormsApplication5\\bin\\Debug", "");
                filePath = workingDir + "Kparameters\\excecutionErrorSummery.txt";
            }
            else filePath = Form1.CallibrationDataFolderPath + "\\Kparameters\\excecutionErrorSummery.txt";
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine("Couldn't open the port for device " +id);
            }
        }
    }
}

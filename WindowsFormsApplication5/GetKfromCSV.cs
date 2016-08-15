using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.IO;

namespace WindowsFormsApplication5
{
    class GetKfromCSV
    {
        //All have to do is average the values from csv file

        public void getAllKParas(List<Form1.EnableListDeviceInformation> devicesData)
        {
            for (int k = 0; k < devicesData.Count; k++)
            {
                try
                {//get data from exel sheet and write to the device 

                   
                    SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

                    String filepath;
                    if (Form1.CallibrationDataFolderPath == "None")
                    {
                        string workingDir = Directory.GetCurrentDirectory().Replace("WindowsFormsApplication5\\bin\\Release", "").Replace("WindowsFormsApplication5\\bin\\Debug", "");
                        filepath = workingDir + "OffsetValues\\" + devicesData[k].DeviceId + ".csv";
                    }
                    else filepath = Form1.CallibrationDataFolderPath + "\\" + devicesData[k].DeviceId + ".csv";

                    var workbook = new GemBox.Spreadsheet.ExcelFile();
                    GemBox.Spreadsheet.ExcelWorksheet worksheet = null;
                    try{
                        workbook = ExcelFile.Load(filepath);
                        worksheet = workbook.Worksheets.ActiveWorksheet;
                    }
                    catch (Exception ex) { this.excecutionErrorSummery(devicesData[k].DeviceId, "some"); continue; }
                    StringBuilder ep = new StringBuilder();
                    ep.Append("OFFSET_EEPROM,");
                    try
                    {
                        Console.WriteLine("current Ks");
                        ep.Append(Math.Round(Convert.ToDouble(worksheet.Cells["C9"].GetFormattedValue())) + ",");
                        ep.Append(Math.Round(Convert.ToDouble(worksheet.Cells["D9"].GetFormattedValue())) + ",");
                        ep.Append(Math.Round(Convert.ToDouble(worksheet.Cells["E9"].GetFormattedValue())) + ",");
                        ep.Append(Math.Round(Convert.ToDouble(worksheet.Cells["C17"].GetFormattedValue())) + ",");
                        ep.Append(Math.Round(Convert.ToDouble(worksheet.Cells["D17"].GetFormattedValue())) + ",");
                        ep.Append(Math.Round(Convert.ToDouble(worksheet.Cells["E17"].GetFormattedValue())) + ",");
                        ep.Append(Math.Round(Convert.ToDouble(worksheet.Cells["C26"].GetFormattedValue())) + ",");
                        ep.Append(Math.Round(Convert.ToDouble(worksheet.Cells["D26"].GetFormattedValue())) + ",");
                        ep.Append(Math.Round(Convert.ToDouble(worksheet.Cells["E26"].GetFormattedValue())) + ",");
                        ep.Append(Math.Round(Convert.ToDouble(worksheet.Cells["C37"].GetFormattedValue())) + ",");
                        ep.Append(Math.Round(Convert.ToDouble(worksheet.Cells["D37"].GetFormattedValue())) + ",");
                        ep.Append(Math.Round(Convert.ToDouble(worksheet.Cells["E37"].GetFormattedValue())) + ",");
                        ep.Append(Math.Round(Convert.ToDouble(worksheet.Cells["C48"].GetFormattedValue())) + ",");
                        ep.Append(Math.Round(Convert.ToDouble(worksheet.Cells["D48"].GetFormattedValue())) + ",");
                        ep.Append(Math.Round(Convert.ToDouble(worksheet.Cells["E48"].GetFormattedValue())) + ",");

                    }
                    catch (Exception ex) { this.excecutionErrorSummery(devicesData[k].DeviceId); continue; }
              //     WriteToDb(ep.ToString(), devicesData[k].DeviceId, devicesData[k].Ct);
                    
                    //write to the port now
                  //  MessageBox.Show(ep.ToString());
                    
                    SerialPort port = new SerialPort();
                    port.PortName = devicesData[k].Port;
                      // Set the properties.
                    port.BaudRate = 115200;
                    port.Parity = Parity.None;
                    port.StopBits = StopBits.One;
                    // port.ReadTimeout = 10000;
                    try
                    {
                        port.Open();
                        port.Write(ep.ToString());
                    }
                    catch (Exception ex) { this.excecutionErrorSummery(devicesData[k].Port, 0); continue; }

                    Thread.Sleep(1000);

                    StringBuilder ep2 = new StringBuilder();
                    ep2.Append("KPARA_EEPROM,");
                    
	                List<string> iKparas = new List<string>();
                    List<string> iRsqrd = new List<string>();
                    ///for one device
                    for (int j = 0; j <= 7; j++)
                    {   
                        string filepathParameter = null;
                        switch (j)
                        {
                            case 0: filepathParameter = "ReactivePower\\"; break;
                            case 1: filepathParameter = "ActivePower\\"; break;
                            case 2: filepathParameter = "ApperentPower\\"; break;
                            case 3: filepathParameter = "ReactiveEnergy\\"; break;
                            case 4: filepathParameter = "ActiveEnergy\\"; break;
                            case 5: filepathParameter = "ApperentEnergy\\"; break;
                            case 6: filepathParameter = "Current\\"; break;
                            case 7: filepathParameter = "Voltage\\"; break;
                           
                        }
                        String filepath2;
                        if (Form1.CallibrationDataFolderPath == "None")
                        {
                            string workingDir = Directory.GetCurrentDirectory().Replace("WindowsFormsApplication5\\bin\\Release", "").Replace("WindowsFormsApplication5\\bin\\Debug", "");
                            filepath2 = workingDir + "Kparameters\\" + devicesData[k].DeviceId + ".csv";
                        }
                        else filepath2 = Form1.CallibrationDataFolderPath + "\\Kparameters\\" + devicesData[k].DeviceId + ".csv";

                        var workbook2 = new GemBox.Spreadsheet.ExcelFile();
                        GemBox.Spreadsheet.ExcelWorksheet worksheet2 = null;
                        workbook2 = ExcelFile.Load(filepath2);
                        worksheet2 = workbook2.Worksheets.ActiveWorksheet;
                      
                        ArrayList phaseA = new ArrayList();
                        ArrayList phaseB = new ArrayList();
                        ArrayList phaseC = new ArrayList();
                        int startnormal = 4; int endnormal = 28;
                        if (filepathParameter == "ReactiveEnergy\\" || filepathParameter == "ActiveEnergy\\" || filepathParameter == "ApperentEnergy\\") { endnormal = 4; }

                        for (int y = startnormal; y <= endnormal; y++)
                        {
                            try
                            {
                                string columnNameA1st = "B" + y;
                                double tempA1st = ((Convert.ToDouble(worksheet2.Cells[columnNameA1st].GetFormattedValue())));
                                string columnNameA2nd = "C" + y;
                                double tempA2nd = ((Convert.ToDouble(worksheet2.Cells[columnNameA2nd].GetFormattedValue())));
                                phaseA.Add(new ReportPoint(tempA2nd,tempA1st));

                                string columnNameB1st = "F" + y;
                                double tempB1st = ((Convert.ToDouble(worksheet2.Cells[columnNameB1st].GetFormattedValue())));
                                string columnNameB2nd = "G" + y;
                                double tempB2nd = ((Convert.ToDouble(worksheet2.Cells[columnNameB2nd].GetFormattedValue())));
                                phaseB.Add(new ReportPoint(tempB2nd, tempB1st));

                                string columnNameC1st = "J" + y;
                                double tempC1st = ((Convert.ToDouble(worksheet2.Cells[columnNameC1st].GetFormattedValue())));
                                string columnNameC2nd = "K" + y;
                                double tempC2nd = ((Convert.ToDouble(worksheet2.Cells[columnNameC2nd].GetFormattedValue())));
                                phaseC.Add(new ReportPoint(tempC2nd, tempC1st));
                            }
                            catch { continue; }
                        }

                        ReportPoint report = new ReportPoint();
                        string phAreturn =report.calcValues(phaseA);
                        string phBreturn = report.calcValues(phaseB);
                        string phCreturn = report.calcValues(phaseC);
                       //lists are created,send to the function to get the slope and rsqd
                        string SlopeA = (phAreturn.Split(','))[0];
                        string R2A = (phAreturn.Split(','))[1];
                        string SlopeB = (phBreturn.Split(','))[0];
                        string R2B = (phBreturn.Split(','))[1];
                        string SlopeC = (phCreturn.Split(','))[0];
                        string R2C = (phAreturn.Split(','))[1];


                        string Rsqr = R2A.ToString() + "," + R2B.ToString() + "," + R2C.ToString();
                        string writeKpara = SlopeA.ToString() + "," + SlopeB.ToString() + "," + SlopeC.ToString() + ",";
                        iKparas.Add(writeKpara);
                        iRsqrd.Add(Rsqr); 
                        
                        //write to kparameter file
                        string[] pathspliter = filepathParameter.Split('\\');

                        writeBackToOffsetFile writ = new writeBackToOffsetFile();
                        writ.writeBack(devicesData[k].DeviceId, pathspliter[0], R2A, R2B, R2C,SlopeA,SlopeB,SlopeC);
                    }

                    ep2.Append(iKparas[0] + iKparas[1] + iKparas[2] + iKparas[3] + iKparas[4] + iKparas[5] + iKparas[6]+ iKparas[7]);

                  //wRITE k TO DB

                  //  writeKtoDb(ep2.ToString(), devicesData[k].DeviceId.ToString());  
  
                /////////////////////
                    string[] TEMP = (ep2.ToString()).Split(',');
                    int count = TEMP.Count();
                 
                    Thread.Sleep(3000);
                    try
                    {

                        port.Write(ep2.ToString());

                        Thread.Sleep(3000);

                        string RET = port.ReadExisting();
                        //write the id to the device
                        int deviceID = Int32.Parse(devicesData[k].DeviceId);
                        Thread.Sleep(500);
                        port.Write("DEVICEID_EEPROM," + deviceID + ",");
                        port.Close();
                    }
                    catch (Exception ex) { this.excecutionErrorSummery(devicesData[k].Port, 0); }

                //end device
                }
                catch (Exception e) { Console.WriteLine(e); continue; }
            }
        
        }

        public void WriteToDb(string OFFsetSet,string deviceId,string CT) {

            try
            {
                string[] array = OFFsetSet.Split(',');

                int DeviceId = Int32.Parse(deviceId);
                int CTdb = Int32.Parse(CT);
                decimal AIRMSOS = decimal.Parse(array[1]);
                decimal BIRMSOS = decimal.Parse(array[2]);
                decimal CIRMSOS = decimal.Parse(array[3]);
                decimal AVRMSOS = decimal.Parse(array[4]);
                decimal BVRMSOS = decimal.Parse(array[5]);
                decimal CVRMSOS = decimal.Parse(array[6]);
                decimal APHCAL = decimal.Parse(array[7]);
                decimal BPHCAL = decimal.Parse(array[8]);
                decimal CPHCAL = decimal.Parse(array[9]);
                decimal AWATTOS = decimal.Parse(array[10]);
                decimal BWATTOS = decimal.Parse(array[11]);
                decimal CWATTOS = decimal.Parse(array[12]);
                decimal AVAROS = decimal.Parse(array[13]);
                decimal BVAROS = decimal.Parse(array[14]);
                decimal CVAROS = decimal.Parse(array[15]);

                SqlConnection myConnection = new SqlConnection("Data Source=ASUS;Initial Catalog=CalibrationData;Integrated Security=True");
                myConnection.Open();
                SqlCommand cmd = myConnection.CreateCommand();
                cmd.CommandText = "Execute dbo.Test2 @DeviceID,@AIRMSOS,@BIRMSOS,@CIRMSOS,@AVRMSOS,@BVRMSOS,@CVRMSOS,@APHCAL,@BPHCAL,@CPHCAL,@AWATTOS,@BWATTOS,@CWATTOS,@AVAROS,@BVAROS,@CVAROS,@CT";

                cmd.Parameters.Add("@DeviceID", SqlDbType.Decimal, 8).Value = DeviceId;
                cmd.Parameters.Add("@CT", SqlDbType.Decimal, 8).Value = CTdb;
                cmd.Parameters.Add("@AIRMSOS", SqlDbType.Decimal, 8).Value = AIRMSOS;
                cmd.Parameters.Add("@BIRMSOS", SqlDbType.Decimal, 50).Value = BIRMSOS;
                cmd.Parameters.Add("@CIRMSOS", SqlDbType.Decimal, 50).Value = CIRMSOS;
                cmd.Parameters.Add("@AVRMSOS", SqlDbType.Decimal, 8).Value = AVRMSOS;
                cmd.Parameters.Add("@BVRMSOS", SqlDbType.Decimal, 50).Value = BVRMSOS;
                cmd.Parameters.Add("@CVRMSOS", SqlDbType.Decimal, 50).Value = CVRMSOS;
                cmd.Parameters.Add("@APHCAL", SqlDbType.Decimal, 8).Value = APHCAL;
                cmd.Parameters.Add("@BPHCAL", SqlDbType.Decimal, 50).Value = BPHCAL;
                cmd.Parameters.Add("@CPHCAL", SqlDbType.Decimal, 50).Value = CPHCAL;
                cmd.Parameters.Add("@AWATTOS", SqlDbType.Decimal, 8).Value = AWATTOS;
                cmd.Parameters.Add("@BWATTOS", SqlDbType.Decimal, 50).Value = BWATTOS;
                cmd.Parameters.Add("@CWATTOS", SqlDbType.Decimal, 50).Value = CWATTOS;
                cmd.Parameters.Add("@AVAROS", SqlDbType.Decimal, 8).Value = AVAROS;
                cmd.Parameters.Add("@BVAROS", SqlDbType.Decimal, 50).Value = BVAROS;
                cmd.Parameters.Add("@CVAROS", SqlDbType.Decimal, 50).Value = CVAROS;

                cmd.ExecuteNonQuery();
                myConnection.Close();
            }
            catch (Exception ex){ MessageBox.Show(ex.ToString()); }
        }


        public void writeKtoDb(string Kparameters, string deviceId)
        {
            try
            {
                string[] array = Kparameters.Split(',');

                int DeviceId = Int32.Parse(deviceId);
               
                decimal Q1 = decimal.Parse(array[1]);
                decimal Q2 = decimal.Parse(array[2]);
                decimal Q3 = decimal.Parse(array[3]);
                decimal P1 = decimal.Parse(array[4]);
                decimal P2 = decimal.Parse(array[5]);
                decimal P3 = decimal.Parse(array[6]);
                decimal S1 = decimal.Parse(array[7]);
                decimal S2 = decimal.Parse(array[8]);
                decimal S3 = decimal.Parse(array[9]);
                decimal WQ1 = decimal.Parse(array[10]);
                decimal WQ2 = decimal.Parse(array[11]);
                decimal WQ3 = decimal.Parse(array[12]);
                decimal WP1 = decimal.Parse(array[13]);
                decimal WP2 = decimal.Parse(array[14]);
                decimal WP3 = decimal.Parse(array[15]);
                decimal WS1 = decimal.Parse(array[16]);
                decimal WS2 = decimal.Parse(array[17]);
                decimal WS3 = decimal.Parse(array[18]);
                decimal A1 = decimal.Parse(array[19]);
                decimal A2 = decimal.Parse(array[20]);
                decimal A3 = decimal.Parse(array[21]);
                decimal V1 = decimal.Parse(array[22]);
                decimal V2 = decimal.Parse(array[23]);
                decimal V3 = decimal.Parse(array[24]);

                SqlConnection myConnection = new SqlConnection("Data Source=ASUS;Initial Catalog=CalibrationData;Integrated Security=True");
                myConnection.Open();
                SqlCommand cmd = myConnection.CreateCommand();
                cmd.CommandText = "Execute dbo.Test2K @DeviceID,@Q1,@Q2,@Q3,@P1,@P2,@P3,@S1,@S2,@S3,@WQ1,@WQ2,@WQ3,@WP1,@WP2,@WP3,@WS1,@WS2,@WS3,@A1,@A2,@A3,@V1,@V3,@V3";

                cmd.Parameters.Add("@DeviceID", SqlDbType.Decimal, 8).Value = DeviceId;

                cmd.Parameters.Add("@Q1", SqlDbType.Decimal, 8).Value = Q1;
                cmd.Parameters.Add("@Q2", SqlDbType.Decimal, 50).Value = @Q2;
                cmd.Parameters.Add("@Q3", SqlDbType.Decimal, 50).Value = @Q3;
                cmd.Parameters.Add("@P1", SqlDbType.Decimal, 8).Value = P1;
                cmd.Parameters.Add("@P2", SqlDbType.Decimal, 50).Value = P2;
                cmd.Parameters.Add("@P3", SqlDbType.Decimal, 50).Value = P3;
                cmd.Parameters.Add("@S1", SqlDbType.Decimal, 8).Value = S1;
                cmd.Parameters.Add("@S2", SqlDbType.Decimal, 50).Value = S2;
                cmd.Parameters.Add("@S3", SqlDbType.Decimal, 50).Value = S3;
                cmd.Parameters.Add("@WQ1", SqlDbType.Decimal, 8).Value = WQ1;
                cmd.Parameters.Add("@WQ2", SqlDbType.Decimal, 50).Value = WQ2;
                cmd.Parameters.Add("@WQ3", SqlDbType.Decimal, 50).Value = WQ3;
                cmd.Parameters.Add("@WP1", SqlDbType.Decimal, 8).Value = WP1;
                cmd.Parameters.Add("@WP2", SqlDbType.Decimal, 50).Value = WP2;
                cmd.Parameters.Add("@WP3", SqlDbType.Decimal, 50).Value = WP3;
                cmd.Parameters.Add("@WS1", SqlDbType.Decimal, 8).Value = WS1;
                cmd.Parameters.Add("@WS2", SqlDbType.Decimal, 50).Value = WS2;
                cmd.Parameters.Add("@WS3", SqlDbType.Decimal, 50).Value = WS3;
                cmd.Parameters.Add("@A1", SqlDbType.Decimal, 8).Value = A1;
                cmd.Parameters.Add("@A2", SqlDbType.Decimal, 50).Value = A2;
                cmd.Parameters.Add("@A3", SqlDbType.Decimal, 50).Value = A3;
                cmd.Parameters.Add("@V1", SqlDbType.Decimal, 8).Value = V1;
                cmd.Parameters.Add("@V2", SqlDbType.Decimal, 50).Value = V2;
                cmd.Parameters.Add("@V3", SqlDbType.Decimal, 50).Value = V3;

                cmd.ExecuteNonQuery();
                myConnection.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }


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
                sw.WriteLine("There are problems with the data in  " + id + ".csv file");
            }
        }
        public void excecutionErrorSummery(string id, int i)
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
                sw.WriteLine("Couldn't open the port for device " + id);
            }
        }
    }

   




}

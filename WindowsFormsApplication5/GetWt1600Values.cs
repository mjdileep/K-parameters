using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.NI4882;
using System.Threading;

namespace WindowsFormsApplication5
{
    public class GetdataWT1600
    {

        // Program pro=new Program();    

        private Device device;

        int BoardId = 0;
        byte primaryAddress = 1;
        byte currentSecondaryAddress = 0;


        public void OpenDevice()
        {

            device = new Device(0, primaryAddress, currentSecondaryAddress);
            //   SetupControlState(true);

        }

        public void CloseDevice()
        {

            device.Dispose();

        }


        public string writeAndtakeAlltheParameters(string type)
        {
            if (type == "firstCurrent") 
            {
                device.Write(ReplaceCommonEscapeSequences("INPUT:CURRENT:RANGE:ALL AUTO"));
                Thread.Sleep(3000);
            }


            String command = "NUM:NORM:NUM 90\n";
           // device.Write(ReplaceCommonEscapeSequences(ibpct));
            device.Write(ReplaceCommonEscapeSequences(command));
            // device.Write(ReplaceCommonEscapeSequences("*CLS/n"));
            device.Write(ReplaceCommonEscapeSequences("NUM:NORM:VAL?\n"));
            String readbuffer;
            readbuffer = InsertCommonEscapeSequences(device.ReadString(1024));
            // handleCurrent(readbuffer);
            List<string> substrings = readbuffer.Split(',').ToList();
            //  Console.WriteLine(substrings.Count);
            List<decimal> VRMS = new List<decimal>();
            List<decimal> IRMS = new List<decimal>();
            List<decimal> PowerFactor = new List<decimal>();
            List<decimal> ActivePower = new List<decimal>();
            List<decimal> ReactivePower = new List<decimal>();
            List<decimal> ApperentPower = new List<decimal>();
            //decimal temp_value = 0;
            for (int i = 0; i < substrings.Count; i = i + 15)
            {
                VRMS.Add(Decimal.Parse(substrings[i], System.Globalization.NumberStyles.Float));
            }

            for (int i = 4; i < substrings.Count; i = i + 15)
            {
                IRMS.Add(Decimal.Parse(substrings[i], System.Globalization.NumberStyles.Float));
            }

            for (int i = 8; i < substrings.Count; i = i + 15)
            {
                ActivePower.Add(Decimal.Parse(substrings[i], System.Globalization.NumberStyles.Float));
            }

            for (int i = 10; i < substrings.Count; i = i + 15)
            {
                ReactivePower.Add(Decimal.Parse(substrings[i], System.Globalization.NumberStyles.Float));
            }

            for (int i = 9; i < substrings.Count; i = i + 15)
            {
                ApperentPower.Add(Decimal.Parse(substrings[i], System.Globalization.NumberStyles.Float));
            }
          //  if (type != "firstCurrent")
          //  {

           for (int i = 11; i < substrings.Count; i = i + 15)
                { //deviding each elements 8th and 9th elements (from 0 to 14)
                    decimal p = Decimal.Parse(substrings[i], System.Globalization.NumberStyles.Float);

                    PowerFactor.Add(p);
                }
       //     }

            decimal averagedVrms = 0; decimal averagedIrms = 0; decimal averagedPowerFactor = 0; decimal averageActivePower = 0;
            decimal averageReactivePower = 0; decimal averageApperentPower = 0;
            for (int i = 0; i < VRMS.Count; i++)
            {
                averagedVrms = averagedVrms + VRMS[i];
            }


            for (int i = 0; i < IRMS.Count; i++)
            {
                averagedIrms = averagedIrms + IRMS[i];
            }
            for (int i = 0; i < VRMS.Count; i++)
            {
                averageActivePower = averageActivePower + ActivePower[i];
            }

            for (int i = 0; i < VRMS.Count; i++)
            {
                averageReactivePower = averageReactivePower + ReactivePower[i];
            }

            for (int i = 0; i < VRMS.Count; i++)
            {
                averageApperentPower = averageApperentPower + ApperentPower[i];
            }

        //    if (type != "firstCurrent")
          //  {

            for (int i = 0; i < PowerFactor.Count; i++)
            {
                    averagedPowerFactor = averagedPowerFactor + PowerFactor[i];
            }
        //    }

            averagedVrms = averagedVrms / 6;
            averagedIrms = averagedIrms / 6;
            averagedPowerFactor = averagedPowerFactor / 6;
            averageActivePower = averageActivePower / 6;
            averageReactivePower = averageReactivePower / 6;
            averageApperentPower = averageApperentPower / 6;

            device.Write(ReplaceCommonEscapeSequences("*Rst\n"));

            //if (type == "firstCurrent")
            //{
            //    return averagedIrms + "," + averagedVrms;
            //}
            //else
            //{
            return averagedIrms + "," + averagedVrms + "," + averagedPowerFactor + "," + averageActivePower+","+averageReactivePower+","+averageApperentPower;
          //  }
        }

        public string writeAndtakeAlltheParametersForVoltage()
        {
             String command = "NUM:NORM:NUM 90\n";
            // device.Write(ReplaceCommonEscapeSequences(ibpct));
            device.Write(ReplaceCommonEscapeSequences(command));
            // device.Write(ReplaceCommonEscapeSequences("*CLS/n"));
            device.Write(ReplaceCommonEscapeSequences("NUM:NORM:VAL?\n"));
            String readbuffer;
            readbuffer = InsertCommonEscapeSequences(device.ReadString(1024));
            // handleCurrent(readbuffer);
            List<string> substrings = readbuffer.Split(',').ToList();
            //  Console.WriteLine(substrings.Count);
            List<decimal> VRMS = new List<decimal>();
            List<decimal> IRMS = new List<decimal>();
         
            //decimal temp_value = 0;
            for (int i = 0; i < substrings.Count; i = i + 15)
            {
                VRMS.Add(Decimal.Parse(substrings[i], System.Globalization.NumberStyles.Float));
            }

            for (int i = 4; i < substrings.Count; i = i + 15)
            {
                IRMS.Add(Decimal.Parse(substrings[i], System.Globalization.NumberStyles.Float));
            }

            


            decimal averagedVrms = 0; decimal averagedIrms = 0; decimal averagedPowerFactor = 0;
            for (int i = 0; i < VRMS.Count; i++)
            {
                averagedVrms = averagedVrms + VRMS[i];
            }


            for (int i = 0; i < VRMS.Count; i++)
            {
                averagedIrms = averagedIrms + IRMS[i];
            }


            
            averagedVrms = averagedVrms / 6;
            averagedIrms = averagedIrms / 6;
         

            device.Write(ReplaceCommonEscapeSequences("*Rst\n"));

            return averagedIrms + "," + averagedVrms; 
        }


        private string ReplaceCommonEscapeSequences(string s)
        {
            return s.Replace("\\n", "\n").Replace("\\r", "\r");
        }

        private string InsertCommonEscapeSequences(string s)
        {
            return s.Replace("\n", "").Replace("\r", "\\r");
            // return s;
        }




    }

}

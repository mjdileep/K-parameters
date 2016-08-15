using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Xml.Serialization;
using System.Configuration;
using System.Threading;
using System.Diagnostics;
using System.IO;


namespace WindowsFormsApplication5
{
    class ExecutionOfProgramme
    {
        List<string> CurrentCalibrationDatafromRegisters = new List<string>();
        List<string> TestCurrentCalibrationDatafromRegisters = new List<string>();

        List<string> VoltageCalibrationDatafromRegisters = new List<string>();
        List<string> TestVoltageCalibrationDatafromRegisters = new List<string>();

        List<string> PhaseCalibrationDatafromRegisters = new List<string>();
        List<string> TestPhaseCalibrationDatafromRegisters = new List<string>();

        List<string> ActiveEnergyDatafromRegisters = new List<string>();
        List<string> TestActiveEnergyCalibrationDatafromRegisters = new List<string>();

        string indatacurrent; string indataTestcurrent; string indataVoltage; string indataTestVoltage; string indataPhase; string indataTestphase;

        string activeEnergy; string testActiveEnergy;

        static string energyFromTheMeterForActive;
        static string testenergyFromTheMeterForActive;

        static string linecycle;
        static string testLineCycle;

        static string currentPortData;




        List<SerialPort> ports = new List<SerialPort>();
        List<SerialPort> Testports = new List<SerialPort>();

        public void maincontroller(string port)
        { //open the main controllers port and wtrite to it
            try
            {
                SerialPort portMain = new SerialPort(port);
                //  int deviceId = Int32.Parse(deviceDatalist[i].DeviceId);
                // int CT = Int32.Parse(deviceDatalist[i].Ct);

                // Set the properties.
                portMain.BaudRate = 115200;
                portMain.Parity = Parity.None;
                portMain.ReadTimeout = 10;
                portMain.StopBits = StopBits.One;

            }
            catch { Console.Write("error"); }
        }



        //firmware uploader
        public void uploadFirmwares(List<WindowsFormsApplication5.Form1.EnableListDeviceInformation> devicesData, string firmwareName, Label label8)
        {

            List<WindowsFormsApplication5.Form1.EnableListDeviceInformation> deviceDatalist = devicesData;
            try
            {
                for (int i = 0; i < deviceDatalist.Count; i++)
                {
                    try
                    {
                        label8.Text = "Firmware Uploading " + (i + 1) + "/" + deviceDatalist.Count;
                        label8.Refresh();
                        //do whatever need to do to these ports one by one
                        SerialPort port = new SerialPort(deviceDatalist[i].Port);
                        //  int deviceId = Int32.Parse(deviceDatalist[i].DeviceId);
                        //  int CT = Int32.Parse(deviceDatalist[i].Ct);

                        // Set the properties.
                        port.BaudRate = 115200;
                        port.Parity = Parity.None;
                        port.ReadTimeout = 10;
                        port.StopBits = StopBits.One;

                        //upload the firmares oneby one
                        port.PortName = deviceDatalist[i].Port;

                        string workingDirectory = Directory.GetCurrentDirectory();
                        workingDirectory = workingDirectory.Replace("\\", "/");
                        workingDirectory = workingDirectory.Replace("/WindowsFormsApplication5/bin/Release", "/");
                        workingDirectory = workingDirectory.Replace("/WindowsFormsApplication5/bin/Debug", "/");
                        string Filpath = workingDirectory + "AVRDUDEpro";
                        string Command = Filpath + "/epro" + " -C " + Filpath + "/epro.conf" + " -v -patmega2560 -cwiring -" + "P" + port.PortName + " -b115200 -D -Uflash:w:" + Filpath + "/" + firmwareName + ":i";

                        ProcessStartInfo ProcessInfo;
                        Process Process;

                        ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + Command);
                        ProcessInfo.CreateNoWindow = true;
                        ProcessInfo.UseShellExecute = false;
                        ProcessInfo.CreateNoWindow = true;
                        Process = Process.Start(ProcessInfo);
                        port.Close();
                    }
                    catch { continue; }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


        }

    }
}
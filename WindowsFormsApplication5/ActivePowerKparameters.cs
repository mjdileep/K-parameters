using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.NI4882;
using System.Configuration;
using System.Threading;
using System.Diagnostics;
using System.IO.Ports;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    class ActivePowerKparameters
    {
        List<string> ActivePowerRegDatafromRegisters = new List<string>();
        List<string> TestActivePowerRegCalibrationDatafromRegisters = new List<string>();
        List<SerialPort> portsForActive = new List<SerialPort>();
        List<SerialPort> TestportsForReactive = new List<SerialPort>();
      //  static string linecycle;
        static string testLineCycle;
        string ActivePowerReg;
        string testActivePowerReg;
        static string ActivePowerFromMeter;
        static string testActivePowerFromMeter;
        

        public void activePowerMethod(List<Form1.EnableListDeviceInformation> devicesData, string mainport, Label label8, Label label11, Label label12)
        {

            List<WindowsFormsApplication5.Form1.EnableListDeviceInformation> deviceDatalist = devicesData;
            int facter = 0;

            try
            {
               

                // get the correct value from wt1600 for display
                try
                {
                    GetdataWT1600 gDisplay = new GetdataWT1600();
                    gDisplay.OpenDevice();
                    string[] WT1600data3 = gDisplay.writeAndtakeAlltheParameters("firstCurrent").Split(',');
                    gDisplay.CloseDevice();

                    label8.Text = "Active power K parameter";
                    label8.Refresh();

                    label11.Text = WT1600data3[0]; label11.Refresh();
                    label12.Text = WT1600data3[1]; label12.Refresh();
                }
                catch (Exception Exception) { Console.WriteLine(Exception); }
                //////for display
                var stopwatchbeforevolt34 = Stopwatch.StartNew();
                Thread.Sleep(3000);
                stopwatchbeforevolt34.Stop();

                GetdataWT1600 getActiveEnergy = new GetdataWT1600();
                getActiveEnergy.OpenDevice();
                string[] WT1600data = getActiveEnergy.writeAndtakeAlltheParameters("firstCurrent").Split(',');
                ActivePowerFromMeter = WT1600data[3];



                for (int i = 0; i < deviceDatalist.Count; i++)
                {
                    try
                    {
                        label8.Text = "Active power k value " + (i + 1) + "/" + deviceDatalist.Count + "(Total devices)";
                        label8.Refresh();
                        //do whatever need to do to these ports one by one

                        SerialPort port = new SerialPort();
                        port.PortName = deviceDatalist[i].Port;
                        int deviceId = Int32.Parse(deviceDatalist[i].DeviceId);
                        int CT = Int32.Parse(deviceDatalist[i].Ct);

                        // Set the properties.
                        port.BaudRate = 115200;
                        port.Parity = Parity.None;
                        // port.ReadTimeout = 10000;
                        port.StopBits = StopBits.One;
                        port.RtsEnable = true;
                        portsForActive.Add(port);
                        // Write a message into the port.
                        port.Open();

                        //write the messages ,can use relevent CT and deviceId

                    
                      //  port.ReadExisting();//just to make sure indata buffer is cleaned
                        string command ="ACTIVE_POWER";
                        port.Write(command);
                        Console.WriteLine(command + "is written");
                    }
                    catch { continue; }
                }
                //wait till lineCycle complets and collect data
         
                ///wait

                Thread.Sleep(5000);

                //////////////////////////////////////////////////////////////////////////////////////////////
                for (int i = 0; i < deviceDatalist.Count; i++)
                {
                    switch (deviceDatalist[i].Ct)
                    {
                        case "30":
                            facter = 1;
                            break;
                        case "60":
                            facter = 2;
                            break;
                        case "100":
                            facter = 3;
                            break;
                        case "200":
                            facter = 6;
                            break;
                        case "400":
                            facter = 13;
                            break;
                        case "600":
                            facter = 20;
                            break;
                        case "1000":
                            facter = 33;
                            break;
                        case "1200":
                            facter = 40;
                            break;
                        case "1500":
                            facter = 50;
                            break;
                        default:
                            facter = 1;
                            break;
                    }
                    try
                    {
                        int deviceId = Int32.Parse(deviceDatalist[i].DeviceId);
                        int CT = Int32.Parse(deviceDatalist[i].Ct);


                        int temp1 = 0;
                        // portsForReactive[i].DiscardInBuffer();
                        while (ActivePowerReg == null || ActivePowerReg == "")
                        {

                            if (temp1 == 3)
                            {
                                Console.WriteLine ("Data not recieving");
                                break;
                            }
                   
                            ActivePowerReg = portsForActive[i].ReadExisting();
                            Console.WriteLine(temp1 + "=" + ActivePowerReg);
                            temp1++;
                              }

                        string[] array = ActivePowerReg.Split(',');
                        ActivePowerRegDatafromRegisters.Add(ActivePowerReg);

                        double ActivePowerK = facter*double.Parse(ActivePowerFromMeter) / double.Parse(array[1]);
                        double ActivePowerKB = facter*double.Parse(ActivePowerFromMeter) / double.Parse(array[2]);
                        double ActivePowerKC = facter*double.Parse(ActivePowerFromMeter) / double.Parse(array[3]);

                        FileWriter currentfile = new FileWriter();
                        currentfile.Csvwriter(devicesData[i].DeviceId, "ActivePower", (facter * double.Parse(ActivePowerFromMeter)).ToString(), array[1].ToString(), ActivePowerK.ToString(), (facter * double.Parse(ActivePowerFromMeter)).ToString(), array[2].ToString(), ActivePowerKB.ToString(), (facter * double.Parse(ActivePowerFromMeter)).ToString(), array[3].ToString(), ActivePowerKC.ToString());
                      //  MessageBox.Show(ActivePowerK.ToString());
                        ActivePowerReg = null;
                        //when device gives the data ,stop the meter too


                        portsForActive[i].DiscardInBuffer();
                        portsForActive[i].DiscardOutBuffer();
                        portsForActive[i].Close();
                    }
                    catch { continue; }
                }

                //finally finish the integration on meter and take the value


                getActiveEnergy.CloseDevice();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            ActivePowerRegDatafromRegisters.Clear();
            TestActivePowerRegCalibrationDatafromRegisters.Clear();
            portsForActive.Clear();
            TestportsForReactive.Clear();


        }


    }



}



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
    class ReactivePowerKparameters
    {
        List<string> ReactivePowerRegDatafromRegisters = new List<string>();
        List<string> TestReactivePowerRegCalibrationDatafromRegisters = new List<string>();
        List<SerialPort> portsForReactive = new List<SerialPort>();
        List<SerialPort> TestportsForReactive = new List<SerialPort>();
        static string linecycle;
        static string testLineCycle;
        string ReactivePowerReg;
        string testReactivePowerReg;
        static string ReactivePowerFromMeter;
        static string testReactivePowerFromMeter;

        public void reactivePowerMethod(List<Form1.EnableListDeviceInformation> devicesData, string mainport, Label label8, Label label11, Label label12)
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

                    label8.Text = "Reactive power K parameter";
                    label8.Refresh();

                    label11.Text = WT1600data3[0]; label11.Refresh();
                    label12.Text = WT1600data3[1]; label12.Refresh();
                }
                catch { }
                //////for display
                var stopwatchbeforevolt34 = Stopwatch.StartNew();
                Thread.Sleep(3000);
                stopwatchbeforevolt34.Stop();

                GetdataWT1600 getActiveEnergy = new GetdataWT1600();
                getActiveEnergy.OpenDevice();
                string[] WT1600data = getActiveEnergy.writeAndtakeAlltheParameters("firstCurrent").Split(',');
                ReactivePowerFromMeter = WT1600data[4];



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
                        portsForReactive.Add(port);
                        // Write a message into the port.
                        port.Open();

                       
                        //write the messages ,can use relevent CT and deviceId

                        int lineCircles = 200;
                        linecycle = lineCircles.ToString();
                        port.ReadExisting();//just to make sure indata buffer is cleaned
                        string command ="REACTIVE_POWER";
                        port.Write(command);
                        Console.WriteLine(command + "is written");
                    }
                    catch { continue; }
                }
                //wait till lineCycle complets and collect data
                double secondsDouble = Convert.ToDouble("200");
                double tempdub = secondsDouble / 50;
                int linecircles = (int)(Math.Ceiling(tempdub));
                ///wait

                Thread.Sleep(3000);

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
                        while (ReactivePowerReg == null || ReactivePowerReg == "")
                        {

                            if (temp1 ==3)
                            {
                                Console.WriteLine ("Data not recieving");
                                break;
                            }
                          
                            ReactivePowerReg = portsForReactive[i].ReadExisting();
                            Console.WriteLine(temp1 + "=" + ReactivePowerReg);
                            temp1++;
                                }

                        string[] array = ReactivePowerReg.Split(',');
                        ReactivePowerRegDatafromRegisters.Add(ReactivePowerReg);

                        double ReactivePowerK = facter*double.Parse(ReactivePowerFromMeter) / double.Parse(array[1]);
                        double ReactivePowerKB = facter*double.Parse(ReactivePowerFromMeter) / double.Parse(array[2]);
                        double ReactivePowerKC = facter*double.Parse(ReactivePowerFromMeter) / double.Parse(array[3]);
                        FileWriter currentfile = new FileWriter();
                        currentfile.Csvwriter(devicesData[i].DeviceId,"ReactivePower", (facter * double.Parse(ReactivePowerFromMeter)).ToString(), (array[1]).ToString(), ReactivePowerK.ToString(), (facter * double.Parse(ReactivePowerFromMeter)).ToString(), (array[2]).ToString(), ReactivePowerKB.ToString(), (facter * double.Parse(ReactivePowerFromMeter)).ToString(), (array[3]).ToString(), ReactivePowerKC.ToString());
                      
                      //  MessageBox.Show(ReactivePowerK.ToString());
                        ReactivePowerReg = null;
                        //when device gives the data ,stop the meter too


                        portsForReactive[i].DiscardInBuffer();
                        portsForReactive[i].DiscardOutBuffer();
                        portsForReactive[i].Close();
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

            ReactivePowerRegDatafromRegisters.Clear();
            TestReactivePowerRegCalibrationDatafromRegisters.Clear();
            portsForReactive.Clear();
            TestportsForReactive.Clear();

        }


    }



}



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
    class CurrentKparameters
    {
        List<string> CurrentRegDatafromRegisters = new List<string>();
        List<SerialPort> portsForActive = new List<SerialPort>();
        string CurrentReg;
        static string CurrentFromMeter;

        public void CurrentMethod(List<Form1.EnableListDeviceInformation> devicesData, string mainport, Label label8, Label label11, Label label12)
        {

            List<WindowsFormsApplication5.Form1.EnableListDeviceInformation> deviceDatalist = devicesData;
            int facter=0;

            try
            {

                // get the correct value from wt1600 for display
                try
                {
                    GetdataWT1600 gDisplay = new GetdataWT1600();
                    gDisplay.OpenDevice();
                    string[] WT1600data3 = gDisplay.writeAndtakeAlltheParameters("firstCurrent").Split(',');
                    gDisplay.CloseDevice();

                    label8.Text = "Current K parameter";
                    label8.Refresh();

                    label11.Text = WT1600data3[0]; label11.Refresh();
                    label12.Text = WT1600data3[1]; label12.Refresh();
                }
                catch(Exception ex) { Console.WriteLine("getting meter value"+"error is"+ex); }
                //////for display
                var stopwatchbeforevolt34 = Stopwatch.StartNew();
                Thread.Sleep(3000);
                stopwatchbeforevolt34.Stop();

                GetdataWT1600 getActiveEnergy = new GetdataWT1600();
                getActiveEnergy.OpenDevice();
                string[] WT1600data = getActiveEnergy.writeAndtakeAlltheParameters("firstCurrent").Split(',');
                CurrentFromMeter = WT1600data[0];



                for (int i = 0; i < deviceDatalist.Count; i++)
                {
                    try
                    {/////////////// find the CT factor
                        
                        ////////////////
                        label8.Text = "Current k value " + (i + 1) + "/" + deviceDatalist.Count + "(Total devices)";
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

                     
                     //   port.ReadExisting();//just to make sure indata buffer is cleaned
                        string command ="C_CAL";
                        port.Write(command);
                        Console.WriteLine(command + "is written");
                    }
                    catch (Exception Exception) { Console.WriteLine(Exception); continue; }
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
                    //  string portnametemp = "port" + i;
                    // SerialPort port = new SerialPort(deviceDatalist[i].Port);
                    try
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


                        int deviceId = Int32.Parse(deviceDatalist[i].DeviceId);
                        int CT = Int32.Parse(deviceDatalist[i].Ct);


                        int temp1 = 0;
                        // portsForReactive[i].DiscardInBuffer();
                        while (CurrentReg == null || CurrentReg == "")
                        {

                            if (temp1 == 3)
                            {
                                Console.WriteLine("Data not recieving");
                                break;
                            }
                           
                            CurrentReg = portsForActive[i].ReadExisting();
                            Console.WriteLine(temp1 + "=" + CurrentReg);
                            temp1++;
                            CurrentRegDatafromRegisters.Add(CurrentReg);

                              }

                        string []array = CurrentReg.Split(',');
                        double CurrentK = double.Parse(array[1]) / ((facter)*double.Parse(CurrentFromMeter));
                        double CurrentKB = double.Parse(array[2]) / ((facter) * double.Parse(CurrentFromMeter));
                        double CurrentKC = double.Parse(array[3]) / ((facter)*double.Parse(CurrentFromMeter));
                        FileWriter currentfile=new FileWriter();
                        currentfile.Csvwriter(devicesData[i].DeviceId, "Current", array[1].ToString(), ((facter) * double.Parse(CurrentFromMeter)).ToString(), CurrentK.ToString(), array[2].ToString(), ((facter) * double.Parse(CurrentFromMeter)).ToString(), CurrentKB.ToString(), array[2].ToString(), ((facter) * double.Parse(CurrentFromMeter)).ToString(), CurrentKC.ToString());
                        
                      //  MessageBox.Show(CurrentK.ToString());
                        CurrentReg = null;
                        //when device gives the data ,stop the meter too


                        portsForActive[i].DiscardInBuffer();
                        portsForActive[i].DiscardOutBuffer();
                        portsForActive[i].Close();
                    }
                    catch (Exception ex) { Console.WriteLine(  ex ); continue; }
                }

                //finally finish the integration on meter and take the value


                getActiveEnergy.CloseDevice();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            CurrentRegDatafromRegisters.Clear();
            portsForActive.Clear();
        }


    }



}



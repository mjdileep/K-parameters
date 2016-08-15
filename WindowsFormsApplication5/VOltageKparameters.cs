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
    class VoltageKparameters
    {
        List<string> VoltageRegDatafromRegisters = new List<string>();
        List<SerialPort> portsForActive = new List<SerialPort>();
        string VoltageReg;
        static string VoltageFromMeter;

        public void VoltageMethod(List<Form1.EnableListDeviceInformation> devicesData, string mainport, Label label8, Label label11, Label label12)
        {

            List<WindowsFormsApplication5.Form1.EnableListDeviceInformation> deviceDatalist = devicesData;


            try
            {

                // get the correct value from wt1600 for display
                try
                {
                    GetdataWT1600 gDisplay = new GetdataWT1600();
                    gDisplay.OpenDevice();
                    string[] WT1600data3 = gDisplay.writeAndtakeAlltheParameters("firstCurrent").Split(',');
                    gDisplay.CloseDevice();

                    label8.Text = "Voltage K parameter";
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
                VoltageFromMeter = WT1600data[1];



                for (int i = 0; i < deviceDatalist.Count; i++)
                {
                    try
                    {
                        label8.Text = "Voltage k value " + (i + 1) + "/" + deviceDatalist.Count + "(Total devices)";
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

                        port.ReadExisting();//just to make sure indata buffer is cleaned
                        string command = "V_CAL";
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

                Thread.Sleep(linecircles * 1000 + 3000);

                //////////////////////////////////////////////////////////////////////////////////////////////
                for (int i = 0; i < deviceDatalist.Count; i++)
                {
                    //  string portnametemp = "port" + i;
                    // SerialPort port = new SerialPort(deviceDatalist[i].Port);
                    try
                    {
                        int deviceId = Int32.Parse(deviceDatalist[i].DeviceId);
                        int CT = Int32.Parse(deviceDatalist[i].Ct);


                        int temp1 = 0;
                        // portsForReactive[i].DiscardInBuffer();
                        while (VoltageReg == null || VoltageReg == "")
                        {

                            if (temp1 ==3)
                            {
                                MessageBox.Show("Data not recieving");
                                break;
                            }
                           
                            VoltageReg = portsForActive[i].ReadExisting();
                            Console.WriteLine(temp1 + "=" + VoltageReg);
                            temp1++;
                            VoltageRegDatafromRegisters.Add(VoltageReg);

                        }
                        string[] array = VoltageReg.Split(',');
                        double VoltageK = double.Parse(array[1]) / double.Parse(VoltageFromMeter);
                        double VoltageKB = double.Parse(array[2]) / double.Parse(VoltageFromMeter);
                        double VoltageKC = double.Parse(array[3]) / double.Parse(VoltageFromMeter);
                        FileWriter currentfile = new FileWriter();
                        currentfile.Csvwriter(devicesData[i].DeviceId,"Voltage",(array[1]).ToString(), VoltageFromMeter.ToString(), VoltageK.ToString(), (array[2]).ToString(), VoltageFromMeter.ToString(), VoltageKB.ToString(), (array[3]).ToString(), VoltageFromMeter.ToString(), VoltageKC.ToString());
                        
                      //  MessageBox.Show(VoltageK.ToString());
                        VoltageReg = null;
                    
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

            VoltageRegDatafromRegisters.Clear();
            portsForActive.Clear();

        }


    }



}



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
    class ApperentEnergyKparameters
    {
        List<string> ApperentEnergyRegDatafromRegisters = new List<string>();
        List<SerialPort> portsForActive = new List<SerialPort>();
        string ApperentEnergyReg;
        static string ApperentEnergyFromMeter;
        static string powerFactor;

        public void ApperentEnergyMethod(List<Form1.EnableListDeviceInformation> devicesData, string mainport, Label label8, Label label11, Label label12)
        {
            int facter = 0;
            List<WindowsFormsApplication5.Form1.EnableListDeviceInformation> deviceDatalist = devicesData;


            try
            {
                

                // get the correct value from wt1600 for display
                try
                {
                    GetdataWT1600 gDisplay = new GetdataWT1600();
                    gDisplay.OpenDevice();
                    string[] WT1600data3 = gDisplay.writeAndtakeAlltheParameters("firstApperent").Split(',');
                    gDisplay.CloseDevice();

                    label8.Text = "Reactive energy K parameter";
                    label8.Refresh();

                    label11.Text = WT1600data3[0]; label11.Refresh();
                    label12.Text = WT1600data3[1]; label12.Refresh();
                    powerFactor = WT1600data3[2];

                }
                catch (Exception Exception) { Console.WriteLine(Exception); }
                //////for display
                var stopwatchbeforevolt34 = Stopwatch.StartNew();
                Thread.Sleep(3000);
                stopwatchbeforevolt34.Stop();

                GetdataWT1600Energy getApperentEnergy = new GetdataWT1600Energy();
                getApperentEnergy.OpenDevice();
                string time = "0,20,0";
                getApperentEnergy.startIntegration(time, "high");



                for (int i = 0; i < deviceDatalist.Count; i++)
                {
                    try
                    {
                        label8.Text = "Apperent energy k value " + (i + 1) + "/" + deviceDatalist.Count + "(Total devices)";
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


                        //just to make sure indata buffer is cleaned
                        string command = "APP_ENERGY,20,\n";
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

                Thread.Sleep(1200000 + 18000);

                //////////////////////////////////////////////////////////////////////////////////////////////
                string[] WT1600data = getApperentEnergy.returnEnergy("Energynormal").Split(',');
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
                        while (ApperentEnergyReg == null || ApperentEnergyReg == "")
                        {

                            if (temp1 == 5)
                            {
                                Console.WriteLine ("Data not recieving");
                                break;
                            }
                      
                            ApperentEnergyReg = portsForActive[i].ReadExisting();

                            if (ApperentEnergyReg == "") { Thread.Sleep(2000); }

                            Console.WriteLine(temp1 + "=" + ApperentEnergyReg);
                            temp1++;
                            ApperentEnergyRegDatafromRegisters.Add(ApperentEnergyReg);

                            
                        }
                        
                        ApperentEnergyFromMeter = WT1600data[0];

                       // getApperentEnergy.CloseDevice();
                        double meterValForreactive = ((double.Parse(WT1600data[0])) * Math.Tan(Math.Acos((double)(decimal.Parse(powerFactor)))));
                        double meterValForactive = ((double.Parse(WT1600data[0])));
                        double meterValApperent = facter*Math.Sqrt(meterValForreactive * meterValForreactive + meterValForactive * meterValForactive);
                        
                        string[] array = ApperentEnergyReg.Split(',');
                        
                        double ApperentEnergyK  = (double.Parse(meterValApperent.ToString()) / double.Parse(array[1])) / 1000;
                        double ApperentEnergyKB = (double.Parse(meterValApperent.ToString()) / double.Parse(array[2])) / 1000;
                        double ApperentEnergyKC = (double.Parse(meterValApperent.ToString()) / double.Parse(array[3])) / 1000;


                        FileWriter currentfile = new FileWriter();
                        currentfile.Csvwriter(devicesData[i].DeviceId, "ApperentEnergy", (double.Parse((meterValApperent / 1000).ToString()).ToString()), (array[1]).ToString(), ApperentEnergyK.ToString(), (double.Parse((meterValApperent / 1000).ToString()).ToString()), (array[2]).ToString(), ApperentEnergyKB.ToString(), (double.Parse((meterValApperent / 1000).ToString()).ToString()), (array[3]).ToString(), ApperentEnergyKC.ToString());
                      
                        //   MessageBox.Show(ApperentEnergyK.ToString());
                        ApperentEnergyReg = null;
                        //when device gives the data ,stop the meter too


                        portsForActive[i].DiscardInBuffer();
                        portsForActive[i].DiscardOutBuffer();
                        portsForActive[i].Close();
                    }
                    catch (Exception Exception){ Console.WriteLine( Exception ); continue; }
                }

                //finally finish the integration on meter and take the value


              //  getApperentEnergy.CloseDevice();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            ApperentEnergyRegDatafromRegisters.Clear();
            portsForActive.Clear();

        }


    }



}



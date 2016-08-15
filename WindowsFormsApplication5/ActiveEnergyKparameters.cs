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
    class ActiveEnergyKparameters
    {
        List<string> ActiveEnergyRegDatafromRegisters = new List<string>();
        List<SerialPort> portsForActive = new List<SerialPort>();
        string ActiveEnergyReg;
        static string ActiveEnergyFromMeter;

   
        public void ActiveEnergyMethod(List<Form1.EnableListDeviceInformation> devicesData, string mainport, Label label8, Label label11, Label label12)
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
                catch { }
                //////for display
                var stopwatchbeforevolt34 = Stopwatch.StartNew();
                Thread.Sleep(3000);
                stopwatchbeforevolt34.Stop();

                GetdataWT1600Energy getActiveEnergy = new GetdataWT1600Energy();
                getActiveEnergy.OpenDevice();
             
                string time = "0,20,0";
                getActiveEnergy.startIntegration(time, "high");



                for (int i = 0; i < deviceDatalist.Count; i++)
                {
                   
                    try
                    {
                        label8.Text = "Active energy k value " + (i + 1) + "/" + deviceDatalist.Count + "(Total devices)";
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


                       // port.ReadExisting();//just to make sure indata buffer is cleaned
                        string command = "A_ENERGY,20,\n";
                        port.Write(command);
                        Console.WriteLine(command + "is written");
                    }
                    catch { continue; }
                }
                //wait till lineCycle complets and collect data
               
                ///wait

                Thread.Sleep(1200000+ 18000);

                //////////////////////////////////////////////////////////////////////////////////////////////
                string[] WT1600data = getActiveEnergy.returnEnergy("Energynormal").Split(',');
               // getActiveEnergy.CloseDevice();
                for (int i = 0; i < deviceDatalist.Count; i++)
                {
                    //  string portnametemp = "port" + i;
                    // SerialPort port = new SerialPort(deviceDatalist[i].Port);
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
                        //get the value of meter
                        
                        

                        while (ActiveEnergyReg == null || ActiveEnergyReg == "")
                        {

                            if (temp1 ==5)
                            {
                                Console.WriteLine ("Data not recieving");
                                break;
                            }
                           
                            ActiveEnergyReg = portsForActive[i].ReadExisting();
                            Console.WriteLine(temp1 + "=" + ActiveEnergyReg);
                            temp1++;
                            ActiveEnergyRegDatafromRegisters.Add(ActiveEnergyReg);

                            if (ActiveEnergyReg == "") { Thread.Sleep(2000); }

                            ActiveEnergyFromMeter = WT1600data[0];
                              }

                        string[] array = ActiveEnergyReg.Split(',');
                        double ActiveEnergyK = ((facter*double.Parse(ActiveEnergyFromMeter)) / double.Parse(array[1]))/1000;
                        double ActiveEnergyKB = ((facter*double.Parse(ActiveEnergyFromMeter)) / double.Parse(array[2]))/1000;
                        double ActiveEnergyKC = ((facter*double.Parse(ActiveEnergyFromMeter)) / double.Parse(array[3]))/1000;
                        FileWriter currentfile = new FileWriter();
                        currentfile.Csvwriter(devicesData[i].DeviceId, "ActiveEnergy",(((facter*double.Parse(ActiveEnergyFromMeter))/1000).ToString()),array[1].ToString(), ActiveEnergyK.ToString(),(((facter*double.Parse(ActiveEnergyFromMeter))/1000).ToString()),array[2].ToString(),ActiveEnergyKB.ToString(),(((facter*double.Parse(ActiveEnergyFromMeter))/1000).ToString()),array[1].ToString(),ActiveEnergyKC.ToString());
                      
                      //  MessageBox.Show(ActiveEnergyK.ToString());
                        ActiveEnergyReg = null;
                        //when device gives the data ,stop the meter too


                        portsForActive[i].DiscardInBuffer();
                        portsForActive[i].DiscardOutBuffer();
                        portsForActive[i].Close();
                    }
                    catch { continue; }
                }

                //finally finish the integration on meter and take the value
           
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                GetdataWT1600Energy getActiveEnergy = new GetdataWT1600Energy();
                getActiveEnergy.CloseDevice();
            }

            ActiveEnergyRegDatafromRegisters.Clear();
            portsForActive.Clear();

        }


    }



}



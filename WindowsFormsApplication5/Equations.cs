//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.IO.Ports;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;


//namespace WindowsFormsApplication5
//{
//    class Equations
//    {

//        internal void currentCalibarionCalculations(List<ExecutionOfProgramme.testcurrentfromgatherdata> TestCurrentDataObjects, List<ExecutionOfProgramme.currentlistfromgatherdData> CurrentDataObjects)
//        {
//            try
//            {
//                List<ExecutionOfProgramme.currentlistfromgatherdData> CurrentcaliDataObjectlist = CurrentDataObjects;
//                List<ExecutionOfProgramme.testcurrentfromgatherdata> TestCurrentcaliDataObjectlist = TestCurrentDataObjects;
//                //  var finalcurrentObjects = new List<currentlistTowriteFile>();
//                ///////////////////for each device do the following and write to a csv file
//                for (int i = 0; i < CurrentcaliDataObjectlist.Count; i++)
//                {
//                    try
//                    {
//                        int facter;
//                        switch (CurrentcaliDataObjectlist[i].CT)
//                        {
//                            case "30":
//                                facter = 1;
//                                break;
//                            case "60":
//                                facter = 2;
//                                break;
//                            case "100":
//                                facter = 3;
//                                break;
//                            case "200":
//                                facter = 6;
//                                break;
//                            case "400":
//                                facter = 13;
//                                break;
//                            case "600":
//                                facter = 20;
//                                break;
//                            case "1000":
//                                facter = 33;
//                                break;
//                            case "1200":
//                                facter = 40;
//                                break;
//                            case "1500":
//                                facter = 50;
//                                break;
//                            default:
//                                facter = 1;
//                                break;
//                        }

//                        currentlistTowriteFile temp = new currentlistTowriteFile();
//                        decimal fulscaleCurrentdiv500 = decimal.Parse(CurrentcaliDataObjectlist[i].Wt1600Readcurrent) * facter;
//                        decimal PhaseARegValuForFcurrentDiv50 = decimal.Parse(CurrentcaliDataObjectlist[i].PhaseA);
//                        decimal PhaseBRegValuForFcurrentDiv50 = decimal.Parse(CurrentcaliDataObjectlist[i].PhaseB);
//                        decimal PhaseCRegValuForFcurrentDiv50 = decimal.Parse(CurrentcaliDataObjectlist[i].PhaseC);
//                        decimal testCurrent = decimal.Parse(TestCurrentcaliDataObjectlist[i].testWt1600Readcurrent) * facter;
//                        decimal RegvalueFORTestCurrent = decimal.Parse(TestCurrentcaliDataObjectlist[i].testPhaseA);
//                        decimal RegvaluePhaseBFORTestCurrent = decimal.Parse(TestCurrentcaliDataObjectlist[i].testPhaseB);
//                        decimal RegvaluePhaseCFORTestCurrent = decimal.Parse(TestCurrentcaliDataObjectlist[i].testPhaseC);
//                        // decimal RegvalueFORTestCurrent = decimal.Parse(TestCurrentcaliDataObjectlist[i].testPhaseA);

//                        decimal IRMSOS = (testCurrent * testCurrent * PhaseARegValuForFcurrentDiv50 * PhaseARegValuForFcurrentDiv50 - ((fulscaleCurrentdiv500 * fulscaleCurrentdiv500) * (RegvalueFORTestCurrent * RegvalueFORTestCurrent))) / (16384 * ((fulscaleCurrentdiv500 * fulscaleCurrentdiv500) - (testCurrent * testCurrent)));
//                        decimal IRMSOSPhaseB = (testCurrent * testCurrent * PhaseBRegValuForFcurrentDiv50 * PhaseARegValuForFcurrentDiv50 - ((fulscaleCurrentdiv500 * fulscaleCurrentdiv500) * (RegvaluePhaseBFORTestCurrent * RegvaluePhaseBFORTestCurrent))) / (16384 * ((fulscaleCurrentdiv500 * fulscaleCurrentdiv500) - (testCurrent * testCurrent)));
//                        decimal IRMSOSPhaseC = (testCurrent * testCurrent * PhaseCRegValuForFcurrentDiv50 * PhaseARegValuForFcurrentDiv50 - ((fulscaleCurrentdiv500 * fulscaleCurrentdiv500) * (RegvaluePhaseCFORTestCurrent * RegvaluePhaseCFORTestCurrent))) / (16384 * ((fulscaleCurrentdiv500 * fulscaleCurrentdiv500) - (testCurrent * testCurrent)));
//                        //  string  hexValue_currentcalibraion = IRMSOS.ToString("X");
//                        temp.deviceID = CurrentcaliDataObjectlist[i].DeviceId;
//                        temp.fulscaleCurrentdiv500 = fulscaleCurrentdiv500;
//                        temp.PhaseARegValuForFcurrentDiv50 = PhaseARegValuForFcurrentDiv50;
//                        temp.PhaseBRegValuForFcurrentDiv50 = PhaseBRegValuForFcurrentDiv50;
//                        temp.PhaseCRegValuForFcurrentDiv50 = PhaseCRegValuForFcurrentDiv50;
//                        temp.testCurrent = testCurrent;
//                        temp.RegvaluePhaseAforTestCurrent = RegvalueFORTestCurrent;
//                        temp.RegvaluePhaseBforTestCurrent = RegvaluePhaseBFORTestCurrent;
//                        temp.RegvaluePhaseCforTestCurrent = RegvaluePhaseCFORTestCurrent;
//                        temp.IRMSOSPhaseA = IRMSOS;
//                        temp.IRMSOSPhaseB = IRMSOSPhaseB;
//                        temp.IRMSOSPhaseC = IRMSOSPhaseC;

//                        //calculate the K parameters
//                        decimal KvalueCurrent = PhaseARegValuForFcurrentDiv50 / fulscaleCurrentdiv500;

//                        temp.port = CurrentcaliDataObjectlist[i].port;
//                        filewriter(temp);

//                        SerialPort port = new SerialPort();
//                        port.PortName = CurrentcaliDataObjectlist[i].port;
//                        port.BaudRate = 115200;
//                        port.Parity = Parity.None;
//                        port.StopBits = StopBits.One;
//                        port.RtsEnable = true;
 
                     
                        
//                      ////////////////////////////
//                    }
//                    catch { continue; }
//                }
//            }
//            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
//            /////////////////////

//        }

//        internal void VoltageCalibarionCalculations(List<ExecutionOfProgramme.testvoltagelistfromgatherdData> TestVoltageDataObjects, List<ExecutionOfProgramme.voltagelistfromgatherdData> voltagelist)
//        {
//            try
//            {
//                List<ExecutionOfProgramme.voltagelistfromgatherdData> VoltagecaliDataObjectlist = voltagelist;
//                List<ExecutionOfProgramme.testvoltagelistfromgatherdData> testvoltagelistfromgatherdData = TestVoltageDataObjects;
//                for (int i = 0; i < voltagelist.Count; i++)
//                {
//                    try
//                    {
//                        VoltagelistTowriteFile temp = new VoltagelistTowriteFile();
//                        decimal fulscaleVoltageDiv20 = decimal.Parse(voltagelist[i].Wt1600ReadVoltage);
//                        decimal PhaseARegValuForFVoltageDiv20 = decimal.Parse(voltagelist[i].PhaseA);
//                        decimal PhaseBRegValuForFVoltageDiv20 = decimal.Parse(voltagelist[i].PhaseB);
//                        decimal PhaseCRegValuForFVoltageDiv20 = decimal.Parse(voltagelist[i].PhaseC);


//                        decimal testVoltage = decimal.Parse(TestVoltageDataObjects[i].testWt1600ReadVoltage);
//                        decimal testRegvalForVoltagePhaseA = decimal.Parse(testvoltagelistfromgatherdData[i].testPhaseA);
//                        decimal testRegvalForVoltagePhaseB = decimal.Parse(testvoltagelistfromgatherdData[i].testPhaseB);
//                        decimal testRegvalForVoltagePhaseC = decimal.Parse(testvoltagelistfromgatherdData[i].testPhaseC);

//                        decimal VRMSOS = ((testVoltage * PhaseARegValuForFVoltageDiv20) - (fulscaleVoltageDiv20 * testRegvalForVoltagePhaseA)) / (64 * (fulscaleVoltageDiv20 - testVoltage));
//                        decimal PhaseBVRMSOS = ((testVoltage * PhaseBRegValuForFVoltageDiv20) - (fulscaleVoltageDiv20 * testRegvalForVoltagePhaseB)) / (64 * (fulscaleVoltageDiv20 - testVoltage));
//                        decimal PhaseCVRMSOS = ((testVoltage * PhaseCRegValuForFVoltageDiv20) - (fulscaleVoltageDiv20 * testRegvalForVoltagePhaseC)) / (64 * (fulscaleVoltageDiv20 - testVoltage));

//                        temp.deviceID = voltagelist[i].DeviceId;
//                        temp.fulscaleVoltageDiv20 = fulscaleVoltageDiv20;
//                        temp.PhaseARegValuForFVoltageDiv20 = PhaseARegValuForFVoltageDiv20;
//                        temp.PhaseBRegValuForFVoltageDiv20 = PhaseBRegValuForFVoltageDiv20;
//                        temp.PhaseCRegValuForFVoltageDiv20 = PhaseCRegValuForFVoltageDiv20;
//                        temp.testVoltage = testVoltage;
//                        temp.testRegvalForVoltagePhaseA = testRegvalForVoltagePhaseA;
//                        temp.testRegvalForVoltagePhaseB = testRegvalForVoltagePhaseB;
//                        temp.testRegvalForVoltagePhaseC = testRegvalForVoltagePhaseC;
//                        temp.PhaseAVRMSOS = VRMSOS;
//                        temp.PhaseBVRMSOS = PhaseBVRMSOS;
//                        temp.PhaseCVRMSOS = PhaseCVRMSOS;
//                        // string  hexValue_votagecalibraion = VRMSOS.ToString("X");

//                        decimal KvalueVoltage = PhaseARegValuForFVoltageDiv20 / fulscaleVoltageDiv20;

//                        filewriterforVoltage(temp);
//                        //write observed offset parameters to the epro
//                        SerialPort port = new SerialPort();
//                        port.PortName = VoltagecaliDataObjectlist[i].port;
//                        port.BaudRate = 115200;
//                        port.Parity = Parity.None;
//                        port.StopBits = StopBits.One;
//                        port.RtsEnable = true;

//                        //write to the port    
                      
//                    }
//                    catch { continue; }
//                }
//            }
//            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
//        }

//        internal void PhaseCalibarionCalculations(List<ExecutionOfProgramme.TestPhaselistfromgatherdData> TestPhaseDataObjects, List<ExecutionOfProgramme.PhaselistfromgatherdData> PhaseDataObjects)
//        {
//            try
//            {
//                List<ExecutionOfProgramme.PhaselistfromgatherdData> PhasecaliDataObjectlist = PhaseDataObjects;
//                List<ExecutionOfProgramme.TestPhaselistfromgatherdData> testPhaselistfromgatherdData = TestPhaseDataObjects;
//                for (int i = 0; i < PhasecaliDataObjectlist.Count; i++)
//                {
//                    try
//                    {
//                        PhaselistTowriteFile temp = new PhaselistTowriteFile();

//                        decimal Power_at_pf_1_at_Itest_and_Vnom_forPhaseA = decimal.Parse(PhaseDataObjects[i].PhaseA);
//                        decimal Power_at_pf_1_at_Itest_and_Vnom_forPhaseB = decimal.Parse(PhaseDataObjects[i].PhaseB);
//                        decimal Power_at_pf_1_at_Itest_and_Vnom_forPhaseC = decimal.Parse(PhaseDataObjects[i].PhaseC);

//                        decimal Power_at_pf_x_at_Itest_and_VnomforPhaseA = decimal.Parse(TestPhaseDataObjects[i].testPhaseA);
//                        decimal Power_at_pf_x_at_Itest_and_VnomforPhaseB = decimal.Parse(TestPhaseDataObjects[i].testPhaseB);
//                        decimal Power_at_pf_x_at_Itest_and_VnomforPhaseC = decimal.Parse(TestPhaseDataObjects[i].testPhaseC);

//                        double Used_Power_factor = (double)(decimal.Parse(TestPhaseDataObjects[i].testWt1600ReadPhase));

//                        double errorPhaseA = ((double)Power_at_pf_x_at_Itest_and_VnomforPhaseA - ((double)Power_at_pf_1_at_Itest_and_Vnom_forPhaseA * Used_Power_factor)) / (double)((double)Power_at_pf_1_at_Itest_and_Vnom_forPhaseA * Used_Power_factor);
//                        double errorPhaseB = ((double)Power_at_pf_x_at_Itest_and_VnomforPhaseB - ((double)Power_at_pf_1_at_Itest_and_Vnom_forPhaseB * Used_Power_factor)) / (double)((double)Power_at_pf_1_at_Itest_and_Vnom_forPhaseB * Used_Power_factor);
//                        double errorPhaseC = ((double)Power_at_pf_x_at_Itest_and_VnomforPhaseC - ((double)Power_at_pf_1_at_Itest_and_Vnom_forPhaseC * Used_Power_factor)) / (double)((double)Power_at_pf_1_at_Itest_and_Vnom_forPhaseC * Used_Power_factor);

//                        double tempA = (double)((decimal)errorPhaseA / Convert.ToDecimal(Math.Sqrt(3)));
//                        double tempB = (double)((decimal)errorPhaseB / Convert.ToDecimal(Math.Sqrt(3)));
//                        double tempC = (double)((decimal)errorPhaseC / Convert.ToDecimal(Math.Sqrt(3)));

//                        double phaseErrorA = -Math.Asin(tempA);
//                        double phaseErrorB = -Math.Asin(tempB);
//                        double phaseErrorC = -Math.Asin(tempC);

//                        double varA = 0; double varB = 0; double varC = 0;
//                        if (errorPhaseA < 0) { varA = 1.2; }
//                        if (errorPhaseA >= 0) { varA = 2.4; }
//                        if (errorPhaseB < 0) { varB = 1.2; }
//                        if (errorPhaseB >= 0) { varB = 2.4; }
//                        if (errorPhaseC < 0) { varC = 1.2; }
//                        if (errorPhaseC >= 0) { varC = 2.4; }
                        
//                        decimal xphcalA;decimal xphcalB;decimal xphcalC;
//                        if ((phaseErrorA / (varA * 1 * 2 * Math.PI * Math.Pow(10, -6))) <= -64) {  xphcalA = -64; }
//                        if ((phaseErrorA / (varA * 1 * 2 * Math.PI * Math.Pow(10, -6))) > 63) { xphcalA = 63; }
//                        else { xphcalA=(decimal)(phaseErrorA / (varA * 1 * 2 * Math.PI * Math.Pow(10, -6))); }

//                        if ((phaseErrorB / (varB * 1 * 2 * Math.PI * Math.Pow(10, -6))) <= -64) {xphcalB = -64; }
//                        if ((phaseErrorB / (varB * 1 * 2 * Math.PI * Math.Pow(10, -6))) > 63) {  xphcalB= 63; }
//                        else { xphcalB = (decimal)(phaseErrorB / (varB * 1 * 2 * Math.PI * Math.Pow(10, -6))); }

//                        if ((phaseErrorC / (varC * 1 * 2 * Math.PI * Math.Pow(10, -6))) <= -64) { xphcalC = -64; }
//                        if ((phaseErrorC / (varC * 1 * 2 * Math.PI * Math.Pow(10, -6))) > 63) {  xphcalC= 63; }
//                        else { xphcalC = (decimal)(phaseErrorA / (varC * 1 * 2 * Math.PI * Math.Pow(10, -6))); }
                        
//                        temp.deviceID = PhaseDataObjects[i].DeviceId;
//                        temp.Power_at_pf_1_at_Itest_and_Vnom_phaseA = Power_at_pf_1_at_Itest_and_Vnom_forPhaseA;
//                        temp.Power_at_pf_1_at_Itest_and_Vnom_phaseB = Power_at_pf_1_at_Itest_and_Vnom_forPhaseB;
//                        temp.Power_at_pf_1_at_Itest_and_Vnom_phaseC = Power_at_pf_1_at_Itest_and_Vnom_forPhaseC;
//                        temp.Power_at_pf_x_at_Itest_and_Vnom_phaseA = Power_at_pf_x_at_Itest_and_VnomforPhaseA;
//                        temp.Power_at_pf_x_at_Itest_and_Vnom_phaseB = Power_at_pf_x_at_Itest_and_VnomforPhaseB;
//                        temp.Power_at_pf_x_at_Itest_and_Vnom_phaseC = Power_at_pf_x_at_Itest_and_VnomforPhaseC;
//                        temp.ErrorA = errorPhaseA;
//                        temp.ErrorB = errorPhaseB;
//                        temp.ErrorC = errorPhaseC;
//                        temp.Phase_ErrorA = (decimal)phaseErrorA;
//                        temp.Phase_ErrorB = (decimal)phaseErrorB;
//                        temp.Phase_ErrorC = (decimal)phaseErrorC;
                       
//                        temp.xPHCALA = xphcalA;
//                        temp.xPHCALB = xphcalB;
//                        temp.xPHCALC = xphcalC;

//                        temp.Used_Power_factor = Used_Power_factor;

//                   //     decimal KvaluePhase = decimal.Parse(PhaseDataObjects[i].PhaseA) / fulscaleVoltageDiv20;

//                        filewriterforPhase(temp);
//                        SerialPort port = new SerialPort();
//                        port.PortName = PhaseDataObjects[i].port;
//                        port.BaudRate = 115200;
//                        port.Parity = Parity.None;
//                        port.StopBits = StopBits.One;
//                        port.RtsEnable = true;

//                        //write to the port    
//                        port.Open();
//                        string portTemporyOffsets = "OFFSET_Ph," + Math.Round((xphcalA)) + "," + Math.Round(xphcalB) + "," + Math.Round(xphcalC) + ",";
//                        port.Write(portTemporyOffsets);
//                        port.DiscardInBuffer();
//                        port.DiscardOutBuffer();
//                        port.Close();

//                    }
//                    catch { continue; }
//                }
//            }
//            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            
//        }

//        internal void EneryCalibarionCalculations(List<ExecutionOfProgramme.TestActiveEnergylistfromgatherdData> TestEnergyDataObjects, List<ExecutionOfProgramme.ActiveEnergylistfromgatherdData> ActiveEnergyDataObjects)
//        {
//            List<ExecutionOfProgramme.ActiveEnergylistfromgatherdData> EnergycaliDataObjectlist = ActiveEnergyDataObjects;
//            List<ExecutionOfProgramme.TestActiveEnergylistfromgatherdData> testEnergylistfromgatherdData = TestEnergyDataObjects;
//            for (int i = 0; i < EnergycaliDataObjectlist.Count; i++)
//            {
//                try
//                {
//                    // double Fullscalecurrentdiv500 ;
//                    double PhaseACorrespondingWATTHRregValueForFirst = double.Parse(EnergycaliDataObjectlist[i].phaseAenergy);
//                    double PhaseBCorrespondingWATTHRregValueForFirst = double.Parse(EnergycaliDataObjectlist[i].phaseBenergy);
//                    double PhaseCCorrespondingWATTHRregValueForFirst = double.Parse(EnergycaliDataObjectlist[i].phaseCenergy);
//                    double LineCycle1 = double.Parse(EnergycaliDataObjectlist[i].lineCircle);
//                    double firstMeasuredEnergy = double.Parse(EnergycaliDataObjectlist[i].Wt1600MeasuredEnergy);
//                    //double testCurrent = double.Parse(EnergycaliDataObjectlist[i].t);
//                    double TestPhaseACorrespondingWATTHRregValue = double.Parse(TestEnergyDataObjects[i].TestphaseAenergy);
//                    double TestPhaseBCorrespondingWATTHRregValue = double.Parse(TestEnergyDataObjects[i].TestphaseBenergy);
//                    double TestPhaseCCorrespondingWATTHRregValue = double.Parse(TestEnergyDataObjects[i].TestphaseCenergy);
//                    double TestLineCycle = double.Parse(testEnergylistfromgatherdData[i].TestlineCircle);
//                    double secondMeasuredEnergy = double.Parse(testEnergylistfromgatherdData[i].TestWt1600MeasuredEnergy);
//                    //  double offset = ((CorrespondingWATTHRregValue * testCurrent) - (((TestCorrespondingWATTHRregValue * LineCycle1) / TestLineCycle) * Fullscalecurrentdiv500)) / (Fullscalecurrentdiv500 - testCurrent);

//                    double offsetA = ((PhaseACorrespondingWATTHRregValueForFirst * secondMeasuredEnergy) - (TestPhaseACorrespondingWATTHRregValue * firstMeasuredEnergy)) / (((LineCycle1 / TestLineCycle) * firstMeasuredEnergy) - secondMeasuredEnergy);
//                    double offsetB = ((PhaseBCorrespondingWATTHRregValueForFirst * secondMeasuredEnergy) - (TestPhaseBCorrespondingWATTHRregValue * firstMeasuredEnergy)) / (((LineCycle1 / TestLineCycle) * firstMeasuredEnergy) - secondMeasuredEnergy);
//                    double offsetC = ((PhaseCCorrespondingWATTHRregValueForFirst * secondMeasuredEnergy) - (TestPhaseCCorrespondingWATTHRregValue * firstMeasuredEnergy)) / (((LineCycle1 / TestLineCycle) * firstMeasuredEnergy) - secondMeasuredEnergy);

//                    double xWATTOSA = ((offsetA * 4) * (2 * Math.Pow(2, 29))) / (100 * Math.Pow(10, 7));
//                    double xWATTOSB = ((offsetB * 4) * (2 * Math.Pow(2, 29))) / (100 * Math.Pow(10, 7));
//                    double xWATTOSC = ((offsetC * 4) * (2 * Math.Pow(2, 29))) / (100 * Math.Pow(10, 7));


//                    energylistWriteFile temp = new energylistWriteFile();
//                    temp.deviceId = EnergycaliDataObjectlist[i].DeviceId;
//                    temp.PhaseACorrespondingWATTHRregValueForFirst = PhaseACorrespondingWATTHRregValueForFirst.ToString();
//                    temp.PhaseBCorrespondingWATTHRregValueForFirst = PhaseBCorrespondingWATTHRregValueForFirst.ToString();
//                    temp.PhaseCCorrespondingWATTHRregValueForFirst = PhaseCCorrespondingWATTHRregValueForFirst.ToString();
//                    temp.TestPhaseACorrespondingWATTHRregValue = TestPhaseACorrespondingWATTHRregValue.ToString();
//                    temp.TestPhaseBCorrespondingWATTHRregValue = TestPhaseBCorrespondingWATTHRregValue.ToString();
//                    temp.TestPhaseCCorrespondingWATTHRregValue = TestPhaseCCorrespondingWATTHRregValue.ToString();
//                    temp.LineCycle1 = LineCycle1.ToString();
//                    temp.TestLineCycle = TestLineCycle.ToString();
//                    temp.energfrimWt16001st = firstMeasuredEnergy.ToString();
//                    temp.energfrimWt16002nd = secondMeasuredEnergy.ToString();
//                    temp.OffsetA = offsetA.ToString();
//                    temp.OffsetB = offsetB.ToString();
//                    temp.OffsetC = offsetC.ToString();
//                    temp.xWATTOSA = xWATTOSA.ToString();
//                    temp.xWATTOSB = xWATTOSB.ToString();
//                    temp.xWATTOSC = xWATTOSC.ToString();

//                    fileWriterForActiveEnergy(temp);

//                    SerialPort port = new SerialPort();
//                    port.PortName = EnergycaliDataObjectlist[i].port;
//                    port.BaudRate = 115200;
//                    port.Parity = Parity.None;
//                    port.StopBits = StopBits.One;
//                    port.RtsEnable = true;

//                    double kValueForActiveE = PhaseACorrespondingWATTHRregValueForFirst / firstMeasuredEnergy;
//                    //write to the port    
                   
//                }
//                catch { continue; }
//            }

//            }

//        internal void ReactiveEneryCalibarionCalculations(List<ReactiveEnergyExecution.TestReactiveEnergylistfromgatherdData> TestEnergyDataObjects, List<ReactiveEnergyExecution.ReactiveEnergylistfromgatherdData> ReactiveEnergyDataObjects)
//        {
//            List<ReactiveEnergyExecution.ReactiveEnergylistfromgatherdData> EnergycaliDataObjectlist = ReactiveEnergyDataObjects;
//            List<ReactiveEnergyExecution.TestReactiveEnergylistfromgatherdData> testEnergylistfromgatherdData = TestEnergyDataObjects;
//            for (int i = 0; i < EnergycaliDataObjectlist.Count; i++)
//            {
//                try
//                {
//                    // double Fullscalecurrentdiv500 ;
//                    double PhaseACorrespondingWATTHRregValueForFirst = double.Parse(EnergycaliDataObjectlist[i].phaseAenergy);
//                    double PhaseBCorrespondingWATTHRregValueForFirst = double.Parse(EnergycaliDataObjectlist[i].phaseBenergy);
//                    double PhaseCCorrespondingWATTHRregValueForFirst = double.Parse(EnergycaliDataObjectlist[i].phaseCenergy);
//                    double LineCycle1 = double.Parse(EnergycaliDataObjectlist[i].lineCircle);
//                    double firstMeasuredEnergy = double.Parse(EnergycaliDataObjectlist[i].Wt1600MeasuredReactiveEnergy);
//                    //double testCurrent = double.Parse(EnergycaliDataObjectlist[i].t);
//                    double TestPhaseACorrespondingWATTHRregValue = double.Parse(TestEnergyDataObjects[i].TestphaseAenergy);
//                    double TestPhaseBCorrespondingWATTHRregValue = double.Parse(TestEnergyDataObjects[i].TestphaseBenergy);
//                    double TestPhaseCCorrespondingWATTHRregValue = double.Parse(TestEnergyDataObjects[i].TestphaseCenergy);
//                    double TestLineCycle = double.Parse(testEnergylistfromgatherdData[i].TestlineCircle);
//                    double secondMeasuredEnergy = double.Parse(testEnergylistfromgatherdData[i].TestWt1600MeasuredEnergy);
//                    //  double offset = ((CorrespondingWATTHRregValue * testCurrent) - (((TestCorrespondingWATTHRregValue * LineCycle1) / TestLineCycle) * Fullscalecurrentdiv500)) / (Fullscalecurrentdiv500 - testCurrent);
//                    double offsetA = ((PhaseACorrespondingWATTHRregValueForFirst * secondMeasuredEnergy) - (TestPhaseACorrespondingWATTHRregValue * firstMeasuredEnergy)) / (((LineCycle1 / TestLineCycle) * firstMeasuredEnergy) - secondMeasuredEnergy);
//                    double offsetB = ((PhaseBCorrespondingWATTHRregValueForFirst * secondMeasuredEnergy) - (TestPhaseBCorrespondingWATTHRregValue * firstMeasuredEnergy)) / (((LineCycle1 / TestLineCycle) * firstMeasuredEnergy) - secondMeasuredEnergy);
//                    double offsetC = ((PhaseCCorrespondingWATTHRregValueForFirst * secondMeasuredEnergy) - (TestPhaseBCorrespondingWATTHRregValue * firstMeasuredEnergy)) / (((LineCycle1 / TestLineCycle) * firstMeasuredEnergy) - secondMeasuredEnergy);

//                    double xWATTOSA = ((offsetA * 4) * (2 * Math.Pow(2, 29))) / (100 * Math.Pow(10, 7));
//                    double xWATTOSB = ((offsetB * 4) * (2 * Math.Pow(2, 29))) / (100 * Math.Pow(10, 7));
//                    double xWATTOSC = ((offsetC * 4) * (2 * Math.Pow(2, 29))) / (100 * Math.Pow(10, 7));

//                    ReactiveEnergylistWriteFile temp = new ReactiveEnergylistWriteFile();
//                    temp.deviceId = EnergycaliDataObjectlist[i].DeviceId;
//                    temp.PhaseACorrespondingWATTHRregValueForFirst = PhaseACorrespondingWATTHRregValueForFirst.ToString();
//                    temp.PhaseBCorrespondingWATTHRregValueForFirst = PhaseBCorrespondingWATTHRregValueForFirst.ToString();
//                    temp.PhaseCCorrespondingWATTHRregValueForFirst = PhaseCCorrespondingWATTHRregValueForFirst.ToString();
//                    temp.TestPhaseACorrespondingWATTHRregValue = TestPhaseACorrespondingWATTHRregValue.ToString();
//                    temp.TestPhaseBCorrespondingWATTHRregValue = TestPhaseBCorrespondingWATTHRregValue.ToString();
//                    temp.TestPhaseCCorrespondingWATTHRregValue = TestPhaseCCorrespondingWATTHRregValue.ToString();
//                    temp.LineCycle1 = LineCycle1.ToString();
//                    temp.TestLineCycle = TestLineCycle.ToString();
//                    temp.energfrimWt16001st = firstMeasuredEnergy.ToString();
//                    temp.energfrimWt16002nd = secondMeasuredEnergy.ToString();
//                    temp.OffsetA  = offsetA.ToString();
//                    temp.OffsetB  = offsetB.ToString();
//                    temp.OffsetC  = offsetC.ToString();
//                    temp.xWATTOSA = xWATTOSA.ToString();
//                    temp.xWATTOSB = xWATTOSB.ToString();
//                    temp.xWATTOSC = xWATTOSC.ToString();
//                    fileWriterForAReactiveEnergy(temp);

//                    SerialPort port= new SerialPort();
//                    port.PortName  = EnergycaliDataObjectlist[i].port;
//                    port.BaudRate  = 115200;
//                    port.Parity    = Parity.None;
//                    port.StopBits  = StopBits.One;
//                    port.RtsEnable = true;


//                    double kValueForReactiveE = PhaseACorrespondingWATTHRregValueForFirst / firstMeasuredEnergy;
                   
//                }
//                catch { continue; }
//            }
//        }
    
//        private void filewriter(currentlistTowriteFile temp)
//        {
//            try
//            {
//                string filePath = "F:\\calibrationFiles\\" + temp.deviceID + ".csv";
//                string tempOffsetfilePath="F:\\calibrationFiles\\temporyOffsetFiles\\" + temp.deviceID + ".csv";
//                if (!File.Exists(filePath))
//                {
//                    File.Create(filePath).Close();
//                }

//                //if (!File.Exists(tempOffsetfilePath))
//                //{
//                //    File.Create(tempOffsetfilePath).Close();
//                //}

//                string delimiter = ",";
//                string[][] output = new string[][]{
//            new string[]{temp.deviceID,temp.port,"phaseA","PhaseB","PhaseC"+"\n\n","Current calibration\n\n","fullscaleCurrent/500",temp.fulscaleCurrentdiv500.ToString(),temp.fulscaleCurrentdiv500.ToString(),temp.fulscaleCurrentdiv500.ToString()+"\n","Corresponding IRMS reg value",temp.PhaseARegValuForFcurrentDiv50.ToString(),temp.PhaseBRegValuForFcurrentDiv50.ToString(),temp.PhaseCRegValuForFcurrentDiv50.ToString()+"\n","test current",temp.testCurrent.ToString(),temp.testCurrent.ToString(),temp.testCurrent.ToString()+"\n"+" ","Corresponding IRMS reg Value",temp.RegvaluePhaseAforTestCurrent.ToString(),temp.RegvaluePhaseBforTestCurrent.ToString(),temp.RegvaluePhaseBforTestCurrent.ToString()+"\n","IRMSOS",(temp.IRMSOSPhaseA).ToString(),(temp.IRMSOSPhaseB).ToString(),(temp.IRMSOSPhaseC).ToString()+"\n"} /*add the values that you want inside a csv file. Mostly this function can be used in a foreach loop.*/
//            };
//                int length = output.GetLength(0);
//                StringBuilder sb = new StringBuilder();
//                for (int index = 0; index < length; index++)
//                    sb.AppendLine(string.Join(delimiter, output[index]));
//                File.AppendAllText(filePath, sb.ToString());

//                //write to the tempory file
//                //string delimiter2 = ",";
//                //string[][] output2 = new string[][]{
//                //new string[]{temp.deviceID+"\n","IRMSOS",(temp.IRMSOSPhaseA).ToString(),(temp.IRMSOSPhaseB).ToString(),(temp.IRMSOSPhaseC).ToString()+"\n"} /*add the values that you want inside a csv file. Mostly this function can be used in a foreach loop.*/
//                //};
//                //int length2 = output2.GetLength(0);
//                //StringBuilder sb2 = new StringBuilder();
//                //for (int index = 0; index < length; index++)
//                //    sb2.AppendLine(string.Join(delimiter2, output2[index]));
//                //File.AppendAllText(tempOffsetfilePath, sb2.ToString());
                




//            }
//            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
//        }

//        public void filewriterforVoltage(VoltagelistTowriteFile temp)
//        {
//            try
//            {
//                string filePath ="F:\\calibrationFiles\\" + temp.deviceID + ".csv";
//                string tempOffsetfilePath = "F:\\calibrationFiles\\temporyOffsetFiles\\" + temp.deviceID + ".csv";
               
//                if (!File.Exists(filePath))
//                {
//                    File.Create(filePath).Close();
//                }
//                //if (!File.Exists(tempOffsetfilePath))
//                //{
//                //    File.Create(tempOffsetfilePath).Close();
//                //}
              
//                string delimiter = ",";
//                string[][] output = new string[][]{
//                new string[]{" ","Voltage Calibration"+"\n\n","Fullscale Voltage / 20",temp.fulscaleVoltageDiv20.ToString(),temp.fulscaleVoltageDiv20.ToString(),temp.fulscaleVoltageDiv20.ToString()+"\n","Corresponding VRMS reg Value",temp.PhaseARegValuForFVoltageDiv20.ToString(),temp.PhaseBRegValuForFVoltageDiv20.ToString(),temp.PhaseCRegValuForFVoltageDiv20.ToString()+"\n","test Voltage",temp.testVoltage.ToString(),temp.testVoltage.ToString(),temp.testVoltage.ToString()+"\n","Corresponding VRMS reg Value",temp.testRegvalForVoltagePhaseA.ToString(),temp.testRegvalForVoltagePhaseB.ToString(),temp.testRegvalForVoltagePhaseC.ToString()+"\n","VRMSOS",(temp.PhaseAVRMSOS).ToString(),(temp.PhaseBVRMSOS).ToString(),(temp.PhaseCVRMSOS).ToString()+"\n"} /*add the values that you want inside a csv file. Mostly this function can be used in a foreach loop.*/
//            };
//                int length = output.GetLength(0);
//                StringBuilder sb = new StringBuilder();
//                for (int index = 0; index < length; index++)
//                    sb.AppendLine(string.Join(delimiter, output[index]));
//                File.AppendAllText(filePath, sb.ToString());

//                //string[][] output2 = new string[][]{
//                //new string[]{temp.deviceID+"\n","VRMSOS",(temp.PhaseAVRMSOS).ToString(),(temp.PhaseBVRMSOS).ToString(),(temp.PhaseCVRMSOS).ToString()+"\n"} /*add the values that you want inside a csv file. Mostly this function can be used in a foreach loop.*/
//                //};
//                //int length2 = output2.GetLength(0);
//                //StringBuilder sb2 = new StringBuilder();
//                //for (int index = 0; index < length; index++)
//                //    sb2.AppendLine(string.Join(delimiter, output2[index]));
//                //File.AppendAllText(tempOffsetfilePath, sb2.ToString());


//            }
//            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
//            // File.


//        }

//        public void filewriterforPhase(PhaselistTowriteFile temp)
//        {
//            try
//            {
//                string filePath = "F:\\calibrationFiles\\" + temp.deviceID + ".csv";
//                if (!File.Exists(filePath))
//                {
//                    File.Create(filePath).Close();
//                }
//                string delimiter = ",";
//                string[][] output = new string[][]{
//            new string[]{" ","Phase Calibration"+"\n\n","Power at pf 1 at Itest and Vnom",temp.Power_at_pf_1_at_Itest_and_Vnom_phaseA.ToString(),temp.Power_at_pf_1_at_Itest_and_Vnom_phaseB.ToString(),temp.Power_at_pf_1_at_Itest_and_Vnom_phaseC.ToString()+"\n","Power at pf x at Itest and Vnom",temp.Power_at_pf_x_at_Itest_and_Vnom_phaseA.ToString(),temp.Power_at_pf_x_at_Itest_and_Vnom_phaseB.ToString(),temp.Power_at_pf_x_at_Itest_and_Vnom_phaseC.ToString()+"\n","Used Power factor",temp.Used_Power_factor.ToString(),temp.Used_Power_factor.ToString(),temp.Used_Power_factor.ToString()+"\n","Error",temp.ErrorA.ToString(),temp.ErrorB.ToString(),temp.ErrorC.ToString()+"\n","Phase Error",temp.Phase_ErrorA.ToString(),temp.Phase_ErrorB.ToString(),temp.Phase_ErrorC.ToString()+"\n","xPHCAL",(temp.xPHCALA).ToString(),(temp.xPHCALB).ToString(),(temp.xPHCALC).ToString()} /*add the values that you want inside a csv file. Mostly this function can be used in a foreach loop.*/
//            };
//                int length = output.GetLength(0);
//                StringBuilder sb = new StringBuilder();
//                for (int index = 0; index < length; index++)
//                    sb.AppendLine(string.Join(delimiter, output[index]));
//                File.AppendAllText(filePath, sb.ToString());
//                // File.
//            }
//            catch (Exception ex) { MessageBox.Show(ex.ToString()); }


//        }

//        public void fileWriterForActiveEnergy(energylistWriteFile temp) {
//            try
//            {
//                string filePath = "F:\\calibrationFiles\\" + temp.deviceId + ".csv";
//                if (!File.Exists(filePath))
//                {
//                    File.Create(filePath).Close();
//                }
//                string delimiter = ",";
//                string[][] output = new string[][]{
//            new string[]{"\n","Active Energy"+"\n\n","Wt1600MeasureFirstEnergy",temp.energfrimWt16001st.ToString(),temp.energfrimWt16001st.ToString(),temp.energfrimWt16001st.ToString()+"\n","Wt1600MeasureSecondEnergy",temp.energfrimWt16002nd.ToString(),temp.energfrimWt16002nd.ToString(),temp.energfrimWt16002nd.ToString()+"\n","Corresponding reg value for 1st",temp.PhaseACorrespondingWATTHRregValueForFirst.ToString(),temp.PhaseBCorrespondingWATTHRregValueForFirst.ToString(),temp.PhaseCCorrespondingWATTHRregValueForFirst.ToString()+"\n","Line Cycles",temp.LineCycle1.ToString(),temp.LineCycle1.ToString(),temp.LineCycle1.ToString()+"\n","Reg values For second intergration",temp.TestPhaseACorrespondingWATTHRregValue.ToString(),temp.TestPhaseBCorrespondingWATTHRregValue.ToString(),temp.TestPhaseCCorrespondingWATTHRregValue.ToString()+"\n","Line cycle for 2nd",temp.TestLineCycle.ToString(),temp.TestLineCycle.ToString(),temp.TestLineCycle.ToString()+"\n","Offset",temp.OffsetA.ToString(),temp.OffsetB.ToString(),temp.OffsetC.ToString()+"\n","xWATTOS",temp.xWATTOSA.ToString(),temp.xWATTOSB.ToString(),temp.xWATTOSC.ToString()} /*add the values that you want inside a csv file. Mostly this function can be used in a foreach loop.*/
//                };
//                int length = output.GetLength(0);
//                StringBuilder sb = new StringBuilder();
//                for (int index = 0; index < length; index++)
//                    sb.AppendLine(string.Join(delimiter, output[index]));
//                File.AppendAllText(filePath, sb.ToString());
//                // File.
//            }
//            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
//        }

//        private void fileWriterForAReactiveEnergy(ReactiveEnergylistWriteFile temp)
//        {
//            try
//            {
//                string filePath = "F:\\calibrationFiles\\" + temp.deviceId + ".csv";
//                if (!File.Exists(filePath))
//                {
//                    File.Create(filePath).Close();
//                }
//                string delimiter = ",";
//                string[][] output = new string[][]{
//            new string[]{"\n","Reactive Energy"+"\n\n","Wt1600MeasureFirstEnergy",temp.energfrimWt16001st.ToString(),temp.energfrimWt16001st.ToString(),temp.energfrimWt16001st.ToString()+"\n","Wt1600MeasureSecondEnergy",temp.energfrimWt16002nd.ToString(),temp.energfrimWt16002nd.ToString(),temp.energfrimWt16002nd.ToString()+"\n","Corresponding reg value for 1st",temp.PhaseACorrespondingWATTHRregValueForFirst.ToString(),temp.PhaseBCorrespondingWATTHRregValueForFirst.ToString(),temp.PhaseCCorrespondingWATTHRregValueForFirst.ToString()+"\n","Line Cycles",temp.LineCycle1.ToString(),temp.LineCycle1.ToString(),temp.LineCycle1.ToString()+"\n","Reg values For second intergration",temp.TestPhaseACorrespondingWATTHRregValue.ToString(),temp.TestPhaseBCorrespondingWATTHRregValue.ToString(),temp.TestPhaseCCorrespondingWATTHRregValue.ToString()+"\n","Line cycle for 2nd",temp.TestLineCycle.ToString(),temp.TestLineCycle.ToString(),temp.TestLineCycle.ToString()+"\n","Offset",temp.OffsetA.ToString(),temp.OffsetB.ToString(),temp.OffsetC.ToString()+"\n","xWATTOS",temp.xWATTOSA.ToString(),temp.xWATTOSB.ToString(),temp.xWATTOSC.ToString()} /*add the values that you want inside a csv file. Mostly this function can be used in a foreach loop.*/
//            };
//                int length = output.GetLength(0);
//                StringBuilder sb = new StringBuilder();
//                for (int index = 0; index < length; index++)
//                    sb.AppendLine(string.Join(delimiter, output[index]));
//                File.AppendAllText(filePath, sb.ToString());
//                // File.
//            }
//            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
//        }


//        public class currentlistTowriteFile
//        {
//            public decimal fulscaleCurrentdiv500;
//            public decimal PhaseARegValuForFcurrentDiv50;
//            public decimal PhaseBRegValuForFcurrentDiv50;
//            public decimal PhaseCRegValuForFcurrentDiv50;
//            public decimal testCurrent;
//            public decimal RegvaluePhaseAforTestCurrent;
//            public decimal RegvaluePhaseBforTestCurrent;
//            public decimal RegvaluePhaseCforTestCurrent;
//            public decimal IRMSOSPhaseA;
//            public decimal IRMSOSPhaseB;
//            public decimal IRMSOSPhaseC;
//            public string  deviceID;
//            public string port;
               
//        }

//        public class VoltagelistTowriteFile
//        {
//            public string deviceID;

//            public decimal fulscaleVoltageDiv20;
//            public decimal PhaseARegValuForFVoltageDiv20;
//            public decimal PhaseBRegValuForFVoltageDiv20;
//            public decimal PhaseCRegValuForFVoltageDiv20;

//            public decimal testVoltage;
//            public decimal testRegvalForVoltagePhaseA;
//            public decimal testRegvalForVoltagePhaseB;
//            public decimal testRegvalForVoltagePhaseC;

//            public decimal PhaseAVRMSOS;
//            public decimal PhaseBVRMSOS;
//            public decimal PhaseCVRMSOS;

//        }

//        public class PhaselistTowriteFile 
//        {
//        public decimal Power_at_pf_1_at_Itest_and_Vnom_phaseA;
//        public decimal Power_at_pf_1_at_Itest_and_Vnom_phaseB;
//        public decimal Power_at_pf_1_at_Itest_and_Vnom_phaseC;

//        public decimal Power_at_pf_x_at_Itest_and_Vnom_phaseA;
//        public decimal Power_at_pf_x_at_Itest_and_Vnom_phaseB;
//        public decimal Power_at_pf_x_at_Itest_and_Vnom_phaseC;
//        public double Used_Power_factor;

//        public double ErrorA, ErrorB, ErrorC;
//        public decimal Phase_ErrorA, Phase_ErrorB, Phase_ErrorC;
//        public decimal xPHCALA, xPHCALB, xPHCALC;
//        public string deviceID;

//        }

//        public class energylistWriteFile
//        {
//            public string deviceId;
//            public string PhaseACorrespondingWATTHRregValueForFirst;
//            public string PhaseBCorrespondingWATTHRregValueForFirst;
//            public string PhaseCCorrespondingWATTHRregValueForFirst;
//            public string energfrimWt16001st;
//            public string LineCycle1;
//            public string OffsetA;
//            public string OffsetB;
//            public string OffsetC;
//            public string xWATTOSA;
//            public string xWATTOSB;
//            public string xWATTOSC;
          
//            public string TestPhaseACorrespondingWATTHRregValue;
//            public string TestPhaseBCorrespondingWATTHRregValue;
//            public string TestPhaseCCorrespondingWATTHRregValue;
//            public string energfrimWt16002nd;
//            public string TestLineCycle;
              
//        }

//        public class ReactiveEnergylistWriteFile
//        {
//            public string deviceId;
//            public string PhaseACorrespondingWATTHRregValueForFirst;
//            public string PhaseBCorrespondingWATTHRregValueForFirst;
//            public string PhaseCCorrespondingWATTHRregValueForFirst;
//            public string energfrimWt16001st;
//            public string LineCycle1;

//            public string TestPhaseACorrespondingWATTHRregValue;
//            public string TestPhaseBCorrespondingWATTHRregValue;
//            public string TestPhaseCCorrespondingWATTHRregValue;
//            public string energfrimWt16002nd;
//            public string TestLineCycle;

//            public string OffsetA;
//            public string OffsetB;
//            public string OffsetC;
//            public string xWATTOSA;
//            public string xWATTOSB;
//            public string xWATTOSC;

//        }







       
//    }
//}

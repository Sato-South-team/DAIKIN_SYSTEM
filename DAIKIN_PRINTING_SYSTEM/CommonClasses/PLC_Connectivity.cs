using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAIKIN_PRINTING_SYSTEM.CommonClasses
{
    class PLC_Connectivity
    {
        EasyModbus.ModbusClient Client = null;
        public delegate void DataArrivalHandler(string Message, string Client);
        public delegate void ScannerStatusChangeHandler(bool flag);
        public event DataArrivalHandler OnDataArrived;
        public event ScannerStatusChangeHandler OnScannerStatusChanged;
        bool Flag = false;
        bool Ping = false;
        bool dataRead = false;
        public string IP; public int port; public int PLCAddress;

        bool strData = false;

        public virtual void ScannerDataArrived(string Data, string Client)
        {
            if (!ReferenceEquals(this.OnDataArrived, null))
            {
                this.OnDataArrived(Data, Client);
            }
        }
        public virtual void ScannerStatusChanged(bool Data)
        {
            if (!ReferenceEquals(this.OnScannerStatusChanged, null))
            {
                this.OnScannerStatusChanged(Data);
            }
        }
        private void Reconnect()
        {
            try
            {
                if (Client == null)
                    Client = new EasyModbus.ModbusClient();

                Client.Connect(IP, port);
                Flag = true;
                Thread.Sleep(15000);
                ScannerStatusChanged(true);
                if(dataRead==false)
                Read();
            }
            catch (Exception ex)
            {
                Dispose();
                Reconnect();
            }

        }
        public void ReadData()
        {
           int[] a= Client.ReadHoldingRegisters(4, 1);
        }

        public bool Connect()
        {
            try
            {
                if (Client == null)
                    Client = new EasyModbus.ModbusClient();
                if (Client.Connected == false)
                {
                    //
                    //if(strData)
                    //{
                    CheckPinging();
                    Client.Connect(IP, port);
                    Flag = true;
                    ScannerStatusChanged(true);
                    Read();
                       
                    // }


                }
                else
                    Flag = true;

            }
            catch (Exception ex)
            {
                Flag = false;
                StreamWriter str = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\" + "PLC-" + System.DateTime.Now.ToString("dd-MMM-YYYY"), true);
                str.WriteLine(ex.Message.ToString() + " " + System.DateTime.Now);
                str.Close();
                //  Dispose();
                //Connect();
            }
            return Flag;
        }

        public bool CheckPinging()
        {
            try
            {
                Thread th = new Thread(new ThreadStart(delegate
                {
                Thread.Sleep(4000);
                while (true)
                {
                        try
                        {
                            Ping ping = new Ping();
                            PingReply reply = ping.Send(IP);
                            strData = reply.Status == IPStatus.Success;
                            if (strData == false)
                            {
                                Ping = true;
                                ScannerStatusChanged(false);
                            }
                            if (strData == true)
                            {
                                if (Ping == true)
                                {
                                    Dispose();
                                    Reconnect();
                                    Ping = false;
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            strData = false;
                            ScannerStatusChanged(false);
                        }
                    }
                   

                }));
                th.Start();
            }
            catch (Exception ex)
            {
                strData = false;
                ScannerStatusChanged(false);
                //  Client.Disconnect();
                // Connect();
            }
            return strData;

        }


        public void Read()
        {
            //  string strData = "";
            StringBuilder sr = new StringBuilder();
            bool Data = false;
            dataRead = true;
            bool[] Result1 = { false, false, false, false, false, false, false, false, false, false, false, false, false };
            try
            {
                Thread th = new Thread(new ThreadStart(delegate
                {
                    while (Flag)
                    {
                        try
                        {
                            Thread.Sleep(50);
                         //   bool[] Result = Client.ReadCoils(PLCAddress, 10);
                         ////   int[] Result = Client.ReadHoldingRegisters(1, 1);
                         //   for (int i = 0; i < Result.Length; i++)
                         //   {
                         //       if (Result[i] == true)
                         //       {

                         //           // ScannerStatusChanged(true, IP);
                         //           Result1[i] = true;
                         //         //  Client.WriteSingleRegister(1, 0);
                         //           ScannerDataArrived("1", i.ToString());
                         //           //Thread.Sleep(2000);
                         //           // break;
                         //       }
                                //if (Result[i] == false && Result1[i] == true)
                                //{

                                //    // ScannerStatusChanged(true, IP);
                                //    Result1[i] = false;
                                //    //break;
                                //    //ScannerDataArrived(Client.IPAddress, i.ToString());
                                //    //Thread.Sleep(2000);
                                //    //break;
                                //}
                        //    }

                            //if (Result[0] == true)
                            //{
                            //    ScannerDataArrived("1", IP);
                            //    Thread.Sleep(600);
                            //}

                        }
                        catch (Exception ex)
                        {
                            // Dispose();
                            //Connect();
                            //StreamWriter str = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\" + "PLC-" + System.DateTime.Now.ToString("dd-MMM-yyyy"), true);
                            //str.WriteLine(ex.Message.ToString() + " " + System.DateTime.Now);
                            //str.Close();                            //throw ex;
                        }

                    }
                }));
                th.Start();
            }
            catch (Exception ex)
            {
                // Dispose();
                //Connect();
                StreamWriter str = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\" + "PLC-" + System.DateTime.Now.ToString("dd-MMM-YYYY"), true);
                str.WriteLine(ex.Message.ToString() + " " + System.DateTime.Now);
                str.Close();
            }
            //return strData;
        }
        public void Write(int Value, int Address)
        {
            try
            {
                Client.WriteSingleRegister(Address, Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Dispose()
        {
            try
            {
                if (Client != null)
                {
                    if (Client.Connected)
                    {
                        Client.Disconnect();
                    }
                }
                ScannerStatusChanged(false);
                Client = null;
            }
            catch (Exception ex)
            {
                StreamWriter str = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\" + "PLC-" + System.DateTime.Now.ToString("dd-MMM-YYYY"), true);
                str.WriteLine(ex.Message.ToString() + " " + System.DateTime.Now);
                str.Close();
            }
        }
    }
}

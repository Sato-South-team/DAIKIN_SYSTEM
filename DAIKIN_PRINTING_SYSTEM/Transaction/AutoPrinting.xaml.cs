using DAIKIN_PRINTING_SYSTEM.CommonClasses;
using DAIKIN_PRINTING_SYSTEM.StartUp;
using DATA_LAYER.DatabaseConnectivity;
using Sato_Network_Client_DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DAIKIN_PRINTING_SYSTEM.Transaction
{
    /// <summary>
    /// Interaction logic for LabelPrinting.xaml
    /// </summary>
    public partial class AutoPrinting : Page
    {
        public AutoPrinting()
        {
            InitializeComponent();
        }
        #region Variable and Objects

        private NetworkClient obj_Client = new NetworkClient();
        private NetworkClient obj_Client1 = new NetworkClient();
        private NetworkClient obj_Client2 = new NetworkClient();
        private NetworkClient obj_Client3 = new NetworkClient();
        private PLC_Connectivity obj_Client4 = new PLC_Connectivity();

        BUSINESS_LAYER.Business_Layer.Business_Layer obj_BL = new BUSINESS_LAYER.Business_Layer.Business_Layer();
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer dispatcherTimer1 = new System.Windows.Threading.DispatcherTimer();
        int Scanner1Address = 0,Scanner2Address = 0, Value = 0;
        #endregion


        private void Start()
        {
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 15);
            dispatcherTimer.Start();

            
        }
        private void Start1()
        {
            dispatcherTimer1.Tick += new EventHandler(dispatcherTimer1_Tick);
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer1.Start();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (txtResult.Text != "WAITING")
                {
                    txtResult.Text = "WAITING";
                    txtResult.Foreground = Brushes.SteelBlue;
                }
                if (txtResult1.Text != "WAITING")
                {
                    txtResult1.Text = "WAITING";
                    txtResult1.Foreground = Brushes.SteelBlue;
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LABEL_PRINTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void dispatcherTimer1_Tick(object sender, EventArgs e)
        {
            try
            {

                if(txtStation1Printer.Text != "ONLINE")
                {
                    if (txtStation1Printer.IsVisible == true)
                        txtStation1Printer.Visibility = Visibility.Hidden;
                    else
                        txtStation1Printer.Visibility = Visibility.Visible;

                }
                else
                    txtStation1Printer.Visibility = Visibility.Visible;


                if (txtStation1Scaner.Text != "CONNECTED")
                {
                    if (txtStation1Scaner.IsVisible == true)
                        txtStation1Scaner.Visibility = Visibility.Hidden;
                    else
                        txtStation1Scaner.Visibility = Visibility.Visible;

                }
                else
                    txtStation1Scaner.Visibility = Visibility.Visible;

                if (txtStation2Printer.Text != "ONLINE")
                {
                    if (txtStation2Printer.IsVisible == true)
                        txtStation2Printer.Visibility = Visibility.Hidden;
                    else
                        txtStation2Printer.Visibility = Visibility.Visible;

                }
                else
                    txtStation2Printer.Visibility = Visibility.Visible;

                if (txtStation2Scaner.Text != "CONNECTED")
                {
                    if (txtStation2Scaner.IsVisible == true)
                        txtStation2Scaner.Visibility = Visibility.Hidden;
                    else
                        txtStation2Scaner.Visibility = Visibility.Visible;
                }
                else
                    txtStation2Scaner.Visibility = Visibility.Visible;

                if (txtResult.Text != "SUCCESSFULL" && txtResult.Text != "WAITING")
                {
                    if (txtResult.IsVisible == true)
                        txtResult.Visibility = Visibility.Hidden;
                    else
                        txtResult.Visibility = Visibility.Visible;
                }
                else
                    txtResult.Visibility = Visibility.Visible;

                if (txtResult1.Text != "SUCCESSFULL" && txtResult1.Text != "WAITING")
                {
                    if (txtResult1.IsVisible == true)
                        txtResult1.Visibility = Visibility.Hidden;
                    else
                        txtResult1.Visibility = Visibility.Visible;
                }
                else
                    txtResult1.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LABEL_PRINTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        #region Station_1_Scanner
        private void Station_1_Scanner_OnStatusChanged(bool Flag) => this.Station_1_Scanner_StatusChange(Flag);

        private void Station_1_Scanner_StatusChange(bool Flag)
        {
            try
            {

                this.Dispatcher.Invoke((Action)(() =>
                {
                    if (Flag)
                    {
                        this.txtStation1Scaner.Text = "CONNECTED";
                        this.txtStation1Scaner.Foreground = Brushes.Lime;
                    }
                    else
                    {
                        this.txtStation1Scaner.Text = "NOT CONNECTED";
                        this.txtStation1Scaner.Foreground = Brushes.Red;
                    }
                }));

            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    this.txtResult.Text = ex.Message.ToString() + " - STATION-1 SCANNER - STATUS CHANGE";
                    this.txtResult.Foreground = Brushes.Red;
                }));

            }
        }


        private void Station_1_Scanner_ONDataReacive(string Barcode, string ClientIP) => this.Station_1_ScannerDataReacive(Barcode, ClientIP);

        private async void Station_1_ScannerDataReacive(string Barcode, string Client)
        {
            Start();
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    dispatcherTimer.Stop();

                    if (!Barcode.Contains("ERROR"))
                    {
                        string ModelNo = Barcode.Substring(1, 3);
                        if (txtStation1Printer.Text == "ONLINE")
                        {
                            DatabaseConnections databaseConnections = new DatabaseConnections();

                            DataSet ds = databaseConnections.ExecuteDataSetParam("Proc_Transaction", Barcode, cmbLine.Text, ModelNo, txtStation1.Text, CommonClasses.CommonVariable.PlantCode, "Save", "Printing", ENTITY_LAYER.Entity_Layer.Entity_Layer.LoginID,"Auto", CommonClasses.CommonVariable.ItemType, "", "","");

                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows[0][0].ToString() == "Saved")
                                {
                                    txts1ModelNo.Text = ModelNo;
                                    txts1ModelName.Text = ds.Tables[0].Rows[0]["ModelName"].ToString();
                                    txts1Pipe.Text = Barcode;
                                    txts1Serial.Text = ds.Tables[0].Rows[0]["SerialNo"].ToString();
                                    txts1Barcode.Text = ds.Tables[0].Rows[0]["Barcode"].ToString();
                                    txts1Count.Text = ds.Tables[0].Rows[0]["Count"].ToString();
                                    txts1Date.Text = System.DateTime.Now.ToString("dd MMM yyyy HH:MM:ss");

                                    string prn = ds.Tables[0].Rows[0]["PRN"].ToString();
                                    prn = prn.Replace("{MODEL_NO}", ModelNo.PadLeft(4, ' '));
                                    prn = prn.Replace("{MODEL_NAME}", ds.Tables[0].Rows[0]["ModelName"].ToString());
                                    prn = prn.Replace("{REF}", ds.Tables[0].Rows[0]["Ref"].ToString());
                                    prn = prn.Replace("{REF_OIL}", ds.Tables[0].Rows[0]["RefOil"].ToString());
                                    prn = prn.Replace("{ML}", ds.Tables[0].Rows[0]["ML"].ToString());
                                    prn = prn.Replace("{MFG_No}", ds.Tables[0].Rows[0]["MFGNo"].ToString());
                                    prn = prn.Replace("{BARCODE}", ds.Tables[0].Rows[0]["Barcode"].ToString());
                                    prn = prn.Replace("{AP}", ds.Tables[0].Rows[0]["AP"].ToString());
                                    prn = prn.Replace("{DATE}", ds.Tables[0].Rows[0]["Date"].ToString());

                                    obj_Client2.Write(prn);

                                    this.txtResult.Text = "SUCCESSFULL";
                                    this.txtResult.Foreground = Brushes.Green;

                                    txtTotCount.Text = (Convert.ToInt32(txts1Count.Text) + Convert.ToInt32(txts2Count.Text)).ToString();

                                    obj_Client4.Write(Value, Scanner1Address);

                                }
                                else
                                {
                                    this.txtResult.Text = ds.Tables[0].Rows[0][0].ToString();
                                    this.txtResult.Foreground = Brushes.Red;
                                }
                            }
                        }
                        else
                        {
                            this.txtResult.Text = "PRINTER ERROR";
                            this.txtResult.Foreground = Brushes.Red;
                        }
                    }
                    else
                    {
                        this.txtResult.Text = "BARCODE IS UNREADABLE";
                        this.txtResult.Foreground = Brushes.Red;
                    }

                   

                }));
                //Barcode = "1234";
                //    DataSet dt = databaseConnections.ExecuteDataSetParam("Proc_PLCCommunication", Client, "", "", "MainScanner", Barcode, txtIRN.Text, cmbLine.Text, cmbCategory.Text);

                //if (dt.Tables.Count > 0)
                //{

                //}

            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    this.txtResult.Text = ex.Message.ToString() + " - STATION-1 SCANNER - DATA RECEIVE";
                    this.txtResult.Foreground = Brushes.Red;
                }));
            }
        }
        #endregion

        #region Station_2_Scanner
        private void Station_2_Scanner_OnStatusChanged(bool Flag) => this.Station_2_Scanner_StatusChange(Flag);

        private void Station_2_Scanner_StatusChange(bool Flag)
        {
            try
            {

                this.Dispatcher.Invoke((Action)(() =>
                {
                    if (Flag)
                    {
                        this.txtStation2Scaner.Text = "CONNECTED";
                        this.txtStation2Scaner.Foreground = Brushes.Lime;
                    }
                    else
                    {
                        this.txtStation2Scaner.Text = "NOT CONNECTED";
                        this.txtStation2Scaner.Foreground = Brushes.Red;
                    }
                }));
               
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    this.txtResult1.Text = ex.Message.ToString() + " - STATION-2 SCANNER - STATUS CHANGE";
                    this.txtResult1.Foreground = Brushes.Red;
                }));
               
            }
        }

        private void Station_2_Scanner_ONDataReacive(string Barcode, string ClientIP) => this.Station_2_ScannerDataReacive(Barcode, ClientIP);

        private async void Station_2_ScannerDataReacive(string Barcode, string Client)
        {
            Start();

            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    dispatcherTimer.Stop();

                    if (!Barcode.Contains("ERROR"))
                    {

                        string ModelNo = Barcode.Substring(1, 3);


                        if (txtStation2Printer.Text == "ONLINE")
                        {
                            DatabaseConnections databaseConnections = new DatabaseConnections();
                            DataSet ds = databaseConnections.ExecuteDataSetParam("Proc_Transaction", Barcode, cmbLine.Text, ModelNo, txtStation2.Text, CommonClasses.CommonVariable.PlantCode, "Save", "Printing", ENTITY_LAYER.Entity_Layer.Entity_Layer.LoginID, "Auto", CommonClasses.CommonVariable.ItemType,"","", "");

                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows[0][0].ToString() == "Saved")
                                {
                                    txts2ModelNo.Text = ModelNo;
                                    txts2ModelName.Text = ds.Tables[0].Rows[0]["ModelName"].ToString();
                                    txts2Pipe.Text = Barcode;
                                    txts2Serial.Text = ds.Tables[0].Rows[0]["SerialNo"].ToString();
                                    txts2Barcode.Text = ds.Tables[0].Rows[0]["Barcode"].ToString();
                                    txts2Count.Text = ds.Tables[0].Rows[0]["Count"].ToString();
                                    txts2Date.Text = System.DateTime.Now.ToString("dd MMM yyyy HH:MM:ss");

                                    string prn = ds.Tables[0].Rows[0]["PRN"].ToString();
                                    prn = prn.Replace("{MODEL_NO}", ModelNo.PadLeft(4, ' '));
                                    prn = prn.Replace("{MODEL_NAME}", ds.Tables[0].Rows[0]["ModelName"].ToString());
                                    prn = prn.Replace("{REF}", ds.Tables[0].Rows[0]["Ref"].ToString());
                                    prn = prn.Replace("{REF_OIL}", ds.Tables[0].Rows[0]["RefOil"].ToString());
                                    prn = prn.Replace("{ML}", ds.Tables[0].Rows[0]["ML"].ToString());
                                    prn = prn.Replace("{MFG_No}", ds.Tables[0].Rows[0]["MFGNo"].ToString());
                                    prn = prn.Replace("{BARCODE}", ds.Tables[0].Rows[0]["Barcode"].ToString());
                                    prn = prn.Replace("{AP}", ds.Tables[0].Rows[0]["AP"].ToString());
                                    prn = prn.Replace("{DATE}", ds.Tables[0].Rows[0]["Date"].ToString());
                                    obj_Client3.Write(prn);
                                    this.txtResult1.Text = "SUCCESSFULL";
                                    this.txtResult1.Foreground = Brushes.Green;
                                    txtTotCount.Text = (Convert.ToInt32(txts1Count.Text) + Convert.ToInt32(txts2Count.Text)).ToString();
                                    obj_Client4.Write(Value, Scanner2Address);

                                }
                                else
                                {
                                    this.txtResult1.Text = ds.Tables[0].Rows[0][0].ToString();
                                    this.txtResult1.Foreground = Brushes.Red;
                                }
                            }
                        }
                        else
                        {
                            this.txtResult1.Text = "PRINTER ERROR";
                            this.txtResult1.Foreground = Brushes.Red;
                        }
                    }
                    else
                    {
                        this.txtResult1.Text = "BARCODE IS UNREADABLE";

                        this.txtResult1.Foreground = Brushes.Red;
                    }


                    //Start();
                }));
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    this.txtResult1.Text = ex.Message.ToString() + " - STATION-2 SCANNER - DATA RECEIVE";
                    this.txtResult1.Foreground = Brushes.Red;
                }));


            }
        }
        #endregion

        #region Station_2_Printer
        private void Station_2_Printer_OnStatusChanged(bool Flag) => this.Station_2_Printer_StatusChange(Flag);

        private void Station_2_Printer_StatusChange(bool Flag)
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    if (Flag)
                    {
                        this.txtStation2Printer.Text = "ONLINE";
                        this.txtStation2Printer.Foreground = Brushes.Lime;
                    }
                    else
                    {
                        this.txtStation2Printer.Text = "OFFLINE";
                        this.txtStation2Printer.Foreground = Brushes.Red;
                    }
                }));
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    this.txtResult1.Text = ex.Message.ToString() + " - STATION-1 PRINTER - STATUS CHANGE";
                    this.txtResult1.Foreground = Brushes.Red;
                }));
               
            }
        }
        private void Station_2_Printer_ONDataReacive(string Barcode, string ClientIP) => this.Station_2_PrinterDataReacive(Barcode, ClientIP);

        private async void Station_2_PrinterDataReacive(string Barcode, string Client)
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    if (Barcode.ToUpper().Contains("A") || Barcode.ToUpper().Contains("S") || Barcode.ToUpper().Contains("G"))
                    {
                        txtStation2Printer.Text = "ONLINE";
                        txtStation2Printer.Foreground = Brushes.Lime;
                    }
                    else if (Barcode.Contains("b"))
                    {
                        txtStation2Printer.Text = "HEAD OPEN";
                        txtStation2Printer.Foreground = Brushes.Red;
                    }
                    else if (Barcode.Contains("c"))
                    {
                        txtStation2Printer.Text = "PAPER END";
                        txtStation2Printer.Foreground = Brushes.Red;
                    }
                    else if (Barcode.Contains("d"))
                    {
                        txtStation2Printer.Text = "RIBBON END";
                        txtStation2Printer.Foreground = Brushes.Red;
                    }
                    else if (Barcode.Contains("e"))
                    {
                        txtStation2Printer.Text = "PRINTER ERROR";
                        txtStation2Printer.Foreground = Brushes.Red;
                    }
                    else if (Barcode.Contains("f"))
                    {
                        txtStation2Printer.Text = "PAPER JAM OR SENSOR ERROR";
                        txtStation2Printer.Foreground = Brushes.Red;
                    }
                    else if (Barcode.Contains("0000000"))
                    {
                        txtStation2Printer.Text = "OFFLINE";
                        txtStation2Printer.Foreground = Brushes.Red;
                    }
                    else
                    {
                        //MessageBox.Show(Barcode);
                        txtStation2Printer.Text = "MACHINE ERROR";
                        txtStation2Printer.Foreground = Brushes.Red;
                    }
                }));


            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    this.txtResult1.Text = ex.Message.ToString() + " - STATION-1 PRINTER - DATA RECEIVE";
                    this.txtResult1.Foreground = Brushes.Red;
                }));

            }
        }


        #endregion

        #region Station_1_Printer
        private void Station_1_Printer_OnStatusChanged(bool Flag) => this.Station_1_Printer_StatusChange(Flag);

        private void Station_1_Printer_StatusChange(bool Flag)
        {
            try
            {

                this.Dispatcher.Invoke((Action)(() =>
                {
                    if (Flag)
                    {
                        this.txtStation1Printer.Text = "ONLINE";
                        this.txtStation1Printer.Foreground = Brushes.Lime;
                    }
                    else
                    {
                        this.txtStation1Printer.Text = "OFFLINE";
                        this.txtStation1Printer.Foreground = Brushes.Red;
                    }
                }));

            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    this.txtResult.Text = ex.Message.ToString() + " - STATION-1 PRINTER - STATUS CHANGE";
                    this.txtResult.Foreground = Brushes.Red;
                }));
                
            }
        }


        private void Station_1_Printer_ONDataReacive(string Barcode, string ClientIP) => this.Station_1_PrinterDataReacive(Barcode, ClientIP);

        private async void Station_1_PrinterDataReacive(string Barcode, string Client)
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    if (Barcode.ToUpper().Contains("A") || Barcode.ToUpper().Contains("S") || Barcode.ToUpper().Contains("G"))
                    {
                        txtStation1Printer.Text = "ONLINE";
                        txtStation1Printer.Foreground = Brushes.Lime;
                    }
                    else if (Barcode.Contains("b"))
                    {
                        txtStation1Printer.Text = "HEAD OPEN";
                        txtStation1Printer.Foreground = Brushes.Red;
                    }
                    else if (Barcode.Contains("c"))
                    {
                        txtStation1Printer.Text = "PAPER END";
                        txtStation1Printer.Foreground = Brushes.Red;
                    }
                    else if (Barcode.Contains("d"))
                    {
                        txtStation1Printer.Text = "RIBBON END";
                        txtStation1Printer.Foreground = Brushes.Red;
                    }
                    else if (Barcode.Contains("e"))
                    {
                        txtStation1Printer.Text = "PRINTER ERROR";
                        txtStation1Printer.Foreground = Brushes.Red;
                    }
                    else if (Barcode.Contains("f"))
                    {
                        txtStation1Printer.Text = "PAPER JAM OR SENSOR ERROR";
                        txtStation1Printer.Foreground = Brushes.Red;
                    }
                    else if (Barcode.Contains("0000000"))
                    {
                        txtStation1Printer.Text = "OFFLINE";
                        txtStation1Printer.Foreground = Brushes.Red;
                    }
                    else
                    {
                        //MessageBox.Show(Barcode);
                        txtStation1Printer.Text = "MACHINE ERROR";
                        txtStation1Printer.Foreground = Brushes.Red;
                    }

                    //  obj_Client2.Write("");
                }));
              

            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    this.txtResult.Text = ex.Message.ToString() + " - STATION-1 PRINTER - DATA RECEIVE";
                    this.txtResult.Foreground = Brushes.Red;
                }));

            }
        }

        #endregion
        #region PLC
        private void PLC_OnStatusChanged(bool Flag) => this.PLC_StatusChange(Flag);

        private void PLC_StatusChange(bool Flag)
        {
            try
            {

                this.Dispatcher.Invoke((Action)(() =>
                {
                    if (Flag)
                    {
                        this.txtPLCStatus.Text = "CONNECTED";
                        this.txtPLCStatus.Foreground = Brushes.Lime;
                    }
                    else
                    {
                        this.txtPLCStatus.Text = "NOT CONNECTED";
                        this.txtPLCStatus.Foreground = Brushes.Red;
                    }
                }));

            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    this.txtResult.Text = ex.Message.ToString() + " - STATION-1 PRINTER - STATUS CHANGE";
                    this.txtResult.Foreground = Brushes.Red;
                }));

            }
        }
        #endregion
        private void Transaction(string Type)
        {

            if (Type == "GetLineName")
            {
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode = CommonClasses.CommonVariable.PlantCode;
                DataTable DT = obj_BL.BL_Transaction().Tables[0];
                CommonClasses.CommonMethods.FillComboBox(cmbLine, DT, "LineNo", "LineName");
            }
            if (Type == "GetIPDetails")
            {
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode = CommonClasses.CommonVariable.PlantCode;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.LineNo = cmbLine.Text.ToString();
                DataTable table = obj_BL.BL_Transaction().Tables[0];

                for (int index = 0; index < table.Rows.Count; ++index)
                {

                    if (table.Rows[index]["HardwareType"].ToString() == "SCANNER")
                    {
                        if (table.Rows[index]["Category"].ToString() == "ASSEMBLY 1")
                        {
                            this.obj_Client = (NetworkClient)null;
                            this.obj_Client = new NetworkClient();
                            this.obj_Client.OnDataArrived += new NetworkClient.DataArrivalHandler(Station_1_ScannerDataReacive);
                            this.obj_Client.OnScannerStatusChanged += new NetworkClient.ScannerStatusChangeHandler(Station_1_Scanner_StatusChange);
                            this.obj_Client.connection(table.Rows[index]["IPAddress"].ToString(), Convert.ToInt32(Convert.ToInt32(table.Rows[index]["PortNo"])));
                            txtStation1.Text = table.Rows[index]["Station"].ToString();
                        }
                        if (table.Rows[index]["Category"].ToString() == "ASSEMBLY 2")
                        {
                            this.obj_Client1 = (NetworkClient)null;
                            this.obj_Client1 = new NetworkClient();
                            this.obj_Client1.OnDataArrived += new NetworkClient.DataArrivalHandler(Station_2_ScannerDataReacive);
                            this.obj_Client1.OnScannerStatusChanged += new NetworkClient.ScannerStatusChangeHandler(Station_2_Scanner_StatusChange);
                            this.obj_Client1.connection(table.Rows[index]["IPAddress"].ToString(), Convert.ToInt32(Convert.ToInt32(table.Rows[index]["PortNo"])));
                            txtStation2.Text = table.Rows[index]["Station"].ToString();

                        }
                    }
                    if (table.Rows[index]["HardwareType"].ToString() == "PRINTER")
                    {
                        if (table.Rows[index]["Category"].ToString() == "ASSEMBLY 1")
                        {
                            this.obj_Client2 = (NetworkClient)null;
                            this.obj_Client2 = new NetworkClient();
                            this.obj_Client2.OnDataArrived += new NetworkClient.DataArrivalHandler(Station_1_PrinterDataReacive);

                            this.obj_Client2.OnScannerStatusChanged += new NetworkClient.ScannerStatusChangeHandler(Station_1_Printer_StatusChange);
                            this.obj_Client2.connection(table.Rows[index]["IPAddress"].ToString(), Convert.ToInt32(Convert.ToInt32(table.Rows[index]["PortNo"])));
                        }
                        if (table.Rows[index]["Category"].ToString() == "ASSEMBLY 2")
                        {
                            this.obj_Client3 = (NetworkClient)null;
                            this.obj_Client3 = new NetworkClient();
                            this.obj_Client3.OnDataArrived += new NetworkClient.DataArrivalHandler(Station_2_PrinterDataReacive);
                            this.obj_Client3.OnScannerStatusChanged += new NetworkClient.ScannerStatusChangeHandler(Station_2_Printer_StatusChange);
                            this.obj_Client3.connection(table.Rows[index]["IPAddress"].ToString(), Convert.ToInt32(Convert.ToInt32(table.Rows[index]["PortNo"])));
                        }
                    }
                    if (table.Rows[index]["HardwareType"].ToString() == "PLC")
                    {
                        this.obj_Client4 = (PLC_Connectivity)null;
                        this.obj_Client4 = new PLC_Connectivity();
                        obj_Client4.IP = table.Rows[index]["IPAddress"].ToString();
                        obj_Client4.port = Convert.ToInt32(Convert.ToInt32(table.Rows[index]["PortNo"]));
                        this.obj_Client4.OnScannerStatusChanged += new PLC_Connectivity.ScannerStatusChangeHandler(PLC_StatusChange);
                        this.obj_Client4.Connect();

                        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\PLC_Address.txt"))
                        {
                            StreamReader SR1 = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\PLC_Address.txt");
                            string Data = SR1.ReadToEnd();
                            SR1.Dispose();
                            SR1.Close();
                            string[] Data1 = Data.Replace("\n", "").Split('\r');

                            for (int i = 0; i < Data1.Length; i++)
                            {
                                if (Data1[i].Contains("Scanner1"))
                                    Scanner1Address = Convert.ToInt32(Data1[i].Split(':')[1]);
                                if (Data1[i].Contains("Scanner2"))
                                    Scanner2Address = Convert.ToInt32(Data1[i].Split(':')[1]);
                                if (Data1[i].Contains("Value"))
                                    Value = Convert.ToInt32(Data1[i].Split(':')[1]);
                            }


                        }
                    }

                }
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //MessageBox.Show("Loaded");
                Transaction("GetLineName");
                txtHeader.Text = CommonClasses.CommonVariable.ItemType+ " AUTO LABEL PRINTING";
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Line.txt"))
                {
                    StreamReader SR = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\Line.txt");
                    string Line = SR.ReadToEnd();
                    SR.Dispose();
                    SR.Close();
                    cmbLine.Text = Line;
                }
                Start1();
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LABEL_PRINTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void CmbLine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbLine.SelectedIndex > -1)
                {
                    DataRowView dr = (DataRowView)cmbLine.SelectedItem;
                    cmbLine.Text = dr[1].ToString();
                    StreamWriter SW = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Line.txt", false);
                    SW.Write(cmbLine.Text.ToString());
                    SW.Dispose();
                    SW.Close();
                    Transaction("GetIPDetails");
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LABEL_PRINTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void ImgSmily3_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                CommonVariable.PageOpenClose = "Close";
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LABEL_PRINTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (obj_Client != null)
                    obj_Client.Dispose();
                if (obj_Client1 != null)
                    obj_Client1.Dispose();
                if (obj_Client2 != null)
                    obj_Client2.Dispose();
                if (obj_Client3 != null)
                    obj_Client3.Dispose();
                if (obj_Client4 != null)
                    obj_Client4.Dispose();
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LABEL_PRINTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }
    }
}

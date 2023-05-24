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
    /// Interaction logic for ManualPrinting.xaml
    /// </summary>
    public partial class ManualPrinting : Page
    {
        public ManualPrinting()
        {
            InitializeComponent();
        }

        #region Variable and Objects
        private NetworkClient obj_Client = new NetworkClient();

        BUSINESS_LAYER.Business_Layer.Business_Layer obj_BL = new BUSINESS_LAYER.Business_Layer.Business_Layer();
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer dispatcherTimer1 = new System.Windows.Threading.DispatcherTimer();

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
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MANUAL_PRINTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
        private void dispatcherTimer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (txtStationPrinter.Text != "ONLINE")
                {
                    if (txtStationPrinter.IsVisible == true)
                        txtStationPrinter.Visibility = Visibility.Hidden;
                    else
                        txtStationPrinter.Visibility = Visibility.Visible;
                }
                else
                    txtStationPrinter.Visibility = Visibility.Visible;


                if (txtResult.Text != "SUCCESSFULL" && txtResult.Text != "WAITING")
                {
                    if (txtResult.IsVisible == true)
                        txtResult.Visibility = Visibility.Hidden;
                    else
                        txtResult.Visibility = Visibility.Visible;
                }
                else
                    txtResult.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MANUAL_PRINTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
        #region Station_Printer
        private void Station_Printer_OnStatusChanged(bool Flag) => this.Station_Printer_StatusChange(Flag);

        private void Station_Printer_StatusChange(bool Flag)
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    if (Flag)
                    {
                        this.txtStationPrinter.Text = "ONLINE";
                        this.txtStationPrinter.Foreground = Brushes.Lime;
                    }
                    else
                    {
                        this.txtStationPrinter.Text = "OFFLINE";
                        this.txtStationPrinter.Foreground = Brushes.Red;
                    }
                }));
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    this.txtResult.Text = ex.Message.ToString() + " - STATION PRINTER - STATUS CHANGE";
                    this.txtResult.Foreground = Brushes.Red;
                }));

            }
        }
        private void Station_Printer_ONDataReacive(string Barcode, string ClientIP) => this.Station_PrinterDataReacive(Barcode, ClientIP);

        private async void Station_PrinterDataReacive(string Barcode, string Client)
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    if (Barcode.ToUpper().Contains("A") || Barcode.ToUpper().Contains("S") || Barcode.ToUpper().Contains("G"))
                    {
                        txtStationPrinter.Text = "ONLINE";
                        txtStationPrinter.Foreground = Brushes.Lime;
                    }
                    else if (Barcode.Contains("b"))
                    {
                        txtStationPrinter.Text = "HEAD OPEN";
                        txtStationPrinter.Foreground = Brushes.Red;
                    }
                    else if (Barcode.Contains("c"))
                    {
                        txtStationPrinter.Text = "PAPER END";
                        txtStationPrinter.Foreground = Brushes.Red;
                    }
                    else if (Barcode.Contains("d"))
                    {
                        txtStationPrinter.Text = "RIBBON END";
                        txtStationPrinter.Foreground = Brushes.Red;
                    }
                    else if (Barcode.Contains("e"))
                    {
                        txtStationPrinter.Text = "PRINTER ERROR";
                        txtStationPrinter.Foreground = Brushes.Red;
                    }
                    else if (Barcode.Contains("f"))
                    {
                        txtStationPrinter.Text = "PAPER JAM OR SENSOR ERROR";
                        txtStationPrinter.Foreground = Brushes.Red;
                    }
                    else if (Barcode.Contains("0000000"))
                    {
                        txtStationPrinter.Text = "OFFLINE";
                        txtStationPrinter.Foreground = Brushes.Red;
                    }
                    else
                    {
                        //MessageBox.Show(Barcode);
                        txtStationPrinter.Text = "MACHINE ERROR";
                        txtStationPrinter.Foreground = Brushes.Red;
                    }
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

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtScan.Text == "")
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE SCAN PIPE BARCODE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    txtScan.Focus();
                    return;
                }
                Transaction("Save");
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MANUAL_PRINTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                CommonVariable.PageOpenClose = "Close";
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MANUAL_PRINTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Transaction("GetLineName");
                txtHeader.Text = CommonClasses.CommonVariable.ItemType + " MANUAL LABEL PRINTING";
                Start1();
                //if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Line.txt"))
                //{
                //    StreamReader SR = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\Line.txt");
                //    string Line = SR.ReadToEnd();
                //    SR.Dispose();
                //    SR.Close();
                //    cmbLine.Text = Line;
                //}
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MANUAL_PRINTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.P) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.P))
            {
                BtnPrint_Click(sender, e);
            }

            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.C) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.C))
            {
                BtnClear_Click(sender, e);
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.E) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.E) || Keyboard.IsKeyDown(Key.Escape) && Keyboard.IsKeyDown(Key.Escape))
            {
                BtnExit_Click(sender, e);
            }
        }

        private void Transaction(string Type)
        {

            if (Type == "GetLineName")
            {
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode = CommonClasses.CommonVariable.PlantCode;
                DataTable DT = obj_BL.BL_Transaction().Tables[0];
                CommonClasses.CommonMethods.FillComboBox(cmbLine, DT, "LineNo", "LineName");
            }
            if (Type == "GetStation")
            {
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode = CommonClasses.CommonVariable.PlantCode;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.LineNo = cmbLine.Text.ToString();
                DataTable table = obj_BL.BL_Transaction().Tables[0];
                CommonClasses.CommonMethods.FillComboBox(cmbStation, table, "Station", "Station");
            }
            if (Type == "GetIPDetails")
            {
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode = CommonClasses.CommonVariable.PlantCode;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.LineNo = cmbLine.Text.ToString();
                DataTable dt = obj_BL.BL_Transaction().Tables[0];

                DataView dv = new DataView(dt);
                dv.RowFilter = "Station='"+cmbStation.Text+"'";
                DataTable table = dv.ToTable();

                for (int index = 0; index < table.Rows.Count; ++index)
                {

                    if (obj_Client != null)
                        obj_Client.Dispose();

                    this.obj_Client = (NetworkClient)null;
                    this.obj_Client = new NetworkClient();
                    this.obj_Client.OnDataArrived += new NetworkClient.DataArrivalHandler(Station_PrinterDataReacive);

                    this.obj_Client.OnScannerStatusChanged += new NetworkClient.ScannerStatusChangeHandler(Station_Printer_StatusChange);
                    this.obj_Client.connection(table.Rows[index]["IPAddress"].ToString(), Convert.ToInt32(Convert.ToInt32(table.Rows[index]["PortNo"])));
                }
            }
            if (Type == "Save")
            {
                if (txtStationPrinter.Text == "ONLINE")
                {
                    string ModelNo = txtScan.Text.Substring(1, 3);

                    ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.TransactionType = "Printing";
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode = CommonClasses.CommonVariable.PlantCode;
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.LineNo = cmbLine.Text.ToString();
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.Station = cmbStation.Text.ToString();
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.Barcode = txtScan.Text.ToString();
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.ModelNo = ModelNo.ToString();
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.Process = "Manual";
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.ItemType = CommonClasses.CommonVariable.ItemType;
                    DataSet ds = obj_BL.BL_Transaction();

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0][0].ToString() == "Saved")
                        {
                            txtModelNo.Text = ModelNo;
                            txtModelName.Text = ds.Tables[0].Rows[0]["ModelName"].ToString();
                            txtPipe.Text = txtScan.Text;
                            txtSerial.Text = ds.Tables[0].Rows[0]["SerialNo"].ToString();
                            txtBarcode.Text = ds.Tables[0].Rows[0]["Barcode"].ToString();
                            txtCount.Text = ds.Tables[0].Rows[0]["Count"].ToString();
                            txtDate.Text = System.DateTime.Now.ToString("dd MMM yyyy HH:MM:ss");

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
                            obj_Client.Write(prn);
                            this.txtResult.Text = "SUCCESSFULL";
                            this.txtResult.Foreground = Brushes.Green;
                            txtScan.Text = "";
                            txtScan.Focus();
                            Start();
                        }
                        else
                        {
                            this.txtResult.Text = ds.Tables[0].Rows[0][0].ToString();
                            this.txtResult.Foreground = Brushes.Red;
                            CommonClasses.CommonMethods.MessageBoxShow(ds.Tables[0].Rows[0][0].ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

                        }
                    }
                }
                else
                {
                    this.txtResult.Text = "PRINTER ERROR";
                    this.txtResult.Foreground = Brushes.Red;
                    CommonClasses.CommonMethods.MessageBoxShow("PRINTER ERROR", CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

                }
            }
        }
        

        private void CmbLineNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbLine.SelectedIndex > -1)
                {
                    DataRowView dr = (DataRowView)cmbLine.SelectedItem;
                    cmbLine.Text = dr[1].ToString();
                    //StreamWriter SW = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Line.txt", false);
                    //SW.Write(cmbLine.Text.ToString());
                    //SW.Dispose();
                    //SW.Close();
                    Transaction("GetStation");
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MANUAL_PRINTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void CmbStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbStation.SelectedIndex > -1)
                {
                    DataRowView dr = (DataRowView)cmbStation.SelectedItem;
                    cmbStation.Text = dr[0].ToString();
                    Transaction("GetIPDetails");
                    txtScan.Focus();
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MANUAL_PRINTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void TxtScan_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Return)
            {
                BtnPrint_Click(sender, e);
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (obj_Client != null)
                obj_Client.Dispose();
        }
    }
}

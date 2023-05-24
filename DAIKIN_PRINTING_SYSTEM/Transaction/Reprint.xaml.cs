using DAIKIN_PRINTING_SYSTEM.CommonClasses;
using DAIKIN_PRINTING_SYSTEM.StartUp;
using Sato_Network_Client_DLL;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for Reprint.xaml
    /// </summary>
    public partial class Reprint : Page
    {
        public Reprint()
        {
            InitializeComponent();
        }
        #region Variable and Objects

        BUSINESS_LAYER.Business_Layer.Business_Layer obj_BL = new BUSINESS_LAYER.Business_Layer.Business_Layer();
        DataTable dt = new DataTable();
        #endregion

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dvgMasterDeatils.Items.Count > 0)
                {
                    TextRange range = new TextRange(txtRemarks.Document.ContentStart, txtRemarks.Document.ContentEnd);
                    if (range.Text.Trim() == "")
                    {
                        CommonMethods.MessageBoxShow("PLEASE ENTER REMARKS", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                        txtRemarks.Focus();
                        return;
                    }

                    Transaction("Reprint");
                }
                else
                    CommonClasses.CommonMethods.MessageBoxShow("DATA NOT FOUND", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "RE-PRINTING", CommonClasses.CommonVariable.UserID);
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
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "RE-PRINTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            txtScan.Text = "";
            cmbLine.Text = "";
            cmbStation.Text = "";
            dtpFrom.Text = "";
            dtpTo.Text = "";
            txtRemarks.Document.Blocks.Clear();
            PCDetails.IsChecked = false;

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Transaction("GetLineName");
                txtHeader.Text = CommonClasses.CommonVariable.ItemType + " RE-PRINTING";
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "RE-PRINTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.P) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.P))
            {
                BtnPrint_Click(sender, e);
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.S))
            {
                BtnShow_Click(sender, e);
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
                CommonClasses.CommonMethods.FillComboBox(cmbLine, DT, "LineNo", "LineNo");
            }
            if (Type == "GetStation")
            {
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode = CommonClasses.CommonVariable.PlantCode;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.LineNo = cmbLine.SelectedValue.ToString();
                DataTable table = obj_BL.BL_Transaction().Tables[0];
                CommonClasses.CommonMethods.FillComboBox(cmbStation, table, "Station", "Station");

            }
            if (Type == "GetReprintData")
            {
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode = CommonClasses.CommonVariable.PlantCode;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.ItemType = CommonClasses.CommonVariable.ItemType;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Barcode = txtScan.Text;
                TextRange range = new TextRange(txtRemarks.Document.ContentStart, txtRemarks.Document.ContentEnd);
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Remarks = range.Text;
                if (dtpFrom.Text != "" && dtpTo.Text != "")
                {
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.FromDate = dtpFrom.SelectedDate.Value.ToString("dd MMM yyyy HH:mm:ss");
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.ToDate = dtpTo.SelectedDate.Value.ToString("dd MMM yyyy HH:mm:ss");
                }

                DataTable table = obj_BL.BL_Transaction().Tables[0];
                DataView dv = new DataView(table);

                if (cmbLine.Text != "")
                {
                    dv.RowFilter = "LineNo='" + cmbLine.Text + "'";
                }
                if (cmbStation.Text != "")
                {
                    dv.RowFilter = "Station='" + cmbStation.Text + "'";
                }
                if (txtScan.Text != "")
                {
                    dv.RowFilter = "DPMBarcode='" + txtScan.Text + "' or Barcode='" + txtScan.Text + "'";
                }
                dt = dv.ToTable();
                dvgMasterDeatils.ItemsSource = dt.DefaultView;

            }
            if (Type == "Reprint")
            {
                string Result = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Flag"].ToString() == "True")
                    {
                        ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                        ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode = CommonClasses.CommonVariable.PlantCode;
                        ENTITY_LAYER.Entity_Layer.Entity_Layer.Barcode = dt.Rows[i]["Barcode"].ToString();
                        ENTITY_LAYER.Entity_Layer.Entity_Layer.ModelNo = dt.Rows[i]["ModelNo"].ToString();
                        ENTITY_LAYER.Entity_Layer.Entity_Layer.LineNo = dt.Rows[i]["LineNo"].ToString();
                        ENTITY_LAYER.Entity_Layer.Entity_Layer.Station = dt.Rows[i]["Station"].ToString();
                        DataSet ds = obj_BL.BL_Transaction();
                        if (ds.Tables.Count > 0)
                        {
                            //rrttt
                            if (ds.Tables[0].Rows[0][0].ToString() == "Saved")
                            {

                                string prn = ds.Tables[0].Rows[0]["PRN"].ToString();
                                prn = prn.Replace("{MODEL_NO}", ds.Tables[0].Rows[0]["ModelNo"].ToString().PadLeft(4, ' '));
                                prn = prn.Replace("{MODEL_NAME}", ds.Tables[0].Rows[0]["ModelName"].ToString());
                                prn = prn.Replace("{REF}", ds.Tables[0].Rows[0]["Ref"].ToString());
                                prn = prn.Replace("{REF_OIL}", ds.Tables[0].Rows[0]["RefOil"].ToString());
                                prn = prn.Replace("{ML}", ds.Tables[0].Rows[0]["ML"].ToString());
                                prn = prn.Replace("{MFG_No}", ds.Tables[0].Rows[0]["MFGNo"].ToString());
                                prn = prn.Replace("{BARCODE}", ds.Tables[0].Rows[0]["Barcode"].ToString());
                                prn = prn.Replace("{AP}", ds.Tables[0].Rows[0]["AP"].ToString());
                                prn = prn.Replace("{DATE}", ds.Tables[0].Rows[0]["Date"].ToString());

                                NetworkClient obj_Client = new NetworkClient();
                                obj_Client.connection(ds.Tables[1].Rows[0]["IPAddress"].ToString(), Convert.ToInt32(Convert.ToInt32(ds.Tables[1].Rows[0]["PortNo"])));
                                obj_Client.Write(prn);
                                Thread.Sleep(500);
                                obj_Client.Dispose();

                                Result = "Saved";

                            }
                            else
                            {
                                CommonClasses.CommonMethods.MessageBoxShow(ds.Tables[0].Rows[0][0].ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                                return;
                            }
                        }

                    }
                }

                if (Result == "Saved")
                {
                    CommonClasses.CommonMethods.MessageBoxShow("REPRINTED SUCCSSFULLY", CustomMessageBox.CustomStriing.Successfull.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    PCDetails.IsChecked = false;
                }
                else
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE SELECT DATA TO REPRINT", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void CmbLineNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbLine.SelectedIndex > -1)
                {
                    Transaction("GetStation");
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "RE-PRINTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void PCDetails_Checked(object sender, RoutedEventArgs e)
        {
            try
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Flag"] = "True";
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "RE-PRINTING", CommonClasses.CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void PCDetails_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Flag"] = "False";
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "RE-PRINTING", CommonClasses.CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              
                Transaction("GetReprintData");
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "RE-PRINTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void TxtScan_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    Transaction("GetReprintData");
                    PCDetails.IsChecked = true;
                    BtnPrint_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "RE-PRINTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }
    }
}

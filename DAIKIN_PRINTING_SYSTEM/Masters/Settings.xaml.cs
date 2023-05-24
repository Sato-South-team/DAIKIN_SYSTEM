using DAIKIN_PRINTING_SYSTEM.CommonClasses;
using DAIKIN_PRINTING_SYSTEM.StartUp;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace DAIKIN_PRINTING_SYSTEM.Masters
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
        }

        #region Variable and Objects
        BUSINESS_LAYER.Business_Layer.Business_Layer obj_BL = new BUSINESS_LAYER.Business_Layer.Business_Layer();
        int RefNo = 0;
        DataTable dt = new DataTable();
        #endregion

        #region Methods

        private bool ControlValidation()
        {
            if (cmbLine.SelectedIndex == -1)
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE SELECT LINE NAME", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                cmbLine.Focus();
                return false;
            }
            if (cmbStation.SelectedIndex == -1)
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE SELECT STATION", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                cmbStation.Focus();
                return false;
            }
            if (cmbCategory.SelectedIndex == -1)
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE SELECT CATEGORY", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                cmbCategory.Focus();
                return false;
            }
            if (cmbHardwareType.SelectedIndex == -1)
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE SELECT HARDWARE TYPE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                cmbHardwareType.Focus();
                return false;
            }

            if (txtIP.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER IP ADDRESS", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtIP.Focus();
                return false;
            }
            if (txtPort.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER PORT NO", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtPort.Focus();
                return false;
            }

            //if (cmbHardwareType.Text == "PRINTER")
            //{

            //    TextRange range = new TextRange(txtPRN.Document.ContentStart, txtPRN.Document.ContentEnd);
            //    if (range.Text.Trim() == "")
            //    {
            //        CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER PRN DATA", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            //        txtPRN.Focus();
            //        return false;
            //    }
            //}
            return true;
        }
        private void Transaction(string Type)
        {

            if (Type == "Save" || Type == "Update" || Type == "Delete")
            {
                ENTITY_LAYER.Entity_Layer.Entity_Layer.HardwareType = cmbHardwareType.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.IP = txtIP.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Port = txtPort.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.LineNo = cmbLine.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Station = cmbStation.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Category = cmbCategory.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode = CommonClasses.CommonVariable.PlantCode;

                //TextRange range = new TextRange(txtPRN.Document.ContentStart, txtPRN.Document.ContentEnd);
                //ENTITY_LAYER.Entity_Layer.Entity_Layer.PRN = range.Text;

                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.RefNo = RefNo;
                CommonClasses.CommonVariable.Result = obj_BL.BL_SettingsTransaction();
                if (CommonClasses.CommonVariable.Result == "Saved")
                {
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.DataSaved, CustomMessageBox.CustomStriing.Successfull.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    Clear();
                }
                else if (CommonClasses.CommonVariable.Result == "Updated")
                {
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.DataUpdated, CustomMessageBox.CustomStriing.Successfull.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    Clear();
                }
                else if (CommonClasses.CommonVariable.Result == "Duplicate")
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.Duplicate, CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                else if (CommonClasses.CommonVariable.Result != "Deleted")
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.Result, CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
            if (Type == "LoadDetails")
            {
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode = CommonClasses.CommonVariable.PlantCode;

                if (cmbStation.SelectedIndex > -1)
                {
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.LineNo = cmbLine.SelectedValue.ToString();
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.Station = cmbStation.SelectedValue.ToString();
                }
                else
                {
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.LineNo = "";
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.Station = "";
                }

                DataTable dt = obj_BL.BL_SettingsDetails().Tables[0];
                dvgMasterDeatils.ItemsSource = dt.DefaultView;
            }
            if (Type == "GetLineName")
            {
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode = CommonClasses.CommonVariable.PlantCode;
                dt = obj_BL.BL_SettingsDetails().Tables[0];
                DataView dv = new DataView(dt);
                CommonClasses.CommonMethods.FillComboBox(cmbLine, dv.ToTable(true, "LineNo"), "LineNo", "LineNo");
            }

        }
        private void Clear()
        {
            cmbHardwareType.Text = "";
            txtIP.Text = "";
            cmbCategory.Text = "";
            txtPort.Text = "";
            cmbLine.Text = "";
            cmbStation.Text = "";
            Transaction("LoadDetails");
            cmbLine.Focus();
            RefNo = 0;
        }
        #endregion

        #region Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                Transaction("LoadDetails");
                Transaction("GetLineName");
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "IP_SETTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dvgMasterDeatils.SelectedItems.Count == 0)
                {
                    if (ControlValidation())
                    {
                        Transaction("Save");
                    }
                }
                else
                    CommonClasses.CommonMethods.MessageBoxShow("YOU CAN NOT SAVE THE RECORDS PLEASE GO FOR DELETION OR UPDATION", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "IP_SETTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dvgMasterDeatils.SelectedItems.Count > 0)
                {
                    if (dvgMasterDeatils.SelectedItems.Count == 1)
                    {
                        if (ControlValidation())
                            Transaction("Update");
                    }
                    else
                        CommonClasses.CommonMethods.MessageBoxShow("MULTIPLE SELECTION WILL NOT SUPPORT FOR UPDATE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                }
                else
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.RowSelection, CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "IP_SETTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dvgMasterDeatils.SelectedItems.Count > 0)
                {
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.DeleteConfirm, CustomMessageBox.CustomStriing.Question.ToString(), CustomMessageBox.CustomStriing.YESNO.ToString());
                    if (CommonClasses.CommonVariable.Result == "YES")
                    {
                        for (int i = 0; i < dvgMasterDeatils.SelectedItems.Count; i++)
                        {
                            DataRowView dr = (DataRowView)dvgMasterDeatils.SelectedItems[i];
                            RefNo = Convert.ToInt32(dr["Refno"]);
                            Transaction("Delete");
                        }

                        if (CommonClasses.CommonVariable.Result == "Deleted")
                        {
                            CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.DataDeleted, CustomMessageBox.CustomStriing.Successfull.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                            Clear();
                        }
                        else
                        {
                            CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.Result, CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                        }
                    }
                }
                else
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.RowSelection, CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "IP_SETTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "IP_SETTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CommonVariable.PageOpenClose = "Close";
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "IP_SETTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
        private void dvgMasterDeatils_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dvgMasterDeatils.SelectedItems.Count == 1)
                {
                    DataRowView dr = (DataRowView)dvgMasterDeatils.SelectedItems[0];
                    RefNo = Convert.ToInt32(dr["Refno"]);
                    cmbHardwareType.Text = dr["HardwareType"].ToString();
                    cmbCategory.Text = dr["Category"].ToString();
                    txtPort.Text = dr["PortNo"].ToString();
                    txtIP.Text = dr["IPAddress"].ToString();
                    cmbLine.Text = dr["LineNo"].ToString();
                    cmbStation.Text = dr["Station"].ToString();
                    cmbLine.Focus();
                }
                else
                {
                    RefNo = 0;
                    cmbHardwareType.Text = "";
                    txtIP.Text = "";
                    txtPort.Text = "";
                    cmbLine.Text = "";
                    cmbStation.Text = "";
                   // txtPRN.Document.Blocks.Clear();
                    cmbLine.Focus();
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "IP_SETTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.S))
            {
                btnSave_Click(sender, e);
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.U) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.U))
            {
                btnUpdate_Click(sender, e);
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.C) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.C))
            {
                btnClear_Click(sender, e);
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.E) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.E) || Keyboard.IsKeyDown(Key.Escape) && Keyboard.IsKeyDown(Key.Escape))
            {
                btnExit_Click(sender, e);
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.D))
            {
                btnDelete_Click(sender, e);
            }
        }

        #endregion


        private void CmbLine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbLine.SelectedIndex > -1)
                {
                    DataView dv = new DataView(dt);
                    dv.RowFilter = "LineNo=" + cmbLine.SelectedValue.ToString();
                    CommonClasses.CommonMethods.FillComboBox(cmbStation, dv.ToTable(), "Station", "Station");
                }
                else
                {
                    cmbStation.Text = "";
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "IP_SETTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
        private void CmbStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbStation.SelectedIndex > -1)
                {
                    if (dvgMasterDeatils.SelectedItems.Count == 0)
                        Transaction("LoadDetails");
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "IP_SETTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void CmbHardwareType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbHardwareType.SelectedIndex > -1)
                {
                    txtIP.Text = "";
                    txtPort.Text = "";
                  //  txtPRN.Document.Blocks.Clear();
                    //txtPRN.IsEnabled = true;
                    ComboBoxItem cb = (ComboBoxItem)cmbHardwareType.SelectedItem;
                    cmbHardwareType.Text = cb.Content.ToString();
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "IP_SETTING", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
    }
}

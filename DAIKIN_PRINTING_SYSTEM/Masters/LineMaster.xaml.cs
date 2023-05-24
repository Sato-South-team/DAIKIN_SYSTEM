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
    /// Interaction logic for LineMaster.xaml
    /// </summary>
    public partial class LineMaster : Page
    {
        public LineMaster()
        {
            InitializeComponent();
        }
        #region Variable and Objects
        BUSINESS_LAYER.Business_Layer.Business_Layer obj_BL = new BUSINESS_LAYER.Business_Layer.Business_Layer();
        int RefNo = 0;
        #endregion

        #region Methods

        private bool ControlValidation()
        {
            if (txtPlantName.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER PLANT CODE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtPlantName.Focus();
                return false;
            }
            if (txtPlantCode.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER PLANT NAME", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtPlantCode.Focus();
                return false;
            }
            if (cmbLineNo.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER/SELECT LINE NO", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                cmbLineNo.Focus();
                return false;
            }
            if (txtLineName.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER LINE NAME", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtLineName.Focus();
                return false;
            }
            if (txtStationName.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER STATION NAME", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtStationName.Focus();
                return false;
            }
            return true;
        }
        private void Transaction(string Type)
        {
            if (Type == "Save" || Type == "Update" || Type == "Delete")
            {
                ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode = txtPlantCode.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantName = txtPlantName.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.LineNo = cmbLineNo.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.LineName = txtLineName.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Station = txtStationName.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.RefNo = RefNo;
                CommonClasses.CommonVariable.Result = obj_BL.BL_LineMasterTransaction();
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
                if (cmbLineNo.SelectedIndex > -1)
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.LineNo = cmbLineNo.SelectedValue.ToString();
                else
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.LineNo = "";

                DataTable dt = obj_BL.BL_LineMasterDetails().Tables[0];
                dvgMasterDeatils.ItemsSource = dt.DefaultView;
            }
            if (Type == "GetLineName")
            {
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                DataTable dt = obj_BL.BL_LineMasterDetails().Tables[0];
                CommonClasses.CommonMethods.FillComboBox(cmbLineNo, dt, "LineNo", "LineName");
            }

        }
        private void Clear()
        {
            cmbLineNo.Text = "";
            txtLineName.Text = "";
            txtStationName.Text = "";
            Transaction("LoadDetails");
            Transaction("GetLineName");
            txtPlantName.Focus();
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
                txtPlantName.Focus();
                txtPlantCode.Text = CommonClasses.CommonVariable.PlantCode;
                txtPlantName.Text = CommonClasses.CommonVariable.PlantName;

            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LINE_MASTER", CommonClasses.CommonVariable.UserID);
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
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LINE_MASTER", CommonClasses.CommonVariable.UserID);
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
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LINE_MASTER", CommonClasses.CommonVariable.UserID);
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
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LINE_MASTER", CommonClasses.CommonVariable.UserID);
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
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LINE_MASTER", CommonClasses.CommonVariable.UserID);
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
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LINE_MASTER", CommonClasses.CommonVariable.UserID);
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
                    cmbLineNo.Text = dr["LineNo"].ToString();
                    txtLineName.Text = dr["LineName"].ToString();
                    txtPlantName.Text = dr["PlantName"].ToString();
                    txtPlantCode.Text = dr["PlantCode"].ToString();
                    txtStationName.Text = dr["Station"].ToString();
                    txtPlantName.Focus();
                }
                else
                {
                    RefNo = 0;
                    cmbLineNo.Text = "";
                    txtLineName.Text = "";
                    txtStationName.Text = "";
                    txtPlantName.Focus();
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LINE_MASTER", CommonClasses.CommonVariable.UserID);
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

      

        private void CmbLineNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dvgMasterDeatils.SelectedItems.Count == 0)
                {
                    if (cmbLineNo.SelectedIndex > -1)
                    {
                        txtLineName.Text = cmbLineNo.SelectedValue.ToString();
                        Transaction("LoadDetails");
                    }
                    else
                    {
                        txtLineName.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LINE_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
    }
}

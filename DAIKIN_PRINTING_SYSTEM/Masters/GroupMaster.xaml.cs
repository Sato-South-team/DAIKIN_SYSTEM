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
using System.Windows.Shapes;

namespace DAIKIN_PRINTING_SYSTEM.Masters
{
    /// <summary>
    /// Interaction logic for GroupMaster.xaml
    /// </summary>
    public partial class GroupMaster : Page
    {
        public GroupMaster()
        {
            InitializeComponent();
        }

        #region Variable and Objects
        BUSINESS_LAYER.Business_Layer.Business_Layer obj_BL = new BUSINESS_LAYER.Business_Layer.Business_Layer();
        #endregion

        #region Methods
       

        private void Transaction(string Type)
        {
            if (Type == "Save" || Type == "Delete")
            {
                string Rights = "";
                ENTITY_LAYER.Entity_Layer.Entity_Layer.GroupID = cmbgroupid.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                for (int i = 0; i < dvgModules.Items.Count; i++)
                {
                    DataRowView Row = (DataRowView)dvgModules.Items[i];
                    string Flag = Row["Flag"].ToString();
                    if (Flag == "True")
                    {
                        Rights = Rights + Row["ModuleName"].ToString() + ",";
                    }

                }

                if (Rights == "")
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE SELECT MODULES FROM LIST VIEW", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    return;
                }

                ENTITY_LAYER.Entity_Layer.Entity_Layer.Rights = Rights;
                CommonClasses.CommonVariable.Result = obj_BL.BL_GroupMasterTransaction();
                if (CommonClasses.CommonVariable.Result == "Saved")
                {
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.DataSaved, CustomMessageBox.CustomStriing.Successfull.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    Clear();
                }
                else if (CommonClasses.CommonVariable.Result == "Deleted")
                {
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.DataDeleted, CustomMessageBox.CustomStriing.Successfull.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    Clear();
                }
                else
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.Result, CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
            if (Type == "LoadFormDetails")
            {
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                DataTable dt = obj_BL.BL_GroupMasterDetails().Tables[0];
                dt.Columns["Flag"].ReadOnly = false;
                dvgModules.ItemsSource = dt.DefaultView;
            }
            if (Type == "getRightsDetails")
            {
                GridItemUnChecked();
                DataTable dtGoupDetails = new DataTable();
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.GroupID = Convert.ToString(cmbgroupid.SelectedValue);
                dtGoupDetails = obj_BL.BL_GroupMasterDetails().Tables[0];
                if (cmbgroupid.SelectedIndex == -1)
                    CommonClasses.CommonMethods.FillComboBox(cmbgroupid, dtGoupDetails, "GroupID", "GroupID");
                else
                {

                    if (dtGoupDetails.Rows.Count > 0)
                    {
                        string[] ModuleName = dtGoupDetails.Rows[0]["Rights"].ToString().Split(',');
                        for (int i = 0; i < dvgModules.Items.Count; i++)
                        {
                            for (int j = 0; j < ModuleName.Length; j++)
                            {
                                DataRowView row = (DataRowView)dvgModules.Items[i];
                                if (row["ModuleName"].ToString() == ModuleName[j])
                                {
                                    row["Flag"] = "True";
                                }
                            }
                        }
                    }
                }
            }
        }
        private void Clear()
        {
            cmbgroupid.SelectedIndex = -1;
            cmbgroupid.Text = "";
            Transaction("getRightsDetails");
            GridItemUnChecked();
            cmbgroupid.Focus();
        }

        private void GridItemUnChecked()
        {
            PCDetails.IsChecked = false;
            for (int i = 0; i < dvgModules.Items.Count; i++)
            {
                DataRowView row = (DataRowView)dvgModules.Items[i];
                row["Flag"] = "false";
            }

        }
        private void GridItemChecked()
        {
            PCDetails.IsChecked = true;
            for (int i = 0; i < dvgModules.Items.Count; i++)
            {
                DataRowView row = (DataRowView)dvgModules.Items[i];
                row["Flag"] = "True";
            }

        }

        #endregion

        #region Events

       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
                Transaction("LoadFormDetails");
                Transaction("getRightsDetails");
                cmbgroupid.Focus();
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "GROUP_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbgroupid.Text != "")
                {
                    Transaction("Save");
                }
                else
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER GROUP ID", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    cmbgroupid.Focus();
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "GROUP_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbgroupid.SelectedIndex > -1)
                {
                    CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.DeleteConfirm, CustomMessageBox.CustomStriing.Question.ToString(), CustomMessageBox.CustomStriing.YESNO.ToString());
                    if (CommonClasses.CommonVariable.Result == "YES")
                    {
                        Transaction("Delete");
                    }
                }
                else
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE SELECT GROUP ID", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    cmbgroupid.Focus();
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "GROUP_MASTER", CommonClasses.CommonVariable.UserID);
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
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "GROUP_MASTER", CommonClasses.CommonVariable.UserID);
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
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "GROUP_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void PCDetails_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                GridItemChecked();
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "GROUP_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void PCDetails_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                GridItemUnChecked();
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "GROUP_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
        private void cmbgroupid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbgroupid.SelectedIndex > -1)
                {
                    Transaction("getRightsDetails");
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "GROUP_MASTER", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.S))
            {
                btnSave_Click(sender, e);
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
    }
}

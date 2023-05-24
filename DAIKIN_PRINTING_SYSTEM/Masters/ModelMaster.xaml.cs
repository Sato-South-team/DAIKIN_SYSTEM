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
    /// Interaction logic for PartMaster.xaml
    /// </summary>
    public partial class ModelMaster : Page
    {
        public ModelMaster()
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
            if (txtModelNo.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER MODEL NO", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtModelNo.Focus();
                return false;
            }
            if (txtModelName.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER MODEL NAME", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtModelName.Focus();
                return false;
            }
            if (txtDescrip.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER DESCRIPTION", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtDescrip.Focus();
                return false;
            }
            if (txtAP.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER AP", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtAP.Focus();
                return false;
            }
            if (txtRefOil.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER REF OIL", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtRefOil.Focus();
                return false;
            }
            if (txtRef.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER REF", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtRef.Focus();
                return false;
            }
            if (txtML.Text == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER ML", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtML.Focus();
                return false;
            }
            TextRange range = new TextRange(txtPRN.Document.ContentStart, txtPRN.Document.ContentEnd);
            if (range.Text.Trim() == "")
            {
                CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER PRN DATA", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtPRN.Focus();
                return false;
            }
            return true;
        }
        private void Transaction(string Type)
        {
            if (Type == "Save" || Type == "Update" || Type == "Delete")
            {
                ENTITY_LAYER.Entity_Layer.Entity_Layer.ModelNo = txtModelNo.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.ModelName = txtModelName.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Descriptions = txtDescrip.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.AP = txtAP.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.RefOil = txtRefOil.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Ref = txtRef.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.ML = txtML.Text;
                TextRange range = new TextRange(txtPRN.Document.ContentStart, txtPRN.Document.ContentEnd);
                ENTITY_LAYER.Entity_Layer.Entity_Layer.PRN = range.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.RefNo = RefNo;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode = CommonClasses.CommonVariable.PlantCode;

                CommonClasses.CommonVariable.Result = obj_BL.BL_ModelMasterTransaction();
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

                if (txtModelNo.Text!="")
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.ModelNo = txtModelNo.Text;
                else
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.ModelNo = "";

                DataTable dt = obj_BL.BL_ModelMasterDetails().Tables[0];
                dvgMasterDeatils.ItemsSource = dt.DefaultView;
            }
        }
        private void Clear()
        {
            txtModelName.Text = "";
            txtModelNo.Text = "";
            txtDescrip.Text = "";
            txtAP.Text = "";
            txtRef.Text = "";
            txtRefOil.Text = "";
            txtML.Text = "";
            txtPRN.Document.Blocks.Clear();
            Transaction("LoadDetails");
            txtModelNo.Focus();
            RefNo = 0;
        }
        #endregion

        #region Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Transaction("LoadDetails");
                txtModelNo.Focus();
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MODEL_MASTER", CommonClasses.CommonVariable.UserID);
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
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MODEL_MASTER", CommonClasses.CommonVariable.UserID);
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
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MODEL_MASTER", CommonClasses.CommonVariable.UserID);
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
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MODEL_MASTER", CommonClasses.CommonVariable.UserID);
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
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MODEL_MASTER", CommonClasses.CommonVariable.UserID);
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
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MODEL_MASTER", CommonClasses.CommonVariable.UserID);
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
                    txtModelNo.Text = dr["ModelNo"].ToString();
                    txtModelName.Text = dr["ModelName"].ToString();
                    txtDescrip.Text = dr["Descriptions"].ToString();
                    txtAP.Text = dr["AP"].ToString();
                    txtRefOil.Text = dr["RefOil"].ToString();
                    txtRef.Text = dr["Ref"].ToString();
                    txtML.Text = dr["ML"].ToString();
                    txtPRN.Document.Blocks.Clear();
                    txtPRN.AppendText(dr["PRN"].ToString());
                    txtModelNo.Focus();
                }
                else
                {
                    RefNo = 0;
                    txtModelName.Text = "";
                    txtModelNo.Text = "";
                    txtDescrip.Text = "";
                    txtAP.Text = "";
                    txtRef.Text = "";
                    txtRefOil.Text = "";
                    txtML.Text = "";
                    txtPRN.Document.Blocks.Clear();
                    txtModelNo.Focus();
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MODEL_MASTER", CommonClasses.CommonVariable.UserID);
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

    }
}

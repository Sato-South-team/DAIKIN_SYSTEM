using DAIKIN_PRINTING_SYSTEM.CommonClasses;
using DAIKIN_PRINTING_SYSTEM.StartUp;
using Microsoft.Win32;
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

namespace DAIKIN_PRINTING_SYSTEM.Reports
{
    /// <summary>
    /// Interaction logic for PrintingReport.xaml
    /// </summary>
    public partial class PrintingReport : Page
    {
        public PrintingReport()
        {
            InitializeComponent();
        }
        #region Variable and Objects
        BUSINESS_LAYER.Business_Layer.Business_Layer obj_BL = new BUSINESS_LAYER.Business_Layer.Business_Layer();
        int RefNo = 0;
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        DataTable ModuleList = new DataTable();
        DataTable StationList = new DataTable();

        #endregion


        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbType.SelectedIndex == -1)
                {
                    CommonMethods.MessageBoxShow("PLEASE SELECT REPORT TYPE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    cmbType.Focus();
                    return;
                }
                Transaction("Report");
            }
            catch (Exception ex)
            {
                this.obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PRINTING_REPORT", CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
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
                this.obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PRINTING_REPORT", CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
        private void Clear()
        {
            chkModel.IsChecked = false;
            chkStation.IsChecked = false;
            dtpFrom.Text = "";
            dtpTo.Text = "";
            cmbType.Text = "";
            cmbLineNo.Text = "";
            brdDatagrid.Visibility = Visibility.Hidden;

        }
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                this.obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PRINTING_REPORT", CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Transaction(string Type)
        {

            if (Type == "LineNo")
            {
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode = CommonClasses.CommonVariable.PlantCode;

                dt1 = obj_BL.BL_ReportDetails().Tables[0];
                DataView dv = new DataView(dt1);
                CommonClasses.CommonMethods.FillComboBox(cmbLineNo, dv.ToTable(true, "LineNo"), "LineNo", "LineNo");
            }
            if (Type == "ModelNo")
            {
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                ModuleList = obj_BL.BL_ReportDetails().Tables[0];
                this.listModel.ItemsSource = this.ModuleList.DefaultView;
            }
            if (Type == "Report")
            {
                if (dtpFrom.Text != "" && dtpTo.Text != "")
                {
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.FromDate = dtpFrom.SelectedDate.Value.ToString("dd MMM yyyy HH:mm:ss");
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.ToDate = dtpTo.SelectedDate.Value.ToString("dd MMM yyyy HH:mm:ss");
                }
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = cmbType.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.LineNo = cmbLineNo.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode = CommonClasses.CommonVariable.PlantCode;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Station = "";
                ENTITY_LAYER.Entity_Layer.Entity_Layer.ModelNo = "";
                for (int index = 0; index < this.StationList.Rows.Count; ++index)
                {
                    if (this.StationList.Rows[index]["Flag"].ToString() == "True")
                        ENTITY_LAYER.Entity_Layer.Entity_Layer.Station = !(ENTITY_LAYER.Entity_Layer.Entity_Layer.Station == "") ? ENTITY_LAYER.Entity_Layer.Entity_Layer.Station + ",'" + this.StationList.Rows[index][1].ToString() + "'" : "'" + this.StationList.Rows[index][1].ToString() + "'";
                }
                //if (ENTITY_LAYER.Entity_Layer.Entity_Layer.Station == "")
                //{
                //    this.chkStation.IsChecked = new bool?(true);
                //    for (int index = 0; index < this.StationList.Rows.Count; ++index)
                //    {
                //        if (this.StationList.Rows[index]["Flag"].ToString() == "True")
                //            ENTITY_LAYER.Entity_Layer.Entity_Layer.Station = !(ENTITY_LAYER.Entity_Layer.Entity_Layer.Station == "") ? ENTITY_LAYER.Entity_Layer.Entity_Layer.Station + ",'" + this.StationList.Rows[index][1].ToString() + "'" : "'" + this.StationList.Rows[index][1].ToString() + "'";
                //    }
                //}
                for (int index = 0; index < this.ModuleList.Rows.Count; ++index)
                {
                    if (this.ModuleList.Rows[index]["Flag"].ToString() == "True")
                        ENTITY_LAYER.Entity_Layer.Entity_Layer.ModelNo = !(ENTITY_LAYER.Entity_Layer.Entity_Layer.ModelNo == "") ? ENTITY_LAYER.Entity_Layer.Entity_Layer.ModelNo + ",'" + this.ModuleList.Rows[index][0].ToString() + "'" : "'" + this.ModuleList.Rows[index][0].ToString() + "'";
                }
               

                dt = obj_BL.BL_ReportDetails().Tables[0];

                DataView dv = new DataView(dt);
                if(ENTITY_LAYER.Entity_Layer.Entity_Layer.Station!="")
                dv.RowFilter = "STATION_NAME in ("+ ENTITY_LAYER.Entity_Layer.Entity_Layer.Station+")";

                if (ENTITY_LAYER.Entity_Layer.Entity_Layer.ModelNo != "")
                    dv.RowFilter = "MODEL_NO in (" + ENTITY_LAYER.Entity_Layer.Entity_Layer.ModelNo + ")";
                if (dv.ToTable().Rows.Count > 0)
                {
                    dvgMasterDeatils.ItemsSource = dv.ToTable().DefaultView;
                    brdDatagrid.Visibility = Visibility.Visible;
                }
                else
                    CommonMethods.MessageBoxShow("DATA NOT FOUND", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                brdDatagrid.Visibility = Visibility.Hidden;
                dtpFrom.Focus();
                Transaction("LineNo");
                Transaction("ModelNo");
                
            }
            catch (Exception ex)
            {
                this.obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PRINTING_REPORT", CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }





        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
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
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.P) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.P))
            {
                BtnExportPDF_Click(sender, e);
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.X) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.X))
            {
                BtnExportExcel_Click(sender, e);
            }
        }

        private void BtnExportPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF files (*.Pdf)|*.Pdf";
                saveFileDialog.ShowDialog();
                if (saveFileDialog.FileName != "")
                {
                    CommonMethods.CreateExcellFile(dt, saveFileDialog.FileName, cmbType.Text + " Data", true, cmbType.Text + " REPORT");
                }
            }
            catch (Exception ex)
            {
                this.obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PRINTING_REPORT", CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void BtnExportExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "EXCEL files (*.xlsx)|*.xlsx";
                saveFileDialog.ShowDialog();
                if (saveFileDialog.FileName != "")
                {
                    CommonMethods.CreateExcellFile(dt, saveFileDialog.FileName, cmbType.Text + " Data", false, cmbType.Text + " REPORT");
                }
            }
            catch (Exception ex)
            {
                this.obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PRINTING_REPORT", CommonVariable.UserID);
                CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void ChkStation_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                bool? isChecked = this.chkStation.IsChecked;
                bool flag = true;
                if (!(isChecked.GetValueOrDefault() == flag & isChecked.HasValue))
                    return;
                for (int index = 0; index < this.StationList.Rows.Count; ++index)
                    this.StationList.Rows[index]["Flag"] = (object)true;
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PRINTING_REPORT", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void ChkModel_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                bool? isChecked = this.chkModel.IsChecked;
                bool flag = true;
                if (!(isChecked.GetValueOrDefault() == flag & isChecked.HasValue))
                    return;
                for (int index = 0; index < this.ModuleList.Rows.Count; ++index)
                    this.ModuleList.Rows[index]["Flag"] = (object)true;
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PRINTING_REPORT", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void ChkModel_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                bool? isChecked = this.chkModel.IsChecked;
                bool flag = false;
                if (!(isChecked.GetValueOrDefault() == flag & isChecked.HasValue))
                    return;
                for (int index = 0; index < this.ModuleList.Rows.Count; ++index)
                    this.ModuleList.Rows[index]["Flag"] = (object)false;
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PRINTING_REPORT", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void ChkStation_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                bool? isChecked = this.chkStation.IsChecked;
                bool flag = false;
                if (!(isChecked.GetValueOrDefault() == flag & isChecked.HasValue))
                    return;
                for (int index = 0; index < this.StationList.Rows.Count; ++index)
                    this.StationList.Rows[index]["Flag"] = (object)false;
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PRINTING_REPORT", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void CmbLineNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(cmbLineNo.SelectedIndex>-1)
                {
                    DataRowView dr = (DataRowView)cmbLineNo.SelectedItem;
                    cmbLineNo.Text = dr[0].ToString();
                    DataView dv = new DataView(dt1);
                    dv.RowFilter = "LineNo="+cmbLineNo.Text;
                    StationList = dv.ToTable();
                    this.listStation.ItemsSource = StationList.DefaultView;
                }
                else
                {
                    this.listStation.ItemsSource = null;
                }
               
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PRINTING_REPORT", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
    }
}

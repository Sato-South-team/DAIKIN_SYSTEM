using System;
using System.Collections.Generic;
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
using System.Diagnostics;
using System.Data;
using DAIKIN_PRINTING_SYSTEM.CommonClasses;
using System.IO;
using Microsoft.VisualBasic;

namespace DAIKIN_PRINTING_SYSTEM.StartUp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Varialble and Objects
        BUSINESS_LAYER.Business_Layer.Business_Layer obj_BL = new BUSINESS_LAYER.Business_Layer.Business_Layer();
        string Rights = "";
        #endregion

        #region Methods
        private void ShowDateTime()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
        private void Transaction(string Type)
        {
            
            if (Type == "GetLineName")
            {
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                DataTable dt = obj_BL.BL_LineMasterDetails().Tables[0];
                CommonClasses.CommonMethods.FillComboBox(cmbPlant, dt, "PlantCode", "PlantName");
            }
            if (Type == "GetRights")
            {
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = Type;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.UserID = "";
                Rights = obj_BL.BL_Login();
            }

            

        }
        #endregion

        #region Events

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                this.txtDatetime.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");
                //while (this.NavigationService.CanGoBack)
                //{
                //    try
                //    {
                //        this.NavigationService.RemoveBackEntry();
                //    }
                //    catch (Exception ex)
                //    {
                //        break;
                //    }
                //}
                if (!(CommonVariable.PageOpenClose == "Close"))
                {
                    return;
                }
                if (CommonVariable.PageOpenClose == "Close")
                {
                    if (CommonVariable.ItemType == "PRODUCTION")
                    {
                        this.Background = (Brush)Brushes.White;
                        grdFrame.Visibility = Visibility.Visible;
                        //GridSubMenu.Visibility = Visibility.Visible;
                        grdFrame.Visibility = Visibility.Visible;
                        GridSubMenu.Visibility = Visibility.Hidden;
                        this.frmPage.Navigate((Uri)null);

                        MnuProduction_Click(null, null);

                    }
                    if (CommonVariable.ItemType == "PROTOTYPE")
                    {
                        this.Background = (Brush)Brushes.White;
                        grdFrame.Visibility = Visibility.Visible;
                        //GridSubMenu.Visibility = Visibility.Visible;
                        grdFrame.Visibility = Visibility.Visible;
                        GridSubMenu.Visibility = Visibility.Hidden;
                        this.frmPage.Navigate((Uri)null);

                        MnuPrototype_Click(null, null);

                    }
                    else
                    {
                        this.frmPage.Navigate((Uri)null);
                        this.Background = (Brush)Brushes.White;
                        grdFrame.Visibility = Visibility.Hidden;
                        GridSubMenu.Visibility = Visibility.Visible;
                    }
                }
                //else
                //{

                //}

                //if (CommonVariable.ChangeType == "ReportView")
                //    MnuReportView_Click(null, null);
                //else
                //{

                // }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                txtuserID.Text ="Application is using by "+  CommonClasses.CommonVariable.UserName;
                SizeToContent = SizeToContent.Manual;
                ShowDateTime();
                this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                WindowState = WindowState.Maximized;
                Transaction("GetLineName");

                if(File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Plant.txt"))
                {
                    StreamReader SR = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\Plant.txt");
                    CommonClasses.CommonVariable.PlantCode = SR.ReadToEnd();
                    SR.Dispose();
                    SR.Close();
                    cmbPlant.Text = CommonClasses.CommonVariable.PlantCode;
                }

                
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void BtnMasters_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int ControlsCount = 0;
                GridSubMenu.Children.RemoveRange(0, 9);

             


                if (CommonClasses.CommonVariable.Rights == "" || CommonClasses.CommonVariable.Rights == null)
                {
                    CommonClasses.CommonMethods.MessageBoxShow("NO RIGHTS TO ACCESS THE MASTERS", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    return;
                }
                grdFrame.Visibility = Visibility.Hidden;
                GridSubMenu.Visibility = Visibility.Visible;
                CommonVariable.ItemType = "MASTER";
                this.frmPage.Navigate((Uri)null);

                for (int J = 0; J < GridSubMenu.RowDefinitions.Count; J++)
                {
                    if (ControlsCount != 4)   //how many Controls in Grid
                    {
                        for (int i = 0; i < GridSubMenu.ColumnDefinitions.Count; i++)
                        {
                            if (i == 0 && J == 0)
                            {
                                Grid g = new Grid();
                                Button obj = new Button();
                                obj.Content = "LINE MASTER";
                                obj.Height = 100;
                                obj.Width = 230;
                                obj.Style = (Style)FindResource("SubMenuButton");
                                Grid.SetColumn(obj, i);
                                Grid.SetRow(obj, J);
                                GridSubMenu.Children.Add(obj);
                                ControlsCount = ControlsCount + 1;
                                // obj.Click += GroupMaster_Click;
                                if (CommonClasses.CommonVariable.Rights.Contains("LINE MASTER"))
                                    obj.Click += LineMaster_Click;
                                else
                                {
                                    obj.Click -= LineMaster_Click;
                                    obj.ToolTip = "Access Denied";
                                }
                            }
                            if (i == 1 && J == 0)
                            {
                                Grid g = new Grid();
                                Button obj = new Button();
                                obj.Content = "USER MASTER";
                                obj.Height = 100;
                                obj.Width = 230;
                                obj.Style = (Style)FindResource("SubMenuButton");
                                Grid.SetColumn(obj, i);
                                Grid.SetRow(obj, J);
                                GridSubMenu.Children.Add(obj);
                                ControlsCount = ControlsCount + 1;

                                if (CommonClasses.CommonVariable.Rights.Contains("USER MASTER"))
                                {
                                    obj.Click += UserMaster_Click;
                                }
                                else
                                {
                                    obj.Click -= UserMaster_Click;
                                    obj.ToolTip = "Access Denied";
                                }
                            }
                            if (i == 2 && J == 0)
                            {
                                Grid g = new Grid();
                                Button obj = new Button();
                                obj.Content = "GROUP MASTER";
                                obj.Height = 100;
                                obj.Width = 230;
                                obj.Style = (Style)FindResource("SubMenuButton");
                                Grid.SetColumn(obj, i);
                                Grid.SetRow(obj, J);
                                GridSubMenu.Children.Add(obj);
                                ControlsCount = ControlsCount + 1;
                               // obj.Click += GroupMaster_Click;
                                if (CommonClasses.CommonVariable.Rights.Contains("GROUP MASTER"))
                                    obj.Click += GroupMaster_Click;
                                else
                                {
                                    obj.Click -= GroupMaster_Click;
                                    obj.ToolTip = "Access Denied";
                                }
                            }
                            if (i == 3 && J == 0)
                            {
                                Grid g = new Grid();
                                Button obj = new Button();
                                obj.Content = "MODEL MASTER";
                                obj.Height = 100;
                                obj.Width = 230;
                                obj.Style = (Style)FindResource("SubMenuButton");
                                Grid.SetColumn(obj, i);
                                Grid.SetRow(obj, J);
                                GridSubMenu.Children.Add(obj);
                                ControlsCount = ControlsCount + 1;
                                // obj.Click += GroupMaster_Click;
                                if (CommonClasses.CommonVariable.Rights.Contains("MODEL MASTER"))
                                    obj.Click += ModelMaster_Click;
                                else
                                {
                                    obj.Click -= ModelMaster_Click;
                                    obj.ToolTip = "Access Denied";
                                }
                            }

                           

                            if (i == 4 && J == 0)
                            {
                                Grid g = new Grid();
                                Button obj = new Button();
                                obj.Content = "SETTINGS";
                                obj.Height = 100;
                                obj.Width = 230;
                                obj.Style = (Style)FindResource("SubMenuButton");
                                Grid.SetColumn(obj, i);
                                Grid.SetRow(obj, J);
                                GridSubMenu.Children.Add(obj);
                                ControlsCount = ControlsCount + 1;
                                // obj.Click += GroupMaster_Click;
                                if (CommonClasses.CommonVariable.Rights.Contains("SETTINGS"))
                                    obj.Click += Settings_Click;
                                else
                                {
                                    obj.Click -= Settings_Click;
                                    obj.ToolTip = "Access Denied";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void ModelMaster_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CommonVariable.PlantCode == "")
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE SELECT PLANT CODE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    return;
                }
                grdFrame.Visibility = Visibility.Visible;
                GridSubMenu.Visibility = Visibility.Hidden;
                CommonVariable.PageOpenClose = "Open";
                this.Background = (Brush)Brushes.White;
                this.frmPage.Navigate(new Uri("/Masters/ModelMaster.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CommonVariable.PlantCode == "")
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE SELECT PLANT CODE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    return;
                }
                grdFrame.Visibility = Visibility.Visible;
                GridSubMenu.Visibility = Visibility.Hidden;
                CommonVariable.PageOpenClose = "Open";
                this.Background = (Brush)Brushes.White;
                this.frmPage.Navigate(new Uri("/Masters/Settings.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void LineMaster_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grdFrame.Visibility = Visibility.Visible;
                GridSubMenu.Visibility = Visibility.Hidden;
                CommonVariable.PageOpenClose = "Open";
                this.Background = (Brush)Brushes.White;
                this.frmPage.Navigate(new Uri("/Masters/LineMaster.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

       
        private void GroupMaster_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grdFrame.Visibility = Visibility.Visible;
                GridSubMenu.Visibility = Visibility.Hidden;
                CommonVariable.PageOpenClose = "Open";
                this.Background = (Brush)Brushes.White;
                this.frmPage.Navigate(new Uri("/Masters/GroupMaster.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }

        }

        private void UserMaster_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CommonVariable.PlantCode == "")
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE SELECT PLANT CODE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    return;
                }

                grdFrame.Visibility = Visibility.Visible;
                GridSubMenu.Visibility = Visibility.Hidden;
                CommonVariable.PageOpenClose = "Open";
                this.Background = (Brush)Brushes.White;
                this.frmPage.Navigate(new Uri("/Masters/UserMaster.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }

        }

        private void BtnTransport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int ControlsCount = 0;
                GridSubMenu.Children.RemoveRange(0, 9);
             
                grdFrame.Visibility = Visibility.Hidden;
                GridSubMenu.Visibility = Visibility.Visible;
                CommonVariable.PageOpenClose = "Open";
                CommonVariable.ItemType = "TRANSACTION";

                this.frmPage.Navigate((Uri)null);

                for (int J = 0; J < GridSubMenu.RowDefinitions.Count; J++)
                {
                    if (ControlsCount != 3)   //how many Controls in Grid
                    {
                        for (int i = 0; i < GridSubMenu.ColumnDefinitions.Count; i++)
                        {
                            if (i == 0 && J == 0)
                            {
                                //Grid g = new Grid();
                                //Button obj = new Button();
                                //obj.Content = "LABEL PRINTING";
                                //obj.Height = 100;
                                //obj.Width = 230;
                                //obj.Style = (Style)FindResource("SubMenuButton");
                                //Grid.SetColumn(obj, i);
                                //Grid.SetRow(obj, J);
                                //GridSubMenu.Children.Add(obj);
                                //ControlsCount = ControlsCount + 1;

                                //if (CommonClasses.CommonVariable.Rights.Contains("LABEL PRINTING"))
                                //{
                                //    obj.Click += LabelPrinting_Click;
                                //}
                                //else
                                //{
                                //    obj.Click -= LabelPrinting_Click;
                                //    obj.ToolTip = "Access Denied";
                                //}

                                Grid g = new Grid();
                                Button obj = new Button();
                                obj.Content = "PROTOTYPE";
                                obj.Height = 100;
                                obj.Width = 230;
                                obj.Style = (Style)FindResource("SubMenuButton");
                                Grid.SetColumn(obj, i);
                                Grid.SetRow(obj, J);
                                GridSubMenu.Children.Add(obj);
                                ControlsCount = ControlsCount + 1;
                                obj.Click += MnuPrototype_Click;
                               

                            }
                            if (i == 1 && J == 0)
                            {
                             
                                Grid g = new Grid();
                                Button obj = new Button();
                                obj.Content = "PRODUCTION";
                                obj.Height = 100;
                                obj.Width = 230;
                                obj.Style = (Style)FindResource("SubMenuButton");
                                Grid.SetColumn(obj, i);
                                Grid.SetRow(obj, J);
                                GridSubMenu.Children.Add(obj);
                                ControlsCount = ControlsCount + 1;

                                obj.Click += MnuProduction_Click;


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }


        private void MnuProduction_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                this.GridSubMenu.Children.RemoveRange(0, 20);
                if (CommonClasses.CommonVariable.Rights == "" || CommonClasses.CommonVariable.Rights == null)
                {
                    CommonClasses.CommonMethods.MessageBoxShow("NO RIGHTS TO ACCESS THE TRANSACTION", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    return;
                }

                grdFrame.Visibility = Visibility.Hidden;
                GridSubMenu.Visibility = Visibility.Visible;

                Rights = "";
                if (CommonVariable.PageOpenClose == "Open")
                {

                    Password_Entry objPAS = new Password_Entry();
                    objPAS.ShowDialog();
                    if (Password_Entry.Password != "")
                    {
                        ENTITY_LAYER.Entity_Layer.Entity_Layer.Password = Password_Entry.Password;
                        //ENTITY_LAYER.Entity_Layer.Entity_Layer.Password = Interaction.InputBox("SECURITY", "ENTER PASSWORD", XPos: 500, YPos: 300);
                        Transaction("GetRights");

                        if (!Rights.Contains("PRODUCTION ACCESS PASSWORD"))
                        {
                            CommonClasses.CommonMethods.MessageBoxShow("INVALID PASSWORD", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                            BtnTransport_Click(null, null);
                            return;
                        }

                    }
                    else
                    {
                        BtnTransport_Click(null, null);
                        return;
                    }
                }

                CommonVariable.ItemType = "PRODUCTION";
                CommonVariable.PageOpenClose = "Open";

                for (int index1 = 0; index1 < this.GridSubMenu.RowDefinitions.Count; ++index1)
                {
                    int num = 0;
                    if (num != 5)
                    {
                        for (int index2 = 0; index2 < this.GridSubMenu.ColumnDefinitions.Count; ++index2)
                        {
                            if (index2 == 0 && index1 == 0)
                            {
                                Button element = new Button();
                                element.Content = (object)"PRODUCTION AUTO PRINT";
                                element.Style = (Style)null;
                                element.Style = (Style)this.FindResource((object)"SubMenuButton");
                                Grid.SetColumn((UIElement)element, index2);
                                Grid.SetRow((UIElement)element, index1);
                                this.GridSubMenu.Children.Add((UIElement)element);
                                ++num;
                                if (CommonVariable.Rights.Contains("PRODUCTION AUTO PRINT"))
                                {
                                    element.Click += new RoutedEventHandler(this.AutoPrinting_Click);
                                }
                                else
                                {
                                    element.Click -= new RoutedEventHandler(this.AutoPrinting_Click);
                                    element.ToolTip = (object)"Access Denied";
                                }
                            }
                            if (index2 == 1 && index1 == 0)
                            {
                                Button element = new Button();
                                element.Content = (object)"PRODUCTION MANUAL PRINT";
                                element.Style = (Style)null;
                                element.Style = (Style)this.FindResource((object)"SubMenuButton");
                                Grid.SetColumn((UIElement)element, index2);
                                Grid.SetRow((UIElement)element, index1);
                                this.GridSubMenu.Children.Add((UIElement)element);
                                ++num;
                                if (CommonVariable.Rights.Contains("PRODUCTION MANUAL PRINT"))
                                {
                                    element.Click += new RoutedEventHandler(this.ManualPrinting_Click);
                                }
                                else
                                {
                                    element.Click -= new RoutedEventHandler(this.ManualPrinting_Click);
                                    element.ToolTip = (object)"Access Denied";
                                }
                            }

                            if (index2 == 2 && index1 == 0)
                            {
                                Button element = new Button();
                                element.Content = (object)"PRODUCTION RE-PRINT";
                                element.Style = (Style)null;
                                element.Style = (Style)this.FindResource((object)"SubMenuButton");
                                Grid.SetColumn((UIElement)element, index2);
                                Grid.SetRow((UIElement)element, index1);
                                this.GridSubMenu.Children.Add((UIElement)element);
                                ++num;
                                if (CommonVariable.Rights.Contains("PRODUCTION RE-PRINT"))
                                {
                                    element.Click += new RoutedEventHandler(this.Reprinting_Click);
                                }
                                else
                                {
                                    element.Click -= new RoutedEventHandler(this.Reprinting_Click);
                                    element.ToolTip = (object)"Access Denied";
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void MnuPrototype_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                this.GridSubMenu.Children.RemoveRange(0, 20);
                if (CommonClasses.CommonVariable.Rights == "" || CommonClasses.CommonVariable.Rights == null)
                {
                    CommonClasses.CommonMethods.MessageBoxShow("NO RIGHTS TO ACCESS THE TRANSACTION", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    return;
                }

                grdFrame.Visibility = Visibility.Hidden;
                GridSubMenu.Visibility = Visibility.Visible;

                if (CommonVariable.PageOpenClose == "Open")
                {
                    Password_Entry objPAS = new Password_Entry();
                    objPAS.ShowDialog();
                    if (Password_Entry.Password != "")
                    {
                        ENTITY_LAYER.Entity_Layer.Entity_Layer.Password = Password_Entry.Password;
                        //ENTITY_LAYER.Entity_Layer.Entity_Layer.Password = Interaction.InputBox("SECURITY", "ENTER PASSWORD", XPos: 500, YPos: 300);
                        Transaction("GetRights");

                        if (!Rights.Contains("PROTOTYPE ACCESS PASSWORD"))
                        {
                            CommonClasses.CommonMethods.MessageBoxShow("INVALID PASSWORD", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                            BtnTransport_Click(null, null);
                            return;
                        }
                    }
                    else
                    {
                        BtnTransport_Click(null, null);
                        return;
                    }
                }
                CommonVariable.ItemType = "PROTOTYPE";
                CommonVariable.PageOpenClose = "Open";
                for (int index1 = 0; index1 < this.GridSubMenu.RowDefinitions.Count; ++index1)
                {
                    int num = 0;
                    if (num != 5)
                    {
                        for (int index2 = 0; index2 < this.GridSubMenu.ColumnDefinitions.Count; ++index2)
                        {
                            if (index2 == 0 && index1 == 0)
                            {
                                Button element = new Button();
                                element.Content = (object)"PROTOTYPE AUTO PRINT";
                                element.Style = (Style)null;
                                element.Style = (Style)this.FindResource((object)"SubMenuButton");
                                Grid.SetColumn((UIElement)element, index2);
                                Grid.SetRow((UIElement)element, index1);
                                this.GridSubMenu.Children.Add((UIElement)element);
                                ++num;
                                if (CommonVariable.Rights.Contains("PROTOTYPE AUTO PRINT"))
                                {
                                    element.Click += new RoutedEventHandler(this.AutoPrinting_Click);
                                }
                                else
                                {
                                    element.Click -= new RoutedEventHandler(this.AutoPrinting_Click);
                                    element.ToolTip = (object)"Access Denied";
                                }
                            }
                            if (index2 == 1 && index1 == 0)
                            {
                                Button element = new Button();
                                element.Content = (object)"PROTOTYPE MANUAL PRINT";
                                element.Style = (Style)null;
                                element.Style = (Style)this.FindResource((object)"SubMenuButton");
                                Grid.SetColumn((UIElement)element, index2);
                                Grid.SetRow((UIElement)element, index1);
                                this.GridSubMenu.Children.Add((UIElement)element);
                                ++num;
                                if (CommonVariable.Rights.Contains("PROTOTYPE MANUAL PRINT"))
                                {
                                    element.Click += new RoutedEventHandler(this.ManualPrinting_Click);
                                }
                                else
                                {
                                    element.Click -= new RoutedEventHandler(this.ManualPrinting_Click);
                                    element.ToolTip = (object)"Access Denied";
                                }
                            }

                            if (index2 == 2 && index1 == 0)
                            {
                                Button element = new Button();
                                element.Content = (object)"PROTOTYPE RE-PRINT";
                                element.Style = (Style)null;
                                element.Style = (Style)this.FindResource((object)"SubMenuButton");
                                Grid.SetColumn((UIElement)element, index2);
                                Grid.SetRow((UIElement)element, index1);
                                this.GridSubMenu.Children.Add((UIElement)element);
                                ++num;
                                if (CommonVariable.Rights.Contains("PROTOTYPE RE-PRINT"))
                                {
                                    element.Click += new RoutedEventHandler(this.Reprinting_Click);
                                }
                                else
                                {
                                    element.Click -= new RoutedEventHandler(this.Reprinting_Click);
                                    element.ToolTip = (object)"Access Denied";
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
        private void AutoPrinting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CommonVariable.PlantCode == "")
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE SELECT PLANT CODE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    return;
                }

                grdFrame.Visibility = Visibility.Visible;
                GridSubMenu.Visibility = Visibility.Hidden;
                CommonVariable.PageOpenClose = "Open";
                this.Background = (Brush)Brushes.White;
                this.frmPage.Navigate(new Uri("/Transaction/AutoPrinting.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void ManualPrinting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CommonVariable.PlantCode == "")
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE SELECT PLANT CODE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    return;
                }

               
                Password_Entry objPAS = new Password_Entry();
                Password_Entry.Password = "";
                objPAS.ShowDialog();
                if (Password_Entry.Password != "")
                {
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.Password = Password_Entry.Password;
                    //ENTITY_LAYER.Entity_Layer.Entity_Layer.Password = Interaction.InputBox("SECURITY", "ENTER PASSWORD", XPos: 500, YPos: 300);
                    Transaction("GetRights");

                    if (!Rights.Contains("MANUAL PRINT ACCESS PASSWORD"))
                    {
                        CommonClasses.CommonMethods.MessageBoxShow("INVALID PASSWORD", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                        if (CommonClasses.CommonVariable.ItemType == "PRODUCTION")
                            MnuProduction_Click(null, null);
                        if (CommonClasses.CommonVariable.ItemType == "PROTOTYPE")
                            MnuPrototype_Click(null, null);
                        return;
                    }
                    
                }
                else
                {
                    if (CommonClasses.CommonVariable.ItemType == "PRODUCTION")
                        MnuProduction_Click(null, null);
                    if (CommonClasses.CommonVariable.ItemType == "PROTOTYPE")
                        MnuPrototype_Click(null, null);
                    return;
                }

                grdFrame.Visibility = Visibility.Visible;
                GridSubMenu.Visibility = Visibility.Hidden;
                CommonVariable.PageOpenClose = "Open";
                this.Background = (Brush)Brushes.White;
                this.frmPage.Navigate(new Uri("/Transaction/ManualPrinting.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
        private void Reprinting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CommonVariable.PlantCode == "")
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE SELECT PLANT CODE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    return;
                }

                
                Password_Entry objPAS = new Password_Entry();
                Password_Entry.Password = "";
                objPAS.ShowDialog();
                if (Password_Entry.Password != "")
                {
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.Password = Password_Entry.Password;
                    //ENTITY_LAYER.Entity_Layer.Entity_Layer.Password = Interaction.InputBox("SECURITY", "ENTER PASSWORD", XPos: 500, YPos: 300);
                    Transaction("GetRights");

                    if (!Rights.Contains("REPRINT ACCESS PASSWORD"))
                    {
                        CommonClasses.CommonMethods.MessageBoxShow("INVALID PASSWORD", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                        if (CommonClasses.CommonVariable.ItemType == "PRODUCTION")
                            MnuProduction_Click(null, null);
                        if (CommonClasses.CommonVariable.ItemType == "PROTOTYPE")
                            MnuPrototype_Click(null, null);
                        return;
                    }
                }
                else
                {
                    if (CommonClasses.CommonVariable.ItemType == "PRODUCTION")
                        MnuProduction_Click(null, null);
                    if (CommonClasses.CommonVariable.ItemType == "PROTOTYPE")
                        MnuPrototype_Click(null, null);
                    return;
                }

                grdFrame.Visibility = Visibility.Visible;
                GridSubMenu.Visibility = Visibility.Hidden;
                CommonVariable.PageOpenClose = "Open";
                this.Background = (Brush)Brushes.White;
                this.frmPage.Navigate(new Uri("/Transaction/Reprint.xaml", UriKind.RelativeOrAbsolute));

            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int ControlsCount = 0;
                GridSubMenu.Children.RemoveRange(0, 9);

             
                if (CommonClasses.CommonVariable.Rights == "" || CommonClasses.CommonVariable.Rights == null)
                {
                    CommonClasses.CommonMethods.MessageBoxShow("NO RIGHTS TO ACCESS THE REPORTS", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    return;
                }
                grdFrame.Visibility = Visibility.Hidden;
                GridSubMenu.Visibility = Visibility.Visible;
                this.frmPage.Navigate((Uri)null);

                for (int J = 0; J < GridSubMenu.RowDefinitions.Count; J++)
                {
                    if (ControlsCount != 3)   //how many Controls in Grid
                    {
                        for (int i = 0; i < GridSubMenu.ColumnDefinitions.Count; i++)
                        {
                            if (i == 0 && J == 0)
                            {
                                Grid g = new Grid();
                                Button obj = new Button();
                                obj.Content = "PRINTING REPORT";
                                obj.Height = 100;
                                obj.Width = 230;
                                obj.Style = (Style)FindResource("SubMenuButton");
                                Grid.SetColumn(obj, i);
                                Grid.SetRow(obj, J);
                                GridSubMenu.Children.Add(obj);
                                ControlsCount = ControlsCount + 1;

                                if (CommonClasses.CommonVariable.Rights.Contains("PRINTING REPORT"))
                                {
                                    obj.Click += Printing_Click;
                                }
                                else
                                {
                                    obj.Click -= Printing_Click;
                                    obj.ToolTip = "Access Denied";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void Printing_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CommonVariable.PlantCode == "")
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE SELECT PLANT CODE", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    return;
                }

                grdFrame.Visibility = Visibility.Visible;
                GridSubMenu.Visibility = Visibility.Hidden;
                CommonVariable.PageOpenClose = "Open";
                this.Background = (Brush)Brushes.White;
                this.frmPage.Navigate(new Uri("/Reports/PrintingReport.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        #endregion

        private void Grid_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Process.GetProcessesByName("DAIKIN_PRINTING_SYSTEM")[0].Kill();
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void CmbPlant_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(cmbPlant.SelectedIndex>-1)
                {
                    DataRowView dr = (DataRowView)cmbPlant.SelectedItem;
                    cmbPlant.Text = dr[2].ToString();
                    StreamWriter SW = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Plant.txt", false);
                    SW.Write(cmbPlant.Text.ToString());
                    CommonClasses.CommonVariable.PlantCode = cmbPlant.Text.ToString();
                    CommonClasses.CommonVariable.PlantName = cmbPlant.SelectedValue.ToString();
                    SW.Dispose();
                    SW.Close();
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAIN_WINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
    }
}

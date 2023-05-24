
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
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

namespace DAIKIN_PRINTING_SYSTEM.StartUp
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        #region Variable and Objects
        BUSINESS_LAYER.Business_Layer.Business_Layer obj_BL = new BUSINESS_LAYER.Business_Layer.Business_Layer();
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        #endregion

        #region Methods
        private void ShowCapslock()
        {
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void ValidateLogin()
        {
            ENTITY_LAYER.Entity_Layer.Entity_Layer.UserID = txtUserID.Text;
            ENTITY_LAYER.Entity_Layer.Entity_Layer.Password = txtPassword.Password;
            ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = "Login";
            CommonClasses.CommonVariable.Result = obj_BL.BL_Login();
            if (CommonClasses.CommonVariable.Result.StartsWith("VALID CREDENTIAL"))
            {
                CommonClasses.CommonVariable.UserID = txtUserID.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.LoginID = txtUserID.Text;
                CommonClasses.CommonVariable.UserName = CommonClasses.CommonVariable.Result.Split('+')[1].ToString();
                CommonClasses.CommonVariable.Rights = CommonClasses.CommonVariable.Result.Split('+')[2].ToString();
                StartUp.MainWindow objMainWindow = new MainWindow();
                this.Close();
                objMainWindow.ShowDialog();
            }
            else if (CommonClasses.CommonVariable.Result == "INVALID USER ID")
            {
                CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.Result, CustomMessageBox.CustomStriing.Exclamatory.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtUserID.Text = "";
                txtUserID.Focus();
            }
            else if (CommonClasses.CommonVariable.Result == "INVALID PASSWORD")
            {
                CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.Result, CustomMessageBox.CustomStriing.Exclamatory.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtPassword.Password = "";
                txtPassword.Focus();
            }
            else if (CommonClasses.CommonVariable.Result == "FIRST TIME LOGIN")
            {
                if ((txtUserID.Text.ToUpper() == "SARBLR") && (txtPassword.Password.ToUpper() == "SARBLR"))
                {
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.UserName = "SARBLR";
                    CommonClasses.CommonVariable.Rights = "USER MASTER,GROUP MASTER";
                    CommonClasses.CommonVariable.UserID = "SARBLR";
                    StartUp.MainWindow objMainWindow = new MainWindow();
                    this.Close();
                    objMainWindow.ShowDialog();
                }
                else
                {
                    CommonClasses.CommonMethods.MessageBoxShow("FIRST TIME LOGIN. PLEASE ENTER VALID USER ID OR PASSWORD", CustomMessageBox.CustomStriing.Exclamatory.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    return;
                }
            }
            else
            {
                CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.Result, CustomMessageBox.CustomStriing.Exclamatory.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                txtUserID.Focus();
            }
        }
        #endregion

        #region Events
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Boolean Capslock = Console.CapsLock;
            if (txtPassword.IsFocused == true)
            {
                if (Capslock == true)
                    txtPasswordPopup.IsOpen = true;
                else
                    txtPasswordPopup.IsOpen = false;
            }
            else
            {
                txtPasswordPopup.IsOpen = false;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                txtUserID.Focus();
                // GetDriverList();
                Version Version = Assembly.GetExecutingAssembly().GetName().Version;
                txtVersion.Text = String.Format(this.txtVersion.Text, Version.Major, Version.Minor, Version.Build, Version.Revision);
                ShowCapslock();

                //  BarCode barcode = new BarCode();
                //  barcode.Symbology = KeepAutomation.Barcode.Symbology.QRCode;
                //  barcode.CodeToEncode = "12 T150YEBA2473 89704-0DG60-00 00002/00002,T150Y,Y,PTD-12-1,2019070202,XD17,8,EB,08,,PC   -  07";
                //  barcode.X = 2;
                //  barcode.Y = 2;
                //  //barcode.TopMargin = 10;
                //  //barcode.BottomMargin = 10;
                //  //barcode.LeftMargin = 10;
                //  //barcode.RightMargin = 10;
                //  //barcode.DisplayText = true;
                //  //barcode.DisplayStartStop = true;
                ////  barcode.BarcodeUnit = KeepAutomation.Barcode.BarcodeUnit.Pixel;
                //  barcode.Orientation = KeepAutomation.Barcode.Orientation.Degree270;
                // // barcode.DPI = 72;
                //  //barcode.
                //  barcode.ImageFormat = ImageFormat.Jpeg;
                //  barcode.generateBarcodeToImageFile("D://barcode-upca-csharp.bmp");
                //byte[] asarry=  barcode.generateBarcodeToByteArray();
                //Barcode qrcode = new Barcode();

                //qrcode.Data = "12 T150YEBA2473 89704-0DG60-00 00001/00002,T150Y,Y,PTD-12-1,2019070202,XD17,8,EB,08,,PC   -  07";
                //qrcode.BarType = BarCodeType.QRCode;
                //qrcode.QRCodeECL = ErrorCorrectionLevelMode.L;
                //qrcode.Width = 300;
                //qrcode.Height = 300;
                
                //qrcode.CreateBarcode("qrcode-winforms.jpg");

            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LOGIN", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtUserID.Text == "")
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER THE USER ID", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    txtUserID.Focus();
                    return;
                }
                if (txtPassword.Password == "")
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER THE PASSWORD", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    txtPassword.Focus();
                    return;
                }
                ValidateLogin();
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LOGIN", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();

            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LOGIN", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void LinkForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtUserID.Text == "")
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER THE USER ID", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    txtUserID.Focus();
                    return;
                }
                ENTITY_LAYER.Entity_Layer.Entity_Layer.UserID = txtUserID.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = "GetRights";
                CommonClasses.CommonVariable.Result = obj_BL.BL_Login();
                if (CommonClasses.CommonVariable.Result.Contains("FORGOT PASSWORD"))
                {
                    this.Hide();
                    ForgotPassword objForgotPassword = new ForgotPassword();
                    objForgotPassword.ShowDialog();
                }
                else
                    CommonClasses.CommonMethods.MessageBoxShow("ACCESS DENIED", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LOGIN", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void LinkChangePassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtUserID.Text == "")
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER THE USER ID", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    txtUserID.Focus();
                    return;
                }

                ENTITY_LAYER.Entity_Layer.Entity_Layer.UserID = txtUserID.Text;
                ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = "GetRights";
                CommonClasses.CommonVariable.Result = obj_BL.BL_Login();
                if (CommonClasses.CommonVariable.Result.Contains("CHANGE PASSWORD"))
                {
                    this.Hide();
                    ChangePassword objChangePassword = new ChangePassword();
                    objChangePassword.ShowDialog();
                }
                else
                    CommonClasses.CommonMethods.MessageBoxShow("ACCESS DENIED", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "LOGIN", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.L) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.L))
            {
                btnLogin_Click(sender, e);
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.E) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.E))
            {
                btnExit_Click(sender, e);
            }
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                btnLogin_Click(sender, e);
        }
        #endregion

    }
   
}

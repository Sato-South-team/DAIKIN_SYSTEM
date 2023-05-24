using DAIKIN_PRINTING_SYSTEM.CommonClasses;
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

namespace DAIKIN_PRINTING_SYSTEM.StartUp
{
    /// <summary>
    /// Interaction logic for Password_Entry.xaml
    /// </summary>
    public partial class Password_Entry : Window
    {
        public Password_Entry()
        {
            InitializeComponent();
            txtPassword.Focus();
        }
        public static string  Password="";

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(txtPassword.Password=="")
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLEASE ENTER PASSWORD", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    txtPassword.Focus();
                    return;
                }

                if (txtPassword.Password != "")
                {
                    Password = txtPassword.Password;
                    txtPassword.Password = "";
                }
                this.Close();
            }
            catch (Exception ex)
            {
                CommonClasses.CommonMethods.CreatLogDetails(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PASSWORD_BOX", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.O) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.O))
                {
                    BtnOK_Click(sender, e);
                }
                if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.C) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.C))
                {
                    BtnCancel_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                CommonClasses.CommonMethods.CreatLogDetails(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PASSWORD_BOX", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void TxtPassword_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key==Key.Enter)
            {
                //BtnOK_Click(sender, e);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CommonVariable.PageOpenClose = "Close";
                this.Close();
            }
            catch (Exception ex)
            {
                CommonClasses.CommonMethods.CreatLogDetails(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "PASSWORD_BOX", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }
    }
}

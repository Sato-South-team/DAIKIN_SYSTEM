﻿using System;
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
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Window
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }
        #region Variables and Objects
        BUSINESS_LAYER.Business_Layer.Business_Layer obj_BL = new BUSINESS_LAYER.Business_Layer.Business_Layer();
        #endregion

        #region Methods
        private void Clear()
        {
            txtUserID.Text = "";
            txtPassword.Text = "YOUR PASSWORD IS ?";
            txtUserID.Focus();
        }
        #endregion

        #region Events
        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtUserID.Text == "")
                {
                    CommonClasses.CommonMethods.MessageBoxShow("PLASE ENTER USER ID", CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    txtUserID.Focus();
                }
                else
                {
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.UserID = txtUserID.Text;
                    ENTITY_LAYER.Entity_Layer.Entity_Layer.Type = "ForgotPassword";
                    CommonClasses.CommonVariable.Result = obj_BL.BL_Login();
                    if (CommonClasses.CommonVariable.Result.StartsWith("YOUR PASSOWRD IS"))
                    {
                        txtPassword.Text = "YOUR PASSWORD IS " + CommonClasses.CommonVariable.Result.Split('+')[1].ToString();
                        txtUserID.Focus();
                    }
                    else
                    {
                        CommonClasses.CommonMethods.MessageBoxShow(CommonClasses.CommonVariable.Result, CustomMessageBox.CustomStriing.Information.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                        txtUserID.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "FORGOT_PASSWORD", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StartUp.Login obj_BLin = new Login();
                this.Close();
                obj_BLin.ShowDialog();
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "FORGOT_PASSWORD", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                txtUserID.Focus();
            }
            catch (Exception ex)
            {
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "FORGOT_PASSWORD", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void txtUserID_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                btnShow_Click(sender, e);
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
                obj_BL.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "FORGOT_PASSWORD", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.S))
            {
                btnShow_Click(sender, e);
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.C) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.C))
            {
                btnClear_Click(sender, e);
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.E) || Keyboard.IsKeyDown(Key.RightAlt) && Keyboard.IsKeyDown(Key.E))
            {
                btnExit_Click(sender, e);
            }
        }
        #endregion

    }
}

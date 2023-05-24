using DAIKIN_PRINTING_SYSTEM.StartUp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DAIKIN_PRINTING_SYSTEM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        BUSINESS_LAYER.Business_Layer.Business_Layer obj_Log = new BUSINESS_LAYER.Business_Layer.Business_Layer();


        private void StartUP(object sender, StartupEventArgs e)
        {
            try
            {
                bool Running;

                using (Mutex mutex = new Mutex(true, "DAIKIN_PRINTING_SYSTEM", out Running))
                {
                    if (Running)
                    {

                        string data = ConfigurationManager.AppSettings["ConnectionString"].ToString();

                        if (data != "")
                        {
                            string[] DataSplit = data.Split(',');
                            if (DataSplit.Length > 0)
                            {
                                ENTITY_LAYER.Entity_Layer.Entity_Layer.SqldbServer = DataSplit[0].ToString();
                                ENTITY_LAYER.Entity_Layer.Entity_Layer.SqlDBUserID = DataSplit[1].ToString();
                                ENTITY_LAYER.Entity_Layer.Entity_Layer.SqlDBPassword = DataSplit[2].ToString();
                                ENTITY_LAYER.Entity_Layer.Entity_Layer.SqlDBName = DataSplit[3].ToString();
                                StartUp.Login obj_Login = new StartUp.Login();
                                obj_Login.ShowDialog();
                                // App.Current.MainWindow.Content = new StartUp.Login();
                            }
                            else
                            {
                                CommonClasses.CommonMethods.MessageBoxShow("INCORRECT DATABASE SETTING!!", CustomMessageBox.CustomStriing.Exclamatory.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

                            }
                        }
                        else
                        {
                            CommonClasses.CommonMethods.MessageBoxShow("PLEASE DO THE DATABASE SETTING!!", CustomMessageBox.CustomStriing.Exclamatory.ToString(), CustomMessageBox.CustomStriing.OK.ToString());

                        }

                    }
                    else
                    {
                        CommonClasses.CommonMethods.MessageBoxShow("APPLICATION IS ALREADY RUNNING!!!", CustomMessageBox.CustomStriing.Exclamatory.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAINWINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Grid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAINWINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

        private void Grid_MouseLeftButtonUp_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();

            }
            catch (Exception ex)
            {
                obj_Log.CreateLog(ex.Message.ToString(), MethodBase.GetCurrentMethod().ToString(), "MAINWINDOW", CommonClasses.CommonVariable.UserID);
                CommonClasses.CommonMethods.MessageBoxShow(ex.Message.ToString(), CustomMessageBox.CustomStriing.Error.ToString(), CustomMessageBox.CustomStriing.OK.ToString());
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIKIN_PRINTING_SYSTEM.CommonClasses
{
 public class CommonVariable
    {
        #region Common_Variables
        public static string DataSaved = "DATA SAVED SUCCESSFULLY";
        public static string DataDeleted = "DATA DELETED SUCCESSFULLY";
        public static string DataUpdated = "DATA UPDATED SUCCESSFULLY";
        public static string Duplicate = "DATA ALREADY EXIST";
        public static string DeleteConfirm = "DO YOU WANT TO DELETE SELECTED DATA?";
        public static string RowSelection = " PLEASE SELECT ROW FROM LIST VIEW FOR YOUR TRANSACTION!!!";
        public static string UserID = "";
        public static string UserName = "";
        public static string UserType = "";
        public static string Rights = "";
        public static string PlantCode = "";
        public static string PlantName = "";
        public static string ItemType = "";
        public static string ChangeType = "";
        public static int RefNo = 0;
        public static string Result = "";
        public static string PageOpenClose = "";
        public static Microsoft.Office.Interop.Excel.Application xlApp1 = new Microsoft.Office.Interop.Excel.Application();

        #endregion
    }
}

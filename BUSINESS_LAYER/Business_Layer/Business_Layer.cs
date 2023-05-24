// Decompiled with JetBrains decompiler
// Type: BUSINESS_LAYER.Business_Layer.Business_Layer
// Assembly: BUSINESS_LAYER, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 327BC3B6-698A-4D56-863C-2DDBC39BF096
// Assembly location: D:\Sato\mold..xbap_4ffec223c589b33d_0001.0000_92cb2251cacf4555\BUSINESS_LAYER.dll

using DATA_LAYER.DatabaseConnectivity;
using System;
using System.Data;
using System.IO;
using System.Reflection;

namespace BUSINESS_LAYER.Business_Layer
{
    public class Business_Layer
    {
        private DatabaseConnections obj_DB = new DatabaseConnections();

        public void CreateLog(
          string ErrorDescription,
          string MethodName,
          string ModulName,
          string UserId)
        {
            try
            {
                this.obj_DB.ExecuteProcedureParam("Proc_LogDetails", (object)ErrorDescription, (object)MethodName, (object)ModulName, (object)UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BL_Login()
        {
            try
            {
                return this.obj_DB.ExecuteProcedureParam("Proc_Login", (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.UserID, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Password, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.ConfirmPassword, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Type);
            }
            catch (Exception ex)
            {
                //StreamWriter streamWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Log\\"  + DateTime.Now.ToString("dd-MM-yyyy") + ".txt", true);
                //streamWriter.WriteLine(ex.Message.ToString()+", " + DateTime.Now.ToString());
                //streamWriter.Dispose();
                //streamWriter.Close();
                throw ex;
            }
        }

        public string BL_GroupMasterTransaction()
        {
            try
            {
                return this.obj_DB.ExecuteProcedureParam("Proc_GroupMaster", (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.GroupID, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Rights, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Type, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.LoginID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet BL_GroupMasterDetails()
        {
            try
            {
                return this.obj_DB.ExecuteDataSetParam("Proc_GroupMaster", (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.GroupID, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Rights, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Type, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BL_UserMasterTransaction()
        {
            try
            {
                return this.obj_DB.ExecuteProcedureParam("Proc_UserMaster", (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.RefNo, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.UserID, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.UserName, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Password, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.GroupID, ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.LoginID, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet BL_UserMasterDetails()
        {
            try
            {
                return this.obj_DB.ExecuteDataSetParam("Proc_UserMaster", (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.RefNo, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.UserID, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.UserName, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Password, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.GroupID,ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.LoginID, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BL_ModelMasterTransaction()
        {
            try
            {
                return this.obj_DB.ExecuteProcedureParam("Proc_ModelMaster", (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.RefNo, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.ModelNo, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.ModelName, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Descriptions, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.AP, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.RefOil, ENTITY_LAYER.Entity_Layer.Entity_Layer.Ref, ENTITY_LAYER.Entity_Layer.Entity_Layer.ML, ENTITY_LAYER.Entity_Layer.Entity_Layer.PRN, ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode, ENTITY_LAYER.Entity_Layer.Entity_Layer.LoginID, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet BL_ModelMasterDetails()
        {
            try
            {
                return this.obj_DB.ExecuteDataSetParam("Proc_ModelMaster", (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.RefNo, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.ModelNo, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.ModelName, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Descriptions, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.AP, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.RefOil, ENTITY_LAYER.Entity_Layer.Entity_Layer.Ref, ENTITY_LAYER.Entity_Layer.Entity_Layer.ML, ENTITY_LAYER.Entity_Layer.Entity_Layer.PRN, ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode, ENTITY_LAYER.Entity_Layer.Entity_Layer.LoginID, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BL_LineMasterTransaction()
        {
            try
            {
                return this.obj_DB.ExecuteProcedureParam("Proc_LineMaster", (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.RefNo, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantName, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.LineNo, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.LineName, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Station,  (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.LoginID, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet BL_LineMasterDetails()
        {
            try
            {
                return this.obj_DB.ExecuteDataSetParam("Proc_LineMaster", (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.RefNo, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantName, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.LineNo,(object)ENTITY_LAYER.Entity_Layer.Entity_Layer.LineName, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Station,ENTITY_LAYER.Entity_Layer.Entity_Layer.LoginID, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BL_SettingsTransaction()
        {
            try
            {
                return this.obj_DB.ExecuteProcedureParam("Proc_IPMaster", (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.RefNo, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.LineNo, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Station, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Category, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.HardwareType, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.IP, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Port, ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode,  ENTITY_LAYER.Entity_Layer.Entity_Layer.LoginID, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet BL_SettingsDetails()
        {
            try
            {
                return this.obj_DB.ExecuteDataSetParam("Proc_IPMaster", (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.RefNo, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.LineNo, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Station, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Category, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.HardwareType, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.IP, ENTITY_LAYER.Entity_Layer.Entity_Layer.Port, ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode,  ENTITY_LAYER.Entity_Layer.Entity_Layer.LoginID, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet BL_Transaction()
        {
            try
            {
                return this.obj_DB.ExecuteDataSetParam("Proc_Transaction", (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Barcode, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.LineNo, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.ModelNo, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Station, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Type, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.TransactionType, ENTITY_LAYER.Entity_Layer.Entity_Layer.LoginID, ENTITY_LAYER.Entity_Layer.Entity_Layer.Process, ENTITY_LAYER.Entity_Layer.Entity_Layer.ItemType, ENTITY_LAYER.Entity_Layer.Entity_Layer.FromDate, ENTITY_LAYER.Entity_Layer.Entity_Layer.ToDate, ENTITY_LAYER.Entity_Layer.Entity_Layer.Remarks);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet BL_ReportDetails()
        {
            try
            {
                return this.obj_DB.ExecuteDataSetParam("Proc_Report", (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.PlantCode, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.FromDate, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.ToDate, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.LineNo, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Station, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.ModelNo, (object)ENTITY_LAYER.Entity_Layer.Entity_Layer.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

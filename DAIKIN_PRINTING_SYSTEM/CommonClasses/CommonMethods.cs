using Microsoft.Office.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DAIKIN_PRINTING_SYSTEM.CommonClasses
{
    class CommonMethods
    {
        #region Common_Methods
        public static string DataTableToString(DataTable dt1)
        {
            try
            {
                StringBuilder str = new StringBuilder();
                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    for (int k = 0; k < dt1.Columns.Count; k++)
                    {
                        str.AppendFormat("{0}:{1}$", dt1.Columns[k].ColumnName, dt1.Rows[j][k]);
                    }
                }
                return str.ToString();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable StringToDataTable(string str)
        {
            try
            {
                DataTable dt = new DataTable();
                // str = str.Replace("\n", "&");
                if (str != "")
                {
                    string[] str1 = str.Split('\n');

                    string[] str2 = str1[0].Split(',');
                    for (int i = 0; i < str2.Length; i++)
                    {
                        if (str2[i].ToString() != " ")
                        {
                            dt.Columns.Add(str2[i].Split(':')[0]);
                        }
                    }

                    for (int j = 1; j < str1.Length; j++)
                    {
                        string[] str3 = str1[j].Split(',');
                        if (str1[j].ToString() != "")
                        {
                            DataRow row = dt.NewRow();
                            for (int k = 0; k < str3.Length; k++)
                            {
                                if (str3[k] != "")
                                    row[k] = str3[k].Replace("\r","");
                            }
                            dt.Rows.Add(row);
                        }
                    }
                }

                return dt;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public static void DigitsOnly(TextCompositionEventArgs e)
        {
            try
            {
                if (e.Text != "")
                {
                    char c = Convert.ToChar(e.Text);
                    if (Char.IsNumber(c))
                        e.Handled = false;
                    else
                        e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void MessageBoxShow(string Description, string ErrorType, string Result)
        {
            try
            {
                StartUp.CustomMessageBox objCustomMsg = new StartUp.CustomMessageBox(Description, ErrorType, Result);
                objCustomMsg.ShowDialog(); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void FillComboBox(System.Windows.Controls.ComboBox ComboBox, DataTable dt, string DisPlayMember)
        {
            try
            {
                ComboBox.ItemsSource = null;
                ComboBox.DisplayMemberPath = "";
                ComboBox.ItemsSource = dt.DefaultView;
                ComboBox.DisplayMemberPath = DisPlayMember;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UNFill_ComboBox(System.Windows.Controls.ComboBox ComboBox)
        {
            try
            {
                ComboBox.ItemsSource = null;
                ComboBox.DisplayMemberPath = "";
                ComboBox.SelectedValuePath = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void NumericValue(TextCompositionEventArgs e)
        {
            try
            {
                if (e.Text != "")
                {
                    char c = Convert.ToChar(e.Text);
                    if (Char.IsNumber(c))
                        e.Handled = false;
                    else
                        e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void FillComboBox(System.Windows.Controls.ComboBox ComboBox, DataTable dt, string DisPlayMember, string ValueMember)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    ComboBox.ItemsSource = null;
                    ComboBox.DisplayMemberPath = "";
                    ComboBox.ItemsSource = dt.DefaultView;
                    ComboBox.DisplayMemberPath = DisPlayMember;
                    ComboBox.SelectedValuePath = ValueMember;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CreatLogDetails(string ErrorDescrription, string methodName, string ModuleName, string CreatedBy)
        {
            try
            {
                StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Log\\" + ModuleName + "-" + System.DateTime.Now.ToString("dd-MM-yyyy") + ".txt", true);
                sw.WriteLine(ErrorDescrription + " , " + methodName + " , " + ModuleName + " , " + CreatedBy + " , " + System.DateTime.Now.ToString());
                sw.Dispose();
                sw.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void CreatDataBaseLogDetails(string DBServerName, string DBSarverID, string DBServerPassword, string DataBaseName)
        {
            try
            {
                StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "DataBaseSetting.txt");
                sw.WriteLine("DBServerName" + "," + "DBSarverID" + "," + "DBServerPassword" + "," + "DataBaseName");
                sw.WriteLine(DBServerName + "," + DBSarverID + "," + DBServerPassword + "," + DataBaseName);
                sw.Dispose();
                sw.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static string ReadFile(string Path)
        {
            try
            {
                string Result = "";
                if (File.Exists(Path))
                {
                    StreamReader SR = new StreamReader(Path);
                    Result = SR.ReadToEnd();
                    SR.Dispose();
                    SR.Close();
                }
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string Encrypt_data(string str)
        {
            try
            {
                char[] arr = str.ToCharArray();
                Array.Reverse(arr);
                str = new string(arr);
                return Convert.ToBase64String(Encoding.Unicode.GetBytes(str));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string Decrypt_data(string str)
        {
            try
            {
                char[] arr = Encoding.Unicode.GetString(Convert.FromBase64String(str)).ToCharArray();
                Array.Reverse(arr);
                return new string(arr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool CreateExcellFile(DataTable dt, string FilePath, string SheetName, bool PDFExport, string HearderName)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application excel = CommonVariable.xlApp1; ;
                //Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook worKbooK;
                Microsoft.Office.Interop.Excel.Worksheet worKsheeT;
                Microsoft.Office.Interop.Excel.Range celLrangE;

                excel.DisplayAlerts = false;
                worKbooK = excel.Workbooks.Add(Type.Missing);

                worKsheeT = (Microsoft.Office.Interop.Excel.Worksheet)worKbooK.ActiveSheet;
                worKsheeT.Name = SheetName;

                worKsheeT.Cells.WrapText = true;
                worKsheeT.Cells.Font.Size = 8;
                if (PDFExport == true)
                {
                    worKsheeT.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape;
                    worKsheeT.PageSetup.PaperSize = Microsoft.Office.Interop.Excel.XlPaperSize.xlPaperA4;
                }
                int cell = 1;
                if (HearderName != "")
                {
                    cell = 2;

                    //   Margins margins = new Margins(100, 100, 100, 100);
                    //worKsheeT.PageSetup.RightMargin=100;
                    //worKsheeT.PageSetup.LeftMargin = 100;
                    worKsheeT.Range[worKsheeT.Cells[1, 1], worKsheeT.Cells[1, dt.Columns.Count]].Merge();
                    worKsheeT.Cells.HorizontalAlignment =
                    Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    worKsheeT.Cells[1, 1] = HearderName;
                    worKsheeT.Cells[1, 1].Font.Bold = true;
                    worKsheeT.Cells[1, 1].Font.Size = 20;
                    Microsoft.Office.Interop.Excel.Range oRange = (Microsoft.Office.Interop.Excel.Range)worKsheeT.Cells[1, 1];
                    oRange.RowHeight = 50;
                    oRange.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                    float Left = (float)((double)oRange.Left+5);
                    float Top = (float)((double)oRange.Top+5);
                    float Width = (float)((double)25);
                    float Height = (float)((double)25);
                    worKsheeT.Shapes.AddPicture(AppDomain.CurrentDomain.BaseDirectory + "Logo.png", MsoTriState.msoFalse, MsoTriState.msoCTrue, Left, Top, Width, Height);
                    worKsheeT.PageSetup.CenterFooter = "&P/&N";
                    //  worKsheeT.Range[1,10].Merge();
                }
                worKsheeT.Cells.NumberFormat = "@";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worKsheeT.Cells[cell, i + 1].NumberFormat = "@";
                    worKsheeT.Cells[cell, i + 1] = dt.Columns[i].ColumnName.ToString();
                    worKsheeT.Cells[cell, i + 1].Font.Bold = true;
                }

                cell = 2;
                //if (HearderName != "")
                //    cell = 2;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        worKsheeT.Cells[cell + 1, j + 1].NumberFormat = "@";
                        worKsheeT.Cells[cell + 1, j + 1] = dt.Rows[i][j].ToString();
                    }
                    cell++;
                }


                celLrangE = worKsheeT.Range[worKsheeT.Cells[1, 1], worKsheeT.Cells[dt.Rows.Count + 2, dt.Columns.Count]];
                celLrangE.EntireColumn.AutoFit();
                Microsoft.Office.Interop.Excel.Borders border = celLrangE.Borders;
                border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                if (PDFExport == false)
                {
                    worKsheeT.Range[worKsheeT.Cells[dt.Rows.Count + 4, 1], worKsheeT.Cells[dt.Rows.Count + 4, dt.Columns.Count]].Merge();

                    worKsheeT.Cells[dt.Rows.Count + 4, 1].HorizontalAlignment =
                 Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                    worKsheeT.Cells[dt.Rows.Count + 4, 1].NumberFormat = "@";
                    worKsheeT.Cells[dt.Rows.Count + 4, 1] = "GENERATED BY : " + CommonVariable.UserID;
                    //  worKsheeT.Cells[dt.Rows.Count + 3, 1].NumberFormat = "@";
                    //worKsheeT.Range[worKsheeT.Cells[dt.Rows.Count + 3, dt.Columns.Count - 1], worKsheeT.Cells[dt.Rows.Count + 3, 2]].Merge();


                    //worKsheeT.Cells[dt.Rows.Count + 3, dt.Columns.Count-1] = CommonVariable.UserID;


                    worKsheeT.Range[worKsheeT.Cells[dt.Rows.Count + 5, 1], worKsheeT.Cells[dt.Rows.Count + 5, dt.Columns.Count]].Merge();
                    worKsheeT.Cells[dt.Rows.Count + 5, 1].HorizontalAlignment =
                    Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                    worKsheeT.Cells[dt.Rows.Count + 5, 1].NumberFormat = "@";
                    worKsheeT.Cells[dt.Rows.Count + 5, 1] = "GENERATED ON : " + System.DateTime.Now.ToString("dd/MMM/yyyy HH:mm"); ;


                    worKbooK.SaveAs(FilePath);
                    excel.Visible = true;

                }
                else
                {
                    worKsheeT.Range[worKsheeT.Cells[dt.Rows.Count + 4, 1], worKsheeT.Cells[dt.Rows.Count + 4, dt.Columns.Count]].Merge();

                    worKsheeT.Cells[dt.Rows.Count + 4, 1].HorizontalAlignment =
                 Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                    worKsheeT.Cells[dt.Rows.Count + 4, 1].NumberFormat = "@";
                    worKsheeT.Cells[dt.Rows.Count + 4, 1] = "GENERATED BY : " + CommonVariable.UserID;
                    //  worKsheeT.Cells[dt.Rows.Count + 3, 1].NumberFormat = "@";
                    //worKsheeT.Range[worKsheeT.Cells[dt.Rows.Count + 3, dt.Columns.Count - 1], worKsheeT.Cells[dt.Rows.Count + 3, 2]].Merge();


                    //worKsheeT.Cells[dt.Rows.Count + 3, dt.Columns.Count-1] = CommonVariable.UserID;


                    worKsheeT.Range[worKsheeT.Cells[dt.Rows.Count + 5, 1], worKsheeT.Cells[dt.Rows.Count + 5, dt.Columns.Count]].Merge();
                    worKsheeT.Cells[dt.Rows.Count + 5, 1].HorizontalAlignment =
                    Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                    worKsheeT.Cells[dt.Rows.Count + 5, 1].NumberFormat = "@";
                    worKsheeT.Cells[dt.Rows.Count + 5, 1] = "GENERATED ON : " + System.DateTime.Now.ToString("dd/MMM/yyyy HH:mm"); ;
                    // worKsheeT.Cells[dt.Rows.Count + 4, dt.Columns.Count-2].NumberFormat = "@";

                    //worKsheeT.Range[worKsheeT.Cells[dt.Rows.Count + 4, dt.Columns.Count - 1], worKsheeT.Cells[dt.Rows.Count + 4, 2]].Merge();
                    //worKsheeT.Cells[dt.Rows.Count + 4, dt.Columns.Count-1] = System.DateTime.Now.ToString("dd/MMM/yyyy HH:mm");
                    worKbooK.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, FilePath);
                    System.Diagnostics.Process.Start(FilePath);
                      worKbooK.Close();
                excel.Quit();
                }
              
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
            return true;
        }
        public static DataTable ReadExcelData(string fileName, string SheetName,int Header,int Row )
        {
            try
            {
                string conn = string.Empty;
                DataTable dtexcel = new DataTable();

                Microsoft.Office.Interop.Excel.Application xlApp = CommonVariable.xlApp1;
                Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;

                var missing = System.Reflection.Missing.Value;

                xlWorkBook = xlApp.Workbooks.Open(fileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                if(SheetName!="")
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(SheetName);
                else
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);


                Microsoft.Office.Interop.Excel.Range xlRange = xlWorkSheet.UsedRange;
                Array myValues = (Array)xlRange.Cells.Value2;

                int vertical = myValues.GetLength(0);
                int horizontal = myValues.GetLength(1);


                // must start with index = 1
                // get header information
                for (int i = 1; i <= horizontal; i++)
                {
                    dtexcel.Columns.Add(new DataColumn(myValues.GetValue(Header, i).ToString()));
                }

                // Get the row information
                for (int a = Row; a <= vertical; a++)
                {
                    object[] poop = new object[horizontal];
                    for (int b = 1; b <= horizontal; b++)
                    {
                        poop[b - 1] = myValues.GetValue(a, b);
                    }
                    DataRow row = dtexcel.NewRow();
                    row.ItemArray = poop;
                    dtexcel.Rows.Add(row);
                }
                xlWorkBook.Close(true, missing, missing);
                xlApp.Quit();

                return dtexcel;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        #endregion
    }
}

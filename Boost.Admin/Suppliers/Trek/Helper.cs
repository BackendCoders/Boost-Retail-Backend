using OfficeOpenXml;
using System.Data;
using System.Data.OleDb;
using System.Net;
using System.Text;

namespace SIM
{
    public class Helper
    {
        #region Helper Methods
        /// <summary>
        /// Reads a remote file from a url.
        /// </summary>
        /// <param name="url">the target url to read from.</param>
        /// <returns>a string of the read data.</returns>
        public static string ReadFileFromUrl(string url)
        {
            var results = "";
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Timeout = 180000;
            req.KeepAlive = false;
            req.AllowAutoRedirect = true;
            req.Proxy = null;

            // temp hack as the server returns an invlaid cert - the below will allow it to be valid
            ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var tries = 0;
            StreamReader sr = null;

            try
            {
                tries++;
                using (var resp = (HttpWebResponse)req.GetResponse())
                {
                    sr = new StreamReader(resp.GetResponseStream());
                    results = sr.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Message != "The request was aborted: Could not create SSL/TLS secure channel.")
                {
                    if (tries < 3)
                    {
                        // wait 2 minutes then retry
                        Thread.Sleep(12000);
                        Console.WriteLine("error downloading file from url - waiting 2 minutes are retrying", ex);
                        ReadFileFromUrl(url);

                    }
                    Console.WriteLine("error downloading file from url after 3 attempts", ex);
                }
                else
                {
                    Console.WriteLine("unable to download file : ", ex);
                }
            }
            req = null;
            // cleanup
            try
            {
                sr.Close();
                sr.Dispose();
            }
            catch (NullReferenceException)
            {
                //sr is null..do nothing
            }
            return results;
        }

        /// <summary>
        /// Reads a remote file from a url.
        /// </summary>
        /// <param name="url">The target url to read from.</param>
        /// <param name="cookies">A Cookies Container</param>
        /// <returns>a string of the read data.</returns>
        public static string ReadFileFromUrl(string url, CookieContainer cookies)
        {
            var results = "";
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Timeout = 60000;
            req.KeepAlive = false;
            req.AllowAutoRedirect = true;
            req.CookieContainer = cookies;
            StreamReader sr;
            using (var resp = (HttpWebResponse)req.GetResponse())
            {
                sr = new StreamReader(resp.GetResponseStream());
                results = sr.ReadToEnd();
            }
            req = null;
            // cleanup
            sr.Close();
            sr.Dispose();
            return results;
        }

        /// <summary>
        /// Generates a Stream object from a string.
        /// </summary>
        /// <param name="str">the string to convert to a stream.</param>
        /// <returns>Stream Object of string</returns>
        public static Stream GenerateStreamFromString(string str)
        {
            var byteArray = Encoding.UTF8.GetBytes(str);
            var stream = new MemoryStream(byteArray);
            // cleanup
            byteArray = null;
            return stream;
        }

        /// <summary>
        /// Generates a Stream object from a string.
        /// </summary>
        /// <param name="str">the string to convert to a stream.</param>
        /// <returns>Stream Object of string</returns>
        public static Stream GenerateStreamFromString(string str, Encoding encode)
        {
            var byteArray = encode.GetBytes(str);
            var stream = new MemoryStream(byteArray);
            // cleanup
            byteArray = null;
            return stream;
        }

        public static DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            var maxColumn = 134;
            DataTable tbl = new DataTable();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader
                        ? firstRowCell.Text
                        : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, maxColumn];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        var index = cell.Start.Column - 1;
                        if (cell.Value != null)
                        {
                            row[index] = cell.Value.ToString();
                        }
                        else
                            row[index] = string.Empty;
                    }
                }
            }
            return tbl;
        }

        public static DataTable ReadXlsxOledb(string filepath)
        {
            DataTable dtexcel = new DataTable();
            var sheets = GetExcelSheetNames(filepath);
            var worksheet = sheets[0];
            var conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties='Excel 12.0;HDR=NO';"; //for above excel 2007  
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter($"select * from [{worksheet}]", con); //here we read data from sheet1  
                    oleAdpt.Fill(dtexcel); //fill excel data into dataTable  
                }
                catch (Exception ex)
                {

                }
            }
            return dtexcel;
        }
        private static String[] GetExcelSheetNames(string excelFile)
        {
            OleDbConnection objConn = null;
            DataTable dt = null;

            try
            {
                // Connection String. Change the excel file to the file you
                // will search.
                String connString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                  "Data Source=" + excelFile + ";Extended Properties=Excel 12.0;";
                // Create connection object by using the preceding connection string.
                objConn = new OleDbConnection(connString);
                // Open connection with the database.
                objConn.Open();
                // Get the data table containg the schema guid.
                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dt == null)
                {
                    return null;
                }

                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;

                // Add the sheet name to the string array.
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString();
                    i++;
                }
                return excelSheets;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                // Clean up.
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
        }
        //public static DataTable ReadXlsx(string path, bool hasHeader = true)
        //{
        //    DataTable tbl = new DataTable();
        //    using (var pck = new OfficeOpenXml.ExcelPackage())
        //    {
        //        using (var stream = File.OpenRead(path))
        //        {
        //            pck.Load(stream);
        //        }
        //        var ws = pck.Workbook.Worksheets.First();
        //        foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
        //        {
        //            tbl.Columns.Add(hasHeader
        //                ? firstRowCell.Text
        //                : string.Format("Column {0}", firstRowCell.Start.Column));
        //        }
        //        var startRow = hasHeader ? 2 : 1;
        //        for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
        //        {
        //            var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
        //            DataRow row = tbl.Rows.Add();
        //            foreach (var cell in wsRow)
        //            {
        //                row[cell.Start.Column - 1] = cell.Text;
        //            }
        //        }
        //    }
        //    return tbl;
        //}
        #endregion

    }
}

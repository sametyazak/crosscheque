using DataStreams.Csv;
using LinkedListLoop.src.server.entities;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace LinkedListLoop.src.server.db_connection
{
    public class MsDb : IDb<SqlParameter>
    {
        SqlConnection CurrentConnection;
        bool Intranscation = false;
        string connectionString = string.Empty;

        public MsDb()
        {
            InitDbConnection();
        }

        void SetConnection()
        {
            if (CurrentConnection.State == ConnectionState.Closed)
            {
                CurrentConnection.Open();
            }
        }

        void CloseConnection()
        {
            try
            {
                CurrentConnection.Close();
            }
            catch (Exception ex)
            {
                LogManager.InsertExceptionLog(ex);
                throw ex;
            }
        }

        private void InitDbConnection()
        {
            ConnectionStringSettings connectionStringSetting = ConfigurationManager.ConnectionStrings["DefaultConnection"];
            if (connectionStringSetting == null || string.IsNullOrEmpty(connectionStringSetting.ConnectionString))
            {
                throw new Exception(ResourceHelper.GetString("ConnectionStringNotFound"));
            }
            else
            {
                connectionString = connectionStringSetting.ConnectionString;
            }

            //CurrentConnection = new SqlConnection(connectionString);
        }

        public IEnumerable<T> ExecuteReader<T>(string commandText, IEnumerable<SqlParameter> paramList)
        {
            List<T> itemList = new List<T>();

            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    using (var cmdselect = new SqlCommand(commandText, conn))
                    {
                        cmdselect.CommandType = CommandType.StoredProcedure;
                        cmdselect.CommandTimeout = 0;
                        cmdselect.Parameters.AddRange(paramList.ToArray());

                        conn.Open();
                        using (var reader = cmdselect.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                T item = GetObjectFromReader<T>(reader);
                                itemList.Add(item);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    LogManager.InsertExceptionLog(e);
                    throw e;
                }
                finally
                {
                    if (conn != null && conn.State == ConnectionState.Open)
                    {
                        conn.Dispose();
                    }
                }
            }


            return itemList;
        }

        public int ExecuteQuery(string commandText, CommandType cmdType)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    using (var cmdselect = new SqlCommand(commandText, conn))
                    {
                        cmdselect.CommandType = cmdType;
                        cmdselect.CommandTimeout = 0;

                        conn.Open();
                        return cmdselect.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    LogManager.InsertExceptionLog(e);
                    throw e;
                }
                finally
                {
                    if (conn != null && conn.State == ConnectionState.Open)
                    {
                        conn.Dispose();
                    }
                }
            }

        }

        private T GetObjectFromReader<T>(SqlDataReader reader)
        {
            object item = Activator.CreateInstance(typeof(T));

            PropertyInfo[] itemProperties = item.GetType().GetProperties();

            foreach (PropertyInfo property in itemProperties)
            {
                var attributes = property.GetCustomAttributes(false);
                var columnMapping = attributes.FirstOrDefault(a => a.GetType() == typeof(CustomeDbAttribute));

                if (columnMapping != null)
                {
                    var dbMapping = columnMapping as CustomeDbAttribute;
                    var itemValue = reader[dbMapping.Name];

                    if (itemValue != null && itemValue != DBNull.Value)
                    {
                        property.SetValue(item, Convert.ChangeType(itemValue, property.PropertyType), null);
                    }

                }

            }

            return (T)item;
        }

        public List<NetworkItem> ExecuteReaderNetwork(string commandText, IEnumerable<SqlParameter> paramList)
        {
            List<NetworkItem> itemList = new List<NetworkItem>();

            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    using (var cmdselect = new SqlCommand(commandText, conn))
                    {
                        cmdselect.CommandType = CommandType.StoredProcedure;
                        cmdselect.CommandTimeout = 0;
                        cmdselect.Parameters.AddRange(paramList.ToArray());

                        conn.Open();
                        using (var reader = cmdselect.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                NetworkItem item = new NetworkItem();

                                item.from = reader["Sender"] != null && reader["Sender"] != DBNull.Value ? reader["Sender"].ToString() : string.Empty;
                                item.to = reader["Receiver"] != null && reader["Receiver"] != DBNull.Value ? reader["Receiver"].ToString() : string.Empty;
                                item.value = reader["RecordCount"] != null && reader["RecordCount"] != DBNull.Value ? (int)reader["RecordCount"] : 0;

                                itemList.Add(item);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    LogManager.InsertExceptionLog(e);
                    throw e;
                }
                finally
                {
                    if (conn != null && conn.State == ConnectionState.Open)
                    {
                        conn.Dispose();
                    }
                }
            }


            return itemList;
        }

        private Dictionary<string, string> GetCreateTmpTableParams<T>(string fileId)
        {
            Dictionary<string, string> tableParameters = new Dictionary<string, string>();

            object item = Activator.CreateInstance(typeof(T));
            string tableName = string.Empty;

            if (item == null)
            {
                throw new Exception(ResourceHelper.GetStringFormat("InstanceNotCreated", typeof(T).Name));
            }

            var attributes = item.GetType().GetCustomAttributes(false);
            var tableAttr = attributes.FirstOrDefault(a => a.GetType() == typeof(TableAttribute));

            //string columnList = string.Join(",", item.GetType().GetProperties().Select(a => { return string.Format("{0} nvarchar(max)", a.Name); }));

            if (tableAttr != null)
            {
                TableAttribute tableDef = tableAttr as TableAttribute;
                string tempTableName = string.Format("tmp.{0}__{1}", tableDef.Name, fileId.Replace("-", "_"));
                //string createTableScript = string.Format("create table {0} ({1})", tempTableName, columnList);
                string createTableScript = string.Format("select top 0 * into {0} from {1}(nolock)", tempTableName, tableDef.Name);

                tableParameters.Add("tableName", tempTableName);
                tableParameters.Add("createScript", createTableScript);
            }
            else
            {
                throw new Exception(ResourceHelper.GetStringFormat("TableAttributeNotFound", item.GetType().Name));
            }

            return tableParameters;
        }

        public string BulkInsertToTemp<T>(Stream fileData, List<SqlBulkCopyColumnMapping> columnMappingList, string fileId)
        {
            Dictionary<string, string> tableParams = GetCreateTmpTableParams<T>(fileId);

            // declare CsvDataReader object which will act as a source for data for SqlBulkCopy
            using (LumenWorks.Framework.IO.Csv.CsvReader csvData = new LumenWorks.Framework.IO.Csv.CsvReader(new StreamReader(fileData, true), true, ';'))
            {
                csvData.MissingFieldAction = LumenWorks.Framework.IO.Csv.MissingFieldAction.ReplaceByEmpty;
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        using (var cmdselect = new SqlCommand(tableParams["createScript"], conn, transaction))
                        {
                            cmdselect.CommandType = CommandType.Text;
                            cmdselect.CommandTimeout = 0;

                            cmdselect.ExecuteNonQuery();
                        }

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity, transaction))
                        {
                            bulkCopy.DestinationTableName = tableParams["tableName"];

                            foreach (SqlBulkCopyColumnMapping columnMapping in columnMappingList)
                                bulkCopy.ColumnMappings.Add(columnMapping);

                            // call WriteToServer which starts import
                            bulkCopy.WriteToServer(csvData);

                            transaction.Commit();
                            return tableParams["tableName"];
                        } // dispose of SqlBulkCopy object
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.InsertExceptionLog(ex);
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }

            } // dispose of CsvDataReader object
        }
    }
}
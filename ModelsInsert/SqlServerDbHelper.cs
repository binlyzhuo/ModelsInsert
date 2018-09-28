using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsInsert
{
    class SqlServerDbHelper
    {
        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="connString"></param>
        /// <param name="items1"></param>
        /// <param name="tableName1"></param>
        /// <param name="result"></param>
        public void Insert<T1>(string connString, List<T1> items1,List<string> fields, string tableName1, out bool result)
        {
            var dt1 = items1.ConvertToDataTable();
            using (var sqlConn = new SqlConnection(connString))
            {
                sqlConn.Open();
                var tran = sqlConn.BeginTransaction();
                try
                {

                    var bulkCopy = new SqlBulkCopy(sqlConn, SqlBulkCopyOptions.Default, tran);
                    bulkCopy.DestinationTableName = tableName1;
                    bulkCopy.BatchSize = dt1.Rows.Count;

                    //bulkCopy.ColumnMappings.Add(0, "ID");
                    //bulkCopy.ColumnMappings.Add(1, "Name");
                    //bulkCopy.ColumnMappings.Add(2, "Sex");
                    //bulkCopy.ColumnMappings.Add(3, "Address");

                    for (int i = 0; i < fields.Count; i++)
                    {
                        bulkCopy.ColumnMappings.Add(fields[i], fields[i]);
                    }

                    if (dt1.Rows.Count != 0)
                        bulkCopy.WriteToServer(dt1);



                    tran.Commit();
                    sqlConn.Close();
                    result = true;
                }
                catch (Exception)
                {
                    tran.Rollback();
                    result = false;
                    throw;
                }
                finally
                {
                    sqlConn.Close();
                }

            }

        }
    }
}

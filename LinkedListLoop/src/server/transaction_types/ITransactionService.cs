using LinkedListLoop.src.server.entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.transaction_types
{
    public interface ITransactionService<T>
    {
        /// <summary>
        /// Get all file records
        /// </summary>
        /// <returns>generic list</returns>
        List<T> GetAll();
        
        /// <summary>
        /// Get all file records
        /// </summary>
        /// <param name="fileId">upload file id</param>
        /// <returns>generic list</returns>
        List<T> GetAllByParentId(TranFileInfo file);

        /// <summary>
        /// Get all file records
        /// </summary>
        /// <param name="fileId">upload file id</param>
        /// <returns>generic list</returns>
        List<T> GetPreviewByParentId(TranFileInfo file);

        /// <summary>
        /// Service method to create new transaction record
        /// </summary>
        /// <param name="transactionRecord">transction type model</param>
        /// <returns>true or false</returns>
        bool Create(T transactionRecord);

        /// <summary>
        /// Method to update transction record
        /// </summary>
        /// <param name="transactionRecord">transction type model</param>
        /// <returns>true or false</returns>
        bool Update(T transactionRecord);

        /// <summary>
        /// Method to get transction record by id
        /// </summary>
        /// <param name="id">record id, guid</param>
        /// <returns>record object</returns>
        T GetRecordById(string id);

        /// <summary>
        /// Bulk insert
        /// </summary>
        /// <param name="recordList">bulk insert</param>
        /// <returns>true or false</returns>
        bool BulkInsert(List<T> recordList);

        /// <summary>
        /// Returns group by records
        /// </summary>
        /// <param name="parentId">parent Id</param>
        /// <returns>true or false</returns>
        List<NetworkItem> GetGroupedParentList(string parentId);

        /// <summary>
        /// Creates a temp table, inserts into and returns table name
        /// </summary>
        /// <param name="fileData">file stream</param>
        /// <param name="columnMappingList">bulk copy column mapping list</param>
        /// <param name="fileId">file id</param>
        /// <returns>created table name</returns>
        string BulkInsertIntoTemp(Stream fileData, List<SqlBulkCopyColumnMapping> columnMappingList, string fileId);
        
    }
}
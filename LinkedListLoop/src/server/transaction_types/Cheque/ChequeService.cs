using LinkedListLoop.entities;
using LinkedListLoop.src.server.entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using EntityFramework.BulkInsert.Extensions;
using System.Transactions;
using System.Data.Entity;
using LinkedListLoop.src.server.db_connection;
using System.Data.SqlClient;
using System.IO;


namespace LinkedListLoop.src.server.transaction_types.Cheque
{
    public class ChequeService : ITransactionService<ChequeInfo>
    {
        public List<ChequeInfo> GetAllByParentId(TranFileInfo file)
        {
            if (file.ContentLocation == FileContentLocation.MainTable)
            {
                using (var context = new EntityContext())
                {
                    List<ChequeInfo> members = context.ChequeList.AsNoTracking().Where(a => a.FileId == file.Id).ToList();

                    return members;
                }
            }

            if (file.ContentLocation == FileContentLocation.TempTable)
            {
                using (var db = new DbContext("DefaultConnection"))
                {
                    string query = string.Format("select * from {0}", file.TempTableName);
                    return db.Database.SqlQuery<ChequeInfo>(query).ToList();
                }
            }

            return new List<ChequeInfo>();
        }

        public List<ChequeInfo> GetPreviewByParentId(TranFileInfo file)
        {
            if (file.ContentLocation == FileContentLocation.MainTable)
            {
                using (var context = new EntityContext())
                {

                    List<ChequeInfo> members = context.ChequeList.AsNoTracking().Where(a => a.FileId == file.Id).Take(Constants.MaxUiRecordCount).ToList();

                    return members;
                }
            }

            if (file.ContentLocation == FileContentLocation.TempTable)
            {
                using (var db = new DbContext("DefaultConnection"))
                {
                    string query = string.Format("select * from {0}", file.TempTableName);
                    return db.Database.SqlQuery<ChequeInfo>(query).Take(Constants.MaxUiRecordCount).ToList();
                }
            }

            return new List<ChequeInfo>();
        }

        public List<ChequeInfo> GetAll()
        {
            using (var context = new EntityContext())
            {
                List<ChequeInfo> members = context.ChequeList.ToList();

                return members;
            }
        }

        public bool Create(ChequeInfo chequeInfo)
        {
            using (var context = new EntityContext())
            {
                if (chequeInfo.id == null)
                {
                    chequeInfo.id = Guid.NewGuid().ToString();
                }
                
                context.ChequeList.Add(chequeInfo);
                bool result = context.SaveChanges() > 0;

                return result;
            }
        }

        public bool Update(ChequeInfo chequeInfo)
        {
            using (var context = new EntityContext())
            {
                context.ChequeList.Attach(chequeInfo);
                context.Entry(chequeInfo).State = EntityState.Modified;
                return context.SaveChanges() > 0;
            }
        }

        public ChequeInfo GetRecordById(string id)
        {
            using (var context = new EntityContext())
            {
                ChequeInfo membership = context.ChequeList
                    .Where(i => i.id == id)
                    .SingleOrDefault();

                return membership;
            } 
        }

        public bool BulkInsert(List<ChequeInfo> list)
        {
            using (var context = new EntityContext())
            {
                using (var transactionScope = new TransactionScope())
                {
                    context.BulkInsert(list);

                    bool result = context.SaveChanges() > 0;
                    transactionScope.Complete();

                    return result;
                }
            }
        }

        public List<NetworkItem> GetGroupedParentList(string parentId)
        {
            MsDb db = new MsDb();

            List<NetworkItem> networkList = new List<NetworkItem>();

            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("fileId", parentId));

            networkList = db.ExecuteReaderNetwork("[dbo].[SelectFileRecordByGroup]", paramList);

            return networkList;
        }

        public string BulkInsertIntoTemp(Stream fileData, List<SqlBulkCopyColumnMapping> columnMappingList, string fileId)
        {
            MsDb db = new MsDb();
            return db.BulkInsertToTemp<ChequeInfo>(fileData, columnMappingList, fileId);
        }
    }


}
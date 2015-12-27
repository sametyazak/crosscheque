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
using System.Data.SqlClient;
using System.IO;
using LinkedListLoop.src.server.db_connection;


namespace LinkedListLoop.src.server.transaction_types.Cheque
{
    public class EInvoiceService : ITransactionService<EInvoiceInfo>
    {
        public List<EInvoiceInfo> GetAllByParentId(TranFileInfo file)
        {
            if (file.ContentLocation == FileContentLocation.MainTable)
            {
                using (var context = new EntityContext())
                {
                    List<EInvoiceInfo> members = context.EInvoiceList.AsNoTracking().Where(a => a.FileId == file.Id).ToList();

                    return members;
                }
            }

            if (file.ContentLocation == FileContentLocation.TempTable)
            {
                using (var db = new DbContext("DefaultConnection"))
                {
                    string query = string.Format("select * from {0}", file.TempTableName);
                    return db.Database.SqlQuery<EInvoiceInfo>(query).ToList();
                }
            }

            return new List<EInvoiceInfo>();
        }

        public List<EInvoiceInfo> GetPreviewByParentId(TranFileInfo file)
        {
            if (file.ContentLocation == FileContentLocation.MainTable)
            {
                using (var context = new EntityContext())
                {

                    List<EInvoiceInfo> members = context.EInvoiceList.AsNoTracking().Where(a => a.FileId == file.Id).Take(Constants.MaxUiRecordCount).ToList();

                    return members;
                }
            }

            if (file.ContentLocation == FileContentLocation.TempTable)
            {
                using (var db = new DbContext("DefaultConnection"))
                {
                    string query = string.Format("select * from {0}", file.TempTableName);
                    return db.Database.SqlQuery<EInvoiceInfo>(query).Take(Constants.MaxUiRecordCount).ToList();
                }
            }

            return new List<EInvoiceInfo>();
        }

        public List<EInvoiceInfo> GetAll()
        {
            using (var context = new EntityContext())
            {
                List<EInvoiceInfo> members = context.EInvoiceList.ToList();

                return members;
            }
        }

        public bool Create(EInvoiceInfo einvoiceInfo)
        {
            using (var context = new EntityContext())
            {
                if (einvoiceInfo.id == null)
                {
                    einvoiceInfo.id = Guid.NewGuid().ToString();
                }
                
                context.EInvoiceList.Add(einvoiceInfo);
                bool result = context.SaveChanges() > 0;

                return result;
            }
        }

        public bool Update(EInvoiceInfo einvoiceInfo)
        {
            using (var context = new EntityContext())
            {
                context.EInvoiceList.Attach(einvoiceInfo);
                context.Entry(einvoiceInfo).State = EntityState.Modified;
                return context.SaveChanges() > 0;
            }
        }

        public EInvoiceInfo GetRecordById(string id)
        {
            using (var context = new EntityContext())
            {
                EInvoiceInfo membership = context.EInvoiceList
                    .Where(i => i.id == id)
                    .SingleOrDefault();

                return membership;
            } 
        }

        public bool BulkInsert(List<EInvoiceInfo> list)
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
            return db.BulkInsertToTemp<EInvoiceInfo>(fileData, columnMappingList, fileId);
        }
    }


}
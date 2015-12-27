using LinkedListLoop.entities;
using LinkedListLoop.src.server.entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.transaction_types.File
{
    public class FileService : ITransactionService<TranFileInfo>
    {
        public List<TranFileInfo> GetAllByParentId(TranFileInfo file)
        {
            throw new NotImplementedException();
        }

        public List<TranFileInfo> GetPreviewByParentId(TranFileInfo file)
        {
            throw new NotImplementedException();
        }

        public List<TranFileInfo> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Create(TranFileInfo fileInfo)
        {
            using (var context = new EntityContext())
            {
                if (fileInfo.Id == null)
                {
                    fileInfo.Id = Guid.NewGuid().ToString();
                }

                context.FileList.Add(fileInfo);
                bool result = context.SaveChanges() > 0;

                return result;
            }
        }

        public bool Update(TranFileInfo fileInfo)
        {
            using (var context = new EntityContext())
            {
                context.FileList.Attach(fileInfo);
                context.Entry(fileInfo).State = EntityState.Modified;
                return context.SaveChanges() > 0;
            }
        }

        public TranFileInfo GetRecordById(string id)
        {
            using (var context = new EntityContext())
            {
                TranFileInfo file = context.FileList
                    .Where(i => i.Id == id)
                    .SingleOrDefault();

                return file;
            }
        }

        public TranFileInfo GetRecordByPath(string path)
        {
            using (var context = new EntityContext())
            {
                TranFileInfo file = context.FileList
                    .Where(i => i.Path == path)
                    .SingleOrDefault();

                return file;
            }
        }

        public bool BulkInsert(List<TranFileInfo> list)
        {
            throw new NotImplementedException();
        }

        public List<TranFileInfo> GetUserFiles(string userName, string transctionType)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new Exception(ResourceHelper.GetString("UserNameIsEmpty"));
            }

            if (string.IsNullOrEmpty(transctionType))
            {
                throw new Exception(ResourceHelper.GetString("TranTypeIsEmpty"));
            }

            using (var context = new EntityContext())
            {
               List<TranFileInfo> file = context.FileList
                   .AsNoTracking() 
                   .Where(i => i.UserInfo == userName && i.TransactionType == transctionType)
                    .OrderByDescending(a=>a.InsertEndDate)
                    .ToList();

                return file;
            }
        }

        public List<NetworkItem> GetGroupedParentList(string parentId)
        {
            throw new NotImplementedException();
        }

        public string BulkInsertIntoTemp(Stream fileData, List<SqlBulkCopyColumnMapping> columnMappingList, string fileId)
        {
            throw new NotImplementedException();
        }
    }
}
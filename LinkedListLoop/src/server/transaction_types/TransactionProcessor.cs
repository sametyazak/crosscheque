using LinkedListLoop.entities;
using LinkedListLoop.src.server.file_reader;
using LinkedListLoop.src.server.transaction_types;
using LinkedListLoop.src.server.transaction_types.File;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.entities.transaction_types
{
    public abstract class TransactionProcessorBase
    {
        public abstract void SaveFileRecords(TranFileInfo fileInfo, object record);
        public abstract AjaxResult ReadFileRecords(string fileId, TransactionType tranType);
        public abstract LoopProcessResult ProcessSenderFile(string fileId, TransactionType tranType);
        public abstract string UploadFile(string tranTypeName, Stream fileData, string fileId);
    }

    public abstract class TransactionProcessor<T> : TransactionProcessorBase
    {
        private SenderReceiverList baseList;
        private List<string> ProcessedSenders;

        public override void SaveFileRecords(TranFileInfo fileInfo, object record)
        {

        }

        public override AjaxResult ReadFileRecords(string fileId, TransactionType tranType)
        {
            return ReadFileRecordsInternal(fileId, tranType, true);
        }

        public virtual AjaxResult ReadFileRecordsInternal(string fileId, TransactionType tranType, bool isPreview)
        {
            AjaxResult result = new AjaxResult();

            try
            {
                FileService fileService = new FileService();
                TranFileInfo file = fileService.GetRecordById(fileId);

                if (file == null)
                {
                    throw new Exception(ResourceHelper.GetStringFormat("FileNotFound", fileId));
                }

                ITransactionService<T> service = TransactionHelper.GetDynamicTranTypeService<T>(tranType.Name);
                result.ResultObject = service.GetPreviewByParentId(file);
                
                result.IsError = false;
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.ErrorMessage = ex.Message;
                result.ErrorDetail = ex.InnerException != null ? ex.InnerException.Message : ex.ToString();
            }

            return result;
        }

        protected abstract List<TransactionTypeDefinition> GetTranBaseList(List<T> senderList);

        public override LoopProcessResult ProcessSenderFile(string fileId, TransactionType tranType)
        {
            return ProcessSenderFileInternal(fileId, tranType);
        }

        protected virtual List<LoopResult> ProcessLoops(List<NetworkItem> networkList)
        {
            List<LoopResult> processResult = new List<LoopResult>();

            if (networkList != null && networkList.Count > 0)
            {
                baseList = GetSenderTotalList(networkList);

                if (baseList != null && baseList.TotalList.Count > 0)
                {
                    processResult = ProcessLoopsResursive(baseList.TotalList);
                }
            }

            return processResult;
        }

        protected virtual List<LoopResult> ProcessLoopsResursive(Dictionary<string, List<NetworkItem>> totalNetworkList)
        {
            List<LoopResult> loopResult = new List<LoopResult>();
            ProcessedSenders = new List<string>();

            int counter = 0;

            KeyValuePair<string, List<NetworkItem>>[] networkArray = totalNetworkList.ToArray();

            // try to iterate through all senders
            for (int i = 0; i < networkArray.Length; i++)
            {
                KeyValuePair<string, List<NetworkItem>> network = networkArray[i];
                // sender may be processed in sub loops, if so, skip it
                if (!ProcessedSenders.Contains(network.Key))
                {
                    List<string> currentPath = new List<string>();
                    List<LoopResult> subLoopList = ProcessSubSenderList(network.Key, network.Value, currentPath);
                    loopResult.AddRange(subLoopList);
                }

                counter++;
            }

            return loopResult;
        }

        protected virtual List<LoopResult> ProcessSubSenderList(string sender, List<NetworkItem> receiverList, List<string> parentPath)
        {
            List<LoopResult> loopResult = new List<LoopResult>();

            List<string> currentPath = new List<string>();
            currentPath.AddRange(parentPath);

            // add processed senders, so do not process in parent loop again
            if (!ProcessedSenders.Contains(sender))
            {
                ProcessedSenders.Add(sender);
            }

            // loop found, generate loop object and return
            if (currentPath.Contains(sender))
            {
                currentPath.Add(sender);
                LoopResult loopRes = GetLoopResult(currentPath, sender);
                loopResult.Add(loopRes);
                return loopResult;
            }

            // log network path
            currentPath.Add(sender);

            List<NetworkItem> filteredReceiverList = receiverList.Where(a => a.to != sender).ToList();

            // loop not found, try to find in sub objects
            foreach (NetworkItem receiver in filteredReceiverList)
            {
                if (baseList.TotalList.ContainsKey(receiver.to))
                {
                    List<NetworkItem> receiverSenders = baseList.TotalList[receiver.to];

                    loopResult.AddRange(ProcessSubSenderList(receiver.to, receiverSenders, currentPath));
                }
            }

            return loopResult;
        }

        protected virtual SenderReceiverList GetSenderReceiverList(List<TransactionTypeDefinition> senderList)
        {
            SenderReceiverList senderReceiverList = new SenderReceiverList();
            List<string> rootList = new List<string>();

            Dictionary<string, List<string>> srList = new Dictionary<string, List<string>>();
            List<string> allReceiverList = new List<string>();

            string toBeRemovedRoot = string.Empty;

            for (var i = 0; i < senderList.Count; i++)
            {
                TransactionTypeDefinition cheque = senderList[i];

                if (!string.IsNullOrEmpty(cheque.Sender)
                    && !string.IsNullOrEmpty(cheque.Receiver)
                    && cheque.Sender != cheque.Receiver)
                {
                    bool senderExists = srList.Keys.Contains(cheque.Sender);
                    bool receiverExists = srList.Keys.Contains(cheque.Receiver);
                    bool allReceiverExists = allReceiverList.Contains(cheque.Sender);

                    allReceiverList.Add(cheque.Receiver);

                    if (!senderExists)
                    {
                        srList.Add(cheque.Sender, new List<string>());

                        if (!allReceiverExists)
                        {
                            if (!string.IsNullOrEmpty(toBeRemovedRoot))
                            {
                                rootList.Remove(toBeRemovedRoot);
                                toBeRemovedRoot = string.Empty;
                            }

                            rootList.Add(cheque.Sender);

                            if (receiverExists)
                            {
                                if (rootList.Count > 1)
                                {
                                    rootList.Remove(cheque.Receiver);
                                }
                                else
                                {
                                    toBeRemovedRoot = cheque.Receiver;
                                }
                            }
                        }
                    }

                    List<string> receiverList = srList[cheque.Sender];

                    if (!receiverList.Contains(cheque.Receiver))
                    {
                        receiverList.Add(cheque.Receiver);
                    }

                }
            }

            senderReceiverList.RootList = rootList;
            //senderReceiverList.TotalList = srList;

            return senderReceiverList;
        }

        protected virtual SenderReceiverList GetSenderTotalList(List<NetworkItem> networkList)
        {
            SenderReceiverList senderTotalList = new SenderReceiverList();

            senderTotalList.RootList = new List<string>();
            senderTotalList.TotalList = GetSenderList(networkList);

            return senderTotalList;
        }

        protected virtual Dictionary<string, List<NetworkItem>> GetSenderList(List<NetworkItem> networkList)
        {
            Dictionary<string, List<NetworkItem>> senderList = new Dictionary<string, List<NetworkItem>>();

            senderList = networkList
                .GroupBy(o => o.from)
                .ToDictionary(g => g.Key, g => g.ToList());

            return senderList;
        }

        protected virtual List<LoopResult> GetTreeLoopsRecursive(string rootNode, List<string> path, Dictionary<string, List<NetworkItem>> senderReceiverList)
        {
            List<LoopResult> loops = new List<LoopResult>();
            List<string> subPath = new List<string>();
            subPath.AddRange(path);

            if (subPath.Contains(rootNode))
            {
                subPath.Add(rootNode);
                LoopResult loopRes = GetLoopResult(subPath, rootNode);
                loops.Add(loopRes);
                return loops;
            }

            subPath.Add(rootNode);

            if (senderReceiverList.ContainsKey(rootNode))
            {
                List<string> subSenders = senderReceiverList[rootNode].Select(a => a.to).ToList();

                foreach (string sub in subSenders)
                {
                    loops.AddRange(GetTreeLoopsRecursive(sub, subPath, senderReceiverList));
                }
            }

            loops.Add(GetLoopResult(subPath, string.Empty));
            return loops;
        }

        protected virtual LoopResult GetLoopResult(List<string> path, string loopElement)
        {
            LoopResult result = new LoopResult();
            result.Items = path;

            if (!string.IsNullOrEmpty(loopElement))
            {
                int start = path.IndexOf(loopElement);
                int end = path.LastIndexOf(loopElement);

                if (start < 0 || end < 0 || end <= start)
                {
                    throw new Exception(ResourceHelper.GetString("InvalidLoopIndex"));
                }

                result.Loop = path.GetRange(start, (end - start) + 1);
                result.HasLoop = true;
                result.LoopHtmlText = GetLoopText(result.Items, result.Loop);
            }

            return result;
        }

        protected virtual string GetLoopText(List<string> items, List<string> loop)
        {
            string itemsText = items != null ? string.Join("->", items) : string.Empty;
            string loopText = loop != null ? string.Join("->", loop) : string.Empty;

            if (!string.IsNullOrEmpty(itemsText) && !string.IsNullOrEmpty(loopText))
            {
                int startIndex = itemsText.IndexOf(loopText);

                if (startIndex > -1)
                {
                    if (loopText.Length + startIndex <= itemsText.Length)
                    {
                        string first = startIndex > 0 ? itemsText.Substring(0, startIndex) : string.Empty;
                        string highLight = string.Format("<span class\"loopHightLight\" style=\"font-size: large;font-weight: bold;\">{0}</span>", itemsText.Substring(startIndex, loopText.Length));
                        string last = startIndex + loopText.Length == itemsText.Length ? string.Empty : itemsText.Substring(startIndex + +loopText.Length);

                        return string.Format("{0}{1}{2}", first, highLight, last);
                    }
                    else
                    {
                        return ResourceHelper.GetString("NoLoopFound");
                    }
                }
                else
                {
                    return ResourceHelper.GetString("NoLoopMatching");
                }
            }
            else
            {
                return string.Empty;
            }
        }

        protected virtual LoopProcessResult ProcessSenderFileInternal(string fileId, TransactionType tranType)
        {
            DateTime processStartDate = DateTime.Now;

            LoopProcessResult result = new LoopProcessResult();

            if (!string.IsNullOrEmpty(fileId))
            {
                ITransactionService<T> service = TransactionHelper.GetDynamicTranTypeService<T>(tranType.Name);
                List<NetworkItem> networkList = service.GetGroupedParentList(fileId);

                if (networkList.Count > Constants.MaxUiNetrowkCount)
                {
                    result.Message = ResourceHelper.GetStringFormat("MaxUINetworkRecordCountExceeded", Constants.MaxUiNetrowkCount);
                    result.NetworkList = new List<NetworkItem>();
                }
                else
                {
                    result.NetworkList = networkList;
                }

                DateTime recordsProcessDate = DateTime.Now;

                result.ProcessPerformace = new List<KeyValue>();
                result.ProcessPerformace.Add(new KeyValue() { Key = ResourceHelper.GetString("DbReadPerformans"), Value = recordsProcessDate - processStartDate });

                // do file process
                List<LoopResult> loops = ProcessLoops(networkList);

                result.ProcessPerformace.Add(new KeyValue() { Key = ResourceHelper.GetString("RecordsProcessPerformans"), Value = DateTime.Now - recordsProcessDate });
                result.ProcessPerformace.Add(new KeyValue() { Key = ResourceHelper.GetString("FileProcessPerformans"), Value = DateTime.Now - processStartDate });
                result.CanSeePerformanceResult = UserManager.CurrentUserHasRole("Developer");

                result.LoopList = loops;
            }

            return result;
        }

        public override string UploadFile(string tranTypeName, Stream fileData, string fileId)
        {
            ITransactionService<T> service = TransactionHelper.GetDynamicTranTypeService<T>(tranTypeName);
            TransactionType tranType = TransactionHelper.GetTransactionType(tranTypeName);

            return service.BulkInsertIntoTemp(fileData, GetBulkCopyColumnMapping(tranType), fileId);
        }

        protected virtual List<SqlBulkCopyColumnMapping> GetBulkCopyColumnMapping(TransactionType tranType)
        {
            List<SqlBulkCopyColumnMapping> mappingList = new List<SqlBulkCopyColumnMapping>();

            if (tranType != null)
            {
                if (tranType.ColumnList == null)
                {
                    return mappingList;
                }

                mappingList = tranType.ColumnList.Select(
                    a => new SqlBulkCopyColumnMapping(a.FileFieldName, a.Name)
                )
                .ToList();
            }
            else
            {
                throw new Exception(ResourceHelper.GetString("TranTypeNotIsNull"));
            }

            return mappingList;
        }
    }

}
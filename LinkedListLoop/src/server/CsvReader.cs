using LinkedListLoop.entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src
{
    public class CsvReader<T> where T : class
    {
        public char Delimiter = ';';
        public bool IsFirstLineColumn = true;

        public List<T> ReadFileAsList(string filePath)
        {
            List<T> fileList = new List<T>();

            using (var reader = new StreamReader(File.OpenRead(filePath)))
            {
                int lineCounter = 0;
                List<string> columnList = new List<string>();

                while (!reader.EndOfStream)
                {
                    lineCounter++;
                    var line = reader.ReadLine();

                    if (!string.IsNullOrEmpty(line))
                    {
                        var lineValues = line.Split(Delimiter);

                        if (lineValues != null && lineValues.Length > 0)
                        {
                            if (lineCounter == 1 && IsFirstLineColumn)
                            {
                                columnList = lineValues.ToList();
                            }
                            else
                            {
                                List<string> valueList = lineValues.ToList();
                                object fileObject = Activator.CreateInstance(typeof(T));
                                //IList<PropertyInfo> props = new List<PropertyInfo>(nodeType.GetProperties());

                                for (int i = 0; i < valueList.Count; i++)
                                {
                                    if (columnList.Count > i)
                                    {
                                        string columnName = columnList[i];

                                    }
                                }
                            }
                        }
                    }
                }
            }

            return fileList;
        }

        public List<ChequeInfo> GetTempList(string filePath)
        { 
            List<ChequeInfo> fileList = new List<ChequeInfo>();

            using (var reader = new StreamReader(File.OpenRead(filePath)))
            {
                int lineCounter = 0;

                while (!reader.EndOfStream)
                {
                    lineCounter++;
                    var line = reader.ReadLine();

                    if (!string.IsNullOrEmpty(line))
                    {
                        var lineValues = line.Split(Delimiter);
                        List<string> valueList = lineValues.ToList();

                        if (lineCounter > 1)
                        {
                            if (valueList != null && valueList.Count == 4)
                            {
                                ChequeInfo cheque = new ChequeInfo();
                                cheque.Sender = valueList[0];
                                cheque.Receiver = valueList[1];
                                cheque.Amount = Converter.ToDecimal(valueList[2]);
                                cheque.Date = valueList[3];

                                fileList.Add(cheque);
                            }
                        }
                    }
                }
            }

            return fileList;
        }
    }
}
using LinkedListLoop.entities;
using LinkedListLoop.src.server;
using LinkedListLoop.src.server.entities;
using LinkedListLoop.src.server.entities.transaction_types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace LinkedListLoop.src
{
    public class CsvReader : IFileReader
    {
        public char Delimiter = ';';
        public bool IsFirstLineColumn = true;
        public bool IgnoreRecordError = true;

        public List<T> ReadFileAsList<T>(string filePath, List<UiGridColumn> columnDefinitionList)
        {
            List<T> fileList = new List<T>();

            using (var reader = new StreamReader(File.OpenRead(filePath), System.Text.Encoding.UTF8))
            {
                int lineCounter = 0;
                object[] formatArgs = new object[3];


                List<string> columnList = new List<string>();

                while (!reader.EndOfStream)
                {
                    try
                    {
                        lineCounter++;
                        var line = reader.ReadLine();

                        if (string.IsNullOrEmpty(line))
                        {
                            throw new KnownException(ResourceHelper.GetStringFormat("EmptyLineForFile", line));
                        }

                        var lineValues = line.Split(Delimiter);

                        if (lineValues == null || lineValues.Length == 0)
                        {
                            throw new KnownException(ResourceHelper.GetStringFormat("NoColumnForFileWithDelimiter", this.Delimiter));
                        }

                        if (lineCounter == 1 && IsFirstLineColumn)
                        {
                            columnList = lineValues.ToList();
                        }
                        else
                        {
                            List<string> valueList = lineValues.ToList();
                            object fileObject = Activator.CreateInstance(typeof(T));
                            //IList<PropertyInfo> props = new List<PropertyInfo>(nodeType.GetProperties());

                            formatArgs[0] = lineCounter;
                            formatArgs[1] = columnList.Count;
                            formatArgs[2] = valueList.Count;

                            if (columnList.Count != valueList.Count)
                            {
                                throw new KnownException(ResourceHelper.GetStringFormat("InvalidHeaderOrRowColumnNumber", formatArgs));
                            }

                            for (int i = 0; i < valueList.Count; i++)
                            {
                                string columnName = columnList[i];
                                string columnValue = valueList[i];
                                UiGridColumn columnDef = columnDefinitionList.FirstOrDefault(a => a.FileFieldName == columnName);

                                if (columnDef == null)
                                {
                                    //throw new Exception(ResourceHelper.GetStringFormat("ColoumnDefinitionNotFound", columnName));
                                    continue;
                                }

                                string propName = columnDef.Name;
                                PropertyInfo propInfo = fileObject.GetType().GetProperty(propName);

                                if (propInfo == null)
                                {
                                    throw new KnownException(ResourceHelper.GetStringFormat("PropertyNotFound", propName));
                                }

                                if (!string.IsNullOrEmpty(columnValue))
                                {
                                    propInfo.SetValue(fileObject, Convert.ChangeType(columnValue, propInfo.PropertyType), null);
                                }
                            }

                            fileList.Add((T)fileObject);
                        }
                    }
                    catch (KnownException ex)
                    {
                        if (!this.IgnoreRecordError)
                            throw ex;
                    }
                    catch (Exception ex)
                    {
                        if (!this.IgnoreRecordError)
                            throw new Exception(ResourceHelper.GetStringFormat("InvalidHeaderOrRowColumnNumber", formatArgs), ex);
                    }

                }

            }

            return fileList;
        }
    }
}
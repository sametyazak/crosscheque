using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LinkedListLoop.src.server.db_connection
{
    public interface IDb<T>
    {
        /// <summary>
        /// Execute reader
        /// </summary>
        /// <param name="commandText">sp or t-sql</param>
        /// <param name="paramList">generic parameter list of command</param>
        /// <returns>reader object</returns>
        IEnumerable<K> ExecuteReader<K>(string commandText, IEnumerable<T> paramList);

        /// <summary>
        /// Execute query
        /// </summary>
        /// <param name="commandText">t-sql statement</param>
        /// <param name="cmdType">sp or text</param>
        /// <returns>number of rows affected</returns>
        int ExecuteQuery(string commandText, CommandType cmdType);

    }
}
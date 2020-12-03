using Dapper;
using System.Data.SQLite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GoDollarLibrary.Databases
{
    public class SqliteDataAccess : ISqliteDataAccess
    {
        private readonly IConfiguration _config;

        public SqliteDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public List<T> Query<T, U>(string sqlStatement, U parameters, string connectionStringName)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection conn = new SQLiteConnection(connectionString))
            {
                var rows = conn.Query<T>(sqlStatement, parameters)?.ToList();
                return rows;
            }
        }

        public void Execute<T>(string sqlStatement, T parameters, string connectionStringName)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection conn = new SQLiteConnection(connectionString))
            {
                _ = conn.Execute(sqlStatement, parameters);
            }
        }

        public void ExecuteMultiple<T>(string sqlStatement, List<T> parameters, string connectionStringName)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection conn = new SQLiteConnection(connectionString))
            {
                foreach(T t in parameters)
                {
                    _ = conn.Execute(sqlStatement, t);
                }
                
            }
        }


    }
}

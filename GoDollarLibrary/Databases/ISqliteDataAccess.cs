using System.Collections.Generic;

namespace GoDollarLibrary.Databases
{
    public interface ISqliteDataAccess
    {
        List<T> Query<T, U>(string sqlStatement, U parameters, string connectionStringName);
        void Execute<T>(string sqlStatement, T parameters, string connectionStringName);
    }
}
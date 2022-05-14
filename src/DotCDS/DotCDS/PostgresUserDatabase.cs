using System;
using DotCDS.DatabaseClient;

namespace DotCDS
{
	public class PostgresUserDatabase
	{
        #region Private Fields
        private string _connectionString;
        private string _dbName;
        private PostgresClient _client;
        #endregion

        #region Public Properties
        public string DatabaseName => _dbName;
        #endregion

        #region Constructors
        public PostgresUserDatabase(string connectionString)
		{
            _connectionString = connectionString;
            _dbName = PostgresConnectionParams.FromConnectionString(_connectionString).DatabaseName;
		}
        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion
    }
}


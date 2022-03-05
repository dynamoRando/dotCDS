using DotCDS.DatabaseClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;

namespace DotCDS.Database
{
    /// <summary>
    /// A backing library for interacting with a Sqlite database
    /// </summary>
    internal class SqliteClient : IDatabaseClient
    {
        #region Private Fields
        private string _connectionString;
        private string _backingDbName;
        private string _rootFolder;
        private const string _fileExtension = ".db3";
        private string _dbFileLocation;
        #endregion

        #region Public Properties
        public string ConnectionString => _connectionString;
        public string DbFileLocation => _dbFileLocation;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of the Sqlite client. If the backing database or structures in the backing database
        /// do not exist, it will create them.
        /// </summary>
        /// <param name="connectionString">The connection string for the database</param>
        /// <param name="backingDbName">The name of the backing database</param>
        /// <param name="rootFolder">The folder where the database exists</param>
        public SqliteClient(string connectionString, string backingDbName, string rootFolder)
        {
            _connectionString = connectionString;
            _backingDbName = backingDbName;
            _rootFolder = rootFolder;

            ConfigureBackingDb();
        }
        #endregion

        #region Public Methods
        public bool TryExecuteSqlStatement(string sqlStatement)
        {
            throw new NotImplementedException();
        }

        public bool TryCreateDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public bool TryCreateTable(string databaseName, string createTableStatement)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods
        private void ConfigureBackingDb()
        {
            _backingDbName += _fileExtension;
            _dbFileLocation = Path.Combine(_rootFolder, _backingDbName);

            if (!File.Exists(_dbFileLocation))
            {
                SQLiteConnection.CreateFile(_dbFileLocation);
            }
        }
        #endregion

    }
}

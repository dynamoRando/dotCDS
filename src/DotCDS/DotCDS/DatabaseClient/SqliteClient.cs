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
        public SqliteClient(string connectionString, string backingDbName, string rootFolder)
        {
            _connectionString = connectionString;
            _backingDbName = backingDbName;
            _rootFolder = rootFolder;

            ConfigureBackingDb();
        }
        #endregion

        #region Public Methods
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

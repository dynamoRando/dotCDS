using DotCDS.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS
{
    internal class SqliteUserDatabaseManager
    {
        #region Private Fields
        private List<SqliteUserDatabase> _userDatabases;
        private string _rootFolder;
        private SqliteClient _sqliteClient;
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        public SqliteUserDatabaseManager(string rootFolder)
        {
            _userDatabases = new List<SqliteUserDatabase>();
            _rootFolder = rootFolder;

            _sqliteClient = new SqliteClient(rootFolder);
        }
        #endregion

        #region Public Methods
        public SqliteUserDatabase? GetSqliteUserDatabase(string databaseName)
        {
            foreach (var database in _userDatabases)
            {
                if (string.Equals(database.DatabaseName, databaseName, StringComparison.OrdinalIgnoreCase))
                {
                    return database;
                }
            }

            return null;
        }

        public bool HasDatabase(string databaseName)
        {
            bool hasDatabase;

            hasDatabase = CollectionHasDatabase(databaseName);

            if (!hasDatabase)
            {
                hasDatabase = _sqliteClient.HasDatabase(databaseName);

                if (hasDatabase)
                {
                    var db = new SqliteUserDatabase(_rootFolder, databaseName);
                    _userDatabases.Add(db);
                }
            }

            return hasDatabase;
        }

        public ActionResult CreateUserDatabase(string databaseName)
        {
            ActionResult result = new ActionResult();
            if (!HasDatabase(databaseName))
            {
                try
                {
                    _sqliteClient.CreateDatabase(databaseName);
                    result.IsSuccessful = true;
                }
                catch (Exception ex)
                {
                    result.IsSuccessful = false;
                    result.Message = ex.Message;
                }
            }
            else
            {
                result.IsSuccessful = false;
                result.Message = "Database already exists";
            }

            return result;
        }
        #endregion

        #region Private Methods
        private bool CollectionHasDatabase(string name)
        {
            foreach (var database in _userDatabases)
            {
                if (string.Equals(database.DatabaseName, name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

    }
}

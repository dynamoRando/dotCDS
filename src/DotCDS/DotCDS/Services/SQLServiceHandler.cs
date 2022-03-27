using DotCDS.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Services
{
    internal class SQLServiceHandler
    {
        #region Private Fields
        private SqliteClient _sqliteClient;
        private SqliteCDSStore _cooperativeStore;
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        public void SetSqliteClient(SqliteClient client)
        {
            _sqliteClient = client;
        }

        public void SetCooperativeStore(SqliteCDSStore store)
        {
            _cooperativeStore = store;
        }

        public bool IsValidLogin(string un, string pw)
        {
            return _cooperativeStore.IsValidLogin(un, pw);
        }

        public bool HandleCreateDatabase(string un, string pw, string databaseName)
        {
            if (_cooperativeStore.IsValidLogin(un, pw))
            {
                if (_cooperativeStore.UserIsInRole(un, InternalSQLStatements.RoleNames.SYS_ADMIN))
                {
                    _sqliteClient.CreateDatabase(databaseName);
                    return true;
                }
            }

            return false;
        }

        public bool ExecuteStatement(string un, string pw, string dbName, string statement)
        {
            if (_cooperativeStore.IsValidLogin(un, pw))
            {
                if (_cooperativeStore.UserIsInRole(un, InternalSQLStatements.RoleNames.SYS_ADMIN))
                {

                }
            }

            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods
        #endregion

    }
}

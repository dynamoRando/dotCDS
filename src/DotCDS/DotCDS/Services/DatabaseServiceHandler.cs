using DotCDS.Common;
using DotCDS.Database;
using DotCDS.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotCDS.Mapper;

namespace DotCDS.Services
{
    internal class DatabaseServiceHandler
    {
        #region Private Fields
        private DatabaseServiceHandler _handler;
        private SqliteClient _sqliteClient;
        private SqliteCDSStore _cooperativeStore;
        private SqliteUserDatabaseManager _userDatabaseManager;
        private QueryParser _queryParser;
        private RemoteNetworkManager _remoteNetworkManager;
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        public void SetDatabaseServiceHandler(DatabaseServiceHandler handler)
        {
            _handler = handler;
        }

        public void SetQueryParser(QueryParser parser)
        {
            _queryParser = parser;
        }

        public void SetSqliteUserDatabaseManager(SqliteUserDatabaseManager manager)
        {
            _userDatabaseManager = manager;
        }

        public void SetSqliteClient(SqliteClient client)
        {
            _sqliteClient = client;
        }

        public void SetCooperativeStore(SqliteCDSStore store)
        {
            _cooperativeStore = store;
        }

        public void SetRemoteNetworkManager(RemoteNetworkManager remote)
        {
            _remoteNetworkManager = remote;
        }

        public bool IsValidLogin(string un, string pw)
        {
            return _cooperativeStore.IsValidLogin(un, pw);
        }

        public bool HandleSaveContract(Contract contract)
        {
            var isSuccessful = false;
            var dbContract = ContractMapper.Map(contract);
            isSuccessful = _cooperativeStore.SavePendingContract(dbContract);
            return isSuccessful;
        }

        #endregion

        #region Private Methods
        #endregion

    }
}

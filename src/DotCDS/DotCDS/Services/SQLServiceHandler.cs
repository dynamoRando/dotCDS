using DotCDS.Common;
using DotCDS.Common.Enum;
using DotCDS.Database;
using DotCDS.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotCDS.Enum;
using DotCDS.Model;

namespace DotCDS.Services
{
    internal class SQLServiceHandler
    {
        #region Private Fields
        // this is deprecated in favor of having a reference to SqliteUserDatabaseManager
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

        public ActionResult HandleCreateDatabase(string un, string pw, string databaseName)
        {
            ActionResult result = new ActionResult();

            if (_cooperativeStore.IsValidLogin(un, pw))
            {
                if (_cooperativeStore.UserIsInRole(un, InternalSQLStatements.RoleNames.SYS_ADMIN))
                {
                    result = _userDatabaseManager.CreateUserDatabase(databaseName);
                }
                else
                {
                    result.IsSuccessful = false;
                    result.Message = "User does not have sufficent permissions";
                }
            }
            else
            {
                result.IsSuccessful = false;
                result.Message = "Login failed";
            }

            return result;
        }

        public bool HandleHasTable(string un, string pw, string databaseName, string tableName)
        {
            if (_cooperativeStore.IsValidLogin(un, pw))
            {
                if (_cooperativeStore.UserIsInRole(un, InternalSQLStatements.RoleNames.SYS_ADMIN))
                {
                    return _sqliteClient.HasTable(databaseName, tableName);
                }
            }

            return false;
        }

        public uint ExecuteWrite(string un, string pw, string databaseName, string statement)
        {
            if (_cooperativeStore.IsValidLogin(un, pw))
            {
                if (_cooperativeStore.UserIsInRole(un, InternalSQLStatements.RoleNames.SYS_ADMIN))
                {
                    // we will need to change this to determine if this is a write
                    // if this is not a write, we need to return an error

                    return (uint)_sqliteClient.ExecuteWrite(databaseName, statement);
                }
            }

            return 0;
        }

        public StatementResultset ExecuteRead(string un, string pw, string databaseName, string statement)
        {
            var errorResult = new StatementResultset();
            if (_cooperativeStore.IsValidLogin(un, pw))
            {
                if (_cooperativeStore.UserIsInRole(un, InternalSQLStatements.RoleNames.SYS_ADMIN))
                {
                    // we will need to change this to determine if this is a cooperative read
                    // if this is not a read, we need to return an error

                    var isReadStatement = _queryParser.IsReadStatement(statement, databaseName);
                    if (isReadStatement.Result == false)
                    {
                        errorResult.IsError = true;
                        errorResult.ResultMessage = isReadStatement.Message;
                    }

                    var isCooperative = _queryParser.HasCooperativeObjects(statement, databaseName);
                    if (isCooperative.Result == true)
                    {
                        // we need to figure out how to send the query to all participants that are part of the query
                        var cooperativeParts = _queryParser.GetCooperativeReferences(statement, databaseName);

                        // build a 'shell' data table so that have a schema to populate
                        // note - this works because for any tables that have a logical storage policy other than 'host'
                        // we build basically an empty table locally so that we have a shell schema to work with
                        // but no data is actually stored locally, we gotta get the remote rows 
                        // and then populate the shell data table
                        DataTable result = _sqliteClient.ExecuteRead(databaseName, statement);

                        // there should be no data in this data table, should only be a shell final schema
                        if (result.Rows.Count > 0)
                        {
                            throw new InvalidOperationException("There exists data in shell schema");
                        }

                        // fetch the data from each participant via the cooperative parts variable above

                        // using the remote data rows, populate this in memory table and then return to the client
                    }
                    else
                    {
                        // this is a query to be ran locally
                        DataTable result = _sqliteClient.ExecuteRead(databaseName, statement);
                        return Resultset.ToStatementResultset(result);
                    }
                }
                else
                {
                    errorResult.IsError = true;
                    errorResult.ResultMessage = "Login does not have permission";
                }
            }
            else
            {
                errorResult.IsError = true;
                errorResult.ResultMessage = "Incorrect login";
            }

            return errorResult;
        }

        public bool HandleAcceptPendingContract(string hostAlias)
        {
            throw new NotImplementedException();
        }

        public bool HandleGenerateContract(string un, string pw, string hostName, string description, string databaseName, RemoteDeleteBehavior deleteBehavior)
        {
            bool isSuccessful = false;
            if (_cooperativeStore.IsValidLogin(un, pw))
            {
                if (_cooperativeStore.UserIsInRole(un, InternalSQLStatements.RoleNames.SYS_ADMIN))
                {
                    _cooperativeStore.GenerateHostInformation(hostName);

                    if (_userDatabaseManager.HasDatabase(databaseName))
                    {
                        var db = _userDatabaseManager.GetSqliteUserDatabase(databaseName);
                        isSuccessful = db.GenerateContract(description, deleteBehavior);
                    }
                }
            }

            return isSuccessful;
        }

        public LogicalStoragePolicy HandleGetLogicalStoragePolicy(string databaseName, string tableName)
        {
            throw new NotImplementedException();
        }

        public bool HandleAddParticipant(string un, string pw, string databaseName, string alias, string ipAddress, uint portNumber)
        {
            bool isSuccessful = false;
            if (_cooperativeStore.IsValidLogin(un, pw))
            {
                if (_cooperativeStore.UserIsInRole(un, InternalSQLStatements.RoleNames.SYS_ADMIN))
                {

                    if (_userDatabaseManager.HasDatabase(databaseName))
                    {
                        var db = _userDatabaseManager.GetSqliteUserDatabase(databaseName);
                        isSuccessful = db.AddParticipant(alias, ipAddress, portNumber);
                    }
                }
            }

            return isSuccessful;
        }

        public uint HandleCooperativeWrite(string un, string pw, string alias, Guid participantId, string databaseName, string statement)
        {
            var errorResult = new StatementResultset();
            if (_cooperativeStore.IsValidLogin(un, pw))
            {
                if (_cooperativeStore.UserIsInRole(un, InternalSQLStatements.RoleNames.SYS_ADMIN))
                {
                    // 

                }
                else
                {
                    errorResult.IsError = true;
                    errorResult.ResultMessage = "Login does not have permission";
                }
            }
            else
            {
                errorResult.IsError = true;
                errorResult.ResultMessage = "Incorrect login";
            }

            throw new NotImplementedException();
        }

        public bool HandleRejectPendingContract(string alias)
        {
            throw new NotImplementedException();
        }

        public Contract[] HandleViewPendingContracts()
        {
            throw new NotImplementedException();
        }

        public bool HandleSendContractToParticipant(string alias, string databaseName, string un, string pw)
        {
            bool isSuccessful = false;
            if (_cooperativeStore.IsValidLogin(un, pw))
            {
                if (_cooperativeStore.UserIsInRole(un, InternalSQLStatements.RoleNames.SYS_ADMIN))
                {
                    if (_userDatabaseManager.HasDatabase(databaseName))
                    {
                        var db = _userDatabaseManager.GetSqliteUserDatabase(databaseName);
                        DatabaseContract contract = db.GetActiveContract(true);
                        contract.Status = Common.Enum.ContractStatus.Pending;
                        var participant = db.GetDatabaseParticipant(alias);
                        var hostInfo = _cooperativeStore.GetHostInformation();
                        isSuccessful = _remoteNetworkManager.SendContractToParticipant(participant, contract, hostInfo);

                        throw new NotImplementedException();
                    }
                }
            }

            throw new NotImplementedException();
        }

        public bool HandleSetLogicalStoragePolicy(string un, string pw, string databaseName, string tableName, LogicalStoragePolicy policy)
        {
            bool isSuccessful = false;
            if (_cooperativeStore.IsValidLogin(un, pw))
            {
                if (_cooperativeStore.UserIsInRole(un, InternalSQLStatements.RoleNames.SYS_ADMIN))
                {
                    if (_userDatabaseManager.HasDatabase(databaseName))
                    {
                        var db = _userDatabaseManager.GetSqliteUserDatabase(databaseName);
                        isSuccessful = db.SetLogicalStoragePolicy(tableName, policy);
                    }
                }
            }

            return isSuccessful;
        }

        public bool HandleEnableCooperativeFeatures(string un, string pw, string databaseName)
        {
            bool isSuccessful = false;

            if (_cooperativeStore.IsValidLogin(un, pw))
            {
                if (_cooperativeStore.UserIsInRole(un, InternalSQLStatements.RoleNames.SYS_ADMIN))
                {
                    if (_userDatabaseManager.HasDatabase(databaseName))
                    {
                        var db = _userDatabaseManager.GetSqliteUserDatabase(databaseName);
                        var result = db.EnableCooperativeFeatures();
                        if (result.Status == ActionOptionalResultStatus.Success ||
                            result.Status == ActionOptionalResultStatus.Information)
                        {
                            isSuccessful = true;
                        }
                    }
                }
            }

            return isSuccessful;
        }
        #endregion

        #region Private Methods
        #endregion

    }
}

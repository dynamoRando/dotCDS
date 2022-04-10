using DotCDS.Common;
using DotCDS.Common.Enum;
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
            return (uint)_sqliteClient.ExecuteWrite(databaseName, statement);
        }

        public StatementResultset ExecuteRead(string un, string pw, string databaseName, string statement)
        {
            var errorResult = new StatementResultset();
            if (_cooperativeStore.IsValidLogin(un, pw))
            {
                if (_cooperativeStore.UserIsInRole(un, InternalSQLStatements.RoleNames.SYS_ADMIN))
                {
                    var result = _sqliteClient.ExecuteRead(databaseName, statement);
                    return Resultset.ToStatementResultset(result);

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

        public bool HandleGenerateContract(string hostName, string description, string databaseName)
        {
            throw new NotImplementedException();
        }

        public LogicalStoragePolicy HandleGetLogicalStoragePolicy(string databaseName, string tableName)
        {
            throw new NotImplementedException();
        }

        public bool HandleAddParticipant(string databaseName, string alias, string ipAddress, uint portNumber)
        {
            throw new NotImplementedException();
        }

        public uint HandleCooperativeWrite(string un, string pw, string alias, Guid participantId, string databaseName, string statement)
        {
            var errorResult = new StatementResultset();
            if (_cooperativeStore.IsValidLogin(un, pw))
            {
                if (_cooperativeStore.UserIsInRole(un, InternalSQLStatements.RoleNames.SYS_ADMIN))
                {
                    //var result = _sqliteClient.ExecuteRead(databaseName, statement);
                    //return Resultset.ToStatementResultset(result);

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

        public bool HandleSetLogicalStoragePolicy(string un, string pw, string databaseName, string tableName)
        {
            throw new NotImplementedException();
        }

        public bool HandleEnableCooperativeFeatures(string un, string pw, string databaseName)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods
        #endregion

    }
}

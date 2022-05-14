using DotCDS.DatabaseClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using DotCDS.Model;
using static DotCDS.InternalSQLStatements;

namespace DotCDS
{
    /// <summary>
    /// A backing library for working with a Postgres database
    /// </summary>
    internal class PostgresCDSStore : ICooperativeStore
    {
        #region Private Fields
        private string _connectionString;
        private PostgresClient _client;
        private const string _CDS = "cds";
        #endregion

        #region Public Properties
        public string ConnectionString => _connectionString;
        #endregion

        #region Constructors
        public PostgresCDSStore(string connectionString)
        {
            _connectionString = connectionString;
            _client = new PostgresClient(_connectionString);

            ConfigureBackingDb();
        }
        #endregion

        #region Public Methods
        public void SetSQLSettings(PortSettings settings)
        {
            throw new NotImplementedException();
        }

        public void SetDataSettings(PortSettings settings)
        {
            throw new NotImplementedException();
        }

        public bool HasLogin(string loginName)
        {
            throw new NotImplementedException();
        }

        public bool CreateLogin(string userName, string pw)
        {
            throw new NotImplementedException();
        }

        public bool AddUserToRole(string userName, string roleNmae)
        {
            throw new NotImplementedException();
        }

        public bool IsValidLogin(string userName, string pw)
        {
            throw new NotImplementedException();
        }

        public bool UserIsInRole(string userName, string roleName)
        {
            throw new NotImplementedException();
        }

        public bool HasTable(string tableName)
        {
            return _client.HasTable(_CDS, tableName);
        }

        public bool HasRole(string roleName)
        {
            string sql = Postgres.COUNT_OF_ROLES_WITH_NAME.Replace("role_name", roleName);
            var dt = _client.ExecuteRead(sql);
            int totalRows = Convert.ToInt32(dt.Rows[0]["ROLECOUNT"]);
            return totalRows > 0;
        }

        public bool GenerateHostInformation(string hostName)
        {
            throw new NotImplementedException();
        }

        public DatabaseHostInfo GetHostInformation()
        {
            throw new NotImplementedException();
        }

        public bool SavePendingContract(DatabaseContract contract)
        {
            throw new NotImplementedException();
        }

        public DatabaseContract[] GetPendingContracts()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods
        private void CreateUserTable()
        {
            if (!HasTable(TableNames.CDS.USER))
            {
                _client.ExecuteWrite(Postgres.CREATE_USER_TABLE);
            }
        }

        private void CreateRoleTable()
        {
            if (!HasTable(TableNames.CDS.ROLE))
            {
                _client.ExecuteWrite(Postgres.CREATE_ROLE_TABLE);
            }
        }

        private void CreateUserRoleTable()
        {
            if (!HasTable(TableNames.CDS.USER_ROLE))
            {
                _client.ExecuteWrite(Postgres.CREATE_USER_ROLE_TABLE);
            }
        }

        private void CreateHostInfoTable()
        {
            if (!HasTable(TableNames.CDS.HOST_INFO))
            {
                _client.ExecuteWrite(Postgres.CREATE_HOST_INTO_TABLE);
            }
        }

        private void CreateContractsTables()
        {
            if (!HasTable(TableNames.CDS.CONTRACTS))
            {
                _client.ExecuteRead(Postgres.CREATE_CDS_CONTRACTS_TABLE);
            }

            if (!HasTable(TableNames.CDS.CONTRACTS_TABLES))
            {
                _client.ExecuteRead(Postgres.CREATE_CDS_CONTRACTS_TABLE_TABLE);
            }

            if (!HasTable(TableNames.CDS.CONTRACTS_TABLE_SCHEMAS))
            {
                _client.ExecuteRead(Postgres.CREATE_CDS_CONTRACTS_TABLE_SCHEMA_TABLE);
            }
        }

        private void ConfigureBackingDb()
        {
            CreateUserTable();
            CreateRoleTable();
            CreateUserRoleTable();
            CreateHostInfoTable();
            CreateContractsTables();

            if (!HasRole(RoleNames.SYS_ADMIN))
            {
                _client.ExecuteWrite(SQLLite.ADD_ADMIN_ROLE);
            }
        }
        #endregion

    }
}

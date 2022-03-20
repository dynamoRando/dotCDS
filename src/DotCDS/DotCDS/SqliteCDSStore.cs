using DotCDS.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DotCDS.InternalSQLStatements;
using DotCDS.DatabaseClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using System.Data;
using static DotCDS.InternalSQLStatements;


namespace DotCDS
{
    /// <summary>
    /// Represents interacting with a SQLite instance of a CDS store
    /// </summary>
    internal class SqliteCDSStore : ICooperativeStore
    {
        #region Private Fields
        private SqliteClient _client;
        private string _connectionString;
        private string _backingDbName;
        private string _rootFolder;
        private const string _fileExtension = ".db";
        private string _dbFileLocation;
        private Crypto _crypt = new Crypto();
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        public SqliteCDSStore(string backingDbName, string rootFolder)
        {
            _client = new SqliteClient(rootFolder);
            _rootFolder = rootFolder;
            _backingDbName = backingDbName;

            ConfigureBackingDb();
        }
        #endregion

        #region Public Methods
        public bool HasLogin(string loginName)
        {
            string sql = SQLLite.COUNT_OF_USERS_WITH_NAME.Replace("user_name", loginName);
            var dt = _client.ExecuteRead(_backingDbName, sql);
            int totalRows = Convert.ToInt32(dt.Rows[0]["USERCOUNT"]);
            return totalRows > 0;
        }

        public bool CreateLogin(string userName, string pw)
        {
            if (!HasLogin(userName))
            {
                int iterations = _crypt.GetRandomNumber();
                int length = _crypt.GetByteLength();

                var pwByte = Encoding.ASCII.GetBytes(pw);

                var salt = _crypt.GenerateSalt(length);
                var hash = _crypt.GenerateHash(pwByte, salt, iterations, length);

                var dict = new Dictionary<string, object>();
                dict.Add("@username", userName);
                dict.Add("@bytelength", length);
                dict.Add("@salt", salt);
                dict.Add("@hash", hash);
                dict.Add("@workfactor", iterations);

                var result = _client.ExecuteWrite(_backingDbName, SQLLite.ADD_LOGIN, dict);

                return result > 0;
            }

            return false;
        }

        public void AddAdminRole()
        {
            _client.ExecuteWrite(_backingDbName, SQLLite.ADD_ADMIN_ROLE);
        }

        public bool AddUserToRole(string userName, string roleName)
        {
            var dict = new Dictionary<string, object>();
            dict.Add("@username", userName);
            dict.Add("@rolename", roleName);

            var result = _client.ExecuteWrite(_backingDbName, SQLLite.ADD_USER_TO_ROLE, dict);

            if (result > 0)
            { 
                return true; 
            }

            return false;
        }

        public bool IsValidLogin(string userName, string pw)
        {
            var dict = new Dictionary<string, object>();
            dict.Add("@username", userName);

            DataTable result = _client.ExecuteRead(_backingDbName, SQLLite.GET_LOGIN, dict);
            if (result.Rows.Count > 0)
            {
                var pwByte = Encoding.ASCII.GetBytes(pw);

                foreach (DataRow row in result.Rows)
                {
                    string? un = Convert.ToString(row["USERNAME"]);
                    int byteLength = Convert.ToInt32(row["BYTELENGTH"]);
                    byte[] salt = (byte[])row["SALT"];
                    byte[] hash = (byte[])row["HASH"];
                    int workFactor = Convert.ToInt32(row["WORKFACTOR"]);

                    var computedHash = _crypt.GenerateHash(pwByte, salt, workFactor, byteLength);

                    if (hash.SequenceEqual(computedHash))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool UserIsInRole(string userName, string roleName)
        {
            var dict = new Dictionary<string, object>();
            dict.Add("@username", userName);
            dict.Add("@rolename", roleName);

            var dt = _client.ExecuteRead(_backingDbName, SQLLite.COUNT_OF_USER_WITH_ROLE, dict);
            int totalRows = Convert.ToInt32(dt.Rows[0]["TOTALCOUNT"]);
            return totalRows > 0;
        }

        public bool HasTable(string tableName)
        {
            string sql = SQLLite.COUNT_OF_TABLES_WITH_NAME.Replace("table_name", tableName);
            var dt = _client.ExecuteRead(_backingDbName, sql);
            int totalRows = Convert.ToInt32(dt.Rows[0]["TABLECOUNT"]);
            return totalRows > 0;
        }

        public bool HasRole(string roleName)
        {
            string sql = SQLLite.COUNT_OF_ROLES_WITH_NAME.Replace("role_name", roleName);
            var dt = _client.ExecuteRead(_backingDbName, sql);
            int totalRows = Convert.ToInt32(dt.Rows[0]["ROLECOUNT"]);
            return totalRows > 0;
        }

        #endregion

        #region Private Methods
        private void CreateUserTable()
        {
            if (!HasTable(TableNames.CDS_USER))
            {
                _client.ExecuteWrite(_backingDbName, SQLLite.CREATE_USER_TABLE);
            }
        }

        private void CreateRoleTable()
        {
            if (!HasTable(TableNames.CDS_ROLE))
            {
                _client.ExecuteWrite(_backingDbName, SQLLite.CREATE_ROLE_TABLE);
            }
        }

        private void CreateUserRoleTable()
        {
            if (!HasTable(TableNames.CDS_USER_ROLE))
            {
                _client.ExecuteWrite(_backingDbName, SQLLite.CREATE_USER_ROLE_TABLE);
            }
        }

        private void ConfigureBackingDb()
        {
            _backingDbName += _fileExtension;
            _dbFileLocation = Path.Combine(_rootFolder, _backingDbName);

            if (!File.Exists(_dbFileLocation))
            {
                SQLiteConnection.CreateFile(_dbFileLocation);
            }

            CreateUserTable();
            CreateRoleTable();
            CreateUserRoleTable();

            if (!HasRole(RoleNames.SYS_ADMIN))
            {
                _client.ExecuteWrite(_backingDbName, SQLLite.ADD_ADMIN_ROLE);
            }
        }
        #endregion
    }
}

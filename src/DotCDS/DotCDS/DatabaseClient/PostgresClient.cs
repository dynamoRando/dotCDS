using System;
using Npgsql;
using System.Data;

namespace DotCDS.DatabaseClient
{
    internal class PostgresClient
    {
        #region Private Fields
        private string _connectionString = string.Empty;
        #endregion

        #region Public Properties
        public string ConnectionString => _connectionString;
        #endregion

        #region Constructor
        public PostgresClient(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region Public Methods
        public static string BuildConnectionString(string host, string un, string pw, string dbName)
        {
            return $"Host={host};Username={un};Password={pw};Database={dbName}";
        }

        public void CreateDatabase(string dbName)
        {
            string sql = $"CREATE DATABASE {dbName};";
            ExecuteWrite(sql);
        }

        public bool HasSchema(string schemaName)
        {
            bool result = false;

            string sql = $@"select exists(SELECT schema_name FROM information_schema.schemata WHERE schema_name = '{schemaName}');";

            DataTable dtResult = ExecuteRead(sql);

            if (dtResult.Rows.Count > 0)
            {
                result = Convert.ToBoolean(dtResult.Rows[0][0]);
            }

            return result;
        }

        public bool HasDatabase(string dbName)
        {
            bool result = false;
            string sql = $@"select exists(
             SELECT datname FROM pg_catalog.pg_database WHERE lower(datname) = lower('{dbName}')
            );";

            DataTable dtResult = ExecuteRead(sql);

            if (dtResult.Rows.Count > 0)
            {
                result = Convert.ToBoolean(dtResult.Rows[0][0]);
            }

            return result;
        }

        /// <summary>
        /// Returns the schema for the specified table
        /// </summary>
        /// <param name="schemaName">The schema the table is in</param>
        /// <param name="tableName">The name of the table</param>
        /// <returns>A data table representing the schema of the table. The column orders are:
        /// [0] - tableName,
        /// [1] - columnName,
        /// [2] - ordinal,
        /// [3] - isNullable,
        /// [4] - dataType,
        /// [5] - characterMaxLength
        /// [6] - user defined type name
        /// [7] - isIdentity
        /// </returns>
        public DataTable GetSchemaForTable(string schemaName, string tableName)
        {
            DataTable result = null;

            string sql = $@"
            SELECT 
	            s.table_name,
	            s.column_name,
	            s.ordinal_position,
	            s.is_nullable,
	            s.data_type,
	            s.character_maximum_length,
	            s.udt_name,
	            s.is_identity 
            FROM information_schema.columns s 
            WHERE TABLE_NAME = '{tableName}'
            AND TABLE_SCHEMA = '{schemaName}'
            ";

            result = ExecuteRead(sql);

            return result;
        }

        public bool HasTable(string schemaName, string tableName)
        {
            bool result = false;
            string sql = @$"SELECT EXISTS (
                                SELECT FROM
                                    pg_tables
                                WHERE
                                    schemaname = '{schemaName}' AND
                                    tablename = '{tableName}'
                            ); ";

            DataTable dtResult = ExecuteRead(sql);

            if (dtResult.Rows.Count > 0)
            {
                result = Convert.ToBoolean(dtResult.Rows[0][0]);
            }

            return result;
        }

        public DataTable ExecuteRead(string query, Dictionary<string, object> args)
        {
            DataTable result = null;

            if (!string.IsNullOrEmpty(_connectionString))
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var comm = new NpgsqlCommand(query, conn))
                    {
                        foreach (var arg in args)
                        {
                            comm.Parameters.AddWithValue(arg.Key, arg.Value);
                        }

                        comm.Prepare();

                        var da = new NpgsqlDataAdapter(query, _connectionString);
                        result = new DataTable();
                        da.Fill(result);
                        da.Dispose();
                    }
                }
            }

            return result;
        }

        public DataTable ExecuteRead(string query)
        {
            DataTable result = null;

            if (!string.IsNullOrEmpty(_connectionString))
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var comm = new NpgsqlCommand(query, conn))
                    {
                        var da = new NpgsqlDataAdapter(query, _connectionString);
                        result = new DataTable();
                        da.Fill(result);
                        da.Dispose();
                    }
                }
            }

            return result;
        }

        public int ExecuteWrite(string query)
        {
            int results = 0;

            if (!string.IsNullOrEmpty(_connectionString))
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var comm = new NpgsqlCommand(query, conn))
                    {
                        results = comm.ExecuteNonQuery();
                    }
                }
            }

            return results;
        }

        public int ExecuteWrite(string query, Dictionary<string, object> args)
        {
            int results = 0;

            if (!string.IsNullOrEmpty(_connectionString))
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var comm = new NpgsqlCommand(query))
                    {
                        foreach (var arg in args)
                        {
                            comm.Parameters.AddWithValue(arg.Key, arg.Value);
                        }

                        comm.Prepare();

                        results = comm.ExecuteNonQuery();
                    }
                }
            }

            return results;
        }
        #endregion

        #region Private Methods
        #endregion
    }
}


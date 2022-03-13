﻿using DotCDS.DatabaseClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using System.Data;

namespace DotCDS.Database
{
    /// <summary>
    /// A backing library for interacting with a Sqlite database
    /// </summary>
    internal class SqliteClient : IDatabaseClient
    {
        /*
         * https://devtut.github.io/csharp/using-sqlite-in-c.html#creating-simple-crud-using-sqlite-in-c
         * https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli
         */

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
        /// <summary>
        /// Executes an INSERT, UPDATE, DELETE statement with the specified values
        /// </summary>
        /// <param name="dbName">The name of the db to execute the statement against</param>
        /// <param name="query">The SQL query statement</param>
        /// <param name="args">The args to replace</param>
        /// <returns>The number of rows affected</returns>
        /// <remarks>Example: "INSERT INTO User(FirstName, LastName) VALUES(@firstName, @lastName)"</remarks>
        public int ExecuteWrite(string dbName, string query, Dictionary<string, object> args)
        {
            int numberOfRowsAffected;

            //setup the connection to the database
            using (var con = new SQLiteConnection($"Data Source={dbName}.db"))
            {
                con.Open();

                //open a new command
                using (var cmd = new SQLiteCommand(query, con))
                {
                    //set the arguments given in the query
                    foreach (var pair in args)
                    {
                        cmd.Parameters.AddWithValue(pair.Key, pair.Value);
                    }

                    //execute the query and get the number of row affected
                    numberOfRowsAffected = cmd.ExecuteNonQuery();
                }

                return numberOfRowsAffected;
            }
        }

        /// <summary>
        /// Executes an INSERT, UPDATE, DELETE statement with the specified values 
        /// </summary>
        /// <param name="dbName">The name of the database to execute the query against</param>
        /// <param name="query">The SQL query statement</param>
        /// <returns>The number of rows affected</returns>
        public int ExecuteWrite(string dbName, string query)
        {
            int numberOfRowsAffected;

            //setup the connection to the database
            using (var con = new SQLiteConnection($"Data Source={dbName}.db"))
            {
                con.Open();

                //open a new command
                using (var cmd = new SQLiteCommand(query, con))
                {
                    //execute the query and get the number of row affected
                    numberOfRowsAffected = cmd.ExecuteNonQuery();
                }

                return numberOfRowsAffected;
            }
        }

        /// <summary>
        /// Executes a SELECT statement with the specified values
        /// </summary>
        /// <param name="dbName">The name of the database to execute the query against</param>
        /// <param name="query">The SELECT statement</param>
        /// <param name="args">The args to replace</param>
        /// <remarks>Example: "SELECT * FROM User WHERE Id = @id"</remarks>
        /// <returns></returns>
        public DataTable ExecuteRead(string dbName, string query, Dictionary<string, object> args)
        {
            if (string.IsNullOrEmpty(query.Trim()))
                return null;

            using (var con = new SQLiteConnection($"Data Source={dbName}.db"))
            {
                con.Open();
                using (var cmd = new SQLiteCommand(query, con))
                {
                    foreach (KeyValuePair<string, object> entry in args)
                    {
                        cmd.Parameters.AddWithValue(entry.Key, entry.Value);
                    }

                    var da = new SQLiteDataAdapter(cmd);

                    var dt = new DataTable();
                    da.Fill(dt);

                    da.Dispose();
                    return dt;
                }
            }
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

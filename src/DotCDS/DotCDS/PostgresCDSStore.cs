using DotCDS.DatabaseClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Database
{
    /// <summary>
    /// A backing library for working with a Postgres database
    /// </summary>
    internal class PostgresClient : ICooperativeStore
    {
        #region Private Fields
        private string _connectionString;
        #endregion

        #region Public Properties
        public string ConnectionString => _connectionString;
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion

    }
}

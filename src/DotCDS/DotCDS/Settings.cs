using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS
{
    /// <summary>
    /// Represents a CDS appsettings.json file
    /// </summary>
    public class Settings
    {
        #region Private Fields
        private string _rootFolder = string.Empty;
        #endregion

        #region Public Properties
        /// <summary>
        /// The default IP Address to use for bringing online services
        /// </summary>
        public string DefaultIP4 { get; set; }

        /// <summary>
        /// The port used for SQL statements to be submitted to
        /// </summary>
        public int SQLServicePort { get; set; }

        /// <summary>
        /// The port used for database actions to be submitted to
        /// </summary>
        public int DatabaseServicePort { get; set; }

        /// <summary>
        /// The port use for admin functions for a CDS instance
        /// </summary>
        public int AdminServicePort { get; set; }

        /// <summary>
        /// The root folder the application should use for settings.
        /// If blank, this is the same folder the app is located in.
        /// </summary>
        public string RootFolder
        {
            get
            {
                return GetDatabaseFolder();
            }
            set
            {
                _rootFolder = value;
            }
        }

        /// <summary>
        /// The type of database that is backing dotCDS. See <see cref="DatabaseClientType"/> for more information.
        /// </summary>
        public int DatabaseClientType { get; set; }

        /// <summary>
        /// The schema name that dotCDS will use for storing information in each database that needs cooperative features.
        /// </summary>
        public string CoopSchemaName { get; set; }

        /// <summary>
        /// The database name that dotCDS uses for all internal service actions.
        /// </summary>
        public string BackingDatabaseName { get; set; }
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        private string GetDatabaseFolder()
        {
            if (string.IsNullOrEmpty(_rootFolder))
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            else
            {
                return _rootFolder;
            }
        }
        #endregion
    }
}

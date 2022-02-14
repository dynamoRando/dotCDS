using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS
{
    internal class Settings
    {
        #region Private Fields
        private string _rootFolder = string.Empty;
        #endregion

        #region Public Properties
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
        public int DatabaseClientType { get; set; }
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.Tests
{
    /// <summary>
    /// Used to support testing of a single instance of the dotCDS service
    /// </summary>
    public class SingleHarness
    {
        #region Private Fields
        private string _rootFolder;
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a testing harness for a single instance of dotCDS with the specific root folder directory
        /// </summary>
        /// <param name="rootFolder"></param>
        public SingleHarness(string rootFolder)
        {
            _rootFolder = rootFolder;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Configures the root folder for testing. If there are any files in the root folder, it will delete them. Otherwise, if the directory does 
        /// not exist, it will create it.
        /// </summary>
        public void SetupTempFolder()
        {
            var directory = new DirectoryInfo(_rootFolder);
            if (directory.Exists)
            {
                foreach (var file in directory.GetFiles())
                {
                    file.Delete();
                }
            }
            else
            {
                directory.Create();
            }
        }

        #endregion

        #region Private Methods
        #endregion

    }
}

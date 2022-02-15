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
        public SingleHarness(string rootFolder)
        {
            _rootFolder = rootFolder;
        }
        #endregion

        #region Public Methods
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

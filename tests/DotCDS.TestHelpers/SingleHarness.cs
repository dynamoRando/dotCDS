using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DotCDS;
using DotCDS.TestHelpers;

namespace DotCDS.Tests
{
    /// <summary>
    /// Used to support testing of a single instance of the dotCDS service
    /// </summary>
    public class SingleHarness
    {
        #region Private Fields
        private string _rootFolder;
        private Process _process;
        private int _adminPortNumber;
        private int _databasePortNumber;
        private int _sqlPortNumber;
        #endregion

        #region Public Properties
        public string RootFolder => _rootFolder;
        public int AdminPortNumber => _adminPortNumber;
        public int DatabasePortNumber => _databasePortNumber;
        public int SqlPortNumber => _sqlPortNumber;
        public Process Process => _process;
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

        public SingleHarness(bool useTempFolder, [CallerMemberName] string memberName = "")
        {
            if (useTempFolder)
            {
                _rootFolder = Path.Combine(TestConstants.TEST_TEMP_FOLDER, memberName);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns a path to the temp folder iwth the calling function name as the name of the folder.
        /// If the folder already exists, it will clear all files in it.
        /// </summary>
        /// <param name="memberName">The name of the calling function</param>
        /// <returns>A path to an empty test directory</returns>
        public static string GetAndConfigureTempFolder([CallerMemberName] string memberName = "")
        {
            string rootFolder = Path.Combine(TestConstants.TEST_TEMP_FOLDER, memberName);
            SetupTemporaryFolder(rootFolder);

            return rootFolder;
        }

        /// <summary>
        /// Returns a path to the temp folder with the calling function name as the name of the folder
        /// </summary>
        /// <param name="memberName">The name of the calling function</param>
        /// <returns>A path to an empty test directory</returns>
        public static string GetTestTempFolder([CallerMemberName] string memberName = "")
        {
            return Path.Combine(TestConstants.TEST_TEMP_FOLDER, memberName);
        }

        /// <summary>
        /// Deletes all files in the specified directory
        /// </summary>
        /// <param name="rootFolder">The folder to clear out</param>
        public static void SetupTemporaryFolder(string rootFolder)
        {
            var directory = new DirectoryInfo(rootFolder);
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

        /// <summary>
        /// Brings online a new instance of DotCDS and configures the root folder 
        /// and the next available ports for the test
        /// </summary>
        /// <remarks>This will default to NOT using HTTPS for the test</remarks>
        public void BringOnline()
        {
            SetupTempFolder();
            _process = new Process(_rootFolder);
            _process.Start();

            _adminPortNumber = TestPortManager.GetNextAvailablePortNumber();
            _databasePortNumber = TestPortManager.GetNextAvailablePortNumber();
            _sqlPortNumber = TestPortManager.GetNextAvailablePortNumber();

            _process.StartAdminService(_adminPortNumber, false);
            _process.StartDatabaseService(_databasePortNumber, false);
            _process.StartSQLService(_sqlPortNumber, false);
        }

        public void BringOnline(string adminUn, string adminPw)
        {
            SetupTempFolder();
            _process = new Process(_rootFolder);
            _process.Start();
            _process.Test_SetupAdmin(adminUn, adminPw);

            _adminPortNumber = TestPortManager.GetNextAvailablePortNumber();
            _databasePortNumber = TestPortManager.GetNextAvailablePortNumber();
            _sqlPortNumber = TestPortManager.GetNextAvailablePortNumber();

            _process.StartAdminService(_adminPortNumber, false);
            _process.StartDatabaseService(_databasePortNumber, false);
            _process.StartSQLService(_sqlPortNumber, false);
        }

        #endregion

        #region Private Methods
        #endregion
    }
}

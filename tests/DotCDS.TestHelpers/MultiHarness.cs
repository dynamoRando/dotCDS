using DotCDS.Client;
using DotCDS.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DotCDS.TestHelpers
{
    public class MultiHarness
    {
        #region Private Fields
        private string _rootFolder;
        private List<ProcessContainer> _processContainers;
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        public MultiHarness(string rootFolder)
        {
            _rootFolder = rootFolder;
            _processContainers = new List<ProcessContainer>();
        }

        public MultiHarness(bool useTempFolder, [CallerMemberName] string memberName = "")
        {
            if (useTempFolder)
            {
                _rootFolder = Path.Combine(TestConstants.TEST_TEMP_FOLDER, memberName);
            }

            _processContainers = new List<ProcessContainer>();
        }
        #endregion

        #region Public Methods
        public void AddProcess(string name)
        {
            ProcessContainer processContainer = new ProcessContainer();
            processContainer.Name = name;
            processContainer.Index = _processContainers.Count + 1;
            processContainer.Address = "http://localhost";
            _processContainers.Add(processContainer);
        }

        public void StartAllProcesses(string adminUn, string adminPw)
        {
            foreach (var processContainer in _processContainers)
            {
                var process = processContainer.Process;
                var client = processContainer.Client;

                var path = Path.Combine(_rootFolder, processContainer.Index.ToString());
                SetupTempFolder(path);

                if (process is null)
                {
                    process = new Process(path);
                }

                if (client is null)
                {
                    client = new StoreClient();
                }

                process.Start();
                process.Test_SetupAdmin(adminUn, adminPw);

                var adminPortNumber = TestPortManager.GetNextAvailablePortNumber();
                var databasePortNumber = TestPortManager.GetNextAvailablePortNumber();
                var sqlPortNumber = TestPortManager.GetNextAvailablePortNumber();

                processContainer.AdminPortNumber = adminPortNumber;
                processContainer.DatabasePortNumber = databasePortNumber;
                processContainer.SqlPortNumber = sqlPortNumber;

                process.StartAdminService(adminPortNumber, false);
                process.StartDatabaseService(databasePortNumber, false);
                process.StartSQLService(sqlPortNumber, false);

                client.Configure(processContainer.Address, sqlPortNumber);

                processContainer.Process = process;
                processContainer.Client = client;
            }
        }

        public int GetProcessAdminPort(string name)
        {
            var container = GetProcessContainer(name);
            return container.AdminPortNumber;
        }

        public int GetProcessSqlPort(string name)
        {
            var container = GetProcessContainer(name);
            return container.SqlPortNumber;
        }

        public int GetProcessDatabasePort(string name)
        {
            var container = GetProcessContainer(name);
            return container.DatabasePortNumber;
        }

        public Process GetProcess(string name)
        {
            return GetProcessContainer(name).Process;
        }

        public ProcessContainer GetProcessContainer(string name)
        {
            foreach (var process in _processContainers)
            {
                if (string.Equals(process.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return process;
                }
            }

            return null;
        }

        public StoreClient GetClient(string name)
        {
            return GetProcessContainer(name).Client;
        }
        #endregion

        #region Private Methods
        private void SetupTempFolder(string path)
        {
            var directory = new DirectoryInfo(path);
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
    }
}

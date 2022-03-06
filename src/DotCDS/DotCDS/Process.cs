using DotCDS.Database;
using DotCDS.DatabaseClient;

namespace DotCDS
{
    /// <summary>
    /// Represents the root CDS process
    /// </summary>
    public class Process
    {
        #region Private Fields
        private Settings _settings;
        private Configurator _configurator;
        private string _rootPath;
        private bool _overrideDefaultDb = false;
        private DatabaseClientType _clientType;
        private string _connectionString;
        private IDatabaseClient _cooperativeStore;
        private NetworkManager _networkManager;
        #endregion

        #region Public Properties
        public Settings Settings => _settings;
        #endregion

        #region Constructors
        public Process()
        {
            _networkManager = new NetworkManager();
        }

        public Process(string rootPath) : this()
        {
            _rootPath = rootPath;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Loads the appsettings.json file and attempts to startup the services
        /// </summary>
        public void Start()
        {
            LoadConfiguration();
            ConfigureBackingStore();
        }

        /// <summary>
        /// Loads the appsettings.json file starts the services and overrides the 
        /// appsettings.json backing database type with the supplied connection string. Used in testing.
        /// </summary>
        /// <param name="connectionString">The connection string to the database</param>
        /// <param name="databaseType">The type of database conection string</param>
        public void Start(string connectionString, DatabaseClientType databaseType)
        {
            _connectionString = connectionString;
            _clientType = databaseType;
            _overrideDefaultDb = true;

            LoadConfiguration();
            ConfigureBackingStore();
        }

        /// <summary>
        /// Used in xunit tests to make sure we can access the setting
        /// </summary>
        /// <returns></returns>
        public string TestConnectionString()
        {
            return _configurator.TestDefaultConnection();
        }

        public void StartDatabaseService()
        {
            PortSettings settings = new PortSettings();
            settings.IPAddress = _settings.DefaultIP4;
            settings.PortNumber = _settings.DatabaseServicePort;

            _networkManager.StartServerForDatabaseService(settings, true);
        }

        public void StartDatabaseService(int overrideSettingsPortNumber, bool overrideSettingsUseHttps)
        {
            PortSettings settings = new PortSettings();
            settings.IPAddress = _settings.DefaultIP4;
            settings.PortNumber = overrideSettingsPortNumber;

            _networkManager.StartServerForDatabaseService(settings, overrideSettingsUseHttps);
        }

        public void StartSQLService()
        {
            PortSettings settings = new PortSettings();
            settings.IPAddress = _settings.DefaultIP4;
            settings.PortNumber = _settings.SQLServicePort;

            _networkManager.StartServerForSqlService(settings, true);
        }

        public void StartSQLService(int overrideSettingsPortNumber, bool overrideSettingsUseHttps)
        {
            PortSettings settings = new PortSettings();
            settings.IPAddress = _settings.DefaultIP4;
            settings.PortNumber = overrideSettingsPortNumber;

            _networkManager.StartServerForSqlService(settings, overrideSettingsUseHttps);
        }

        public void StartAdminService()
        {
            PortSettings settings = new PortSettings();
            settings.IPAddress = _settings.DefaultIP4;
            settings.PortNumber = _settings.AdminServicePort;

            _networkManager.StartServerForAdminService(settings, true);
        }

        public void StartAdminService(int overrideSettingsPortNumber, bool overrideSettingsUseHttps)
        {
            PortSettings settings = new PortSettings();
            settings.IPAddress = _settings.DefaultIP4;
            settings.PortNumber = overrideSettingsPortNumber;

            _networkManager.StartServerForAdminService(settings, overrideSettingsUseHttps);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the appsettings file
        /// </summary>
        private void LoadConfiguration()
        {
            _settings = new Settings();
            _configurator = new Configurator();
            _settings = _configurator.Load();
        }

        /// <summary>
        /// Checks the backing database to see if it needs to be setup
        /// </summary>
        private void ConfigureBackingStore()
        {
            const string unknownDbType = "Unknown client databasetype";

            if (_overrideDefaultDb)
            {
                if (string.IsNullOrEmpty(_rootPath))
                {
                    _rootPath = Settings.RootFolder;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_rootPath))
                {
                    _rootPath = Settings.RootFolder;
                }
                _clientType = (DatabaseClientType)Settings.DatabaseClientType;
                _connectionString = _configurator.DefaultConnection();
            }

            if (!Directory.Exists(_rootPath))
            {
                Directory.CreateDirectory(_rootPath);
            }

            switch (_clientType)
            {
                case DatabaseClientType.Unknown:
                    throw new InvalidOperationException(unknownDbType);
                case DatabaseClientType.SQLServer:
                    _cooperativeStore = new SQLServerClient();
                    break;
                case DatabaseClientType.Postgres:
                    _cooperativeStore = new PostgresClient();
                    break;
                case DatabaseClientType.Sqlite:
                    _cooperativeStore = new SqliteClient(_connectionString, Settings.BackingDatabaseName, _rootPath);
                    break;
                default:
                    throw new InvalidOperationException(unknownDbType);

            }
        }

        #endregion
    }
}
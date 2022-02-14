namespace DotCDS
{
    public class Process
    {
        #region Private Fields
        private Settings _settings;
        private Configurator _configurator;
        private string _rootPath;
        #endregion

        #region Public Properties
        public Settings Settings => _settings;
        #endregion

        #region Constructors
        public Process()
        {
        }

        public Process(string rootPath)
        {

        }
        #endregion

        #region Public Methods
        public void Start()
        {
            LoadConfiguration();
        }

        public string TestConnectionString()
        {
            return _configurator.TestDefaultConnection();
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
        private void ConfigureBackingDatabase()
        {

        }
        #endregion

    }
}
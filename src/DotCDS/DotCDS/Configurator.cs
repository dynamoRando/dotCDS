using Microsoft.Extensions.Configuration;
using System;

namespace DotCDS
{
    internal class Configurator
    {
        #region Private Fields
        IConfiguration _config;
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        public Configurator()
        {
            _config = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json").Build();
        }
        #endregion

        #region Public Methods
        internal Settings Load()
        {
            var allSettings = _config.GetSection(nameof(Settings));
            return allSettings.Get<Settings>();
        }
        #endregion

        #region Private Methods
        #endregion

    }
}

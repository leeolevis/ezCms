using Microsoft.Extensions.Configuration;
using System.IO;

namespace ez.Core.Helpers
{
    public static class ConfigHelper
    {
        #region GetJsonConfig(获取Json配置文件)

        /// <summary>
        /// 获取Json配置文件
        /// </summary>
        /// <param name="configFileName">配置文件名。默认：appsettings.json</param>
        /// <param name="basePath">基路径</param>
        /// <returns></returns>
        public static IConfigurationRoot GetJsonConfig(string configFileName = "appsettings.json", string basePath = "")
        {
            basePath = string.IsNullOrWhiteSpace(basePath)
                ? Directory.GetCurrentDirectory()
                : Path.Combine(Directory.GetCurrentDirectory(), basePath);

            var configuration = new ConfigurationBuilder().SetBasePath(basePath)
                .AddJsonFile(configFileName, false, true)
                .Build();

            return configuration;
        }

        #endregion
    }
}

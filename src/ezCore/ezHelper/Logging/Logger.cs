using ezHelper.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;

namespace ezHelper.Logging
{
    public class Logger : ILogger
    {
        private IConfiguration _config { get; }
        private readonly IHostingEnvironment _environment;
        private Func<string> AccountId { get; }
        private static readonly object LogWriting = new object();

        public Logger(IConfiguration config, IHostingEnvironment environment)
        {
            IHttpContextAccessor accessor = new HttpContextAccessor();
            AccountId = () => accessor.HttpContext?.User.Id();
            _config = config;
            _environment = environment;
        }
        public Logger(IConfiguration config, string accountId)
        {
            AccountId = () => accountId;
            _config = config;
        }

        public void Log(string message, string fileName = "Log")
        {
            var logDirectory = Path.Combine(_environment.ContentRootPath, _config["Logger:Directory"]);
            var backupSize = long.Parse(_config["Logger:BackupSize"]);
            var logPath = Path.Combine(logDirectory, $"{fileName}.txt");

            var log = new StringBuilder();
            log.AppendLine("Time   : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            log.AppendLine("Account: " + AccountId());
            log.AppendLine("Message: " + message);
            log.AppendLine();

            lock (LogWriting)
            {
                Directory.CreateDirectory(logDirectory);
                File.AppendAllText(logPath, log.ToString());

                if (new FileInfo(logPath).Length >= backupSize)
                    File.Move(logPath, Path.Combine(logDirectory, $"{fileName} {DateTime.Now:yyyy-MM-dd HHmmss}.txt"));
            }
        }
        public void Log(Exception exception, string fileName)
        {
            while (exception.InnerException != null)
                exception = exception.InnerException;

            Log($"{exception.GetType()}: {exception.Message}{Environment.NewLine}{exception.StackTrace}", fileName);
        }
    }
}

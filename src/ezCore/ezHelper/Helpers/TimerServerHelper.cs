using System;
using System.Collections.Generic;
using System.Text;
using Itms.Core.Entity.HangfireHost;
using static Itms.Core.Entity.HangfireHost.Web.TimerServerApiModels;

namespace Itms.Core.Helpers
{
    public static class TimerServerHelper
    {
        public static ApiResponse<bool> SetTaskStatus(string apiAddress, SetTaskStatusModel model)
        {
            var uri = new Uri(apiAddress);
            return Web.WebApiClient<Core.Entity.HangfireHost.ApiResponse<bool>>($"{uri.Scheme}://{uri.Host}{(uri.Port == 80 ? "" : ":" + uri.Port.ToString())}/", uri.PathAndQuery.Remove(0, 1), model);
        }

        public static ApiResponse<object> GetTaskById(string apiAddress, GetTaskByIdModel model)
        {
            var uri = new Uri(apiAddress);
            return Web.WebApiClient<Core.Entity.HangfireHost.ApiResponse<object>>($"{uri.Scheme}://{uri.Host}{(uri.Port == 80 ? "" : ":" + uri.Port.ToString())}/", uri.PathAndQuery.Remove(0, 1), model);
        }

        public static ApiResponse<bool> DeleteTaskById(string apiAddress, DeleteTaskByIdModel model)
        {
            var uri = new Uri(apiAddress);
            return Web.WebApiClient<Core.Entity.HangfireHost.ApiResponse<bool>>($"{uri.Scheme}://{uri.Host}{(uri.Port == 80 ? "" : ":" + uri.Port.ToString())}/", uri.PathAndQuery.Remove(0, 1), model);
        }

        public static ApiResponse<bool> RecordTaskLog(string apiAddress, RecordTaskLogModel model)
        {
            var uri = new Uri(apiAddress);
            return Web.WebApiClient<Core.Entity.HangfireHost.ApiResponse<bool>>($"{uri.Scheme}://{uri.Host}{(uri.Port == 80 ? "" : ":" + uri.Port.ToString())}/", uri.PathAndQuery.Remove(0, 1), model);
        }

        public static ApiResponse<bool> ExecutionSql(string apiAddress, ExecutionSqlModel model)
        {
            var uri = new Uri(apiAddress);
            return Web.WebApiClient<Core.Entity.HangfireHost.ApiResponse<bool>>($"{uri.Scheme}://{uri.Host}{(uri.Port == 80 ? "" : ":" + uri.Port.ToString())}/", uri.PathAndQuery.Remove(0, 1), model);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace ez.Core.Helpers
{
    public static class ThreadHelper
    {
        #region WaitAll(执行多个操作)

        /// <summary>
        /// 执行多个操作，多个操作将同时执行
        /// </summary>
        /// <param name="actions">操作集合</param>
        public static void WaitAll(params Action[] actions)
        {
            if (actions == null)
            {
                return;
            }
            List<Task> tasks = new List<Task>();
            foreach (var action in actions)
            {
                tasks.Add(Task.Factory.StartNew(action, TaskCreationOptions.None));
            }
            Task.WaitAll(tasks.ToArray());
        }

        #endregion

        #region ThreadId(获取线程编号)

        /// <summary>
        /// 获取线程编号
        /// </summary>
        public static string ThreadId => System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();

        #endregion

        #region CurrentPrincipal(获取或设置 线程的当前负责人)

        /// <summary>
        /// 获取或设置 线程的当前负责人
        /// </summary>
        public static IPrincipal CurrentPrincipal
        {
            get { return System.Threading.Thread.CurrentPrincipal; }
            set { System.Threading.Thread.CurrentPrincipal = value; }
        }

        #endregion

        #region MaxThreadNumberInThreadPool(获取线程池中最大线程)

        /// <summary>
        /// 获取线程池中最大线程
        /// </summary>
        public static int MaxThreadNumberInThreadPool
        {
            get
            {
                int maxNumber;
                int ioNumber;
                ThreadPool.GetMaxThreads(out maxNumber, out ioNumber);
                return maxNumber;
            }
        }

        #endregion

        #region Sleep(将当前线程挂起指定的时间)

        /// <summary>
        /// 将当前线程挂起指定的时间
        /// </summary>
        /// <param name="time">挂起时间，单位：毫秒</param>
        public static void Sleep(int time)
        {
            System.Threading.Thread.Sleep(time);
        }

        #endregion

        #region StartTask(启动异步任务)

        /// <summary>
        /// 启动异步任务
        /// </summary>
        /// <param name="handler">任务，范例：() => { 代码 }</param>
        public static void StartTask(Action handler)
        {
            Task.Factory.StartNew(handler);
        }

        /// <summary>
        /// 启动异步任务
        /// </summary>
        /// <param name="handler">任务，范例：t => { 代码 }</param>
        /// <param name="state">传递的参数</param>
        public static void StartTask(Action<object> handler, object state)
        {
            Task.Factory.StartNew(handler, state);
        }

        #endregion
    }
}

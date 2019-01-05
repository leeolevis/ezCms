﻿using System;
using System.Diagnostics;
using System.Reflection;

namespace ez.Core
{
    public static partial class Extensions
    {
        #region GetFileVersion(获取程序集的文件版本)

        /// <summary>
        /// 获取程序集的文件版本
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        public static Version GetFileVersion(this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
            return new Version(info.FileVersion);
        }

        #endregion

        #region GetProductVersion(获取程序集的产品版本)

        /// <summary>
        /// 获取程序集的产品版本
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        public static Version GetProductVersion(this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
            return new Version(info.ProductVersion);
        }

        #endregion
    }
}

﻿using System;
using System.IO;

namespace ez.Core.Helpers {
    /// <summary>
    /// 常用公共操作
    /// </summary>
    public static class Common {
        /// <summary>
        /// 获取类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public static Type GetType<T>() {
            var type = typeof( T );
            return Nullable.GetUnderlyingType( type ) ?? type;
        }

        /// <summary>
        /// 换行符
        /// </summary>
        public static string Line => Environment.NewLine;

        /// <summary>
        /// 获取物理路径
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        public static string GetPhysicalPath( string relativePath ) {
            if( string.IsNullOrWhiteSpace( relativePath ) )
                return string.Empty;
            var rootPath = Web.WebRootPath;
            if( string.IsNullOrWhiteSpace( rootPath ) )
                return Path.GetFullPath( relativePath );
            return $"{Web.RootPath}\\{relativePath.Replace( "/", "\\" ).TrimStart( '\\' )}";
        }
    }
}

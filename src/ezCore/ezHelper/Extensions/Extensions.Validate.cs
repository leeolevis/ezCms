using System;

namespace ez.Core
{
    /// <summary>
    /// 系统扩展 - 验证
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 检测对象是否为null,为null则抛出<see cref="ArgumentNullException"/>异常
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="parameterName">参数名</param>
        public static void CheckNull(this object obj, string parameterName)
        {
            if (obj == null)
                throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value">值</param>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value">值</param>
        public static bool IsEmpty(this Guid value)
        {
            return value == Guid.Empty;
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value">值</param>
        public static bool IsEmpty(this Guid? value)
        {
            if (value == null)
                return true;
            return value == Guid.Empty;
        }

        #region IsNull(是否为空)
        /// <summary>
        /// 判断目标对象是否为空
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <returns></returns>
        public static bool IsNull(this object target)
        {
            return target.IsNull<object>();
        }

        /// <summary>
        /// 判断目标对象是否为空
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="target">目标对象</param>
        /// <returns></returns>
        public static bool IsNull<T>(this T target)
        {
            var result = ReferenceEquals(target, null);
            return result;
        }
        #endregion
    }
}

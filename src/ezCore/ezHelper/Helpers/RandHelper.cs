using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ez.Core.Helpers
{
    /// <summary>
    /// 随机数帮助类
    /// </summary>
    public static class RandHelper
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public static int RandIndex;

        #region 数字随机数

        /// <summary>
        /// 随机主键
        /// </summary>
        /// <param name="name">下划线前缀</param>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public static string RndString(string name = "", int num1 = 1000000, int num2 = 9000000)
        {
            if (RandIndex >= 1000000) RandIndex = 1;
            Random rnd = new Random(DateTime.Now.Millisecond + RandIndex);
            RandIndex++;
            return string.IsNullOrEmpty(name) 
                ? $"{rnd.Next(num1, num2)}" 
                : $"{name}_{rnd.Next(num1, num2)}";
        }

        /// <summary>
        /// 数字随机数
        /// </summary>
        /// <param name="num1">开始</param>
        /// <param name="num2">结束</param>
        /// <returns>从多少到多少之间的数据 包括开始不包括结束</returns>
        public static int RndInt(int num1, int num2)
        {
            if (RandIndex >= 1000000) RandIndex = 1;
            Random rnd = new Random(DateTime.Now.Millisecond + RandIndex);
            RandIndex++;
            return rnd.Next(num1, num2);
        }

        /// <summary>
        /// 获取一定数量的随机数
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static IList<int> RndInt(int num1, int num2, int len)
        {
            IList<int> list = new List<int>();
            for (int i = 0; i < len; i++) list.Add(RndInt(num1, num2));
            return list;
        }

        /// <summary>
        /// 获取从0到 int.MaxValue 一定数量的随机数
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static IList<int> RndInt(int len)
        {
            IList<int> list = RndInt(0, int.MaxValue, len);
            return list;
        }

        #endregion

        #region 获取一定长度的数字随机数

        /// <summary>
        /// 获取一定长度的数字随机数
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <returns>返回指定长度的数字随机串</returns>
        public static string RndNum(int length)
        {
            if (RandIndex >= 1000000) RandIndex = 1;
            char[] arrChar = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            StringBuilder num = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Millisecond + RandIndex);
            for (int i = 0; i < length; i++)
            {
                num.Append(arrChar[rnd.Next(0, 9)].ToString());
            }
            return num.ToString();
        }

        #endregion

        #region 日期+随机数
        /// <summary>
        /// 日期+随机数
        /// </summary>
        /// <returns>返回日期随机串</returns>
        public static string RndDateStr()
        {
            return RndDateStr(10000, 99999);
        }
        /// <summary>
        /// 日期+随机数
        /// </summary>
        /// <returns>返回日期随机串</returns>
        public static string RndDateStr(int min, int max)
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + RndInt(min, max);
        }

        public static IList<string> RndDateStr(int len)
        {
            IList<string> list = new List<string>();
            for (int i = 0; i < len; i++) list.Add(RndDateStr());
            return list;
        }

        #endregion

        #region 数字和字母随机数
        /// <summary>
        /// 数字和字母随机数
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <returns>返回指定长度的数字和字母的随机串</returns>
        public static string RndCode(int length)
        {
            if (RandIndex >= 1000000) RandIndex = 1;
            char[] arrChar =
            {
                'a','b','d','c','e','f','g','h','j','k','m','n','p','r','q','s','t','u','v','w','z','y','x',
                '2','3','4','5','6','7','8','9',
                'A','B','C','D','E','F','G','H','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z'};
            StringBuilder num = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                num.Append(arrChar[RndInt(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }

        /// <summary>
        /// 数字和字母随机数 Case True 大写 False 小写
        /// </summary>
        /// <param name="length"></param>
        /// <param name="upperCase">大小写  True 大写 False 小写</param>
        /// <returns></returns>
        public static string RndCode(int length, bool upperCase)
        {
            return upperCase ? RndCode(length).ToUpper() : RndCode(length).ToLower();
        }

        public static IList<string> RndCodeList(int len)
        {
            IList<string> list = new List<string>();
            for (int i = 0; i < len; i++) list.Add(RndCode(len));
            return list;
        }
        #endregion

        #region 字母随机数
        /// <summary>
        /// 字母随机数
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <returns>返回指定长度的字母随机数</returns>
        public static string RndLetter(int length)
        {
            if (RandIndex >= 1000000) RandIndex = 1;
            char[] arrChar =
            {
                'a','b','d','c','e','f','g','h','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x',
                'A','B','C','D','E','F','G','H','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z'};
            StringBuilder num = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                num.Append(arrChar[RndInt(0, arrChar.Length)].ToString());
            }
            return num.ToString();
        }

        /// <summary> 
        /// 字母随机数 Case True 大写 False 小写
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <param name="isCase"> </param>
        /// <returns>返回指定长度的字母随机数</returns>
        public static string RndLetter(int length, bool isCase)
        {
            return isCase ? RndLetter(length).ToUpper() : RndLetter(length).ToLower();
        }

        public static IList<string> RndLetterList(int len)
        {
            IList<string> list = new List<string>();
            for (int i = 0; i < len; i++) list.Add(RndLetter(len));
            return list;
        }
        #endregion

        #region Bool随机

        public static bool NextBool(this Random random)
        {
            return random.NextDouble() > 0.5;
        }

        #endregion

        #region 日期随机

        /// <summary>
        /// DateTime d = random.NextDateTime(new DateTime(2000, 1, 1), new DateTime(2010, 12, 31));
        /// </summary>
        /// <param name="random"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static DateTime NextDateTime(this Random random, DateTime minValue, DateTime maxValue)
        {
            var ticks = minValue.Ticks + (long)((maxValue.Ticks - minValue.Ticks) * random.NextDouble());
            return new DateTime(ticks);
        }
        public static DateTime NextDateTime(this Random random)
        {
            return NextDateTime(random, DateTime.MinValue, DateTime.MaxValue);
        }


        #endregion

        public static int GetTimestampId()
        {
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(2017, 7, 1));
            return int.Parse((DateTime.Now - startTime).TotalMilliseconds.ToString(CultureInfo.InvariantCulture).Substring(0, 9));
        }

    }
}

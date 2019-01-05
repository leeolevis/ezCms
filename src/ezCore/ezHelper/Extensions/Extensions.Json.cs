using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ez.Core
{
    /// <summary>
    /// 系统扩展 - 验证
    /// </summary>
    public static partial class Extensions
    {
        public static T Deserialize<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static string Serialize(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static T DeserializeBytes<T>(this byte[] value)
        {
            var serializedValue = Encoding.UTF8.GetString(value);
            return serializedValue.Deserialize<T>();
        }

        public static byte[] SerializeBytes(this object value)
        {
            var serializedValue = value.Serialize();
            return Encoding.UTF8.GetBytes(serializedValue);
        }

        /// <summary>
        /// 写入Json文件
        /// </summary>
        /// <param name="value"></param>
        /// <param name="jsonPath"></param>
        public static void SerializeWriterFile(this object value, string jsonPath,string fileName, IHostingEnvironment iHostingEnvironment)
        {
             var directoryPath = Path.Combine(iHostingEnvironment.WebRootPath, fileName);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            using (StreamWriter sw = new StreamWriter(jsonPath))
            {

                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;

                JsonWriter write = new JsonTextWriter(sw);

                serializer.Serialize(write, value);

                write.Close();
                sw.Close();
            }
        }

        /// <summary>
        /// 读取Json文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonPath"></param>
        /// <returns></returns>
        public static T SerializeReadFile<T>(this string jsonPath)
        {
            using (StreamReader sr = new StreamReader(jsonPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;

                //构建Json.net的读取流  
                JsonReader reader = new JsonTextReader(sr);
                //对读取出的Json.net的reader流进行反序列化，并装载到模型中  
                var result = serializer.Deserialize<T>(reader);
                return result;
            }
        }

        /// <summary>
        /// 自定义动态Json序列化
        /// </summary>
        /// <param name="value">序列化对象</param>
        /// <param name="perperties">序列化显示字段和排序</param>
        /// <returns></returns>
        public static string SerializeDynamic(this object value, List<KeyValuePair<string, int>> perperties)
        {
            var jsonString = JsonConvert.SerializeObject(value, Formatting.None, new JsonSerializerSettings()
            {
                ContractResolver = new JsonResolver(perperties)
            });
            return jsonString;
        }
    }

    /// <summary>
    /// 可支持字段排序的动态Json序列化
    /// </summary>
    public class JsonResolver : DefaultContractResolver
    {
        private readonly List<KeyValuePair<string, int>> _propertiesList;
        public JsonResolver(List<KeyValuePair<string, int>> propertiesEnumerable)
        {
            if (propertiesEnumerable != null)
            {
                _propertiesList = propertiesEnumerable.OrderBy(t => t.Value).ToList();
            }
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            foreach (var item in properties)
            {
                var find = _propertiesList.SingleOrDefault(t => t.Key == item.PropertyName);
                if (!string.IsNullOrEmpty(find.Key))
                    item.Order = find.Value;
            }
            //只序列化构造器中传入的包含在字符串中的属性
            if (_propertiesList != null && _propertiesList.Any())
            {
                properties = properties.Where(p => _propertiesList.Exists(pString => pString.Key == p.PropertyName)).OrderBy(t => t.Order).ToList();
            }
            return properties;
        }
    }
}

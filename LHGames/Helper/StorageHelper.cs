using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LHGames.Helper
{
    public static class StorageHelper
    {
        private static Dictionary<string, string> _document;
        private static string _path;

        public static void Write(string key, object data)
        {
            Init();

            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                _document[key] = JsonConvert.SerializeObject(data, settings);
                Store();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static T Read<T>(string key)
        {
            Init();

            try
            {
                if (_document.TryGetValue(key, out var data))
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    return JsonConvert.DeserializeObject<T>(data, settings);
                }

                return default(T);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default(T);
            }
        }

        private static void Init()
        {
            try
            {
                if (Environment.GetEnvironmentVariable("LOCAL_STORAGE") != null)
                {
                    _path = $"{Environment.GetEnvironmentVariable("LOCAL_STORAGE")}/document.json";
                }
                else
                {
                    _path = "/data/document.json";
                }

                if (System.IO.File.Exists(_path))
                {
                    var data = System.IO.File.ReadAllText(_path);
                    JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    _document = JsonConvert.DeserializeObject<Dictionary<string, string>>(data, settings);
                }
                else
                {
                    _document = new Dictionary<string, string>();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void Store()
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

                System.IO.File.WriteAllText(_path, JsonConvert.SerializeObject(_document, settings));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
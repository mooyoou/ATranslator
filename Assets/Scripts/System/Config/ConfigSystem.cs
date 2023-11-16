using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;


namespace System.Config
{
    public class ProjectConfig
    {
        public ProjectConfig(List<string> loadRules = null)
        {
            if(loadRules!=null)
            {
                _loadRules = loadRules;
            }
        }
        
        [JsonConstructor]
        public ProjectConfig(bool skipHideFolder, List<string> loadRules = null)
        {
            _skipHideFolder = skipHideFolder;
            if(loadRules!=null)
            {
                _loadRules = loadRules;
            }
        }

        private bool _skipHideFolder = true;
        public bool SkipHideFolder
        {
            get
            {
                return _skipHideFolder;
            }
        }
        
        private List<string> _loadRules = new List<string>()
        {
            "\\.txt$",
            "\\.word$",
            "\\.md$",
            "\\.html$",
            "\\.xml$",
            "\\.json$",
            "\\.csv$",
            "\\.log$",
            "\\.cfg$",
            "\\.ini$"
        };
        public List<string> LoadRules
        {
            get
            {
                return _loadRules;
            }
        }

        [JsonIgnore]
        public string RuleRegex
        {
            get
            {
                string regexRule = "";
                for (int i = 0; i <  _loadRules.Count; i++)
                {
                    regexRule += _loadRules[i];
                    if (i != _loadRules.Count - 1)
                    {
                        regexRule += "|";
                    }
                }

                return regexRule;
            }
        }
        
        
    }
    
    public static class ConfigSystem
    {
        private static ProjectConfig _projectConfig = new ProjectConfig();
        public static ProjectConfig ProjectConfig
        {
            get
            {
                return _projectConfig;
            }
            set
            {
                _projectConfig = value;
                InitConfigFile(_projectConfig);
                InvokeConfigUpdateEvent();
            }
        }
        public static string CurConfigFolderPath
        {
            get
            {
                return _curConfigFolderPath;
            }
        }
        private static string _curConfigFolderPath;


        /// <summary>
        ///  应用全局配置
        /// </summary>
        private static Dictionary<string, string> _globalPrefs = new Dictionary<string, string>()
        {
            { "OpenProjectHistory", "" },//项目打开历史

        };

        
        
        private static List<string> _openProjectHistory = new List<string>();
        public static List<string> OpenProjectHistories
        {
            get
            {
                return _openProjectHistory;
            }
        }

        public static event EventHandler ConfigUpdate;

        private static void InvokeConfigUpdateEvent()
        {
            ConfigUpdate?.Invoke(typeof(This),null);
        }
        
        /// <summary>
        /// 更新程序偏好设置
        /// </summary>
        public static void RefreshGlobbalPlayerPrefs()
        {
            string[] keys = _globalPrefs.Keys.ToArray();
            foreach (var kePref in keys)
            {
                if (PlayerPrefs.HasKey(kePref))
                {
                    _globalPrefs[kePref] = PlayerPrefs.GetString(kePref);
                }
            }

            //加载历史打开路径
            if (_globalPrefs.TryGetValue("OpenProjectHistory", out string openProjectHistories))
            {
                _openProjectHistory = openProjectHistories.Split(",").ToList();
            }

        }

        /// <summary>
        /// 添加历史打开路径
        /// </summary>
        /// <param name="path"></param>
        public static void AddOpenProjectHistory(string path)
        {
            _openProjectHistory.RemoveAll(string.IsNullOrEmpty);

            _openProjectHistory.Insert(0,path);
            _openProjectHistory = _openProjectHistory.Distinct().ToList();
            if(_openProjectHistory.Count>10)
            {
                _openProjectHistory.RemoveRange(10,_openProjectHistory.Count-10);
            }

            string value = "";
            for (int i = 0; i < _openProjectHistory.Count; i++)
            {
                value += _openProjectHistory[i];
                if (i != _openProjectHistory.Count - 1)
                {
                    value += ",";
                }
            }
            PlayerPrefs.SetString("OpenProjectHistory", value);
            PlayerPrefs.Save(); // 保存操作是异步的，使用Save方法确保立即保存
            
        }


        /// <summary>
        /// 加载或者初始化工程配置文件夹.AT
        /// </summary>
        /// <param name="rootNode"></param>
        public static void InitProject(string rootNodePath)
        {
            string configFolderPath = Path.Combine(rootNodePath, ".AT");

            if (Directory.Exists(configFolderPath) && LoadProjectConfig(configFolderPath))
            {
                //确保配置文件夹为隐藏文件夹
                FileAttributes attributes = File.GetAttributes(configFolderPath);
                attributes |= FileAttributes.Hidden;
                File.SetAttributes(configFolderPath, attributes);
            }
            else
            {
                ///重新生成配置文件夹
                if (Directory.Exists(configFolderPath))Directory.Delete(configFolderPath, true);
                InitProjectConfig(configFolderPath);
            }


        }
        
        
        /// <summary>
        /// 加载工程设置
        /// </summary>
        /// <returns>是否加载成功</returns>
        private static bool LoadProjectConfig(string configFolderPath)
        {
            if(!LoadConfigFile(configFolderPath))return false ;
            return true;
        }
        
        private static bool LoadConfigFile(string configFolderPath)
        {
            try
            {
                string configfilePath = Path.Combine(configFolderPath, "config");

                string configfile = File.ReadAllText(configfilePath);
                _curConfigFolderPath = configFolderPath;
                ProjectConfig = JsonConvert.DeserializeObject<ProjectConfig>(configfile);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
            return true;
        }
        
        private static void InitProjectConfig(string configFolderPath)
        {
            Directory.CreateDirectory(configFolderPath);
            DirectoryInfo folderInfo = new DirectoryInfo(configFolderPath);
            folderInfo.Attributes |= FileAttributes.Hidden;
            _curConfigFolderPath = configFolderPath;
            ProjectConfig = new ProjectConfig();
        }

        /// <summary>
        /// 建立配置文件
        /// </summary>
        /// <param name="configFolderPath"></param>
        private static void InitConfigFile(ProjectConfig projectConfig)
        {
            if(string.IsNullOrEmpty(_curConfigFolderPath))return;
            
            string configfilePath = Path.Combine(_curConfigFolderPath, "config");

            // JsonConvert.
            string json = JsonConvert.SerializeObject(projectConfig);

            File.WriteAllText(configfilePath, json);
        }
        
        
    }
}

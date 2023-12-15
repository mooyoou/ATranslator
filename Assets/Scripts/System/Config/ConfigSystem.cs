using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


namespace System.Config
{
    public class ProjectConfig
    {
        public ProjectConfig()
        {
        }

        public ProjectConfig(ProjectConfig projectConfig)
        {
            _skipHideFolder = projectConfig._skipHideFolder;
            _displaySpecificFileTypes = projectConfig._displaySpecificFileTypes;
            _globalTextMatchRules = new List<string>(projectConfig._globalTextMatchRules);
            _specialTextMatchRules = new Dictionary<string, List<string>>();
            foreach (var kv in projectConfig._specialTextMatchRules)
            {
                _specialTextMatchRules[kv.Key]=new List<string>(kv.Value);
            }
        }
        
        [JsonConstructor]
        public ProjectConfig(bool skipHideFolder,bool onlyShowFileSpecify, Dictionary<string,List<string>> specialTextMatchRules = null)
        {
            _skipHideFolder = skipHideFolder;
            _displaySpecificFileTypes = onlyShowFileSpecify;
            if(specialTextMatchRules!=null)
            {
                _specialTextMatchRules = specialTextMatchRules;
            }
        }

        private bool _skipHideFolder = true;
        public bool SkipHideFolder
        {
            get
            {
                return _skipHideFolder;
            }
            set
            {
                _skipHideFolder = value;
            }
        }
        
        private bool _displaySpecificFileTypes = true;
        public bool DisplaySpecificFileTypes
        {
            get
            {
                return _displaySpecificFileTypes;
            }
            set
            {
                _displaySpecificFileTypes = value;
            }
        }
        
        
        /// <summary>
        /// 全局适用的可翻译文本匹配规则
        /// </summary>
        private List<string> _globalTextMatchRules = new List<string>()
        {
            ";(.+)",
            "//(.+)"
        };
        public List<string> GlobalTextMatchRules
        {
            get { return _globalTextMatchRules; }
            set
            {
                _globalTextMatchRules = value.Distinct().ToList();
            }
        }

        /// <summary>
        /// 特殊文件的专用可翻译文本匹配规则
        /// </summary>
        private Dictionary<string,List<string>> _specialTextMatchRules = new Dictionary<string,List<string>>()
        {
            // { "\\.ERB$",new List<string>()
            //     {
            //         "^[\\s]*PRINT[^\\s]*[\\s]+(.+)$",
            //         "^[\\s]*DATA[^\\s]*[\\s]+(.+)$",
            //         "^[\\s]*LOCAL[S]?\\s?\\+?\\=\\s?(.+)$",
            //         "^[\\s]*CALL[\\s]PRINT[^@]*@(.+)$",
            //         "^[\\s]*CALL[\\s]KPRINT[\\w\\s,\"]+\\\"(.+)\\$",
            //         "^[\\s]*CALL[\\s]COLOR_PRINT[\\w\\s@(,]?\\((.+)\\)$",
            //         "^[\\s]*CALL[\\s]ASK[\\w\\s@,]*\\((.+)\\)$",
            //         "^[\\s]*CALL[\\s]+PRINT_ADD_EXP\\(.+,[\\s]*\"(.+)\\\"[\\s]*.+\\)$"
            //     } 
            // },            
            { "\\.xml$",new List<string>()
                {
                   "<[^/].*?>(.*?)</.*?>"
                } 
            }
        };
        public Dictionary<string, List<string>> SpecialTextMatchRules
        {
            get
            {
                return _specialTextMatchRules;
            }
            set
            {
                foreach (var kv in value)
                {
                    value[kv.Key] = kv.Value.Distinct().ToList();
                }
            }
        }

        /// <summary>
        /// 获取文本匹配规则
        /// </summary>
        /// <param name="fileName">要匹配的文件名（可支持单文件名或者全路径模式切换）</param>
        /// <returns></returns>
        public string GetTextMatchRegex(string fileName= "")
        {
            string regexRule = string.Join("|", _globalTextMatchRules);
            foreach (var rule in _specialTextMatchRules)
            {
                Regex regex = new Regex(rule.Key);
                Match match = regex.Match(fileName);
                if (match.Success && rule.Value.Count>0)
                {
                    regexRule += $"|{string.Join("|", rule.Value)}";
                }
            }
            return regexRule;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType())) { return false; }

            bool result = true;
            ProjectConfig p = (ProjectConfig) obj;
            result &= p._skipHideFolder == _skipHideFolder;
            result &= p._displaySpecificFileTypes == _displaySpecificFileTypes;
            result &= p.SpecialTextMatchRules.SequenceEqual(SpecialTextMatchRules);
            
            return result;
        }

        [JsonIgnore]
        public string FIleMatchRegex
        {
            get
            {
                if (_specialTextMatchRules.Count == 0)
                {
                    return "";
                }
                string regexRule = string.Join("|", _specialTextMatchRules.Keys);
                return regexRule;
            }
        }
        
        
    }
    
    public static class ConfigSystem
    {
        private static ProjectConfig _projectConfig = new ProjectConfig();

        /// <summary>
        /// 默认的匹配选项
        /// </summary>
        public static RegexOptions RegexOptions = RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.ExplicitCapture;
        public static ProjectConfig ProjectConfig
        {
            get
            {
                return _projectConfig;
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

        private static string _projectPath = "";
        /// <summary>
        /// 当前工程路径
        /// </summary>
        public static string ProjectPath
        {
            get
            {
                return _projectPath;
            }
        }
        
        public static event EventHandler<string> ConfigUpdate;

        private static void InvokeConfigUpdateEvent()
        {
            ConfigUpdate?.Invoke(null,_projectPath);
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

            string value = string.Join(",", _openProjectHistory);
            PlayerPrefs.SetString("OpenProjectHistory", value);
            PlayerPrefs.Save(); // 保存操作是异步的，使用Save方法确保立即保存
            
        }


        /// <summary>
        /// 加载或者初始化工程配置文件夹.AT
        /// </summary>
        /// <param name="rootNodePath">工程目录</param>
        public static void InitProject(string rootNodePath)
        {
            _projectPath = rootNodePath;
            string configFolderPath = Path.Combine(rootNodePath, ".AT");

            if (Directory.Exists(configFolderPath) && LoadConfigFile(configFolderPath))
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
        /// 更新工程设置
        /// </summary>
        /// <param name="projectConfig"></param>
        public static void UpdateProjectConfig(ProjectConfig projectConfig)
        {
            _projectConfig = projectConfig;
            InitConfigFile(_projectConfig);
            InvokeConfigUpdateEvent();
        }
        
        /// <summary>
        /// 加载工程设置
        /// </summary>
        /// <returns>是否加载成功</returns>
        private static bool LoadConfigFile(string configFolderPath)
        {
            try
            {
                string configfilePath = Path.Combine(configFolderPath, "config");

                string configfile = File.ReadAllText(configfilePath);
                _curConfigFolderPath = configFolderPath;
                UpdateProjectConfig(JsonConvert.DeserializeObject<ProjectConfig>(configfile));
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
            UpdateProjectConfig(new ProjectConfig());
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

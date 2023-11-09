using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace System.ProjectConfig
{
    public class ProjectConfig
    {
        public ProjectConfig(string configFolderPath)
        {
            ConfigFolderPath = configFolderPath;
        }

        public string ConfigFolderPath = "";
        public bool SkipHideFolder = true;
    }
    
    public static class ConfigSystem
    {
        private static ProjectConfig _projectConfig;
        public static ProjectConfig ProjectConfig
        {
            get
            {
                return _projectConfig;
            }
        }

        private static Dictionary<string, string> _globalPrefs = new Dictionary<string, string>()
        {
            { "OpenProjectHistory", "" },

        };

        private static List<string> _openProjectHistory = new List<string>();

        public static List<string> OpenProjectHistories
        {
            get
            {
                return _openProjectHistory;
            }
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
            _projectConfig.ConfigFolderPath = configFolderPath;
            return true;
        }
        
        private static bool LoadConfigFile(string configFolderPath)
        {
            try
            {
                string configfilePath = Path.Combine(configFolderPath, "config");
                _projectConfig = JsonUtility.FromJson<ProjectConfig>(File.ReadAllText(configfilePath));
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        
        private static void InitProjectConfig(string configFolderPath)
        {
            Directory.CreateDirectory(configFolderPath);
            DirectoryInfo folderInfo = new DirectoryInfo(configFolderPath);
            folderInfo.Attributes |= FileAttributes.Hidden;
            
            InitConfigFile(configFolderPath);
            _projectConfig.ConfigFolderPath = configFolderPath;
        }


        private static void InitConfigFile(string configFolderPath)
        {
            string configfilePath = Path.Combine(configFolderPath, "config");
            
            _projectConfig = new ProjectConfig(configFolderPath);
            string json = JsonUtility.ToJson(_projectConfig,true);
            
            File.WriteAllText(configfilePath, json);
        }
        
        
    }
}

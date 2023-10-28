using System.Windows.Forms;
// using Microsoft.WindowsAPICodePack.Dialogs;
namespace Utility
{
    public static class Misc
    {
        /// <summary>
        /// 简易版文件选择
        /// </summary>
        /// <param name="defaultPath"></param>
        /// <returns></returns>
        public static string OpenFileDialog(string defaultPth = null)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;//该值确定是否可以选择多个文件
            dialog.Title = "请选择文件夹";
            dialog.Filter = "所有文件(*.*)|*.*";
            if (!string.IsNullOrEmpty(defaultPth)) dialog.InitialDirectory = defaultPth;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.FileName;
            }
            return null;
        }

        public static string OpenFolderBrowserDialog(string defaultPth = null)
        {
            FolderBrowserDialog openFolderBrowserDialog = new FolderBrowserDialog();
            if (!string.IsNullOrEmpty(defaultPth)) openFolderBrowserDialog.SelectedPath = defaultPth;
            if (openFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                return openFolderBrowserDialog.SelectedPath;
            }
            return null;
        }


        
        // public static string OpenCommonOpenFileDialog(bool isfolder=false)
        // {
        //     CommonOpenFileDialog dialog = new CommonOpenFileDialog();
        //     dialog.InitialDirectory = "C:\\";
        //     dialog.IsFolderPicker = isfolder;
        //     if (dialog.ShowDialog() == CommonFileDialogResult.OK)
        //     {
        //         return dialog.FileName;
        //     }
        //     return null;
        // }
    }
}
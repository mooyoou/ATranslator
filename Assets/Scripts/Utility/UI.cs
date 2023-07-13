
using UnityEngine;
namespace Utility
{
    /// <summary>
    /// UI相关工具函数
    /// </summary>
    public static class UI 
    {

        /// <summary>
        /// 坐标位于指定矩形范围检测
        /// </summary>
        /// <param name="parentTransform">限制范围的矩形物体</param>
        /// <param name="screenPoint">输入坐标</param>
        /// <param name="widthOffest">横向偏移</param>
        /// <param name="heightOffest">纵向偏移</param>
        /// <param name="fixPoint"></param>
        /// <returns>如果位于窗口外则true</returns>
        public static bool InRectRange(Transform parentTransform,Vector2 screenPoint,out Vector2 fixPoint,float widthOffest = 0,float heightOffest = 0)
        {
            RectTransform canvasRectTransform = parentTransform as RectTransform;
            Vector3[] canvasCorners = new Vector3[4];
            canvasRectTransform!.GetWorldCorners(canvasCorners);
            var minPoint = RectTransformUtility.WorldToScreenPoint(Camera.main,canvasCorners[0]);
            var maxPoint = RectTransformUtility.WorldToScreenPoint(Camera.main,canvasCorners[2]);
            if (widthOffest > maxPoint.x - minPoint.x)
            {
                widthOffest = 0;
                Debug.LogWarning("横向偏移设置过大，widthOffest失效");
            }
            if (heightOffest > maxPoint.y - minPoint.y)
            {
                heightOffest = 0;
                Debug.LogWarning("纵向偏移设置过大，heightOffest失效");
            }
        
            fixPoint = new Vector2(Mathf.Clamp(screenPoint.x, minPoint.x + widthOffest, maxPoint.x - widthOffest),Mathf.Clamp(screenPoint.y, minPoint.y + heightOffest, maxPoint.y - heightOffest));
            return Mathf.Approximately(screenPoint.x, fixPoint.x) && Mathf.Approximately(screenPoint.y, fixPoint.y);
        }
        
        
        
        
    }

}


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
        /// <param name="screenPoint">输入画布坐标</param>
        /// <param name="fixPoint">约束后坐标</param>
        /// <param name="widthOffset">横向内部偏移</param>
        /// <param name="heightOffset">纵向内部偏移</param>
        /// <returns>如果位于约束范围外则true</returns>
        public static bool InRectRange(RectTransform parentTransform,Vector2 screenPoint,out Vector2 fixPoint,float widthOffset = 0,float heightOffset = 0)
        {
            // 获取左下角坐标
            var rect = parentTransform.rect;
            Vector2 minPoint = new Vector2(rect.xMin,rect.yMin);
            // 获取右上角坐标
            Vector2 maxPoint = new Vector2(rect.xMax,rect.yMax);
            
            
            if (widthOffset > maxPoint.x - minPoint.x)
            {
                widthOffset = 0;
                Debug.LogWarning("横向偏移设置过大，widthOffest失效");
            }
            if (heightOffset > maxPoint.y - minPoint.y)
            {
                heightOffset = 0;
                Debug.LogWarning("纵向偏移设置过大，heightOffest失效");
            }
        
            fixPoint = new Vector2(Mathf.Clamp(screenPoint.x, minPoint.x + widthOffset, maxPoint.x - widthOffset),Mathf.Clamp(screenPoint.y, minPoint.y + heightOffset, maxPoint.y - heightOffset));
            return Mathf.Approximately(screenPoint.x, fixPoint.x) && Mathf.Approximately(screenPoint.y, fixPoint.y);
        }
        
        
        
        
    }

}

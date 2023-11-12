
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
        /// <param name="parentTransform">限制范围的矩形物体(目前只能传入MainCanvas)</param>
        /// <param name="screenPoint">输入屏幕坐标（PointEventData）</param>
        /// <param name="fixPoint">约束后的相对坐标</param>
        /// <param name="widthOffset">横向内部偏移</param>
        /// <param name="heightOffset">纵向内部偏移</param>
        /// <returns>如果位于约束范围外则true</returns>
        public static bool InRectRange(RectTransform parentTransform,Vector2 screenPoint,out Vector2 fixPoint,float widthOffset = 0,float heightOffset = 0)
        {
            if (widthOffset > parentTransform.rect.width/2)
            {
                widthOffset = 0;
                Debug.LogWarning("横向偏移设置过大，widthOffest失效");
            }
            if (heightOffset > parentTransform.rect.height/2)
            {
                heightOffset = 0;
                Debug.LogWarning("纵向偏移设置过大，heightOffest失效");
            }

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentTransform, screenPoint, null, out localPoint);

            bool InRange = true;
            
            if (Mathf.Abs(localPoint.x) > parentTransform.rect.width / 2 - widthOffset)
            {
                localPoint.x = localPoint.x / Mathf.Abs(localPoint.x) * (parentTransform.rect.width / 2 - widthOffset);
                InRange = false;
            }
            
            if (Mathf.Abs(localPoint.y) > parentTransform.rect.height / 2 - heightOffset)
            {
                localPoint.y = localPoint.y / Mathf.Abs(localPoint.y) * (parentTransform.rect.height / 2 - heightOffset);
                InRange = false;
            }
            
            fixPoint =  parentTransform.TransformPoint(localPoint);

            return InRange;
        }
        
        /// <summary>
        /// 切换轴心
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="newPivot"></param>
        public static void ChangePivotWithoutMovingPosition(RectTransform rectTransform, Vector2 newPivot)
        {
            Vector2 currentPivot = rectTransform.pivot;
        
            Vector2 pivotOffset = newPivot - currentPivot;

            Vector3 positionOffset = new Vector3(rectTransform.rect.width * pivotOffset.x, rectTransform.rect.height * pivotOffset.y, 0);

            rectTransform.pivot = newPivot;
            rectTransform.localPosition += positionOffset;
        }
        
        
    }

}

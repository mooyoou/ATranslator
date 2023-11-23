using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CannotDragScrollRect : ScrollRect
{
   public override void OnDrag(PointerEventData eventData)
   {
      return;
   }

   public override void OnBeginDrag(PointerEventData eventData)
   {
      return;
   }
   
   public override void OnEndDrag(PointerEventData eventData)
   {
      return;
   }

   public override void OnScroll(PointerEventData data)
   {
      if (horizontal && !vertical)
      {
         var dataScrollDelta = data.scrollDelta;
         dataScrollDelta.y *= -1;
         data.scrollDelta = dataScrollDelta;
      }
      base.OnScroll(data);
   }
   
   public void ScrollToTarget(RectTransform targetItem)
   {
      Vector2 contentPosition = (Vector2)transform.InverseTransformPoint(targetItem.position);
      if (horizontal)
      {
         var leftX = contentPosition.x - targetItem.pivot.x * targetItem.rect.width;
         var contentX = -leftX;
         content.anchoredPosition = new Vector2(contentX, content.anchoredPosition.y);
      }
      if (vertical)
      {
         var topY = contentPosition.y + (1-targetItem.pivot.y) * targetItem.rect.height;
         var contentY = -(viewport.rect.height - topY);
         content.anchoredPosition = new Vector2(content.anchoredPosition.x, contentY);
      }
   }
   
   
}

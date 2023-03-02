//AdvancedFileBrowser by Alcatraz. 
//Flight Dream Studio (C) 2015. 
//alcatrazalex@gmail.com
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


public class ResizeablePanel : MonoBehaviour, IPointerDownHandler, IDragHandler
{

    [HideInInspector]public Vector2 pointerOffset;
    private RectTransform canvasRectTransform;
    public RectTransform panelRectTransform;
    public Vector2 minSize;
    public AdvancedFileBrowser controller;
    public GridLayoutGroup group;

    void Awake()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvasRectTransform = canvas.transform as RectTransform;
        }
        group.constraintCount = (int)(controller.fileArea.rect.size.x / (group.cellSize.x + group.spacing.x));
        controller.CalcSize(group.constraintCount);
    }

    

    public void OnPointerDown(PointerEventData data)
    {
        panelRectTransform.SetAsLastSibling();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panelRectTransform, data.position, data.pressEventCamera, out pointerOffset);
    }


    public void OnDrag(PointerEventData data)
    {
        Vector2 pointerPostion = panelRectTransform.sizeDelta;
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, pointerPostion, data.pressEventCamera, out localPointerPosition
            ))
        {
            Vector3[] canvasCorners = new Vector3[4];
            canvasRectTransform.GetWorldCorners(canvasCorners);
             panelRectTransform.sizeDelta = new Vector2(Mathf.Clamp(panelRectTransform.sizeDelta.x + data.delta.x, minSize.x, canvasCorners[2].x), 
                 Mathf.Clamp(panelRectTransform.sizeDelta.y - data.delta.y, minSize.y, canvasCorners[2].x));
        }
       group.constraintCount = (int) (controller.fileArea.rect.size.x / (group.cellSize.x + group.spacing.x));
       controller.CalcSize(group.constraintCount);
    }

}

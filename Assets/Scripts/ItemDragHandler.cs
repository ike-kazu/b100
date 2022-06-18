using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    private Vector3 itemPos;
    RectTransform rectTransform;

    // void Start()
    // {
    //     itemPos = GetComponent<RectTransform>().localPosition;
    // }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemPos = GetComponent<RectTransform>().anchoredPosition3D;
        Debug.Log("OnBeginDrag");
        Debug.Log(itemPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(itemPos);
        GetComponent<RectTransform>().anchoredPosition3D = itemPos;
    }
}

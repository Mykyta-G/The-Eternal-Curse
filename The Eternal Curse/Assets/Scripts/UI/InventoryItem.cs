using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDropHandler, IEndDragHandler
{
    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private TMP_Text quantityTxt;

    [SerializeField]
    private Image borderImage;

    public event Action<InventoryItem> OnItemClicked;
    public event Action<InventoryItem> OnItemDroppedOn;
    public event Action<InventoryItem> OnItemBeginDrag;
    public event Action<InventoryItem> OnItemEndDrag;
    public event Action<InventoryItem> OnRightMouseBtnClick;

    private bool empty = true;

    private void Awake()
    {
        ResetData();
        Deselect();
    }

    public void ResetData()
    {
        itemImage.gameObject.SetActive(false);
        empty = true;
    }

    public void Deselect()
    {
        borderImage.enabled = false;
    }

    public void SetData(Sprite sprite, int quantity)
    {
        itemImage.gameObject.SetActive(true);
        itemImage.sprite = sprite;
        quantityTxt.text = quantity.ToString();
        empty = false;
    }

    public void Select()
    {
        borderImage.enabled = true;
    }

    // === Unity's Event Interface methods (called automatically if no EventTrigger is used) ===

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (empty)
            return;
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseBtnClick?.Invoke(this);
        }
        else
        {
            OnItemClicked?.Invoke(this);
        }
    }

    public void OnPointerClickHandler(BaseEventData data)
    {
        if (empty){
            return;
        }
        PointerEventData pointerData = (PointerEventData)data;
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseBtnClick?.Invoke(this);
        }
        else
        {
            OnItemClicked?.Invoke(this);
        }
    }

    public void OnDropHandler(BaseEventData data)
    {
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnBeginDragHandler(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;
        if (empty)
            return;
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnEndDragHandler(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;
        OnItemEndDrag?.Invoke(this);
    }
}

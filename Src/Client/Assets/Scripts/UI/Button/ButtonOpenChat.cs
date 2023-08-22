using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonOpenChat : Button {
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        this.image.color = new Color(0, 0, 0, 1);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        this.image.color = new Color(0, 0, 0, 0);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        this.image.color = new Color(0, 0, 0, 0);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        this.image.color = new Color(0, 0, 0, 0);
    }
}

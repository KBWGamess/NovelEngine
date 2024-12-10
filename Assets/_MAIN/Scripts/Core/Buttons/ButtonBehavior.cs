using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static ButtonBehavior selectedButton = null;
    public Animator anim;

    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.Play("Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (selectedButton != null && selectedButton != this)
        {
            selectedButton.OnPointerExit(null);
        }

        anim.Play("Exit");
        selectedButton = this;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionAnimator : MonoBehaviour
{
    public AnimatorController cubo;

    public void OnPointerEnter()
    {
        //cubo.ActiveTrigger();
        cubo.ToogleBool();
    }

    public void OnPointerExit()
    {
        cubo.ToogleBool();
    }
}

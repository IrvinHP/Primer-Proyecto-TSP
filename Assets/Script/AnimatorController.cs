using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public Animator animator;

    public string tiggerName;
    public string boolName;

    public void ActiveTrigger()
    {
        animator.SetTrigger(tiggerName);
    }

    public void ToogleBool()
    {
        bool currentState = animator.GetBool(boolName);
        animator.SetBool(boolName, !currentState);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

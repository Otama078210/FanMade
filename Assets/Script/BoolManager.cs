using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolManager : MonoBehaviour
{
    public Animator animator;

    public void SetBool()
    {
        animator.SetBool("Push", true);
    }
}

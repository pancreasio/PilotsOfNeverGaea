using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSignSelected : MonoBehaviour
{
    Animator animator = null;
    private void Awake()
    {
        
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetSelected()
    {
        animator.SetTrigger("Selected");
    }
    private void Aux()
    {
        GetComponent<IndependentButton>().interactable = true;
    }
}

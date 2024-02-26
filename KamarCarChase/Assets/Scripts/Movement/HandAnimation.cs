using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimation : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        float gripValue = gripAnimationAction.action.ReadValue<float>();

        animator.SetFloat("Trigger", triggerValue);
        animator.SetFloat("Grip", gripValue);
    }
}

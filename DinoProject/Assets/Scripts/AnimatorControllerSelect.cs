using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class AnimatorControllerSelect : MonoBehaviour
{
    public int selected = 0;
    public AnimatorController[] animatorControllers = new AnimatorController[4];
    public Animator animator;

    void Update()
    {
        bool change = false;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            selected -= 1;
            change = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            selected += 1;
            change = true;
        }

        if (change)
        {
            if (selected < 0)
                selected = animatorControllers.Length - 1;

            if (selected == animatorControllers.Length)
                selected = 0;

            animator.runtimeAnimatorController = animatorControllers[selected];
        }
    }
}

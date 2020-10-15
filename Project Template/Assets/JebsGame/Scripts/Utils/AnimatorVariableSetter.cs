using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JorgeJGnz
{
    public class AnimatorVariableSetter : MonoBehaviour
    {
        [Header("Receiver")]
        public Animator animator;

        [Header("Variables")]
        public string nameOfIntVar;
        public string nameOfFloatVar;
        public string nameOfBoolVar;

        public LetterCheck letterCheck;

        public void SetAnimatorVar(int value)
        {
            if (!letterCheck.currentLetter.Equals(letterCheck.letterInChest))
            {
                animator.SetInteger(nameOfIntVar, 0);
            }
            else
            {
                animator.SetInteger(nameOfIntVar, value);
            }
        }

        public void SetInt(int value)
        {
            animator.SetInteger(nameOfIntVar, value);
        }
        public void SetFloat(float value)
        {
            animator.SetFloat(nameOfFloatVar, value);
        }
        public void SetBool(bool value)
        {
            animator.SetBool(nameOfBoolVar, value);
        }
    }
}

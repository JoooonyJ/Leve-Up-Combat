using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] InputActionReference attack;
    Animator animator;
    // Start is called before the first frame update
    void OnEnable()
    {
        attack.action.Enable();
    }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        if(attack.action.WasPerformedThisFrame())
        {
            animator.SetTrigger("Attack");
        }
    }
    // Update is called once per frame
    void OnDisable()
    {
        attack.action.Disable();
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Behaviour")]
    [SerializeField] float hitCollidersActiveDuration = 0.25f;

    [Header("Location of HitColliders")]
    [SerializeField] Transform hitCollidersParent;

    [Header("Inputs")]
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

    public void OnAnimationAttack(string hitColliderObjectName)
    {
        Transform hitCollider = hitCollidersParent.Find(hitColliderObjectName);

        if (hitCollider)
        {
            hitCollider.gameObject.SetActive(true);
            DOVirtual.DelayedCall(hitCollidersActiveDuration,
                () => hitCollider.gameObject.SetActive(false),
                false
                );
        }

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

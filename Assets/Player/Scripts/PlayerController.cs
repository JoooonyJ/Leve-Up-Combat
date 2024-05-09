using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum MovementMode
    {
        Global,
        FromCamera,
    }

    [Header("Movement")]
    [SerializeField] MovementMode movementMode = MovementMode.FromCamera;
    [Space(10)]
    [SerializeField] float speed = 5f;


    public enum OrientationMode
    {
        ToMoveDirection,
        ToCameraDirection,
    }
    [Header("Orientation")]
    [SerializeField] OrientationMode orientationMode = OrientationMode.ToCameraDirection;
    [SerializeField] private float angularSpeed = 360f;

    [Header("Jump")]
    [SerializeField] float jumpSpeed = 7f;

    [Header("Gravity")]
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float minimunFallVelocity = -3f;


    [Header("Animation")]
    [SerializeField] float smoothAnimationSpeed = 4f;

    [Header("Input")]
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;

    CharacterController characterController;
    Camera mainCamera;
    Animator animator;

    float verticalVelocity = 0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        move.action.Enable();
        jump.action.Enable();
    }

    Vector3 smoothedLocalMovementToApply = Vector3.zero;
    private void Update()
    {
        Vector3 movementOnPlane = UpdateMovement();
        UpdateOrientation(movementOnPlane);
        Vector3 verticalMovement = UpdateVerticalVelocity();
        UpdateAnimation(movementOnPlane);

        Vector3 movementToApply = movementOnPlane + verticalMovement;
        characterController.Move(movementToApply * speed * Time.deltaTime);


        Vector3 UpdateMovement()
        {
            Vector2 actionValue = move.action.ReadValue<Vector2>();
            Vector3 movementInput = new Vector3(actionValue.x, 0, actionValue.y);

            Vector3 movementToApply = Vector3.zero;
            switch (movementMode)
            {
                case MovementMode.Global:
                    movementToApply = movementInput;
                    break;
                case MovementMode.FromCamera:
                    {
                        movementToApply = mainCamera.transform.TransformDirection(movementInput);
                        float movementMagnitude = movementToApply.magnitude;
                        movementToApply.y = 0;
                        movementToApply = movementToApply.normalized * movementMagnitude;
                    }
                    break;
            }
            return movementToApply;
        }

        void UpdateOrientation(Vector3 movementToApply)
        {
            Vector3 desiredDirection = Vector3.zero;
            switch (orientationMode)
            {
                case OrientationMode.ToMoveDirection:
                    desiredDirection = movementToApply;
                    break;
                case OrientationMode.ToCameraDirection:
                    desiredDirection = mainCamera.transform.forward;
                    desiredDirection.y = 0f; 
                    break;
            }

            float signedAngularDistance = Vector3.SignedAngle(transform.forward, desiredDirection, Vector3.up);
            float angleToApply = angularSpeed * Time.deltaTime;
            angleToApply = Mathf.Sign(signedAngularDistance) * Mathf.Min(angleToApply, Mathf.Abs(signedAngularDistance));

            Quaternion rotationToApply = Quaternion.AngleAxis(angleToApply, Vector3.up);
            transform.rotation = rotationToApply * transform.rotation;            
        }

        
        void UpdateAnimation(Vector3 movementToApply)
        {
            Vector3 localMovementToApply = transform.InverseTransformDirection(movementToApply);

            Vector3 direction = localMovementToApply - smoothedLocalMovementToApply;
            float smoothingStep = smoothAnimationSpeed * Time.deltaTime;

            smoothingStep = Mathf.Min(direction.magnitude, smoothingStep);
            smoothedLocalMovementToApply += direction.normalized * smoothingStep;

            animator.SetFloat("SideSpeed", smoothedLocalMovementToApply.x);
            animator.SetFloat("ForwardSpeed", smoothedLocalMovementToApply.z);
            animator.SetBool("Grounded", characterController.isGrounded);

            float t = Mathf.InverseLerp(jumpSpeed, -jumpSpeed, verticalVelocity);
            animator.SetFloat("NormalizedVSpeed", t);
        }

        Vector3 UpdateVerticalVelocity()
        {
            if (characterController.isGrounded)
            {
                verticalVelocity = minimunFallVelocity;
            }

            verticalVelocity += gravity * Time.deltaTime;

            if (jump.action.WasPressedThisFrame() && characterController.isGrounded)
            {
                verticalVelocity = jumpSpeed;
            }

            return verticalVelocity * Vector3.up;
        }
    }

    

    private void OnDisable()
    {
        move.action.Disable();
        jump.action.Disable();
    }
}

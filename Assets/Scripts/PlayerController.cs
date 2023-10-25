using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float minTurnSpeed = 0.3f;
    public float turnSpeed = 5f;
    
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _sprintAction;
    private Rigidbody _rigidbody;
    private InputAction _interactaAction;
    private float _moveSpeed;
    private Transform _camTransform;
    private Animator _animator;
    
    [SerializeField]
    private Interactable _CurrentInteractable;
    
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions.FindAction("Move");
        _sprintAction = _playerInput.actions.FindAction("Sprint");
        _rigidbody = GetComponent<Rigidbody>();
        _camTransform = Camera.main.transform;
        _interactaAction = _playerInput.actions.FindAction("Interact");
        _interactaAction.performed += Interact;
        _animator = GetComponentInChildren<Animator>();
        _moveSpeed = walkSpeed;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        // Handle Sprint
        if (_sprintAction.ReadValue<float>() == 0)  
        {
            _moveSpeed = walkSpeed; // when shift is not pressed
        }
        else
        {
            _moveSpeed = runSpeed; // when shift is pressed
        }
        
        // Get Input
        Vector2 input = _moveAction.ReadValue<Vector2>();
        float horizontalInput = input.x;
        float verticalInput = input.y;
        
        // Calculate velocity based on camera view
        Vector3 horizontalVelocity = Vector3.ProjectOnPlane(_camTransform.right, Vector3.up).normalized * horizontalInput;
        Vector3 verticalVelocity = Vector3.ProjectOnPlane(_camTransform.forward, Vector3.up).normalized * verticalInput;
        Vector3 velocity = Vector3.ClampMagnitude(horizontalVelocity + verticalVelocity, 1f) * _moveSpeed;
        
        // Rotate character
        if (velocity.magnitude > minTurnSpeed)
        {
            Quaternion rotation = Quaternion.Lerp(_rigidbody.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * turnSpeed);
            _rigidbody.rotation = rotation;
        }
 
        // Add gravity
        velocity.y = _rigidbody.velocity.y;
        
        // Set final velocity
        _rigidbody.velocity = velocity;
        
        float speed = velocity.magnitude;
        if (input == Vector2.zero)
        {
            speed = 0.0f;
        }
        _animator.SetFloat(name:"Speed", speed);


    }

    void Interact(InputAction.CallbackContext obj)
    {
        Debug.Log(message:"Interact");
    }

    private void OnTriggerEnter(Collider other)
    {
       Debug.Log(message:other+ " entered");

       Interactable collidedInteractable = other.GetComponent<Interactable>();
       if (collidedInteractable == null) { return;}

       _CurrentInteractable = collidedInteractable;
    }
    private void OnTriggerExit(Collider other)
    {
        
        Interactable collidedInteractable = other.GetComponent<Interactable>();
        
        if (_CurrentInteractable == null) {return;}

        _CurrentInteractable = null;

        
    }
    
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoboInteractionController : MonoBehaviour
{
    [SerializeField] private Transform pickupPosition;

    private HoboCharacterController _hoboCharacterController;
    
    // Controls
    private int _interactMouseButton = 0;

    private HashSet<InteractableObject> _nearbyInteractables;
    private Bridge _bridge;
    public Bridge bridge => _bridge;

    private PickupObject _currentPickupObject;

    ///-/////////////////////////////////////////////////////////////////////////////////////
    ///
    private void Awake()
    {
        _hoboCharacterController = GetComponentInParent<HoboCharacterController>();
        
        _nearbyInteractables = new HashSet<InteractableObject>();
    }
    
    ///-/////////////////////////////////////////////////////////////////////////////////////
    /// 
    private void Update()
    {
        OnUpdateInput();
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////
    /// 
    private void OnTriggerEnter(Collider other)
    {
        InteractableObject interactableObject = other.GetComponent<InteractableObject>();
        if (interactableObject != null)
        {
            _nearbyInteractables.Add(interactableObject);
        }

        Bridge bridge = other.GetComponent<Bridge>();
        if (bridge != null)
        {
            _bridge = bridge;
        }
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////
    ///
    private void OnTriggerExit(Collider other)
    {
        InteractableObject interactableObject = other.GetComponent<InteractableObject>();
        if (interactableObject != null)
        {
            _nearbyInteractables.Remove(interactableObject);
        }
        
        Bridge bridge = other.GetComponent<Bridge>();
        if (bridge != null)
        {
            _bridge = null;
        }
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////
    ///
    private void OnUpdateInput()
    {
        // Check for interaction input
        if (Input.GetMouseButtonDown(_interactMouseButton))
        {
            if (_currentPickupObject == null)
            {
                // Player is not currently carrying an object
                TryInteract();
            }
            else
            {
                // Player is currently carrying an object
                DropObject();
            }
        }
    }
    
    ///-/////////////////////////////////////////////////////////////////////////////////////
    ///
    private void TryInteract()
    {
        if (_nearbyInteractables.Count == 0)
        {
            return;
        }
        
        // FIND CLOSEST OBJECT
        InteractableObject closestObject = null;
        float closestDistance = 1000000f;
            
        // Iterate through all nearby objects
        foreach (InteractableObject interactableObject in _nearbyInteractables)
        {
            float distance = Vector3.Distance(transform.position, interactableObject.transform.position);
            if (distance < closestDistance)
            {
                closestObject = interactableObject;
                closestDistance = distance;
            }
        }
        
        if (closestObject != null)
        {
            InteractWithObject(closestObject);
        }
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////
    ///
    private void InteractWithObject(InteractableObject interactableObject)
    {
        if (interactableObject is PickupObject pickupObject && pickupObject.isPickedUp == false)
        {
            PickupObject(pickupObject);
        }

        _nearbyInteractables.Remove(interactableObject);
        
        interactableObject.Interact(this);
    }

    ///-/////////////////////////////////////////////////////////////////////////////////////
    ///
    private void PickupObject(PickupObject pickupObject)
    {
        // Set current pickup object
        _currentPickupObject = pickupObject;
        
        // Parent and position pickup object
        _currentPickupObject.transform.parent = _hoboCharacterController.transform;
        _currentPickupObject.transform.localPosition = pickupPosition.localPosition;
    }
    
    ///-/////////////////////////////////////////////////////////////////////////////////////
    ///
    private void DropObject()
    {
        _nearbyInteractables.Remove(_currentPickupObject);
        _currentPickupObject.Drop(this);
        _currentPickupObject.transform.parent = null;
        _currentPickupObject = null;
    }
}

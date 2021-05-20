﻿using UnityEngine;

public class DragRigidbodySword : MonoBehaviour
{
    public Test test;

    public float forceAmount = 500;

    private Rigidbody selectedRigidbody;
    public Vector3 desiredPosition;
    public Vector2 screenSize;

    Camera targetcamera;
    Vector3 originalScreenTargetPosition;
    Vector3 originalRigidbodyPos;
    
    float selectionDistance;
    bool isRigidbodySelected;
    public bool getPositionFromCamera;

    public Vector3 trackerPosition = Vector3.zero;

    private void Start()
    {
        targetcamera = GetComponent<Camera>();
        isRigidbodySelected = false;
        getPositionFromCamera = false;
        trackerPosition = Vector3.zero;
    }

    private void Update()
    {
        trackerPosition.x = test.positionX;
        trackerPosition.y = test.positionY;

        if (Input.GetKeyDown(KeyCode.C))
        {
            getPositionFromCamera = !getPositionFromCamera;
        }
        
        screenSize.y = Screen.height;
        screenSize.x = Screen.width;
        
        //if(!getPositionFromCamera)
        //    desiredPosition = Input.mousePosition;

        desiredPosition = !getPositionFromCamera ? Input.mousePosition : trackerPosition;

        if (!targetcamera)
            return;

        if (Input.GetMouseButtonDown(0) && !isRigidbodySelected)
        {
            // Grab
            isRigidbodySelected = true;
            selectedRigidbody = GetRigidbodyFromMouseClick();
        }
    }

    private void FixedUpdate()
    {
        if (selectedRigidbody)
        {
            Vector3 mousePositionOffset = targetcamera.ScreenToWorldPoint(new Vector3(desiredPosition.x, desiredPosition.y, selectionDistance)) - originalScreenTargetPosition;
            selectedRigidbody.velocity = (originalRigidbodyPos + mousePositionOffset - selectedRigidbody.transform.position) * forceAmount * Time.deltaTime;
        }
    }

    Rigidbody GetRigidbodyFromMouseClick()
    {
        RaycastHit hitInfo = new RaycastHit();
        Ray ray = targetcamera.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(ray, out hitInfo);
        if (hit)
        {
            if (hitInfo.collider.gameObject.GetComponent<Rigidbody>())
            {
                selectionDistance = Vector3.Distance(ray.origin, hitInfo.point);
                originalScreenTargetPosition = targetcamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectionDistance));
                originalRigidbodyPos = hitInfo.collider.transform.position;

                return hitInfo.collider.gameObject.GetComponent<Rigidbody>();
            }
        }

        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragRigidbody : MonoBehaviour
{
    public float forceAmount = 500;

    public Rigidbody selectedRigidbody;
    public Vector3 mousePosition;
    public Vector2 screenSize;

    Camera targetcamera;
    Vector3 originalScreenTargetPosition;
    Vector3 originalRigidbodyPos;
    float selectionDistance;

    private void Start()
    {
        targetcamera = GetComponent<Camera>();
    }

    private void Update()
    {
        mousePosition = Input.mousePosition;
        screenSize.y = Screen.height;
        screenSize.x = Screen.width;
        if (!targetcamera)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            // Grab
            selectedRigidbody = GetRigidbodyFromMouseClick();
        }

        if (Input.GetMouseButtonUp(0) && selectedRigidbody)
        {
            // Release
            selectedRigidbody = null;
        }
    }

    private void FixedUpdate()
    {
        if(selectedRigidbody)
        {
            Vector3 mousePositionOffset = targetcamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, selectionDistance)) - originalScreenTargetPosition;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleRaycast : MonoBehaviour
{
    [SerializeField] private int numOfRay;
    [SerializeField] private float rayRange;

    private void Start()
    {
        GetObjCollideRaycast(this.transform, "Cube (3)", rayRange);
    }

    private void Update()
    {
        Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hit, rayRange);
        Debug.DrawRay(this.transform.position, this.transform.forward * rayRange);
    }

    private GameObject GetObjCollideRaycast(Transform startObj, string nameObjToFind, float rayRange)
    {
        Physics.Raycast(startObj.position, this.transform.forward, out RaycastHit hit, rayRange);
        if (hit.collider != null)
        {
            GameObject blockObj = hit.collider.gameObject;
            Debug.LogWarning(blockObj.gameObject.name);
            Debug.DrawRay(startObj.position, this.transform.forward * rayRange);
            if (blockObj.name != nameObjToFind)
            {
                return GetObjCollideRaycast(blockObj.transform, nameObjToFind, rayRange);
            }
            else
            {
                return blockObj;
            }
        }
        else return startObj.gameObject;
    }
}


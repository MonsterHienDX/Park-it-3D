using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public int slotID;
    public bool isEmpty = true;
    private Collider _collider;

    public Vector3 gatePos;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        isEmpty = true;
    }

    public void EnableSlot(bool enable)
    {
        isEmpty = enable;
        this._collider.enabled = enable;
    }



}
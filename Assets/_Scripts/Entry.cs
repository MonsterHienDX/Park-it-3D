using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entry : MonoBehaviour
{
    public Slot[] slotList;

    public void Init()
    {
        slotList = GetComponentsInChildren<Slot>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public List<Slot> slotList = new List<Slot>();

    private void Start()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            slotList[i].slotID = i;
        }
    }
}

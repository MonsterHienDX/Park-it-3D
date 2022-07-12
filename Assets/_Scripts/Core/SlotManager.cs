using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public List<Slot> slotList = new List<Slot>();

    public void Init(Entry[] entries)
    {
        foreach (Entry e in entries)
        {
            for (int i = 0; i < e.slots.Length; i++)
                this.slotList.Add(e.GetSlot(i));
        }

        for (int i = 0; i < slotList.Count; i++)
        {
            slotList[i].slotID = i;
        }
    }
}

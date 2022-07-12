using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryManager : MonoBehaviour
{
    public Entry[] entries;

    public void Init()
    {
        entries = GetComponentsInChildren<Entry>();

        foreach (Entry e in entries)
        {
            e.Init();
        }
    }
}
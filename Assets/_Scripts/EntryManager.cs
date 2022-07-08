using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryManager : MonoBehaviour
{
    public List<Entry> entryList = new List<Entry>();
    private int entryCount = 0;
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        entryCount = entryList.Count;
        foreach (Entry e in entryList)
        {
            e.Init();
        }
    }
}

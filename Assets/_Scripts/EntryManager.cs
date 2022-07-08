using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryManager : MonoBehaviour
{
    public List<Transform> entryList = new List<Transform>();
    private int entryCount = 0;
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        entryCount = entryList.Count;
    }

    public Transform GetEntry()
    {
        return entryList[0];
    }

    public Transform GetNearestEntryFromCar(Transform carTrans)
    {
        float[] distanceCarEntries = new float[entryCount];
        float minDis = Vector3.Distance(carTrans.position, entryList[0].position);
        Transform nearestTrans = entryList[0];
        for (int i = 0; i < entryCount; i++)
        {
            distanceCarEntries[i] = Vector3.Distance(carTrans.position, entryList[i].position);
            if (distanceCarEntries[i] < minDis)
            {
                minDis = distanceCarEntries[i];
                nearestTrans = entryList[i];
            }
        }


        return nearestTrans;
    }

}

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public List<Car> carList = new List<Car>();
    private int _carInParkCount = 0;
    public int carParkedCount { get => _carInParkCount; }

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.CarGetInSlot, IncreaseCarParked);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.CarGetInSlot, IncreaseCarParked);
    }

    public void ResetCarsPos()
    {
        foreach (Car c in carList)
        {
            c.ResetState();
        }
    }

    public void MoveCarToSlot(Car car, Transform entryTrans, Slot slot)
    {
        car.MoveToEntryThenSlotByDOPath(entryTrans, slot);
    }

    private void IncreaseCarParked(object param = null)
    {
        _carInParkCount++;
        if (carParkedCount == carList.Count)
        {
            _ = GameManager.instance.WinLevel();
        }
    }
    private void DecreaseCarParked(object param = null)
    {
        if (_carInParkCount > 0)
            _carInParkCount--;
    }
}


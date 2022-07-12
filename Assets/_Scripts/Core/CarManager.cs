using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public Car2[] cars;
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

    public void Init()
    {
        cars = GetComponentsInChildren<Car2>();
    }

    public void MoveCarToSlot(Car car, Transform entryTrans, Slot slot)
    {
        car.MoveToEntryThenSlotByDOPath(entryTrans, slot);
    }

    private void IncreaseCarParked(object param = null)
    {
        _carInParkCount++;
        if (carParkedCount == cars.Length)
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
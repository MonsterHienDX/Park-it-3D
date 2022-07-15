using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public Car2[] cars;
    private int _carInParkCount = 0;
    public int carParkedCount { get => _carInParkCount; }
    private Stack<Car2> carMovedStack;

    private void Start()
    {
        carMovedStack = new Stack<Car2>();
    }

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.CarGetInSlot, IncreaseCarParked);
        EventDispatcher.Instance.RegisterListener(EventID.CarGetOutSlot, DecreaseCarParked);
        EventDispatcher.Instance.RegisterListener(EventID.CarStartMove, AddCarMovedToStack);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.CarGetInSlot, IncreaseCarParked);
        EventDispatcher.Instance.RemoveListener(EventID.CarGetOutSlot, DecreaseCarParked);
        EventDispatcher.Instance.RemoveListener(EventID.CarStartMove, AddCarMovedToStack);
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

    public void MovePreviousCarToStartPos()
    {
        if (carMovedStack.Count > 0)
            carMovedStack.Pop().StartMoveToStartPos();
        if (carMovedStack.Count <= 0)
            UIManager.instance.EnableUndoButton(false);
    }

    private void AddCarMovedToStack(object param = null)
    {
        Car2 c;
        if (param != null)
        {
            c = (Car2)param;
            carMovedStack.Push(c);
        }

        UIManager.instance.EnableUndoButton(true);
    }


}
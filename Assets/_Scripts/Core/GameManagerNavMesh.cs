using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerNavMesh : MonoBehaviour
{
    [SerializeField] private Car selectedCar;
    [SerializeField] private Slot selectedSlot;
    private float rayRange = 100f;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayRange))
            {
                if (hit.collider.TryGetComponent(out Car c))
                    selectedCar = c;
                if (hit.collider.TryGetComponent(out Slot slot))
                    if (slot.isEmpty)
                        selectedSlot = slot;
            }
            Debug.DrawRay(Camera.main.transform.position, ray.direction * rayRange);
        }

        if (selectedCar != null && selectedSlot != null)
        {
            MoveCarToSlot_NavMesh();
            selectedSlot.isEmpty = false;
            selectedCar = null;
            selectedSlot = null;
        }
    }

    private void MoveCarToSlot_NavMesh()
    {
        selectedCar.agent.SetDestination(selectedSlot.transform.position);
    }


}

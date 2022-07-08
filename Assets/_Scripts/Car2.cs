using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Car2 : MonoBehaviour
{
    [SerializeField] float _velocity = 0f;
    private bool isMoving;
    private Sequence mainTween;
    [SerializeField] private float rayRange;
    [SerializeField] private Slot testSlot;
    private bool findOutSlot;

    public void StartMoveToSlot(Slot slot)
    {
        isMoving = true;
        findOutSlot = false;
        mainTween = MoveAroundCarPark(slot)
              .Play();
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;

        Quaternion forwardRotation = Quaternion.AngleAxis(0, Vector3.up);
        Vector3 forwardDir = forwardRotation * transform.forward;
        Physics.Raycast(this.transform.position, forwardDir, out RaycastHit forwardHit, rayRange / 3);
        if (forwardHit.collider != null)
        {
            GameObject frontObj = forwardHit.collider.gameObject;
            Debug.LogWarning(frontObj.name);
            if (frontObj.tag == "Car")
            {
                mainTween.Pause();
            }

            else
            {
                mainTween.Play();
            }
        }
        else
        {
            mainTween.Play();
        }

        Quaternion backRotation = Quaternion.AngleAxis(180, Vector3.up);
        Vector3 backDir = backRotation * transform.forward;
        Physics.Raycast(this.transform.position, backDir, out RaycastHit backHit, rayRange);
        // if (hi)

        // if (FindSelectedSlotDuringMove(testSlot) != null && !findOutSlot)
        // {
        //     Vector3 slotPos = FindSelectedSlotDuringMove(testSlot).transform.position;
        //     // if (this.transform.position.x == slotPos.x || this.transform.position.z == slotPos.z)
        //     // {
        //     MoveToSlot(slotPos)
        //         .Play()
        //         .OnComplete(() =>
        //         {
        //             isMoving = false;
        //         });
        //     ;
        //     findOutSlot = true;
        //     // }
        // };

        Quaternion rightRayRotation = Quaternion.AngleAxis(90, Vector3.up);
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Physics.Raycast(this.transform.position, rightRayDirection, out RaycastHit rightHit, rayRange);
        // if (hi)
    }

    private void OnDrawGizmosSelected()
    {
        Quaternion leftRayRotation = Quaternion.AngleAxis(270, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);

        // Quaternion rightRayRotation = Quaternion.AngleAxis(90, Vector3.up);
        // Vector3 rightRayDirection = rightRayRotation * transform.forward;
        // Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);

        Quaternion forwardRotation = Quaternion.AngleAxis(0, Vector3.up);
        Vector3 forwardDir = forwardRotation * transform.forward;
        Gizmos.DrawRay(transform.position, forwardDir * rayRange / 3);

        // Quaternion backRotation = Quaternion.AngleAxis(180, Vector3.up);
        // Vector3 backDir = backRotation * transform.forward;
        // Gizmos.DrawRay(transform.position, backDir * rayRange);
    }

    private Sequence MoveAroundCarPark(Slot slot)
    {
        // Get main path points
        List<Vector3> mainPathPoints = GameManager.instance.mainPath.getPoints();

        // Find the nearest position
        float[] disToPathPoints = new float[mainPathPoints.Count];
        float minDis = Vector3.Distance(this.transform.position, mainPathPoints[0]);
        Vector3 nearestPoint = mainPathPoints[0];
        int nearestPointIndex = 0;
        for (int i = 0; i < mainPathPoints.Count; i++)
        {
            disToPathPoints[i] = Vector3.Distance(this.transform.position, mainPathPoints[i]);
            if (disToPathPoints[i] < minDis)
            {
                minDis = disToPathPoints[i];
                nearestPoint = mainPathPoints[i];
                nearestPointIndex = i;
            }
        }

        Sequence s = DOTween.Sequence();

        // Look at nearest position
        s.Append(this.transform.DOLookAt(nearestPoint, 0.3f));

        // Make the path for car to go around the car park from the position that nearest with start position
        List<Vector3> pathPointList = new List<Vector3>();
        pathPointList.Add(nearestPoint);
        for (int i = nearestPointIndex; i < mainPathPoints.Count; i++)
        {
            pathPointList.Add(mainPathPoints[i]);
        }
        for (int i = 0; i < nearestPointIndex; i++)
        {
            pathPointList.Add(mainPathPoints[i]);
        }
        pathPointList.Add(nearestPoint);

        s.Append(this.transform.DOPath(pathPointList.ToArray(), 10)
            .SetEase(Ease.Linear)
            .SetLookAt(0.03f))
            .OnUpdate(() =>
            {
                Slot selectedSlot = FindSelectedSlotDuringMove(slot);
                if (
                    (selectedSlot != null)
                    && (selectedSlot.transform.forward != this.transform.forward)
                   )
                {
                    Vector3 slotPos = FindSelectedSlotDuringMove(slot).transform.position;
                    // if (this.transform.position.x == slotPos.x || this.transform.position.z == slotPos.z)
                    // {
                    MoveToSlot(slotPos)
                        .Play()
                        .OnComplete(() =>
                        {
                            isMoving = false;
                        });
                    ;
                    findOutSlot = true;
                }
            })
            ;


        return s;
    }

    private Slot FindSelectedSlotDuringMove(Slot slotInput)
    {
        Vector3 leftRayDirection = Quaternion.AngleAxis(270, Vector3.up) * transform.forward;
        Physics.Raycast(this.transform.position, leftRayDirection, out RaycastHit leftHit, rayRange);

        GameObject blockRaycastObj = null;
        Slot slot = null;
        if (leftHit.collider != null)
        {
            blockRaycastObj = leftHit.collider.gameObject;
            Debug.LogWarning("_blockRaycastObj left: " + blockRaycastObj.name);
            if (blockRaycastObj.gameObject.tag == GameObjectTag.Entry.ToString())
            {
                slot = blockRaycastObj.GetComponentInParent<Slot>();
                if (slot != null)
                    if (slot.slotID == slotInput.slotID) return slot;
            }

            // Make another raycast form the position of the GO that collide with raycast if it's not the true slot
            // bool hasAnotherHit = false;
            // do
            // {
            //     Physics.Raycast(blockRaycastObj.transform.position, leftRayDirection, out RaycastHit hit, rayRange);
            //     if (hit.collider != null)
            //     {
            //         // FindSelectedSlotDuringMove(slot);
            //     }
            //     else
            //     {
            //         hasAnotherHit = true;
            //     }
            //     Debug.DrawRay(blockRaycastObj.transform.position, leftRayDirection);

            // } while (!hasAnotherHit);
        }
        return null;
    }
    private Tween TurnLeft(bool isLeft)
    {
        float newYRot = isLeft ? this.transform.localEulerAngles.y - 90 : this.transform.localEulerAngles.y + 90;

        Vector3 newDir = new Vector3(this.transform.localEulerAngles.x, newYRot, this.transform.localEulerAngles.z);

        Tween t = this.transform.DOLocalRotate(newDir, 0.3f)
            .SetEase(Ease.Linear);
        return t;
    }

    private Sequence MoveToSlot(Vector3 slotPosition)
    {
        mainTween.Kill();
        Vector3[] pathPoints = new Vector3[2];
        pathPoints[0] = this.transform.position;
        pathPoints[1] = new Vector3(slotPosition.x, this.transform.position.y, slotPosition.z);

        Sequence s = DOTween.Sequence();

        Tween t = this.transform.DOMove(pathPoints[1], 1f)
            // .SetLookAt(0.03f)
            .SetEase(Ease.Linear);

        s.Append(TurnLeft(true));
        s.Append(t);
        return s;
    }

}

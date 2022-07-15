using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Car2 : MonoBehaviour
{
    [SerializeField] private float _velocity = 0f;
    public bool isMoving;
    private Sequence mainSeq;
    [SerializeField] private float rayRange;
    [SerializeField] private float forceIntensity;
    private bool findOutSlot;
    public bool isInsidePark = false;
    private Vector3 startPos;
    private Slot selectedSlot;
    private Collider _collider;
    private Sequence moveBackSeq;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }
    private void Start()
    {
        startPos = this.transform.position;
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;

        Quaternion forwardRotation = Quaternion.AngleAxis(0, Vector3.up);
        Vector3 forwardDir = forwardRotation * transform.forward;
        Physics.Raycast(this.transform.position, forwardDir, out RaycastHit forwardHit, rayRange);
        HandlePauseCar(forwardHit, mainSeq);

        // Quaternion backRotation = Quaternion.AngleAxis(180, Vector3.up);
        // Vector3 backDir = backRotation * transform.forward;
        // Physics.Raycast(this.transform.position, backDir, out RaycastHit backHit, rayRange / 3);
        // HandlePauseCar(backHit, mainSeq);
    }

    private void OnDrawGizmosSelected()
    {
        Quaternion leftRayRotation = Quaternion.AngleAxis(270, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);

        Quaternion forwardRotation = Quaternion.AngleAxis(0, Vector3.up);
        Vector3 forwardDir = forwardRotation * transform.forward;
        Gizmos.DrawRay(transform.position, forwardDir * rayRange);

        // Quaternion backRotation = Quaternion.AngleAxis(180, Vector3.up);
        // Vector3 backDir = backRotation * transform.forward;
        // Gizmos.DrawRay(transform.position, backDir * rayRange / 3);
    }

    private void HandlePauseCar(RaycastHit forwardHit, Tween movingTween)
    {
        if (forwardHit.collider != null)
        {
            GameObject frontObj = forwardHit.collider.gameObject;
            if (frontObj.tag == "Car")
            {
                movingTween.Pause();
            }

            else
            {
                movingTween.Play();
            }
        }
        else
        {
            movingTween.Play();
        }
    }

    public void StartMoveToSlot(Slot slot)
    {
        // TODO
        if (isInsidePark) return;
        isMoving = true;
        findOutSlot = false;
        selectedSlot = slot;
        Debug.Log("slot.slotID: " + slot.slotID);
        // moveBackSeq.Kill();
        if (mainSeq != null) mainSeq.Kill();
        mainSeq = MoveAroundCarPark(slot)
            .Play()
            .OnUpdate(() =>
            {
                Slot slotFound = FindSelectedSlotDuringMove(slot);
                if (
                    (slotFound != null)
                   // && (slotFound.transform.forward != this.transform.forward)
                   )
                {
                    Vector3 slotPos = slotFound.transform.position;
                    this.mainSeq.Kill();
                    this.mainSeq = MoveToSlot(slotPos)
                        .Play()
                        .OnComplete(() =>
                        {

                            isMoving = false;
                            slot.EnableSlot(false);
                            EventDispatcher.Instance.PostEvent(EventID.CarGetInSlot);
                            EventDispatcher.Instance.PostEvent(EventID.CarMoing, false);
                        })
                        .OnKill(() =>
                        {
                            EventDispatcher.Instance.PostEvent(EventID.CarMoing, false);
                        })
                        ;
                    ;
                    findOutSlot = true;
                }
            })
            .OnKill(() =>
            {
                EventDispatcher.Instance.PostEvent(EventID.CarMoing, false);
            });
        EventDispatcher.Instance.PostEvent(EventID.CarMoing, true);
        EventDispatcher.Instance.PostEvent(EventID.CarStartMove, this);

    }

    private Sequence MoveAroundCarPark(Slot slot)
    {
        // List<Vector3> pathPointList = CreatePathToMoveAround(this.transform.position);
        List<Vector3> pathPointList = GameManager.instance.GetMainPathPointList();

        Sequence s = DOTween.Sequence();
        // Look at nearest position
        s.Append(this.transform.DOLookAt(pathPointList[0], 0.3f));
        s.Append(this.transform.DOPath(pathPointList.ToArray(), 100 / _velocity)
            .SetEase(Ease.Linear)
            .SetLookAt(0.03f))

            ;
        return s;
    }

    private List<Vector3> CreatePathToMoveAround(Vector3 startPos)
    {
        // Get main path points
        List<Vector3> mainPathPoints = GameManager.instance.GetMainPathPointList();

        // Find the nearest position
        Vector3 nearestPoint;
        int nearestPointIndex;
        FindNearestPointInPath(startPos, mainPathPoints, out nearestPoint, out nearestPointIndex);

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

        return pathPointList;
    }

    private void FindNearestPointInPath(Vector3 startPos, List<Vector3> mainPathPoints, out Vector3 nearestPoint, out int nearestPointIndex)
    {
        float[] disToPathPoints = new float[mainPathPoints.Count];
        float minDis = Vector3.Distance(startPos, mainPathPoints[0]);
        nearestPoint = mainPathPoints[0];
        nearestPointIndex = 0;
        for (int i = 0; i < mainPathPoints.Count; i++)
        {
            disToPathPoints[i] = Vector3.Distance(startPos, mainPathPoints[i]);
            if (disToPathPoints[i] < minDis)
            {
                minDis = disToPathPoints[i];
                nearestPoint = mainPathPoints[i];
                nearestPointIndex = i;
            }
        }
    }

    private Sequence MoveToFindStartPos()
    {
        if (!this.isInsidePark)
            return null;

        Sequence s = DOTween.Sequence();

        if (MoveBackToEntry() != null)
            s.Append(MoveBackToEntry());

        List<Vector3> pathPointList = CreatePathToMoveAround(selectedSlot.gatePos);

        // List<Vector3> mainPathPoints = GameManager.instance.GetMainPathPointList();

        // FindNearestPointInPath(startPos, mainPathPoints, out Vector3 nearestPoint, out int nearestPointIndex);

        s.Append(this.transform.DOLookAt(pathPointList[0], 0.3f));
        s.Append(this.transform.DOPath(pathPointList.ToArray(), 10f)
            .SetEase(Ease.Linear)
            .SetLookAt(0.03f))

            ;

        return s;
    }

    public void StartMoveToStartPos()
    {
        if (!this.isInsidePark) return;

        if (selectedSlot != null)
            selectedSlot.EnableSlot(true);
        moveBackSeq = MoveToFindStartPos()
            .OnUpdate(() =>
            {
                Vector3 startPosXZ = new Vector3(startPos.x, 1f, startPos.z);
                Vector3 currentPosXZ = new Vector3(this.transform.position.x, 1f, this.transform.position.z);
                Vector3 dir = (currentPosXZ - startPosXZ).normalized;
                Physics.Raycast(startPos, dir, out RaycastHit hit, rayRange * 2.5f);
                Debug.DrawRay(startPosXZ, dir * rayRange * 2.5f);
                if (hit.collider == this._collider)
                {
                    moveBackSeq.Kill();
                    moveBackSeq = MoveToStartPos()
                        .Play()
                        .OnComplete(() =>
                        {
                        }
                        )
                        ;
                }
            })
            .Play()
            .OnComplete(() =>
            {

                EventDispatcher.Instance.PostEvent(EventID.CarMoing, false);

            })
            .OnKill(() =>
            {
                EventDispatcher.Instance.PostEvent(EventID.CarMoing, false);
            })
            ;
        EventDispatcher.Instance.PostEvent(EventID.CarMoing, true);
        EventDispatcher.Instance.PostEvent(EventID.CarGetOutSlot, true);

    }

    private Sequence MoveToStartPos()
    {
        moveBackSeq = DOTween.Sequence();
        Sequence s = DOTween.Sequence();
        s.Append(this.transform.DOLookAt(startPos, 0.3f).SetEase(Ease.Linear));
        s.Append(this.transform.DOMove(startPos, 1f).SetEase(Ease.Linear));
        return s;
    }

    private Tween MoveBackToEntry()
    {
        if (selectedSlot != null)
        {
            Tween t = this.transform.DOMove(selectedSlot.gatePos, 1f)
                .SetEase(Ease.InQuad);
            return t;
        }
        return null;
    }

    private Slot FindSelectedSlotDuringMove(Slot slotInput)
    {
        Vector3 leftRayDirection = Quaternion.AngleAxis(270, Vector3.up) * transform.forward;
        Physics.Raycast(this.transform.position, leftRayDirection, out RaycastHit leftHit, rayRange);

        GameObject blockRaycastObj = null;
        if (leftHit.collider != null)
        {
            blockRaycastObj = leftHit.collider.gameObject;
            if (blockRaycastObj.gameObject.tag == GameObjectTag.Entry.ToString())
            {
                Entry entry = blockRaycastObj.GetComponentInParent<Entry>();
                if (entry != null)
                {
                    if ((int)Vector3.Angle(this.transform.forward, entry.transform.forward) == 90)
                        foreach (Slot s in entry.slots)
                        {
                            if (s.slotID == slotInput.slotID)
                                return s;
                        }
                }
            }
        }
        return null;
        // return GetSlotByRaycastAndSlotID(this.transform.position, leftRayDirection, slotInput, rayRange).GetComponent<Slot>();
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
        mainSeq.Kill();
        Vector3 slotPos = new Vector3(slotPosition.x, this.transform.position.y, slotPosition.z);
        float distance = Mathf.Abs(Vector3.Distance(this.transform.position, slotPos));

        Sequence s = DOTween.Sequence();

        Tween t = this.transform.DOMove(slotPos, distance / _velocity)
            .SetEase(Ease.OutQuad);

        s.Append(TurnLeft(true));
        s.Append(t);
        return s;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == GameObjectTag.CarPark.ToString())
        {
            this.isInsidePark = true;
        }

        if (collider.gameObject.tag == GameObjectTag.Slot.ToString())
        {
            this.selectedSlot.EnableSlot(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == GameObjectTag.CarPark.ToString())
        {
            this.isInsidePark = false;
        }

        if (collider.gameObject.tag == GameObjectTag.Slot.ToString())
        {
            this.selectedSlot.EnableSlot(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.instance.isPlaying && collision.gameObject.name.StartsWith("Car_"))
        {
            _ = GameManager.instance.LoseLevel();
            collision.gameObject.GetComponent<AudioSource>().Play();
            mainSeq.Kill();
            moveBackSeq.Kill();
            this.GetComponent<Rigidbody>().AddForce(this.transform.forward * forceIntensity, ForceMode.Impulse);
        }
    }

    // private GameObject GetSlotByRaycastAndSlotID(Vector3 start, Vector3 dir, Slot slotInput, float rayRange)
    // {
    //     Physics.Raycast(start, dir, out RaycastHit hit, rayRange);
    //     if (hit.collider != null)
    //     {
    //         GameObject blockObj = hit.collider.gameObject;
    //         Debug.LogWarning(blockObj.gameObject.name);
    //         Debug.DrawRay(start, dir * rayRange, new Color32((byte)UnityEngine.Random.Range(0, 255), 1, 1, 255));
    //         Slot slot = blockObj.GetComponent<Slot>();
    //         if (blockObj.GetComponent<Slot>() != null)
    //         {
    //             if (slot.slotID == slotInput.slotID)
    //                 return blockObj;

    //             else
    //                 return GetSlotByRaycastAndSlotID(blockObj.transform.position, dir, slotInput, rayRange);
    //         }
    //         else
    //             return GetSlotByRaycastAndSlotID(blockObj.transform.position, dir, slotInput, rayRange);
    //     }
    //     else
    //         return GetSlotByRaycastAndSlotID(start, dir, slotInput, rayRange);
    // }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class Car : MonoBehaviour
{
    [SerializeField] private float moveDurationPerStep;

    [Tooltip("The feild Y in Rotation of Transform component")]
    public List<MoveActionInfo> moveActionInfoList = null;
    private float yLocalRot;
    private float posX;
    private float posZ;
    [SerializeField] private float rayRange;
    public bool hasPark;
    private bool isMoving;
    private Tween mainTween;
    private Vector3 startPos;

    [SerializeField] private List<PathCreator> pathCreatorList = new List<PathCreator>();

    private void Start()
    {
        hasPark = false;
        posX = this.transform.position.x;
        posZ = this.transform.position.z;
        yLocalRot = transform.localEulerAngles.y;
        startPos = this.transform.position;
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;

        Quaternion forwardRotation = Quaternion.AngleAxis(0, Vector3.up);
        Vector3 forwardDir = forwardRotation * transform.forward;
        Physics.Raycast(this.transform.position, forwardDir, out RaycastHit forwardHit, rayRange);
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

        Quaternion leftRayRotation = Quaternion.AngleAxis(270, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Physics.Raycast(this.transform.position, leftRayDirection, out RaycastHit leftHit, rayRange);
        // if (leftHit.collider != null) Debug.LogWarning(leftHit.collider.gameObject.name);

        Quaternion rightRayRotation = Quaternion.AngleAxis(90, Vector3.up);
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Physics.Raycast(this.transform.position, rightRayDirection, out RaycastHit rightHit, rayRange);
        // if (hi)
    }

    private void OnDrawGizmosSelected()
    {
        Quaternion leftRayRotation = Quaternion.AngleAxis(270, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        // Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);

        Quaternion rightRayRotation = Quaternion.AngleAxis(90, Vector3.up);
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        // Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);

        Quaternion forwardRotation = Quaternion.AngleAxis(0, Vector3.up);
        Vector3 forwardDir = forwardRotation * transform.forward;
        // Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, forwardDir * rayRange);

        Quaternion backRotation = Quaternion.AngleAxis(180, Vector3.up);
        Vector3 backDir = backRotation * transform.forward;
        // Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, backDir * rayRange);
    }

    private Tween MoveAhead(bool isAhead, float distance)
    {
        distance = Mathf.Abs(distance);
        switch ((yLocalRot + 360) % 360)
        {
            case 90:
                posX = isAhead ? posX + distance : posX - distance;
                break;
            case 180:
                posZ = isAhead ? posZ - distance : posZ + distance;
                break;
            case 270:
                posX = isAhead ? posX - distance : posX + distance;
                break;
            case 360:
            case 0:
                posZ = isAhead ? posZ + distance : posZ - distance;
                break;
            default:
                break;
        }

        Vector3 newPos = new Vector3(posX, this.transform.position.y, posZ);

        Tween t;
        t = this.transform.DOMove(newPos, moveDurationPerStep * distance);
        // t.SetLoops(steps, LoopType.Incremental);
        t.SetEase(Ease.Linear);
        return t;
    }

    private Tween TurnLeft(bool isLeft)
    {
        if (isLeft) yLocalRot = yLocalRot - 90;
        else yLocalRot = yLocalRot + 90;
        // yLocalRot = isLeft ? yLocalRot - 90 : yLocalRot + 90;
        if (yLocalRot < 0) yLocalRot += 360;
        yLocalRot %= 360;

        float newYRot = isLeft ? this.transform.localEulerAngles.y - 90 : this.transform.localEulerAngles.y + 90;

        Vector3 newDir = new Vector3(this.transform.localEulerAngles.x, newYRot, this.transform.localEulerAngles.z);

        Tween t = this.transform.DOLocalRotate(newDir, moveDurationPerStep);
        t.SetEase(Ease.Linear);
        return t;
    }

    private Sequence ConverInfoToSequence(List<MoveActionInfo> actionList)
    {
        if (actionList == null)
        {
            Debug.LogWarning("Action list is null");
            return null;
        }
        Sequence s = DOTween.Sequence();
        foreach (MoveActionInfo a in actionList)
        {
            switch (a.moveDirection)
            {
                case MoveDirection.MoveAhead:
                    s.Append(MoveAhead(true, a.distance));
                    break;
                case MoveDirection.MoveBack:
                    s.Append(MoveAhead(false, a.distance));
                    break;
                case MoveDirection.TurnLeft:
                    s.Append(TurnLeft(true));
                    break;
                case MoveDirection.TurnRight:
                    s.Append(TurnLeft(false));
                    break;
                default: break;
            }
        }

        return s;
    }

    private Sequence MoveToPoint(Vector3 startPoint, Vector3 endPoint)
    {
        Sequence s = DOTween.Sequence();
        List<Vector3> pathPoints = new List<Vector3>();

        int zDirRatio = 0;
        int xDirRatio = 0;

        float disZ = endPoint.z - startPoint.z;
        bool targetIsFront = disZ < 0;
        zDirRatio = (targetIsFront) ? (int)CarDirection.NegativeZ : (int)CarDirection.PositiveZ;

        // Vector3 dir = (startPoint - endPoint).normalized;

        float disX = endPoint.x - startPoint.z;
        bool targetIsLeft = disX < 0;
        xDirRatio = targetIsLeft ? (int)CarDirection.NegativeX : (int)CarDirection.PositiveX;

        if (zDirRatio == (int)CarDirection.NegativeZ)
        {
            s.Append(MoveToNegativeZ(disZ));
        }
        else
        {
            s.Append(MoveToPositiveZ(disZ));
        }

        if (xDirRatio == (int)CarDirection.NegativeX)
        {
            s.Append(MoveToNegativeX(disX));
        }
        else
        {
            s.Append(MoveToPositiveX(disX));
        }

        return s;
    }
    public void MoveToEntryThenSlot(Transform entryTrans, Slot slot)
    {
        MoveToPoint(this.transform.position, entryTrans.position)
            .Play()
            .OnComplete(() =>
            {
                MoveToPoint(this.transform.position, slot.transform.position)
                    .Play();
            });
    }


    public void MoveToEntryThenSlotByDOPath(Transform entryTrans, Slot slot)
    {
        isMoving = true;
        mainTween = MoveToPointByDOPath(this.transform.position, entryTrans.position, slot)
               .Play()
               .OnComplete(() =>
               {
                   //    MoveToPointByDOPath(this.transform.position, slot.transform.position)
                   //        .Play()
                   //        .OnComplete(() =>
                   //        {
                   EventDispatcher.Instance.PostEvent(EventID.CarGetInSlot);
                   mainTween = null;
                   isMoving = false;

                   //    if (slot.isVertical)
                   //        _ = ActionAfterPark(TurnLeft(true));
                   //    });
               });
    }

    private Tween MoveToPointByDOPath(Vector3 startPoint, Vector3 endPoint, Slot slot = null)
    {
        List<Vector3> pathPoints = new List<Vector3>();

        pathPoints.Add(new Vector3(startPoint.x, startPoint.y, endPoint.z));
        pathPoints.Add(new Vector3(endPoint.x, startPoint.y, endPoint.z));

        List<Vector3> pathPointsCreator = new List<Vector3>();
        PathCreator p = null;
        if (GetPathCreatorBySlot(slot) != null) p = GetPathCreatorBySlot(slot);

        pathPointsCreator = p.getPoints();

        Tween t = transform.DOPath(pathPointsCreator.ToArray(), moveDurationPerStep * pathPoints.Count * 10).SetLookAt(0.05f);
        t.SetEase(Ease.Linear);
        t.OnWaypointChange((index) =>
        {
            if (index != pathPoints.Count - 1)
            {
                // TweenSettingsExtensions.SetLookAt(0.05f);
            }
            UpdateYLocalRot();
        });
        return t;
    }

    private void UpdateYLocalRot()
    {
        yLocalRot = transform.localEulerAngles.y;
        // Debug.LogWarning("yLocalRot: " + yLocalRot);
    }

    private PathCreator GetPathCreatorBySlot(Slot slot)
    {
        foreach (PathCreator p in pathCreatorList)
        {
            if (slot == p.slot)
                return p;
        }
        return null;
    }

    private Sequence MoveToNegativeZ(float distance)
    {
        Sequence s = DOTween.Sequence();
        if ((int)yLocalRot != (int)CarDirection.NegativeZ)
        {
            int rotateTime = (int)yLocalRot / 90;
            for (int i = 0; i < rotateTime; i++)
                s.Append(TurnLeft(false));
        }
        s.Append(MoveAhead(true, distance));
        return s;
    }
    private Sequence MoveToPositiveZ(float distance)
    {
        Sequence s = DOTween.Sequence();
        if ((int)yLocalRot != (int)CarDirection.PositiveZ)
        {
            int rotateTime = (int)yLocalRot / 90;
            for (int i = 0; i < rotateTime; i++)
                s.Append(TurnLeft(false));
        }
        s.Append(MoveAhead(true, distance));
        return s;
    }

    private Sequence MoveToNegativeX(float distance)
    {
        Sequence s = DOTween.Sequence();
        if ((int)yLocalRot != (int)CarDirection.NegativeX)
        {
            int rotateTime = ((int)yLocalRot + 90) / 90;
            bool checkLeft = (yLocalRot == 0 || yLocalRot == 360) ? true : false;

            for (int i = 0; i < rotateTime; i++)
                s.Append(TurnLeft(checkLeft));
        }
        s.Append(MoveAhead(true, distance));
        return s;
    }

    private Sequence MoveToPositiveX(float distance)
    {
        Sequence s = DOTween.Sequence();
        if ((int)yLocalRot != (int)CarDirection.PositiveX)
        {
            int rotateTime = ((int)yLocalRot + 90) / 90;
            bool checkLeft = (yLocalRot == 0 || yLocalRot == 360) ? true : false;
            for (int i = 0; i < rotateTime; i++)
                s.Append(TurnLeft(checkLeft));
        }
        s.Append(MoveAhead(true, distance));
        return s;
    }

    private async UniTask ActionAfterPark(Tween tween)
    {
        Debug.LogWarning(this.gameObject.name + "ActionAfterPark");

        await UniTask.Delay(500);
        if (tween != null) tween.Play();
    }

    public void ResetState()
    {
        this.transform.position = startPos;
        isMoving = false;
    }

}
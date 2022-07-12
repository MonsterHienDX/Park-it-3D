using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    public Car2 selectedCar = null;
    public Slot selectedSlot = null;
    [SerializeField] private float rayRange;
    [SerializeField] private Transform levelRootTrans;
    [SerializeField] private List<GameObject> levelPrefabList = new List<GameObject>();
    private GameObject currentLevel;
    private CarManager _carManager;
    private EntryManager _entryManager;
    private SlotManager _slotManager;
    [SerializeField] private CanvasGroup winPanelCvg;
    public PathCreatorMini _mainPath;
    private void Start()
    {
        Application.targetFrameRate = 60;
        LoadLevel();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayRange))
            {
                if (hit.collider.TryGetComponent(out Car2 c))
                    if (!c.isMoving)
                        selectedCar = c;
                if (hit.collider.TryGetComponent(out Slot slot))
                    if (slot.isEmpty)
                        selectedSlot = slot;
            }
            Debug.DrawRay(Camera.main.transform.position, ray.direction * rayRange);
        }

        if (selectedCar != null && selectedSlot != null)
        {
            ActionCar();
            selectedCar = null;
            selectedSlot = null;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
        }
    }

    private void ActionCar()
    {
        selectedCar.StartMoveToSlot(selectedSlot);
    }

    private void LoadLevel()
    {
        winPanelCvg.DOFade(0f, 0.3f);
        winPanelCvg.blocksRaycasts = false;

        if (currentLevel == null)
            currentLevel = Instantiate(levelPrefabList.PickRandom());

        currentLevel.transform.SetParent(levelRootTrans);

        LevelInfo levelInfo = currentLevel.GetComponent<LevelInfo>();
        _carManager = levelInfo.carManager;
        _slotManager = levelInfo.slotManager;
        _entryManager = levelInfo.entryManager;

        _entryManager.Init();
        _carManager.Init();
        _slotManager.Init(_entryManager.entries);

        _mainPath = levelInfo.mainPath;
    }

    public async UniTask WinLevel()
    {
        ShowCongratPanel();
        await UniTask.Delay(1000);

        if (levelRootTrans.childCount > 0)
            Destroy(levelRootTrans.GetChild(0).gameObject);
        currentLevel = null;
        LoadLevel();
    }

    private void ShowCongratPanel()
    {
        winPanelCvg.DOFade(1f, 0.3f);
        winPanelCvg.blocksRaycasts = true;
    }

    public List<Vector3> GetMainPathPointList()
    {
        return _mainPath.getPoints();
    }
}
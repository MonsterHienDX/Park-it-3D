using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    public Car selectedCar = null;
    public Slot selectedSlot = null;
    [SerializeField] private float rayRange;
    [SerializeField] private Transform levelRootTrans;
    [SerializeField] private List<GameObject> levelPrefabList = new List<GameObject>();
    private GameObject currentLevel;
    private CarManager _carManager;
    private EntryManager _entryManager;
    [SerializeField] private CanvasGroup winPanelCvg;

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
            MoveCarToSlot(selectedCar.transform);
            selectedSlot.isEmpty = false;
            selectedCar = null;
            selectedSlot = null;
        }
    }
    private void MoveCarToSlot(Transform carTrans)
    {
        _carManager.MoveCarToSlot(selectedCar, selectedSlot.entryTrans, selectedSlot);
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
        _entryManager = levelInfo.entryManager;

    }

    public async UniTask WinLevel()
    {
        Debug.LogWarning("Win level");
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

}

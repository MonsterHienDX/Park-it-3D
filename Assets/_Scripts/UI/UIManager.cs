using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    [SerializeField] private CanvasGroup winPanelCvg;
    [SerializeField] private CanvasGroup losePanelCvg;
    [SerializeField] private Button undoButton;
    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        undoButton.onClick.AddListener(GameManager.instance.MovePreviousCarToStartPos);
        EventDispatcher.Instance.RegisterListener(EventID.CarMoing, EnableUndoButton);
    }

    private void OnDisable()
    {
        undoButton.onClick.RemoveListener(GameManager.instance.MovePreviousCarToStartPos);
        EventDispatcher.Instance.RemoveListener(EventID.CarMoing, EnableUndoButton);
    }

    public void EnableWinLosePanel(bool isWin, bool enable)
    {
        if (isWin)
        {
            if (enable)
            {
                winPanelCvg.DOFade(1f, Const.PANEL_SLIDE_SPEED);
                winPanelCvg.blocksRaycasts = true;
            }
            else
            {
                winPanelCvg.DOFade(0f, Const.PANEL_SLIDE_SPEED);
                winPanelCvg.blocksRaycasts = false;
            }
        }
        else
        {
            if (enable)
            {
                losePanelCvg.DOFade(1f, Const.PANEL_SLIDE_SPEED);
                losePanelCvg.blocksRaycasts = true;
            }
            else
            {
                losePanelCvg.DOFade(0f, Const.PANEL_SLIDE_SPEED);
                losePanelCvg.blocksRaycasts = false;
            }
        }
    }

    public void EnableUndoButton(object param = null)
    {
        bool enable = (bool)param;

        if (param != null) enable = !enable;

        undoButton.enabled = enable;
    }
}

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    [SerializeField] private CanvasGroup winPanelCvg;
    [SerializeField] private CanvasGroup losePanelCvg;

    protected override void Awake()
    {
        base.Awake();
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
}

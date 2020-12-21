using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum UIType
{
    MainMenu,
    GameUI,
    EndScreen,
    GameOver
}
public class UIManager : MonoBehaviour
{
    List<UIController> uiControllerList;
    UIController activeUI;

    private void OnEnable()
    {
        EventManager.onSwitchUI += SwitchUI;
    }

    private void OnDisable()
    {
        EventManager.onSwitchUI -= SwitchUI;
    }
    void Awake()
    {
        uiControllerList = GetComponentsInChildren<UIController>().ToList();
        uiControllerList.ForEach(x => x.gameObject.SetActive(false));
        SwitchUI(UIType.MainMenu);
    }

    void SwitchUI(UIType ui)
    {
        if (activeUI != null)
        {
            activeUI.gameObject.SetActive(false);
        }

        UIController uiToShow = uiControllerList.Find(x => x.UIType == ui);
        if (uiToShow != null)
        {
            uiToShow.gameObject.SetActive(true);
            activeUI = uiToShow;
        }
    }

    public void OnStartGamePressed()
    {
        SwitchUI(UIType.GameUI);
        EventManager.StartGame();
    }

    public void OnRetryPressed()
    {
        EventManager.Retry();
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum UIType
{
    PausePanel,
    PlayInfoPanel,
    GameOverPanel,
}

public class UIPanelManager : Singleton<UIPanelManager>
{
    [SerializeField] private GameObject root;

    private Dictionary<UIType, UIPanel> panelDictionary = new Dictionary<UIType, UIPanel>();

    private List<UIType> unActiveExcludePanel = new List<UIType>();

    private void Awake()
    {
        SetExcludePanel();
    }

    private void SetExcludePanel()
    {
        unActiveExcludePanel.Add(UIType.PlayInfoPanel);
    }

    public T GetPanel<T>(UIType type) where T : UIPanel
    {
        return panelDictionary.ContainsKey(type) ? panelDictionary[type] as T : null;
    }

    public void OpenPanel(UIType type)
    {
        AllUnActivePanel();

        if (panelDictionary.ContainsKey(type))
        {
            panelDictionary[type].Active = true;
        }
        else
        {
            panelDictionary.Add(type, CreatePanel(type));
        }
    }

    public void ClosePanel(UIType type)
    {
        if (panelDictionary.ContainsKey(type))
        {
            UIPanel panel = panelDictionary[type];
            if (panel != null)
            {
                panel.Active = false;
            }
        }
    }

    private void AllUnActivePanel()
    {
        foreach (KeyValuePair<UIType, UIPanel> panel in panelDictionary)
        {
            if (!unActiveExcludePanel.Any(x => x == panel.Key))
            {
                if (panel.Value != null)
                    panel.Value.Active = false;
            }
        }
    }

    public UIPanel CreatePanel(UIType type)
    {
        GameObject panel = null;
        switch (type)
        {
            case UIType.PausePanel:
                panel = Instantiate(Resources.Load<GameObject>("UI/PausePanel"));
                break;
            case UIType.PlayInfoPanel:
                panel = Instantiate(Resources.Load<GameObject>("UI/PlayInfoPanel"));
                break;
            case UIType.GameOverPanel:
                panel = Instantiate(Resources.Load<GameObject>("UI/GameOverPanel"));
                break;
        }

        panel.transform.SetParent(root.transform, false);
        panel.transform.localScale = Vector3.one;

        ResetTranformOffset(panel.GetComponent<RectTransform>());
        return panel.GetComponent<UIPanel>();
    }

    private void ResetTranformOffset(RectTransform rectTransform)
    {
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
}

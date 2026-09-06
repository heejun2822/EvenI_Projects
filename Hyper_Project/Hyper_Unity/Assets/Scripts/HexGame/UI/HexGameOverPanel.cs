using UnityEngine;
using UnityEngine.UI;

/// <summary>Displays the game-over panel and publishes its home request.</summary>
public sealed class HexGameOverPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelObject;

    private Button homeButton;

    private void Awake()
    {
        homeButton = panelObject.GetComponentInChildren<Button>(true);
    }

    private void OnEnable()
    {
        homeButton.onClick.AddListener(ReturnToTitle);
        EventBus<GameResultChangedEvent>.Subscribe(Refresh);
    }

    private void OnDisable()
    {
        homeButton.onClick.RemoveListener(ReturnToTitle);
        EventBus<GameResultChangedEvent>.Unsubscribe(Refresh);
    }

    private void ReturnToTitle() => EventBus<ReturnToTitleRequestedEvent>.Publish(new ReturnToTitleRequestedEvent());

    private void Refresh(GameResultChangedEvent payload)
    {
        bool show = payload.Result == HexGameResult.GameOver;
        panelObject.SetActive(show);
    }
}

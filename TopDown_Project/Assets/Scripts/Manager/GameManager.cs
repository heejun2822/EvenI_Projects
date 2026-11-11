using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerController player { get; private set; }
    private ResourceController _playerResourceController;

    [SerializeField] private int currentWaveIndex = 0;

    private EnemyManager enemyManager;
    private UIManager uiManager;
    public static bool isFirstLoading = true;

    private void Awake()
    {
        instance = this;
        player = FindAnyObjectByType<PlayerController>();
        player.Init(this);

        uiManager = FindAnyObjectByType<UIManager>();

        _playerResourceController = player.GetComponent<ResourceController>();
        _playerResourceController.RemoveHealthChangeEvent(uiManager.ChangePlayerHP);
        _playerResourceController.AddHealthChangeEvent(uiManager.ChangePlayerHP);

        enemyManager = GetComponentInChildren<EnemyManager>();
        enemyManager.Init(this);
    }

    private void Start()
    {
        if (!isFirstLoading)
        {
            StartGame();
        }
        else
        {
            isFirstLoading = false;
        }
    }

    public void StartGame()
    {
        uiManager.SetPlayGame();
        StartNextWave();
    }

    private void StartNextWave()
    {
        currentWaveIndex += 1;
        uiManager.ChangeWave(currentWaveIndex);
        enemyManager.StartWave(1 + currentWaveIndex / 5);
    }

    public void EndOfWave()
    {
        StartNextWave();
    }

    public void GameOver()
    {
        uiManager.SetGameOver();
        enemyManager.StopWave();
    }
}

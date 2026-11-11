using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerController player { get; private set; }
    private ResourceController _playerResourceController;

    [SerializeField] private int currentStageIndex = 0;
    [SerializeField] private int currentWaveIndex = 0;

    private EnemyManager enemyManager;
    private UIManager uiManager;
    public static bool isFirstLoading = true;

    private CameraShake cameraShake;

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

        cameraShake = FindAnyObjectByType<CameraShake>();
        MainCameraShake();
    }

    public void MainCameraShake()
    {
        cameraShake.ShakeCamera(1, 1, 1);
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
        // StartNextWave();
        StartStage();
    }

    private void StartNextWave()
    {
        currentWaveIndex += 1;
        uiManager.ChangeWave(currentWaveIndex);
        enemyManager.StartWave(1 + currentWaveIndex / 5);
    }

    public void EndOfWave()
    {
        // StartNextWave();
        StartNextWaveInStage();
    }

    public void GameOver()
    {
        uiManager.SetGameOver();
        enemyManager.StopWave();
    }

    public void StartStage()
    {
        StageInfo stageInfo = GetStageInfo(currentStageIndex);

        if (stageInfo == null)
        {
            Debug.Log("스테이지 정보가 없습니다.");
            return;
        }

        uiManager.ChangeWave(currentStageIndex + 1);

        enemyManager.StartStage(stageInfo.waves[currentWaveIndex]);
    }

    public void StartNextWaveInStage()
    {
        StageInfo stageInfo = GetStageInfo(currentStageIndex);
        if (stageInfo.waves.Length - 1 > currentWaveIndex)
        {
            currentWaveIndex++;
            StartStage();
        }
        else
        {
            CompleteStage();
        }
    }

    public void CompleteStage()
    {
        currentStageIndex++;
        currentWaveIndex = 0;
        StartStage();
    }

    private StageInfo GetStageInfo(int stageKey)
    {
        foreach (var stage in StageData.Stages)
        {
            if (stage.stageKey == stageKey)
                return stage;
        }
        return null;
    }
}

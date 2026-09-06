using UnityEngine;

/// <summary>Coordinates game-flow transitions between independently focused gameplay services.</summary>
public class HexGameController : BaseSceneDirector
{
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private HexPlayerController playerController;

    private HexGameFlow gameFlow;
    private PointerGestureInput gestureInput;
    private HexBoardView boardView;
    private HexStageController stageController;

    protected override void Awake()
    {
        base.Awake();
        gestureInput = GetComponent<PointerGestureInput>();
        boardView = GetComponent<HexBoardView>();
        stageController = GetComponent<HexStageController>();
    }

    protected override bool InitializeScene()
    {
        if (!HasRequiredReferences())
        {
            return false;
        }

        gestureInput.Initialize();
        gameFlow = new HexGameFlow(new HexGameSession(gameSettings), stageController, playerController);
        gameFlow.Start();
        return true;
    }

    protected override void OnDestroy()
    {
        gameFlow?.Dispose();
        base.OnDestroy();
    }

    private void Update()
    {
        if (!IsInitialized)
        {
            return;
        }

        gameFlow?.Tick(Time.deltaTime);
    }

    private bool HasRequiredReferences()
    {
        if (gameSettings != null && gestureInput != null && boardView != null && boardView.IsConfigured &&
            stageController != null && stageController.IsConfigured && playerController != null && playerController.IsConfigured)
        {
            return true;
        }

        Debug.LogError("HexGameController is missing a required reference.", this);
        return false;
    }
}

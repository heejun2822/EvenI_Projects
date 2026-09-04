using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

/// <summary>Coordinates game-flow transitions between independently focused gameplay services.</summary>
public class HexGameController : BaseSceneDirector
{
    [Header("Scene References")]
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase normalTile;
    [SerializeField] private Transform player;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private TextMeshPro formulaLabelPrefab;
    [SerializeField] private SpriteRenderer glowPrefab;
    [SerializeField] private Transform labelRoot;
    [SerializeField] private Transform glowRoot;

    [Header("Tile Colors")]
    [SerializeField] private Color defaultTileColor = new(.87f, .72f, .21f);
    [SerializeField] private Color goalTileColor = new(1f, .45f, .45f);
    [SerializeField] private Color healthTileColor = new(.3f, .85f, .5f);
    [SerializeField] private Color unavailableTileColor = new(.32f, .32f, .32f);

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI goalText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button[] formulaButtons;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject clearPanel;
    [SerializeField] private Button[] homeButtons;

    private HexGameFlow gameFlow;
    private CameraPanZoom cameraPanZoom;

    protected override bool InitializeScene()
    {
        if (!HasRequiredReferences() || !TryGetGestureInput(out PointerGestureInput gestureInput))
        {
            return false;
        }

        gestureInput.Initialize();
        gameFlow = CreateGameFlow(gestureInput);
        gameFlow.Start();
        return true;
    }

    protected override void OnDestroy()
    {
        gameFlow?.Dispose();
        cameraPanZoom?.UnbindGestureInput();
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

    private HexGameFlow CreateGameFlow(PointerGestureInput gestureInput)
    {
        HexBoardView boardView = new(tilemap, normalTile, formulaLabelPrefab, glowPrefab, labelRoot, glowRoot,
            defaultTileColor, goalTileColor, healthTileColor, unavailableTileColor);
        cameraPanZoom = mainCamera.GetComponent<CameraPanZoom>();
        if (cameraPanZoom != null)
        {
            cameraPanZoom.BindGestureInput(gestureInput);
        }

        HexGameSession session = new(gameSettings);
        HexStageController stageController = new(tilemap, player, boardView, cameraPanZoom);
        HexPlayerController playerController = new(mainCamera, tilemap, stageController);
        playerController.Bind(gestureInput);
        HexGameUi ui = new(scoreText, healthText, goalText, timerText, formulaButtons, gameOverPanel, clearPanel, homeButtons);
        return new HexGameFlow(session, stageController, playerController, ui);
    }

    private bool TryGetGestureInput(out PointerGestureInput gestureInput)
    {
        gestureInput = GetComponent<PointerGestureInput>();
        if (gestureInput != null)
        {
            return true;
        }

        Debug.LogError("HexGameController requires PointerGestureInput.", this);
        return false;
    }

    private bool HasRequiredReferences()
    {
        if (gameSettings != null && tilemap != null && normalTile != null && player != null && mainCamera != null &&
            formulaLabelPrefab != null && glowPrefab != null && labelRoot != null && glowRoot != null &&
            scoreText != null && healthText != null && goalText != null && timerText != null && formulaButtons != null &&
            gameOverPanel != null && clearPanel != null && homeButtons != null)
        {
            return true;
        }

        Debug.LogError("HexGameController is missing a required reference.", this);
        return false;
    }
}

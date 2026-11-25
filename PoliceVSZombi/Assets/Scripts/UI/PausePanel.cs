using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : UIPanel
{
    GameManager gameManager;

    private void OnEnable()
    {
        if(gameManager == null)
            gameManager = GameManager.Instance;

        gameManager.Pause(true);
    }

    private void OnDisable()
    {
        gameManager.Pause(false);
    }

}

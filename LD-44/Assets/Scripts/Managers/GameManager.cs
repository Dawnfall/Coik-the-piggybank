using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using helper;

public class GameManager : AMonoSingleton<GameManager>
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            HelperUnity.ExitGame();
    }

    public int SavedHealth = -1;
}

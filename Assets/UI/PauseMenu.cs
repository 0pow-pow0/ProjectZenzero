using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private GameObject cardPowerUpViewerScreen;

    #region SCREENS BUTTONS


    #endregion

    ///-----------------------------
    ///------ PAUSE MAIN MENU
    ///-----------------------------
    void RenderPauseMenu()
    {
        //GameManager.GI().isGamePaused = true;

        //  cardViewerScreen.RenderScreen();

    }

    ///-----------------------------
    ///------ RENDER POWER UP SELECTION SCREEN
    ///-----------------------------
    void RenderCardPowerUpViewerScreen()
    {

    }

    // Start is called before the first frame update
    void Awake()
    {
        //cardSelectionScreen = UtilityShit.FindChildWithName(gameObject, "CardPowerUpSelection")
         //   .GetComponent<CardPowerUpsSelectionScreen>();
        
        //cardViewerScreen = UtilityShit.FindChildWithName(gameObject, "CardPowerUpsViewer")
        //    .GetComponent<CardPowerUpsViewerScreen>();
 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameManager.GI().isGamePaused)
            {

            }
            else 
            {
                
                //RenderPauseMenu();
            }
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            if (GameManager.GI().isGamePaused)
            {
                //RenderPowerUpSelectionScreen(false);
            }

            else
            {
                //RenderPowerUpSelectionScreen(true);
            }
        }
    }
}

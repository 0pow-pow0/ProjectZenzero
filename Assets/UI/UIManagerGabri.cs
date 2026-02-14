using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Dovrei trovare un modo per fare in modo che possano esistere solo singole schermate.
/// Al momento gli script si possono agganciare a piu' gameobject, ma non ha senso questa robas.
/// </summary>
public class UIManagerGabri : MonoBehaviour
{
    #region SINGLETONSHIT
    private static UIManagerGabri instance;

    public static UIManagerGabri GI()
    {
        if (instance == null)
        {
            Debug.Log("UIManager non instanziato");
        }

        return instance;
    }
    #endregion

    //------ Reference a tutti i pannelli/schermate
    public InGameUIPanel inGameUIPanel { get; private set; }
    public OptionsMenuPanel optionsMenuPanel { get; private set; }
    public CardPowerUpsViewerScreen cardPowerUpsViewerScreen { get; private set; }
    public CardPowerUpsSelectionScreen cardPowerUpsSelectionScreen { get; private set; }

    public GameObject canvas;

    ///<summary>
    /// Non tutti gli elementi lo possiedono.
    /// Serve per sapere se il mouse si trova sopra un'elemento della ui.
    /// In pratica capitava che il mouse triggerasse l'attacco del player anche quando si
    /// voleva cliccare un'elemento della UI. Questo bool risolve il problema.
    /// Utilizzato nella sezione input del player.
    /// </summary>
    public bool isMouseHoveringUI { get; private set; } = false;

    public AudioMixerGroup Mixer1 { get; private set; }
    
    [NonSerialized, Range(0.7f, 1f)] public float pressScale = 0.8f;
    public float pressDuration { get; private set; } = 0.08f;   // durata compressione
    public float pressHold { get; private set; } = 0.03f;       // piccolo �hold� premuto



    private void Awake()
    {
        instance = this;

        InGameUIPanel[] _inGameUIPanel = GetComponentsInChildren<InGameUIPanel>();
        OptionsMenuPanel[] _optionsMenuPanel = GetComponentsInChildren<OptionsMenuPanel>();
        CardPowerUpsViewerScreen _cardPowerUpsViewerScreen = 
            GetComponentInChildren<CardPowerUpsViewerScreen>();
        CardPowerUpsSelectionScreen[] _cardPowerUpsSelectionScreens = 
            GetComponentsInChildren<CardPowerUpsSelectionScreen>();

        canvas = GameObject.Find("Canvas"); 

        inGameUIPanel = _inGameUIPanel[0];
        optionsMenuPanel = _optionsMenuPanel[0];
        cardPowerUpsSelectionScreen = _cardPowerUpsSelectionScreens[0];
    }

    public void Update()
    {
        //Debug.Log("Mouse hovering UI: " + isMouseHoveringUI);
    }

    public IEnumerator PressThen(GameObject button, Action afterPress)
    {
        if (button == null) yield break;

        Transform tr = button.transform;
        Vector3 original = tr.localScale;
        Vector3 pressed = original * GI().pressScale;

        // shrink fluido
        float t = 0f;
        while (t < GI().pressDuration)
        {
            t += Time.unscaledDeltaTime;
            tr.localScale = Vector3.Lerp(original, pressed, t / GI().pressDuration);
            yield return null;
        }

        if (GI().pressHold > 0f)
            yield return new WaitForSecondsRealtime(GI().pressHold);

        // ritorna alla scala originale
        t = 0f;
        while (t < GI().pressDuration)
        {
            t += Time.unscaledDeltaTime;
            tr.localScale = Vector3.Lerp(pressed, original, t / GI().pressDuration);
            yield return null;
        }
        tr.localScale = original;

        // solo ora nascondi/mostra cose
        afterPress?.Invoke();
    }

    public void EnterUIElement()
    {
        isMouseHoveringUI = true;
    }

    public void ExitUIElement()
    {
        isMouseHoveringUI = false;
    }

}





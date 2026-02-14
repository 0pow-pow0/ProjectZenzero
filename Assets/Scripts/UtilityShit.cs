using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using System.Collections;
using System;
using System.ComponentModel.Design;

/// <summary>
/// Serve per evitare di andare a cercare in tutto 
/// l'albero un gameobject con un tag determinato.
/// </summary>
/// <param name="parent">Padre da cui si parte per ricercare.</param>
/// <param name="nameToFind">Tag da ricercare.</param>
/// <returns></returns>
/// 
class UtilityShit
{
    /// <summary>
    /// Non ricorsivo, si ferma al livello piu' alto di figli
    /// </summary>
    /// <returns></returns>
    public static GameObject[] GetChildrenAsGameObjects(GameObject parent)
    {
        GameObject[] children = new GameObject[parent.transform.childCount];
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            children[i] = parent.transform.GetChild(i).gameObject;
        }

        return children;
    }

    public static GameObject[] FindChildrenWithName(GameObject parent, string nameToFind)
    {
        List<GameObject> foundChildren = new List<GameObject>();
        foreach (Transform trf in parent.transform)
        {
            if (trf.gameObject.name == nameToFind)
            {
                foundChildren.Add(trf.gameObject);
            }
        }

        return foundChildren.ToArray();
    }

    public static GameObject FindChildWithName(GameObject parent, string nameToFind, bool reiterated = false)
    {
        foreach (Transform trf in parent.transform)
        {
            if (trf.childCount > 0)
            {
                GameObject obj = FindChildWithName(trf.gameObject, nameToFind, true);
                if (obj != null)
                {
                    return obj;
                }
            }

            if (trf.gameObject.name == nameToFind)
            {
                return trf.gameObject;
            }
        }
        if (!reiterated)
            Debug.LogWarning("Gameobject with name: " + nameToFind + " not found!");
        return null;
    }

    public static GameObject FindChildWithTag(GameObject parent, string tagToFind, bool reiterated = false)
    {
        foreach (Transform trf in parent.transform)
        {
            if (trf.childCount > 0)
            {
                GameObject obj = FindChildWithName(trf.gameObject, tagToFind, true);
                if (obj != null)
                {
                    return obj;
                }
            }

            if (trf.gameObject.tag == tagToFind)
            {
                return trf.gameObject;
            }
        }
        if (!reiterated)
            Debug.LogWarning("Gameobject with tag: " + tagToFind + " not found!");
        return null;
    }

    ///<summary>
    /// Permette di colorare il testo O.O :O
    ///</summary>
    public static void Log(object message, Color clr)
    {
        if(clr == null)
        {
            Debug.Log(message);
        }
        Debug.Log("<color=" + "#" + clr.ToHexString() + ">" + message + "</color>");
    }

    public static string AddColor(string original, Color toAddColor)
    {
        string result = "<color=#" + toAddColor.ToHexString() + ">" + original + "</color>";
        return result;
    }
}

/// <summary>
/// Timer dipendente dalla variabile deltaTime di unity
/// </summary>
public class DeltaTimer
{

    private float elapsedTime; // Tempo trascorso dall'attivazione del timer

    public float duration { get; private set; } // Durata del timer
    public bool hasEnded { get; private set; } = false;
    private DeltaTimer(float _duration) 
    { 
        duration = _duration;
    }

    static public DeltaTimer CreateTimer(float _duration)
    {
        DeltaTimer newTimer = new DeltaTimer(_duration);
        GameManager.GI().AddDeltaTimer(newTimer);
        return newTimer;
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= duration)
        {
            hasEnded = true;
        }
    }

    ~DeltaTimer()
    {
        GameManager.GI().RemoveDeltaTimer(this);
    }
}

/// <summary>
/// Cronometro influenzato da deltaTime
/// </summary>
public class Stopwatch
{
    public float elapsedTime { get; private set; } = 0;


    private Stopwatch()
    {

    }

    public bool toDestroy { get; private set; } = false;
    private bool isPaused = false;
    public void Pause()
    {
        isPaused = true;
    }

    public void Reset()
    {
        elapsedTime = 0;
    }

    public static Stopwatch CreateStopwatch()
    {
        Stopwatch newStopWatch = new Stopwatch();
        GameManager.GI().AddStopwatch(newStopWatch);
        return newStopWatch;
    }

    public void Update()
    {
        if(!isPaused)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    ~Stopwatch()
    {
        toDestroy = true;
        //GameManager.GI().RemoveStopwatch(this);
    }
}

public class DeltaTimerAction
{
    // Codice da eseguire allo scadere del timer
    private Action toExec;
    
    // Dev'essere rimosso dopo aver eseguito toExec?
    private bool destroyAfterAction;

    private float elapsedTime = 0f;
    private float duration;


    private DeltaTimerAction() { }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="toDestroy">Deve essere distrutto dopo aver eseguito
    /// la funzione allo scadere del timer?</param>
    public static DeltaTimerAction CreateDeltaTimerAction(Action _toExe, float _duration, bool toDestroy) 
    {
        DeltaTimerAction newDTA = new DeltaTimerAction();
        newDTA.destroyAfterAction = toDestroy;
        newDTA.duration = _duration;
        newDTA.toExec = _toExe;

        GameManager.GI().AddDeltaTimerAction(newDTA);
        return newDTA;
    }

    bool toDestroy = false;
    public void Update()
    {
        elapsedTime += Time.deltaTime;
        
        if(elapsedTime >= duration)
        {
            toExec.Invoke();

            if(destroyAfterAction)
            {
                GameManager.GI().RemoveDeltaTimerAction(this);
            }

            elapsedTime = 0f;
        }

    }
}

public class DebugText
{
    private string textToRender;

    public GameObject gameObject;
    private TextMeshProUGUI text;

    public DebugText()
    {
        gameObject = new GameObject();
        gameObject.transform.SetParent(GameManager.GI().debugTextWrapper.transform);
        text = gameObject.AddComponent<TextMeshProUGUI>();
        RectTransform rctTrans = gameObject.GetComponent<RectTransform>();

        // Setta al centro dello schermo
        rctTrans.anchorMin = new Vector2(0.5f, 0.5f);
        rctTrans.anchorMax = new Vector2(0.5f, 0.5f);
        rctTrans.pivot = new Vector2(0.5f, 0.5f);
    }

    public void UpdateDebugText(string _toRender, Vector3 pos, Camera mainCam)
    {
        Vector3 screenPos = mainCam.WorldToScreenPoint(pos);
        gameObject.transform.position = screenPos;
        text.text = _toRender;
    }
    


    ~DebugText()
    {
    }
}


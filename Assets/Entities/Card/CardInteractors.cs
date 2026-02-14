using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Interactions
{ 
    POINTER_UP =0,
    POINTER_DOWN,
    POINTER_ENTER,
    SELECT,
    DESELECT,
    
}


/// <summary>
/// Se si aggiungono piu' iteratori che definiscono la stessa funzione chiaramente si sminchiera' qualcosa xD
/// </summary>
public class UIInteractors
{
    /// <summary>
    /// Al momento ogni UIInteractors supporta solo UNA funzione per ogni key.
    /// </summary>
    public Dictionary<EventTriggerType, Action> events;

    protected UIInteractors()
    {
        events = new Dictionary<EventTriggerType, Action>();

    }
}

/// <summary>
/// Durante i potenziamenti quando seleziono la carta da aggiungere alla mia pool di potenziamenti
/// </summary>
public class UIInteractorCardPowerUpSelection : UIInteractors
{
    public bool isSelected = false;
    private UnityEngine.Vector2 placeholder;
    private CardUIMimic scr;

    private GameObject selectButton;
    private bool isOnSelectableButton;
    private RectTransform selectButtonRectTrans;
    EventTrigger.Entry GenerateDelegate(EventTriggerType eventType, System.Action<PointerEventData> act)
    {
        EventTrigger.Entry ev = new EventTrigger.Entry();
        ev.eventID = eventType;
        ev.callback.AddListener((data) => { act.Invoke((PointerEventData)data); });
        return ev;
    }
    //private 
    public UIInteractorCardPowerUpSelection(CardUIMimic crdM, UnityEngine.Vector3 plc)
    {
        scr = crdM;
        placeholder = new UnityEngine.Vector2(plc.x, plc.y);
        events.Add(EventTriggerType.PointerEnter, PointerHover);
        events.Add(EventTriggerType.PointerDown, PointerDown);
        events.Add(EventTriggerType.Select, ObjectSelect);
        events.Add(EventTriggerType.Deselect, ObjectDeselect);

        // Se non ha il component aggiungilo
        if(scr.gameObject.GetComponent<Selectable>() == null)
        {
            Selectable sle = scr.gameObject.AddComponent<Selectable>();
            sle.transition = Selectable.Transition.None;    
        }

        selectButton = GameObject.Instantiate(GameManager.GI().GetPrefab("CardButton"), scr.transform);
        
        
        EventTrigger selectEvent = selectButton.GetComponent<EventTrigger>();
        selectEvent.triggers.Add(GenerateDelegate(EventTriggerType.PointerEnter, (PointerEventData ptrEv) => { isOnSelectableButton = true; }));
        selectEvent.triggers.Add(GenerateDelegate(EventTriggerType.PointerExit, (PointerEventData ptrEv) => { isOnSelectableButton = false; }));

        //scr.rectTrans.anchoredPosition = new UnityEngine.Vector2(placeholder.x, placeholder.y);

        selectButtonRectTrans = selectButton.GetComponent<RectTransform>();
        selectButton.transform.SetAsFirstSibling();
        selectButtonRectTrans.anchoredPosition = UnityEngine.Vector2.zero;
        //selectButton.transform.position = scr.rectTrans.anchoredPosition;

        selectButton.GetComponentInChildren<Button>().onClick.AddListener(PointerDownOnSelectionButton);

        selectButton.SetActive(false);
    }


    bool isAnimatingEnter = false;

    IEnumerator AnimationSelectionButtonEnter()
    {
        if (isAnimatingEnter)
            yield break;

        if (isAnimatingExit)
            yield break;

        isAnimatingEnter = true;

        selectButton.SetActive(true);

        float yDest = scr.rectTrans.anchoredPosition.y + scr.rectTrans.rect.height / 2;

        float distFromDest = yDest - selectButtonRectTrans.anchoredPosition.y;

        

        while(distFromDest >= 0.01f) // Se non e' arrivato a destinazione
        {
            selectButtonRectTrans.anchoredPosition = new UnityEngine.Vector2(
                selectButtonRectTrans.anchoredPosition.x,
                Mathf.Lerp(selectButtonRectTrans.anchoredPosition.y, yDest, 0.2f)
                );


            yield return null;
            distFromDest = yDest - selectButtonRectTrans.anchoredPosition.y;
        }

            Debug.Log("Mhanz");
        isAnimatingEnter = false;
    }

    bool isAnimatingExit = false;
    IEnumerator AnimationSelectionButtonExit()
    {
        Debug.Log("Entering");
        if (isAnimatingExit)
            yield break;

        if (isAnimatingEnter)
            yield break;

        isAnimatingExit = true;


        //UnityEngine.Vector2 dest = 
        //    new UnityEngine.Vector2(
        //        scr.rectTrans.anchoredPosition.x,
        //        scr.rectTrans.anchoredPosition.y +
        //        scr.rectTrans.rect.height/2
        //        );

        float yDest = scr.rectTrans.anchoredPosition.y;

        float distFromDest = selectButtonRectTrans.anchoredPosition.y - yDest;



        while (distFromDest >= 0.01f) // Se non e' arrivato a destinazione
        {
            selectButtonRectTrans.anchoredPosition = new UnityEngine.Vector2(
                selectButtonRectTrans.anchoredPosition.x,
                Mathf.Lerp(selectButtonRectTrans.anchoredPosition.y, yDest, 0.2f)
                );

            yield return null;

            distFromDest = selectButtonRectTrans.anchoredPosition.y - yDest;
        }

        Debug.Log("End");
        isAnimatingExit = false;
        selectButton.SetActive(false);
    }

    /// <summary>
    /// Evento di click sul bottone generato in realtime
    /// </summary>
    void PointerDownOnSelectionButton()
    {
        isSelected = true;
        Debug.Log("Card has been chosen!!!!!");
    }

    void PointerDown()
    {
        ////isSelected = true;
        
        //selectButton.SetActive(true);
        //scr.StartCoroutine(AnimationSelectionButton());

        //Debug.Log("Button Showing!!!!");
    }

    void PointerHover()
    {
    }

    void ObjectSelect()
    {
        Debug.Log("Selected");
        scr.StartCoroutine(AnimationSelectionButtonEnter());
    }

    void ObjectDeselect()
    {
        if (isOnSelectableButton)
            return;

        Debug.Log("Deselected");
        scr.StartCoroutine(AnimationSelectionButtonExit());
        isOnSelectableButton = false;
    }
}

public class UIInteractorPowerUpLabel : UIInteractors
{
    // Colore dei numeri che potenziano/depotenziano la statistica
    public static Color upgradedColor { get; private set; }
    public static Color downgradedColor { get; private set; }
    public static Color textColor { get; private set; }

    // Cio' che verra' renderizzato
    private string labelText;

    UIInteractorPowerUpLabel(string text)
    {
        labelText = text;

        events.Add(EventTriggerType.PointerEnter, PointerHover);
    }

    void PointerHover()
    {

    }
}

/// <summary>
/// Tiene la carta in un solo punto, se poi si grabba non appena 
/// rilasciata torna nel punto del placeholder
/// </summary>
public class UIInteractorLockInPlaceholder : UIInteractors
{
    private RectTransform placeholder;
    private CardUIMimic scr;
    public UIInteractorLockInPlaceholder(RectTransform placeholder, CardUIMimic card)
    {
        this.placeholder = placeholder;
        this.scr = card;
        events.Add(EventTriggerType.PointerUp, PointerUp);
        events.Add(EventTriggerType.Drag, OnDrag);
        
    }

    void PointerUp()
    {
        scr.transform.SetParent(placeholder.transform);
        scr.rectTrans.anchoredPosition = UnityEngine.Vector2.zero;
    }

    void OnDrag()
    {
        // Per semplicita me la sbrigo cosi'
        scr.transform.SetParent(UIManagerGabri.GI().canvas.transform);
        scr.rectTrans.transform.position = 
            new UnityEngine.Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

}

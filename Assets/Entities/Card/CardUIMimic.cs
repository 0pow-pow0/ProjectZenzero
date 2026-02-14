using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



/// <summary>
/// Crea un mimicUI con il solo scopo di mostrare delle informazioni all'interno della UI.
/// Sono una copia delle sprite di una determinata carta, senza le informazioni della carta in se',
/// ad eccezione delle informazioni utili per l'UI
/// 
/// Possiamo affidargli UIInteractors per fare in modo che possa interagire in modo unico per ogni instanza.
/// 
/// Necessario per renderizzare piu' volte la stessa carta con diverse interazioni nella UI
/// ma condividendo le informazioni del singleton da cui derivano.
/// 
/// Puoi instanziare un Mimic solo tramite la funzione statica CreateUIMimic()
/// </summary>
public class CardUIMimic : MonoBehaviour
{
    /// <summary>
    /// Firma 
    /// </summary>
    public Card original { get; private set; }

    public CardSprite spr;
    public RectTransform rectTrans;
    public Animator anim;
    public EventTrigger evs; // Serve per richiamare gli eventi
    public List<UIInteractors> UIInters { get; private set; }

    // Component immagini carte
    public Image cardFront;
    public Image cardArtwork;
    public Image cardTopDecoration;
    public Image cardBottomDecoration;

    public GameObject spritesWrapper { get; set; }

    public UIInteractors GetInteractor<T>()
    {
        foreach(UIInteractors uii in UIInters)
        {
            if(uii is T)
            {
                return uii;
            }
        }

        return null;
    }

    private T MustHaveGetComponent<T>() where T : Component
    {
        T component = GetComponent<T>();
        // Se non e' presente, crealo.
        if(component == null)
        {
            Debug.LogWarning("Creating component!");
            component = gameObject.AddComponent<T>();
        }

        return component;
    }

    void Awake()
    {
        UIInters = new List<UIInteractors>();
        spritesWrapper = UtilityShit.FindChildWithName(gameObject, "SpritesWrapper");
        rectTrans = MustHaveGetComponent<RectTransform>();
        anim = MustHaveGetComponent<Animator>();
        evs = MustHaveGetComponent<EventTrigger>();

        evs.triggers.Add(GenerateDelegate(EventTriggerType.PointerDown, OnPointerDownDelegate));
        evs.triggers.Add(GenerateDelegate(EventTriggerType.PointerUp, OnPointerUpDelegate));
        evs.triggers.Add(GenerateDelegate(EventTriggerType.PointerEnter, OnPointerEnterDelegate));
        evs.triggers.Add(GenerateDelegate(EventTriggerType.Select, OnSelectDelegate));
        evs.triggers.Add(GenerateDelegate(EventTriggerType.Deselect, OnDeselectDelegate));
        evs.triggers.Add(GenerateDelegate(EventTriggerType.Drag, OnDragDelegate));

        cardFront = 
            UtilityShit.FindChildWithName(spritesWrapper, "CardFront").GetComponent<Image>();
        cardArtwork = 
            UtilityShit.FindChildWithName(spritesWrapper, "CardArtwork").GetComponent<Image>();
        cardTopDecoration = 
            UtilityShit.FindChildWithName(spritesWrapper, "TopDecoration").GetComponent<Image>();
        cardBottomDecoration = 
            UtilityShit.FindChildWithName(spritesWrapper, "BottomDecoration").GetComponent<Image>();

        Debug.Log("Awake triggered");
    }

    private void SetComponentsToSprite()
    {
        cardFront.sprite = spr.cardFront;
        cardArtwork.sprite = spr.cardArtwork;
        cardTopDecoration.sprite = spr.cardTopDecoration;
        cardBottomDecoration.sprite = spr.cardBottomDecoration;
    }

    public void AddInteractor(UIInteractors newInter)
    {
        if (!UIInters.Contains(newInter))
            UIInters.Add(newInter);
        else
            Debug.Log("Stai provando ad aggiungere un UIInteractor gia' presente");
    }

    private CardUIMimic() { }

    #region DELEGATES
    EventTrigger.Entry GenerateDelegate(EventTriggerType eventType, System.Action<PointerEventData> act)
    {
        EventTrigger.Entry ev = new EventTrigger.Entry();
        ev.eventID = eventType;
        ev.callback.AddListener((data) => { act.Invoke((PointerEventData)data); });
        return ev;
    }

    void OnPointerDownDelegate(PointerEventData ptrEv)
    {
        IterateAllInteractors(EventTriggerType.PointerDown);
    }

    void OnPointerUpDelegate(PointerEventData ptrEv) 
    {
        IterateAllInteractors(EventTriggerType.PointerUp);
    }


    void OnPointerEnterDelegate(PointerEventData ptrEv)
    {
        IterateAllInteractors(EventTriggerType.PointerEnter);
    }

    void OnSelectDelegate(PointerEventData ptrEv)
    {
        IterateAllInteractors(EventTriggerType.Select);
    }

    void OnDeselectDelegate(PointerEventData ptrEv)
    {
        IterateAllInteractors(EventTriggerType.Deselect);
    }

    private void OnDragDelegate(PointerEventData ptrEv)
    {
        IterateAllInteractors(EventTriggerType.Drag);
    }

    #endregion

    /// <summary>
    /// Ritorna lo script con attaccato il GO appena creato
    /// </summary>
    /// <returns>Script con GO attaccato</returns>
    static public CardUIMimic CreateMimic(CardSprite cardSpriteToMimic, Card org, UIInteractors[] newUII = null,
        Transform parent = null)
    {
        Debug.Log(cardSpriteToMimic.cardFront + " " + org.signature + " " + newUII);

        Debug.Log(GameManager.GI().GetPrefab("MimicTemplate") + " " 
            + GameManager.GI().UICardMimicWrapper);

        Transform newParent;
        if (parent == null)
            newParent = GameManager.GI().UICardMimicWrapper.transform;
        else
            newParent = parent;


        GameObject newGOCardUiMimic =
                (GameObject)Instantiate(GameManager.GI().GetPrefab("MimicTemplate"),
                    newParent);

        newGOCardUiMimic.name = "CardMimic";
        newGOCardUiMimic.AddComponent<CardUIMimic>(); 
        CardUIMimic newCardUIMimic = newGOCardUiMimic.GetComponent<CardUIMimic>();



        newCardUIMimic.original = org;

        //spritesWrapper = UtilityShit.FindChildWithName(newGOCardUiMimic, "SpritesWrapper");

        newCardUIMimic.spr = cardSpriteToMimic;
        newCardUIMimic.SetComponentsToSprite();

        //newCardUIMimic.spr.cardFront

        if (newUII != null)
        {
            foreach(UIInteractors ui in newUII)
            {
                newCardUIMimic.UIInters.Add(ui);
            }
        }


        return newCardUIMimic;
    }

    /// <summary>
    /// Prende tutte le "Action" degli UIInteractors associate alla corrispettiva "Interaction"
    /// </summary>x 
    /// <param name=""></param>
    void IterateAllInteractors(EventTriggerType interactionToIterate)
    {
        foreach(UIInteractors uii in UIInters)
        {
            if(uii.events.ContainsKey(interactionToIterate))
            {
                uii.events[interactionToIterate].Invoke();
            }
        }
    }   
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

/// <summary>
/// Serve per sapere se una carta e' contenuta in un array di Card senza conoscere l'eredita'
/// </summary>
public enum CardSignature : int
{
    CardHealthPowerUp = 1,
    CardHealthRegenPowerUp,
    CardDamagePowerUp,
    CardSpeedPowerUp,
    CardFireRatePowerUp,
}



/// <summary>
/// Questa classe contiene tutta l'infrastruttura per: 
/// 1.
/// 2. aggiungergli degli UIInteractors (come: "al passaggio del mouse mostra un dialog", "brilla al passaggio del mouse)
/// 
/// Ma NON contiene
/// 1. Valori da scalare per il livello del powerup
/// 2. Modi per renderizzare la carta
/// 
/// Deve essere:
/// 1. Ogni classe che deriva da card deve essere inizializzata in "CardManager"
/// 
/// 
/// Inizialmente dovevamo avere tante carte, 
/// </summary>
public class Card
{
    

    #region REFERENCES
    public CardSprite activeSprite { get; private set; }

    // Il prefab della carta
    public GameObject parent;
    #endregion



    #region UI INTERACTORS
    
    // Descrizione da mostrare all'UI
    string cardDescription;
    #endregion

    public CardSignature signature { get; private set; }
    public Card(CardSignature sign)   
    {
        signature = sign;
    }

    /// <summary>
    /// Qui instanziamo il prefab dell'arma e inizializziamo tutte le variabili dell classe
    /// </summary>
    /// <param name="maxLvl">In sostanza il numero massimo di potenziamenti che possiede</param>
    /// 
    bool setupHasBeenCalled = false;
   
    protected void Setup(string newObjectName)
    {
        setupHasBeenCalled = true;

        //uii = new List<UIInteractors>();

        parent = new GameObject(newObjectName);
        parent.transform.SetParent(GameManager.GI().CardManagerWrapper.transform);


        // Riferimenti agli Image component della carta
        //cF = UtilityShit.FindChildWithName(parent, "CardFront").GetComponent<Image>();
        //cA = UtilityShit.FindChildWithName(parent, "CardArtwork").GetComponent<Image>();
        //cTD = UtilityShit.FindChildWithName(parent, "TopDecoration").GetComponent<Image>();
        //cBD = UtilityShit.FindChildWithName(parent, "BottomDecoration").GetComponent<Image>();

        //Debug.AssertFormat(rectTrans != null, "Impossibile trovare RectTransform", this);
    }

    public virtual void Init() { /*Setup();*/}

    /// <summary>
    /// Richiamato da CardManager per lanciare errori se mi dimentico di inizializzare robe xD
    /// </summary>
    public void Asserts()
    {
        Debug.AssertFormat(setupHasBeenCalled, "Setup() carta non chiamato ma carta instanziata", this);
    }

    #region SPRITE LOGIC
    /// <summary>
    /// Setta i component Image alla variabile cardSprite associato
    /// </summary>
    public void SwitchActiveSprite(CardSprite newSpr)
    {
        //activeSprite = newSpr;
        //cF.sprite = activeSprite.cardFront;
        //cA.sprite = activeSprite.cardArtwork;
        //cTD.sprite = activeSprite.cardTopDecoration;
        //cBD.sprite = activeSprite.cardBottomDecoration;
    }

    /// <summary>
    /// Disattiva o attiva il rendering della sprite della carta
    /// </summary>
    public void SetSpriteRendering(bool toRender)
    {
        //cF.gameObject.SetActive(toRender);
        //cA.gameObject.SetActive(toRender);
        //cTD.gameObject.SetActive(toRender);
        //cBD.gameObject.SetActive(toRender);
    }

    #endregion
     

    /// <summary>
    /// Cio' che accade quanto la carte leveluppa
    /// </summary>
    public virtual void UpgradeLevelAction()
    {

    }

    /// <summary>
    /// Cio' che la carta modifica appena viene collezionata
    /// </summary>
    public virtual void OnCollectionAction()
    {

    }

    /// <summary>
    /// Cio' che la carta modifica in modo persistente.
    /// Es: dare la possibilita' di schivare; rendere i proiettili di fuoco
    /// </summary>
    public virtual void PersistentAction()
    {

    }

    public virtual CardUIMimic RequestMimic(Transform parent = null)
    {
        Debug.LogWarning("Codice RequestMimic non realizzato!");
        return default(CardUIMimic);
    }

    public virtual void OnDestroy()
    {

    }
}

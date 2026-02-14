using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    # region SINGLETON SHIT
    private static CardManager inst;

    static public CardManager GI() 
    { 
        if(inst == null)
        {
            Debug.LogError("CardManager non instanziato!");
            return null;
        }

        return inst;
    }

    #endregion


    /// <summary>
    /// Tutte le carte si instanziano automaticamente qui dentro.
    /// VEDI: Card.cs #public Card()
    /// </summary>
    private List<Card> allCards;
    public List<Card> poweruppableCards { get; private set; }

    public Card GetRandomPoweruppableCard()
    {
        if(poweruppableCards.Count <= 0)
        {
            return null;
        }

        int randNum = Random.Range(0, poweruppableCards.Count);
        Debug.Log("random num: " + randNum);
        return poweruppableCards[randNum];
    }

    /// <summary>
    /// Serve per comunicare all'array che la carta ha raggiunto il massimo livello
    /// e non puo' essere piu' potenziata.
    /// </summary>
    /// <returns></returns>
    public void RemovePoweruppableCard(Card cardToRemove)
    {
        if(poweruppableCards.Contains(cardToRemove))
        {
            poweruppableCards.Remove(cardToRemove);
            Debug.Log("Carta rimossa");
        }
        else
        {
            Debug.LogWarning("Impossibile trovare carta da rimuovere!");
        }
    }

    private void Awake()
    {
        inst = this;
        allCards = new List<Card>();
        poweruppableCards = new List<Card>();

        // Devo chiamare tutti le classi delle carte per instanziarli :(, non mi convince moltissimo
        // forse agg sbagliato quacche cosa a livello architetturalbalsfl
        allCards.Add(CardHealthPowerUp.GI());
        poweruppableCards.Add(CardHealthPowerUp.GI());
        
        allCards.Add(CardHealthRegenPowerUp.GI());
        poweruppableCards.Add(CardHealthRegenPowerUp.GI());

        allCards.Add(CardDamagePowerUp.GI());
        poweruppableCards.Add(CardDamagePowerUp.GI());

        allCards.Add(CardSpeedPowerUp.GI());
        poweruppableCards.Add(CardSpeedPowerUp.GI());

        allCards.Add(CardFireRatePowerUp.GI());
        poweruppableCards.Add(CardFireRatePowerUp.GI());



        foreach (Card c in allCards)
        {
            c.Init();
            c.Asserts();
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

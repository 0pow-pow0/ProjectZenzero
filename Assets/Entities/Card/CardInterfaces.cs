using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum LEVELS
{
    LEVEL_1 = 0,
    LEVEL_2,
    LEVEL_3,
    LEVEL_4
}


public struct CardSprite
{
    public Sprite cardFront;
    public Sprite cardArtwork;
    public Sprite cardTopDecoration;
    public Sprite cardBottomDecoration;

    /// <summary>
    /// Se uno qualsiasi delle variabili della struct e'
    /// vuoto lancia errore alla console di unity.
    /// </summary>
    public void Assert()
    {
        Debug.Assert(cardFront != null);
        Debug.Assert(cardArtwork != null);
        Debug.Assert(cardTopDecoration != null);
        Debug.Assert(cardBottomDecoration != null);
    }

    public void SetSprites(
        GameObject father,
        Sprite _cardFront,
        Sprite _cardArtwork,
        Sprite _cardTopDecoration,
        Sprite _cardBottomDecoration)
    {
        cardFront = _cardFront;
        cardArtwork = _cardArtwork;
        cardTopDecoration = _cardTopDecoration;
        cardBottomDecoration = _cardBottomDecoration;
    }   

    public void SetSprites(CardSprite newSpr)
    {
        cardFront = newSpr.cardFront;
        cardArtwork = newSpr.cardArtwork;
        cardTopDecoration = newSpr.cardTopDecoration;
        cardBottomDecoration = newSpr.cardBottomDecoration;
    }

    /// <summary>
    /// Logga in console i nomi delle sprite.
    /// </summary>
    public void PrintNames()
    {
        Debug.Log(cardFront.texture.name);
        Debug.Log(cardArtwork.texture.name);
        Debug.Log(cardTopDecoration.texture.name);
        Debug.Log(cardBottomDecoration.texture.name);
    }
}
/// <summary>
/// Interfaccia da associare a carte potenziabili con effetti di potenziamento delle statistiche
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICardUpgradableOneTimeBase
{
    public int level { get; set; }
    public int MAX_LEVEL { get; set; }

    // Descrizione da mostrare all'UI al passaggio del mouse
    string cardPowerUpDescription { get; set; }

    /// <summary>
    /// Contiene le sprite di ogni carta potenziata.
    /// Per ora e' sempre la stessa.
    /// </summary>
    Dictionary<LEVELS, CardSprite> cardLevelSprite { get; set; }


    /// <summary>
    /// Ritorna vero se stiamo SORPASSANDO il gap massimo del livello della carta
    /// </summary>
    /// <returns></returns
    public bool IsAtMaximumLevel()
    {
        return level >= MAX_LEVEL;
    }

    public virtual CardSprite GetCurrentLevelSprite()
    {
        //if(cardLevelSprite.ContainsKey(level))
        //    return cardLevelSprite[level];
        // Costante alla prima sprite visto che ne avremo solo una,
        // abbiamo cambiato direction.
        if(cardLevelSprite.ContainsKey(LEVELS.LEVEL_1))
            return cardLevelSprite[LEVELS.LEVEL_1];

        Debug.LogError("Impossibile trovare lo sprite della carta specificata!");
        return default(CardSprite);
    }

    public virtual CardSprite GetNextLevelCardSprite()
    {
        //if (IsAtMaximumLevel())
        //{
        //    return cardLevelSprite[level];
        //}

        // Cappato sempre al primo livello, leggi altri commenti per spiegazione
        return cardLevelSprite[LEVELS.LEVEL_1];
    }

    public virtual CardSprite GetLevelCardSprite(LEVELS lvl)
    {
        if (cardLevelSprite.ContainsKey(lvl))
        {
            return cardLevelSprite[lvl];
        }

        Debug.LogError("Impossibile trovare carta specificata!");
        return default(CardSprite);
    }

    // Ottieni descrizione del livello x    
    //public virtual string GetCardDescrption

    public virtual void UpgradeLevel()
    {
        if (IsAtMaximumLevel())
        {
            return;
        }

        level += 1;
    }
}

public interface ICardUpgradableOneTime<T> : ICardUpgradableOneTimeBase
{
    // Non si utilizza piu' levels
    // Perche' abbiamo cxambiato idea
    public Dictionary<int, T> cardLevelUpValues { get; set; }
        
        
    public virtual T GetCurrentLevelValues()
    {
        if(cardLevelUpValues.ContainsKey(level))
            return cardLevelUpValues[level];

        Debug.LogError("Impossibile trovare valori livello della carta!");
        return default(T);
    }

}
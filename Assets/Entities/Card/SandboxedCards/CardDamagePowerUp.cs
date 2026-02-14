using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Le carte sono uniche, dunque saranno singleton
/// </summary>
public class CardDamagePowerUp : Card, ICardUpgradableOneTime<int>
{
    #region SINGLETON SHIT
    private static CardDamagePowerUp instance;
    public static CardDamagePowerUp GI()
    {
        if (instance == null)
        {
            //Debug.LogError("CardAttackPowerUp non inizializzato!");
            instance = new CardDamagePowerUp();
        }

        return instance;
    }

    #endregion

    #region ICardUpgradableOneTime Interface
    ICardUpgradableOneTime<int> insideInterface;

    
    public int level { get; set; } = 1;
    
    // rip vecchio sistema di carte
    public int MAX_LEVEL { get; set; }
    public Dictionary<LEVELS, CardSprite> cardLevelSprite { get; set; }
    
    // Si usa una costante ormai
    public Dictionary<int, int> cardLevelUpValues { get; set; }

    public string cardPowerUpDescription { get; set; }

   // private void RequestCardPowerUpDescription(LEVELS)
   //{

    //}
    #endregion


    private CardDamagePowerUp() :
        base(CardSignature.CardDamagePowerUp)
    {
    }   

    /// <summary>
    /// Qui specificheremo i valori di ogni singolo livello e come interagiscono
    /// </summary>
    public override void Init()
    {
        base.Setup("CardDamagePowerUpFather");
        insideInterface = (ICardUpgradableOneTime<int>)this;

        cardLevelSprite = new Dictionary<LEVELS, CardSprite>();
        cardLevelUpValues = new Dictionary<int, int>();

        CardSprite lvl1Sprite = new CardSprite();
        lvl1Sprite.SetSprites(parent, GameManager.GI().GetSprite("CardFrontPowerUpWeapon"),
            GameManager.GI().GetSprite("CardArtworkDamage"),
            GameManager.GI().GetSprite("CardArtworkDamage"),
            GameManager.GI().GetSprite("CardArtworkDamage"));

        //CardSprite lvl2Sprite = new CardSprite();
        //lvl2Sprite.SetSprites(parent, GameManager.GI().GetSprite("CardFrontPowerUpStat"),
        //    GameManager.GI().GetSprite("CardArtworkSword"),
        //    GameManager.GI().GetSprite("CardArtworkSword"),
        //    GameManager.GI().GetSprite("CardArtworkSword"));

        //CardSprite lvl3Sprite = new CardSprite();
        //lvl3Sprite.SetSprites(parent, GameManager.GI().GetSprite("CardFrontPowerUpStat"),
        //    GameManager.GI().GetSprite("CardArtworkSword"),
        //    GameManager.GI().GetSprite("CardArtworkSword"),
        //    GameManager.GI().GetSprite("CardArtworkSword"));

        //MAX_LEVEL = LEVELS.LEVEL_3;
        cardLevelSprite.Add(LEVELS.LEVEL_1, lvl1Sprite);
        //cardLevelSprite.Add(LEVELS.LEVEL_2, lvl2Sprite);
        //cardLevelSprite.Add(LEVELS.LEVEL_3, lvl3Sprite);

        //cardLevelUpValues.Add(LEVELS.LEVEL_1, 5);
        //cardLevelUpValues.Add(LEVELS.LEVEL_2, 15);
        //cardLevelUpValues.Add(LEVELS.LEVEL_3, 30);

        // Settiamo powerups
        MAX_LEVEL = 100;
        int powerUpFactor = 10;
        cardLevelUpValues[1] = 0;
        for(int i = level+1; i <= MAX_LEVEL; i++)
        {
            cardLevelUpValues.Add(i, powerUpFactor * (i - 1));
        }


        SwitchActiveSprite(cardLevelSprite[LEVELS.LEVEL_1]);

        //Debug.Log("initted");
    }
    public override void UpgradeLevelAction()
    {
        if (insideInterface.IsAtMaximumLevel())
        {
            Debug.LogWarning("Stai provando ad aumentare il livello di una carta al livello massimo!");
            return;
        }



        // Rimuovi upgrade corrente
        GameManager.GI().plrScr.weaponScr.projectileDamage -= cardLevelUpValues[level];
        level++;
        GameManager.GI().plrScr.weaponScr.projectileDamage += cardLevelUpValues[level];

        if (insideInterface.IsAtMaximumLevel())
        {
            CardManager.GI().RemovePoweruppableCard(this);
        }
    }



    public override void OnDestroy()
    {
        // Rimuovi ultimo potenziamento messo
        GameManager.GI().plrScr.weaponScr.projectileDamage -= cardLevelUpValues[level];
    }

    public override void OnCollectionAction()
    {
        GameManager.GI().plrScr.weaponScr.projectileDamage += cardLevelUpValues[1];
    }

    public override void PersistentAction()
    {

    }
        
    public override CardUIMimic RequestMimic(Transform parent = null)
    {
        //if(requestedLevel >= MAX_LEVEL)
        //{
        //    Debug.LogError("Hai richieto un livello di carta troppo alto!");
        //    return null;
        //}

        CardUIMimic res =
            CardUIMimic.CreateMimic(insideInterface.GetCurrentLevelSprite(), this, parent:parent);
        return res;
    }

}
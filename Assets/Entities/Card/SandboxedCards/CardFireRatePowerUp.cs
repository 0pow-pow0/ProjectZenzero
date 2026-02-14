using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Le carte sono uniche, dunque saranno singleton
/// </summary>
public class CardFireRatePowerUp : Card, ICardUpgradableOneTime<float>
{
    #region SINGLETON SHIT
    private static CardFireRatePowerUp instance;
    public static CardFireRatePowerUp GI()
    {
        if (instance == null)
        {
            //Debug.LogError("CardHealthPowerUp non inizializzato!");
            instance = new CardFireRatePowerUp();
        }

        return instance;
    }

    #endregion

    #region ICardUpgradableOneTime Interface
    ICardUpgradableOneTime<float> insideInterface;

    public int level { get; set; } = 1;
    public int MAX_LEVEL { get; set; }
    public Dictionary<LEVELS, CardSprite> cardLevelSprite { get; set; }
    public Dictionary<int, float> cardLevelUpValues { get; set; }

    public string cardPowerUpDescription { get; set; }
    #endregion




    private CardFireRatePowerUp() :
        base(CardSignature.CardFireRatePowerUp)
    {
    }

    /// <summary>
    /// Qui specificheremo i valori di ogni singolo livello e come interagiscono
    /// </summary>
    public override void Init()
    {
        base.Setup("CardFireRatePowerUpFather");
        insideInterface = (ICardUpgradableOneTime<float>)this;

        cardLevelSprite = new Dictionary<LEVELS, CardSprite>();
        cardLevelUpValues = new Dictionary<int, float>();

        CardSprite lvl1Sprite = new CardSprite();
        lvl1Sprite.SetSprites(parent, GameManager.GI().GetSprite("CardFrontPowerUpStat"),
            GameManager.GI().GetSprite("CardArtworkAttackSpeed"),
            GameManager.GI().GetSprite("CardArtworkAttackSpeed"),
            GameManager.GI().GetSprite("CardArtworkAttackSpeed"));

        //CardSprite lvl2Sprite = new CardSprite();
        //lvl2Sprite.SetSprites(parent, GameManager.GI().GetSprite("CardFrontPowerUpStat"),
        //    GameManager.GI().GetSprite("CardArtworkHeartYellow"),
        //    GameManager.GI().GetSprite("CardArtworkHeartYellow"),
        //    GameManager.GI().GetSprite("CardArtworkHeartYellow"));

        //CardSprite lvl3Sprite = new CardSprite();
        //lvl3Sprite.SetSprites(parent, GameManager.GI().GetSprite("CardFrontPowerUpStat"),
        //    GameManager.GI().GetSprite("CardArtworkHeartViolet"),
        //    GameManager.GI().GetSprite("CardArtworkHeartViolet"),
        //    GameManager.GI().GetSprite("CardArtworkHeartViolet"));

        cardLevelSprite.Add(LEVELS.LEVEL_1, lvl1Sprite);
        //cardLevelSprite.Add(LEVELS.LEVEL_2, lvl2Sprite);
        //cardLevelSprite.Add(LEVELS.LEVEL_3, lvl3Sprite);

        MAX_LEVEL = 100;
        float powerUpFactor = 0.2f;
        cardLevelUpValues[1] = 0;
        for (int i = level + 1; i <= MAX_LEVEL; i++)
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
        GameManager.GI().plrScr.weaponScr.fireRate -= cardLevelUpValues[level];
        level++;
        GameManager.GI().plrScr.weaponScr.fireRate -= cardLevelUpValues[level];

        if (insideInterface.IsAtMaximumLevel())
        {
            CardManager.GI().RemovePoweruppableCard(this);
        }
    }



    public override void OnDestroy()
    {
        // Rimuovi ultimo potenziamento messo
        GameManager.GI().plrScr.weaponScr.fireRate -= cardLevelUpValues[level];
    }

    public override void OnCollectionAction()
    {
        GameManager.GI().plrScr.weaponScr.fireRate -= cardLevelUpValues[level];
    }

    public override void PersistentAction()
    {

    }

    public override CardUIMimic RequestMimic(Transform parent = null)
    {


        CardUIMimic res =
            CardUIMimic.CreateMimic(insideInterface.GetCurrentLevelSprite(), this , parent: parent);
        return res;
    }

}
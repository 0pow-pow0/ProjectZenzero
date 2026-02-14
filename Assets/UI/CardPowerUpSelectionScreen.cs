using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardPowerUpsSelectionScreen : MonoBehaviour
{
    //public static Vector2 cardScale = new Vector2(0.33f, 0.33f);
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject[] placeholders;

    /// <summary>
    /// Le carte che il player potra' potenziare.
    /// Saranno renderizzate nella posizione dei placeHolder
    /// 
    /// Saranno selezionate randomicamente da questa classe.
    /// 
    /// Sono presenti esclusivamente le carte che non sono al livello massimo
    /// </summary>
    [NonSerialized] private List<CardUIMimic> cardPoweruppable;


    /// <summary>
    /// Serve per verificare se la lista contiene una Card di tipo T
    /// </summary>
    private bool CheckSignature(CardSignature sign)
    {
        foreach(CardUIMimic cum in cardPoweruppable)
        {
            if(cum.original.signature == sign)
            {
                return true;
            }
        }
        return false;
    }    

    public void RenderScreen()
    {
        panel.SetActive(true);
        GameManager.GI().PauseGame();
        List<Card> avaiableCard = new List<Card>(CardManager.GI().poweruppableCards);
        // Prendi la posizione dei placeholder
        for (int i = 0; i < placeholders.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, avaiableCard.Count);
            Debug.Log("Random index: " + randomIndex + " Avaiable cards: " + avaiableCard.Count);
            Card cardToSelect = avaiableCard[randomIndex];
            avaiableCard.Remove(cardToSelect);

            CardUIMimic mimic = cardToSelect.RequestMimic(placeholders[i].transform);

            mimic.rectTrans.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            mimic.rectTrans.anchoredPosition = Vector2.zero;
            mimic.anim.updateMode = AnimatorUpdateMode.UnscaledTime;
            //mimic.rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal);
            mimic.AddInteractor(
                new UIInteractorCardPowerUpSelection(mimic, placeholders[i].transform.position));

            cardPoweruppable.Add(mimic);
        }
        
    }

    public void UnrenderScreen()
    {
        // Distruggi mimic
        foreach(CardUIMimic mim in cardPoweruppable)
        {
            Destroy(mim.gameObject);
        }



        GameManager.GI().UnpauseGame();
        cardPoweruppable.Clear();
        panel.SetActive(false);
    }

    // Start is called before the first frame update
    void Awake()
    {
        cardPoweruppable = new List<CardUIMimic>();

        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!panel.activeInHierarchy)
            return;


        foreach(CardUIMimic card in cardPoweruppable)
        {
            // Prendiamo il UII che ci serve in questo caso...
            UIInteractorCardPowerUpSelection uiiPowerUp = 
                (UIInteractorCardPowerUpSelection)
                card.GetInteractor<UIInteractorCardPowerUpSelection>();
            if(uiiPowerUp != null)
            {
                // ... per verificare se e' stato selezionato
                if(uiiPowerUp.isSelected)
                {
                    ICardUpgradableOneTimeBase upgradeInterface = 
                        (ICardUpgradableOneTimeBase)card.original;

                    card.original.UpgradeLevelAction();
                    // DeINIT PROCESS
                    Debug.Log("Upgraded!");
                    UnrenderScreen();
                    break;
                }
            }
        }
    }
}

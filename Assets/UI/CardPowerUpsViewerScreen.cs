using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardPowerUpsViewerScreen : MonoBehaviour
{
    /// <summary>
    /// Rappresentano la struttura di alcuni GameObject di questa parte della ui
    /// </summary>
    public class CardPlaceholder 
    {
        public GameObject gameObject;
        public CardUIMimic mimic;
        // Contiene il numero di carte raccolte
        // Nonche' il livello della carta
        public TextMeshProUGUI counterText;      
        
        /// <summary>
        /// Contiene il testo che conterra' i numeri di 
        /// statistiche bonus ottenuti rispetto ai valori di base del player
        /// </summary>
        public TextMeshProUGUI modifierText;
    }
    Dictionary<string, CardPlaceholder> cardPlaceholders;


    public class CardPowerUpPlaceholder
    {
        public GameObject gameObject;
        public CardUIMimic mimic; 
    }
    CardPowerUpPlaceholder[] cardPowerUpPlaceholder;

    //---------- Riferimenti
    public GameObject cardsPanel;
    public GameObject cardsButton;
    public GameObject resumeButton;

    #region BOTTONI
    public void PressCardsButton()
    {
        Debug.Log("Pressed");
        StartCoroutine(UIManagerGabri.GI().PressThen(cardsButton, () =>
            {
                cardsPanel.SetActive(true);
                cardsButton.SetActive(false);
                GameManager.GI().PauseGame();
                RefreshValues();
            }));
    }

    public void PressResumeButton()
    {
        StartCoroutine(UIManagerGabri.GI().PressThen(resumeButton, () =>
        {
            cardsPanel.SetActive(false);
            UIManagerGabri.GI().ExitUIElement();
            cardsButton.SetActive(true);

            foreach (CardPlaceholder cardPlaceholder in cardPlaceholders.Values)
            {
                Destroy(cardPlaceholder.mimic.gameObject);
            }

            GameManager.GI().UnpauseGame();
        }));
    }

    #endregion

    //[NonSerialized] public CardUIMimic[] collectedCards;

    

    private void RefreshValues()
    {
        //cardPlaceholders[]
        Debug.Log(CardHealthPowerUp.GI().MAX_LEVEL);
        CardPlaceholder placeholderHealth = cardPlaceholders["Health"];
        CardPlaceholder placeholderHealthRegen = cardPlaceholders["HealthRegen"];
        CardPlaceholder placeholderSpeed = cardPlaceholders["Speed"];
        CardPlaceholder placeholderAttackSpeed = cardPlaceholders["AttackSpeed"];
        CardPlaceholder placeholderDamage = cardPlaceholders["Damage"];
        Vector3 cardScale = new Vector3(0.1f, 0.1f, 0.1f);
        
        placeholderHealth.mimic = 
            CardHealthPowerUp.GI().RequestMimic(placeholderHealth.gameObject.transform);

        placeholderHealthRegen.mimic = 
            CardHealthRegenPowerUp.GI().RequestMimic(placeholderHealthRegen.gameObject.transform);

        placeholderSpeed.mimic = 
            CardSpeedPowerUp.GI().RequestMimic(placeholderSpeed.gameObject.transform);

        placeholderDamage.mimic = 
            CardDamagePowerUp.GI().RequestMimic(placeholderDamage.gameObject.transform);

        placeholderAttackSpeed.mimic = 
            CardFireRatePowerUp.GI().RequestMimic(placeholderAttackSpeed.gameObject.transform);

        foreach(CardPlaceholder crdPlc in cardPlaceholders.Values)
        {
            crdPlc.mimic.rectTrans.anchoredPosition = Vector3.zero;
            crdPlc.mimic.anim.updateMode = AnimatorUpdateMode.UnscaledTime;
            crdPlc.mimic.AddInteractor(
                new UIInteractorLockInPlaceholder(crdPlc.gameObject.GetComponent<RectTransform>(),
                crdPlc.mimic));
        }


        ///------------------------------ COUNTER CARTE 


        placeholderHealth.counterText.text = "x" + CardHealthPowerUp.GI().level;
        placeholderHealthRegen.counterText.text = "x";
        placeholderSpeed.counterText.text = "x";
        placeholderDamage.counterText.text = "x";
        placeholderAttackSpeed.counterText.text = "x";





        ///------------------------------ STATISTICHE PLAYER


        placeholderHealth.modifierText.text = 
            $"{GameManager.GI().plrScr.maxHP:G3}";

        placeholderHealthRegen.modifierText.text = 
            $"{GameManager.GI().plrScr.healthRegen:G3}";

        placeholderSpeed.modifierText.text = 
            $"{GameManager.GI().plrScr.speed:G3}";

        placeholderDamage.modifierText.text = 
            $"{GameManager.GI().plrScr.weaponScr.projectileDamage:G3}";

        placeholderAttackSpeed.modifierText.text = 
            $"{GameManager.GI().plrScr.weaponScr.fireRate:G3}";


        //cardPowerUpPlaceholder[0].mimic;

    }


    // Start is called before the first frame update
    void Awake()
    {
        ///<summary>
        ///Utilizzo gli nomi stessi dei gameObject per fillare il dizionario perche' mi va :).
        ///</summary>
        GameObject _cardPlaceholdersParent = UtilityShit.FindChildWithName(gameObject, "Placeholders");
        cardPlaceholders = new Dictionary<string, CardPlaceholder>();
        // Setta cardPlaceholders dall'albero di unity
        foreach(Transform trf in _cardPlaceholdersParent.transform)
        {
            cardPlaceholders[trf.name] = new CardPlaceholder();
            CardPlaceholder crdplc = cardPlaceholders[trf.name]; 
            crdplc.gameObject = trf.gameObject;
            crdplc.counterText = crdplc.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        }

        GameObject _cardModifiers = UtilityShit.FindChildWithName(gameObject, "Stat Panel");
        foreach(Transform trf in _cardModifiers.transform)
        {
            if(cardPlaceholders.ContainsKey(trf.name))
            {
                cardPlaceholders[trf.name].modifierText = trf.gameObject.GetComponentInChildren<TextMeshProUGUI>();
                //Debug.Log(cardPlaceholders[trf.name].modifierText);
            }
        }
        
        GameObject _cardPowerUpPlaceholdersParent = UtilityShit.
            FindChildWithName(gameObject, "PowerUpPlaceholders");
        cardPowerUpPlaceholder = new CardPowerUpPlaceholder[_cardPowerUpPlaceholdersParent.transform.childCount];
            //Debug.Log(_cardPowerUpPlaceholdersParent.transform.childCount);
        
        int i = 0;
        foreach(Transform trf in _cardPowerUpPlaceholdersParent.transform)
        {
            cardPowerUpPlaceholder[i] = new CardPowerUpPlaceholder();
            cardPowerUpPlaceholder[i].gameObject = trf.gameObject;
            i++;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

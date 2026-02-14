using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Weapons;

/// <summary>
/// Utilizza tag per la ricerca dei suoi child
/// </summary>
public class Player : MonoBehaviour
{
    #region REFERENCES
    [NonSerialized] public Rigidbody rb;
    [SerializeField] public Camera cam;
    [NonSerialized] public Animator anim;
    [NonSerialized] public Material[] materials;
    public GameObject projectileSpawnPoint { get; private set; }    

    [NonSerialized] public GameObject weaponPivot;
    

    // Per accedere alla variabile 

    /// Dovrei creare un weapon manager
    public RangedWeapon weaponScr;

    #endregion

    List<Card> deck;


    #region STATS CONST

    /// Non relative al gameplay
    private float ROTATION_SPEED = 1.5f;


    //---------- GAMEPLAY SHIT
    private int START_MAXIMUM_HP = 100;
    private float START_HEALTH_REGEN = 5f; // Al secondo

    private float START_SPEED = 6;

    // Relativo all'arma
    private float START_FIRE_RATE = 1f; // Proiettili sparati al secondo
    private int START_DAMAGE = 10;

    private float START_EXP = 0;

    #endregion

    //---------------------------------------------------------------------------------

    #region STATS REALTIME

    [NonSerialized] public int maxHP;
    public int HP {  get; private set; }
    public void AddHP(int toAdd)
    {
        if (HP + toAdd <= 0)
        {
            HP = 0;
        }
        else if (HP + toAdd > maxHP)
        {
            HP = maxHP;
        }
        else

        {
            HP += toAdd;
        }

        UIManagerGabri.GI().inGameUIPanel.SetPlayerHPSliderValue(HP, maxHP);
    }

    /// <summary>
    /// Rimuovi HP al player.
    /// Se e' invincibile questa funzione non ha effetto
    /// </summary>
    /// <param name="amount">Ammontare del danno</param>
    public void DamageHP(int amount)
    {
        if (isInvincible)
            return;

        if (amount <= 0)
            Debug.LogWarning("Stai provando a danneggiare il player con un valore negativo o nullo!");

        HP -= amount;

        UIManagerGabri.GI().inGameUIPanel.SetPlayerHPSliderValue(HP, maxHP);
    }

    [NonSerialized] public float healthRegen;
    private Stopwatch healthRegenStopwatch;

    [NonSerialized] public float speed;

    // Livello massimo raggiungibile per architettura del gioco
    public int MAX_LEVEL = 150;
    

    ///------------------------------------ LIVELLI
    [NonSerialized] public int level = 1; 
    // inserendo il livello si ottiene l'exp necessaria per raggiungerlo
    [NonSerialized] private Dictionary<int, float> levelExpRequirements;
    [NonSerialized] public float exp = 9;

    float GetNextLevelRequiredExp()
    {
        if(level + 1 >= MAX_LEVEL)
        {
            return 0f;
        }
        return levelExpRequirements[level + 1];
    }
    float GetPreaviousLevelRequiredExp()
    {
        if (level - 1 < 1)
        {
            return 0f;
        }

        return levelExpRequirements[level - 1];
    }


    public void AddEXP(float toAdd)
    {
        if (toAdd <= 0)
        {
            Debug.LogError("Stai provando ad aggiungere un valore di EXP negativo o nullo! " + toAdd);
            return;
        }
        else
        {
            exp += toAdd;
        }

        
        
        if(exp >= GetNextLevelRequiredExp())
        {
            LevelUp();
        }
        Debug.Log("exp: " + exp + " nextLvlExp" + GetNextLevelRequiredExp());
        UIManagerGabri.GI().inGameUIPanel.SetPlayerEXPSliderValue
            (exp, GetNextLevelRequiredExp(), GetPreaviousLevelRequiredExp());

    }

    void LevelUp()
    {
        if(level + 1 >= MAX_LEVEL)
        {
            return;
        }
        level++;
        // Triggera level up schermata
        Debug.Log("SKIBIDI");
        UIManagerGabri.GI().cardPowerUpsSelectionScreen.RenderScreen();
    }


    [NonSerialized] public bool isInvincible = false;

    void InitStatsToStartValues()
    {
        maxHP = START_MAXIMUM_HP;
        HP = maxHP;
        healthRegen = START_HEALTH_REGEN;

        speed = START_SPEED;


        weaponScr.projectileDamage = START_DAMAGE;
        weaponScr.fireRate = START_FIRE_RATE;

        exp = 9;
    }

    #endregion


    /// <summary>
    /// Navigates the unity tree and gets necessary components
    /// </summary>
    private void InitReferences()
    {
        levelExpRequirements = new Dictionary<int, float>();
        // Questi sono gli EXP cap attuali 
        levelExpRequirements[1] = 10f;
        for(int i = 2; i < MAX_LEVEL; i++)
        {
            levelExpRequirements[i] = (i-1) * 10;
        }
        healthRegenStopwatch = Stopwatch.CreateStopwatch();

        rb = gameObject.GetComponent<Rigidbody>();

        anim = gameObject.GetComponent<Animator>();
        weaponPivot = UtilityShit.FindChildWithName(gameObject, "WeaponPivot").gameObject;
        materials = GetComponentInChildren<MeshRenderer>().materials;
        projectileSpawnPoint = UtilityShit.
                FindChildWithName(gameObject, "ProjectileSpawnPointPlayerBased");

        //weaponScr = new Spada(this, weaponPivot);
        //Debug.Log(weaponScr);
        //weaponScr.Init();
        //weapon = ((Spada)weaponScr).obj;
        //Debug.Log(weapon);
        anim.Rebind();  // Importante

        //Debug.Log("Base HPs: " + maximumHP);
        deck = new List<Card>();
        weaponScr = new Cannone(this);
        weaponScr.Init();



        //deck.Add(CardHealthPowerUp.GI());

        //CardUIMimic mimicRef = CardUIMimic.CreateMimic(deck[0].activeSprite).GetComponent<CardUIMimic>();
        //mimicRef.AddInteractor(new UIInteractorCardPowerUpSelection(mimicRef, 
        //    new Vector2(100, 100)));

        //Debug.Log("Card collected!");
        //deck[0].OnCollectionAction();
        //Debug.Log("After Hps: " + maximumHP);
        Debug.Assert(rb != null, "Rigidbody component non trovato!", this);
        Debug.Assert(cam != null, "Camera component non trovato!", this);
        Debug.Assert(anim != null, "Animator component non trovato!", this);
    }

    private void InitUI()
    {
        UIManagerGabri.GI().inGameUIPanel.SetPlayerHPSliderValue(HP, maxHP);
        UIManagerGabri.GI().inGameUIPanel.SetPlayerEXPSliderValue(exp, GetNextLevelRequiredExp(), GetPreaviousLevelRequiredExp());
    }

    /// Regione dedicata alla logica associata all'array: "Card deck"
    #region DECK LOGIC


    #endregion
    /// Qui metto tutti i tastini che servono per eseguire un azione
    /// Tutti possono accedere a queste funzioni per verificare se il tasto movimento sta
    /// venendo premuto ad esempiolo.
    #region INPUT

    public bool InputWalking()
    {
        return Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D)
            && !UIManagerGabri.GI().isMouseHoveringUI;
    }   
    public bool InputAttack()
    {
        return Input.GetKey(KeyCode.Mouse0)
            && !UIManagerGabri.GI().isMouseHoveringUI;
    }
    public void RotatePlayer()
    {
        Ray mouseToTerrain = cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        bool mouseHitOnTerrain = 
            Physics.Raycast
            (mouseToTerrain, out hit, Mathf.Infinity, LayerMask.GetMask("TerrenoMesh", "mouseRayCastPlane"));
        Debug.DrawRay(mouseToTerrain.origin, mouseToTerrain.direction * 1000, Color.yellow);
        GameManager.GI().debugPoint.transform.position = hit.point;
        if (!mouseHitOnTerrain) 
        { 
            //Debug.LogError("IL MOUSE NON HA HITTATO");
            return;
        }   
        
        GameManager.GI().lastHitPositionMouseXZ = new Vector2(hit.point.x, hit.point.z);
        GameManager.GI().lastHitPositionMouse = new Vector3(hit.point.x, hit.point.y, hit.point.z);

        // Ci interessa solo la rotazione attorno all'asse Y
        Quaternion playerPosToMouse = 
            Quaternion.Euler(0,
                Quaternion.LookRotation(hit.point - transform.position).eulerAngles.y, 
                0);

        Quaternion rot = Quaternion.RotateTowards(
            transform.localRotation,
            playerPosToMouse,
            ROTATION_SPEED
            );
        
        transform.localRotation = rot;
    }
    
    #endregion
    
    
    private void Awake ()
    {
        InitReferences();


        InitStatsToStartValues();
        InitUI();
    }

    private void Update()
    {
        if(HP <= 0)
        {
            UtilityShit.Log("MORTO!!!!!!!!", Color.red);
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddEXP(1f); 
        }

        if(healthRegenStopwatch.elapsedTime >= 1f)
        {
            // TODO QUESTA COSA NON CONSERVA LE CIFRE DECIMALI
            AddHP((int)healthRegen);
        }
        //if(Input.GetKeyDown(KeyCode.Q))
        //{
        //    Debug.Log("Level Upping Card!");
        //    Debug.Log("Max HPs Before: " + maxHP);
        //    //deck[0].UpgradeLevelAction();
        //    Debug.Log("Max HPs After: " + maxHP);
        //}

        //if(Input.GetKeyDown(KeyCode.A))
        //{
        //    Debug.Log("Removing card upgrades!");
        //    Debug.Log("Max HPs Before: " + maxHP);

        //    Debug.Log("Max HPs After: " + maxHP);
        //}

        
        weaponScr.Update();
    }
}

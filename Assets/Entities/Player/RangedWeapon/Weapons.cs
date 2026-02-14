using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;


namespace Weapons
{
    /// <summary>
    /// I proiettili vengo considerati come fossero un'arma a parte, 
    /// chiaramente pero' tramite l'enumerazione sappiamo che ci stiamo 
    /// riferendo ad un proiettile e non ad un'arma
    /// </summary>
    public enum WeaponType  
    {
        ARCO = 1,
        ARCO_PROIETTILE,
        BASTONE_MAGICO,
        BASTONE_MAGICO_PROIETTILE,
        SPADA,
        CANNONE,
    };

    public enum WeaponAnimations
    {
        ATTACK
    }

    /// <summary>
    /// Da questa classe erediteranno tutte le armi.
    /// Questa classe semplicemente per triggerare le funzioni delle armi SENZA
    /// sapere quale arma si sta impugnando direttamente.
    /// </summary>
    public abstract class Weapon
    {
        public WeaponType type;
        //public Type childType;
        public Player plrScr;
        public GameObject weapon; 
            
        /// <summary>
        /// Mappo tutte le animazioni possibili dell'arma dentro questo dizionario, 
        /// cosi' poi attraverso gli animation events potro' sapere lo stato di ogni animazione.
        /// Utilie
        /// </summary>
        public Dictionary<WeaponAnimations, Animation> animationVariables;

        protected Weapon(WeaponType t, Player plrScr)
        {
            type = t;
            this.plrScr = plrScr;
        }

        public abstract void Init(); 
        public abstract void Update();    
        /// <summary>
        /// Utile per sapere se e' l'arma e' disponibile per fare un'altro attacco.
        /// Serve per sapere se ha senso per la PlayerFSM switchare allo stato di attacco.
        /// </summary>
        /// <returns>Ritorna vero se e' possibile lanciare un'altro attacco.</returns>
        public abstract bool CanAttack();

        /// <summary>
        /// Chiamare questa funzione senza aver verificato il risultato di "CanAttack()"
        /// bypassa i check e potrebbe creare risultati inaspettati.
        /// </summary>
        public abstract bool UpdateAttack();

        public abstract void InitAttack();
        public abstract void ResetAttackAnimations();

        public abstract void ExitAttack();

        public abstract IProjectile CreateProjectile();


    }

    //public class MeleeWeapon : Weapon
    //{
    //    public int damage;
    //    public float attackSpeed;
    //    // CollidersInfo

    //    protected MeleeWeapon(WeaponType t, Player plrScr) :
    //        base(t, plrScr)
    //    {

    //    }
    //    public override void InitAttack()
    //    {

    //    }

    //    public override void ResetAttackAnimations()
    //    {
            
    //    }

    //    public override bool UpdateAttack()
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public override void ExitAttack()
    //    {

    //    }

    //    public override global::System.Boolean CanAttack()
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public override void Init()
    //    {
    //        Debug.Log("I want a cock!");
    //    }

    //    public override void Update()
    //    {

    //    }
    //}

    public class RangedWeapon : Weapon
    {
        protected RangedWeapon(WeaponType t, Player plrScr) :
            base(t, plrScr)
        {
            fireStopwatch = Stopwatch.CreateStopwatch();
        }

        public Stopwatch fireStopwatch;
        // Velocita' di spawn dei proiettili
        public float fireRate; // In secondi

        public int projectileDamage = 10;
        public float projectileSpeed;
        // Numero di proiettili che vengono sparati
        public int projectilesPerShoot;
        // Massima strada percorribile dai proiettili
        public float projectileTravelDistance;


        // TODO Da chiarire
        public Vector3[] projectilesSpawnPattern;

        // Raggio della circonferenza utilizzata per posizionare i proiettili 
        private float projectileCircumferenceRay;

        public Transform projectileSpawnPoint { get; set; }


        // Utilizzata per rispettare la velocita' di fuoco dell'arma
        protected bool canFire;

        /// <summary>
        /// Calcola il punto di origine da cui far partire i proiettili
        /// </summary>
        public void CalculateProjectilesPivot()
        {
            Vector3 plrForward = plrScr.transform.forward;
            
        }

        public void AddProjectiles()
        {

        }

        public override bool UpdateAttack()
        {
            throw new System.NotImplementedException();
        }

        public override void ExitAttack()
        {
            
        }

        public override global::System.Boolean CanAttack()
        {
            throw new System.NotImplementedException();
        }

        public override void Init()
        {
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            // Se il tempo passato dall'ultimo e' maggiore della velocita' di attacco
            // puoi sparare
            if (!canFire)
            {
                if (fireStopwatch.elapsedTime >= fireRate)
                {
                    canFire = true;
                }
            }
        }

        public override void InitAttack()
        {
            // In modo che possa sparare subito, SOLO SE sta rispettando il tempismo 
            // di fireRate
            if(fireStopwatch.elapsedTime >= fireRate)
            {
                canFire = true;            
                fireStopwatch.Reset();
            }
            
        }

        public override void ResetAttackAnimations()
        {

        }

        public override IProjectile CreateProjectile()
        { 
            throw new System.NotImplementedException();
        }
    }

    #region CODICE SPECIALIZZATO ARMI

    /// <summary>
    /// Non e' monobehaviour perche' non serve letteralmente e mi e' piu' utile che erediti altro, 
    /// creare un instanza di questa classe crea automaticamente un prefab nella hierarchy. 
    /// 
    /// Si instanzia per letteralmente nessun motivo, voglio solo fare pratica con le robe che non conosco.
    /// </summary>
    //public class Spada : MeleeWeapon
    //{
    //    WeaponAnimations currentWeaponAnimationIndex;
    //    BoxCollider[] colls; 

    //    public GameObject obj;

    //    // Statistiche di base dell'arma
    //    public const int BASE_DAMAGE = 10;
    //    public const float BASE_ATTACK_SPEED = 10;
    //    public BoxCollider BASE_COLLIDERS { private set; get; }

    //    // Ha rilasciato il tasto di input? 
    //    private bool _isPressingInput;

    //    /// <summary>
    //    /// </summary>
    //    /// <param name="scr"></param>
    //    /// <param name="index">Indice dell'oggetto instanziato per il Transform del Parent</param>
    //    public Spada(Player scr, GameObject newParent) :
    //        base(WeaponType.SPADA, scr)
    //    {
    //        childType = typeof(Spada);

    //        plrScr = scr;
    //    }

    //    void Awake()
    //    {
    //        Init();
    //        GameObject allah = GameObject.Instantiate(Resources.Load("Prefabs/Spada", typeof(GameObject))) as GameObject;
    //        Debug.Log(allah.transform.position);
    //        allah.transform.SetParent(GameManager.GI().plrScr.transform, false);
            
            

    //        // Informazione utile per l'animator
    //        allah.name = "Spada";

    //        obj = allah;


    //        GameObject wrapperCollider = UtilityShit.FindChildWithName(obj, "CollidersWrapper");
    //        colls = wrapperCollider.GetComponentsInChildren<BoxCollider>();
    //        Debug.Log(colls);
    //        animationVariables = new Dictionary<WeaponAnimations, Animation>();
    //        animationVariables.Add(WeaponAnimations.ATTACK, new Animation("SwordAttack"));
    //    }

    //    /// <summary>
    //    /// Imposta i valori dell'arma ai suoi valori iniziali
    //    /// </summary>
    //    public override void Init()
    //    {
    //        damage = BASE_DAMAGE;
    //        attackSpeed = BASE_ATTACK_SPEED;
    //        SetColliders(false);
    //    }
    //    public override void Update()
    //    {

    //    }

    //    void SetColliders(bool s)
    //    {
    //        foreach(BoxCollider c in colls)
    //        {
    //            c.gameObject.SetActive(s);
    //        }
    //    }

    //    public override void InitAttack()
    //    {
    //        //plrScr.anim.SetBool(plrScr.weaponScr.animationVariables[WeaponAnimations.ATTACK_CHAIN_0].animationName, true);
    //        //currentWeaponAnimationIndex = WeaponAnimations.ATTACK_CHAIN_0;
                
    //        plrScr.anim.SetBool(animationVariables[WeaponAnimations.ATTACK].animationName, true);
    //        _isPressingInput = true;
            
    //        // Abilita colliders
    //        SetColliders(true);
    //    }
            
    //    /// <summary>
    //    /// Utilizzato dalla PlayerFSM
    //    /// </summary>
    //    //public override void UpdateAttack()
    //    //{
    //    //    // Se il player continua a tenere premuto il tasto di input
    //    //    if (plrScr.InputAttack() &&
    //    //        // Massimo numero della catena raggiungibile 
    //    //        currentWeaponAnimationIndex + 1 > WeaponAnimations.ATTACK_CHAIN_2)
    //    //    {
    //    //        // Se possiede un'animazione per la seconda parte della catena
    //    //        if (plrScr.weaponScr.animationVariables.ContainsKey(currentWeaponAnimationIndex + 1))
    //    //        {
    //    //            plrScr.anim.SetBool(
    //    //                plrScr.weaponScr.animationVariables[currentWeaponAnimationIndex].animationName,
    //    //                false);

    //    //            currentWeaponAnimationIndex++;
    //    //            plrScr.anim.SetBool(
    //    //                plrScr.weaponScr.animationVariables[currentWeaponAnimationIndex].animationName,
    //    //                true);
    //    //        }
    //    //    }


    //    //}


    //    public override bool UpdateAttack()
    //    {
    //        Debug.Log(animationVariables[WeaponAnimations.ATTACK].state);
    //        // Se il player rilascia il tasto di input... 
    //        if(!plrScr.InputAttack())
    //        {
    //            _isPressingInput = false;
    //        }


    //        // ...e l'animazione in corso e' terminata...
    //        if (!_isPressingInput &&
    //            animationVariables[WeaponAnimations.ATTACK].state == AnimationStates.END_ANIMATION)
    //        {
    //            // ...possiamo interrompere la routine di attacco
    //            return false;
    //        }
    //        return true;
    //    }

    //    public override void ExitAttack()
    //    {
    //        SetColliders(false);

    //        ResetAttackAnimations();
    //    }

    //    public override void ResetAttackAnimations()
    //    {
    //        plrScr.anim.SetBool(animationVariables[WeaponAnimations.ATTACK].animationName, false);
    //    }


    //    /// <summary>
    //    /// Se qualunque delle animazioni sta venendo eseguita, non si puo' ricominciare
    //    /// la routine di attacco
    //    /// </summary>
    //    public override bool CanAttack()
    //    {
    //        bool success = true;
    //        //foreach(Animation a in animationVariables.Values)
    //        //{
    //        //    if (a.state != AnimationStates.STOPPED)
    //        //    {
    //        //        success = false; break;
    //        //    }
    //        //}
    //        return success;
    //    }



    //}

    public class BastoneMagico : RangedWeapon
    {
        public BastoneMagico(Player s) :
            base(WeaponType.BASTONE_MAGICO, s)
        {

        }

        /// <summary>
        /// Qui inizializzeremo anche le variabili delle statistiche dell'arma
        /// </summary>
        public override void Init()
        {
            GameObject newWeapon = 
                GameObject.Instantiate(GameManager.GI().GetPrefab("BastoneMagico"));
            weapon = newWeapon;
            weapon.transform.SetParent(plrScr.weaponPivot.transform, false);

            //projectileSpawnPoint = UtilityShit.
            //    FindChildWithName(weapon, "ProjectileSpawnPoint").transform;
            projectileSpawnPoint = UtilityShit.
                FindChildWithName(plrScr.gameObject, "ProjectileSpawnPointPlayerBased").transform;
            

            fireRate = 0.4f;
            projectileSpeed = 35f;
            projectileTravelDistance = 20f;
        }

        public override bool CanAttack()
        {
            return canFire;
        }

        public override void ExitAttack()
        {
            base.ExitAttack();
        }


        public override void InitAttack()
        {
            base.InitAttack();
        }

        public override void ResetAttackAnimations()
        {
            base.ResetAttackAnimations();
        }


        public override void Update()
        {
            base.Update();
        }

        public override bool UpdateAttack()
        {
            if(canFire)
            {
                CreateProjectile();
                fireStopwatch.Reset();
                canFire = false;
            }

            return true;
        }

        public override IProjectile CreateProjectile()
        {
            IProjectile prj = ProjectileBastoneMagico.CreateProjectile(
                weapon,
                projectileSpawnPoint.position,
                projectileSpawnPoint.forward,
                projectileSpeed,
                projectileTravelDistance
                );
            return prj;
        }

    }

    public class Cannone : RangedWeapon
    {
        public Cannone(Player s) :
            base(WeaponType.CANNONE, s)
        {

        }

        public override bool CanAttack()
        {
            return true;
        }

        public override IProjectile CreateProjectile()
        {
            // Punto in cui arrivera' il proiettile
            Vector2 endPointXZ = GameManager.GI().lastHitPositionMouseXZ;
            Vector2 spawnPointXZ = new Vector2(
                projectileSpawnPoint.position.x, 
                projectileSpawnPoint.position.z);

            
            // Questi proiettili seguono una logica diversa: GUARDA SUMMARY CLASSE PROIETTILE
            float distance = Vector2.Distance(spawnPointXZ, endPointXZ);

            if(distance > projectileTravelDistance)
            {
                distance = projectileTravelDistance;
            }
            
            
            IProjectile prj = ProjectileCannone.CreateProjectile(
                weapon,
                projectileSpawnPoint.position,
                projectileSpawnPoint.forward,
                projectileDamage,   
                projectileSpeed,
                distance
                );
            return prj;
        }

        public override void ExitAttack()
        {
            base.ExitAttack();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void Init()
        {
            GameObject newWeapon = GameObject.Instantiate(
                GameManager.GI().GetPrefab("Cannone")
                );

            weapon = newWeapon;
            weapon.transform.SetParent(plrScr.weaponPivot.transform, false);

            projectileSpawnPoint = GameManager.GI().plrScr.projectileSpawnPoint.transform;


            fireRate = 1.5f;
            projectileSpeed = 15f;
            projectileTravelDistance = 8f;
            projectileDamage = 10;
        }

        public override void InitAttack()
        {
            base.InitAttack();
        }

        public override void ResetAttackAnimations()
        {
        }


        public override void Update()
        {
            base.Update();
        }

        public override bool UpdateAttack()
        {
            if (canFire)
            {
                CreateProjectile();
                fireStopwatch.Reset();
                canFire = false;
            }

            return true;
        }
    }
    #endregion

    #region ANIMAZIONI
    public enum AnimationStates
    {
        STOPPED,
        STARTED,
        END_ANIMATION, // Rappresenta i frame antecedenti all'effettiva fine dell'animazione, cio' che viene dopo e' trascurabile 
        ENDED   
    }

    /// <summary>
    /// Rappresenta un'animationClip del controller dell'animator
    /// E' estremamente utile per fare in modo di sapere lo stato dell'animazione.
    /// Hard Coded, se cambiano i valori delle clip dell' animator controller, devono cambiare anche i valori delle Animation
    /// </summary>
    public class Animation
    {
        public string animationName;
        public AnimationStates state;

        public Animation(string aN)
        {
            animationName = aN;
            state = AnimationStates.STOPPED;
        }
    }
    #endregion
}
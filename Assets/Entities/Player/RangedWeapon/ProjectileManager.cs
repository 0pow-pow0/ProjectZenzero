using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Singleton SINGLETONIA
/// </summary>
public class ProjectileManager : MonoBehaviour
{
    static ProjectileManager prjMng;

    // Singletonia singleton gameplay
    private ProjectileManager() { }

    public static ProjectileManager GI()
    {
        if (prjMng == null)
        {
            Debug.LogError("Projectile Manager non instanziato!");
            return null;
        }

        return prjMng;
    }

    // Todo, ottimizzabile tantissimo
    public List<IProjectile> projectiles;
    //public Projectile[] projectiles;

    void Awake()
    {
        prjMng = this;
    }

    ///// <summary>
    ///// Updata il codice di tutti i proiettili
    ///// </summary>
    //void Update()
    //{
    //    // Previene problemi se rimuovo dalla lista mentre itero
    //    for (int i = projectiles.Count - 1; i >= 0; i++)
    //    {
    //        if (projectiles[i].mustDestroy)
    //        {
    //            // Distruggi, DISTRUGGI, DISTRUGGI
    //            projectiles[i].Destroy();
    //            projectiles.RemoveAt(i);
    //        }
    //        else
    //        {
    //            //projectiles[i].Update();
    //        }
    //    }
    //}

    /// <summary>
    /// Non posso chiamare un distruggi
    /// </summary>
    void DestroyObjects()
    {

    }
}

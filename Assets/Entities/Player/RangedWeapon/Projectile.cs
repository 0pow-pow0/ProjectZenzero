using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    GameObject parent { get; set;  }
    // Serve per comunicare se dev'essere distrutto al GameManager
    bool mustDestroy { get; set; }


    #region STATISTICHE PROIETTILE
    // Punto in cui viene generato il proiettile
    Vector3 spawnPoint { get; set; }
    // Direzione verso cui si sposta
    Vector3 travelDirection { get; set; }
    Rigidbody rb { get; set; }

    float speed { get; set; }

    // Massima distanza percorribile prima che il proiettile esploda
    float maxTravelDistance { get; set; }
    #endregion

    #region STATISTICHE DI GAMEPLAY
    int damage { get; set; }
    #endregion
    // effetti
    // effect[] effects;
}

public class ProjectileUtility
{ 
    public static IProjectile GetProjectileInterfaceFromProjectileCollider(Collider coll) 
    {
        IProjectile iProj = coll.gameObject.GetComponentInParent<IProjectile>();
        if (iProj == null)
        {
            Debug.LogError("Impossibile trovare interfaccia proiettile");
            return null;
        }

        return iProj;
        //if(coll.gameObject.GetComponentInParent<ProjectileCannone>(true)
        //    != null)
        //{
        //    return (IProjectile)coll.gameObject.GetComponentInParent<ProjectileCannone>(true);
        //}
        //else if(coll.gameObject.GetComponentInParent<ProjectileBastoneMagico>(true)
        //    != null)
        //{
        //    return (IProjectile)coll.gameObject.GetComponentInParent<ProjectileBastoneMagico>(true);
        //}
    }
}

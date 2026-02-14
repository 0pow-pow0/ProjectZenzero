using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System;
using UnityEngine;
using System.Collections.Specialized;
using UnityEditor.Connect;
using System.Net.Security;

/// <summary>
/// Il cannone del proiettile finche' il cursore si trova in un determinato range,
/// a differenza degli altri proiettili, cadra' esattemente in quel punto.
/// Se il cursore si trova aldila' del range cadra' nella direzione del cursore,
/// ma nel punto massimo raggiungibile dal suo range :D.
/// </summary>
public class ProjectileCannone : MonoBehaviour, IProjectile
{
    public GameObject parent { get ; set ; }
    public Vector3 spawnPoint { get ; set ; }
    public Vector3 travelDirection { get ; set ; }
    public Rigidbody rb { get; set; }
    public float speed { get ; set ; }
    
    /// <summary>
    /// STATISTICHE DI GAMEPLAY
    /// </summary>
    public int damage { get; set; }

    public float maxTravelDistance { get ; set ; }
    public bool mustDestroy { get ; set ; }

    // Tempo prima che si distrugga il proiettile dopo la prima collisione
    public readonly float timeAfterTriggerDestroy = .2f;
    public readonly float projectileMass = 20f;
    GameObject meshProjectile;
    GameObject meshExplosion;

    RelayCollider relayProjectile;
    RelayCollider relayExplosion;
    RelayCollider relayWallCollider;

    static public ProjectileCannone CreateProjectile(
        GameObject _parent,
        Vector3 _spawnPoint,
        Vector3 _travelDir,
        int  _damage,
        float _speed,
        float _maxTravelDistance)  
    {
        GameObject newProj = GameObject.Instantiate(GameManager.GI().GetPrefab("PallaDiCannone"));
        ProjectileCannone scr = newProj.GetComponent<ProjectileCannone>();

        /// TODO un po' lentuccio
        scr.meshProjectile = UtilityShit.FindChildWithName(newProj, "Mesh");
        scr.meshExplosion = UtilityShit.FindChildWithName(newProj, "MeshExplosionAndCollider");

        //----- SETTA RELAY 
        scr.relayProjectile = scr.meshProjectile.GetComponent<RelayCollider>();
        scr.relayProjectile.relayOnTriggerEnter = scr.RelayOnTriggerEnterProjectile;
        scr.relayProjectile.relayOnTriggerStay = scr.RelayOnTriggerStayProjectile;
        scr.relayProjectile.relayOnTriggerExit = scr.RelayOnTriggerExitProjectile;

        scr.relayExplosion = scr.meshExplosion.GetComponent<RelayCollider>();
        scr.relayExplosion.relayOnTriggerEnter = scr.RelayOnTriggerEnterExplosion;
        scr.relayExplosion.relayOnTriggerStay = scr.RelayOnTriggerStayExplosion;
        scr.relayExplosion.relayOnTriggerExit = scr.RelayOnTriggerExitExplosion;

        scr.relayWallCollider = 
            UtilityShit.FindChildWithName(scr.gameObject, "WallCollider")
                .GetComponent<RelayCollider>();
        scr.relayWallCollider.relayOnTriggerEnter = scr.RelayOnTriggerEnterWall;




        scr.meshExplosion.SetActive(false); 
        
        newProj.transform.SetParent(GameManager.GI().projectilesWrapper.transform, true);





        //--------------------------------- SETTA VARIABILI 
        scr.spawnPoint = _spawnPoint;
        // Non posso settarlo in awake perche' l'execution order non lo permette
        scr.transform.position = scr.spawnPoint;

        scr.transform.rotation =
            GameManager.GI().plrScr.projectileSpawnPoint.transform.rotation;
        
        //scr.travelDirection = _travelDir;
        scr.speed = _speed;
        scr.damage = _damage;
        scr.maxTravelDistance = _maxTravelDistance;

        scr.rb = newProj.AddComponent<Rigidbody>();
        scr.rb.useGravity = false;
        scr.rb.mass = scr.projectileMass;
        scr.rb.velocity = scr.transform.forward * scr.speed;//+ GameManager.GI().plrScr.rb.velocity;
        scr.rb.constraints = RigidbodyConstraints.FreezeRotation;

        Vector3 projDest = scr.transform.position + scr.transform.forward * _maxTravelDistance ;



        //Ray meshExplosionToTerrain = new Ray();
        //meshExplosionToTerrain.origin = projDest;
        //meshExplosionToTerrain.direction = -Vector3.up; // Punta verso il terreno

        //RaycastHit meshExplosionToTerrainHitInfo;
        ////RaycastHit hitInfos;
        //bool hasHit = Physics.Raycast(meshExplosionToTerrain, out meshExplosionToTerrainHitInfo,
        //    1000f, LayerMask.GetMask("TerrenoMesh"));

        //Vector3 meshExplosionDest;
        //if (hasHit)
        //{
        //    meshExplosionDest = meshExplosionToTerrainHitInfo.point
        //        + new Vector3(0, 0.01f, 0); // Previene compenetrazioni strane
        //}
        //else
        //{
        //    // Se spara fuori dalla mappa e' un big rip sappilo
        //    meshExplosionDest = Vector3.zero;
        //    Debug.LogError("Bug");
        //}
        //projDest = meshExplosionDest;







        //GameObject dbgPt = Instantiate(GameManager.GI().GetPrefab("DebugPoint"));
        //dbgPt.transform.position = projDest;


        //float timeToReachDestination =
        //    (scr.transform.position.magnitude - projDest.magnitude)
        //    /
        //    scr.rb.velocity.magnitude;
        //Debug.Log("Magnitude pos: " + scr.transform.position.magnitude +
        //    " Magnitude dest: " + projDest.magnitude);
        //Debug.Log("TimeToReach: " + timeToReachDestination);

        //float ySpeed = (scr.transform.position.y - projDest.y)
        //    /
        //    timeToReachDestination;
        //Debug.Log(scr.transform.position.y - projDest.y);
        //Debug.Log("yspeed:  " + ySpeed);
        //scr.rb.velocity = new Vector3(
        //    scr.rb.velocity.x,
        //    -ySpeed,
        //    scr.rb.velocity.z
        //    );

        return scr;
    }




    void Update()
    {
        if (mustDestroy) 
        { 
            DestroyME();
            return;
        }
        /// TODO Se collide con i muri
        float distanceFromSpawnPoint = Vector3.Distance(spawnPoint, transform.position);


        if (distanceFromSpawnPoint >= maxTravelDistance)
        {
            mustDestroy = true;
        }
    }

    bool DestroyMECalled = false;
    void DestroyME()
    {
        if (DestroyMECalled)
            return;
        DestroyMECalled = true;

        Debug.Log("Proj Distrutto!");

        // Adesso dobbiamo fermare il proiettile
        // e generare l'esplosione
        meshExplosion.SetActive(true);
        Ray meshExplosionToTerrain = new Ray();
        meshExplosionToTerrain.origin = transform.position; ;
        meshExplosionToTerrain.direction = -Vector3.up; // Punta verso il terreno

        RaycastHit meshExplosionToTerrainHitInfo;
        //RaycastHit hitInfos;
        bool hasHit = Physics.Raycast(meshExplosionToTerrain, out meshExplosionToTerrainHitInfo,
            1000f, LayerMask.GetMask("TerrenoMesh"));

        Vector3 meshExplosionDest;
        if (hasHit)
        {
            meshExplosionDest = meshExplosionToTerrainHitInfo.point 
                + new Vector3(0, 0.01f, 0); // Previene compenetrazioni strane
        }
        else
        {
            // Se spara fuori dalla mappa e' un big rip sappilo
            meshExplosionDest = Vector3.zero;
            Debug.LogError("Bug");
        }
        meshExplosion.transform.position = meshExplosionDest;
        meshProjectile.SetActive(false);
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        Destroy(gameObject, timeAfterTriggerDestroy);
    }

    #region RELAYS
    void RelayOnTriggerEnterProjectile(Collider other)
    {
        if(//other.gameObject.layer == LayerMask.NameToLayer("TerrenoMesh") ||
            other.gameObject.layer == LayerMask.NameToLayer("enemyBodyCollider"))
        {
            Debug.Log("COCK");
            mustDestroy = true;
        }
    }

    void RelayOnTriggerStayProjectile(Collider other)
    {
        //Debug.Log("Collided with: " + LayerMask.LayerToName(other.gameObject.layer));
        if (//other.gameObject.layer == LayerMask.NameToLayer("TerrenoMesh") ||
            other.gameObject.layer == LayerMask.NameToLayer("enemyBodyCollider"))
        {
            mustDestroy = true;
        }
    }

    void RelayOnTriggerExitProjectile(Collider other)
    {

    }

    void RelayOnTriggerEnterWall(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("TerrenoMesh"))
        {
            mustDestroy = true;
        }
    }

    void RelayOnTriggerEnterExplosion(Collider other)
    {

    }

    void RelayOnTriggerStayExplosion(Collider other)
    {

    }

    void RelayOnTriggerExitExplosion(Collider other)
    {

    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

public class ProjectileBastoneMagico : MonoBehaviour, IProjectile
{
    public GameObject parent { get; set; }
    public Vector3 spawnPoint { get; set; }
    public Vector3 travelDirection { get; set; }
    public float speed { get; set; }

    /// <summary>
    /// STATISTICHE DI GAMEPLAY
    /// </summary>
    public int damage { get; set; } = 4;

    public float travelledDistance { get; set; }
    public float maxTravelDistance { get; set; }
    public bool mustDestroy { get; set; }
    public Rigidbody rb { get; set; }


    private ProjectileBastoneMagico() 
    {

    }
    
    static public ProjectileBastoneMagico CreateProjectile(
        GameObject _parent,
        Vector3 _spawnPoint,
        Vector3 _travelDir,
        float _speed,
        float _maxTravelDistance)
    {
        GameObject newProj = GameObject.Instantiate(GameManager.GI().GetPrefab("ProjectileBastoneMagico"));
        ProjectileBastoneMagico scr = newProj.AddComponent<ProjectileBastoneMagico>();
        
        newProj.transform.SetParent(GameManager.GI().projectilesWrapper.transform, true);
        scr.rb = newProj.AddComponent<Rigidbody>();
        scr.rb.useGravity = false;
        
        scr.spawnPoint = _spawnPoint;
        scr.travelDirection = _travelDir;
        scr.speed = _speed;
        scr.rb.velocity = scr.travelDirection * scr.speed;
        scr.maxTravelDistance = _maxTravelDistance;

        // Non posso settarlo in awake perche' l'execution order non lo permette
        scr.transform.position = scr.spawnPoint;

        return scr; 
    }

    void Awake()
    {
    }

    void Update()
    {
        /// TODO Se collide con i muri
        float distanceFromSpawnPoint = Vector3.Distance(spawnPoint, transform.position); 

        if(distanceFromSpawnPoint >= maxTravelDistance)
        {
            mustDestroy = true;
        }

        if(mustDestroy) { Debug.Log("Proj Distrutto!"); Destroy(gameObject); }
    }

    void OnTriggerEnter(Collider other)
    {
    
    }
    
}


using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;


/// <summary>
/// Contenitori della logica per generare nemici ogni tot secondi. 
/// Richiedono array di GameObject in cui generare i suddetti nemici.
/// Dato questo array faranno un giro, generando un nemico in ognuno di loro
/// </summary>
/// <typeparam name="EnemyType"></typeparam>
/// <typeparam name="StatsTemplate"></typeparam>
public class SpawnerEnemy 
{
    public List<Enemy> spawnedEnemies {  get; protected set; }

    protected int spawnCount;   // Quanti ne sono stati spawnati
    protected int toSpawnCount { get; private set; } // Quanti ne devo spawnare
    protected float spawnRate;    // Velocita' di spawn AL SECONDO

    public BaseStats spawnStats { get; set; }

    // Qui verranno passate le funzioni spawn del livello
    // cosi che ogni spawn possa essere aggiunto ad un array
    protected DeltaTimerAction spawnAction;


    /// <summary>
    /// Questo array verra' utilizzato a rotazione, un'indice cambiera' a rotazione
    /// lo spawn da utilizzare.
    /// </summary>
    public GameObject[] spawnPoints;
    int indexRotationSpawnpoint = 0;


    protected SpawnerEnemy(
        int _toSpawnCount,
        float _spawnRate,
        BaseStats _spawnStats
        )
    {
        toSpawnCount = _toSpawnCount;
        spawnRate = _spawnRate;
        spawnStats = _spawnStats;
        spawnAction = DeltaTimerAction.CreateDeltaTimerAction
            (Spawn,
            1 / spawnRate, // SECONDO / rateo di spawn al secondo 
            false
            );

        spawnedEnemies = new List<Enemy>();
    }

    protected GameObject GetGenerationSpawnpoint()
    {
        indexRotationSpawnpoint++;
        if (indexRotationSpawnpoint > spawnPoints.Length - 1)
        {
            // Ricomincia la rotazione
            indexRotationSpawnpoint = 0;
        }

        return spawnPoints[indexRotationSpawnpoint];
    }

    public virtual void Spawn()
    {
        EnemyButcher newEnem = EnemyButcher.Create();
        Debug.Log("New Stats: " + newEnem.stats);
        newEnem.SetStats(spawnStats);
        newEnem.spawnPoint = GetGenerationSpawnpoint();
        newEnem.transform.SetParent(GameManager.GI().enemiesWrapper.transform);
        spawnedEnemies.Add(newEnem);
        Debug.Log("Spawned!");
    }

    public virtual void Update() { }

    protected void ChangeSpawnRate() { Debug.LogError("NON IMPLEMENTATO"); }
}


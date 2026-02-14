using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Contenitori della logica per generare nemici ogni tot secondi. 
/// Richiedono array di GameObject in cui generare i suddetti nemici.
/// Dato questo array faranno un giro, generando un nemico in ognuno di loro
/// </summary>
/// <typeparam name="EnemyType"></typeparam>
/// <typeparam name="StatsTemplate"></typeparam>
public class SpawnerEnemyFarmer : SpawnerEnemy
{


    public SpawnerEnemyFarmer(
        int _toSpawnCount,
        float _spawnRate,
        BaseStatsFarmer _spawnStats) :
        base(_toSpawnCount, _spawnRate, _spawnStats)
    {

    }

    public override void Spawn()
    {
        if (spawnCount > toSpawnCount)
        {
            return;
        }

        EnemyFarmer newEnem = EnemyFarmer.Create();
        newEnem.SetStats(spawnStats);
        //Debug.Log("Spawner Stats: " + spawnStats);
        //Debug.Log("");
        newEnem.spawnPoint = GetGenerationSpawnpoint();
        newEnem.transform.SetParent(GameManager.GI().enemiesWrapper.transform);
        spawnedEnemies.Add(newEnem);
        spawnCount++;
    }

    public override void Update()
    {
        // il gamelevel si occupa di eliminarli
    }

}


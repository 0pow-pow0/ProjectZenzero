using UnityEngine;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

/// <summary>
/// Statistiche base di ogni entita' nemica.
/// Gli HP vengono settati automaticamente a maxHP
/// </summary>
public class BaseStats
{

    public int HP { get; protected set; }
    public int maxHP { get; set; }
    public float speed { get; set; }

    // Danno se si entra nel collider del corpo del nemico
    public int damageBodyCollision { get; set; }

    public bool invincible { get; set; }

    /// <summary>
    /// Valore particella exp che droppera' appena morto
    /// </summary>
    public float droppedExp { get; protected set; } = 0f;

    /// <summary>
    /// Aggiunge HP mantenendo i limiti dettati da maxHP
    /// </summary>
    public void AddHP(int toAdd)
    {
        if(HP + toAdd <= 0)
        {
            HP = 0;
        }
        else if(HP + toAdd > maxHP)
        {
            HP = maxHP;
        }
        else

        {
            HP += toAdd;    
        }
    }

    public void SetDroppedEXP(float newValue)
    {
        if(newValue <= 0)
        {
            Debug.LogError("VALORE EXP NEGATIVO!!!!");
        }
        else
        {
            droppedExp = newValue;
        }
    }


    /// <summary>
    /// Porta gli hp al massimo valore di hp raggiungibile
    /// </summary>
    public void RefillHP()
    {
        HP = maxHP;
    }

    public override string ToString()
    {
        string result = 
            "HP: " + HP +
            "\nMaxHP: " + maxHP +
            "\nSpeed: " + speed +
            "\nDamage: " + damageBodyCollision +
            "\nIs Invincible: " + invincible
            ;

        return result;
    }
}

public class BaseStatsButcher : BaseStats
{

    public BaseStatsButcher() { }
    public BaseStatsButcher(int _maxHP,
        float _speed,
        int _damageBodyCollision,
        float _droppedEXP)
    {
        maxHP = _maxHP;
        HP = maxHP;
        speed = _speed;
        damageBodyCollision = _damageBodyCollision;
        droppedExp = _droppedEXP;
        invincible = false;
    }


}


public class BaseStatsFarmer : BaseStats
{
    public BaseStatsFarmer() { }
    public BaseStatsFarmer(int _maxHP,
        float _speed,
        int _damageBodyCollision,
        float _droppedEXP)
    {
        maxHP = _maxHP;
        HP = maxHP;
        speed = _speed;
        damageBodyCollision = _damageBodyCollision;
        droppedExp = _droppedEXP;
        invincible = false;
    }
}
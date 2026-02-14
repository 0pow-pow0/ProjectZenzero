using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana_Health_input : MonoBehaviour
{

    [System.NonSerialized] Mana manaScript;
    [System.NonSerialized] Healt healthScript;


    private void Start()
    {
        manaScript = GetComponent<Mana>();
        healthScript = GetComponent<Healt>();
    }
    void Update()
    {
        ManaInfo();
        HealthInfo();
      
      
      


    }

    void ManaInfo()
    {

        if (Input.GetKeyDown(KeyCode.M))
        {
            manaScript.numOfMana++;
        }
        else if (Input.GetKeyDown(KeyCode.N))   
        {

            manaScript.TotMana++;
        }


        else if (Input.GetKeyDown(KeyCode.B))
        {
            manaScript.TotMana--;
        }
    }
    void HealthInfo()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            healthScript.numOfHearts++;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {

            healthScript.health++;
        }


        else if (Input.GetKeyDown(KeyCode.A))
        {
            healthScript.health--;
        }
    }
}

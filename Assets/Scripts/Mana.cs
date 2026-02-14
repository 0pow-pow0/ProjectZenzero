using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour
{
    public int TotMana;
    public int numOfMana;

    public Image[] mana;
    public Sprite fullMana;
    public Sprite emptyMana;




    public void Update()
    {

        if (TotMana > numOfMana)
        {
            TotMana = numOfMana;

        }
        for (int i = 0; i < mana.Length; i++)
        {
            if (i < TotMana)
            {
                mana[i].sprite = fullMana;
            }
            else
            {
                mana[i].sprite = emptyMana;
            }

            if (i < numOfMana)
            {
                mana[i].enabled = true;
            }
            else
            {
                mana[i].enabled = false;


            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healt : MonoBehaviour
{
    public int health;
    public int numOfHearts;

    public Image[] heart;
    public Sprite fullHeart;
    public Sprite emptyHeart;




    public void Update()
    {

        if (health > numOfHearts)
        {
            health = numOfHearts;
            
        }
        for (int i = 0; i < heart.Length; i++)
        {
            if(i < health)
            {
                heart[i].sprite = fullHeart;
            }
            else
            {
                heart[i].sprite = emptyHeart;
            }

            if( i < numOfHearts )
            {
                heart[i].enabled = true;
            } 
            else
            {
                heart[i].enabled = false;
                

            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameUIPanel : MonoBehaviour
{
    //---- REFERENCES
    [SerializeField] private Slider sliderPlayerHP;
    [SerializeField] private Slider sliderPlayerEXP;
    [SerializeField] private GameObject characterIcon;


    /// <summary>
    /// Setta la barra della vita calcolandolo sulla base della vita massima
    /// </summary>
    /// <param name="max"></param>
    public void SetPlayerHPSliderValue(int actualHP, int maxHP)
    {
        if(maxHP <= 0)
        {
            Debug.LogError("Impossibile settare barraHP, maxHP inferiori a 0!");
        }

        float newValue = actualHP / maxHP;
        if (newValue > 1)
        {
            newValue = 1;
            Debug.LogWarning("Valore slider superiore a 1");
        }
        else if (newValue < 0)
        {
            newValue = 0;
            Debug.LogWarning("Valore slider inferiore a 0");
        }

        sliderPlayerHP.value = newValue;
    }
    public void SetPlayerEXPSliderValue(float actualEXP, 
        float nextLevelRequiredEXP, float preaviousLevelRequiredExp)
    {
        
        
        float newValue = 
            (actualEXP - preaviousLevelRequiredExp)
            /
            (nextLevelRequiredEXP - preaviousLevelRequiredExp);
        Debug.Log((actualEXP - preaviousLevelRequiredExp) +
            " " + (nextLevelRequiredEXP - preaviousLevelRequiredExp));
        
        if (newValue > 1)
        {
            newValue = 1;
            Debug.LogWarning("Valore slider superiore a 1");
        }
        else if (newValue < 0)
        {
            newValue = 0;
            Debug.LogWarning("Valore slider inferiore a 0");
        }

        sliderPlayerEXP.value = newValue;
    }

    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

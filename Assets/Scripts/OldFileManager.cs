using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Singleton per l'accesso di file di gioco.
/// </summary>
public class FileManager : MonoBehaviour
{
    static FileManager inst;

    private Dictionary<string, Sprite> sprites;
    private Dictionary<string, GameObject> prefabs;

    /// <summary>
    /// Ottieni unica instanza del singleton.
    /// </summary>
    /// <returns></returns>
    public static FileManager GI()
    {
        if(inst == null)
        {
            Debug.LogError("FileManager non instanziato");
            //inst = This//inst = new FileManager();
        }

        return inst;
    }

    void Awake()
    {
        inst = this;

        sprites = new Dictionary<string, Sprite>();
        prefabs = new Dictionary<string, GameObject>();

        sprites["CardFrontPowerUpWeapon"] = Resources.Load<Sprite>("Assets/Cards/CardFrontRed");
        sprites["CardFrontPowerUpStat"] = Resources.Load<Sprite>("Assets/Cards/CardFrontGreen");
        Debug.Log(sprites);
        prefabs["CardTemplate"] = Resources.Load<GameObject>("Prefabs/CardTemplate");
    }  

    public Sprite GetSprite(string spriteName)
    {
        // Se non contiene la chiave
        if(!sprites.ContainsKey(spriteName))
        {
            Debug.LogError("Sprite: " + spriteName + " non trovata!");
            return null;
        }

        return sprites[spriteName];
    }

    public GameObject GetPrefab(string prefabName)
    {
        // Se non contiene la chiave
        if (!prefabs.ContainsKey(prefabName))
        {
            Debug.LogError("Prefab: " + prefabName + " non trovata!");
            return null;
        }

        return prefabs[prefabName];
    }
}

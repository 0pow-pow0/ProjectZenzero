using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Le armi ranged sono leggermente particolari,
/// instanziano proiettili e condividono a ColliderInfo i loro collider,
/// i proiettili da loro instanziato sono trattati come fossero "armi speciali"
/// che non seguono la struttura ad albero delle normali armi 
/// ma possiedono un ArmaBaseCollisionInfo.
/// I proiettili vengono instanziati nel gameObject: "Collision Wrapper".
/// 
/// Questa convenzione che seguo con i proiettili e' utile per poter modificare ogni singolo
/// proiettile senza farlo dipendere da praticamente nulla.
/// Ogni proiettile puo' essere diverso l'uno dall'altro senza alcun problema, ad esempio
/// posso avere 3 proiettili di cui i 2 laterali sono uguali ma quello centrale e' piu' potente
/// ed esplode :). 
/// </summary>
public class Arco : MonoBehaviour
{
    [NonSerialized] List<GameObject> projectiles;

    [SerializeField] Animator anim; 

    void Awake()
    {
        projectiles = new List<GameObject>();
    }

    void Update()
    {

    }

    void InstanziaProiettile()
    {

    }
}

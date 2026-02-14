using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EXPOrb : MonoBehaviour
{
    [SerializeField] private float expValue;
    [SerializeField] public Rigidbody rb;
    public void SetEXPValue(float newValue)
    {
        if (newValue <= 0f)
        {
            Debug.LogError("L'exp orb non puo' avere un valore negativo");
        }
        else
        {
            expValue = newValue;
        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Counter delle sfere con cui si sta collidendo
    //List<Collider> expOrbsCollidingWith;
    void OnTriggerEnter(Collider other)
    {

    }

    void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if(other.gameObject.layer == LayerMask.NameToLayer("playerBodyCollider"))
        {
            transform.position = Vector3.Lerp(transform.position,
                GameManager.GI().plrScr.transform.position, 0.05f);

            Vector2 playerPos2D = new Vector2(GameManager.GI().plrScr.transform.position.x, 
                GameManager.GI().plrScr.transform.position.z);
            if (Vector2.Distance(playerPos2D, 
                    new Vector2(transform.position.x, transform.position.z))
                <= 1.5f)
            {
                GameManager.GI().plrScr.AddEXP(expValue);
                Destroy(gameObject);
            }
        }
        //if(other.gameObject.layer == LayerMask.NameToLayer("EXPGatherCollider"))
        //{
        //    if(!expOrbsCollidingWith.Contains(other))
        //    {
        //        exp
        //    }
        //}
    }
}

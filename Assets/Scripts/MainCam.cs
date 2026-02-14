using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{
    [System.NonSerialized] GameObject plr;
    Vector3 distanceFromPlayer;
    // Start is called before the first frame update
    void Awake()
    {
        plr = GameObject.FindGameObjectWithTag("Player");
        distanceFromPlayer = transform.position - plr.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = plr.transform.position + distanceFromPlayer;


    }
}

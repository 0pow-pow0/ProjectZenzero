using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelayCollider : MonoBehaviour
{
    public Action<Collider> relayOnTriggerEnter;
    public Action<Collider> relayOnTriggerStay;
    public Action<Collider> relayOnTriggerExit;
    public Action<Collision> relayOnCollisionEnter = null;
    public Action<Collision> relayOnCollisionStay = null;
    public Action<Collision> relayOnCollisionExit = null;


    // TODO da rimuovere gli if
    void OnTriggerEnter(Collider other)
    {
        if (relayOnTriggerEnter == null) { return; }
        relayOnTriggerEnter.Invoke(other);
    }

    void OnTriggerStay(Collider other)
    {
        if (relayOnTriggerStay == null) { return; }
        relayOnTriggerStay.Invoke(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (relayOnTriggerExit == null) { return; }
        relayOnTriggerExit.Invoke(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (relayOnTriggerEnter == null) { return; }
            relayOnCollisionEnter.Invoke(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (relayOnTriggerStay == null) { return; }
        relayOnCollisionStay.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (relayOnTriggerExit == null) { return; }
        relayOnCollisionExit.Invoke(collision);
    }
}

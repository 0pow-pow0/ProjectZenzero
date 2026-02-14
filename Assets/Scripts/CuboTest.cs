using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CuboTest : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] TextMeshProUGUI hitboxInfoText;
    [SerializeField] Material mat;
    [NonSerialized] public Color lastColor;
    // Start is called before the first frame update
    void Start()
    {
        lastColor = mat.color;
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform trf = hitboxInfoText.GetComponent<RectTransform>();
        trf.anchoredPosition = cam.WorldToScreenPoint(transform.position);
        trf.anchoredPosition += new Vector2(0, 40);
    }

    void OnTriggerEnter(Collider coll)
    {
        
        Debug.Log(LayerMask.LayerToName(coll.gameObject.layer));
        if(LayerMask.LayerToName(coll.gameObject.layer) == "meleeWeaponHitboxCollider")
        {
            Debug.Log("pene");
            hitboxInfoText.text = "Melee Weapon";

            mat.color = new Color(255, 0, 0);

        }
        if(LayerMask.LayerToName(coll.gameObject.layer) == "ProjectileHitboxCollider")
        {
            hitboxInfoText.text = "Projectile";

            mat.color = new Color(0, 0, 255);

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(!called)
            StartCoroutine(ResetValues());
    }

    bool called = false;
    IEnumerator ResetValues(float t = 2f)
    {
        called = true;
        yield return new WaitForSeconds(t);
        mat.color = lastColor;
        hitboxInfoText.text = "";
        called = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Da distruggere
/// </summary>
public class CardInteractions : MonoBehaviour
{
    [System.NonSerialized] Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Quando il cursore entra dentro il rect della carta
    /// </summary>
    public void PointerEnter()
    {
        anim.SetBool("isHover", true);
        anim.SetTrigger("isHoverTrigg");
        Debug.Log("Entered");
    }

    /// <summary>
    /// Quando il cursore esce
    /// </summary>
    public void PointerExit()
    {
        anim.SetBool("isHover", false);
        Debug.Log("Exited");
    }

    /// <summary>
    /// Verra' eseguita appena la pressione verra' rilasciata
    /// </summary>
    public void PointerUp()
    {
        // Placeholder position TODO scrivi meglio
        transform.position = transform.parent.position;
    }

    public void PointerDrag()
    {
        Debug.Log("DRAG"); 
        Vector3 mousePos = Input.mousePosition;
        transform.position = mousePos;

    }
}

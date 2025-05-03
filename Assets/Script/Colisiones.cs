using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colisiones : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision choque)
    {
        Debug.Log("Entró colisión con " + choque.gameObject.name);
    }

    private void OnCollisionStay(Collision contacto)
    {
        Debug.Log("Sigue tocando " +  contacto.gameObject.name);
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Sale colisión con " +  collision.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger con " + other.gameObject.name);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Sigue Trigger" + other.gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Sale Trigger de " + other.gameObject.name);
    }
}
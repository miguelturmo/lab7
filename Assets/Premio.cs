using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Premio : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("mlagent"))
        {
            Invoke("MoverPosicionInicial", 4);
        }
    }
    private void MoverPosicionInicial()
    {
        bool posicionEncontrada = false;
        int intentos = 100;
        Vector3 posicionPotencial = Vector3.zero;
        while(!posicionEncontrada|| intentos>=0)
        {
            intentos--;
            posicionPotencial = new Vector3(transform.parent.position.x + UnityEngine.Random.Range(-4f, 4f), 0.555f, transform.parent.position.z + UnityEngine.Random.Range(-4f, 4f));
            Collider[] colliders = Physics.OverlapSphere(posicionPotencial, 0.05f);
            if(colliders.Length == 0)
            {
                transform.position = posicionPotencial;
                posicionEncontrada = true;
            }
        }
    }
}

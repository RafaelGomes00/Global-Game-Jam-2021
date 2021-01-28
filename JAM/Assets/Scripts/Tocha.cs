using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tocha : MonoBehaviour
{
    [SerializeField] private int tempoTocha;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().RecarregarTocha(tempoTocha);
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machado : MonoBehaviour
{
    [SerializeField] private int dano;
    [SerializeField] private float velocidadeGiro;

    private void Update()
    {
        transform.Rotate(0, velocidadeGiro*Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !other.isTrigger)
        {
            Debug.Log("player");
            other.gameObject.GetComponent<Player>().TomarDano(dano);
            Destroy(this.gameObject);
        }
    }
}

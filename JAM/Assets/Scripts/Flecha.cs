using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flecha : MonoBehaviour
{
    [SerializeField] private int dano;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !other.isTrigger)
        {
            Debug.Log("player");
            other.gameObject.GetComponent<Player>().TomarDano(dano);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "Inimigo" && !other.isTrigger)
        {
            Debug.Log("inimigo");
            other.gameObject.GetComponent<Inimigo>().TomarDano(dano);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "InimigoTorre" && !other.isTrigger)
        {
            Debug.Log("inimigotrre");
            other.gameObject.GetComponent<InimigoTorre>().TomarDano(dano);
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoTorre : MonoBehaviour
{

    [SerializeField] private float velocidadeReal;
    [SerializeField] private GameObject tiro;
    [SerializeField] private Transform pontoTiro;
    [SerializeField] private float velocidadeAtaque;
    [SerializeField] private float distanciaAtaque;
    [SerializeField] private int maxVida;
    [SerializeField] private GameObject cilindro;
    private int vida;
    private GameObject player;
    private bool podeAtacar;
    private bool move;
    private bool perseguindoPlayer;
    private float cooldownAtaque;
    private Vector3 frente;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        perseguindoPlayer = false;
        podeAtacar = true;
        vida = maxVida;
        frente = transform.TransformDirection(-Vector3.forward);
    }
    public void TomarDano(int dano)
    {
        vida = vida - dano;
        if (vida <= 0)
        {
            Morrer();
        }
    }
    private IEnumerator Atacar()
    {
        cooldownAtaque = velocidadeAtaque;

        Coroutine mira = StartCoroutine(Mirar());

        yield return new WaitForSeconds(1f);

        //anim.SetTrigger("Atirar");
        //audioSource.PlayOneShot(tiro);
        //anim.SetBool("Atacar", false);
        StopCoroutine(mira);
        //anim.ResetTrigger("Atirar");
        GameObject balaInstanciada = Instantiate(tiro, pontoTiro.position, Quaternion.identity);
        balaInstanciada.transform.LookAt(player.transform);
        StartCoroutine(EmpurraBala(balaInstanciada, player.transform));
        Destroy(balaInstanciada, 10f);
        //balaInstanciada.GetComponent<Bala>().SetCasterCollider(this.GetComponent<Collider>());

        yield return new WaitForSeconds(velocidadeAtaque);
        podeAtacar = true;
    }
    private IEnumerator EmpurraBala(GameObject balaInstanciada, Transform playerTransform)
    {
        while (balaInstanciada != null)
        {
            balaInstanciada.transform.position = Vector3.MoveTowards(balaInstanciada.transform.position, playerTransform.position, 10f * Time.deltaTime);
            yield return null;
        }
    }
    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Vector3 jogadorTransform = collider.gameObject.transform.position - transform.position;
            if (Vector3.Dot(frente, jogadorTransform) < 0)
            {
                Debug.Log("Frente");

                float distancia = Vector3.Distance(collider.gameObject.transform.position, gameObject.transform.position);
                if (Vector3.Distance(this.transform.position, player.transform.position) <= distanciaAtaque)
                {
                    move = false;
                    if (podeAtacar)
                    {
                        podeAtacar = false;
                        StartCoroutine(Atacar());
                    }

                }
            }
            else
            {
                Debug.Log("Tras");
            }
        }
    }
    IEnumerator Mirar()
    {

        while (true)
        {
            move = false;
            Vector3 targetPostition = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z);
            this.transform.LookAt(targetPostition);
            yield return null;
        }
    }
    private void Morrer()
    {
        Destroy(cilindro.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        perseguindoPlayer = false;
    }
}

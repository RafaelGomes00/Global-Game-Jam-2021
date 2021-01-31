using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Inimigo : MonoBehaviour
{

    [SerializeField] private float velocidadeReal;
    [SerializeField] private GameObject tiro;
    [SerializeField] private Transform pontoTiro;
    [SerializeField] private float velocidadeAtaque;
    [SerializeField] private float distanciaAtaque;
    [SerializeField] private int maxVida;
    [SerializeField] private Animator animator;


    private float velocidade;
    private bool recarregado;
    private int vida;
    private GameObject player;
    private bool podeAtacar;
    private bool move;
    private NavMeshAgent navMeshAgent;
    private bool perseguindoPlayer;
    private float cooldownAtaque;
    private bool morto;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        perseguindoPlayer = false;
        podeAtacar = true;
        vida = maxVida;
    }
    public void TomarDano(int dano)
    {
        vida = vida - dano;
        if (vida <= 0)
        {
            animator.SetTrigger("Morrer");
            Morrer();
        }
    }
    private IEnumerator Atacar()
    {

        cooldownAtaque = velocidadeAtaque;

        //Coroutine mira = StartCoroutine(Mirar());

        //yield return new WaitForSeconds(1f);

        //anim.SetTrigger("Atirar");
        //audioSource.PlayOneShot(tiro);
        //anim.SetBool("Atacar", false);
        //StopCoroutine(mira);
        //anim.ResetTrigger("Atirar");
        GameObject balaInstanciada = Instantiate(tiro, pontoTiro.position, pontoTiro.transform.rotation);
        balaInstanciada.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
        animator.SetTrigger("Atacar");
        Destroy(balaInstanciada, 1f);
        //balaInstanciada.GetComponent<Bala>().SetCasterCollider(this.GetComponent<Collider>());

        yield return new WaitForSeconds(velocidadeAtaque);
        podeAtacar = true;
    }
    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Vector3 targetPostition = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z);
            this.transform.LookAt(targetPostition);

            perseguindoPlayer = true;

            float distancia = Vector3.Distance(collider.gameObject.transform.position, gameObject.transform.position);
            if (Vector3.Distance(this.transform.position, player.transform.position) <= distanciaAtaque)
            {
                if (!morto)
                {
                    navMeshAgent.isStopped = true;
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
                if (!morto)
                navMeshAgent.isStopped = false;
                Movimentar(player.transform.position);
            }
        }
    }
    private void Movimentar(Vector3 destino, bool move = true)
    {
        if (!move && navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;
            animator.SetBool("Correr", false);
            return;
        }

        if (!morto)
        {
            navMeshAgent.isStopped = false;
            animator.SetBool("Correr", true);
            navMeshAgent.SetDestination(destino);
        }
    }
    IEnumerator Mirar()
    {
        navMeshAgent.isStopped = true;

        while (true)
        {
            move = false;
            yield return null;
        }
    }
    private void Morrer()
    {
        morto = true;
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Movimentar(other.transform.position);
        perseguindoPlayer = true;
    }
    private void OnTriggerExit(Collider other)
    {
        perseguindoPlayer = false;
    }
}

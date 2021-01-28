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

    private float velocidade;
    private bool recarregado;
    private int vida;
    private int maxVida;
    private GameObject player;
    private bool podeAtacar;
    private bool move;
    private NavMeshAgent navMeshAgent;
    private bool perseguindoPlayer;
    private float cooldownAtaque;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        perseguindoPlayer = false;
    }
    private void Update()
    {
    }
    private IEnumerator Atacar()
    {
        podeAtacar = false;

        cooldownAtaque = velocidadeAtaque;

        Coroutine mira = StartCoroutine(Mirar());

        yield return new WaitForSeconds(1f);

        //anim.SetTrigger("Atirar");
        //audioSource.PlayOneShot(tiro);
        //anim.SetBool("Atacar", false);
        StopCoroutine(mira);
        //anim.ResetTrigger("Atirar");
        GameObject balaInstanciada = Instantiate(tiro, pontoTiro.position, pontoTiro.transform.rotation);
        balaInstanciada.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
        Destroy(balaInstanciada, 10f);
        //balaInstanciada.GetComponent<Bala>().SetCasterCollider(this.GetComponent<Collider>());

        yield return new WaitForSeconds(velocidadeAtaque);
        podeAtacar = true;
    }
    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            perseguindoPlayer = true;

            float distancia = Vector3.Distance(collider.gameObject.transform.position, gameObject.transform.position);

            //anim.SetBool("Idle", false);

            //move = !inAttack;
            //goHome = false;

            if (distancia <= distanciaAtaque)
            {
                move = false;
                if (podeAtacar)
                    StartCoroutine(Atacar());
                else
                {
                    if (!podeAtacar)
                        cooldownAtaque -= velocidadeAtaque;
                    if (cooldownAtaque <= 0)
                        podeAtacar = true;
                }
            }
        }

        if (perseguindoPlayer)
        {
            if (Vector3.Distance(this.transform.position, player.transform.position) <= 4)
            {
                navMeshAgent.isStopped = true;
                if (podeAtacar)
                {
                    podeAtacar = false;
                    StartCoroutine(Atacar());
                }
                perseguindoPlayer = false;
            }
        }

    }
    private void Movimentar(Vector3 destino, bool move = true)
    {
        if (!move && navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;
            return;
        }

        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(destino);
    }
    IEnumerator Mirar()
    {
        navMeshAgent.isStopped = true;

        while (true)
        {
            move = false;
            this.transform.LookAt(player.transform);
            yield return null;
        }
    }
    private void Morrer()
    {
        
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

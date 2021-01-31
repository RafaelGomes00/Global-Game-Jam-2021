using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float velocidadeMira;
    [SerializeField] private float velocidadeReal;
    [SerializeField] private GameObject tiro;
    [SerializeField] private Transform pontoTiro;
    [SerializeField] private Transform pontoRotacao;
    [SerializeField] private float velocidadeDash;
    [SerializeField] private float tempoDash;
    [SerializeField] private LayerMask dashLayers;
    [SerializeField] private float fatorGasteTocha;
    [SerializeField] private Image referenciaImagemTocha;
    [SerializeField] private Image referenciaImagemVida;
    [SerializeField] private GameObject referenciaTocha;
    [SerializeField] private Light pointLight;
    [SerializeField] private int maxVida;
    [SerializeField] private Image danoFeedBack;
    [SerializeField] private Image morrendoFeedBack;
    [SerializeField] private Animator animator;

    private float tempoRestante;
    private float velocidade;
    private Vector3 RotacaoInput;
    private Vector3 VelocidadeRotacao;
    private Camera camera;
    private bool recarregado;
    private float animX;
    private float animY;
    private Vector3 clampedDir;
    private int tempoTocha;
    private int vida;
    private Rigidbody rb;
    public bool paused;

    void Start()
    {
        RecarregarTocha(100);
        camera = FindObjectOfType<Camera>();
        recarregado = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        rb = GetComponent<Rigidbody>();
        vida = maxVida;
        referenciaImagemVida.fillAmount = vida / maxVida;
        danoFeedBack.CrossFadeAlpha(0, 0f, true);
        morrendoFeedBack.CrossFadeAlpha(0f, 0f, true);
        velocidade = 0;
        RotacaoInput = new Vector3();
        VelocidadeRotacao = new Vector3();
    }
    void Update()
    {
        Rotacao();
        Movimento();
        Tiro();
        #region Dash
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }
        #endregion
        #region Tocha
        if (tempoRestante <= 0)
        {
            referenciaTocha.SetActive(false);
        }
        else
        {
            tempoRestante -= Time.deltaTime * fatorGasteTocha;
            referenciaImagemTocha.fillAmount = tempoRestante / tempoTocha;
        }
            pointLight.intensity = tempoRestante / tempoTocha;
        #endregion
    }
    public void TomarDano(int dano)
    {
        StartCoroutine(feedBackDano());
        vida = vida - dano;
        referenciaImagemVida.fillAmount = (float)vida / maxVida;
        if (vida <= maxVida * 0.3f) 
        {
            morrendoFeedBack.CrossFadeAlpha(1, 0.5f, true);
        }
        if (vida <= 0)
        {
            animator.SetTrigger("Morrer");
            Morrer();
        }
    }
    IEnumerator feedBackDano()
    {
        float tempo = 0.2f;

        danoFeedBack.CrossFadeAlpha(1, tempo, true);
        yield return new WaitForSeconds(tempo);
        danoFeedBack.CrossFadeAlpha(0, tempo, true);
    }
    void Movimento()
    {
        if (!paused)
        {
            float yAxis = Input.GetAxis("Vertical");
            float xAxis = Input.GetAxis("Horizontal");

            float y = Mathf.Lerp(animY, yAxis, 0.4f);
            float x = Mathf.Lerp(animX, xAxis, 0.4f);

            Vector3 dir = (this.transform.forward * y) + (this.transform.right * x);
            clampedDir = dir;

            if (dir.magnitude > 1)
                clampedDir = dir.normalized;

            rb.velocity = clampedDir * velocidadeReal * Time.deltaTime;
            //transform.position += (clampedDir * velocidade) * Time.deltaTime;
            animator.SetBool("Correr", true);
            animX = x;
            animY = y;
            if(xAxis == 0 && yAxis == 0)
            {
                rb.velocity = new Vector3();
                animator.SetBool("Correr", false);
            }
        }
    }
    void Rotacao()
    {
        if (!paused)
        {
            RotacaoInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            VelocidadeRotacao = RotacaoInput * velocidade;

            Ray cameraRay = camera.ScreenPointToRay(Input.mousePosition);
            Plane planoChao = new Plane(Vector3.up, Vector3.zero);
            float tamanhoRaio;

            if (planoChao.Raycast(cameraRay, out tamanhoRaio))
            {
                Vector3 pontoRotacao = cameraRay.GetPoint(tamanhoRaio);
                transform.LookAt(new Vector3(pontoRotacao.x, transform.position.y, pontoRotacao.z));
            }
        }
    }
    void Tiro()
    {
        if (!paused)
        {
            if (Input.GetMouseButton(1))
            {
                animator.SetBool("Mirar", true);
                velocidade = velocidadeMira;
                Cursor.visible = true;
            }
            else
            {
                animator.SetBool("Mirar", false);
                velocidade = velocidadeReal;
                Cursor.visible = false;
            }

            if (Input.GetMouseButtonDown(0) && recarregado)
            {
                animator.SetTrigger("Atacar");
                GameObject bala = Instantiate(tiro, pontoTiro.position, this.transform.rotation);
                bala.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
                recarregado = false;
                Destroy(bala, 10f);
            }
            else if (!recarregado && Input.GetKeyDown(KeyCode.R))
            {
                animator.SetTrigger("Recarregar");
                Debug.Log("Reccaregei");
                recarregado = true;
            }
        }
    }
    IEnumerator Dash()
    {
        if (!paused)
        {
            float inicioDash = Time.time;
            animator.SetTrigger("Rolar");
            //RaycastHit hit;
            while (Time.time < inicioDash + tempoDash)
            {
                rb.velocity = (clampedDir * velocidadeDash) * Time.deltaTime;
                Debug.Log("nao saiu");
                yield return null;
            }
        }
    }
    private void Morrer()
    {
        Debug.Log("GameOver");
    }
    public void RecarregarTocha(int tempoTocha)
    {
        tempoRestante = tempoTocha;
        referenciaTocha.SetActive(true);
        this.tempoTocha = tempoTocha;
    }
}

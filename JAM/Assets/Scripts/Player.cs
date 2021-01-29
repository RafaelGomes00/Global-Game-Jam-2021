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

            Vector3 dir = (pontoRotacao.forward * y) + (pontoRotacao.right * x);
            clampedDir = dir;

            if (dir.magnitude > 1)
                clampedDir = dir.normalized;

            rb.velocity = clampedDir * velocidadeReal * Time.deltaTime;
            //transform.position += (clampedDir * velocidade) * Time.deltaTime;

            animX = x;
            animY = y;
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
                velocidade = velocidadeMira;
                Cursor.visible = true;
            }
            else
            {
                velocidade = velocidadeReal;
                Cursor.visible = false;
            }

            if (Input.GetMouseButtonDown(0) && recarregado)
            {
                GameObject bala = Instantiate(tiro, pontoTiro.position, Quaternion.identity);
                bala.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
                recarregado = false;
                Destroy(bala, 10f);
            }
            else if (!recarregado && Input.GetKeyDown(KeyCode.R))
            {
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

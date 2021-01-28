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
    [SerializeField] private GameObject referenciaTocha;
    [SerializeField] private Light pointLight;

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
    private int maxVida;

    void Start()
    {
        RecarregarTocha(100);
        camera = FindObjectOfType<Camera>();
        recarregado = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
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

    void TomarDano(int danoRecebido)
    {
        vida = vida - danoRecebido;
        if(vida <= 0)
        {
            //Morreu;
        }
    }
    void Movimento()
    {
        float yAxis = Input.GetAxis("Vertical");
        float xAxis = Input.GetAxis("Horizontal");

        float y = Mathf.Lerp(animY, yAxis, 0.4f);
        float x = Mathf.Lerp(animX, xAxis, 0.4f);

        Vector3 dir = (pontoRotacao.forward * y) + (pontoRotacao.right * x);
        clampedDir = dir;

        if (dir.magnitude > 1)
            clampedDir = dir.normalized;

        transform.position += (clampedDir * velocidade) * Time.deltaTime;

        animX = x;
        animY = y;
    }
    void Rotacao()
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
    void Tiro()
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
            bala.GetComponent<Rigidbody>().AddForce(transform.forward * 20 , ForceMode.Impulse);
            recarregado = false;
            Destroy(bala, 10f);
        }
        else if (!recarregado && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Reccaregei");
            recarregado = true;
        }
    }
    IEnumerator Dash()
    {
        float inicioDash = Time.time;
        //RaycastHit hit;
        while (Time.time < inicioDash + tempoDash)
        {
            /*if (Physics.Raycast(new Vector3(pontoTiro.position.x, pontoTiro.position.y, pontoTiro.position.z), clampedDir, out hit, 10f, dashLayers) && Vector3.Distance(hit.point, this.transform.position) <= 1f)
            {
                Debug.Log(hit.collider.name);
                yield break;
            }
            else
            {*/
            transform.position += (clampedDir * velocidadeDash) * Time.deltaTime;
            //}
            //Debug.Log(Vector3.Distance(hit.point, this.transform.position) <= .1f);
            /*if (!colidindo)
            {
                Debug.Log(colidindo);
                transform.position += (clampedDir * velocidadeDash) * Time.deltaTime;
            }*/
            Debug.Log("nao saiu");
            yield return null;
        }
    }
    public void RecarregarTocha(int tempoTocha)
    {
        tempoRestante = tempoTocha;
        referenciaTocha.SetActive(true);
        this.tempoTocha = tempoTocha;
    }
}

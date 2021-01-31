using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{
    [SerializeField] private int gridX;
    [SerializeField] private int gridZ;
    [SerializeField] private float espacamentoGrid;
    [SerializeField] private GameObject objetoChao;
    [SerializeField] private GameObject objetoPonte;
    [SerializeField] private Transform parentescoHierarquiaChao;
    [SerializeField] private Transform parentescoHierarquiaPontes;
    [SerializeField] private Transform parentescoHierarquiaTochas;
    [SerializeField] private Transform parentescoHierarquiaChaves;
    [SerializeField] private GameObject[] tochas;
    [SerializeField] private GameObject alcaPao;
    [SerializeField] private GameObject chave;
    

    private Vector3[] vertices;
    private int[,] matrizGerada;
    private int[,] matrizGrafo;
    private int[,] matrizInt;
    private bool temAlcapao;
    private int quantidadeChaves;
    private bool temSpawn;
    private Vector3 spawn;
    private int indiceMaior;

    private void Start()
    {
        matrizGerada = this.GetComponent<SimplexNoiseScript>().gerarMatriz(gridX, gridZ);
        quantidadeChaves = 0;
        temAlcapao = false;
        temSpawn = false;

        GerarChao();

    }
    private void GerarChao()
    {
        int[] posicaoAlcapao = new int[2];
        int[] posicaoSpawn = new int[2];
        matrizInt = new int[gridX, gridZ];
        Grafo(matrizInt);

        #region criarChao
        while (!temAlcapao || !temSpawn || quantidadeChaves < 3)
        {
            for (int x = 0; x < gridX; x++)
            {
                for (int z = 0; z < gridZ; z++)
                {
                    Vector3 vertice = new Vector3(x * espacamentoGrid, 0, z * espacamentoGrid) + Vector3.zero;
                    if (matrizGrafo[x, z] == indiceMaior)
                    {

                        if (!temAlcapao && Random.Range(0, 4) == 2)
                        {
                            Instantiate(alcaPao, vertice, Quaternion.identity);
                            temAlcapao = true;
                            posicaoAlcapao[0] = x;
                            posicaoAlcapao[1] = z;
                        }
                        else
                        {
                            if (!temSpawn && Random.Range(0, 4) == 2)
                            {
                                spawn = new Vector3(vertice.x, vertice.y + 0.5f, vertice.z);
                                temSpawn = true;

                                posicaoSpawn[0] = x;
                                posicaoSpawn[1] = z;
                            }
                            else
                            {
                                if (quantidadeChaves < 3 && Random.Range(0, 4) == 2)
                                {
                                    Instantiate(chave, vertice, new Quaternion(0, Random.Range(0, 360), 0, 0), parentescoHierarquiaChaves);
                                    quantidadeChaves++;
                                }
                            }

                            Instantiate(objetoChao, vertice, Quaternion.identity, parentescoHierarquiaChao);
                        }
                        Instantiate(tochas[Random.Range(0, tochas.Length)], vertice, new Quaternion(0, Random.Range(0, 360), 0, 0), parentescoHierarquiaTochas);
                    }
                }
            }
        }
        #endregion

        #region CriarPontes
        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                if (x < gridX - 1)
                {
                    if (matrizGrafo[x + 1, z] == indiceMaior && matrizGrafo[x, z] == indiceMaior)
                    {
                        // Ponte horizontal
                        Vector3 verticePonteHoriz = new Vector3(x * espacamentoGrid, 0, z * espacamentoGrid) + new Vector3(12.5f, 0, 0);
                        Instantiate(objetoPonte, verticePonteHoriz, Quaternion.identity, parentescoHierarquiaPontes);

                    }
                }
                if (z < gridZ - 1)
                {
                    if (matrizGrafo[x, z + 1] == indiceMaior && matrizGrafo[x, z] == indiceMaior)
                    {
                        // Ponte vertical
                        Vector3 verticePonteVert = new Vector3(x * espacamentoGrid, 0, z * espacamentoGrid) + new Vector3(0, 0, 12.5f);
                        Instantiate(objetoPonte, verticePonteVert, Quaternion.identity, parentescoHierarquiaPontes);
                    }
                }
            }
        }
        #endregion

        //Destroy(this.gameObject);
    }
    private void GerarChaves(Vector3 vertice)
    {
        
    }
    private void Grafo(int[,] matrizInt)
    {
        matrizGrafo = new int[gridX, gridZ];

        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                if (matrizGerada[x, z] == 1 || matrizGerada[x,z] == 7 || matrizGerada[x,z] == 4)
                {
                    matrizGrafo[x, z] = -1;
                }
                else
                {
                    matrizGrafo[x, z] = 0;
                }
            }
        }

        int auxiliarContagem = 1;

        for (int xCont = 0; xCont < gridX; xCont++)
        {
            for (int zCont = 0; zCont < gridZ; zCont++)
            {
                if (matrizGrafo[xCont, zCont] == 0)
                {
                    GrafoRecursivo(xCont, zCont, ref auxiliarContagem);
                    auxiliarContagem++;
                }
            }
        }

        int[] vetorContagem = new int[auxiliarContagem];

        for (int i = 0; i < vetorContagem.Length; i++)
        {
            vetorContagem[i] = 0;
        }

        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                if (matrizGrafo[x,z] > 0)
                vetorContagem[matrizGrafo[x,z]-1]++;
            }
        }

        int maiorNumero = vetorContagem[0];
        indiceMaior = 0;

        for (int i = 0; i < vetorContagem.Length; i++)
        {
            if (vetorContagem[i] > maiorNumero)
            {
                maiorNumero = vetorContagem[i];
                indiceMaior = i;
            }
        }
        indiceMaior++;
    }
    private void GrafoRecursivo(int x, int z, ref int auxContagem)
    {
       if (matrizGrafo[x,z] == 0)
       {
            //matrizGrafo[x, z] = -1;
            matrizGrafo[x, z] = auxContagem;

            if (x < gridX - 1) // direita
            {
                GrafoRecursivo(x + 1, z, ref auxContagem);
            }
            if (x > 0) // esquerda
            {
                GrafoRecursivo(x - 1, z, ref auxContagem);
            }
            if (z < gridZ - 1) // cima
            {
                GrafoRecursivo(x , z + 1, ref auxContagem);
            }
            if (z > 0) // baixo
            {
                GrafoRecursivo(x, z - 1, ref auxContagem);
            }
       }
    }
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(spawn, 4f);
    }
}

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
    [SerializeField] LayerMask layer;
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
    GameObject[] pontes;
    private Vector3 spawn;
    private int indiceMaior;

    private void Start()
    {
        matrizGerada = this.GetComponent<SimplexNoiseScript>().gerarMatriz(gridX, gridZ);
        GerarChao();
        quantidadeChaves = 0;
    }
    private void GerarChao()
    {
        matrizInt = new int[gridX, gridZ];
        Grafo(matrizInt);

        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                Vector3 vertice = new Vector3(x * espacamentoGrid, 0, z * espacamentoGrid) + Vector3.zero;

                if (matrizGrafo[x,z] == indiceMaior)
                {
                    Instantiate(objetoChao, vertice, Quaternion.identity, parentescoHierarquiaChao);
                }
            }
        }

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

        #region CriarChao

        for (int i = 0; i < 30; i++)
        {
            if (temAlcapao && quantidadeChaves == 3 && temSpawn)
            {
                break;
            }

            for (int x = 0; x < gridX; x++)
            {
                for (int z = 0; z < gridZ; z++)
                {

                }
            }
        }

        #endregion
        Destroy(this.gameObject);
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

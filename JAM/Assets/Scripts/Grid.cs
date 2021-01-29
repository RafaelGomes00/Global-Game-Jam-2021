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

    private Vector3[] vertices;
    private void Start()
    {
        GerarChao();
        GerarPonte();
    }
    private void GerarChao()
    {
        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                Vector3 vertice = new Vector3(x * espacamentoGrid, 0, z * espacamentoGrid) + Vector3.zero;
                Instantiate(objetoChao, vertice, Quaternion.identity);
            }
        }
    }
    private void GerarPonte()
    {
        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ-1; z++)
            {
                Vector3 verticeChao = new Vector3(x * espacamentoGrid, 0, z * espacamentoGrid) + new Vector3(0,0,12.5f);
                Instantiate(objetoPonte, verticeChao, Quaternion.identity);

                Vector3 verticePonte = new Vector3(z * espacamentoGrid, 0, x * espacamentoGrid) + new Vector3(12.5f, 0, 0);
                Instantiate(objetoPonte, verticePonte, Quaternion.identity);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Threading;
using System.IO;
using System;

public class SincHilos : MonoBehaviour
{
    //Declaración de la variables (Objetos de la escena)
    public GameObject player;
    public Transform entrance;
    NavMeshTriangulation triangulation;

    //Valores de referencia
    float result = 0f;
    private Vector3 entrancePos;
    public List<GameObject> npcs = new List<GameObject>();
    private Vector3 playerPosUpdate = new Vector3();
    public List<Vector3> newPosNPC = new List<Vector3>();
    public List<Vector3> possiblePositions = new List<Vector3>();

    //Valores de repetición
    public int numPosiblePositions = 10000;
    public float detectionRange = 2f;
    public float minDistanceFromEntrance = 20f;

    //Elementos para hilos
    System.Random rand = new System.Random();
    private Thread hiloCalculo;
    private Thread hiloLeerArchivo;
    private static readonly object filelock = new object();
    private static ManualResetEvent writeComplete = new ManualResetEvent(false);

    void Start()
    {
        playerPosUpdate = player.transform.position;
        entrancePos = entrance.transform.position;
        FindAllNPC();
        triangulation = NavMesh.CalculateTriangulation();
    }

    void Update()
    {
        playerPosUpdate = player.transform.position;
        NavMeshAgent agent = this.GetComponent<NavMeshAgent>();

        float distanceToPlayer = Vector3.Distance(this.transform.position, playerPosUpdate);
        if (distanceToPlayer < detectionRange)
        {
            TeleportToEntrance();
            RelocateAllNPC();
            PutNewNPCPos();

        }

        UpdateNPCPath(agent, playerPosUpdate);

    }

    //Función que encuentra a todos los objetos que considera NPC
    void FindAllNPC()
    {
        npcs.Clear();

        var npcObject = FindObjectsOfType<NavMeshAgent>();

        foreach (var npc in npcObject)
        {
            if (npc.gameObject.CompareTag("Enemy"))
            {
                npcs.Add(npc.gameObject);
            }
        }
    }

    void TeleportToEntrance()
    {
        player.transform.position = entrancePos;
        Debug.Log("Player en la entrada");
    }

    void RelocateAllNPC()
    {
        GeneratePossiblePositions();
        ShuffleList(possiblePositions);
        newPosNPC.Clear();
        FindBestPositionForNPC();

        //Hilo Secundario para leer
        hiloLeerArchivo = new Thread(ReadToFile);
        hiloLeerArchivo.Start();
    }

    void GeneratePossiblePositions()
    {
        possiblePositions.Clear();

        if (triangulation.vertices.Length == 0)
        {
            Debug.Log("Error en NavMeshSurface");
            return;
        }

        //Generar posibles posiciones aleatorias dentro de NavMesh
        for (int i = 0; i < numPosiblePositions; i++)
        {
            Vector3 randomPosition = GetRandomPositionWithinNavMesh();

            //Verificar si esa nueva posición cumple la distancia a la entrada

            float aux = Vector3.Distance(randomPosition, entrancePos);

            if (aux >= minDistanceFromEntrance)
            {
                possiblePositions.Add(randomPosition);
            }
        }
    }

    Vector3 GetRandomPositionWithinNavMesh()
    {
        //vector de posición dentro del triángulo

        Vector3 randomPosition;

        //Selección de un triángulo posible dentro del total de triángulos del área

        int triangleIndex = rand.Next(0, triangulation.areas.Length);

        Vector3 v1 = triangulation.vertices[triangulation.indices[triangleIndex * 3]];
        Vector3 v2 = triangulation.vertices[triangulation.indices[triangleIndex * 3 + 1]];
        Vector3 v3 = triangulation.vertices[triangulation.indices[triangleIndex * 3 + 2]];

        float r1 = (float)rand.NextDouble();
        float r2 = (float)rand.NextDouble();

        if (r1 + r2 > 1)
        {
            r1 = 1 - r1;
            r2 = 1 - r2;
        }

        //v1 es vertice de referencia
        //r1 * (v2 - v1) es el vector escalado desde v1 a v2
        //r2 * (v3 - v1) es el vector escalado desde v1 a v3
        randomPosition = v1 + r1 * (v2 - v1) + r2 * (v3 - v1);

        return randomPosition;
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int r = rand.Next(0, i + 1);
            T temp = list[i];
            list[i] = list[r];
            list[r] = temp;
        }
    }

    void FindBestPositionForNPC()
    {
        for (int i = 0; i < npcs.Count; i++)
        {
            newPosNPC.Add(possiblePositions[rand.Next(0, npcs.Count)]);
            //Hilo Secundario
            hiloCalculo = new Thread(PerfomHeavyComputation);
            hiloCalculo.Start();
        }
    }

    void PerfomHeavyComputation()
    {
        result = 0;

        for (int i = 1; i < numPosiblePositions; i++)
        {
            result += ComputeComplex(i);
        }

        GenerateResult(result);
        writeComplete.Set();
    }

    void GenerateResult(float valor)
    {
        string path = Application.dataPath + "/ResultadoSincronizado.txt";

        lock(filelock)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, valor.ToString());
        }
    }

    float ComputeComplex(int x)
    {
        float resultInx = 0;

        for (int i = 1; i <= 100; i++)
        {
            resultInx += (float)((System.Math.Pow(x, i) * System.Math.Sin(x) / (i + 1)) - System.Math.Sqrt(x + i));
        }

        Debug.LogWarning(resultInx);

        return resultInx;
    }

    void PutNewNPCPos()
    {
        for (int i = 0; i < npcs.Count; i++)
        {
            npcs[i].GetComponent<NavMeshAgent>().enabled = false;
            npcs[i].transform.position = newPosNPC[i];
            npcs[i].GetComponent<NavMeshAgent>().enabled = true;
        }
    }

    void UpdateNPCPath(NavMeshAgent agent, Vector3 playerPosUpdate)
    {
        agent.SetDestination(playerPosUpdate);
    }

    private void ReadToFile()
    {
        writeComplete.WaitOne();
        try
        {
            string path = Application.dataPath + "/ResultadoSincronizado.txt";
            Debug.Log(File.ReadAllLines(path).ToString());
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    private void OnApplicationQuit()
    {
        if (hiloCalculo != null && hiloCalculo.IsAlive)
        {
            hiloCalculo.Join(); //Le dices al segundo hilo que se una al primer hilo
        }

        if (hiloLeerArchivo != null && hiloLeerArchivo.IsAlive)
        {
            hiloLeerArchivo.Join(); //Le dices al hilo de lectura que se una al primer hilo
        }
    }
}
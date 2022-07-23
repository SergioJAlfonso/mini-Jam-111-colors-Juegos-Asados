using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum direccion { Up, Down, Left, Right };

public class Tablero : MonoBehaviour
{
    [SerializeField]
    GameObject tile;
    //[SerializeField]
    int x;
    [SerializeField]
    float margin;
    [SerializeField]
    TextAsset txtLvl;
    // Start is called before the first frame update

    //colocar los nodos e indicies, lo que utilizaria la ia para moverse por turnos.
    nodo[] indexes;
    Tile[] tiles;

    System.Random rand;
    //la estructura del nodo
    public struct nodo
    {
        //indices adyadcentes
        public int up, left, right, down;
        public bool transitable;
        // public int coste;
    }

    private struct vertex
    {
        public int index;
        public float distancia;
        public vertex(int v_, float c)
        {
            index = v_;
            distancia = c;
        }
    }

    void Start()
    {
        rand = new System.Random();
        byte[] by = txtLvl.bytes;
        int z = 0;
        int tam = 0;
        while (Convert.ToChar(by[z]) != '\n')
        {
            if (by[z] >= 48)
            {
                tam *= 10;
                tam += by[z] - 48;
            }
            z++;
        }
        x = tam;

        indexes = new nodo[x * x];
        tiles = new Tile[x * x];

        Vector3 pos = new Vector3(0, 0, 0);
        Vector3 sc = tile.transform.localScale;
        pos.x = -sc.x * (x / 2.0f) + sc.x / 2 - margin * ((x - 1) / 2.0f);
        for (int i = 0; i < x; i++)
        {
            pos.z = -sc.z * (x / 2.0f) + sc.y / 2 - margin * ((x - 1) / 2.0f);
            for (int j = 0; j < x; j++)
            {
                z++;
                //asignación de indices
                int id = i * x + j;

                var a = Instantiate(tile, pos, Quaternion.identity, this.transform);
                a.AddComponent<BoxCollider>();
                var b = a.AddComponent<Tile>();
                tiles[id] = b;
                b.index = id;
                b.table = this;

                pos.z += sc.z + margin;


                indexes[id].left = id - 1;
                indexes[id].right = id + 1;
                if ((id) % x == 0)
                {
                    indexes[id].left = -1;
                }
                else if ((id) % x == x - 1)
                {
                    indexes[id].right = -1;
                }

                indexes[id].down = id + x;
                indexes[id].up = id - x;

                if (id < x)
                {
                    indexes[id].up = -1;
                }
                else if (id >= x * (x - 1))
                {
                    indexes[id].down = -1;
                }

                ProcesNumber(by[z], id);
            }
            pos.x += sc.x + margin;
            while (z < by.Length && Convert.ToChar(by[z]) != '\n')
                z++;
        }

        transform.GetChild(x-1).gameObject.AddComponent<EndTile>();
        Instantiate(Resources.Load("HeavenDeco") as GameObject, transform.GetChild(x - 1).transform);
        transform.GetChild(transform.childCount - x).gameObject.AddComponent<EndTile>().heaven = false;
        Instantiate(Resources.Load("HellDeco") as GameObject, transform.GetChild(transform.childCount - x).transform);
    }


    public direccion DjistraAStarFromTo(int ori, int dest)
    {
        //coordenadas de destino para calcular la siguiente casilla mas cercana e ir tirando por ahi el algoritmo
        Vector2 destCord = IndexToCoord(dest);

        int v = ori;
        List<vertex> q = new List<vertex>();

        int[] previous = new int[indexes.Length];
        for (int i = 0; i < previous.Length; i++)
            previous[i] = -1;
        previous[v] = v;

        q.Add(new vertex(v,0));

        while (q.Count != 0)
        {
            v = q[0].index;
            q.RemoveAt(0);

            //Si src (v) == dst, construimos el camino          //Si el nodo actual es el de destino construimos el camino
            if (v == dest)
            {
                //recorremos la lista de previus hasta llenar a nuestra origen;
                int ant = dest;
                v = previous[v];
                while (previous[v] != ori)
                {
                    ant = v;
                    v = previous[v];
                }
                //tenemos los dos ultimos y queda calcular cual iba a ser el movimiento, Up Down...
                int[] neigh = { indexes[v].up, indexes[v].down, indexes[v].left, indexes[v].right };

                for (int i = 0; i < neigh.Length; i++)
                {
                    if (neigh[i] == ant)
                    {
                        return (direccion)i;
                    }
                }

            }


            //meter los vecinos de v en q
            int[] neighbours = { indexes[v].up, indexes[v].left, indexes[v].down, indexes[v].right};

            foreach (int n in neighbours)
            {
                if (n != -1)
                {
                    if ( previous[n] != -1)
                        continue;
                    previous[n] = v; // El vecino n tiene de 'padre' a v

                    q.Add(new vertex(n, IndexDist(n,dest)));
                    //ordenar por distancia a objetivo
                    q.Sort(SortByScore);
                }
            }
            

        }

        //cuadno haya terminao si es que termina, una direcciion no valida
        return new direccion();

    }

    private int SortByScore(vertex x, vertex y)
    {
        return x.distancia.CompareTo(y.distancia);
    }

    float IndexDist(int ori, int dest)
    {
        Vector3 posA = tiles[ori].transform.position;
        Vector3 posB = tiles[dest].transform.position;
        return Vector3.Distance(posA, posB);
    }


    public int ExitIndex(bool heaven)
    {
        if (heaven) return 0;
        else return transform.childCount - 1;
    }

    public Vector2 IndexToCoord(int index)
    {
        Vector2 coord;

        coord.x = (int)(index % x);
        coord.y = (int)(index / x);

        return coord;
    }

    public int CoordToIndex(Vector2 coord)
    {
        return (int)(coord.x + (coord.y * x));
    }

    public Tile getTileByIndex(int index)
    {
        return tiles[index];
    }
    public bool TransitableTile(int index)
    {
        return indexes[index].transitable;
    }
    public int NextMove(int index, direccion dir)
    {
        int next;

        switch (dir)
        {
            case direccion.Up:
                next = indexes[index].up;
                break;
            case direccion.Down:
                next = indexes[index].down;
                break;
            case direccion.Left:
                next = indexes[index].left;
                break;
            case direccion.Right:
                next = indexes[index].right;
                break;
            default:
                next = -1;
                break;
        }

        return next;
    }

    void ProcesNumber(byte b, int id)
    {
        char x = Convert.ToChar(b);
        if (x == '0')
        {
            indexes[id].transitable = true;
        }
        else
        {
            indexes[id].transitable = false;
            string route = "../Prefabs/";
            int obsSize = 0;
            switch (x)
            {
                //Modificar obsSize con los prefabs de cada carpeta
                case '1':
                    route += "Obs1/";

                    route += rand.Next(0, obsSize);
                    break;
                case '2':
                    route += "Obs2/";

                    route += rand.Next(0, obsSize);
                    break;
            }
            if(obsSize != 0)
                Instantiate(Resources.Load(route) as GameObject, transform.GetChild(id).transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

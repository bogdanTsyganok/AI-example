using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

public class BMPLoader : MonoBehaviour
{
    public GameObject mapBlockPrefab;
    public GameObject gathererPrefab;
    public List<Material> materials;
    public string mapname;

    enum colourSquare { White, Yellow, Black, Green, Red, Blue};
    enum gathererState { Idle, Search, MoveGather, Gather, MoveReturn, Return}

    List<List<colourSquare>> bmpNodes = new List<List<colourSquare>>();
    List<List<GameObject>> cubes = new List<List<GameObject>>();
    Graph graph;
    Node depository;

    class Gatherer
    {
        public GameObject gathererGO { get; set; }
        public gathererState state { get; set; }
        public float waitTimer { get; set; }
        public Node curNode { get; set; }
        public Node nextNode { get; set; }
        public bool hasResource { get; set; }
        public Node goalNode { get; set; }
        public Stack<Node> path = new Stack<Node>();
        public void buildPath()
        {
            this.nextNode = this.goalNode;
            while (this.nextNode.parent != this.curNode)
            {
                this.path.Push(this.nextNode);
                this.nextNode = this.nextNode.parent;
            }
        }
    }
    List<Gatherer> gatherers = new List<Gatherer>();



    Bitmap image1;
    // Start is called before the first frame update
    void Start()
    {        
        image1 = (Bitmap)Image.FromFile("Assets/Maps/" + mapname + ".bmp");

        System.Drawing.Color pixelColour = image1.GetPixel(1, 1);
        // Debug.Log(pixelColour.R + " " + pixelColour.G + " " + pixelColour.B);
        graph = new Graph();
        SetUpArea();
        //SetUpGraph();
        //Debug.Log("Stuff");

    }

    // Update is called once per frame
    void Update()
    {
        foreach(Gatherer scav in gatherers)
        {
            
        }
    }

    //void SetUpGraph()
    //{
    //    int row = 0, column = 0;
    //    List<List<Node>> graphNodeMatrix = new List<List<Node>>();
    //    foreach (List<colourSquare> nodeRow in bmpNodes)
    //    {
    //        graphNodeMatrix.Add(new List<Node>());
    //        foreach(colourSquare node in nodeRow)
    //        {
    //            string name = "" + column + row; 
    //            switch (node)
    //            {
    //                case colourSquare.Black:
    //                    graphNodeMatrix[row].Add(null);
    //                    break;
    //                case colourSquare.Blue:
    //                    depository = graph.CreateNode(name, new Vector3(row, 2, column), false);
    //                    graphNodeMatrix[row].Add(depository);
    //                    break;

    //                case colourSquare.Red:
    //                    Node tempNode = graph.CreateNode(name, new Vector3(row, 2, column), true);
    //                    graphNodeMatrix[row].Add(tempNode);
    //                    break;
    //                case colourSquare.Green:
    //                    GameObject collector = Instantiate(gathererPrefab);
    //                    collector.transform.position = new Vector3(row, 2, column);
    //                    Gatherer gatherer = new Gatherer();
    //                    gatherer.gathererGO = collector;
    //                    gatherer.waitTimer = 0;
    //                    gatherer.state = gathererState.Idle;
    //                    gatherer.nextNode = null;

    //                    gatherer.curNode = graph.CreateNode(name, new Vector3(row, 2, column), false);
    //                    graphNodeMatrix[row].Add(gatherer.curNode);

    //                    gatherers.Add(gatherer);
    //                    break;
    //                default:
    //                    //create an empty node
    //                    graphNodeMatrix[row].Add(graph.CreateNode(name, new Vector3(row, 2, column), false));
    //                    break;
    //            }
    //            column++;
    //        }
    //        row++;
    //        column = 0;
    //    }

    //    row = 0; column = 0;

    //    List<Vector2Int> offsets = new List<Vector2Int>();
    //    offsets.Add(new Vector2Int(-1, -1));
    //    offsets.Add(new Vector2Int(-1, 0));
    //    offsets.Add(new Vector2Int(-1, 1));
    //    offsets.Add(new Vector2Int(0, -1));
    //    offsets.Add(new Vector2Int(0, 1));
    //    offsets.Add(new Vector2Int(1, -1));
    //    offsets.Add(new Vector2Int(1, 0));
    //    offsets.Add(new Vector2Int(1, 1));

    //    float normalWeightS = 10;
    //    float normalWeightD = 14;
    //    float highWeightS = 14;
    //    float highWeightD = 28;

    //    foreach (List<Node> nodeRow in graphNodeMatrix)
    //    {
    //        column = 0;
    //        foreach (Node node in nodeRow)
    //        {

    //            if (node == null)
    //            {
    //                column++;
    //                continue;
    //            }
                
    //            foreach (Vector2Int offset in offsets)
    //            {
    //                int rowIndexOffset = row + offset.x; 
    //                int columntIndexOffset = column + offset.y;
    //                if (columntIndexOffset < 0 || columntIndexOffset >= graphNodeMatrix[0].Count)
    //                    continue;
    //                if (rowIndexOffset < 0 || rowIndexOffset >= graphNodeMatrix.Count)
    //                    continue;
    //                if(graphNodeMatrix[rowIndexOffset][columntIndexOffset] == null)
    //                {
    //                    continue;
    //                }
    //                if(bmpNodes[rowIndexOffset][columntIndexOffset] == colourSquare.Yellow)
    //                {
    //                    if (offset.magnitude > 1f)
    //                    {
    //                        if (graphNodeMatrix[row][columntIndexOffset] == null || graphNodeMatrix[rowIndexOffset][column] == null)
    //                            continue;
    //                        graph.AddEdge(graphNodeMatrix[row][column], graphNodeMatrix[rowIndexOffset][columntIndexOffset], highWeightD, false);
    //                    }
    //                    else
    //                    {
    //                        graph.AddEdge(graphNodeMatrix[row][column], graphNodeMatrix[rowIndexOffset][columntIndexOffset], highWeightS, false);
    //                    }
    //                }
    //                else
    //                {
    //                    if (offset.magnitude > 1f)
    //                    {
    //                        if (graphNodeMatrix[row][columntIndexOffset] == null || graphNodeMatrix[rowIndexOffset][column] == null)
    //                            continue;
    //                        graph.AddEdge(graphNodeMatrix[row][column], graphNodeMatrix[rowIndexOffset][columntIndexOffset], normalWeightD, false);
    //                    }
    //                    else
    //                    {
    //                        graph.AddEdge(graphNodeMatrix[row][column], graphNodeMatrix[rowIndexOffset][columntIndexOffset], normalWeightS, false);
    //                    }
    //                }

    //            }
    //            column++;
    //        }
    //        row++;
    //    }
    //}

    void SetUpArea()
    {
        for(int coordX = 0; coordX < image1.Width; coordX++)
        {
            bmpNodes.Add(new List<colourSquare>());
            cubes.Add(new List<GameObject>());
            for (int coordY = 0; coordY < image1.Height; coordY++)
            {
                GameObject mapBlockConcrete = Instantiate(mapBlockPrefab);
                
                System.Drawing.Color pixelColour = image1.GetPixel(coordX, coordY);
                if(pixelColour.R > 0)
                {
                    if(pixelColour.B > 0)
                    {//white
                        Material selectedMaterial = materials[0];
                        mapBlockConcrete.GetComponent<Renderer>().material = selectedMaterial;
                        bmpNodes[coordX].Add(colourSquare.White);
                    }
                    else if(pixelColour.G > 0)
                    {//yellow
                        Material selectedMaterial = materials[1];
                        mapBlockConcrete.GetComponent<Renderer>().material = selectedMaterial;
                        bmpNodes[coordX].Add(colourSquare.Yellow);
                    }
                    else
                    {//red
                        Material selectedMaterial = materials[4];
                        mapBlockConcrete.GetComponent<Renderer>().material = selectedMaterial;
                        bmpNodes[coordX].Add(colourSquare.Red);
                    }
                }
                else
                {
                    if(pixelColour.B > 0)
                    {//blue
                        Material selectedMaterial = materials[5];
                        mapBlockConcrete.GetComponent<Renderer>().material = selectedMaterial;
                        bmpNodes[coordX].Add(colourSquare.Blue);
                    }
                    else if(pixelColour.G > 0)
                    {//green
                        Material selectedMaterial = materials[3];
                        mapBlockConcrete.GetComponent<Renderer>().material = selectedMaterial;
                        bmpNodes[coordX].Add(colourSquare.Green);
                        GameObject collector = Instantiate(gathererPrefab);
                        collector.transform.position = new Vector3(coordX, 2, coordY);
                    }
                    else
                    {//black
                        Material selectedMaterial = materials[2];
                        mapBlockConcrete.GetComponent<Renderer>().material = selectedMaterial;
                        bmpNodes[coordX].Add(colourSquare.Black);
                    }
                }

                mapBlockConcrete.transform.position = new Vector3(coordX, 1, coordY);
                cubes[coordX].Add(mapBlockConcrete);
                if(pixelColour.G < 10)
                {
                    GameObject doubledBlackWall = Instantiate(mapBlockPrefab);
                    doubledBlackWall.transform.position = new Vector3(coordX, 2, coordY);
                }
            }
        }
    }
}

class Node
{

    public string id;
    public bool visited;
    public bool hasGoal;
    public float costSoFar;
    public float hDistance;
    public Vector3 position; //position in our game world. Used for calculating distances
    public Node parent = null; //parent Node that we can use to follow back to the root node
    public List<KeyValuePair<Node, float>> edges = new List<KeyValuePair<Node, float>>(); //Edges pointing to our neighbouring nodes <childNode, edgeWeight>
}

class Graph
{
    public List<Node> nodes = new List<Node>();  //all of the nodes inside my graph

    public const float MAXWEIGHT = 10000000000;

    public Node CreateNode(string id, Vector3 position, bool bHasGoal = false)
    {
        Node node = new Node();
        node.id = id;
        node.visited = false;
        node.costSoFar = MAXWEIGHT;
        node.hDistance = 0;
        node.position = position;
        node.parent = null;
        node.hasGoal = bHasGoal;

        this.nodes.Add(node);
        return node;
    }
    public void AddEdge(Node parent, Node child, float weight, bool bUndirected = true)
    {
        Node localParent = nodes.Find(x => x == parent);
        Node localChild = nodes.Find(x => x == child);
        KeyValuePair<Node, float> edge = new KeyValuePair<Node, float>(localChild, weight);
        localParent.edges.Add(edge);

        if (bUndirected)
        {
            KeyValuePair<Node, float> reverseEdge = new KeyValuePair<Node, float>(localParent, weight);
            localChild.edges.Add(reverseEdge);
        }
    }
    public void PrintGraph()
    {

    }
    public void ResetGraph()
    {
        foreach (Node currNode in this.nodes)
        {
            currNode.visited = false;
            currNode.parent = null;
            currNode.costSoFar = MAXWEIGHT;
        }
    }



    public Node GetLowestCostNode(List<Node> openList)
    {
        float minCost = MAXWEIGHT;
        Node lowestCostNode = null;
        //find the node with the lowest cost from the root node
        for (int index = 0; index < openList.Count; ++index)
        {
            if (openList[index].costSoFar < minCost)
            {
                minCost = openList[index].costSoFar;
                lowestCostNode = openList[index];
            }
        }
        return lowestCostNode;
    }

    public bool IsNodeInOpenList(List<Node> openList, Node neighbour)
    {
        for (int index = 0; index < openList.Count; ++index)
        {
            if (openList[index] == neighbour)
                return true;
        }
        return false;
    }

    public void PrintListContents(List<Node> myList)
    {
        string ids = "";
        for (int index = 0; index < myList.Count; ++index)
        {
            ids += myList[index].id + " ";
        }

        Debug.Log(ids);
    }



    public Node Dijkstra(Graph graph, Node rootNode)
    {
        graph.ResetGraph();

        rootNode.costSoFar = 0;
        List<Node> openList = new List<Node>();
        List<Node> closeList = new List<Node>();
        openList.Add(rootNode);

        while (openList.Count != 0)
        {
            Node currNode = GetLowestCostNode(openList);
            openList.Remove(currNode);
            closeList.Add(currNode);

            currNode.visited = true;
            if (currNode.hasGoal)
            {
                return currNode;
            }

            //Go through every neighbouring node of the current node
            foreach (KeyValuePair<Node, float> neighbour in currNode.edges)
            {
                if (neighbour.Key.visited == false)
                {
                    float weightSoFar = currNode.costSoFar + neighbour.Value;
                    if (weightSoFar < neighbour.Key.costSoFar)
                    {
                        neighbour.Key.costSoFar = weightSoFar;
                        neighbour.Key.parent = currNode;
                        if (!IsNodeInOpenList(openList, neighbour.Key))
                        {
                            openList.Add(neighbour.Key);
                        }
                    }
                }
            }
            //Debug.Log("Open List = ");
            //PrintListContents(openList);
            //Debug.Log("Close List = ");
            //PrintListContents(closeList);
        }
        return null;
    }

    public float CalculateHeuristicDistances(Node node, Node goal)
    {
        float D = 1;
        float dx = Mathf.Abs(node.position.x - goal.position.x);
        float dy = Mathf.Abs(node.position.y - goal.position.y);
        return D * (dx + dy);
    }


    public Node GetLowestFCostNode(List<Node> openList)
    {
        float minCost = MAXWEIGHT;
        Node lowestCostNode = null;
        //find the node with the lowest f cost
        for (int index = 0; index < openList.Count; ++index)
        {
            if (openList[index].costSoFar + openList[index].hDistance < minCost)
            {
                minCost = openList[index].costSoFar + openList[index].hDistance;
                lowestCostNode = openList[index];
            }
        }
        return lowestCostNode;
    }

    public Node AStar(Graph graph, Node rootNode, Node goal)
    {
        graph.ResetGraph();
        rootNode.costSoFar = 0;
        rootNode.hDistance = CalculateHeuristicDistances(rootNode, goal);

        List<Node> openList  = new List<Node>();
        List<Node> closeList = new List<Node>();
        openList.Add(rootNode);

        while (openList.Count != 0)
        {
            //remove the current node from the open list with the lowest f cost
            Node currNode = GetLowestFCostNode(openList);
            openList.Remove(currNode);
            closeList.Add(currNode);



            currNode.visited = true;
            if (currNode == goal)
            {
                return currNode;
            }

            //Go through each neighbouring node of current node
            foreach (KeyValuePair<Node, float> neighbour in currNode.edges)
            {
                if (neighbour.Key.visited == false)
                {
                    float weightSoFar = currNode.costSoFar + neighbour.Value;
                    if (weightSoFar < neighbour.Key.costSoFar)
                    {
                        neighbour.Key.costSoFar = weightSoFar;
                        neighbour.Key.parent = currNode;
                        if (!IsNodeInOpenList(openList, neighbour.Key))
                        {
                            neighbour.Key.hDistance = CalculateHeuristicDistances(neighbour.Key, goal);
                            openList.Add(neighbour.Key);
                        }
                    }
                }
            }
            //Debug.Log("Open List = ");
            //PrintListContents(openList);
            //Debug.Log("Close List = ");
            //PrintListContents(closeList);
        }
        return null;
    }
}
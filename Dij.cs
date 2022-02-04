namespace Kse.Algorithms.Samples
{
    public class Node
    {
        public Node Up { get; set; }
        public Node Down { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public Point Position { get; set; }
        public int TotalCost { get; set; }

        public Node(int cost, Point position)
        {
            Position = position;
            TotalCost = cost;
        }
    }

    public static class MazeSolver
    {
        public static Stack<Node> Dijkstra(string[,] Map, Point Start, Point End)
        {
            if (Map[Start.Column, Start.Row] == MapGenerator.Wall) return null;
            if (Map[End.Column, End.Row] == MapGenerator.Wall) return null;
            if (Start.Equals(End)) return null;

            Node StartNode = null;
            var AllNodes = new List<Node>();
            //Fill node list
            for (int y = 0; y < Map.GetLength(1); y++)
            {
                for (int x = 0; x < Map.GetLength(0); x++)
                {
                    Point current = new(x, y);
                    if (current.Equals(Start))
                    {
                        StartNode = new Node(0, current);
                        AllNodes.Add(StartNode);
                    }
                    if (Map[x, y] != MapGenerator.Wall) AllNodes.Add(new Node(int.MaxValue, current));
                }
            }

            CheckNodes(Map, StartNode, AllNodes); //Setup tree

            Stack<Node> path = new Stack<Node>();
            foreach (var node in AllNodes)
            {
                if (!node.Position.Equals(End)) continue;
                //Start from end and look for the least cost path
                path.Push(node);
                Node current = node;
                while (true)
                {
                    //Find neighbor with least cost
                    if (current.Position.Equals(Start)) break; //Exit if we reached start
                    int childMin = int.MaxValue;
                    Node next = current.Left;
                    if (current.Left != null && current.Left.TotalCost < childMin) {
                        childMin = current.Left.TotalCost;
                        next = current.Left;
                    }
                    if (current.Right != null && current.Right.TotalCost < childMin) {
                        childMin = current.Right.TotalCost;
                        next = current.Right;
                    }
                    if (current.Up != null && current.Up.TotalCost < childMin) {
                        childMin = current.Up.TotalCost;
                        next = current.Up;
                    }
                    if (current.Down != null && current.Down.TotalCost < childMin) {
                        next = current.Down;
                    }
                    
                    current = next;
                    path.Push(current);
                }

                break;
            }
                
            return path;
        }

        public static void CheckNodes(string[,] Map, Node NodeToCheck, List<Node> allNodes, int fromSide = -1)
        {
            if (NodeToCheck == null) return;

            var CurrentPos = NodeToCheck.Position;
            bool toCheck = false;

            Node CheckNode(Point newPos)
            {
                if (newPos.Column < 0 || newPos.Row < 0 || newPos.Column >= Map.GetLength(0) || newPos.Row >= Map.GetLength(1)) return null; //If out-of-bounds
                if (Map[newPos.Column, newPos.Row] == MapGenerator.Wall) return null; //Or a wall

                toCheck = true;
                foreach (var node in allNodes)
                {
                    if (node.Position.Equals(newPos))
                    {
                        var newCost = NodeToCheck.TotalCost + int.Parse(Map[newPos.Column, newPos.Row]);  //Add traffic value to current node
                        if (node.TotalCost > newCost) //Only set new value if it is less than current
                            node.TotalCost = newCost;
                        else toCheck = false;

                       return node;
                    }
                }
                return null;
            }

            //Check all 4 sides
            NodeToCheck.Left = CheckNode(new(CurrentPos.Column - 1, CurrentPos.Row)); 
            if (fromSide != 0 && toCheck) CheckNodes(Map, NodeToCheck.Left, allNodes, 2);
            NodeToCheck.Up = CheckNode(new(CurrentPos.Column, CurrentPos.Row - 1)); 
            if (fromSide != 1 && toCheck) CheckNodes(Map, NodeToCheck.Up, allNodes, 3);
            NodeToCheck.Right = CheckNode(new(CurrentPos.Column + 1, CurrentPos.Row)); 
            if (fromSide != 2 && toCheck) CheckNodes(Map, NodeToCheck.Right, allNodes, 0);
            NodeToCheck.Down = CheckNode(new(CurrentPos.Column, CurrentPos.Row + 1)); 
            if (fromSide != 3 && toCheck) CheckNodes(Map, NodeToCheck.Down, allNodes, 1);
        }
    }
}

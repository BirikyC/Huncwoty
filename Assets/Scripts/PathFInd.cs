using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class PathFInd
{

    class Node
    {
        public Vector2Int pos;
        public float hCost, gCost;
        public Node parent;

        public float fCost => hCost + gCost;

        public Node(Vector2Int pos, float hCost, float gCost, Node parent) 
        { 
            this.pos = pos;
            this.hCost = hCost;
            this.gCost = gCost;
            this.parent = parent;
        }
    }

    public static List<Vector2Int> FindPath(Vector2Int from, Vector2Int to, Tilemap t, GameObject o)
    {
        List<Node> open = new List<Node> { new Node(from, MannDist(from, to), 0, null) };
        List<Vector2Int> closed = new();

        Node[] buff;// = new Node[4];

        int max_dist = 10;

        BoundsInt bounds = t.cellBounds;

        while (open.Count > 0) 
        {
            int min_id = MinF(open);
            Node q = open[min_id];

            if (q.pos == to) return ReconstructPath(q);

            open.RemoveAt(min_id);

            buff = Successors(q, to);

            foreach (Node s in buff) 
            {
                //if ((s.pos - from).sqrMagnitude > 100)
                if (!bounds.Contains((Vector3Int)s.pos))
                    continue;
                if (IsBlocked(s.pos, t, o)) 
                    continue;
                if (closed.Contains(s.pos))
                    continue;
                /*if (!LowerF(open, s))
                    open.Add(s);*/
                int id = open.FindIndex(n => n.pos == s.pos);
                if (id == -1)
                    open.Add(s);
                else if (open[id].gCost > s.gCost)
                    open[id] = s;
            }

            closed.Add(q.pos);
        }

        return null;
    }

    static List<Vector2Int> ReconstructPath(Node goal)
    {
        var path = new List<Vector2Int>();
        Node current = goal;

        while (current != null)
        {
            path.Add(current.pos);
            current = current.parent;
        }

        path.Reverse();
        return path;
    }

    static int MinF(List<Node> l) 
    {
        int min = 0;
        float min_f = l[0].fCost;

        for (int i = 1; i < l.Count; ++i)
            if (l[i].fCost < min_f) 
            {
                min = i;
                min_f = l[i].fCost;
            }
        return min;
    }

    static bool LowerF(List<Node> l, Node a) 
    { 
        foreach (Node a_i in l) 
        {
            if (a_i.pos == a.pos && a_i.fCost < a.fCost) return true;
        }
        return false;
    }

    static Node[] Successors(Node a, Vector2Int to) 
    {
        Node[] buff = new Node[4];
        buff[0] = new Node(new Vector2Int(a.pos.x - 1, a.pos.y), 0, a.gCost + 1, a);
        buff[1] = new Node(new Vector2Int(a.pos.x + 1, a.pos.y), 0, a.gCost + 1, a);
        buff[2] = new Node(new Vector2Int(a.pos.x, a.pos.y - 1), 0, a.gCost + 1, a);
        buff[3] = new Node(new Vector2Int(a.pos.x, a.pos.y + 1), 0, a.gCost + 1, a);
        
        buff[0].hCost = MannDist(buff[0].pos, to);
        buff[1].hCost = MannDist(buff[1].pos, to);
        buff[2].hCost = MannDist(buff[2].pos, to);
        buff[3].hCost = MannDist(buff[3].pos, to);

        return buff;
    }

    static float MannDist(Vector2Int a, Vector2Int b) 
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    static bool IsBlocked(Vector2Int cell, Tilemap tilemap, GameObject omit/*, LayerMask obstacleMask*/)
    {
        Vector3 worldPos = tilemap.GetCellCenterWorld((Vector3Int)cell);

        Collider2D c = Physics2D.OverlapPoint(worldPos);

        //return Physics2D.OverlapPoint(worldPos/*, obstacleMask*/) != null;
        //return c != null && c.gameObject != omit;
        return false;
    }

}

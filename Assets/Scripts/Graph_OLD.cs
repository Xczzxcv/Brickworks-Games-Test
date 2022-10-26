using System.Collections.Generic;

namespace Graph
{
public class Graph_OLD<TContent>
{
    public struct GraphNode
    {
        public TContent Content;

        public static bool operator ==(GraphNode node1, GraphNode node2)
        {
            return node1.Equals(node2);
        }

        public static bool operator !=(GraphNode node1, GraphNode node2)
        {
            return !(node1 == node2);
        }

        public bool Equals(GraphNode node)
        {
            return Content.Equals(node.Content);
        }
    }

    public struct GraphEdge
    {
        public GraphNode Src;
        public GraphNode Dst;
    }

    public IReadOnlyList<GraphNode> Nodes => _nodes;
    public IReadOnlyList<GraphEdge> Edges => _edges;

    private List<GraphNode> _nodes = new List<GraphNode>();
    private List<GraphEdge> _edges = new List<GraphEdge>();

    public void GetEdges(GraphNode node, ref List<GraphEdge> edges)
    {
        edges.Clear();
        foreach (var edge in _edges)
        {
            if (edge.Src == node)
            {
                edges.Add(edge);
            }
        }
    }
}
}
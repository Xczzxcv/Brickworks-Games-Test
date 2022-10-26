using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Graph_Linked
{
public class Graph<TNodeContent> where TNodeContent : IEquatable<TNodeContent>
{
    public readonly struct Node
    {
        public readonly List<Edge> Edges;
        public readonly TNodeContent Content;

        public Node(TNodeContent content)
        {
            Content = content;
            Edges = new List<Edge>();
        }

        public static bool operator==(Node fst, Node snd)
        {
            return fst.Content.Equals(snd.Content);
        }

        public static bool operator !=(Node fst, Node snd)
        {
            return !(fst == snd);
        }
    }

    public readonly struct Edge
    {
        public readonly Node Src;
        public readonly Node Dest;

        public Edge(Node src, Node dest)
        {
            Src = src;
            Dest = dest;
        }
    }
    
    public struct Path
    {
        public List<Node> Nodes;
    }

    public Node Head;

    public void Init(TNodeContent content)
    {
        Head = new Node(content);
    }

    public bool TryGetNode(TNodeContent nodeContent, out Node node)
    {
        var result = FindNode(nodeContent, Head);
        if (result.HasValue)
        {
            node = result.Value;
            return true;
        }

        node = default;
        return false;
    }

    private static Node? FindNode(TNodeContent nodeContent, Node parentNode)
    {
        if (parentNode.Content.Equals(nodeContent))
        {
            return parentNode;
        }

        foreach (var edge in parentNode.Edges)
        {
            var result = FindNode(nodeContent, edge.Dest);
            if (result.HasValue)
            {
                return result;
            }
        }

        return null;
    }

    public Node? TryAddNode(TNodeContent parentNodeContent, TNodeContent content)
    {
        if (!TryGetNode(parentNodeContent, out var parentNode))
        {
            return null;
        }

        return AddNode(parentNode, content);
    }

    public Node? TryAddNode(List<TNodeContent> parentNodesContent, TNodeContent content)
    {
        Debug.Assert(parentNodesContent.Any(), "Node has to have at least one parent");

        var newNode = TryAddNode(parentNodesContent.First(), content);
        if (newNode == null)
        {
            return null;
        }

        for (int i = 1; i < parentNodesContent.Count; i++)
        {
            var parentNodeContent = parentNodesContent[i];
            if (!TryGetNode(parentNodeContent, out var parentNode))
            {
                Debug.LogError($"Can't find parent node {parentNodeContent} for node {newNode}");
                continue;
            }

            AddEdge(parentNode, newNode.Value);
        }

        return newNode;
    }

    public Node AddNode(Node parentNode, TNodeContent content)
    {
        var newNode = new Node(content);
        AddEdge(parentNode, newNode);

        return newNode;
    }

    private void AddEdge(Node src, Node dest)
    {
        Debug.Assert(TryGetNode(src.Content, out _), $"{src.Content} node should already be added");
        // Debug.Assert(TryGetNode(dest.Content, out _), $"{dest.Content} node should already be added");

        src.Edges.Add(new Edge(src, dest));
    }

    public bool HasPath(TNodeContent srcNodeContent, TNodeContent destNodeContent,
        List<TNodeContent> forbiddenNodesContent)
    {
        if (!TryGetNode(srcNodeContent, out var srcNode)
            || !TryGetNode(destNodeContent, out var destNode))
        {
            Debug.LogError($"Can't find node {srcNodeContent} or {destNodeContent}");
            return false;
        }

        var forbiddenNodes = new HashSet<Node>();
        foreach (var forbiddenNodeContent in forbiddenNodesContent)
        {
            if (!TryGetNode(forbiddenNodeContent, out var forbiddenNode))
            {
                Debug.LogError($"Can't find node {forbiddenNodeContent}");
                continue;
            }

            forbiddenNodes.Add(forbiddenNode);
        }

        var searchQueue = new Queue<Node>();
        searchQueue.Enqueue(srcNode);

        while (searchQueue.Count > 0)
        {
            var nodeToCheck = searchQueue.Dequeue();
            if (forbiddenNodes.Contains(nodeToCheck))
            {
                continue;
            }

            foreach (var nodeEdge in nodeToCheck.Edges)
            {
                searchQueue.Enqueue(nodeEdge.Dest);
            }

            if (nodeToCheck == destNode)
            {
                return true;
            }
        }

        return false;
    }

    public override string ToString()
    {
        return NodeToString(Head);
        
        string NodeToString(Node node, int depth = 0)
        {
            const char TAB_CHARACTER = '\t';
            var tab = new string(TAB_CHARACTER, depth);
            var result = $"{tab}C: {node.Content.ToString()}\n";
            
            if (node.Edges.Any())
            {
                result += $"{tab}E:\n";
            }

            foreach (var edge in node.Edges)
            {
                result += NodeToString(edge.Dest, depth + 1);
            }

            return result;
        }
    }
}
}

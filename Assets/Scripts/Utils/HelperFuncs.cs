using System;
using System.Collections.Generic;

public class HelperFuncs
{
    static public IEnumerable<int> GetRandomIndices(int arrayLength, int count)
    {
        if (count > arrayLength) throw new ArgumentOutOfRangeException(
            "count",
            "cannot exceed array length (" + count.ToString() + " > " + arrayLength.ToString() + ")"
        );

        LinkedList<int> indices = new();
        
        for (int i = 0; i < count; i++)
        {
            int idx = UnityEngine.Random.Range(0, arrayLength - i);
            var idxNode = indices.First;
            while (idxNode != null && idxNode.Value <= idx)
            {
                idx++;
                idxNode = idxNode.Next;
            }
            if (idxNode == null) indices.AddLast(idx);
            else indices.AddBefore(idxNode, idx);
            
            yield return idx;
        }
    }
}

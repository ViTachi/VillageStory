using UnityEngine;
using UnityEngine.AI;

public interface IPathFinder
{
    NavMeshPath GetPath(Vector3 from, Vector3 to, out float distance);
}

using UnityEngine;
using UnityEngine.AI;

public class PathFindService : IPathFindService
{
    public NavMeshPath GetPath(Vector3 from, Vector3 to, out float distance)
    {
        NavMeshPath path = new NavMeshPath();
        distance = float.PositiveInfinity;


        NavMesh.CalculatePath(from, to, 1, path);
        if (path.status == NavMeshPathStatus.PathInvalid)
        {
            if (NavMesh.SamplePosition(to, out NavMeshHit hit, 2f, 1))
                NavMesh.CalculatePath(from, hit.position, NavMesh.AllAreas, path);
        }
        if (path.status == NavMeshPathStatus.PathInvalid)
        {
            if (NavMesh.SamplePosition(to, out NavMeshHit hit, float.PositiveInfinity, 1))
                NavMesh.CalculatePath(from, hit.position, NavMesh.AllAreas, path);
        }

        if (path.status == NavMeshPathStatus.PathComplete && path.corners.Length > 1)
        {
            distance = 0f;
            for (int i = 1; i < path.corners.Length; i++)
            {
                distance += (path.corners[i - 1] - path.corners[i]).magnitude;
            }
        }
        return path;
    }
}
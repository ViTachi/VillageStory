using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFinder : MonoBehaviour, IPathFinder
{
    [SerializeField] private List<TestPathData> _testPathDatas;

    [System.Serializable]
    public class TestPathData
    {
        public Color color;
        public Transform from;
        public Transform to;
        public List<Vector3> pathPoints = new List<Vector3>();
        public float distance;
    }

    public NavMeshPath GetPath(Vector3 from, Vector3 to, out float distance)
    {
        NavMeshPath path = new NavMeshPath();
        distance = float.PositiveInfinity;

      
        NavMesh.CalculatePath(from, to, 1, path);
        if(path.status == NavMeshPathStatus.PathInvalid)
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

    [Button]
    public void BuildPathes()
    {
        foreach (var testPath in _testPathDatas)
        {
            NavMeshPath path = GetPath(testPath.from.position, testPath.to.position, out float distance);
            testPath.pathPoints.Clear();
            foreach (var corner in path.corners)
            {
                testPath.pathPoints.Add(corner);
            }
            testPath.distance = distance;
        }
    }

    private void OnDrawGizmos()
    {
        Color c = Gizmos.color;

        if (_testPathDatas == null) return;
        foreach (var testPath in _testPathDatas)
        {
            if (testPath.from == null || testPath.to == null) continue;

            Gizmos.color = testPath.color;
            Gizmos.DrawSphere(testPath.from.position, 0.1f);
            Gizmos.DrawSphere(testPath.to.position, 0.1f);
            if (testPath.pathPoints.Count < 2)
                continue;
            for (int i = 1; i < testPath.pathPoints.Count; i++)
            {
                Vector3 point = testPath.pathPoints[i];
                Vector3 prevPoint = testPath.pathPoints[i - 1];
                Gizmos.DrawLine(prevPoint, point);
            }
        }
        Gizmos.color = c;
    }
}

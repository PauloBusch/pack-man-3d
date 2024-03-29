using Assets.Constants;
using UnityEngine;

public abstract class WayPointBase : MonoBehaviour
{
    protected static GameObject[] _wayPoints;

    public static void LoadWayPoints()
    {
        if (_wayPoints != null && _wayPoints[0] != null) return;
        _wayPoints = GameObject.FindGameObjectsWithTag(Tags.WayPoint);
    }

    public static int CountChildsWithTag(string tag)
    {
        var count = 0;
        LoadWayPoints();
        foreach (var wayPoint in _wayPoints) {
            for (var index = 0; index < wayPoint.transform.childCount; index++)
            {
                if (wayPoint.transform.GetChild(index).gameObject.CompareTag(tag))
                    count++;
            }
        }

        return count;
    }

    protected GameObject GetRandom()
    {
        var index = Random.Range(0, _wayPoints.Length);
        return _wayPoints[index];
    }

    protected GameObject CreateChild(GameObject wayPoint, GameObject gameObjectPrefab, string tag = null)
    {
        ClearWayPoint(wayPoint);
        var childGameObject = Instantiate(
            gameObjectPrefab,
            wayPoint.transform.position,
            Quaternion.identity
        );
        childGameObject.transform.SetParent(wayPoint.transform);
        childGameObject.tag = tag;
        return childGameObject;
    }

    protected void ClearWayPoint(GameObject wayPoint)
    {
        for (var index = 0; index < wayPoint.transform.childCount; index++)
        {
            var child = wayPoint.transform.GetChild(index);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
    }
}

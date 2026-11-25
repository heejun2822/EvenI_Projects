using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Tilemap spawnTilemap;
    private List<Vector3> _spawnPoints;
    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
        _spawnPoints = GetSpawnPointsFromTilemap();
    }

    private List<Vector3> GetSpawnPointsFromTilemap()
    {
        List<Vector3> points = new List<Vector3>();

        foreach (Vector3Int pos in spawnTilemap.cellBounds.allPositionsWithin)
        {
            if(spawnTilemap.HasTile(pos))
            {
                Vector3 worldPos = spawnTilemap.CellToWorld(pos);
                points.Add(worldPos);
            }
        }

        return points;
    }

    public GameObject SpawnEnemy(GameObject enemyPrefab)
    {
        List<Vector3> offScreenSpawnPoints = GetOffScreenSpawnPoints();

        if (offScreenSpawnPoints.Count == 0)
        {
            Debug.Log("사용 가능한 스폰 포인트가 없습니다.");
            return null;
        }

        Vector3 spawnPoint = offScreenSpawnPoints[Random.Range(0, offScreenSpawnPoints.Count)];
        return Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
    }

    private List<Vector3> GetOffScreenSpawnPoints()
    {
        List<Vector3> offSceenPoints = new List<Vector3>();

        foreach (Vector3 point in _spawnPoints)
        {
            Vector3 viewportPoint = _camera.WorldToViewportPoint(point);

            if(viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1)
            {
                offSceenPoints.Add(point);
            }
        }

        return offSceenPoints;
    }
}

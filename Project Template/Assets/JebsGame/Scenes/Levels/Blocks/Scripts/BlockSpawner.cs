using UnityEngine;
using UnityHelpers;

public class BlockSpawner : MonoBehaviour
{
    public Transform blockPrefab;
    private ObjectPool<Transform> BlocksPool { get { if (_blocksPool == null) _blocksPool = new ObjectPool<Transform>(blockPrefab, 5, false, true); return _blocksPool; } }
    private ObjectPool<Transform> _blocksPool;

    public Vector2 spawnArea;

    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, spawnArea.ToXZVector3());
    }

    public void SpawnCube()
    {
        float randX = Random.Range(-spawnArea.x / 2, spawnArea.x / 2);
        float randZ = Random.Range(-spawnArea.y / 2, spawnArea.y / 2);
        Vector3 spawnOffset = transform.right * randX + transform.forward * randZ;
        BlocksPool.Get((block) => { block.position = transform.position + spawnOffset; });
    }
}

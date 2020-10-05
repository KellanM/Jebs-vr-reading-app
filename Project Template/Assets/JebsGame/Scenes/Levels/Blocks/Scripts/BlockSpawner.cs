using UnityEngine;
using UnityHelpers;

public class BlockSpawner : MonoBehaviour
{
    public Transform blockPrefab;
    private ObjectPool<Transform> BlocksPool { get { if (_blocksPool == null) _blocksPool = new ObjectPool<Transform>(blockPrefab, 5, false, true); return _blocksPool; } }
    private ObjectPool<Transform> _blocksPool;

    public void SpawnCube()
    {
        BlocksPool.Get((block) => { block.position = transform.position; });
    }
}

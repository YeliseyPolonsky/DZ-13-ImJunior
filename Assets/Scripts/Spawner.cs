using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;

    [SerializeField] private int _minimumLifetime = 2;
    [SerializeField] private int _maximumLifetime = 5;

    [SerializeField] private float _creationTime = 0.2f;

    private ObjectPool<Cube> _pool;

    private void Start()
    {
        _pool = new ObjectPool<Cube>(
            () => { return Instantiate(_cubePrefab); },
            OnGetCube,
            cube => { cube.gameObject.SetActive(false); },
            cube => { Destroy(cube.gameObject); }
        );
        
        InvokeRepeating(nameof(CreateCube), _creationTime, _creationTime);
    }
    
    private void CreateCube() => _pool.Get();

    private void OnGetCube(Cube cube)
    {
        cube.gameObject.SetActive(true);
        cube.Initialize(Random.Range(_minimumLifetime, _maximumLifetime), KillCube);
        cube.transform.position = GetRandomPositionInSphere();
    }

    private void KillCube(Cube cube) => _pool.Release(cube);

    private Vector3 GetRandomPositionInSphere()
    {
        int diameterToRadius = 2;
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * transform.localScale.x / diameterToRadius;
        return randomPoint;
    }
}
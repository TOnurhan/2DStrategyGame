using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler<T> where T : Component
{
    private readonly T _poolPrefab;
    private readonly int _initialPoolSize;
    private readonly Queue<T> _objectPool = new();
    private readonly Transform _parentTransform;
    private readonly int _expandSize;

    public static ObjectPooler<T> Instance;

    public ObjectPooler(T poolPrefab, int initialPoolSize, int expandSize, Transform parent)
    {
        Instance = this;

        _poolPrefab = poolPrefab;
        _initialPoolSize = initialPoolSize;
        _expandSize = expandSize;
        _parentTransform = parent;

        CreatePoolObject(_initialPoolSize);
    }

    public T SpawnFromPool(Vector3 position)
    {
        if (_objectPool.Count == 0)
        {
            CreatePoolObject(_expandSize);
        }

        var objectToSpawn = _objectPool.Dequeue();

        objectToSpawn.gameObject.SetActive(true);
        objectToSpawn.transform.SetPositionAndRotation(position, Quaternion.identity);

        return objectToSpawn;
    }

    public void ReturnToPool(T objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        _objectPool.Enqueue(objectToReturn);
    }

    private void CreatePoolObject(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var obj = Object.Instantiate(_poolPrefab);
            obj.transform.SetParent(_parentTransform);
            obj.gameObject.SetActive(false);
            _objectPool.Enqueue(obj);
        }
    }
}
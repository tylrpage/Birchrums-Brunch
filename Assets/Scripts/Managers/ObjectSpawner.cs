using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    public enum SpawnModes
    {
        FixedCount, Unlimited
    }
    [SerializeField] private SpawnModes spawnMode;
    [SerializeField] private ObjectSet objectSet;
    [SerializeField] private float spawnY;
    [SerializeField] private float spawnMinX;
    [SerializeField] private float spawnMaxX;
    [SerializeField] private float randomRotationVel;
    [SerializeField] private float randomXVel;
    [SerializeField] private Transform objectsParent;
    [SerializeField] private float objectSpawnCooldown;
    // Max number of objects in scene
    [SerializeField] private int maxFixed;
    // Only spawns objects in this group, only used for fixed mode
    [SerializeField] private int fixedGroup;

    private Dictionary<int, List<ObjectController>> _objects = new Dictionary<int, List<ObjectController>>();
    private HashSet<ObjectController> _activeObjects = new HashSet<ObjectController>();
    private float _timeSinceLastSpawn;

    private void Awake()
    {
        ParseObjectSet(objectSet);
    }

    private void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;
        
        switch (spawnMode)
        {
            case SpawnModes.FixedCount:
                if (_activeObjects.Count < maxFixed && _timeSinceLastSpawn >= objectSpawnCooldown)
                {
                    ObjectController objectToSpawn = GetRandomObject();
                    if (objectToSpawn != null)
                    {
                        Vector3 position = GetRandomSpawnPosition();
                        ObjectController newObject = Instantiate(objectToSpawn, position, Quaternion.identity, objectsParent);
                        newObject.Destroyed += OnObjectDestroyed;
                        newObject.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-randomRotationVel, randomRotationVel);
                        newObject.GetComponent<Rigidbody2D>().velocity = Vector2.right * Random.Range(-randomXVel, randomXVel);
                        _activeObjects.Add(newObject);

                        _timeSinceLastSpawn = 0;
                    }
                }
                break;
            case SpawnModes.Unlimited:
                break;
        }
    }

    private void OnObjectDestroyed(ObjectController sender)
    {
        _activeObjects.Remove(sender);
    }

    public void SetMaxFixed(int newMax)
    {
        maxFixed = newMax;
    }

    private void ParseObjectSet(ObjectSet set)
    {
        foreach (var objectController in set.objects)
        {
            if (!_objects.ContainsKey(objectController.GroupLevel))
            {
                _objects[objectController.GroupLevel] = new List<ObjectController>();
            }
            _objects[objectController.GroupLevel].Add(objectController);
        }
    }

    private ObjectController GetRandomObject()
    {
        if (spawnMode == SpawnModes.FixedCount)
        {
            if (_objects.TryGetValue(fixedGroup, out List<ObjectController> possibles))
            {
                int randomIndex = Random.Range(0, possibles.Count);
                return possibles[randomIndex];
            }
        }
        
        // todo: handle unlimited

        return null;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(spawnMinX, spawnMaxX);
        return new Vector3(randomX, spawnY, 0);
    }
}

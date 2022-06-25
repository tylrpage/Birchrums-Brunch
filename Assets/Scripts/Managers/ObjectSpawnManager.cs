using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectSpawnManager : MonoBehaviour
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
    [SerializeField] private SerializableDictionary<LevelSelectManager.Level, float> objectSpawnCooldownPerLevel;
    [SerializeField] private SerializableDictionary<LevelSelectManager.Level, int> maxFixedPerLevel;
    [SerializeField] private SerializableDictionary<LevelSelectManager.Level, string> fixedGroupsPerLevel;
    [SerializeField] private int greensInARowLimit;
    
    private Dictionary<int, List<ObjectController>> _objects = new Dictionary<int, List<ObjectController>>();
    private HashSet<ObjectController> _activeObjects = new HashSet<ObjectController>();
    private float _timeSinceLastSpawn;
    private bool _enabled;
    private int _greensInARow;
    private float _objectSpawnCooldown;
    private int _maxFixed;
    private List<int> _fixedGroups;

    private void Awake()
    {
        ParseObjectSet(objectSet);
        GameManager.Instance.TimeManager.TimerEnded += OnTimerEnded;
    }

    private void OnTimerEnded()
    {
        _enabled = false;
    }

    private void Update()
    {
        if (!_enabled)
            return;
        
        _timeSinceLastSpawn += Time.deltaTime;
        
        switch (spawnMode)
        {
            case SpawnModes.FixedCount:
                if (_activeObjects.Count < _maxFixed && _timeSinceLastSpawn >= _objectSpawnCooldown)
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
                        if (objectToSpawn.Good)
                            _greensInARow++;
                        else
                            _greensInARow = 0;
                    }
                }
                break;
            case SpawnModes.Unlimited:
                break;
        }
    }

    public void StartLevel(LevelSelectManager.Level level)
    {
        _enabled = true;
        
        _objectSpawnCooldown = objectSpawnCooldownPerLevel[level];
        _maxFixed = maxFixedPerLevel[level];
        _fixedGroups =  fixedGroupsPerLevel[level].Split(',').Select(int.Parse).ToList();
        _greensInARow = 0;
        _timeSinceLastSpawn = float.MaxValue;
    }

    public void DeleteAllActiveObjects()
    {
        foreach (var objectController in _activeObjects.ToList())
        {
            Destroy(objectController.gameObject);
        }
    }

    public void RestartLevel()
    {
        _enabled = true;
    }

    public void StopLevel()
    {
        _enabled = false;
    }

    private void OnObjectDestroyed(ObjectController sender)
    {
        _activeObjects.Remove(sender);
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
            List<ObjectController> possibles = new List<ObjectController>();
            foreach (var group in _fixedGroups)
            {
                // If we exceed the greens in a row limit, force a red item
                if (_greensInARow < greensInARowLimit)
                    possibles.AddRange(_objects[group]);
                else
                    possibles.AddRange(_objects[group].Where(x => !x.Good));
            }
            
            int randomIndex = Random.Range(0, possibles.Count);
            return possibles[randomIndex];
        }

        return null;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(spawnMinX, spawnMaxX);
        return new Vector3(randomX, spawnY, 0);
    }
}

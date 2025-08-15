using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scanner), typeof(ResourceStorage))]
public class MainBase : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private ResourceSpawner _resourceSpawner;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private int _initialUnitsCount = 3;

    private Scanner _scanner;
    private ResourceStorage _storage;
    private List<CollectingBot> _bots = new List<CollectingBot>();

    private void Start()
    {
        _scanner = GetComponent<Scanner>();
        _storage = GetComponent<ResourceStorage>();
        _resourceSpawner.StartSpawning();
        SpawnInitialUnits();
        StartCoroutine(WorkCycle());
    }

    private void SpawnInitialUnits()
    {
        for (int i = 0; i < _initialUnitsCount; i++)
        {
            CollectingBot bot = _unitSpawner.SpawnUnit(transform.position);
            bot.Initialize(transform, OnResourceCollected);
            _bots.Add(bot);
        }
    }

    private IEnumerator WorkCycle()
    {
        var wait = new WaitForSeconds(0.5f);

        while (enabled)
        {
            ScanAndAssignResources();
            yield return wait;
        }
    }

    private void ScanAndAssignResources()
    {
        if (_storage.IsFull)
            return;

        var resources = _scanner.FindResourcesInCollectionArea();
        _storage.UpdateResources(resources);

        foreach (CollectingBot bot in _bots)
        {
            if (bot.IsFree && _storage.HasAvailableResources)
            {
                Resource resource = _storage.GetUnassignedResource();

                if (resource != null && resource.CanBeCollected())
                {
                    bot.AssignResource(resource);
                    _storage.MarkResourceAsAssigned(resource);
                }
            }
        }
    }

    private void OnResourceCollected(int amount)
    {
        if (_storage.TryAddResource(amount) == false)
            Debug.Log("The storage is full!");
    }

    public void RegisterBot(CollectingBot bot)
    {
        if (_bots.Contains(bot) == false)
            _bots.Add(bot);
    }
}
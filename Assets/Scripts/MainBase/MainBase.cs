using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scanner), typeof(ResourceStorage), typeof(BotRetriever))]
public class MainBase : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private ResourceSpawner _resourceSpawner;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private int _initialUnitsCount = 3;

    private Scanner _scanner;
    private ResourceStorage _storage;
    private ResourceProvider _resourceProvider;
    private BotRetriever _botRetriever;
    private List<CollectingBot> _bots = new List<CollectingBot>();

    private void Start()
    {
        _scanner = GetComponent<Scanner>();
        _storage = GetComponent<ResourceStorage>();
        _botRetriever = GetComponent<BotRetriever>();
        _resourceProvider = GetComponent<ResourceProvider>();

        _resourceSpawner.OnResourceSpawned += _resourceProvider.RegisterResource;
        _botRetriever.OnBotArrived += OnBotArrived;

        _resourceSpawner.StartSpawning();
        SpawnInitialUnits();
        StartCoroutine(WorkCycle());
    }

    private IEnumerator WorkCycle()
    {
        var wait = new WaitForSeconds(0.3f);

        while (enabled)
        {
            if (_storage.IsFull == false)
            {
                _scanner.Scan(_resourceProvider);
                AssignTasksToFreeBots();
            }

            yield return wait;
        }
    }

    private void AssignTasksToFreeBots()
    {
        if (_storage.IsFull)
        {
            foreach (CollectingBot bot in _bots)
                bot.ReturnToBase();

            return;
        }

        foreach (CollectingBot bot in _bots)
        {
            if (bot.IsFree)
            {
                if (_resourceProvider.TryGetAvailableResource(out var resource))
                    bot.AssignResource(resource);
                else
                    bot.ReturnToBase();
            }
        }
    }

    private void SpawnInitialUnits()
    {
        for (int i = 0; i < _initialUnitsCount; i++)
        {
            CollectingBot bot = _unitSpawner.SpawnUnit(transform.position);
            bot.Initialize(transform);
            _bots.Add(bot);
        }
    }

    private void OnBotArrived(CollectingBot bot)
    {
        if (bot == null)
            return;

        if (bot.HasResource)
        {
            Resource resource = bot.TakeResource();

            if (resource != null)
            {
                if (_storage.TryAddResource(resource.Amount))
                {
                    _resourceProvider.RemoveResource(resource);
                }
                else
                {
                    Debug.LogWarning("The storage is full! The resource has not been added");
                    Destroy(resource.gameObject);
                }
            }
        }

        if (_storage.IsFull == false && _resourceProvider.TryGetAvailableResource(out var newResource))
            bot.AssignResource(newResource);
        else
            bot.ReturnToBase();
    }
}
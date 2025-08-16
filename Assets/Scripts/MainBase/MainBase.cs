using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Scanner), typeof(ResourceStorage), typeof(BotRetriever))]
public class MainBase : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private ResourceSpawner _resourceSpawner;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private int _initialUnitsCount = 3;
    [SerializeField] private ResourceCounterDisplay _resourceCounterDisplay;

    private Scanner _scanner;
    private ResourceStorage _storage;
    private ResourceProvider _resourceProvider;
    private BotRetriever _botRetriever;
    private List<CollectingBot> _bots = new List<CollectingBot>();
    private List<CollectingBot> _freeBots = new List<CollectingBot>();

    private void Start()
    {
        _scanner = GetComponent<Scanner>();
        _storage = GetComponent<ResourceStorage>();
        _botRetriever = GetComponent<BotRetriever>();
        _resourceProvider = GetComponent<ResourceProvider>();

        _botRetriever.BotArrived += OnBotArrived;
        _storage.ResourceAdded += OnResourceAdded;
        _resourceCounterDisplay.UpdateCounter(0);

        _resourceSpawner.StartSpawning();
        SpawnInitialUnits();
        StartCoroutine(ScanningRoutine());
    }

    private void OnResourceAdded(int count)
    {
        _resourceCounterDisplay.UpdateCounter(count);
    }

    private IEnumerator ScanningRoutine()
    {
        var wait = new WaitForSeconds(0.5f);

        while (enabled)
        {
            _scanner.Scan(_resourceProvider);
            AssignResourcesToFreeBots();
            yield return wait;
        }
    }

    private void OnBotArrived(CollectingBot bot)
    {
        if (bot == null)
            return;

        if (bot.HasResource)
        {
            Resource resource = bot.TakeResource();
            _storage.AddResource();
            _resourceProvider.RemoveResource(resource);
        }

        AddFreeBot(bot);
        TryAssignResourceToBot(bot);
    }

    private void SpawnInitialUnits()
    {
        for (int i = 0; i < _initialUnitsCount; i++)
        {
            CollectingBot bot = _unitSpawner.SpawnUnit(transform.position);
            bot.Initialize(transform);
            _bots.Add(bot);
            _freeBots.Add(bot);
            TryAssignResourceToBot(bot);
        }
    }

    private void AddFreeBot(CollectingBot bot)
    {
        if (_freeBots.Contains(bot) == false)
            _freeBots.Add(bot);
    }

    private void AssignResourcesToFreeBots()
    {
        for (int i = _freeBots.Count - 1; i >= 0; i--)
            TryAssignResourceToBot(_freeBots[i]);
    }

    private void TryAssignResourceToBot(CollectingBot bot)
    {
        if (_resourceProvider.TryAssignResource(out Resource resource))
        {
            bot.AssignResource(resource);
            _freeBots.Remove(bot);
        }
        else
        {
            bot.ReturnToBase();
        }
    }

    private void OnDestroy()
    {
        _botRetriever.BotArrived -= OnBotArrived;
        _storage.ResourceAdded -= OnResourceAdded;
    }
}
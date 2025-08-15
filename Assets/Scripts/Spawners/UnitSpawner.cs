using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private CollectingBot _botPrefab;

    public CollectingBot SpawnUnit(Vector3 position)
    {
        CollectingBot botObject = Instantiate(_botPrefab, position + new Vector3(Random.Range(-1f, 1f), 
            0, 
            Random.Range(-1f, 1f)), Quaternion.identity);

        return botObject;
    }
}
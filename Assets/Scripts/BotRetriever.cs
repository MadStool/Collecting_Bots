using UnityEngine;

public class BotRetriever : MonoBehaviour
{
    public event System.Action<CollectingBot> BotArrived;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CollectingBot>(out var bot))
        {
            HandleBotArrival(bot);
        }
    }

    public void HandleBotArrival(CollectingBot bot)
    {
        BotArrived?.Invoke(bot);
    }
}
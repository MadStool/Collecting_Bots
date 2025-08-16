using UnityEngine;

public class BotRetriever : MonoBehaviour
{
    public event System.Action<CollectingBot> OnBotArrived;

    public void HandleBotArrival(CollectingBot bot)
    {
        OnBotArrived?.Invoke(bot);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CollectingBot>(out var bot))
        {
            HandleBotArrival(bot);
        }
    }
}
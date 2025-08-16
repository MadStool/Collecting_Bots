using UnityEngine;

public class BotRetriever : MonoBehaviour
{
    public event System.Action<CollectingBot> BotArrived;

    public void TriggerBotArrived(CollectingBot bot)
    {
        BotArrived?.Invoke(bot);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CollectingBot bot) && bot.HasResource)
        {
            TriggerBotArrived(bot);
        }
    }
}
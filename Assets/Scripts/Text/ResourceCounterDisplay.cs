using UnityEngine;
using TMPro;

public class ResourceCounterDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _counterText;
    [SerializeField] private string _prefixText = "Resources: ";

    public void UpdateCounter(int count)
    {
        if (_counterText != null)
            _counterText.text = _prefixText + count;
    }
}
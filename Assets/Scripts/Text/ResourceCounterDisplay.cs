using UnityEngine;
using TMPro;

public class ResourceCounterDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _counterText;
    [SerializeField] private string _prefixText = "Resources based on: ";
    private int _totalCollectedResources = 0;

    private void Awake()
    {
        UpdateCounterText();
    }

    public void AddResource()
    {
        _totalCollectedResources++;
        UpdateCounterText();
    }

    private void UpdateCounterText()
    {
        if (_counterText != null)
            _counterText.text = _prefixText + _totalCollectedResources.ToString();
    }
}
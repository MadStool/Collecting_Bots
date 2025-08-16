using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private int _amount = 1;
    public int Amount => _amount;
}
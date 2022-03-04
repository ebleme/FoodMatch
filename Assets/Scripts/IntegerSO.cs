using UnityEngine;

[CreateAssetMenu(fileName = "IntegerSO", menuName = "My Scriptables/Integer SO")]

public class IntegerSO : ScriptableObject
{
    [SerializeField]

    private int _value;
    public int Value { get { return _value; } set { _value = value; } }
}

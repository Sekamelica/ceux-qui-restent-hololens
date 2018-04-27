using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnergyVariable", menuName = "EnergyVariable")]
public class Energy : ScriptableObject {

    // Attributes
    [System.NonSerialized]
    private float value;
    [System.NonSerialized]
    private float maximum;
    [SerializeField]
    private float energy;

    public void Initialize()
    {
        maximum = energy;
        value = energy;
    }

    // Setters
    public bool ChangeValue(float _newValue)
    {
        if (_newValue > maximum)
        {
            value = maximum;
            return false;
        }
        else if (_newValue < 0)
        {
            value = 0;
            return false;
        }
        else
        {
            value = _newValue;
            return true;
        }
    }

    public void ChangeMaximum(float _newMaximum)
    {
        if (value > _newMaximum)
            value = _newMaximum;
        maximum = _newMaximum;
    }

    public bool AddToValue(float _valueAdded)
    {
        return ChangeValue(value + _valueAdded);
    }

    public void AddToMaximum(float _maximumAdded)
    {
        ChangeMaximum(maximum + _maximumAdded);
    }

    // Getters
    public float GetValue()
    {
        return value;
    }

    public float GetMaximum()
    {
        return maximum;
    }
}

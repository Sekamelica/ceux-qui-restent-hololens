using UnityEngine;
using System;

namespace CeuxQuiRestent.Gameplay
{
    [CreateAssetMenu(fileName = "New Energy", menuName = "CeuxQuiRestent/Energy")]
    public class Energy : ScriptableObject
    {
        #region Attributes
        [SerializeField]
        private float[] energyLevels;
        [Tooltip("Start at index 0.")]
        [SerializeField]
        private int startingEnergyLevelIndex = 0;

        [Space]
        [Header("Debug: Change energy value at run-time:")]
        [SerializeField]
        private float value;
        private int currentEnergyLevelIndex = 0;
        
        #endregion

        #region Methods
        public void Initialize()
        {
            if (startingEnergyLevelIndex >= 0 && startingEnergyLevelIndex < energyLevels.Length)
                currentEnergyLevelIndex = startingEnergyLevelIndex;
            value = GetEnergyLevel();
        }

        public bool IncreaseEnergyLevel()
        {
            return SetEnergyLevel(currentEnergyLevelIndex + 1);
        }

        public void Fill()
        {
            value = GetEnergyLevel();
        }
        #endregion

        #region Setters
        public bool ChangeValue(float _newValue)
        {
            if (_newValue > GetEnergyLevel())
            {
                value = GetEnergyLevel();
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

        public bool AddToValue(float _valueAdded)
        {
            return ChangeValue(value + _valueAdded);
        }

        private bool SetEnergyLevel(int _energyLevelIndex)
        {
            if (_energyLevelIndex < 0 || _energyLevelIndex > energyLevels.Length)
                return false;
            else
            {
                currentEnergyLevelIndex = _energyLevelIndex;
                if (value > energyLevels[currentEnergyLevelIndex])
                    value = energyLevels[currentEnergyLevelIndex];
                return true;
            }
        }
        #endregion

        #region Getters
        public float[] GetEnergyLevels()
        {
            return energyLevels;
        }

        public float GetEnergyLevel()
        {
            return energyLevels[currentEnergyLevelIndex];
        }

        public float GetEnergyLevel(int _energyLevelIndex)
        {
            if (_energyLevelIndex < 0 || _energyLevelIndex > energyLevels.Length)
                return 0;
            else
                return energyLevels[_energyLevelIndex];
        }

        public float GetStartingEnergyLevel()
        {
            if (startingEnergyLevelIndex < 0 || startingEnergyLevelIndex > energyLevels.Length)
                return 0;
            else
                return energyLevels[startingEnergyLevelIndex];
        }

        public int GetEnergyLevelIndex()
        {
            return currentEnergyLevelIndex;
        }

        public float GetValue()
        {
            return value;
        }
        #endregion
    }
}
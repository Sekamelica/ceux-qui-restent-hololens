using UnityEngine;
using HoloToolkit.Unity.InputModule;
using Utility;
using CeuxQuiRestent.Links;

namespace CeuxQuiRestent.Interactables
{
    [System.Serializable]
    [RequireComponent(typeof(Focusable))]
    [RequireComponent(typeof(Collider))]
    public class Linkable : MonoBehaviour, IInputClickHandler
    {
        #region Attributes
        // Public Attributes
        public Linkable pair;
        public ActionExecuter actionsToDo;
        public Energy energy;
        
        // Private attributes        
        private bool alreadyLinked = false;
        private Linker linker;
        #endregion

        #region MonoBehaviour Methods
        void Start()
        {
            linker = GameObject.FindGameObjectWithTag("Player").GetComponent<Linker>();
        }

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            if (pair != null)
            {
                // Draw Link
                Debug.DrawLine(transform.position, pair.gameObject.transform.position, Color.yellow, 0.2f);

                // Label Energy
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.yellow;
                style.fontSize = 16;
                Vector3 labelPosition = Vector3.Lerp(gameObject.transform.position, pair.gameObject.transform.position, 0.5f);
                float energyCost = Vector3.Distance(gameObject.transform.position, pair.gameObject.transform.position);
                energyCost = ((float)((int)(energyCost * 100)) / 100.0f);
                string energyStr = "";
                if (energy != null)
                {
                    float[] energyLevels = energy.GetEnergyLevels();
                    for (int i = 0; i < energyLevels.Length && energyStr == ""; i++)
                        if (energyCost < energyLevels[i])
                            energyStr = "\nPalier n° " + i.ToString();
                    if (energyStr == "")
                    {
                        energyStr = "\nImpossible";
                        style.normal.textColor = Color.red;
                    }
                }
                UnityEditor.Handles.Label(labelPosition, energyCost.ToString() + " energy" + energyStr, style);
            }
        }
#endif
        #endregion

        #region Input Management
        public void OnInputClicked(InputClickedEventData eventData)
        {
            Interact();
        }
        #endregion

        #region Linkable Methods
        /// <summary>
        /// Called when you try to interact with this Linkable.
        /// </summary>
        public void Interact()
        {
            if (!alreadyLinked && pair != null)
                linker.LinkableClick(transform.position, gameObject, pair.gameObject);
        }

        /// <summary>
        /// When the linkable is linked to his pair.
        /// </summary>
        public void Linked()
        {
            alreadyLinked = true;
            if (actionsToDo != null)
            {
                actionsToDo.ResetCounter();
                actionsToDo.StartActions();
            }
        }
        #endregion

        #region Getters & Setters
        public bool IsAlreadyLinked()
        {
            return alreadyLinked;
        }

        public bool CanBeLinked()
        {
            return !alreadyLinked;
        }
        #endregion
    }

}
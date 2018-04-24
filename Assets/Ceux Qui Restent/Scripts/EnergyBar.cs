using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[ExecuteInEditMode]
public class EnergyBar : MonoBehaviour {

    [SerializeField]
    private Energy energy;
    private Image filler;
    
    void Start ()
    {
        filler = GetComponent<Image>();
    }

	void Update () {
        filler.fillAmount = energy.GetValue() / energy.GetMaximum();
	}
}

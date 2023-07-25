using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JumpForceBar : MonoBehaviour
{
    public Image fillJumpForceBar;


    // Start is called before the first frame update

    public void UpdateBar(float currentValue, float maxValue)
    {
      fillJumpForceBar.fillAmount = currentValue /maxValue;
    }
}

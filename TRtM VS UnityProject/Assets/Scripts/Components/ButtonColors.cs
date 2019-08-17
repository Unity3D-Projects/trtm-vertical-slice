using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColors : MonoBehaviour
{
    [Header("Pressed Colors")]
    [SerializeField]
    public ColorBlock pressedColors;
    [Header("Not Pressed Colors")]
    [SerializeField]
    public ColorBlock notPressedColors;
}

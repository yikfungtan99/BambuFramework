using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class VersionNumber : MonoBehaviour
{
    private TextMeshProUGUI txtVersionNumber;

    private void Awake()
    {
        txtVersionNumber = GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        txtVersionNumber.text = Application.version;
    }
}

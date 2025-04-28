using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunDisplay : MonoBehaviour
{
    TextMeshProUGUI text;
    public Team team;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText(string content)
    {
        if (text == null) return;
        text.SetText(content);
    }

    public void ClearText()
    {
        if (text == null) return;
        text.SetText("");
    }
}

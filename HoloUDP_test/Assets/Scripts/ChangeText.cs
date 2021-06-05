using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    [SerializeField, Tooltip("文字列を反映するテキストフィールド")]
    private TextMeshPro TargetTextField;

    public void SetASCIIBytes(byte[] bytes)
    {
        string getMessage = Encoding.ASCII.GetString(bytes);
        TargetTextField.text = getMessage;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    public string[] dialog;
    public float textSpeed; // Range: 0.0f (instant), 2 is Very Slow, so 0.05f is normal speed
    public float textSize = 0f;

    private int index;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       textMeshProUGUI.text = string.Empty;
       textMeshProUGUI.fontSize = textSize;
       StartDialog();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textMeshProUGUI.text == dialog[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textMeshProUGUI.text = dialog[index];
            }
        }
    }

    void StartDialog()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in dialog[index].ToCharArray())
        {
            textMeshProUGUI.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < dialog.Length - 1)
        {
            index++;
            textMeshProUGUI.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

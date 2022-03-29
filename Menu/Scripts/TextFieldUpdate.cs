using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFieldUpdate : MonoBehaviour
{
    Text textComponent;

    // Start is called before the first frame update
    void Start()
	{
		GetTextComponent();
	}

	private void OnValidate()
	{
		GetTextComponent();
	}

	private void GetTextComponent()
	{
		textComponent = GetComponent<Text>();
	}

	// Update is called once per frame
	public void SetText(string text)
    {
		textComponent.text = text.ToString();
    }

	public void SetText(int text)
	{
		textComponent.text = text.ToString();
	}

	public void SetText(float text)
	{
		textComponent.text = text.ToString();
	}

	public void SetText(double text)
	{
		textComponent.text = text.ToString();
	}

}

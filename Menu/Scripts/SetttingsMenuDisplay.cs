using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class SetttingsMenuDisplay : MonoBehaviour
{
	private void Start()
	{
		CreateSettingsUI(); //necessary until we switch the addListeners to addPersistantListener
	}

	[Header("Sciprtable Object with Settings Fields")]
    public Object settingsObj;

    List<FieldInfo> allSettings;

    [Header("UI Prefabs")]
    public GameObject buttonPrefab;
    public GameObject fieldPrefab;
    public GameObject togglePrefab;
    public GameObject listboxPrefab;
    public GameObject floatInputPrefab;

    [ContextMenu("update settings)")]
    // Start is called before the first frame update
    public void updatteSettings()
    {
        Debug.Log($"Updating Settings... \ntype = {settingsObj.GetType().ToString()}");

        allSettings = settingsObj.GetType().GetFields().ToList();
        foreach (FieldInfo i in allSettings)
		{
            Debug.Log($"  {i.Name.ToString()} <-> {i.GetValue(settingsObj)} ");
		}
    }

    
    [ContextMenu("Re-generate Settintgs UI on 'content' Child")]
    public void CreateSettingsUI()
	{
		Transform content = GetChildContentObject();

		updatteSettings();
		ClearContent(content);

		foreach (FieldInfo fieldInfo in allSettings)
		{
			//RectTransform newRect = new GameObject(fieldInfo.Name, typeof(RectTransform)).GetComponent<RectTransform>();
			//newRect.SetParent(content.transform);
			//Button newButton = newRect.gameObject.AddComponent<Button>();
			//newButton.GetComponentInChildren<Text>().text = fieldInfo.GetValue(settingsObj).ToString();

			//GameObject newButton = GameObject.Instantiate(buttonPrefab, content);
			//newButton.GetComponentInChildren<Text>().text = fieldInfo.GetValue(settingsObj).ToString();
			//newButton.gameObject.SetActive(true);

			if (fieldInfo.FieldType == typeof(float))
			{
				CreateFloatField(content, fieldInfo);
			}

			if (fieldInfo.FieldType == typeof(int))
			{
				CreateIntField(content, fieldInfo);
			}

			if (fieldInfo.FieldType == typeof(string))
			{
				CreateStringField(content, fieldInfo);
			}
		}
	}

	private void CreateFloatField(Transform content, FieldInfo fieldInfo)
	{
		GameObject newGO = GameObject.Instantiate(floatInputPrefab, content);
		SetLabelTextValue(newGO, fieldInfo.Name);

		InputField inputField = newGO.GetComponentInChildren<InputField>();
		inputField.ActivateInputField();
		inputField.characterValidation = InputField.CharacterValidation.Decimal;
		inputField.text = fieldInfo.GetValue(settingsObj).ToString();

		inputField.onEndEdit.AddListener((string test) =>
		{
			settingsObj.GetType().GetField(fieldInfo.Name).SetValue(settingsObj, float.Parse(test));
		});

		newGO.gameObject.SetActive(true);
	}


	private void CreateIntField(Transform content, FieldInfo fieldInfo)
	{
		GameObject newGO = GameObject.Instantiate(floatInputPrefab, content);
		SetLabelTextValue(newGO, fieldInfo.Name);

		InputField inputField = newGO.GetComponentInChildren<InputField>();
		inputField.ActivateInputField();
		inputField.characterValidation = InputField.CharacterValidation.Integer;
		inputField.text = fieldInfo.GetValue(settingsObj).ToString();

		inputField.onEndEdit.AddListener((string test) =>
		{
			settingsObj.GetType().GetField(fieldInfo.Name).SetValue(settingsObj, int.Parse(test));
		});

		newGO.gameObject.SetActive(true);
	}


	private void CreateStringField(Transform content, FieldInfo fieldInfo)
	{
		GameObject newGO = GameObject.Instantiate(floatInputPrefab, content);
		SetLabelTextValue(newGO, fieldInfo.Name);

		InputField inputField = newGO.GetComponentInChildren<InputField>();
		inputField.ActivateInputField();
		//inputField.characterValidation = InputField.CharacterValidation.Alphanumeric;
		inputField.text = fieldInfo.GetValue(settingsObj).ToString();

		inputField.onEndEdit.AddListener((string test) =>
		{
			settingsObj.GetType().GetField(fieldInfo.Name).SetValue(settingsObj, test);
		});

		newGO.gameObject.SetActive(true);
	}


	private static void SetLabelTextValue(GameObject newGO, string fieldName)
	{
		Text[] textFields = newGO.GetComponentsInChildren<Text>();

		foreach (Text textField in textFields)
		{
			if (textField.gameObject.name == "Label") textField.text = fieldName;
		}
	}


	/// <summary>
	/// Helper function to call Clearcontent() by targetting the child rect transform called "content" and rebuilding tthe UI hierarchy beneath it
	/// </summary>
	[ContextMenu("Clear Content")]
	public void ClearContent()
	{
		Transform content = GetChildContentObject();
		ClearContent(content);
	}


	/// <summary>
	/// Rebuilds the UI hierarchy beneath the given child transform
	/// </summary>
	/// <param name="content"></param>
	private void ClearContent(Transform content)
	{
		for (int i = content.childCount; i > 0; i--)
		{
			GameObject child = content.GetChild(i - 1).gameObject;
			//Destroy(child);
			DestroyImmediate(child);
		}
	}


	private Transform GetChildContentObject()
	{
		Transform content = null;
		Transform parent = this.transform;

		for (int i = 0; i < parent.childCount; i++)
		{
			if (parent.GetChild(i).name == "content")
			{
				content = parent.GetChild(i).transform;
			}
		}

		if (content == null)
		{
			GameObject newGO = new GameObject("content", typeof(RectTransform));
			newGO.transform.SetParent(parent, false);
			//newGO.gameObject.name = "content";
			content = newGO.transform;

		}

		return content;
	}
}

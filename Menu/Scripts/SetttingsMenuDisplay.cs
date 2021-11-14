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

			if (fieldInfo.FieldType == typeof(float)) {
				GameObject newGO = GameObject.Instantiate(floatInputPrefab, content);
				Text[] textFields = newGO.GetComponentsInChildren<Text>();

				foreach (Text textField in textFields)
				{
					if (textField.gameObject.name == "Label") textField.text = fieldInfo.Name;
				}

				InputField inputField = newGO.GetComponentInChildren<InputField>();
				inputField.ActivateInputField();
				inputField.text = fieldInfo.GetValue(settingsObj).ToString();

				inputField.onEndEdit.AddListener((string test) =>
				   {
					   Debug.Log(test);
					   settingsObj.GetType().GetField(fieldInfo.Name).SetValue(settingsObj, float.Parse(test));
				   });

				newGO.gameObject.SetActive(true);
			}

			if (fieldInfo.FieldType == typeof(int))
			{

			}

			if (fieldInfo.FieldType == typeof(string))
			{

			}
		}
	}


	[ContextMenu("Clear Content")]
	public void ClearContent()
	{
		Transform content = GetChildContentObject();
		ClearContent(content);
	}


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

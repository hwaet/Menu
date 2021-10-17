using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class SetttingsMenuDisplay : MonoBehaviour
{
    [Header("Sciprtable Object with Settings Fields")]
    public Object settingsObj;

    List<FieldInfo> allSettings;

    [Header("UI Prefabs")]
    public Button buttonPrefab;

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
        Transform parent = this.transform;
        Transform content = null;

        for (int i = 0; i< parent.childCount; i++)
		{
            if (parent.GetChild(i).name=="content")
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

		for (int i = content.childCount; i > 0 ; i--)
		{
            GameObject child = content.GetChild(i-1).gameObject;
            //Destroy(child);
            DestroyImmediate(child);
		}

        updatteSettings();

        foreach (FieldInfo fieldInfo in allSettings)
		{
            //RectTransform newRect = new GameObject(fieldInfo.Name, typeof(RectTransform)).GetComponent<RectTransform>();
            //newRect.SetParent(content.transform);
            //Button newButton = newRect.gameObject.AddComponent<Button>();
            //newButton.GetComponentInChildren<Text>().text = fieldInfo.GetValue(settingsObj).ToString();

            Button newButton = GameObject.Instantiate(buttonPrefab, content);
            newButton.GetComponentInChildren<Text>().text = fieldInfo.GetValue(settingsObj).ToString();
        }
    }
}

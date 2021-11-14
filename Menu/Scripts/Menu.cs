﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Menu : MonoBehaviour
{
	public bool menuDisabledOnStart = false;
	List<CanvasGroup> canvasGroups;

	public GameObject lastSelectedButton;

	public delegate void MenuTriggerQuit();
	public static event MenuTriggerQuit OnMenuCallExit = delegate {};


    // Start is called before the first frame update
    void Start()
    {
		this.GetComponent<RectTransform>().position = Vector3.zero;
		this.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

		getCanvasGroups();

		if (menuDisabledOnStart == true)
		{
			DisableAll();
		}
    }

	private void OnValidate()
	{
		getCanvasGroups();
	}

	// Update is called once per frame
	void getCanvasGroups()
    {
		canvasGroups = this.gameObject.GetComponentsInChildren<CanvasGroup>().ToList();
	}

	public void Disable(CanvasGroup cg)
	{
		cg.interactable = false;
		cg.alpha = 0;
		cg.blocksRaycasts = false;
	}

	public void Enable(CanvasGroup cg)
	{
		cg.interactable = true;
		cg.alpha = 1;
		cg.blocksRaycasts = true;
	}

	[ContextMenu("Disable All")]
	public void DisableAll()
	{
		lastSelectedButton = EventSystem.current.currentSelectedGameObject;

		for (int i = 0; i < canvasGroups.Count; i++)
		{
			Disable(canvasGroups[i]);
		}
	}

	[ContextMenu("Enable All")]
	public void EnableAll()
	{
		if (lastSelectedButton==null)
		{
			lastSelectedButton = GetComponentInChildren<Selectable>().gameObject;
		}

		StartCoroutine(SetNewSelection());
			
		for (int i = 0; i < canvasGroups.Count; i++)
		{
			Enable(canvasGroups[i]);
		}


	}

	IEnumerator SetNewSelection()
	{
		EventSystem.current.SetSelectedGameObject(null);

		yield return null;

		EventSystem.current.SetSelectedGameObject(lastSelectedButton);
		Selectable selectable = lastSelectedButton.GetComponent<Selectable>();
		selectable.OnSelect(null);
	}


	public void setVisible(bool visFlag)
	{
		if (visFlag == true) EnableAll();
		if (visFlag == false) DisableAll();
	}

	public void switchToMenu(Menu menu)
	{
		//Debug.Assert(menu != null, "Menu switch function must have a target menu to switch to.");
		//Debug.Assert(menu.GetType() != typeof(Menu), "Menu switch function must target a 'Menu' object.");
		if ( (menu != null) && (menu.GetType() == typeof(Menu)) )
		{
			this.DisableAll();
			menu.EnableAll();
		}
		
	}

	public void MenuCallExitDelegate()
	{
		if (Menu.OnMenuCallExit != null) {	OnMenuCallExit(); }
	}
}

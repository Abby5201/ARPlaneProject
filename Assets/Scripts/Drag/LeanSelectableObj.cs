using Lean.Common;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class LeanSelectableObj : LeanSelectableBehaviour
{
	/// <summary>The default color given to the SpriteRenderer.</summary>
	public Color DefaultColor { set { defaultColor = value; UpdateColor(); } get { return defaultColor; } }
	[SerializeField] private Color defaultColor = Color.white;

	/// <summary>The color given to the SpriteRenderer when selected.</summary>
	public Color SelectedColor { set { selectedColor = value; UpdateColor(); } get { return selectedColor; } }
	[SerializeField] private Color selectedColor = Color.green;

	[System.NonSerialized]
	private Renderer cachedRenderer;

	[System.NonSerialized]
	private MaterialPropertyBlock properties;


	private LeanSelectByFinger _leanSelectByFinger;

	protected override void OnSelected(LeanSelect select)
	{
		if (_leanSelectByFinger == null)
			_leanSelectByFinger = FindObjectOfType<LeanSelectByFinger>();

		if (_leanSelectByFinger.Selectables.Count > 0)
		{
			_leanSelectByFinger.Selectables.Clear();
		}
		UpdateColor();
	}

	protected override void OnDeselected(LeanSelect select)
	{
		Debug.Log("--------DeSelected");
		//EventManager.instance.OnTriggerEvent(Event.DeSelectedEvent, null);
	}

	protected override void Start()
	{
		base.Start();

		//UpdateColor();
	}

	public void UpdateColor()
	{
		//Debug.Log("Selected:"+transform.parent.name);
		//EventManager.instance.OnTriggerEvent(Event.SelectedObjEvent, this.gameObject);
	}


}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frog_Feed_Order
{
	[RequireComponent(typeof(LineRenderer))]
	public class FrogNode : BaseNode, IClickable
	{
		public static Action OnFrogClick;

		private LineController lineController;
		private LevelManager levelManager;

		void Awake()
		{
			lineController = GetComponent<LineController>();

			RandomizeColor();
		}

		void Start()
		{
			levelManager = LevelManager.Instance;
		}

		/// <summary>
		/// Called when a frog is clicked. Calculates and assigns the path to its lineRenderer.
		/// </summary>
		public void Clicked()
		{
			Debug.Log("Clicked Frog (" + rowIndex + ", " + columnIndex + ")");

			OnFrogClick?.Invoke();

			lineController.AssignPathToLine(levelManager.GetPath(this, facingDirection, chosenColor));

			// TODO: IF SUCCESSFUL, REMOVE FROG, DECREMENT FROG COUNT

			// levelManager.CheckLevelEnd();
		}
	}
}

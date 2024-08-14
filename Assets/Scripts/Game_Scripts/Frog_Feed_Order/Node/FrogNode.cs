using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frog_Feed_Order
{
	[RequireComponent(typeof(LineRenderer))]
	public class FrogNode : BaseNode, IClickable
	{
		[SerializeField] private FacingDirection facingDirection;
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

		public void Clicked()
		{
			Debug.Log("Clicked Frog (" + rowIndex + ", " + columnIndex + ")");

			lineController.AddNodeToLine();
		}

		public void SetFacingDirection(FacingDirection direction)
		{
			switch (direction)
			{
				case FacingDirection.Up:
					facingDirection = FacingDirection.Up;
					transform.localRotation = Quaternion.Euler(0, 180, 0);
					break;

				case FacingDirection.Down:
					facingDirection = FacingDirection.Down;
					transform.localRotation = Quaternion.Euler(0, 0, 0);
					break;

				case FacingDirection.Left:
					facingDirection = FacingDirection.Left;
					transform.localRotation = Quaternion.Euler(0, 90, 0);
					break;

				case FacingDirection.Right:
					facingDirection = FacingDirection.Right;
					transform.localRotation = Quaternion.Euler(0, -90, 0);
					break;
			}
		}
	}

	public enum FacingDirection
	{
		Up = 0,
		Down = 1,
		Left = 2,
		Right = 3
	}
}

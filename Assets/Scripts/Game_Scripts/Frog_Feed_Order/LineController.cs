using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class LineController : MonoBehaviour
	{
		[SerializeField] private LineRenderer lineRenderer;
		[SerializeField] private Color lineColor;
		[SerializeField] private float lineWidth;
		private Transform[] positions;

		void Awake()
		{
			// Set up the LineRenderer with initial position
			positions = new Transform[1];
			GameObject newGameObject = new GameObject();
			newGameObject.transform.localPosition = new Vector3(0, 0.2f, 0);
			positions[0] = newGameObject.transform;
			SetUpLineRenderer(positions);
		}

		/// <summary>
		/// Set up the LineRenderer points
		/// </summary>
		/// <param name="positions"></param>
		public void SetUpLineRenderer(Transform[] positions)
		{
			lineRenderer.positionCount = positions.Length;
			this.positions = positions;
		}

		/// <summary>
		/// Add a new node to the line
		/// </summary>
		public void AddNodeToLine()
		{
			// Create new game object to use its transform
			GameObject newGameObject = new GameObject();
			newGameObject.transform.SetParent(transform);

			// Assign new position to last tongue positions forward
			newGameObject.transform.localPosition = new Vector3(0, 0.2f, positions[^1].transform.localPosition.z - 1);

			// Create new Transform array and add new position
			Transform[] newPositions = new Transform[positions.Length + 1];
			for (int i = 0; i < positions.Length; i++)
			{
				newPositions[i] = positions[i];
			}
			newPositions[^1] = newGameObject.transform;

			// Assign new positions to LineRenderer
			SetUpLineRenderer(newPositions);

			DrawLine();
		}

		/// <summary>
		/// Draw the line
		/// </summary>
		private void DrawLine()
		{
			if (positions == null || positions.Length == 0) return;

			for (int i = 0; i < positions.Length; i++)
			{
				lineRenderer.SetPosition(i, positions[i].localPosition);
			}
		}
	}
}

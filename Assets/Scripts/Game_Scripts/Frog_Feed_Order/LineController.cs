using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class LineController : MonoBehaviour
	{
		[SerializeField] private FrogNode frogNode;
		[SerializeField] private Transform tongueTr;
		[SerializeField] private LineRenderer lineRenderer;
		[SerializeField] private float lineHeight;
		[SerializeField] private float tongueMoveInterval;
		private Transform[] positions;
		private Transform[] tempPath;

		/// <summary>
		/// Assigns the calculated path to the line
		/// </summary>
		/// <param name="calculatedPath"></param>
		public void AssignPathToLine(Transform[] calculatedPath)
		{
			// Create array of transforms
			positions = new Transform[calculatedPath.Length];
			this.positions = calculatedPath;

			StopAllCoroutines();

			StartCoroutine(ExtendTongue());
		}

		/// <summary>
		/// Routine to extend tongue through the path
		/// </summary>
		/// <returns></returns>
		IEnumerator ExtendTongue()
		{
			bool isCollectable = true;
			List<GrapeNode> visitedNodes = new List<GrapeNode>();

			// Extend Tongue
			for (int i = 0; i < positions.Length; i++)
			{
				// Create a new spot for the next node
				tempPath = new Transform[i + 1];
				lineRenderer.positionCount = tempPath.Length;

				// Assign path position to line
				tempPath[i] = positions[i];

				StartCoroutine(LerpPosition(tempPath[i].position, tongueMoveInterval, i));
				yield return new WaitForSeconds(tongueMoveInterval);

				// Invoke node visited action
				if (positions[i].TryGetComponent(out GrapeNode grapeNode))
				{
					visitedNodes.Add(grapeNode);
					grapeNode.OnVisit?.Invoke(frogNode.chosenColor);
				}
			}

			// Check if the last node is the same color as the frog
			if (visitedNodes[^1].chosenColor != frogNode.chosenColor)
				isCollectable = false;

			// Retract Tongue
			for (int i = positions.Length - 1; i >= 0; i--)
			{
				Vector3 newPos = Vector3.zero;

				StartCoroutine(LerpPosition(positions[i].transform.position, tongueMoveInterval, i));
				yield return new WaitForSeconds(tongueMoveInterval);

				// if path is correct, move and set grapes parent
				if (isCollectable && positions[i].TryGetComponent(out GrapeNode grapeNode))
				{
					grapeNode.GetItem().transform.SetParent(tongueTr, true);

					switch (frogNode.facingDirection)
					{
						case FacingDirection.Down:
							newPos = new Vector3(tongueTr.position.x, tongueTr.position.y, tongueTr.position.z - (positions.Length - i - 1) * 0.5f);
							break;

						case FacingDirection.Up:
							newPos = new Vector3(tongueTr.position.x, tongueTr.position.y, tongueTr.position.z + (positions.Length - i - 1) * 0.5f);
							break;

						case FacingDirection.Left:
							newPos = new Vector3(tongueTr.position.x - (positions.Length - i - 1) * 0.5f, tongueTr.position.y, tongueTr.position.z);
							break;

						case FacingDirection.Right:
							newPos = new Vector3(tongueTr.position.x + (positions.Length - i - 1) * 0.5f, tongueTr.position.y, tongueTr.position.z);
							break;
					}

					grapeNode.GetItem().position = newPos;
					grapeNode.OnRetract?.Invoke(i - 1, tongueMoveInterval);
				}

				lineRenderer.positionCount--;
			}

			// Clear line
			StopCoroutine(nameof(LerpPosition));
			positions = new Transform[0];
			lineRenderer.positionCount = 0;
		}

		/// <summary>
		/// Lerps the position of the line for smooth tongue extension
		/// </summary>
		/// <param name="targetPosition"></param>
		/// <param name="duration"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		IEnumerator LerpPosition(Vector3 targetPosition, float duration, int index)
		{
			float time = 0;
			Vector3 startPosition = tongueTr.position;
			Vector3 lerpedVector;

			while (time < duration)
			{
				lerpedVector = Vector3.Lerp(startPosition, targetPosition, time / duration);
				tongueTr.position = new Vector3(lerpedVector.x, tongueTr.position.y, lerpedVector.z);
				lineRenderer.SetPosition(index, new Vector3(lerpedVector.x, lineHeight, lerpedVector.z));
				time += Time.deltaTime;
				yield return null;
			}

			lerpedVector = targetPosition;
			tongueTr.position = new Vector3(lerpedVector.x, tongueTr.position.y, lerpedVector.z); ;

			// if (index >= 0 && index <= lineRenderer.positionCount)
			// lineRenderer.SetPosition(index, new Vector3(lerpedVector.x, lineHeight, lerpedVector.z));
		}
	}
}

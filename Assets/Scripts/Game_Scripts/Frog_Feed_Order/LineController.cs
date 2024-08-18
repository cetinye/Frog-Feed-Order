using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class LineController : MonoBehaviour
	{
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
				positions[i].GetComponent<BaseNode>().OnVisit?.Invoke();
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
			Vector3 startPosition;
			Vector3 lerpedVector;

			if (index == 0)
				startPosition = transform.position;
			else
				startPosition = positions[index - 1].position;

			while (time < duration)
			{
				lerpedVector = Vector3.Lerp(startPosition, targetPosition, time / duration);
				lineRenderer.SetPosition(index, new Vector3(lerpedVector.x, lineHeight, lerpedVector.z));
				time += Time.deltaTime;
				yield return null;
			}

			lerpedVector = targetPosition;
			lineRenderer.SetPosition(index, new Vector3(lerpedVector.x, lineHeight, lerpedVector.z));
		}
	}
}

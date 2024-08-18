using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private Camera mainCamera;
		[SerializeField] private Vector3 camPosOffset;
		[SerializeField] private Vector3 camRotation;
		private List<Transform> targets;
		private int minLayerCount = 1;

		/// <summary>
		/// Arrange the camera in the middle of the given transforms
		/// </summary>
		/// <param name="transforms"></param>
		public void ArrangeCamera(List<Transform> transforms, int minLayerIndex)
		{
			targets = transforms;
			minLayerCount = minLayerIndex;
			PositionCamera(CalculateMiddlePoint(transforms));
		}

		/// <summary>
		/// Calculate the middle point of the given transforms
		/// </summary>
		/// <param name="targets"></param>
		/// <returns> middle point Vector3</returns>
		private Vector3 CalculateMiddlePoint(List<Transform> targets)
		{
			Vector3 sumOfPositions = Vector3.zero;

			foreach (Transform target in targets)
			{
				sumOfPositions += target.position;
			}

			return sumOfPositions / targets.Count;
		}

		/// <summary>
		/// Position the camera at the calculated middle point
		/// </summary>
		/// <param name="middlePoint"></param>
		private void PositionCamera(Vector3 middlePoint)
		{
			mainCamera.transform.position = new Vector3(middlePoint.x, 10f, middlePoint.z);
			mainCamera.transform.position += camPosOffset;
			mainCamera.transform.rotation = Quaternion.Euler(camRotation);
			mainCamera.orthographicSize = (float)(Math.Sqrt(targets.Count / Mathf.Abs(minLayerCount)) + 4);
		}
	}
}
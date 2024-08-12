using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class LevelManager : MonoBehaviour
	{
		[SerializeField] private CameraController cameraController;
		[SerializeField] private Grid grid;

		void Start()
		{
			cameraController.ArrangeCamera(grid.GetNodeTransforms());
		}
	}
}

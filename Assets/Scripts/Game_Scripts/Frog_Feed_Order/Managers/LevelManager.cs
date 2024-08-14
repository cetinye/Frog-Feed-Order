using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class LevelManager : MonoBehaviour
	{
		public static LevelManager Instance;

		[SerializeField] private CameraController cameraController;
		[SerializeField] private Grid grid;

		void Awake()
		{
			Instance = this;
		}

		void Start()
		{
			cameraController.ArrangeCamera(grid.GetNodeTransforms());
		}

		public BaseNode GetBaseNode(int rowIndex, int columnIndex)
		{
			return grid.GetBaseNode(rowIndex, columnIndex);
		}
	}
}

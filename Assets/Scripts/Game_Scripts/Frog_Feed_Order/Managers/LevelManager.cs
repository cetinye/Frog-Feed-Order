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

		/// <summary>
		/// Get the path from the startNode until the end and return it as an array
		/// </summary>
		/// <param name="startNode"></param>
		/// <param name="direction"></param>
		/// <returns>Array of transforms along the path</returns>
		public Transform[] GetPath(BaseNode startNode, FacingDirection direction)
		{
			return grid.GetPath(startNode.rowIndex, startNode.columnIndex, direction).ToArray();
		}
	}
}

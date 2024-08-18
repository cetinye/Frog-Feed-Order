using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class LevelManager : MonoBehaviour
	{
		public static LevelManager Instance;

		[Header("Level Variables")]
		[SerializeField] int levelId;
		[SerializeField] private TextAsset levelDatasJSON;
		public LevelDatas levelDatas;
		private LevelData level;

		[Space(20)]
		[SerializeField] private UIManager uiManager;
		[SerializeField] private CameraController cameraController;
		[SerializeField] private Grid grid;

		private int leftMoves = 0;
		private int rowSize;
		private int columnSize;
		private int frogCount = 0;

		void Awake()
		{
			Instance = this;

			FrogNode.OnFrogClick += UpdateMovesText;

			ReadJSON();
		}

		void Start()
		{
			StartLevel();
		}

		/// <summary>
		/// Read the JSON file containing the level information
		/// </summary>
		private void ReadJSON()
		{
			levelDatas = JsonUtility.FromJson<LevelDatas>(levelDatasJSON.text);

			level = levelDatas.levelData[levelId];
			leftMoves = level.moves;

			rowSize = level.gridSize[0];
			columnSize = level.gridSize[1];
		}

		/// <summary>
		/// Start the level
		/// </summary>
		private void StartLevel()
		{
			// Generate the level grid
			GenerateLevel();

			// Assign the level values
			AssignVariables();

			// Center the camera
			cameraController.ArrangeCamera(grid.GetNodeTransforms());
		}

		/// <summary>
		/// Generate the level grid
		/// </summary>
		private void GenerateLevel()
		{
			grid.FillGrid(rowSize, columnSize, level);
		}

		/// <summary>
		/// Assign the level values
		/// </summary>
		private void AssignVariables()
		{
			// Get the total number of frogs on the level
			frogCount = grid.GetFrogCount();

			// Update the level text
			UpdateLevelText();

			// Update the moves text
			uiManager.SetMovesText(leftMoves);
		}

		/// <summary>
		/// Updates the level text based on the current level
		/// </summary>
		private void UpdateLevelText()
		{
			uiManager.SetLevelText(level.levelId);
		}

		/// <summary>
		/// Decrement left moves counter and update the moves text
		/// </summary>
		private void UpdateMovesText()
		{
			leftMoves--;
			uiManager.SetMovesText(leftMoves);
		}

		/// <summary>
		/// Check if the level has ended
		/// </summary>
		public void CheckLevelEnd()
		{
			if (IsOutOfMoves())
			{
				Debug.LogWarning("OUT OF MOVES");
				// TODO: LEVEL FAILED PANEL
			}

			if (frogCount == 0)
			{
				Debug.LogWarning("LEVEL COMPLETE");
				// TODO: LEVEL COMPLETED PANEL
			}
		}

		/// <summary>
		/// Get the path from the startNode until the end and return it as an array
		/// </summary>
		/// <param name="startNode"></param>
		/// <param name="direction"></param>
		/// <returns>Array of transforms along the path</returns>
		public Transform[] GetPath(BaseNode startNode, FacingDirection direction, Colors color)
		{
			return grid.GetPath(startNode.rowIndex, startNode.columnIndex, direction, color).ToArray();
		}

		/// <summary>
		/// Check if the level is out of moves
		/// </summary>
		/// <returns>a boolean indicating if the level is out of moves</returns>
		private bool IsOutOfMoves()
		{
			Debug.Log("Left Moves: " + leftMoves + " Frog Count: " + frogCount);
			return leftMoves < frogCount;
		}

		/// <summary>
		/// Decrease the frog count
		/// </summary>
		public void DecreaseFrogCount()
		{
			frogCount--;
		}

		public void RemoveNode(BaseNode node)
		{
			grid.RemoveNode(node);
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class Grid : MonoBehaviour
	{
		[SerializeField] int levelId;

		[Header("JSON")]
		[SerializeField] private TextAsset levelDatasJSON;
		[SerializeField] private LevelDatas levelDatas;
		[SerializeField] private LevelData level;

		[Header("Grid Variables")]
		[SerializeField] private GrapeNode grapeNodePrefab;
		[SerializeField] private ArrowNode arrowNodePrefab;
		[SerializeField] private FrogNode frogNodePrefab;
		[SerializeField] private float startRowPos;
		[SerializeField] private float startColumnPos;
		[SerializeField] private float spaceBetweenRows;
		[SerializeField] private float spaceBetweenColumns;
		private int rowSize;
		private int columnSize;

		[Header("Node Variables")]
		[SerializeField] private List<BaseNode> nodes = new List<BaseNode>();
		private List<Transform> nodesTransforms = new List<Transform>();
		private List<Transform> path = new List<Transform>();


		void Awake()
		{
			ReadJSON();
			FillGrid(rowSize, columnSize);
		}

		/// <summary>
		/// Read the JSON file containing the level information
		/// </summary>
		private void ReadJSON()
		{
			levelDatas = JsonUtility.FromJson<LevelDatas>(levelDatasJSON.text);

			level = levelDatas.levelData[levelId];

			rowSize = level.gridSize[0];
			columnSize = level.gridSize[1];
		}

		/// <summary>
		/// Create a grid with given parameters
		/// </summary>
		/// <param name="rows"></param>
		/// <param name="columns"></param>
		private void FillGrid(int rows, int columns)
		{
			int gridDataReadIndex = 0;

			float posRow = startRowPos;
			float posColumn = startColumnPos;

			for (int rowIndex = 0; rowIndex < rows; rowIndex++)
			{
				for (int columnIndex = 0; columnIndex < columns; columnIndex++)
				{
					BaseNode node = null;

					switch (level.gridData[gridDataReadIndex])
					{
						// Grape
						case "G":
							node = Instantiate(grapeNodePrefab, transform);
							break;

						// Arrow Facing UP
						case "AU":
							node = Instantiate(arrowNodePrefab, transform);
							((ArrowNode)node).SetFacingDirection(FacingDirection.Up);
							break;

						// Arrow Facing DOWN
						case "AD":
							node = Instantiate(arrowNodePrefab, transform);
							((ArrowNode)node).SetFacingDirection(FacingDirection.Down);
							break;

						// Arrow Facing LEFT
						case "AL":
							node = Instantiate(arrowNodePrefab, transform);
							((ArrowNode)node).SetFacingDirection(FacingDirection.Left);
							break;

						// Arrow Facing RIGHT
						case "AR":
							node = Instantiate(arrowNodePrefab, transform);
							((ArrowNode)node).SetFacingDirection(FacingDirection.Right);
							break;

						// Frog Facing UP
						case "FU":
							node = Instantiate(frogNodePrefab, transform);
							((FrogNode)node).SetFacingDirection(FacingDirection.Up);
							break;

						// Frog Facing DOWN
						case "FD":
							node = Instantiate(frogNodePrefab, transform);
							((FrogNode)node).SetFacingDirection(FacingDirection.Down);
							break;

						// Frog Facing LEFT
						case "FL":
							node = Instantiate(frogNodePrefab, transform);
							((FrogNode)node).SetFacingDirection(FacingDirection.Left);
							break;

						// Frog Facing RIGHT
						case "FR":
							node = Instantiate(frogNodePrefab, transform);
							((FrogNode)node).SetFacingDirection(FacingDirection.Right);
							break;

						default:
							Debug.Log("Invalid grid data: " + level.gridData[gridDataReadIndex]);
							break;
					}

					// Setup Node
					node.transform.position = new Vector3(posRow, 0, posColumn);
					node.SetIndex(rowIndex, columnIndex);
					nodes.Add(node);
					nodesTransforms.Add(node.transform);

					// Next grid data
					gridDataReadIndex++;

					// Next row position
					posRow += spaceBetweenRows;
				}

				// Reset row position, next column position
				posRow = startRowPos;
				posColumn += spaceBetweenColumns;
			}
		}

		/// <summary>
		/// Get the list of nodes transforms
		/// </summary>
		/// <returns>The list of nodes transforms</returns>
		public List<Transform> GetNodeTransforms()
		{
			return nodesTransforms;
		}

		/// <summary>
		/// Get the base node with given row index and column index
		/// </summary>
		/// <param name="rowIndex"></param>
		/// <param name="columnIndex"></param>
		/// <returns>BaseNode with given row and column index</returns>
		public BaseNode GetBaseNode(int rowIndex, int columnIndex)
		{
			return nodes.Find(node => node.rowIndex == rowIndex && node.columnIndex == columnIndex);
		}

		/// <summary>
		/// Calculate the tongue path from given start row index, start column index and travel direction
		/// </summary>
		/// <param name="startRowIndex"></param>
		/// <param name="startColumnIndex"></param>
		/// <param name="direction"></param>
		/// <returns>List of Transforms along the path</returns>
		public List<Transform> GetPath(int startRowIndex, int startColumnIndex, FacingDirection direction)
		{
			// Reset variables and assign to starting indexes
			path.Clear();
			int currentRowIndex = startRowIndex;
			int currentColumnIndex = startColumnIndex;

			// Traverse the grid until out of bounds
			while (currentRowIndex >= 0 && currentRowIndex < rowSize && currentColumnIndex >= 0 && currentColumnIndex < columnSize)
			{
				BaseNode currentNode = GetBaseNode(currentRowIndex, currentColumnIndex);

				if (currentNode.TryGetComponent<GrapeNode>(out GrapeNode grapeNode))
				{
					path.Add(currentNode.transform);
				}
				else if (currentNode.TryGetComponent<ArrowNode>(out ArrowNode arrowNode))
				{
					path.Add(currentNode.transform);

					// Update travel direction
					direction = arrowNode.GetFacingDirection();
				}
				else if (currentNode.TryGetComponent<FrogNode>(out FrogNode frogNode))
				{
					path.Add(currentNode.transform);
				}

				// Move to next index regarding the direction
				switch (direction)
				{
					case FacingDirection.Up:
						currentRowIndex--;
						break;

					case FacingDirection.Down:
						currentRowIndex++;
						break;

					case FacingDirection.Left:
						currentColumnIndex--;
						break;

					case FacingDirection.Right:
						currentColumnIndex++;
						break;
				}
			}

			return path;
		}
	}

	[Serializable]
	public class LevelDatas
	{
		public LevelData[] levelData;
	}

	[Serializable]
	public class LevelData
	{
		public int levelId;
		public int[] gridSize;
		public string[] gridData;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class Grid : MonoBehaviour
	{
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
		private List<BaseNode> nodes = new List<BaseNode>();
		private List<BaseNode> frogNodes = new List<BaseNode>();
		private List<Transform> nodesTransforms = new List<Transform>();
		private List<Transform> path = new List<Transform>();

		/// <summary>
		/// Create a grid with given parameters
		/// </summary>
		/// <param name="rows"></param>
		/// <param name="columns"></param>
		public void FillGrid(int rows, int columns, LevelData level)
		{
			nodes.Clear();
			nodesTransforms.Clear();
			frogNodes.Clear();

			int gridDataReadIndex = 0;

			rowSize = rows;
			columnSize = columns;

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
							frogNodes.Add(node);
							break;

						// Frog Facing DOWN
						case "FD":
							node = Instantiate(frogNodePrefab, transform);
							((FrogNode)node).SetFacingDirection(FacingDirection.Down);
							frogNodes.Add(node);
							break;

						// Frog Facing LEFT
						case "FL":
							node = Instantiate(frogNodePrefab, transform);
							((FrogNode)node).SetFacingDirection(FacingDirection.Left);
							frogNodes.Add(node);
							break;

						// Frog Facing RIGHT
						case "FR":
							node = Instantiate(frogNodePrefab, transform);
							((FrogNode)node).SetFacingDirection(FacingDirection.Right);
							frogNodes.Add(node);
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

			ColorNodes();
		}

		/// <summary>
		/// Set the color of nodes in the grid regarding frog color and face direction
		/// </summary>
		private void ColorNodes()
		{
			Colors frogColor;
			FacingDirection dir;
			int currentRowIndex;
			int currentColumnIndex;

			for (int i = 0; i < frogNodes.Count; i++)
			{
				frogColor = frogNodes[i].chosenColor;
				dir = frogNodes[i].GetFacingDirection();
				currentRowIndex = frogNodes[i].rowIndex;
				currentColumnIndex = frogNodes[i].columnIndex;

				// Traverse the grid until out of bounds
				while (currentRowIndex >= 0 && currentRowIndex < rowSize && currentColumnIndex >= 0 && currentColumnIndex < columnSize)
				{
					GetBaseNode(currentRowIndex, currentColumnIndex).SetColor(frogColor);

					// Move to next index regarding the direction
					switch (dir)
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
		public List<Transform> GetPath(int startRowIndex, int startColumnIndex, FacingDirection direction, Colors color)
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

					// If not same color, return
					if (grapeNode.chosenColor != color)
						return path;

				}
				else if (currentNode.TryGetComponent<ArrowNode>(out ArrowNode arrowNode))
				{
					path.Add(currentNode.transform);

					// If same color, update travel direction
					if (arrowNode.chosenColor == color)
						direction = arrowNode.GetFacingDirection();

					else
						return path;
				}
				else if (currentNode.TryGetComponent<FrogNode>(out FrogNode frogNode))
				{
					// If this is the starting node, add to path
					if (frogNode.rowIndex == startRowIndex && frogNode.columnIndex == startColumnIndex)
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

		/// <summary>
		/// Get the total number of frog
		/// </summary>
		/// <returns># of frogs on the grid</returns>
		public int GetFrogCount()
		{
			return frogNodes.Count;
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
		public int moves;
		public int[] gridSize;
		public string[] gridData;
	}
}

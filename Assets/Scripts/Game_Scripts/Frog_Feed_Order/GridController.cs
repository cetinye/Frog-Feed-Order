using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class GridController : MonoBehaviour
	{
		[Header("Grid Variables")]
		[SerializeField] private GrapeNode grapeNodePrefab;
		[SerializeField] private ArrowNode arrowNodePrefab;
		[SerializeField] private FrogNode frogNodePrefab;
		[SerializeField] private float startRowPos;
		[SerializeField] private float startColumnPos;
		[SerializeField] private float spaceBetweenRows;
		[SerializeField] private float spaceBetweenColumns;
		[SerializeField] private float spaceBetweenNodes;
		private int rowSize;
		private int columnSize;
		private int minLayerIndex = 0;

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
			float yPos = 0;
			int layerIndex = 0;

			for (int rowIndex = 0; rowIndex < rows; rowIndex++)
			{
				for (int columnIndex = 0; columnIndex < columns; columnIndex++)
				{
					// Access the current NodeData
					NodeData currentNodeData = level.gridData[gridDataReadIndex];
					layerIndex = 0;
					yPos = 0;
					BaseNode previousNode = null;

					// Loop through the data array of the current NodeData
					for (int cellIndex = 0; cellIndex < currentNodeData.data.Length; cellIndex++)
					{
						BaseNode node = null;

						// Determine the type of node to instantiate based on the data
						switch (currentNodeData.data[cellIndex])
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

							// Handle invalid grid data
							default:
								Debug.Log("Invalid grid data: " + currentNodeData.data[cellIndex]);
								break;
						}

						// Setup Node
						if (node != null)
						{
							node.transform.position = new Vector3(posRow, yPos, posColumn);
							node.SetIndex(rowIndex, columnIndex, layerIndex);
							nodes.Add(node);
							nodesTransforms.Add(node.transform);

							// if (layerIndex != 0)
							// 	node.NodeOff();

							if (layerIndex != 0 && layerIndex > -currentNodeData.data.Length)
								previousNode.nodeUnder = node;

							previousNode = node;

							layerIndex--;
							yPos -= spaceBetweenNodes;

							minLayerIndex = Mathf.Min(minLayerIndex, layerIndex);
						}
					}

					// Move to the next grid data
					gridDataReadIndex++;

					// Adjust the position for the next row
					posRow += spaceBetweenRows;
				}

				// Reset row position and adjust for the next column
				posRow = startRowPos;
				posColumn += spaceBetweenColumns;
			}

			// Apply colors to the nodes
			ColorNodes();

			// Hide all nodes, except top layer nodes
			HideNodes();
		}

		/// <summary>
		/// Set the color of nodes in the grid regarding frog color and face direction
		/// </summary>
		private void ColorNodes()
		{
			List<BaseNode> remainingNodes = new List<BaseNode>(nodes);
			Colors frogColor;
			FacingDirection dir;
			int currentRowIndex;
			int currentColumnIndex;
			int currentLayerIndex;

			for (int i = 0; i < frogNodes.Count; i++)
			{
				frogColor = frogNodes[i].chosenColor;
				dir = frogNodes[i].GetFacingDirection();
				currentRowIndex = frogNodes[i].rowIndex;
				currentColumnIndex = frogNodes[i].columnIndex;
				currentLayerIndex = frogNodes[i].layerIndex;

				// Traverse the grid until out of bounds
				while (currentRowIndex >= 0 && currentRowIndex < rowSize && currentColumnIndex >= 0 && currentColumnIndex < columnSize)
				{
					BaseNode node = GetBaseNode(currentRowIndex, currentColumnIndex, remainingNodes);
					node.SetColor(frogColor);
					node.cell.SetColor(frogColor);
					remainingNodes.Remove(node);

					// Update direction faced arrow node
					if (node.GetType() == typeof(ArrowNode))
					{
						Debug.Log("Arrow Node: " + ((ArrowNode)node).GetFacingDirection());
						Debug.Log("CurrentRowIndex: " + currentRowIndex + " CurrentColumnIndex: " + currentColumnIndex);

						dir = ((ArrowNode)node).GetFacingDirection();
					}

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

				// Override falsely colored cells
				frogNodes[i].UpdateColor();
			}
		}

		/// <summary>
		/// Hide all nodes except top layer nodes
		/// </summary>
		private void HideNodes()
		{
			foreach (BaseNode node in nodes)
			{
				// If not top layer, hide
				if (node.layerIndex != 0)
					node.NodeOff();
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
			for (int cellIndex = 0; cellIndex < nodes.Count; cellIndex++)
			{
				if (nodes[cellIndex].rowIndex == rowIndex && nodes[cellIndex].columnIndex == columnIndex)
				{
					return nodes[cellIndex];
				}
			}

			return null;
		}

		/// <summary>
		/// Get the base node with given row index, column index and layer index
		/// </summary>
		/// <param name="rowIndex"></param>
		/// <param name="columnIndex"></param>
		/// <param name="layerIndex"></param>
		/// <returns>BaseNode with given row, column and layer index</returns>
		public BaseNode GetBaseNode(int rowIndex, int columnIndex, int layerIndex)
		{
			for (int cellIndex = 0; cellIndex < nodes.Count; cellIndex++)
			{
				if (nodes[cellIndex].layerIndex == layerIndex && nodes[cellIndex].rowIndex == rowIndex && nodes[cellIndex].columnIndex == columnIndex)
				{
					return nodes[cellIndex];
				}
			}

			return null;
		}

		/// <summary>
		/// Get the base node with given row index, column index and layer index
		/// </summary>
		/// <param name="rowIndex"></param>
		/// <param name="columnIndex"></param>
		/// <param name="layerIndex"></param>
		/// <returns>BaseNode with given row, column and layer index</returns>
		public BaseNode GetBaseNode(int rowIndex, int columnIndex, List<BaseNode> insideList)
		{
			for (int cellIndex = 0; cellIndex < insideList.Count; cellIndex++)
			{
				if (insideList[cellIndex].rowIndex == rowIndex && insideList[cellIndex].columnIndex == columnIndex)
				{
					return insideList[cellIndex];
				}
			}

			return null;
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

		/// <summary>
		/// Remove the given node from the path calculation list
		/// </summary>
		/// <param name="node"></param>
		public void RemoveNode(BaseNode node)
		{
			nodes.Remove(node);
		}

		public int GetMinLayerIndex()
		{
			return minLayerIndex;
		}

		public void Reset()
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				Destroy(transform.GetChild(i).gameObject);
			}

			nodes.Clear();
			nodesTransforms.Clear();
			frogNodes.Clear();
		}
	}

	[Serializable]
	public class LevelDatas
	{
		public LevelData[] levelData;
	}

	[Serializable]
	public class NodeData
	{
		public string[] data;
	}

	[Serializable]
	public class LevelData
	{
		public int levelId;
		public int moves;
		public int[] gridSize;
		public NodeData[] gridData;
	}
}

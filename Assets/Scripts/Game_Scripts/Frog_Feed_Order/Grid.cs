using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class Grid : MonoBehaviour
	{
		[Header("Grid Variables")]
		[SerializeField] private BaseNode nodePrefab;
		[SerializeField] private int rowSize;
		[SerializeField] private int columnSize;
		[SerializeField] private float startRowPos;
		[SerializeField] private float startColumnPos;
		[SerializeField] private float spaceBetweenRows;
		[SerializeField] private float spaceBetweenColumns;

		[Header("Node Variables")]
		[SerializeField] private List<BaseNode> nodes = new List<BaseNode>();
		private List<Transform> nodesTransforms = new List<Transform>();

		void Awake()
		{
			FillGrid(rowSize, columnSize);
		}

		/// <summary>
		/// Create a grid with given parameters
		/// </summary>
		/// <param name="rows"></param>
		/// <param name="columns"></param>
		private void FillGrid(int rows, int columns)
		{
			float posRow = startRowPos;
			float posColumn = startColumnPos;

			for (int rowIndex = 0; rowIndex < rows; rowIndex++)
			{
				for (int columnIndex = 0; columnIndex < columns; columnIndex++)
				{
					BaseNode node = Instantiate(nodePrefab, transform);
					node.transform.position = new Vector3(posRow, 0, posColumn);

					node.SetIndex(rowIndex, columnIndex);
					nodes.Add(node);
					nodesTransforms.Add(node.transform);

					posRow += spaceBetweenRows;
				}

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
	}
}

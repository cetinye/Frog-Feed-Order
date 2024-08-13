using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class Grid : MonoBehaviour
	{
		public int levelId;

		[Header("JSON")]
		[SerializeField] private TextAsset levelDatasJSON;
		[SerializeField] private LevelDatas levelDatas;
		[SerializeField] private LevelData level;

		[Header("Grid Variables")]
		[SerializeField] private GrapeNode grapeNodePrefab;
		[SerializeField] private ArrowNode arrowNodePrefab;
		[SerializeField] private FrogNode frogNodePrefab;
		private int rowSize;
		private int columnSize;
		[SerializeField] private float startRowPos;
		[SerializeField] private float startColumnPos;
		[SerializeField] private float spaceBetweenRows;
		[SerializeField] private float spaceBetweenColumns;

		[Header("Node Variables")]
		[SerializeField] private List<BaseNode> nodes = new List<BaseNode>();
		private List<Transform> nodesTransforms = new List<Transform>();

		void Awake()
		{
			ReadJSON();
			FillGrid(rowSize, columnSize);
		}

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
						case 1:
							node = Instantiate(grapeNodePrefab, transform);
							break;
						case 2:
							node = Instantiate(arrowNodePrefab, transform);
							break;
						case 3:
							node = Instantiate(frogNodePrefab, transform);
							break;
						default:
							Debug.Log("Invalid grid data: " + level.gridData[gridDataReadIndex]);
							break;
					}

					node.transform.position = new Vector3(posRow, 0, posColumn);
					node.SetIndex(rowIndex, columnIndex);
					nodes.Add(node);
					nodesTransforms.Add(node.transform);

					gridDataReadIndex++;

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
		public int[] gridData;
	}
}

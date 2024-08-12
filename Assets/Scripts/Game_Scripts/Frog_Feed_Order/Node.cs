using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class Node : MonoBehaviour
	{
		[SerializeField] private Cell cell;
		[SerializeField] private Grape grape;
		[SerializeField] private int rowIndex;
		[SerializeField] private int columnIndex;

		void Awake()
		{
			RandomizeColor();
		}

		/// <summary>
		/// Set the index of the node
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		public void SetIndex(int row, int column)
		{
			rowIndex = row;
			columnIndex = column;
		}

		/// <summary>
		/// Randomize the color of the nodes cell and grape
		/// </summary>
		public void RandomizeColor()
		{
			Colors randomColor = (Colors)Random.Range(0, System.Enum.GetNames(typeof(Colors)).Length);

			cell.SetColor(randomColor);
			grape.SetColor(randomColor);
		}
	}

	public enum Colors
	{
		Blue = 0,
		Green = 1,
		Purple = 2,
		Red = 3,
		Yellow = 4
	}

}

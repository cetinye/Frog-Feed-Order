using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class Node : MonoBehaviour
	{
		[SerializeField] private Cell cell;
		[SerializeField] private Grape grape;

		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
		void Awake()
		{
			RandomizeColor();
		}

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class BaseNode : MonoBehaviour
	{
		public Cell cell;

		[Header("Index Variables")]
		public int rowIndex;
		public int columnIndex;

		[Header("Color Variables")]
		public Colors chosenColor;
		public Renderer meshRenderer;
		public List<Material> materials = new List<Material>();

		/// <summary>
		/// Set the index of the node
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		public void SetIndex(int row, int column)
		{
			rowIndex = row;
			columnIndex = column;

			gameObject.name = $"Node ({rowIndex}, {columnIndex})";
		}

		/// <summary>
		/// Sets the color of the grape
		/// </summary>
		/// <param name="color"></param>
		public void SetColor(Colors color)
		{
			chosenColor = color;

			Material[] rendererMaterials = meshRenderer.materials;
			rendererMaterials[0] = Instantiate(materials[(int)color]);
			meshRenderer.materials = rendererMaterials;
		}

		/// <summary>
		/// Randomize the color of the nodes cell and grape
		/// </summary>
		public void RandomizeColor()
		{
			Colors randomColor = (Colors)Random.Range(0, System.Enum.GetNames(typeof(Colors)).Length);

			cell.SetColor(randomColor);
			SetColor(randomColor);
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

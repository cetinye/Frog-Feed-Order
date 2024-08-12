using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class Cell : MonoBehaviour
	{
		[Header("Color Variables")]
		[SerializeField] private MeshRenderer meshRenderer;
		[SerializeField] private List<Material> materials = new List<Material>();
		[SerializeField] private Colors chosenColor;

		/// <summary>
		/// Sets the color of the cell
		/// </summary>
		/// <param name="color"></param>
		public void SetColor(Colors color)
		{
			chosenColor = color;

			Material[] rendererMaterials = meshRenderer.materials;
			rendererMaterials[0] = Instantiate(materials[(int)color]);
			meshRenderer.materials = rendererMaterials;
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class BaseNode : MonoBehaviour
	{
		public Cell cell;
		public Transform item;
		public BaseNode nodeUnder;
		public Action<Colors> OnVisit;
		public Action<int, float, Transform> OnRetract;

		[Header("Index Variables")]
		public int rowIndex;
		public int columnIndex;
		public int layerIndex;

		[Header("Color Variables")]
		public Colors chosenColor;
		public Renderer meshRenderer;
		public List<Material> materials = new List<Material>();

		[Header("Direction Variable")]
		public FacingDirection facingDirection;

		/// <summary>
		/// Set the index of the node
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		public void SetIndex(int row, int column, int layer)
		{
			rowIndex = row;
			columnIndex = column;
			layerIndex = layer;

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
			Colors randomColor = (Colors)UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(Colors)).Length);

			cell.SetColor(randomColor);
			SetColor(randomColor);
		}

		/// <summary>
		/// Sets the facing direction of the node
		/// </summary>
		/// <param name="direction"></param>
		public void SetFacingDirection(FacingDirection direction)
		{
			switch (direction)
			{
				case FacingDirection.Up:
					facingDirection = FacingDirection.Up;
					transform.localRotation = Quaternion.Euler(0, 180, 0);
					break;

				case FacingDirection.Down:
					facingDirection = FacingDirection.Down;
					transform.localRotation = Quaternion.Euler(0, 0, 0);
					break;

				case FacingDirection.Left:
					facingDirection = FacingDirection.Left;
					transform.localRotation = Quaternion.Euler(0, 90, 0);
					break;

				case FacingDirection.Right:
					facingDirection = FacingDirection.Right;
					transform.localRotation = Quaternion.Euler(0, -90, 0);
					break;
			}
		}

		public void NodeOff()
		{
			item.gameObject.SetActive(false);
		}

		public void NodeOn()
		{
			layerIndex = 0;

			item.transform.localScale = Vector3.zero;
			item.gameObject.SetActive(true);
			item.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.Linear);
		}

		/// <summary>
		/// Gets the facing direction of the node
		/// </summary>
		/// <returns>Facing Direction of the node</returns>
		public FacingDirection GetFacingDirection()
		{
			return facingDirection;
		}

		/// <summary>
		/// Virtual function for when the different nodes are visited
		/// </summary>
		public virtual void OnNodeVisited(Colors colors)
		{

		}

		/// <summary>
		/// Virtual function for when the different nodes are retracted
		/// </summary>
		public virtual void OnNodeRetracted(int index, float time, Transform targetPos)
		{

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

	public enum FacingDirection
	{
		Up = 0,
		Down = 1,
		Left = 2,
		Right = 3
	}
}

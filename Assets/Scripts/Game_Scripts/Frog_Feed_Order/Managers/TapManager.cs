using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class TapManager : MonoBehaviour
	{
		[SerializeField] private Camera raycastCamera;

		/// <summary>
		/// Listen for inputs
		/// </summary>
		void Update()
		{
			CheckTap();
		}

		/// <summary>
		/// Perform action if user tapped on an clickable object
		/// </summary>
		private void CheckTap()
		{
			if (Input.GetMouseButtonDown(1))
			{
				Ray raycast = raycastCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit raycastHit;

				if (Physics.Raycast(raycast, out raycastHit))
				{
					if (raycastHit.collider.TryGetComponent<IClickable>(out IClickable clickable))
					{
						clickable.Clicked();
					}
				}
			}
		}
	}
}

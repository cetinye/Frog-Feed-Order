using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class TapManager : MonoBehaviour
	{
		[SerializeField] private Camera raycastCamera;

		void Update()
		{
			CheckTap();
		}

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

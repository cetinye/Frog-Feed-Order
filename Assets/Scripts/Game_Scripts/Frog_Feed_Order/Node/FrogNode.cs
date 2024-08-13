using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class FrogNode : BaseNode, IClickable
	{
		void Awake()
		{
			RandomizeColor();
		}

		public void Clicked()
		{
			Debug.Log("Clicked Frog (" + rowIndex + ", " + columnIndex + ")");
		}
	}
}

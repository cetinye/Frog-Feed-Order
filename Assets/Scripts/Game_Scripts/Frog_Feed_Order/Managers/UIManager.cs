using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField] private TMP_Text levelText;
		[SerializeField] private TMP_Text movesText;
		[SerializeField] private GameObject failPanel;
		[SerializeField] private GameObject winPanel;

		/// <summary>
		/// Sets the level text
		/// </summary>
		/// <param name="level"></param>
		public void SetLevelText(int level)
		{
			levelText.text = "LEVEL " + (level + 1).ToString();
		}

		/// <summary>
		/// Sets the moves text
		/// </summary>
		/// <param name="moves"></param>
		public void SetMovesText(int moves)
		{
			movesText.text = moves.ToString() + " MOVES";
		}

		public void SetFailPanel(bool state)
		{
			failPanel.SetActive(state);
		}

		public void SetWinPanel(bool state)
		{
			winPanel.SetActive(state);
		}

		public void Reset()
		{
			failPanel.SetActive(false);
			winPanel.SetActive(false);
		}
	}
}

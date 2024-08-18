using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Frog_Feed_Order
{
	public class ArrowNode : BaseNode
	{
		private LevelManager levelManager;

		void OnEnable()
		{
			OnVisit += OnNodeVisited;
			OnRetract += OnNodeRetracted;
		}

		void OnDisable()
		{
			OnVisit -= OnNodeVisited;
			OnRetract -= OnNodeRetracted;
		}

		void Start()
		{
			levelManager = LevelManager.Instance;
		}

		/// <summary>
		/// Animate when tongue reaches the node. Invoked when the node is retracted.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="time"></param>
		public override void OnNodeRetracted(int index, float time, Transform target)
		{
			float newTime = time * index;
			transform.DOScale(Vector3.zero, newTime + time).SetEase(Ease.Linear).OnComplete(() =>
			{
				if (nodeUnder != null)
					nodeUnder.NodeOn();

				levelManager.RemoveNode(this);
				gameObject.SetActive(false);
			});
		}
	}
}

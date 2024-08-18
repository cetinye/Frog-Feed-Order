using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class GrapeNode : BaseNode
	{
		[SerializeField] private Transform item;

		[Header("Tween Variables")]
		[SerializeField] private Vector3 scaleTo;
		[SerializeField] private float timeToScale;
		private Tween onVisitAnimation;
		private Sequence onRetractAnimation;

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

		/// <summary>
		/// Animate when tongue reaches the node. Invoked when the node is visited.
		/// </summary>
		public override void OnNodeVisited()
		{
			onVisitAnimation?.Kill();
			onVisitAnimation = meshRenderer.transform.DOScale(scaleTo, timeToScale).SetLoops(2, LoopType.Yoyo);
		}

		/// <summary>
		/// Animate when tongue reaches the node. Invoked when the node is retracted.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="time"></param>
		public override void OnNodeRetracted(int index, float time)
		{
			onRetractAnimation?.Kill();

			float newTime = time * index;

			onRetractAnimation = DOTween.Sequence();
			onRetractAnimation.Append(meshRenderer.transform.DOScale(transform.localScale, newTime));
			onRetractAnimation.Append(meshRenderer.transform.DOScale(Vector3.zero, time));
			onRetractAnimation.Play();
		}

		/// <summary>
		/// Get the item transform
		/// </summary>
		/// <returns>Transform of the item</returns>
		public Transform GetItem()
		{
			return item;
		}
	}
}

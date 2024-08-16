using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class GrapeNode : BaseNode
	{
		[Header("Tween Variables")]
		[SerializeField] private Vector3 scaleTo;
		[SerializeField] private float timeToScale;
		private Tween onVisitAnimation;

		void Awake()
		{
			RandomizeColor();
		}

		void OnEnable()
		{
			OnVisit += OnNodeVisited;
		}

		void OnDisable()
		{
			OnVisit -= OnNodeVisited;
		}

		/// <summary>
		/// Animate when tongue reaches the node. Invoked when the node is visited.
		/// </summary>
		public override void OnNodeVisited()
		{
			onVisitAnimation?.Kill();
			onVisitAnimation = meshRenderer.transform.DOScale(scaleTo, timeToScale).SetLoops(2, LoopType.Yoyo);
		}
	}
}

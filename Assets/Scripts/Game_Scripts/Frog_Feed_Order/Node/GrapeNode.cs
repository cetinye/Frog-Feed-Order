using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Frog_Feed_Order
{
	public class GrapeNode : BaseNode
	{
		private LevelManager levelManager;

		[Header("Tween Variables")]
		[SerializeField] private Vector3 scaleTo;
		[SerializeField] private float timeToScale;
		private Sequence onVisitAnimation;
		private Sequence onRetractAnimation;

		[Header("Retract MoveTowards Variables")]
		private Transform targetTransform;
		private float retractSpeed = 4f;
		private bool isRetracted = false;

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

		void Update()
		{
			// if node is retracted, move item towards target
			if (isRetracted)
			{
				item.transform.position = Vector3.MoveTowards(item.transform.position, targetTransform.position, retractSpeed * Time.deltaTime);
			}
		}

		/// <summary>
		/// Animate when tongue reaches the node. Invoked when the node is visited.
		/// </summary>
		public override void OnNodeVisited(Colors color)
		{
			onVisitAnimation?.Kill();

			onVisitAnimation = DOTween.Sequence();

			onVisitAnimation.Append(meshRenderer.transform.DOScale(scaleTo, timeToScale).SetLoops(2, LoopType.Yoyo));

			if (color != chosenColor)
				onVisitAnimation.Join(meshRenderer.materials[0].DOColor(Color.red, 0.2f).SetLoops(2, LoopType.Yoyo));
		}

		/// <summary>
		/// Animate when tongue reaches the node. Invoked when the node is retracted.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="time"></param>
		public override void OnNodeRetracted(int index, float time, Transform target)
		{
			onRetractAnimation?.Kill();

			targetTransform = target;
			isRetracted = true;

			float newTime = time * index;

			onRetractAnimation = DOTween.Sequence();
			onRetractAnimation.Append(meshRenderer.transform.DOScale(transform.localScale, newTime));
			onRetractAnimation.Join(cell.transform.DOScale(Vector3.zero, newTime + time).SetEase(Ease.Linear));
			onRetractAnimation.Append(meshRenderer.transform.DOScale(Vector3.zero, time).SetEase(Ease.Linear));
			onRetractAnimation.OnComplete(() =>
			{
				if (nodeUnder != null)
					nodeUnder.NodeOn();

				levelManager.RemoveNode(this);
				gameObject.SetActive(false);
			});
		}
	}
}

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Gets info on the last particle collision event. See Unity Particle System docs.")]
	public class GetParticleCollisionInfo : FsmStateAction
	{
		[Tooltip("Get the GameObject hit.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject gameObjectHit;

		public override void Reset()
		{
			gameObjectHit = null;
		}

		private void StoreCollisionInfo()
		{
			gameObjectHit.Value = base.Fsm.ParticleCollisionGO;
		}

		public override void OnEnter()
		{
			StoreCollisionInfo();
			Finish();
		}
	}
}

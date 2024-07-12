namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Gets info on the last particle collision event. See Unity Particle System docs.")]
	[ActionCategory(ActionCategory.Physics)]
	public class GetParticleCollisionInfo : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("Get the GameObject hit.")]
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

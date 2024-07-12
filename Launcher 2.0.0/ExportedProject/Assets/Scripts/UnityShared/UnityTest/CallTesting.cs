using UnityEngine;

namespace UnityTest
{
	public class CallTesting : MonoBehaviour
	{
		public enum Functions
		{
			CallAfterSeconds = 0,
			CallAfterFrames = 1,
			Start = 2,
			Update = 3,
			FixedUpdate = 4,
			LateUpdate = 5,
			OnDestroy = 6,
			OnEnable = 7,
			OnDisable = 8,
			OnControllerColliderHit = 9,
			OnParticleCollision = 10,
			OnJointBreak = 11,
			OnBecameInvisible = 12,
			OnBecameVisible = 13,
			OnTriggerEnter = 14,
			OnTriggerExit = 15,
			OnTriggerStay = 16,
			OnCollisionEnter = 17,
			OnCollisionExit = 18,
			OnCollisionStay = 19,
			OnTriggerEnter2D = 20,
			OnTriggerExit2D = 21,
			OnTriggerStay2D = 22,
			OnCollisionEnter2D = 23,
			OnCollisionExit2D = 24,
			OnCollisionStay2D = 25
		}

		public enum Method
		{
			Pass = 0,
			Fail = 1
		}

		public int afterFrames = 0;

		public float afterSeconds = 0f;

		public Functions callOnMethod = Functions.Start;

		public Method methodToCall;

		private int m_StartFrame;

		private float m_StartTime;

		private void TryToCallTesting(Functions invokingMethod)
		{
			if (invokingMethod == callOnMethod)
			{
				if (methodToCall == Method.Pass)
				{
					IntegrationTest.Pass(base.gameObject);
				}
				else
				{
					IntegrationTest.Fail(base.gameObject);
				}
				afterFrames = 0;
				afterSeconds = 0f;
				m_StartTime = float.PositiveInfinity;
				m_StartFrame = int.MinValue;
			}
		}

		public void Start()
		{
			m_StartTime = Time.time;
			m_StartFrame = afterFrames;
			TryToCallTesting(Functions.Start);
		}

		public void Update()
		{
			TryToCallTesting(Functions.Update);
			CallAfterSeconds();
			CallAfterFrames();
		}

		private void CallAfterFrames()
		{
			if (afterFrames > 0 && m_StartFrame + afterFrames <= Time.frameCount)
			{
				TryToCallTesting(Functions.CallAfterFrames);
			}
		}

		private void CallAfterSeconds()
		{
			if (m_StartTime + afterSeconds <= Time.time)
			{
				TryToCallTesting(Functions.CallAfterSeconds);
			}
		}

		public void OnDisable()
		{
			TryToCallTesting(Functions.OnDisable);
		}

		public void OnEnable()
		{
			TryToCallTesting(Functions.OnEnable);
		}

		public void OnDestroy()
		{
			TryToCallTesting(Functions.OnDestroy);
		}

		public void FixedUpdate()
		{
			TryToCallTesting(Functions.FixedUpdate);
		}

		public void LateUpdate()
		{
			TryToCallTesting(Functions.LateUpdate);
		}

		public void OnControllerColliderHit()
		{
			TryToCallTesting(Functions.OnControllerColliderHit);
		}

		public void OnParticleCollision()
		{
			TryToCallTesting(Functions.OnParticleCollision);
		}

		public void OnJointBreak()
		{
			TryToCallTesting(Functions.OnJointBreak);
		}

		public void OnBecameInvisible()
		{
			TryToCallTesting(Functions.OnBecameInvisible);
		}

		public void OnBecameVisible()
		{
			TryToCallTesting(Functions.OnBecameVisible);
		}

		public void OnTriggerEnter()
		{
			TryToCallTesting(Functions.OnTriggerEnter);
		}

		public void OnTriggerExit()
		{
			TryToCallTesting(Functions.OnTriggerExit);
		}

		public void OnTriggerStay()
		{
			TryToCallTesting(Functions.OnTriggerStay);
		}

		public void OnCollisionEnter()
		{
			TryToCallTesting(Functions.OnCollisionEnter);
		}

		public void OnCollisionExit()
		{
			TryToCallTesting(Functions.OnCollisionExit);
		}

		public void OnCollisionStay()
		{
			TryToCallTesting(Functions.OnCollisionStay);
		}

		public void OnTriggerEnter2D()
		{
			TryToCallTesting(Functions.OnTriggerEnter2D);
		}

		public void OnTriggerExit2D()
		{
			TryToCallTesting(Functions.OnTriggerExit2D);
		}

		public void OnTriggerStay2D()
		{
			TryToCallTesting(Functions.OnTriggerStay2D);
		}

		public void OnCollisionEnter2D()
		{
			TryToCallTesting(Functions.OnCollisionEnter2D);
		}

		public void OnCollisionExit2D()
		{
			TryToCallTesting(Functions.OnCollisionExit2D);
		}

		public void OnCollisionStay2D()
		{
			TryToCallTesting(Functions.OnCollisionStay2D);
		}
	}
}

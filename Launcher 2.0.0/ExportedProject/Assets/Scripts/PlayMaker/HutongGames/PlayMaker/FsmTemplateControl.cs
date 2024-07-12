using System;
using System.Collections.Generic;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmTemplateControl
	{
		public FsmTemplate fsmTemplate;

		public FsmVarOverride[] fsmVarOverrides = new FsmVarOverride[0];

		[NonSerialized]
		private Fsm runFsm;

		public int ID { get; set; }

		public Fsm RunFsm
		{
			get
			{
				return runFsm;
			}
			private set
			{
				runFsm = value;
			}
		}

		public FsmTemplateControl()
		{
			fsmVarOverrides = new FsmVarOverride[0];
		}

		public FsmTemplateControl(FsmTemplateControl source)
		{
			fsmTemplate = source.fsmTemplate;
			fsmVarOverrides = CopyOverrides(source);
		}

		public void SetFsmTemplate(FsmTemplate template)
		{
			fsmTemplate = template;
			ClearOverrides();
			UpdateOverrides();
		}

		public Fsm InstantiateFsm()
		{
			RunFsm = new Fsm(fsmTemplate.fsm);
			ApplyOverrides(RunFsm);
			return RunFsm;
		}

		private static FsmVarOverride[] CopyOverrides(FsmTemplateControl source)
		{
			FsmVarOverride[] array = new FsmVarOverride[source.fsmVarOverrides.Length];
			for (int i = 0; i < source.fsmVarOverrides.Length; i++)
			{
				array[i] = new FsmVarOverride(source.fsmVarOverrides[i]);
			}
			return array;
		}

		private void ClearOverrides()
		{
			fsmVarOverrides = new FsmVarOverride[0];
		}

		public void UpdateOverrides()
		{
			if (fsmTemplate != null)
			{
				List<FsmVarOverride> list = new List<FsmVarOverride>(fsmVarOverrides);
				List<FsmVarOverride> list2 = new List<FsmVarOverride>();
				NamedVariable[] allNamedVariables = fsmTemplate.fsm.Variables.GetAllNamedVariables();
				foreach (NamedVariable namedVariable in allNamedVariables)
				{
					if (namedVariable.ShowInInspector)
					{
						FsmVarOverride fsmVarOverride = list.Find((FsmVarOverride o) => o.variable.Name == namedVariable.Name);
						list2.Add(fsmVarOverride ?? new FsmVarOverride(namedVariable));
					}
				}
				fsmVarOverrides = list2.ToArray();
			}
			else
			{
				fsmVarOverrides = new FsmVarOverride[0];
			}
		}

		public void UpdateValues()
		{
			FsmVarOverride[] array = fsmVarOverrides;
			foreach (FsmVarOverride fsmVarOverride in array)
			{
				fsmVarOverride.fsmVar.UpdateValue();
			}
		}

		public void ApplyOverrides(Fsm overrideFsm)
		{
			FsmVarOverride[] array = fsmVarOverrides;
			foreach (FsmVarOverride fsmVarOverride in array)
			{
				fsmVarOverride.Apply(overrideFsm.Variables);
			}
		}
	}
}

using HutongGames.PlayMaker;
using UnityEngine;

[ExecuteInEditMode]
public class PlayMakerOnGUI : MonoBehaviour
{
	public PlayMakerFSM playMakerFSM;

	public bool previewInEditMode = true;

	public void Start()
	{
		if (playMakerFSM != null)
		{
			playMakerFSM.Fsm.HandleOnGUI = true;
		}
	}

	public void OnGUI()
	{
		if (previewInEditMode && !Application.isPlaying)
		{
			DoEditGUI();
		}
		else if (playMakerFSM != null && playMakerFSM.Fsm != null && playMakerFSM.Fsm.HandleOnGUI)
		{
			playMakerFSM.Fsm.OnGUI();
		}
	}

	private static void DoEditGUI()
	{
		if (PlayMakerGUI.SelectedFSM == null)
		{
			return;
		}
		FsmState editState = PlayMakerGUI.SelectedFSM.EditState;
		if (editState == null || !editState.IsInitialized)
		{
			return;
		}
		FsmStateAction[] actions = editState.Actions;
		foreach (FsmStateAction fsmStateAction in actions)
		{
			if (fsmStateAction.Active)
			{
				fsmStateAction.OnGUI();
			}
		}
	}
}

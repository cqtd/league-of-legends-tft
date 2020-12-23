using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
	public class SpawnPoint : MonoBehaviour
	{
#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			UnityEditor.Handles.color = Color.black;
			UnityEditor.Handles.Label(transform.position, gameObject.name);
		}
#endif
	}
}
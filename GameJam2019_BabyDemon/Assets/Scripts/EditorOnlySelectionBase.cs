using UnityEngine;

namespace Assets.Editor
{
#if UNITY_EDITOR
	[SelectionBase]
	public class EditorOnlySelectionBase : MonoBehaviour
	{
	}
#endif // UNITY_EDITOR
}

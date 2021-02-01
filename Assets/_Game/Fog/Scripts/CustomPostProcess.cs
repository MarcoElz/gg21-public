using UnityEngine;

namespace Ignita.PostProcess
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public class CustomPostProcess : MonoBehaviour
	{
		[SerializeField] Material postProcessMaterial;
		private void Awake()
		{
			if (postProcessMaterial == null)
			{
				enabled = false;
			}
			else
			{
				// This is on purpose ... it prevents the know bug
				// https://issuetracker.unity3d.com/issues/calling-graphics-dot-blit-destination-null-crashes-the-editor
				// from happening
				postProcessMaterial.mainTexture = postProcessMaterial.mainTexture;
			}

		}

		void OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			Graphics.Blit(src, dest, postProcessMaterial);
		}
	}
}
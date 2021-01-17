using UnityEditor;
using UnityEngine;

namespace Prototype.Editor
{
	public class StaticObjectInspector : UnityEditor.Editor
	{
		public override bool HasPreviewGUI()
		{
			StaticData actor = target as StaticData;
			if (actor == null || actor.icon == null)
				return false;
			
			return true;
		}
		
		public override Texture2D RenderStaticPreview (string assetPath, UnityEngine.Object[] subAssets, int width, int height)
		{
			StaticData actor = target as StaticData;
          
			if (actor == null || actor.icon == null)
				return null;
 
			Texture2D previewTexture = null;
 
			while (previewTexture == null)
			{
				previewTexture = AssetPreview.GetAssetPreview(actor.icon);
			}
          
			Texture2D cache = new Texture2D (width, height);
			EditorUtility.CopySerialized (previewTexture, cache);
			return cache;
		}
	}
}
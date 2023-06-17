using UnityEngine.Rendering;

namespace Kamgam.SettingsGenerator
{
    public static class RenderUtils
    {
        public enum RenderPipe { BuiltIn, URP, HDRP }

        public static RenderPipe GetCurrentRenderPipeline()
        {
            // The render pipeline selection process mirrors how Unity selected the active RP.
            // See "Determining the active render pipeline" under
            // https://docs.unity3d.com/Manual/srp-setting-render-pipeline-asset.html

            // Try to get the render pipeline asset for the current quality.
            RenderPipelineAsset rpa = GraphicsSettings.currentRenderPipeline;
            // If there is none defined for the current quality then fall back on the default (will be null if Built-In is used).
            if (rpa == null)
            {
                rpa = GraphicsSettings.defaultRenderPipeline;
            }
            if (rpa != null)
            {
                switch (rpa.GetType().Name)
                {
                    case "UniversalRenderPipelineAsset": return RenderPipe.URP;
                    case "HDRenderPipelineAsset": return RenderPipe.HDRP;
                }
            }

            return RenderPipe.BuiltIn;
        }
    }
}

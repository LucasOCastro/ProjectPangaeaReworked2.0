using RimWorld;
using Verse;
using UnityEngine;
using System.IO;
using System.Reflection;
using HarmonyLib;

namespace ProjectPangaea
{
    public class BundleShaderTypeDef : ShaderTypeDef
    {
        public string shaderName = "";

        private static FieldInfo shaderCacheInfo = AccessTools.Field(typeof(ShaderTypeDef), "shaderInt");
        public void OverrideShader()
        {
            Shader shader = ProjectPangaeaMod.ShaderBundle.LoadAsset<Shader>(shaderName);
            if (shader == null)
            {
                Log.Error(nameof(BundleShaderTypeDef) + " named " + defName + " with invalid shader name!");
                return;
            }
            shaderCacheInfo.SetValue(this, shader);
        }

        public static void OverrideAllShaders()
        {
            foreach (var def in DefDatabase<BundleShaderTypeDef>.AllDefs)
            {
                def.OverrideShader();
            }
        }
    }
}

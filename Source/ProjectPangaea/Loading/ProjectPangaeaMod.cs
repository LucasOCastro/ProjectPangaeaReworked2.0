using Verse;
using System.Linq;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using System.IO;
using System;
using System.Reflection;

namespace ProjectPangaea
{
    public class ProjectPangaeaMod : Mod
    {
        public static ProjectPangaeaMod Mod { get; private set; }
        public static Harmony Harmony { get; private set; }
        public static PangaeaSettings Settings { get; private set; }

        public static AssetBundle ShaderBundle { get; private set; }

        public ProjectPangaeaMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<PangaeaSettings>();

            Harmony = new Harmony(Content.ModMetaData.PackageId);
            Harmony.PatchAll(Assembly.GetExecutingAssembly());            

            Mod = this;

            LoadBundles();
            Log.Message(1f / 78548f + " and " + Mathf.Pow(78548, -1));

            LongEventHandler.ExecuteWhenFinished(BundleShaderTypeDef.OverrideAllShaders);

            LongEventHandler.ExecuteWhenFinished(ResourceGraphicLister.Init);
            LongEventHandler.ExecuteWhenFinished(PangaeaDatabase.Init);
            LongEventHandler.ExecuteWhenFinished(Production.PangaeaRecipeLister.Init);

        }

        private const string shaderBundlePath = @"Common\Materials\pangaeashaderbundle";
        private void LoadBundles()
        {
            AssetBundle loadBundle(string subPath)
            {
                string path = Path.Combine(Content.RootDir, subPath);
                Log.Message(path);
                AssetBundle bundle = AssetBundle.LoadFromFile(path);
                if (bundle == null)
                {
                    throw new Exception("Null AssetBundle for subpath: " + subPath);
                }
                return bundle;
            }

            ShaderBundle = loadBundle(shaderBundlePath);
        }
        

        public override string SettingsCategory() => Content.Name;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Settings.DoSettingsWindowContents(inRect);
        }
    }
}

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

            LongEventHandler.ExecuteWhenFinished(InitAll);
        }

        private void InitAll()
        {
            BundleShaderTypeDef.OverrideAllShaders();
            ResourceGraphicLister.Init();
            PangaeaDatabase.Init();
            Production.PangaeaRecipeLister.Init();

            Log.Message("Test1A: " + RimWorld.PawnKindDefOf.Boomalope.race.GetIcon().NullOrBad().ToStringSafe());
            Log.Message("Test1B: " + (RimWorld.PawnKindDefOf.Boomalope.race.graphic.MatSingle.name));
        }

        private const string shaderBundlePath = @"Common\Materials\pangaeashaderbundle";
        private void LoadBundles()
        {
            AssetBundle loadBundle(string subPath)
            {
                string path = Path.Combine(Content.RootDir, subPath);
                AssetBundle bundle = AssetBundle.LoadFromFile(path);
                if (bundle == null)
                {
                    Log.Error("Null AssetBundle for subpath: " + subPath);
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

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

        public ProjectPangaeaMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<PangaeaSettings>();

            Harmony = new Harmony(Content.ModMetaData.PackageId);
            Harmony.PatchAll(Assembly.GetExecutingAssembly());            

            Mod = this;

            LongEventHandler.ExecuteWhenFinished(ResourceGraphicLister.Init);
            LongEventHandler.ExecuteWhenFinished(PangaeaDatabase.Init);
            LongEventHandler.ExecuteWhenFinished(Production.PangaeaRecipeLister.Init);
        }

        public override string SettingsCategory() => Content.Name;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Settings.DoSettingsWindowContents(inRect);
        }
    }
}

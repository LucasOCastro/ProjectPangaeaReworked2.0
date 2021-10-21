using RimWorld;
using Verse;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectPangaea.Production
{
    public class ITab_Pangaea : ITab
    {
        private static Dictionary<ThingDef, List<RecipeDef>> recipeDefsCache = new Dictionary<ThingDef, List<RecipeDef>>();
        private static Dictionary<ThingDef, List<FloatMenuOption>> recipeOptionsCache = new Dictionary<ThingDef, List<FloatMenuOption>>();
        private static List<RecipeDef> GetRecipeDefsFor(ThingDef thingDef)
        {
            if (!recipeDefsCache.TryGetValue(thingDef, out List<RecipeDef> recipes))
            {
                recipes = new List<RecipeDef>();

                for (int i = 0; i < thingDef.AllRecipes.Count; i++)
                {
                    RecipeDef recipe = thingDef.AllRecipes[i];
                    if (recipe.HasModExtension<RecipeExtension>())
                    {
                        recipes.Add(recipe);
                    }
                }

                if (recipes.Count == 0)
                {
                    Log.Error(nameof(ITab_Pangaea) + " on Thing with no valid RecipeDefs! RecipeDefs need the Pangaea mod extension to be used with this tab.");
                }

                recipeDefsCache.Add(thingDef, recipes);
            }

            return recipes;
        }
        private static List<FloatMenuOption> GetOptionsFor(ThingDef thingDef, ITab_Pangaea tab)
        {
            if (!recipeOptionsCache.TryGetValue(thingDef, out List<FloatMenuOption> options))
            {
                options = new List<FloatMenuOption>();
                List<RecipeDef> recipes = GetRecipeDefsFor(thingDef);
                for (int i = 0; i < recipes.Count; i++)
                {
                    var recipe = recipes[i];
                    options.Add(new FloatMenuOption(recipe.label, () => tab.SelectedRecipe = recipe, recipe.UIIconThing));
                }
                recipeOptionsCache.Add(thingDef, options);
            }
            return options;
        }

        private bool UsingScrollbar => TotalEntriesHeight > Rect.height - (headerHeight + headerGapHeight);

        private float TotalEntriesHeight
        {
            get
            {
                float entryHeight = entryRecipeHeight + entryTitleHeight;
                int recipeCount = SelectedExtension.recipes.Count;
                int reverseableCount = SelectedExtension.ReverseableRecipeCount;

                return (entryHeight * recipeCount) + (reverseableCount * entryRecipeHeight);
            }
        }

        private Thing lastSelectedThing;
        private RecipeDef selectedRecipe;
        private RecipeExtension selectedExtension;
        private RecipeDef SelectedRecipe
        {
            get
            {
                if (SelThing != lastSelectedThing)
                    OnChangeSelectedThing();
                return selectedRecipe;
            }

            set
            {
                selectedRecipe = value;
                selectedExtension = selectedRecipe.GetModExtension<RecipeExtension>();
                entriesScrollPos = Vector2.zero;
            }
        }
        private RecipeExtension SelectedExtension
        {
            get
            {
                if (SelThing != lastSelectedThing)
                    OnChangeSelectedThing();
                return selectedExtension;
            }
        }

        private void OnChangeSelectedThing()
        {
            lastSelectedThing = SelThing;
            selectedRecipe = GetRecipeDefsFor(SelThing.def).FirstOrDefault();
            selectedExtension = selectedRecipe.GetModExtension<RecipeExtension>();
            entriesScrollPos = Vector2.zero;
        }

        private static readonly Vector2 WinSize = new Vector2(420f, 480f);

        private Building_WorkTable SelTable => (Building_WorkTable)SelThing;

        public ITab_Pangaea()
        {
            size = WinSize;
            labelKey = "Pangaea_PangaeaTab";
            tutorTag = "Bills";
        }

        private bool IsDisabled(PangaeaRecipeSettings recipe)
        {
            return recipe.IsAlreadyAddedTo(SelTable.BillStack);
        }
        private bool IsEntirelyDisabled(PangaeaRecipeSettings recipe)
        {
            return IsDisabled(recipe) && (!recipe.canBeReversed || IsDisabled(recipe.Reversed));
        }
        private bool IsMissingIngredients(PangaeaRecipeSettings recipe)
        {
            return !recipe.IsAvailableIn(SelThing.Map);
        }
        private bool IsEntirelyMissingIngredients(PangaeaRecipeSettings recipe)
        {
            return IsMissingIngredients(recipe) && (!recipe.canBeReversed || IsMissingIngredients(recipe.Reversed));
        }
        private int DrawPriority(PangaeaRecipeSettings recipe)
        {
            if (IsEntirelyDisabled(recipe)) return -2;
            if (IsEntirelyMissingIngredients(recipe)) return -1;
            return 0;
        }

        private const float headerHeight = 30f;
        private const float headerGapHeight = Listing.DefaultGap;

        private const float entryTitleHeight = 25f;
        private const float entryRecipeHeight = 75f;
        private const float entryArrowWidthMulti = 0.12f;

        private Rect Rect => new Rect(Vector2.zero, WinSize).ContractedBy(GenUI.Pad);
        private Rect HeaderRect
        {
            get
            {
                Rect rect = Rect.TopPartPixels(headerHeight);
                rect.width -= Widgets.CloseButtonSize;
                return rect;
            }
        }
        private Rect EntriesRect
        {
            get
            {
                Rect rect = new Rect(Rect);
                rect.yMin += headerHeight + headerGapHeight;
                return rect;
            }
        }

        protected override void FillTab()
        {
            if (SelectedRecipe == null)
            {
                CloseTab();
                return;
            }

            MakeHeader(HeaderRect);
            DrawEntries(EntriesRect);

            Text.Anchor = default;
            Text.Font = default;
        }


        private Vector2 entriesScrollPos;
        private static List<PangaeaRecipeSettings> recipesToDrawLast = new List<PangaeaRecipeSettings>();
        private void DrawEntries(Rect rect)
        {
            
            bool usingScrollbar = UsingScrollbar;
            if (usingScrollbar)
            {
                Widgets.BeginScrollView(new Rect(rect) { width = rect.width }, ref entriesScrollPos, new Rect(rect) { width = rect.width - GenUI.ScrollBarWidth, height = TotalEntriesHeight });
                rect.width -= GenUI.ScrollBarWidth;
            }

            float y = rect.yMin;
            foreach (var recipe in SelectedExtension.recipes.OrderByDescending(DrawPriority))
            {
                DrawEntry(recipe, new Vector2(rect.x, y), rect, out float height);
                y += height;
            }

            if (usingScrollbar)
            {
                Widgets.EndScrollView();
            }
        }
        
        private void DrawEntry(PangaeaRecipeSettings recipe, Vector2 position, Rect parentRect, out float height)
        {
            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.MiddleCenter;


            Color beforeColor = GUI.color;
            if (IsEntirelyDisabled(recipe))
            {
                GUI.color = Color.grey;
            }

            string title = !recipe.label.NullOrEmpty() ? recipe.LabelCap : (string)selectedRecipe.LabelCap;
            Rect titleRect = new Rect(position.x, position.y, parentRect.width, entryTitleHeight);
            Widgets.DrawAltRect(titleRect);
            Widgets.DrawBox(titleRect);
            Widgets.LabelFit(titleRect, title);

            GUI.color = beforeColor;


            Text.Anchor = default;
            Text.Font = default;

            Rect recipeRect = new Rect(position.x, titleRect.yMax, parentRect.width, entryRecipeHeight);
            DrawEntryRecipe(recipe, recipeRect, parentRect);

            height = titleRect.height + recipeRect.height;

            if (recipe.canBeReversed)
            {
                Rect reverseRect = new Rect(recipeRect) { y = recipeRect.yMax };
                DrawEntryRecipe(recipe.Reversed, reverseRect, parentRect);
                height += reverseRect.height;
            }
        }

        private void DrawEntryRecipe(PangaeaRecipeSettings recipe, Rect rect, Rect clippingRect)
        {
            bool disabled = IsDisabled(recipe);
            if (disabled)
            {
                GUI.color = Color.grey;
            }

            Widgets.DrawBox(rect);

            float arrowWidth = rect.width * entryArrowWidthMulti;
            Rect arrowRect = new Rect() { size = Vector2.one * arrowWidth, center = rect.center };
            Widgets.DrawTextureFitted(arrowRect, TexUI.ArrowTexRight, 1);

            float portionSpaceWidth = (rect.width - arrowWidth) / 2;

            float pad = GenUI.Pad;
            Rect ingredientsRect = new Rect(rect) { width = portionSpaceWidth }.ContractedBy(pad);
            DrawPortions(recipe.ingredients, ingredientsRect, clippingRect, true);

            Rect resultsRect = new Rect(rect) { width = portionSpaceWidth, x = arrowRect.xMax }.ContractedBy(pad);
            DrawPortions(recipe.results, resultsRect, clippingRect, false);

            //TooltipHandler.TipRegion(rect, recipe.Summary);
            if (disabled)
            {
                TooltipHandler.TipRegion(rect, "Pangaea_RecipeAlreadyAdded".Translate());
            }
            else if (Widgets.ButtonInvisible(rect))
            {
                Select(recipe);
            }
        }

        private void DrawPortions(List<PortionData> portions, Rect rect, Rect clippingParentRect, bool drawRequiredCounts)
        {
            float ingSize = Mathf.Min(rect.width / portions.Count, rect.height);
            float y = rect.center.y - (ingSize / 2);
            float spacing = GenUI.Pad;
            float sideSpacing = (rect.width - ((portions.Count * (ingSize + spacing)) - spacing)) / 2;
            for (int i = 0; i < portions.Count; i++)
            {
                float x = rect.xMin + sideSpacing + (i * (ingSize + spacing));
                Rect portionRect = new Rect(x, y, ingSize, ingSize);
                DrawPortion(portions[i], portionRect, clippingParentRect, drawRequiredCounts);
            }
        }

        private const float portionCountSizeMulti = 0.25f;
        private void DrawPortion(PortionData portion, Rect rect, Rect clippingParentRect, bool drawRequired)
        {
            Rect adjustedScrollRect = new Rect(clippingParentRect);
            if (UsingScrollbar) adjustedScrollRect.position += entriesScrollPos;
            PangaeaUIGen.DrawTexWithMaterialClipped(rect, adjustedScrollRect, portion.Icon, portion.Graphic.MatSingle);
            TooltipHandler.TipRegion(rect, portion.ToString());

            Text.Font = GameFont.Tiny;
            Text.Anchor = TextAnchor.MiddleCenter;

            int count = portion.CountInMap(SelThing.Map);
            int required = portion.count;
            string countLabel = count + "/" + required;//drawRequired ? (count + "/" + required) : count.ToString();
            Vector2 countSize = Text.CalcSize(countLabel);
            Rect countRect = rect.LowerRight(countSize.x, countSize.y);
            Rect countBgRect = countRect.ExpandedBy(countRect.width / 2, 2);
            GUI.DrawTexture(countBgRect, TexUI.GrayTextBG);
            Color beforeColor = GUI.color;
            if (drawRequired)
            {
                beforeColor = GUI.color;
                GUI.color = (count < required) ? Color.red : Color.green;
            }
            Widgets.Label(countRect, countLabel);

            Text.Anchor = default;
            Text.Font = default;
            GUI.color = beforeColor;
        }

        private void Select(PangaeaRecipeSettings recipe)
        {
            SelTable.BillStack.AddBill(new PangaeaBill(SelectedRecipe, recipe));
        }

        private void MakeHeader(Rect rect)
        {
            List<FloatMenuOption> options = GetOptionsFor(SelThing.def, this);
            bool multipleOptions = (options.Count > 1);

            string label = SelectedRecipe.LabelCap;
            SoundDef sound = multipleOptions ? SoundDefOf.Mouseover_Standard : null;
            if (Widgets.ButtonTextSubtle(rect, label, mouseoverSound: sound) && multipleOptions)
            {
                Find.WindowStack.Add(new FloatMenu(options));
            }
        }
    }
}

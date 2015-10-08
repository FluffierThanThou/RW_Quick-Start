using System;
using System.Reflection;
using RimWorld;
using UnityEngine;
using Verse;

namespace Fluffy.QS
{
    public class Page_SelectStoryteller : Window
    {
        public override Vector2 InitialWindowSize
        {
            get
            {
                return new Vector2(1020f, 764f);
            }
        }

        public Page_SelectStoryteller()
        {
            this.forcePause = true;
            this.absorbInputAroundWindow = true;
            MapInitData.ChooseDefaultStoryteller();
        }

        public override void DoWindowContents(Rect inRect)
        {
            // need to create an arg object for the reflection call, which will hopefully get populated with the results and ref parameters.
            // we then need to actually set assign those, but that's after the reflected invoke.
            // http://stackoverflow.com/questions/8779731/how-to-pass-a-parameter-as-a-reference-with-methodinfo-invoke
            object[] args = new object[]
            {
                inRect,
                MapInitData.chosenStoryteller,
                MapInitData.difficulty
            };

            // fetch the method
            var DrawCoreStoryTellerUI = typeof(StorytellerUI).GetMethod("DrawStorytellerSelectionInterface", BindingFlags.NonPublic | BindingFlags.Static);

            // invoke it with the newly created arguments
            DrawCoreStoryTellerUI.Invoke(null, args);

            // and assign those back
            MapInitData.chosenStoryteller = args[1] as StorytellerDef;
            MapInitData.difficulty = args[2] as DifficultyDef;

            // the default back and forth buttons.
            DialogUtility.DoNextBackButtons(inRect, "Next".Translate(), new Action(this.TryGoNext), new Action(this.GoBack));

            // the quick start button
            if (DialogUtility.DoMiddleButton(inRect, "Quick Start"))
            {
                if (MapInitData.difficulty == null) MapInitData.difficulty = DefDatabase<DifficultyDef>.GetRandom();
                if (MapInitData.chosenStoryteller == null)
                    MapInitData.chosenStoryteller = DefDatabase<StorytellerDef>.GetRandom();
                this.Close(true);

                Action newEventAction = delegate
                {
                    QuickStartController.GetInstance.MakeReady();
                };
                LongEventHandler.QueueLongEvent(newEventAction, "Reticulating pawns...");
                this.Close(true);
            }
        }

        private void TryGoNext()
        {
            if (MapInitData.difficulty == null)
            {
                if (!Prefs.DevMode)
                {
                    Messages.Message("MustChooseDifficulty".Translate(), MessageSound.RejectInput);
                    return;
                }
                Messages.Message("Difficulty has been automatically selected (debug mode only)", MessageSound.Silent);
                MapInitData.ChooseDefaultDifficulty();
            }
            Find.WindowStack.Add(new Page_SelectWorld());
            this.Close(true);
        }

        private void GoBack()
        {
            this.Close(true);
        }
    }
}
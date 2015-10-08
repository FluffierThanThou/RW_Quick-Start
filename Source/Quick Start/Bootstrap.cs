using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace Fluffy.QS
{
    class Bootstrap : ITab
    {
        public Bootstrap() : base()
            {
            Log.Message("Initialized " + QuickStartController.ModName + ".");

            // Creates a new Unity Engine GameObject (http://docs.unity3d.com/ScriptReference/GameObject.html)
            // You can name this however you want as long as it doesn't conflict with an
            // existing game object, so don't choose a name that's too generic.  I'm
            // using a different game object for each of my mods, but you could also
            // try to use the same game object for all of your mods.  In that case,
            // you would look for the existing game object before creating a new one.
            // Cleaning up when the mod is unloaded would also be different in that case.
            GameObject gameObject = new GameObject(QuickStartController.GameObjectName);

            // RimWorld has two Unity game "levels"--which don't correspond to what you'd
            // normally think of when you think of game levels.  The menus before you enter
            // gameplay are one "level" and the gameplay itself is the other.  Normally,
            // when a new level gets loaded, all game objects from the previous level get
            // destroyed.  We don't want that to happen with our game object, so we mark
            // it accordingly.
            MonoBehaviour.DontDestroyOnLoad(gameObject);

            // The QuickStartController is a MonoBehavior (http://docs.unity3d.com/ScriptReference/MonoBehaviour.html)
            // You can attach event-driven behaviors to it.  For example, if you define
            // an Update() method in it, it will run that method every frame.  If you
            // define an OnLevelLoaded() method in it, it will run that method whenever
            // RimWorld switches between the main menus and gameplay.  We take advantage
            // of this to control our mod's behavior.  Each of my mods has a single
            // controller that is attached to its GameObject.
            gameObject.AddComponent<QuickStartController>();
        }

        // This method is declared as virtual in the parent ITab class, so we need to
        // define it, but it doesn't do anything.
        protected override void FillTab()
        {
            return;
        }
    }
}
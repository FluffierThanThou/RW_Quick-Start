using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;
using CommunityCoreLibrary;

namespace QuickStart
{
    public class Controller : IMainMenu
    {
        #region Methods

        // The main quick start logic, makes all the pieces ready, and starts the new game.
        public void QuickStart()
        {
            Log.Message( "QuickStart :: Initilizing new game with default/random settings." );

            Action startGame = delegate
                {
                    MakeReadyAndStartGame();
                };

            LongEventHandler.QueueLongEvent( startGame, "Shaving muffalos..." );
        }

        public bool TryLoadNewestWorld()
        {
            FileInfo fileInfo = ( from wf in SavedWorldsDatabase.AllWorldFiles
                                  orderby wf.LastWriteTime descending
                                  select wf ).FirstOrDefault<FileInfo>();
            if ( fileInfo == null )
            {
                return false;
            }

            SaveFileInfo saveFileInfo = new SaveFileInfo( fileInfo );
            if ( VersionControl.BuildFromVersionString( saveFileInfo.GameVersion )
                != VersionControl.BuildFromVersionString( VersionControl.CurrentVersionFull ) )
            {
                return false;
            }

            string fullName = fileInfo.FullName;
            WorldLoader.LoadWorldFromFile( fullName );

            if ( !ModListsMatch( ScribeHeaderUtility.loadedModsList, ( from mod in LoadedModManager.LoadedMods
                                                                       select mod.name ).ToList() ) )
            {
                return false;
            }

            return true;
        }

        private void MakeReadyAndStartGame()
        {
            if ( !TryLoadNewestWorld() )
            // for one reason or another, loading saved world failed, so we'll create a new one.
            {
                WorldGenerationData.Reset();
                WorldGenerator.GenerateWorld();
                GameDataSaver.SaveWorld( Current.World );
                Log.Message( "QuickStart :: World created" );
            }
            else
            {
                Log.Message( "QuickStart :: World loaded" );
            }

            Rand.RandomizeSeedFromTime();
            MapInitData.difficulty = DefDatabase<DifficultyDef>.GetRandom();
            MapInitData.chosenStoryteller = DefDatabase<StorytellerDef>.GetRandom();
            MapInitData.ChooseDecentLandingSite();
            MapInitData.GenerateDefaultColonistsWithFaction();

            // set faction into world.
            MethodInfo SCFIW_MI = typeof( MapInitData ).GetMethod( "SetColonyFactionIntoWorld",
                                                                 BindingFlags.Static | BindingFlags.NonPublic );
            SCFIW_MI.Invoke( null, null );
            MapInitData.mapSize = 150;

            // set started from entry to true to make sure InitNewGeneratedMap() doesn't override our settings,
            // and we get the fancy guitar riff intro.
            MapInitData.startedFromEntry = true;
            Application.LoadLevel( "Gameplay" );
        }

        private bool ModListsMatch( List<string> a, List<string> b )
        {
            if ( a.Count != b.Count )
                return false;
            for ( int index = 0; index < a.Count; ++index )
            {
                if ( a[index] != b[index] )
                    return false;
            }
            return true;
        }

        public void ClickAction()
        {
            QuickStart();
        }

        public bool RenderNow( bool anyWorldFiles, bool anyMapFiles )
        {
            return Game.Mode == GameMode.Entry;
        }

        #endregion Methods
    }
}
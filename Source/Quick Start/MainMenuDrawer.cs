using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace QuickStart
{
    public class MainMenuDrawer
    {
        #region Fields

        private static readonly Texture2D IconBlog      = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Blog", true);
        private static readonly Texture2D IconBook      = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Book", true);
        private static readonly Texture2D IconForums    = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Forums", true);
        private static readonly Texture2D IconTwitter   = ContentFinder<Texture2D>.Get("UI/HeroArt/WebIcons/Twitter", true);
        private static readonly Texture2D TexLudeonLogo = ContentFinder<Texture2D>.Get("UI/HeroArt/LudeonLogoSmall", true);
        private static readonly Texture2D TexTitle      = ContentFinder<Texture2D>.Get("UI/HeroArt/GameTitle", true);

        #endregion Fields

        #region Methods

        public static void DoMainMenuButtons( Rect rect, bool anyWorldFiles, bool anyMapFiles, Action backToGameButtonAction = null )
        {
            Rect rect2 = new Rect( 0f, 0f, 200f, rect.height );
            Rect rect3 = new Rect( rect2.xMax + 17f, 0f, -1f, rect.height );
            rect3.xMax = rect.width;
            Text.Font = GameFont.Small;
            List<ListableOption> list = new List<ListableOption>();
            ListableOption item;
            if ( Game.Mode == GameMode.Entry )
            {
                item = new ListableOption( "CreateWorld".Translate(), delegate
                {
                    MapInitData.Reset();
                    Find.WindowStack.Add( new Page_CreateWorldParams() );
                } );
                list.Add( item );
                item = new ListableOption( "QuickStart".Translate(), delegate
                {
                    QuickStartController.QuickStart();
                } );
                list.Add( item );
                if ( anyWorldFiles )
                {
                    item = new ListableOption( "NewColony".Translate(), delegate
                    {
                        MapInitData.Reset();
                        Find.WindowStack.Add( new Page_SelectStoryteller() );
                    } );
                    list.Add( item );
                }
            }
            if ( Game.Mode == GameMode.MapPlaying )
            {
                if ( backToGameButtonAction != null )
                {
                    item = new ListableOption( "BackToGame".Translate(), backToGameButtonAction );
                    list.Add( item );
                }
                item = new ListableOption( "Save".Translate(), delegate
                {
                    CloseMainTab();
                    Find.WindowStack.Add( new Dialog_MapList_Save() );
                } );
                list.Add( item );
            }
            if ( anyMapFiles )
            {
                item = new ListableOption( "Load".Translate(), delegate
                {
                    CloseMainTab();
                    Find.WindowStack.Add( new Dialog_MapList_Load() );
                } );
                list.Add( item );
            }
            item = new ListableOption( "Options".Translate(), delegate
            {
                CloseMainTab();
                Find.WindowStack.Add( new Dialog_Options() );
            } );
            list.Add( item );
            if ( Game.Mode == GameMode.Entry )
            {
                item = new ListableOption( "Mods".Translate(), delegate
                {
                    Find.WindowStack.Add( new Page_ModsConfig() );
                } );
                list.Add( item );
                item = new ListableOption( "Credits".Translate(), delegate
                {
                    Find.WindowStack.Add( new Page_Credits() );
                } );
                list.Add( item );
            }
            if ( Game.Mode == GameMode.MapPlaying )
            {
                Action action = delegate
                {
                    Find.WindowStack.Add( new Dialog_Confirm( "ConfirmQuit".Translate(), delegate
                    {
                        Application.LoadLevel( "Entry" );
                    }, true ) );
                };
                item = new ListableOption( "QuitToMainMenu".Translate(), action );
                list.Add( item );
                Action action2 = delegate
                {
                    Find.WindowStack.Add( new Dialog_Confirm( "ConfirmQuit".Translate(), delegate
                    {
                        Root.Shutdown();
                    }, true ) );
                };
                item = new ListableOption( "QuitToOS".Translate(), action2 );
                list.Add( item );
            }
            else
            {
                item = new ListableOption( "QuitToOS".Translate(), delegate
                {
                    Root.Shutdown();
                } );
                list.Add( item );
            }
            Rect rect4 = rect2.ContractedBy( 17f );
            OptionListingUtility.DrawOptionListing( rect4, list );
            Text.Font = GameFont.Small;
            List<ListableOption> list2 = new List<ListableOption>();
            ListableOption item2 = new ListableOption_WebLink( "FictionPrimer".Translate(), "https://docs.google.com/document/d/1pIZyKif0bFbBWten4drrm7kfSSfvBoJPgG9-ywfN8j8/pub", IconBlog );
            list2.Add( item2 );
            item2 = new ListableOption_WebLink( "LudeonBlog".Translate(), "http://ludeon.com/blog", IconBlog );
            list2.Add( item2 );
            item2 = new ListableOption_WebLink( "Forums".Translate(), "http://ludeon.com/forums", IconForums );
            list2.Add( item2 );
            item2 = new ListableOption_WebLink( "OfficialWiki".Translate(), "http://rimworldwiki.com", IconBlog );
            list2.Add( item2 );
            item2 = new ListableOption_WebLink( "TynansTwitter".Translate(), "https://twitter.com/TynanSylvester", IconTwitter );
            list2.Add( item2 );
            item2 = new ListableOption_WebLink( "TynansDesignBook".Translate(), "http://tynansylvester.com/book", IconBook );
            list2.Add( item2 );
            item2 = new ListableOption_WebLink( "HelpTranslate".Translate(), "http://ludeon.com/forums/index.php?topic=2933.0", IconForums );
            list2.Add( item2 );
            Rect rect5 = rect3.ContractedBy( 17f );
            float num = OptionListingUtility.DrawOptionListing( rect5, list2 );
            GUI.BeginGroup( rect5 );
            if ( Game.Mode == GameMode.Entry && Widgets.ImageButton( new Rect( 0f, num + 10f, 64f, 32f ), LanguageDatabase.activeLanguage.icon ) )
            {
                List<FloatMenuOption> list3 = new List<FloatMenuOption>();
                foreach ( LoadedLanguage current in LanguageDatabase.AllLoadedLanguages )
                {
                    LoadedLanguage localLang = current;
                    list3.Add( new FloatMenuOption( localLang.FriendlyNameNative, delegate
                    {
                        LanguageDatabase.SelectLanguage( localLang );
                        Prefs.Save();
                    }, MenuOptionPriority.Medium, null, null ) );
                }
                Find.WindowStack.Add( new FloatMenu( list3, false ) );
            }
            GUI.EndGroup();
        }

        private static void CloseMainTab()
        {
            if ( Game.Mode == GameMode.MapPlaying )
            {
                Find.MainTabsRoot.EscapeCurrentTab( false );
            }
        }

        #endregion Methods
    }
}
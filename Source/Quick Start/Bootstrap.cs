using CommunityCoreLibrary;
using System.Reflection;

namespace QuickStart
{
    public class Bootstrap : SpecialInjector
    {
        #region Methods

        public override void Inject()
        {
            MethodInfo vanilla_menu = typeof( RimWorld.MainMenuDrawer ).GetMethod( "DoMainMenuButtons", BindingFlags.Public | BindingFlags.Static );
            MethodInfo quickstart_menu = typeof( QuickStart.MainMenuDrawer ).GetMethod( "DoMainMenuButtons", BindingFlags.Public | BindingFlags.Static );
            Detours.TryDetourFromTo( vanilla_menu, quickstart_menu );
        }

        #endregion Methods
    }
}
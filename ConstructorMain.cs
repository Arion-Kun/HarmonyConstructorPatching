using System;
using System.Reflection;
using MelonLoader;
using ModType = Dawn.POC.ConstructorPatching;

[assembly: MelonInfo(typeof(ModType), "ConstructorPatching", "1.0", "arion#1223")]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonOptionalDependencies("HarmonyLib")]

namespace Dawn.POC
{
    using HarmonyLib;
    //using Harmony;
    internal sealed class ConstructorPatching : MelonMod
    {
        private const string PoC = "Proof of Concept";
    #if MelonLoader
        private static HarmonyInstance instance = HarmonyInstance.Create(PoC);
    #endif
    #if Harmony
        private static readonly Harmony instance = new Harmony(PoC);
    #endif

        public override void OnApplicationStart()
        {
            Console.WriteLine("Starting Patches");
            //Prefix:
            instance.Patch( AccessTools.Constructor( typeof(CreatedConstructor) ), prefix: new HarmonyMethod(typeof(ConstructorPatching).GetMethod("PrefixExample", BindingFlags.Static | BindingFlags.NonPublic)));
            //Postfix:
            instance.Patch( AccessTools.Constructor( typeof(CreatedConstructor)  ), prefix: null, postfix: new HarmonyMethod(typeof(ConstructorPatching).GetMethod("PostfixExample", BindingFlags.Static | BindingFlags.NonPublic)));

            var newinstance = new CreatedConstructor("Dawn");
        }
        

        private static bool PrefixExample()
        {
            Console.WriteLine("This is a Prefix for the Constructor being created.");
            return true;
        }

        private static void PostfixExample(CreatedConstructor __instance)
        {
            Console.WriteLine("This is a Postfix for the Constructor that was created.");
            Console.WriteLine($"You can reference the player that was created from this constructor with __instance");
            Console.WriteLine($"{__instance.username}");
            //This is really cool to know but UnityGames don't just create new constructors.
            //Try adding a MonoBehaviour to the CreatedConstructor and see what happens to your 'newinstance'.
        }
        
    }

    internal class CreatedConstructor 
    {
        internal CreatedConstructor(string name)
        {
            username = name;
            //Some Cool Constructor Code would go in here :3
        }

        public string username;
    }
}




















//o btw, VRC uses how the constructor is made above in the initial constructor instead of creating a new object, so this is useless ;)
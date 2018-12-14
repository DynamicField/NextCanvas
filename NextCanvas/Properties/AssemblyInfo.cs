#region

using System;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using NextCanvas.Properties;

#endregion

// Les informations générales relatives à un assembly dépendent de
// l'ensemble d'attributs suivant. Changez les valeurs de ces attributs pour modifier les informations
// associées à un assembly.
[assembly: AssemblyTitle("NextCanvas")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("NextCanvas")]
[assembly: AssemblyCopyright("Copyright ©  2018")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// L'affectation de la valeur false à ComVisible rend les types invisibles dans cet assembly
// aux composants COM. Si vous devez accéder à un type dans cet assembly à partir de
// COM, affectez la valeur true à l'attribut ComVisible sur ce type.
[assembly: ComVisible(false)]

//Pour commencer à générer des applications localisables, définissez
//<UICulture>CultureUtiliséePourCoder</UICulture> dans votre fichier .csproj
//dans <PropertyGroup>.  Par exemple, si vous utilisez le français
//dans vos fichiers sources, définissez <UICulture> à fr-FR. Puis, supprimez les marques de commentaire de
//l'attribut NeutralResourceLanguage ci-dessous. Mettez à jour "fr-FR" dans
//la ligne ci-après pour qu'elle corresponde au paramètre UICulture du fichier projet.

[assembly: NeutralResourcesLanguage("en", UltimateResourceFallbackLocation.MainAssembly)]


[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //où se trouvent les dictionnaires de ressources spécifiques à un thème
    //(utilisé si une ressource est introuvable dans la page,
    // ou dictionnaires de ressources de l'application)
    ResourceDictionaryLocation.SourceAssembly //où se trouve le dictionnaire de ressources générique
    //(utilisé si une ressource est introuvable dans la page,
    // dans l'application ou dans l'un des dictionnaires de ressources spécifiques à un thème)
)]


// Les informations de version pour un assembly se composent des quatre valeurs suivantes :
//
//      Version principale
//      Version secondaire
//      Numéro de build
//      Révision
//
// Vous pouvez spécifier toutes les valeurs ou indiquer les numéros de build et de révision par défaut
// en utilisant '*', comme indiqué ci-dessous :
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(AssemblyInfo.Version)]
[assembly: AssemblyFileVersion(AssemblyInfo.Version)]
[assembly: Guid(AssemblyInfo.GuidString)]

namespace NextCanvas.Properties
{
    public static class AssemblyInfo
    {
        public const string GuidString = "572E9214-D38A-454A-A3F1-84714AC8C7C3";
        public const string Version = "2.0.0.0";
        public static readonly Guid Guid = new Guid(GuidString);
        public static string VersionWithoutZeros => GetVersionWithoutLeadingZeroes();
        public static string GetVersionWithoutLeadingZeroes()
        {
            var bits = Version.Split('.');
            var result = new StringBuilder();
            for (var i = 0; i < bits.Length; i++)
            {
                var versionPart = bits[i];
                var addDot = i != bits.Length - 1;
                result.Append(versionPart);
                if (i >= 1 && bits.Skip(i + 1).All(s => s.Equals("0"))) // They all are zeroes after.
                {
                    break;
                }

                if (addDot)
                {
                    result.Append('.');
                }
            }

            return result.ToString();
        }
    }
}
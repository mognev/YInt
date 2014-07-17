namespace Core.Scheduler
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Core.Scheduler.Interfaces;
    
    /// <summary>
    /// Provides information about types in the current web application. 
    /// Optionally this type can look at all assemblies in the bin folder.
    /// </summary>
    public class WebAppTypeFinder : ITypeFinder
    {
        private string assemblySkipLoadingPattern = "^System|^mscorlib|^Microsoft|^CppCodeProvider|^VJSharpCodeProvider|^WebDev|^Castle|^Iesi|^log4net|^NHibernate|^nunit|^TestDriven|^MbUnit|^Rhino|^QuickGraph|^TestFu|^Telerik|^ComponentArt|^MvcContrib|^AjaxControlToolkit|^Antlr3|^Remotion|^Recaptcha";
        private string assemblyRestrictToLoadingPattern = ".*";



        public IEnumerable<Type> FindClassesOfType<T>()
        {
            foreach (System.Reflection.Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!Regex.IsMatch(a.FullName, assemblySkipLoadingPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled)
                    && Regex.IsMatch(a.FullName, assemblyRestrictToLoadingPattern))
                {
                    foreach (Type t in a.GetTypes())
                    {
                        if (typeof(T).IsAssignableFrom(t) && !t.IsInterface && t.IsClass && !t.IsAbstract)
                            yield return t;
                    }
                }
            }
        }
    }
}

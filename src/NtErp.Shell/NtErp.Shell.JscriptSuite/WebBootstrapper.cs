using JScriptSuite.BrowserHost;
using JScriptSuite.BrowserHost.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NtErp.Shell.JscriptSuite {
    internal class WebBootstrapper {
        const string HtmlSubfolderPath = "\\html\\";
        const string JavascriptSubfolderPath = "\\js\\";
        const string jsMethodPattern = @"=""javascript:(?<methodName>.+?)[\(]?[\)]?[;]?""";

        static DirectoryInfo OutputDirectory;
        static DirectoryInfo OutputHtmlDirectory;
        static DirectoryInfo OutputJsDirectory;

        static DirectoryInfo SourceRootDirectory;
        static DirectoryInfo SourceHtmlDirectory;
        static DirectoryInfo SourceJsDirectory;

        static readonly Regex jsMethodRegex = new Regex(jsMethodPattern, RegexOptions.Compiled);
        static readonly IList<Type> _assemblyTypes = new List<Type>();
        static IBrowserHost _browserHost;
        static readonly Dictionary<string, LocalPageFactory> _pageFactories = new Dictionary<string, LocalPageFactory>();

        public WebBootstrapper() {
            cacheAssemblyTypes();

            Debug.WriteLine(" > Setting pathes ...");

            OutputDirectory = new DirectoryInfo(Path.GetDirectoryName(typeof(Program).Assembly.Location));
            Debug.WriteLine("     > [out] Folder: " + OutputDirectory.FullName);

            OutputHtmlDirectory = new DirectoryInfo(OutputDirectory + HtmlSubfolderPath);
            Debug.WriteLine("     > [out]   HTML: " + OutputHtmlDirectory.FullName);

            OutputJsDirectory = new DirectoryInfo(OutputDirectory + JavascriptSubfolderPath);
            Debug.WriteLine("     > [out]     JS: " + OutputJsDirectory.FullName);

            SourceRootDirectory = OutputDirectory.Parent.Parent;
            Debug.WriteLine("     > [src] Folder: " + SourceRootDirectory.FullName);

            SourceHtmlDirectory = SourceRootDirectory.GetDirectories("html").Single();
            Debug.WriteLine("     > [src]   HTML: " + SourceHtmlDirectory.FullName);

            SourceJsDirectory = SourceRootDirectory.GetDirectories("js").Single();
            Debug.WriteLine("     > [src]     JS: " + SourceJsDirectory.FullName);

            if (!OutputJsDirectory.Exists)
                OutputJsDirectory.Create();
        }


        private void cacheAssemblyTypes() {
            _assemblyTypes.Clear();

            Debug.WriteLine(" > Scanning assembly types ...");

            foreach (var assemblyType in Assembly.GetExecutingAssembly().GetTypes()) {
                if (!assemblyType.Name.EndsWith("App"))
                    continue;

                Debug.WriteLine("       > Type: " + assemblyType.Name);
                _assemblyTypes.Add(assemblyType);
            }
        }

        private IList<string> parseJavascriptMethodsFromHtml(FileInfo htmlFile) {
            IList<string> methodNames = new List<string>();

            string fileContent = null;

            using (StreamReader sr = htmlFile.OpenText())
                fileContent = sr.ReadToEnd();

            foreach (Match match in jsMethodRegex.Matches(fileContent))
                methodNames.Add(match.Groups["methodName"].Value.Trim());

            return methodNames;
        }

        internal void Run() {
            _pageFactories.Clear();
            _browserHost = Host.CreateBrowserHost();

            FileInfo[] htmlFiles = SourceHtmlDirectory.GetFiles("*.html", SearchOption.AllDirectories);

            Debug.WriteLine(" > Starting to wire up factories and pages ...");
            foreach (var htmlFile in htmlFiles) {
                Debug.WriteLine(" > File: " + htmlFile.FullName);

                string shortFolderPath = htmlFile.DirectoryName.Replace(
                    SourceRootDirectory.FullName + "\\", String.Empty);
                Debug.WriteLine("     > Short: " + shortFolderPath);
                if (shortFolderPath.Contains("\\")) {
                    DirectoryInfo dir = new DirectoryInfo(OutputJsDirectory + shortFolderPath.Substring(shortFolderPath.IndexOf("html\\") + 5));
                    Debug.WriteLine("     >   Out: " + dir.FullName);
                    if (!dir.Exists)
                        dir.Create();
                }

                IList<string> javascriptMethodsInHtml = parseJavascriptMethodsFromHtml(htmlFile);

                LocalPageFactory pageFactory = null;

                if (_pageFactories.ContainsKey(shortFolderPath)) {
                    Debug.WriteLine("     > Using existing page factory: " + shortFolderPath);
                    pageFactory = _pageFactories[shortFolderPath];
                } else {
                    Debug.WriteLine("     > Creating new page factory for: " + shortFolderPath);
                    string htmlPattern = shortFolderPath + "\\{0}.html";
                    Debug.WriteLine("         > HTML Pattern: " + htmlPattern);
                    string scriptPattern = htmlPattern.Replace("html", "js");
                    Debug.WriteLine("         >   JS Pattern: " + scriptPattern);

                    pageFactory = new LocalPageFactory(htmlPattern, scriptPattern);
                    _pageFactories.Add(shortFolderPath, pageFactory);

                    Debug.WriteLine("     > Created page factory and added it to the cache");
                }

                Debug.WriteLine("     > Mapping JS methods to C# methods ...");
                string pageName = htmlFile.Name.Substring(0, htmlFile.Name.IndexOf("."));
                Debug.WriteLine("         >    Page Name: " + pageName);
                string className = pageName[0].ToString().ToUpper() + pageName.Substring(1) + "App";
                Debug.WriteLine("         >   Class Name: " + className);

                Type associatedClass = _assemblyTypes.Single(t => t.Name == className);
                MethodInfo[] methods = associatedClass.GetMethods(BindingFlags.Static | BindingFlags.Public);
                Debug.WriteLine("         > Method Count: " + methods.Count());

                Debug.WriteLine("     > Adding page: " + pageName);
                IPageHost pageHost = pageFactory.AddPage(_browserHost, pageName);
                foreach (string methodName in javascriptMethodsInHtml) {
                    Debug.WriteLine("         > Adding method mapping: " + methodName);
                    MethodInfo mi = methods.Single(m => m.Name == methodName);
                    Action action = (Action)mi.CreateDelegate(typeof(Action));

                    pageHost.Add(action, methodName);
                }

                Debug.WriteLine("     > " + htmlFile.Name + " finished");
            }

            Debug.WriteLine(" > Running browser host");
            _browserHost.Run();
        }
    }
}
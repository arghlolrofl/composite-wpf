using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml.Linq;

namespace LocalizationManager.ViewModels {
    public class MainViewModel : INotifyPropertyChanged {
        public event EventHandler DataTableUpdated;
        private void RaiseDataTableUpdated() => DataTableUpdated?.Invoke(this, EventArgs.Empty);


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        private int _selectedColumnIndex;
        private int _selectedRowIndex;
        #region Fields

        const string resxKeyValuePattern = @"<data name=""(?<key>.+?)"" xml:space=""preserve"">\r\n.+?<value>(?<value>.+?)</value>";
        const string xamlKeyPattern = @"=""\{lex:Loc (?:Key=)?(?<key>.+?)\}";

        private readonly Regex xamlRegex = new Regex(xamlKeyPattern, RegexOptions.Compiled);
        private readonly Regex resxRegex = new Regex(resxKeyValuePattern, RegexOptions.Compiled);


        private bool _hasChanges;
        private bool _isScanInactive;
        private DataTable _locTable;
        private string _solutionPath;
        private DirectoryInfo _solutionDirectory;
        private ObservableCollection<DirectoryInfo> _projectDirectories = new ObservableCollection<DirectoryInfo>();
        private string _selectedDictionary;
        private ObservableCollection<string> _dictionaries = new ObservableCollection<string>();
        private DirectoryInfo _selectedProjectDirectory;
        private DirectoryInfo _resourceDirectory;

        #endregion

        #region Properties

        public bool IsScanInactive {
            get { return _isScanInactive; }
            set { _isScanInactive = value; RaisePropertyChanged(); }
        }

        public bool HasChanges {
            get { return _hasChanges; }
            set { _hasChanges = value; RaisePropertyChanged(); }
        }

        public string SolutionPath {
            get { return SolutionDirectory?.FullName; }
            set {
                _solutionPath = value;
                RaisePropertyChanged();

                if (String.IsNullOrEmpty(_solutionPath) || !Directory.Exists(_solutionPath))
                    return;

                SolutionDirectory = new DirectoryInfo(_solutionPath);
            }
        }

        public string SelectedDictionary {
            get { return _selectedDictionary; }
            set {
                _selectedDictionary = value;
                RaisePropertyChanged();

                SelectedDictionaryChangedCallback();
            }
        }

        public int SelectedRowIndex {
            get { return _selectedRowIndex; }
            set {
                _selectedRowIndex = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(SelectedKey));
                RaisePropertyChanged(nameof(SelectedValue));
            }
        }

        public int SelectedColumnIndex {
            get { return _selectedColumnIndex; }
            set {
                _selectedColumnIndex = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(SelectedValue));
            }
        }

        public string SelectedKey {
            get {
                if (SelectedRowIndex >= LocTable.Rows.Count || SelectedRowIndex < 0)
                    return String.Empty;

                return LocTable.Rows[SelectedRowIndex]["Key"].ToString();
            }
        }

        public string SelectedValue {
            get {
                if (SelectedRowIndex >= LocTable.Rows.Count || SelectedRowIndex < 0)
                    return String.Empty;

                if (SelectedColumnIndex > 0)
                    return LocTable.Rows[SelectedRowIndex][SelectedColumnIndex].ToString();

                return LocTable.Rows[SelectedRowIndex]["Default"].ToString();
            }
        }


        public DataTable LocTable {
            get { return _locTable; }
            set { _locTable = value; RaisePropertyChanged(); }
        }

        public DirectoryInfo SolutionDirectory {
            get { return _solutionDirectory; }
            set {
                _solutionDirectory = value;
                RaisePropertyChanged();

                SolutionDirectoryChangedCallback();
            }
        }

        public DirectoryInfo ResourceDirectory {
            get { return _resourceDirectory; }
            set { _resourceDirectory = value; RaisePropertyChanged(); }
        }

        public DirectoryInfo SelectedProjectDirectory {
            get { return _selectedProjectDirectory; }
            set {
                _selectedProjectDirectory = value;
                RaisePropertyChanged();

                SelectedProjectDirectoryChangedCallback();
            }
        }

        public ObservableCollection<string> Dictionaries {
            get { return _dictionaries; }
            set { _dictionaries = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<DirectoryInfo> ProjectDirectories {
            get { return _projectDirectories; }
            set { _projectDirectories = value; RaisePropertyChanged(); }
        }

        #endregion


        public MainViewModel() {
            IsScanInactive = true;
            SolutionPath = AppRegistry.GetValue(RegistryKeys.SolutionPath);
        }

        /// <summary>
        /// Called, when the solution directory changes
        /// </summary>
        private void SolutionDirectoryChangedCallback() {
            AppRegistry.SetValue(RegistryKeys.SolutionPath, SolutionPath);

            ProjectDirectories = new ObservableCollection<DirectoryInfo>(
                SolutionDirectory.GetFiles("*.csproj", SearchOption.AllDirectories)
                                 .Select(f => f.Directory)
                                 .ToList()
            );

            string lastProjectUsed = AppRegistry.GetValue(RegistryKeys.SelectedProject);
            if (String.IsNullOrEmpty(lastProjectUsed))
                return;

            SelectedProjectDirectory = ProjectDirectories.FirstOrDefault(dir => dir.FullName == lastProjectUsed);
        }

        /// <summary>
        /// Called, when the selected localization project changes
        /// </summary>
        private void SelectedProjectDirectoryChangedCallback() {
            AppRegistry.SetValue(RegistryKeys.SelectedProject, SelectedProjectDirectory.FullName);
            ScanForDefaultDictionaries();
        }

        /// <summary>
        /// Scan for all dicitionaries (default resx files, without culture identification)
        /// </summary>
        private void ScanForDefaultDictionaries() {
            Dictionaries.Clear();

            var culturInfoList = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(c => c.Name.Length > 0);
            _resourceDirectory = SelectedProjectDirectory.GetDirectories("Resources").Single();
            var resxFiles = _resourceDirectory.GetFiles("*.resx");

            foreach (FileInfo resxFile in resxFiles) {
                if (culturInfoList.Any(ci => resxFile.Name.EndsWith(ci.Name + ".resx")))
                    continue;

                Dictionaries.Add(resxFile.Name.Replace(".resx", String.Empty));
            }

            if (Dictionaries.Any()) {
                string lastUsedDictionary = AppRegistry.GetValue(RegistryKeys.SelectedDictionary);
                if (String.IsNullOrEmpty(lastUsedDictionary))
                    SelectedDictionary = Dictionaries.First();
                else
                    SelectedDictionary = Dictionaries.First(dict => dict == lastUsedDictionary);
            }
        }

        /// <summary>
        /// Called, when the selected dictionary changes
        /// </summary>
        private void SelectedDictionaryChangedCallback() {
            AppRegistry.SetValue(RegistryKeys.SelectedDictionary, SelectedDictionary);

            ScanForLocalizations();
        }

        /// <summary>
        /// Scans for exisiting localizations in the resource directory
        /// </summary>
        private void ScanForLocalizations() {
            LocTable = new DataTable();

            // search for default resx file (without culture identification) ...
            var defaultResourceFile = _resourceDirectory.GetFiles(SelectedDictionary + ".resx").First();
            // ... and parse it's values
            ParseLocalizedValues(defaultResourceFile);

            // search for all localized resx files
            var localizedResourceFiles = _resourceDirectory.GetFiles(SelectedDictionary + "*.resx")
                                                           .Where(f => f.Name != defaultResourceFile.Name)
                                                           .OrderBy(f => f.Name.Length)
                                                           .ToList();

            // remember dictionary name and file ending
            string[] parts = defaultResourceFile.Name.Split('.');
            foreach (var localizedResourceFile in localizedResourceFiles) {
                // replace both for every localized resx file to get the culture identification ...
                string cultureString = localizedResourceFile.Name.Replace(parts[0] + ".", String.Empty)
                                                                 .Replace("." + parts[1], String.Empty);

                // ... and pass it to the parser
                ParseLocalizedValues(localizedResourceFile, new CultureInfo(cultureString));
            }

            RaiseDataTableUpdated();
        }

        public void Callback_OnRowEditEnding(object sender, DataGridRowEditEndingEventArgs e) {
            HasChanges = true;
        }

        public void SaveChangesToResxFiles(DataTable dataTable) {
            string targetFileName = SelectedDictionary + ".resx";

            FileInfo targetFile = _resourceDirectory.GetFiles(targetFileName).First();
            WriteColumnToResx(targetFile);

            FileInfo[] targetFiles = _resourceDirectory.GetFiles(SelectedDictionary + ".*.resx");
            foreach (var localizedTargetFile in targetFiles) {
                string columnName = localizedTargetFile.Name.Replace(".resx", String.Empty)
                                                            .Replace(SelectedDictionary + ".", String.Empty);

                WriteColumnToResx(localizedTargetFile, columnName);
            }
        }

        #region Helpers

        private void ParseLocalizedValues(FileInfo resxFile, CultureInfo ci = null) {
            // Prepare LocTable
            if (ci == null) {
                LocTable.Columns.Add("ID");
                LocTable.Columns[0].ReadOnly = true;
                LocTable.PrimaryKey = new DataColumn[] { LocTable.Columns.Add("Key") };
                LocTable.Columns.Add("Default");
            } else
                LocTable.Columns.Add(ci.Name);


            MatchCollection matches = null;
            using (StreamReader sr = resxFile.OpenText())
                matches = resxRegex.Matches(sr.ReadToEnd());


            int rowIndex = 0;
            foreach (Match match in matches) {
                if (!match.Success)
                    continue;

                string key = match.Groups["key"].Value;
                string value = match.Groups["value"].Value;

                if (ci == null) {
                    DataRow row = LocTable.NewRow();
                    row[0] = rowIndex++;
                    row[1] = key;
                    row[2] = value;
                    LocTable.Rows.Add(row);
                } else {
                    DataRow row = LocTable.Rows.Find(key);
                    row[ci.Name] = value;
                }
            }
        }

        private void WriteColumnToResx(FileInfo targetFile, string columnName = null) {
            string fileContent = null;
            using (StreamReader sr = targetFile.OpenText())
                fileContent = sr.ReadToEnd();

            XDocument xmlDoc = XDocument.Parse(fileContent);

            IEnumerable<XElement> dataNodes = xmlDoc.Root.Elements().Where(e => e.Name == "data");
            dataNodes.Remove();

            XNamespace ns = xmlDoc.Root.GetNamespaceOfPrefix("xml");
            foreach (DataRow row in LocTable.Rows) {
                string key = row[0].ToString();

                string value = null;
                if (!String.IsNullOrEmpty(columnName))
                    value = row[columnName].ToString();
                else
                    value = row[1].ToString();

                xmlDoc.Root.Add(
                    new XElement("data",
                        new XAttribute("name", key),
                        new XAttribute(ns + "space", "preserve"),
                        new XElement("value", value)));
            }


            RemoveReadOnlyAttribute(targetFile);
            xmlDoc.Save(targetFile.FullName);
            HasChanges = false;
        }

        private void RemoveReadOnlyAttribute(FileInfo targetFile) {
            FileAttributes attributes = File.GetAttributes(targetFile.FullName);

            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly) {
                // Show the file.
                attributes = RemoveAttribute(attributes, FileAttributes.ReadOnly);
                File.SetAttributes(targetFile.FullName, attributes);

                Console.WriteLine("The {0} file is no longer read-only.", targetFile.Name);
            }
        }

        private static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove) {
            return attributes & ~attributesToRemove;
        }



        #endregion

        #region Missing key scan

        public void FindMissingLocalizations() {
            IsScanInactive = false;

            try {
                IList<string> keys = ParseLocalizationKeys();

                for (int i = 0; i < keys.Count; i++) {
                    string key = keys[i];

                    DataRow row = LocTable.Rows.Find(key);
                    if (row != null)
                        keys.RemoveAt(i--);
                }

                string nl = Environment.NewLine;
                DialogResult result = MessageBox.Show(
                    "Following keys are not yet localized:" +
                    nl + nl +
                    String.Join(nl, keys) +
                    nl + nl +
                    "Do you want to add them?",
                    "Missing localization keys",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes) {
                    foreach (string missingKey in keys) {
                        DataRow row = LocTable.NewRow();
                        row[0] = missingKey;

                        LocTable.Rows.Add(row);
                    }
                }
            } finally {
                IsScanInactive = true;
            }
        }

        private IList<string> ParseLocalizationKeys() {
            List<string> keys = new List<string>();

            FileInfo[] xamlFiles = SolutionDirectory.GetFiles("*.xaml", SearchOption.AllDirectories);

            foreach (FileInfo xamlFile in xamlFiles) {
                MatchCollection matches = null;
                using (StreamReader sr = xamlFile.OpenText())
                    matches = xamlRegex.Matches(sr.ReadToEnd());

                foreach (Match match in matches) {
                    if (!match.Success)
                        continue;

                    keys.Add(match.Groups["key"].Value);
                }
            }

            return keys;
        }

        #endregion
    }
}

/* --------------------------
 *
 * TextTemplateUtilities.cs
 *
 * Description: 
 *
 * Author: Jeremy Smellie
 *
 * Editors:
 *
 * 5/12/2015 - Starvoxel
 * 
 * All rights reserved
 *
 * -------------------------- */

#region Includes
#region Unity Includes
using UnityEngine;
using UnityEditor;
#endregion

#region System Includes
using System.IO;
using System.Collections;
using System.Collections.Generic;
#endregion
#endregion

namespace EditorUtilities
{
    /*
    public class TextTemplateMacroReplacer : UnityEditor.AssetModificationProcessor
    {
        #region Fields & Properties
        //const
        private const string LICENSE_FILE_KEY = "TextTemplateMacroReplacer_LicenseFile";
        private const string AUTHOR_KEY = "TextTemplateMacroReplacer_Author";

        private const string PROJECT_NAMESPACE_COUNT_KEY = "TextTemplateMacroReplacer_ProjectNamespaceCout";
        private const string PROJECT_NAMESPACE_TYPE_KEY = "TextTemplateMacroReplacer_ProjectNamespaceType";
        private const string PROJECT_USE_LONG_NAMESPACE_KEY = "TextTemplateMacroReplacer_ProjectUseLongNamespace";

        private const string MODULE_NAMESPACE_COUNT_KEY = "TextTemplateMacroReplacer_ModuleNamespaceCout";
        private const string MODULE_NAMESPACE_TYPE_KEY = "TextTemplateMacroReplacer_ModuleNamespaceType";
        private const string MODULE_USE_LONG_NAMESPACE_KEY = "TextTemplateMacroReplacer_ModuleUseLongNamespace";

        private const string SCRIPT_TEMPLATE_PATH = "Data/Resources/ScriptTemplates/";
        private const string CSHARP_TEMPLATE_FILENAME = "81-C# Script-NewBehaviourScript.cs.text";

        private const string DEFAULT_LICENSE = "All rights reserved.";

        private const char NAMESPACE_SEPERATOR_CHAR = ',';

        //properties
        private static string LicenseFilePath
        {
            get { return EditorPrefs.GetString(LICENSE_FILE_KEY, null); }
            set { EditorPrefs.SetString(LICENSE_FILE_KEY, value); }
        }

        private static string Author
        {
            get { return EditorPrefs.GetString(AUTHOR_KEY, null); }
            set { EditorPrefs.SetString(AUTHOR_KEY, value); }
        }

        private static bool ProjectUseDescriptiveNamespace
        {
            get { return EditorPrefs.GetBool(PROJECT_USE_LONG_NAMESPACE_KEY, false); }
            set { EditorPrefs.SetBool(PROJECT_USE_LONG_NAMESPACE_KEY, value); }
        }

        private static TextTemplateUtilities.eNamespaceType[] ProjectNamespaceTypes
        {
            get
            {
                string[] typesStrArray = EditorPrefs.GetString(PROJECT_NAMESPACE_TYPE_KEY, string.Empty).Split(NAMESPACE_SEPERATOR_CHAR);

                if (typesStrArray.Length == 0 || string.IsNullOrEmpty(typesStrArray[0]))
                {
                    ProjectNamespaceTypes = new TextTemplateUtilities.eNamespaceType[] { TextTemplateUtilities.eNamespaceType.NONE };
                    return ProjectNamespaceTypes;
                }
                else
                {
                    TextTemplateUtilities.eNamespaceType[] retVal = new TextTemplateUtilities.eNamespaceType[typesStrArray.Length];

                    for (int i = 0; i < retVal.Length; ++i)
                    {
                        retVal[i] = (TextTemplateUtilities.eNamespaceType)int.Parse(typesStrArray[i]);
                    }

                    return retVal;
                }
            }
            set
            {
                string types = string.Empty;

                for (int i = 0; i < value.Length; ++i)
                {
                    if (i != 0)
                    {
                        types += NAMESPACE_SEPERATOR_CHAR;
                    }

                    types += (int)value[i];
                }
                EditorPrefs.SetString(PROJECT_NAMESPACE_TYPE_KEY, types);
            }
        }

        private static bool ModuletUseDescriptiveNamespace
        {
            get { return EditorPrefs.GetBool(MODULE_USE_LONG_NAMESPACE_KEY, false); }
            set { EditorPrefs.SetBool(MODULE_USE_LONG_NAMESPACE_KEY, value); }
        }

        private static TextTemplateUtilities.eNamespaceType[] ModuleNamespaceTypes
        {
            get
            {
                string[] typesStrArray = EditorPrefs.GetString(MODULE_NAMESPACE_TYPE_KEY, string.Empty).Split(NAMESPACE_SEPERATOR_CHAR);

                if (typesStrArray.Length == 0 || string.IsNullOrEmpty(typesStrArray[0]))
                {
                    ModuleNamespaceTypes = new TextTemplateUtilities.eNamespaceType[] { TextTemplateUtilities.eNamespaceType.NONE };
                    return ProjectNamespaceTypes;
                }
                else
                {
                    TextTemplateUtilities.eNamespaceType[] retVal = new TextTemplateUtilities.eNamespaceType[typesStrArray.Length];

                    for (int i = 0; i < retVal.Length; ++i)
                    {
                        retVal[i] = (TextTemplateUtilities.eNamespaceType)int.Parse(typesStrArray[i]);
                    }

                    return retVal;
                }
            }
            set
            {
                string types = string.Empty;

                for (int i = 0; i < value.Length; ++i)
                {
                    if (i != 0)
                    {
                        types += NAMESPACE_SEPERATOR_CHAR;
                    }

                    types += (int)value[i];
                }
                EditorPrefs.SetString(MODULE_NAMESPACE_TYPE_KEY, types);
            }
        }
        #endregion

        #region Public Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        private static bool shouldShowProjectSettings = false;
        private static bool shouldShowModuleSettings = false;

        [PreferenceItem("Text Template")]
        private static void OnPreferenceItem()
        {
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("License File:", GUILayout.Width(100));

                GUI.enabled = !string.IsNullOrEmpty(LicenseFilePath);
                {
                    string compressedPath = LicenseFilePath.Replace(Application.dataPath, "");

                    GUILayout.Label(compressedPath, GUI.skin.textField, GUILayout.MaxWidth(218));
                }
                GUI.enabled = true;

                if (GUILayout.Button("Set", GUILayout.ExpandWidth(false)))
                {
                    LicenseFilePath = EditorUtility.OpenFilePanel("Select License File", string.IsNullOrEmpty(LicenseFilePath) ? Application.dataPath : LicenseFilePath, "txt");
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Author: ", GUILayout.Width(100));
                Author = EditorGUILayout.TextField(Author);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            RenderNamespaceInspector();

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("--- UNFINISHED FEATURES ---", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
            GUI.enabled = false;
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Apply default Text Template"))
                {
                    if (EditorUtility.DisplayDialog("Are you sure?", "WARNING! Applying the default template will remove all custome changes you have made!", "I'm sure", "Cancel"))
                    {
                        TextTemplateMacroReplacer.ApplyDefaultTemplate();
                    }
                }
            }
            GUILayout.EndHorizontal();
            GUI.enabled = true;
        }

        private static void RenderNamespaceInspector()
        {
            shouldShowProjectSettings = EditorGUILayout.Foldout(shouldShowProjectSettings, "Project Namespace Settings");

            if (shouldShowProjectSettings)
            {
                EditorGUI.indentLevel = 1;
                List<TextTemplateUtilities.eNamespaceType> oldNamespaceTypes = new List<TextTemplateUtilities.eNamespaceType>(ProjectNamespaceTypes);

                int size = EditorGUILayout.IntField("Size: ", oldNamespaceTypes.Count);

                if (size > 5)
                {
                    size = 5;
                }
                else if (size < 1)
                {
                    size = 1;
                }

                if (size != oldNamespaceTypes.Count)
                {
                    int dif = size - oldNamespaceTypes.Count;

                    while (dif < 0)
                    {
                        oldNamespaceTypes.RemoveAt(oldNamespaceTypes.Count - 1);
                        dif += 1;
                    }

                    while (dif > 0)
                    {
                        oldNamespaceTypes.Add(TextTemplateUtilities.eNamespaceType.CompagnyName);
                        dif -= 1;
                    }
                }

                bool elementDisabled = false;
                for (int i = 0; i < oldNamespaceTypes.Count; ++i)
                {
                    oldNamespaceTypes[i] = (TextTemplateUtilities.eNamespaceType)EditorGUILayout.EnumPopup("Namespace Type " + i + ": ", oldNamespaceTypes[i]);

                    if (oldNamespaceTypes[i] == TextTemplateUtilities.eNamespaceType.NONE)
                    {
                        elementDisabled = false;
                        GUI.enabled = false;
                    }
                    else if (GUI.enabled && oldNamespaceTypes[i] == TextTemplateUtilities.eNamespaceType.Directory)
                    {
                        elementDisabled = true;
                        GUI.enabled = false;
                    }
                }

                ProjectNamespaceTypes = oldNamespaceTypes.ToArray();
                if (elementDisabled)
                {
                    GUI.enabled = true;
                }

                EditorGUILayout.Space();

                ProjectUseDescriptiveNamespace = EditorGUILayout.Toggle("Use Descriptive Namespace: ", ProjectUseDescriptiveNamespace);
                GUI.enabled = true;
            }

            EditorGUI.indentLevel = 0;

            EditorGUILayout.Space();

            shouldShowModuleSettings = EditorGUILayout.Foldout(shouldShowModuleSettings, "Module Namespace Settings");
            if (shouldShowModuleSettings)
            {
                EditorGUI.indentLevel = 1;
                List<TextTemplateUtilities.eNamespaceType> oldNamespaceTypes = new List<TextTemplateUtilities.eNamespaceType>(ModuleNamespaceTypes);

                int size = EditorGUILayout.IntField("Size: ", oldNamespaceTypes.Count);

                if (size > 5)
                {
                    size = 5;
                }
                else if (size < 1)
                {
                    size = 1;
                }

                if (size != oldNamespaceTypes.Count)
                {
                    int dif = size - oldNamespaceTypes.Count;

                    while (dif < 0)
                    {
                        oldNamespaceTypes.RemoveAt(oldNamespaceTypes.Count - 1);
                        dif += 1;
                    }

                    while (dif > 0)
                    {
                        oldNamespaceTypes.Add(TextTemplateUtilities.eNamespaceType.CompagnyName);
                        dif -= 1;
                    }
                }

                bool elementDisabled = false;
                for (int i = 0; i < oldNamespaceTypes.Count; ++i)
                {
                    oldNamespaceTypes[i] = (TextTemplateUtilities.eNamespaceType)EditorGUILayout.EnumPopup("Namespace Type " + i + ": ", oldNamespaceTypes[i]);

                    if (oldNamespaceTypes[i] == TextTemplateUtilities.eNamespaceType.NONE)
                    {
                        elementDisabled = false;
                        GUI.enabled = false;
                    }
                    else if (GUI.enabled && oldNamespaceTypes[i] == TextTemplateUtilities.eNamespaceType.Directory)
                    {
                        elementDisabled = true;
                        GUI.enabled = false;
                    }
                }

                ModuleNamespaceTypes = oldNamespaceTypes.ToArray();
                if (elementDisabled)
                {
                    GUI.enabled = true;
                }

                EditorGUILayout.Space();

                ProjectUseDescriptiveNamespace = EditorGUILayout.Toggle("Use Descriptive Namespace: ", ProjectUseDescriptiveNamespace);
                GUI.enabled = true;
            }

            EditorGUI.indentLevel = 0;
        }

        private static void ApplyDefaultTemplate()
        {
            string editorPath = Path.GetDirectoryName(EditorApplication.applicationPath);
            editorPath = Path.Combine(editorPath, SCRIPT_TEMPLATE_PATH);

            editorPath = editorPath.Replace('/', Path.DirectorySeparatorChar);

            editorPath += CSHARP_TEMPLATE_FILENAME;

            File.WriteAllText(editorPath, TextTemplateUtilities.DEFAULT_CSHARP_TEMPLATE);
            Debug.Log(editorPath);
        }

        //[MenuItem("Assets/Create/C# Script", false, 75)]
        private static void SmallTest()
        {
            Debug.Log("Test");
        }

        private static string NoNamespace(string file)
        {
            int startIndex = file.IndexOf("#NAMESPACE-START#");
            int endIndex = file.IndexOf("#NAMESPACE-END#");

            if(startIndex > 0)
            {
                if(startIndex - 1 > 0 && file[startIndex - 1] == '\n')
                {
                    file = file.Remove(startIndex - 1, 1);
                    endIndex -= 1;
                }
            }
            if (endIndex > 0)
            {
                if (endIndex - 1 > 0 && file[endIndex - 1] == '\n')
                {
                    file = file.Remove(endIndex - 1);
                }
            }

            file = file.Replace("#NAMESPACE-START#", "");
            file = file.Replace("#NAMESPACE-END#", "");

            return file;
        }

        private static void OnWillCreateAsset(string path)
        {
            path = path.Replace(".meta", "");
            int index = path.LastIndexOf(".");

            if (index < 0)
            {
                return;
            }

            string file = path.Substring(index);
            if (file != ".cs" && file != ".js" && file != ".boo") return;

            string ns = string.Empty;

            string[] folders = Path.GetDirectoryName(path).Split('/');

            int namespaceIndex = 0;

            bool isModuleSript = false;

            while (namespaceIndex < folders.Length && (folders[namespaceIndex] == "Assets" || folders[namespaceIndex] == "Modules" || folders[namespaceIndex] == "Scripts"))
            {
                if (folders[namespaceIndex] == "Modules")
                {
                    isModuleSript = true;
                }
                namespaceIndex += 1;
            }

            TextTemplateUtilities.eNamespaceType[] oldNamespaceTypes;

            if (isModuleSript)
            {
                oldNamespaceTypes = ModuleNamespaceTypes;
            }
            else
            {
                oldNamespaceTypes = ProjectNamespaceTypes;
            }

            if (oldNamespaceTypes != null && oldNamespaceTypes.Length > 0 && oldNamespaceTypes[0] != TextTemplateUtilities.eNamespaceType.NONE)
            {
                for (int i = 0; i < oldNamespaceTypes.Length; ++i)
                {
                    if (i != 0)
                    {
                        ns += '.';
                    }

                    bool breakOut = false;

                    switch (oldNamespaceTypes[i])
                    {
                        case TextTemplateUtilities.eNamespaceType.NONE:
                            breakOut = true;
                            break;
                        case TextTemplateUtilities.eNamespaceType.CompagnyName:
                            ns += TextTemplateMacroReplacer.CodifyString(PlayerSettings.companyName);
                            break;
                        case TextTemplateUtilities.eNamespaceType.ProjectName:
                            ns += TextTemplateMacroReplacer.CodifyString(PlayerSettings.productName);
                            break;
                        case TextTemplateUtilities.eNamespaceType.Directory:
                            breakOut = true;
                            if (namespaceIndex < folders.Length)
                            {
                                ns += folders[namespaceIndex];
                                namespaceIndex += 1;
                            }
                            break;
                    }

                    if (breakOut)
                    {
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(ns) && ProjectUseDescriptiveNamespace)
            {
                while (namespaceIndex < folders.Length)
                {
                    if (folders[namespaceIndex] != "Sripts" && folders[namespaceIndex] != "Modules")
                    {
                        ns += '.' + folders[namespaceIndex];
                    }
                    namespaceIndex += 1;
                }
            }

            index = Application.dataPath.LastIndexOf("Assets");
            path = Application.dataPath.Substring(0, index) + path;
            file = System.IO.File.ReadAllText(path);

            file = file.Replace("#CREATIONDATE#", System.DateTime.Now.ToShortDateString());
            file = file.Replace("#PROJECTNAME#", PlayerSettings.productName);

            if (string.IsNullOrEmpty(PlayerSettings.productName))
            {
                PlayerSettings.productName = "DefaultProject";
            }
            file = file.Replace("#CODE-PROJECTNAME#", TextTemplateMacroReplacer.CodifyString(PlayerSettings.productName));
            file = file.Replace("#COMPANYNAME#", PlayerSettings.companyName);
            file = file.Replace("#AUTHOR#", string.IsNullOrEmpty(Author) ? "" : Author);
            file = file.Replace("#CODE-COMPANYNAME#", TextTemplateMacroReplacer.CodifyString(PlayerSettings.companyName));

            int startIndex = file.IndexOf("#NAMESPACE-START#");
            int endIndex = file.IndexOf("#NAMESPACE-END#");

            if (startIndex > 0 && endIndex > 0)
            {
                if (!string.IsNullOrEmpty(ns))
                {
                    startIndex += "#NAMESPACE-START#".Length;

                    for (int i = startIndex; i < endIndex - 1; ++i)
                    {
                        char curChar = file[i];

                        if (curChar == '\n')
                        {
                            file = file.Insert(i + 1, "\t");
                            endIndex += 1;
                        }
                    }

                    file = file.Replace("#NAMESPACE-START#", "namespace " + ns + "\r\n{");
                    file = file.Replace("#NAMESPACE-END#", "}");
                }
                else
                {
                    file = NoNamespace(file);
                }
            }
            else
            {
                file = NoNamespace(file);
            }

            string licenseMsg = DEFAULT_LICENSE;
            if (!string.IsNullOrEmpty(LicenseFilePath))
            {
                licenseMsg = System.IO.File.ReadAllText(LicenseFilePath);
                licenseMsg = licenseMsg.Replace("\n", "\n * ");
            }

            file = file.Replace("#SOURCELICENSE#", licenseMsg);

            System.IO.File.WriteAllText(path, file);
            AssetDatabase.Refresh();
        }

        private static string CodifyString(string value)
        {
            while (value.Length > 0 && value[0] >= '0' && value[0] <= '9')
            {
                value = value.Remove(0, 1);
            }

            value = value.Replace(" ", "");

            return value;
        }
        #endregion
    }
    */

    
    public class TextTemplateUtilities
    {
        #region Fields & Properties

        //enums
        public enum eNamespaceType
        {
            NONE = 0,
            COMPAGNY_NAME = 1 << 0,
            PROJECT_NAME = 1 << 1,
            DIRECTORY = 1 << 2
        }

        #region Script Templates
        public const string DEFAULT_CSHARP_TEMPLATE =
@"/* --------------------------
 *
 * #SCRIPTNAME#.cs
 *
 * Description: 
 *
 * Author: #AUTHOR#
 *
 * Editors:
 *
 * #CREATIONDATE# - #COMPANYNAME#
 *
 * #SOURCELICENSE#
 *
 * -------------------------- */

#region Includes
#region Unity Includes
using UnityEngine;
#endregion

#region System Includes
using System.Collections;
#endregion
#endregion
        
namespace #CODE-PROJECTNAME#
{
	public class #SCRIPTNAME# : MonoBehaviour 
	{
		#region Fields & Properties
		//const

		//public

		//protected

		//private

		//properties
		#endregion

		#region Unity Methods
		#endregion

		#region Public Methods
		#endregion

		#region Protected Methods
		#endregion

		#region Private Methods
		#endregion
	}
}";
        #endregion
        #endregion
    }
}

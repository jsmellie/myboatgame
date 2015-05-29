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
    public class TextTemplateMacroReplacer : UnityEditor.AssetModificationProcessor
    {
        #region Fields & Properties
        //const
        private const string LICENSE_FILE_KEY = "TextTemplateMacroReplacer_LicenseFile";
        private const string AUTHOR_KEY = "TextTemplateMacroReplacer_Author";
        private const string USE_LONG_NAMESPACE_KEY = "TextTemplateMacroReplacer_UseNamespace";
        private const string NAMESPACE_COUNT_KEY = "TextTemplateMacroReplacer_NamespaceCout";
        private const string NAMESPACE_TYPE_KEY = "TextTemplateMacroReplacer_NamespaceType";


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

        private static bool UseDescriptiveNamespace
        {
            get { return EditorPrefs.GetBool(USE_LONG_NAMESPACE_KEY, false); }
            set { EditorPrefs.SetBool(USE_LONG_NAMESPACE_KEY, value); }
        }

        private static int NamespaceCount
        {
            get { return NamespaceTypes.Length; }
        }

        private static TextTemplateUtilities.eNamespaceType[] NamespaceTypes
        {
            get
            {
                string[] typesStrArray = EditorPrefs.GetString(NAMESPACE_TYPE_KEY, string.Empty).Split(NAMESPACE_SEPERATOR_CHAR);

                if (typesStrArray.Length == 0 || string.IsNullOrEmpty(typesStrArray[0]))
                {
                    NamespaceTypes = new TextTemplateUtilities.eNamespaceType[] { TextTemplateUtilities.eNamespaceType.NONE };
                    return NamespaceTypes;
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
                EditorPrefs.SetString(NAMESPACE_TYPE_KEY, types);
            }
        }
        #endregion

        #region Public Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        private static bool shouldShow = false;
        private static int size;

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

            shouldShow = EditorGUILayout.Foldout(shouldShow, "Namespace Settings");

            if (shouldShow)
            {
                EditorGUI.indentLevel = 1;
                List<TextTemplateUtilities.eNamespaceType> namespaceTypes = new List<TextTemplateUtilities.eNamespaceType>(NamespaceTypes);

                int size = EditorGUILayout.IntField("Size: ", namespaceTypes.Count);

                if (size > 5)
                {
                    size = 5;
                }
                else if (size < 1)
                {
                    size = 1;
                }

                if (size != namespaceTypes.Count)
                {
                    int dif = size - namespaceTypes.Count;

                    while (dif < 0)
                    {
                        namespaceTypes.RemoveAt(namespaceTypes.Count - 1);
                        dif += 1;
                    }

                    while (dif > 0)
                    {
                        namespaceTypes.Add(TextTemplateUtilities.eNamespaceType.COMPAGNY_NAME);
                        dif -= 1;
                    }
                }

                bool elementDisabled = false;
                for (int i = 0; i < namespaceTypes.Count; ++i)
                {
                    namespaceTypes[i] = (TextTemplateUtilities.eNamespaceType)EditorGUILayout.EnumPopup("Namespace Type " + i + ": ", namespaceTypes[i]);

                    if (namespaceTypes[i] == TextTemplateUtilities.eNamespaceType.NONE)
                    {
                        elementDisabled = false;
                        GUI.enabled = false;
                    }
                    else if (GUI.enabled && namespaceTypes[i] == TextTemplateUtilities.eNamespaceType.DIRECTORY)
                    {
                        elementDisabled = true;
                        GUI.enabled = false;
                    }
                }

                NamespaceTypes = namespaceTypes.ToArray();
                if (elementDisabled)
                {
                    GUI.enabled = true;
                }

                EditorGUILayout.Space();

                UseDescriptiveNamespace = EditorGUILayout.Toggle("Use Descriptive Namespace: ", UseDescriptiveNamespace);
                GUI.enabled = true;
            }

            EditorGUI.indentLevel = 0;
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

        private static void ApplyDefaultTemplate()
        {
            string editorPath = Path.GetDirectoryName(EditorApplication.applicationPath);
            editorPath = Path.Combine(editorPath, SCRIPT_TEMPLATE_PATH);

            editorPath = editorPath.Replace('/', Path.DirectorySeparatorChar);

            editorPath += CSHARP_TEMPLATE_FILENAME;

            File.WriteAllText(editorPath, TextTemplateUtilities.DEFAULT_CSHARP_TEMPLATE);
            Debug.Log(editorPath);
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

            TextTemplateUtilities.eNamespaceType[] namespaceTypes = NamespaceTypes;

            string ns = string.Empty;

            string[] folders = Path.GetDirectoryName(path).Split('/');

            int namespaceIndex = 0;

            while (namespaceIndex < folders.Length && (folders[namespaceIndex] == "Assets" || folders[namespaceIndex] == "Modules" || folders[namespaceIndex] == "Scripts"))
            {
                namespaceIndex += 1;
            }

            if (namespaceTypes == null || namespaceTypes.Length == 0 || namespaceTypes[0] == TextTemplateUtilities.eNamespaceType.NONE)
            {
                //Don't Use a namespace!
                ns = "DEFAULT_NAMESPACE";
            }
            else
            {
                for (int i = 0; i < namespaceTypes.Length; ++i)
                {
                    if (i != 0)
                    {
                        ns += '.';
                    }

                    bool breakOut = false;

                    switch (namespaceTypes[i])
                    {
                        case TextTemplateUtilities.eNamespaceType.NONE:
                            breakOut = true;
                            break;
                        case TextTemplateUtilities.eNamespaceType.COMPAGNY_NAME:
                            ns += TextTemplateMacroReplacer.CodifyString(PlayerSettings.companyName);
                            break;
                        case TextTemplateUtilities.eNamespaceType.PROJECT_NAME:
                            ns += TextTemplateMacroReplacer.CodifyString(PlayerSettings.productName);
                            break;
                        case TextTemplateUtilities.eNamespaceType.DIRECTORY:
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

            if (!string.IsNullOrEmpty(ns) && UseDescriptiveNamespace)
            {
                while (namespaceIndex < folders.Length)
                {
                    ns += '.' + folders[namespaceIndex];
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

            //TODO: jsmellie I really need to figure out someway to remove the namespace and it's brackets if ns is empty...  Not sure how to tackle that yet...
            file = file.Replace("#NAMESPACE#", ns);

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

/* --------------------------
 *
 * FlowManager.cs
 *
 * Description: 
 *
 * Author: Jeremy Smellie
 *
 * Editors:
 *
 * 5/30/2015 - Starvoxel
 *
 * All rights reserved.
 *
 * -------------------------- */

#region Includes
#region Unity Includes
using UnityEngine;
#endregion

#region System Includes
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
#endregion
#endregion

namespace Starvoxel.FlowManagement
{
    public class FlowVersionComparer : IComparer<Version>
    {
        public int Compare(Version x, Version y)
        {
            if (x == y)
            {
                return 0;
            }

            if(x.Major > y.Major)
            {
                return 1;
            }
            else if (x.Major < y.Major)
            {
                return -1;
            }
            else
            {
                if(x.Minor > y.Minor)
                {
                    return 1;
                }
                else if(x.Minor < y.Minor)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }

	public class FlowManager : MonoBehaviour
    {
        #region Internal Classes
        protected struct ViewAction
        {
            string m_Name;
            Hashtable m_Parameters;

            public ViewAction(string name)
            {
                m_Name = name;
                m_Parameters = new Hashtable();
            }

            public ViewAction(string name, Hashtable parameters) : this(name)
            {
                m_Parameters = parameters;
            }

        }
        #endregion

        #region Fields & Properties
        //const
	
		//public
	
		//protected
        [SerializeField] protected string m_Path = "XML/FLow.xml";

        protected bool m_IsClosingAllModalOnClose = false;

        protected System.Version m_CurrentVersion = new System.Version("1.0.0");
        protected System.Version m_FileVersion;
	
		//private
	
		//properties
		#endregion
	
		#region Unity Methods
        protected virtual void Awake()
        {
            TextAsset flowFile = Resources.Load<TextAsset>(m_Path);

            if (flowFile != null)
            {
                string error = string.Empty;

                XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(flowFile.text));

                while(reader.NodeType != XmlNodeType.Element)
                {
                    reader.Read();
                }

                if(reader.Name == "FLOW")
                {
                    do
                    {
                        reader.Read();
                    } while (reader.NodeType != XmlNodeType.Element);

                    if (reader.Name == "INFO")
                    {
                        ParseInfo(reader);

                        do
                        {
                            reader.Read();
                        }
                        while (reader.NodeType != XmlNodeType.Element);

                        if (reader.Name == "VIEWS")
                        {
                            ParseViews(reader);
                        }
                        else
                        {
                            error += "Invalid XML.  Second child element must be called VIEWS";
                        }
                    }
                    else
                    {
                        error += "Invalid XML.  First child element must be called INFO";
                    }
                }
                else
                {
                    error += "Invalid XML.  Root element must be called FLOW";
                }
            }
        }
		#endregion
	
		#region Public Methods
		#endregion
	
		#region Protected Methods
        protected void ParseInfo(XmlTextReader reader)
        {
            if(reader != null)
            {
                while(reader.MoveToNextAttribute())
                {
                    switch(reader.Name)
                    {
                        case "version":
                            m_FileVersion = new System.Version(reader.Value);
                            break;
                    }
                }
            }
        }

        protected void ParseViews(XmlTextReader reader)
        {
            if (reader != null)
            {
                while (reader.MoveToNextAttribute())
                {
                    switch(reader.Name)
                    {
                        case "closeAllModal":
                            m_IsClosingAllModalOnClose = Boolean.Parse(reader.Value);
                            break;
                    }
                }
            }
        }
		#endregion
	
		#region Private Methods
		#endregion
	}
}
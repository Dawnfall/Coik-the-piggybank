using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using helper;

namespace aim
{
#if UNITY_EDITOR

    public partial class ConfigAI_E : ScriptableObject, ISerializationCallbackReceiver
    {
        private static ConfigAI_E m_config;
        public static ConfigAI_E Instance
        {
            get
            {
                if (m_config != null)
                    return m_config;

                string[] allConfigs = AssetDatabase.FindAssets("t:" + typeof(ConfigAI_E).ToString());
                if (allConfigs.Length == 0)
                {
                    Debug.Log("No config asset found, new one is being created!");
                    m_config = CreateInstance<ConfigAI_E>();
                    AssetDatabase.CreateAsset(m_config, "Assets/configAI.asset");
                    return m_config;
                }

                if (allConfigs.Length > 1)
                    Debug.Log("There are multiple ConfigAI assets included but there should only be one!");

                m_config = AssetDatabase.LoadAssetAtPath<ConfigAI_E>(AssetDatabase.GUIDToAssetPath(allConfigs[0]));

                m_config.RegisterTypes();
                m_config.ValidateRegisteredTypes();
                m_config.LoadTextures();

                return m_config;

            }
        }

        private void RegisterTypes()
        {
            LoadDefaultBBTypes();

            m_propertyDrawerTypes.Clear();

            foreach (Type type in ReflectionHelper.getAllTypesInAssemblies(ReflectionHelper.getAllAssemblies()))
            {
                if (!type.IsAbstract && typeof(IPropertyDrawer).IsAssignableFrom(type))
                    RegisterPropertyDrawer(type);

                if (IsDefaultBBType(type))
                    m_registeredTypes.Add(new RegisteredType(type), null);
            }

        }

        //*******************************************
        // TYPES
        //********************************************

        //*********************
        // Property Drawers

        private Dictionary<Type, Type> m_propertyDrawerTypes = new Dictionary<Type, Type>(); //property type -> property drawer type
        private void RegisterPropertyDrawer(Type propDrawerType)
        {
            Type[] propertyType = ReflectionHelper.genericSubclassArgumentTypes(typeof(APropertyAIDrawer<>), propDrawerType);

            if (propertyType != null && propertyType.Length == 1)
            {
                if (!m_propertyDrawerTypes.ContainsKey(propertyType[0]))
                    m_propertyDrawerTypes.Add(propertyType[0], propDrawerType);
                else
                    Debug.Log("Property type: " + propertyType[0] + " already has a drawer: " + m_propertyDrawerTypes[propertyType[0]] + " , property drawer: " + propDrawerType + " will be excluded!");
            }
        }

        public IPropertyDrawer GetPropertyDrawer(Type propertyType)
        {
            if (propertyType == null)
                return null;

            Type propertyDrawerType;
            if (ReflectionHelper.findResultsForFirstIncludedSubtype(m_propertyDrawerTypes, propertyType, out propertyDrawerType))
                return Activator.CreateInstance(propertyDrawerType) as IPropertyDrawer;
            return new DefaultPropertyDrawer();
        }

        //*********
        // Registered types

        List<Type> defaultTypes = new List<Type>()
        {
            typeof(int),typeof(float),typeof(double),typeof(bool),typeof(string),
            typeof(Vector2),typeof(Vector3),typeof(Vector4),typeof(Quaternion),typeof(Color),
            typeof(Transform),typeof(GameObject)
        };
        private bool IsDefaultBBType(Type type)
        {
            if (typeof(MonoBehaviour).IsAssignableFrom(type))
                return true;
            if (defaultTypes.Contains(type))
                return true;

            return false;
        }

        [NonSerialized] public SortedList<RegisteredType, RegisteredType> m_registeredTypes = new SortedList<RegisteredType, RegisteredType>();
        private void ValidateRegisteredTypes()
        {
            int i = m_registeredTypes.Count - 1;
            while (i >= 0)
            {
                if (!m_registeredTypes.Keys[i].IsValid)
                    m_registeredTypes.RemoveAt(i);
                i--;
            }
        }
        public List<RegisteredType> GetAllRegisteredTypes(List<Type> baseTypes)
        {
            if (baseTypes == null || baseTypes.Count == 0)
                return new List<RegisteredType>(m_registeredTypes.Keys);

            List<RegisteredType> allReturnedTypes = new List<RegisteredType>();
            foreach (var regType in m_registeredTypes.Keys)
            {
                foreach (Type baseType in baseTypes)
                {
                    if (baseType.IsAssignableFrom(regType.Type))
                    {
                        allReturnedTypes.Add(regType);
                        break;
                    }
                }
            }
            return allReturnedTypes;
        }
        public List<RegisteredType> GetAllRegisteredTypesExcept(List<Type> nonBaseTypes)
        {
            if (nonBaseTypes == null || nonBaseTypes.Count == 0)
                return new List<RegisteredType>(m_registeredTypes.Keys);

            List<RegisteredType> allReturnedTypes = new List<RegisteredType>();
            foreach (var regType in m_registeredTypes.Keys)
            {
                bool doInclude = true;
                foreach (Type nonBaseType in nonBaseTypes)
                {
                    if (nonBaseType.IsAssignableFrom(regType.Type))
                    {
                        doInclude = false;
                        break;
                    }
                }
                if (doInclude)
                    allReturnedTypes.Add(regType);
            }
            return allReturnedTypes;
        }

        //***********
        // bb types

        private void LoadDefaultBBTypes()
        {
            m_registeredTypes.Clear();

            //foreach (Type defType in defaultTypes)
            //    m_registeredTypes.Add(new RegisteredType(defType), null);
        }
        public List<RegisteredType> GetAllBBTypes()
        {
            List<RegisteredType> allBBTypes = new List<RegisteredType>();
            foreach (var regType in m_registeredTypes.Keys)
            {
                //if (regType.IsUsed)
                allBBTypes.Add(regType);
            }
            return allBBTypes;
        }

        //***********
        // other

        public List<Type> GetDerivedTypes(Type baseType, bool doIncludeAbstract)
        {
            return ReflectionHelper.getAllSubTypesInAssemblies(baseType, doIncludeAbstract);
        }
        //public List<Type> GetNodeTypesForGraphType(Type graphType)
        //{
        //    if (graphType == null || !typeof(AGraphAI<>).IsAssignableFrom(graphType))
        //        return new List<Type>();

        //    Type[] allowedNodeTypeBase = ReflectionHelperAI.genericSubclassArgumentTypes(typeof(AGraphAI<>), graphType);
        //    if (allowedNodeTypeBase == null || allowedNodeTypeBase.Length != 1)
        //        return new List<Type>();

        //    List<Type> nodeTypes = new List<Type>();
        //    foreach (Type nodeType in GetDerivedTypes(typeof(ANodeAI), false))
        //    {
        //        if (allowedNodeTypeBase[0].IsAssignableFrom(nodeType))
        //            nodeTypes.Add(nodeType);
        //    }
        //    return nodeTypes;
        //}

        //************************
        // Editor Configuration
        //************************

        //*****************
        // Skin

        public GUISkin m_skin;

        //*******************
        // Background

        public Color m_backgroundColor = Color.black;
        Texture2D m_backgroundTexture;
        public Texture2D GetBackgroundTexture()
        {
            if (m_backgroundTexture == null)
            {
                m_backgroundTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                m_backgroundTexture.SetPixel(0, 0, ConfigAI_E.Instance.m_backgroundColor);
                m_backgroundTexture.Apply();
            }
            return m_backgroundTexture;
        }

        //******************
        // HUD

        public bool m_showHUD = true;
        public float m_hudWidth = 250;

        //****************
        // BB HUD

        public bool m_showBB = true;
        public float m_bbWidth = 600.0f;
        public float m_bbHeight = 600.0f;

        //***************
        // Zooming


        public float m_minZoom = 0.2f;
        public float m_maxZoom = 3f;
        public float m_zoomSpeed = 0.1f;

        //************************
        // External Data
        //************************




        //**************
        // Node sizes

        public Vector2 btNodeSize = new Vector2(120, 140);
        public Vector2 fsmStateNodeSize = new Vector2(150, 150);


        public Vector2 imageSize = new Vector2(30, 30);
        public Vector2 rootImageSize = new Vector2(30, 30);
        public Vector2 connectionPointSize = new Vector2(10, 10);
        public Vector2 breakPointSize = new Vector2(15, 15);
        public Vector2 addRemovePointButtonSize = new Vector2(15, 15);

        //*************
        // Panels

        public Vector2 mainOptionsPanelSize = new Vector2(250, 75);
        public Vector2 graphOptionsPanelSize = new Vector2(250, 150);
        public Vector2 blackboardPanelSize = new Vector2(250, 300);

        public int panelPadding = 20;

        public string nodeBackgroundImage = "node_Panel.png";
        public string connectionPointImage = "connectionPoint.png";
        public string breakPointImage = "breakPointImage.png";

        public string nodeSelectedImage = "selectedNode_Panel.png";
        public string nodeBTFailureImage = "failureStatus_Panel.png";
        public string nodeBTSuccessImage = "successStatus_Panel.png";
        public string nodeBTRunningImage = "runningStatus_Panel.png";

        //public string rootNodeImage = "rootNodeImage.png";

        //************
        // Test

        public Vector2 testPanelSize = new Vector2(250, 100);

        //**************************
        // SERIALIZATION
        //**************************

        [SerializeField] List<RegisteredType> m_serRegisterdTypes;

        public void OnBeforeSerialize()
        {
            m_serRegisterdTypes = new List<RegisteredType>();
            foreach (var regSerType in m_registeredTypes.Keys)
                m_serRegisterdTypes.Add(regSerType);
        }
        public void OnAfterDeserialize()
        {
            m_registeredTypes = new SortedList<RegisteredType, RegisteredType>();
            foreach (var serNodeType in m_serRegisterdTypes)
                m_registeredTypes.Add(serNodeType, null);

            //m_serRegisterdTypes.Clear();
        }

        //*********************
        // STYLES
        //*********************
        Dictionary<string, Texture2D> m_textureDict = new Dictionary<string, Texture2D>();

        public void LoadTextures()
        {
            string[] fileInfos = AssetDatabase.FindAssets("");

            foreach (var infoGUID in fileInfos)
            {
                string info = AssetDatabase.GUIDToAssetPath(infoGUID);

                if (HelperCommon.isInArray<string>(info.Split('/'), "AISprites"))
                {
                    Texture2D newTexture = AssetDatabase.LoadAssetAtPath(info, typeof(Texture2D)) as Texture2D;
                    if (newTexture != null)
                    {
                        string textureName = info.Substring(info.LastIndexOf("/") + 1);

                        if (!m_textureDict.ContainsKey(textureName))
                            m_textureDict.Add(textureName, newTexture);
                    }
                }
            }
        }
        public Texture2D GetTexture(string name)
        {
            Texture2D texture;
            m_textureDict.TryGetValue(name, out texture);
            if (texture == null)
            {
                Debug.Log("texture with name: " + name + " does not exist!");
            }
            return texture;
        }

        public GUIStyle GetNodeRectStyle()
        {
            GUIStyle nodeStyle = new GUIStyle();
            nodeStyle.normal.background = GetTexture(nodeBackgroundImage);

            return nodeStyle;
        }
        public GUIStyle GetSelectedStyle()
        {
            GUIStyle nodeSelectedStyle = new GUIStyle();
            nodeSelectedStyle.normal.background = GetTexture(nodeSelectedImage);
            return nodeSelectedStyle;
        }
        public virtual GUIStyle GetBTStatusStyle(EStatusAI status)
        {
            GUIStyle statusStyle = new GUIStyle();
            switch (status)
            {
                case EStatusAI.INVALID:
                    break;
                case EStatusAI.SUCCESS:
                    statusStyle.normal.background = GetTexture(nodeBTSuccessImage);
                    break;
                case EStatusAI.FAILURE:
                    statusStyle.normal.background = GetTexture(nodeBTFailureImage);
                    break;
                case EStatusAI.RUNNING:
                    statusStyle.normal.background = GetTexture(nodeBTRunningImage);
                    break;
                default:
                    Debug.Log("Status has no image: " + status.ToString());
                    return null;
            }
            return statusStyle;
        }



    }

#endif


}



//if (!type.IsAbstract && typeof(INodeAI).IsAssignableFrom(type))
//{
//    if (type.GetCustomAttributes(typeof(NodeAIAttribute), true).Length > 0)
//        m_nodeTypes.Add(type);
//    else
//        Debug.Log("Node type: " + type + " , does not have NodeAIAttribute assigned.It will not be included in editor!");
//}
//else if (!type.IsAbstract && typeof(IGraphAI).IsAssignableFrom(type))
//{
//    if (type.GetCustomAttributes(typeof(GraphAIAttribute), true).Length > 0)
//        m_graphTypes.Add(type);
//    else
//        Debug.Log("Graph type: " + type + " , does not have GraphAIAttribute assigned.It will no be included in editor!");
//}
//else
//{
//    //TODO: dont register all types!
//    RegisteredType newRegType = new RegisteredType(type);
//    if (!m_registeredTypes.ContainsKey(newRegType))
//        m_registeredTypes.Add(newRegType, null);
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace MetaCopyDevice
{
    public class MetaCopyWindow : EditorWindow
    {
        [MenuItem ("MyComp/MetaCopy/Main")]

        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(MetaCopyWindow));
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnEnable()
        {
            this.titleContent.text = "MetaCopyDevice";
        }

        private void OnDestroy()
        {
            
        }

        private void OnGUI()
        {
            bool defaultEnabled = GUI.enabled;

            GUI.enabled = !m_isExcuting;

            if (GUILayout.Button("Select target file"))
            {
                OpenTargetFilePanel();
            }

            DoShowCopyTargetPathAndExtension();

            if(GUILayout.Button("Select destination folder"))
            {
                OpenDestinationDirectoryPanel();
            }

            DoShowDestinationFolderPath();

            if(GUILayout.Button("Exexcute Meta File Copy"))
            {
                F_Do_LoadMetaFiles();
                F_Do_CopyMetaFiles();
                F_Do_CheckMetaFiles();
            }

            ShowState();

            GUI.enabled = defaultEnabled;
        }

        bool m_isExcuting;        

        void OpenTargetFilePanel()
        {
            string path = EditorUtility.OpenFilePanel("Load png file", m_targetFilePath, "");
            if(path.Length > 0)
            {
                m_targetFilePath = path;
                m_fileExtension = m_targetFilePath.Split('.')[1];
            }
        }
        string m_targetFilePath = "";
        string m_fileExtension = "";

        void DoShowCopyTargetPathAndExtension()
        {
            m_ScrollPosOfCopyTargetPath = GUILayout.BeginScrollView(m_ScrollPosOfCopyTargetPath);

                GUILayout.BeginHorizontal();

                    GUILayout.Label("Current copy target path:");
                    m_targetFilePath = GUILayout.TextField(m_targetFilePath);
                    
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                    GUILayout.Label("Current copy target extension:");
                    m_fileExtension = GUILayout.TextField(m_fileExtension);

                GUILayout.EndHorizontal();

            GUILayout.EndScrollView();
        }
        private Vector2 m_ScrollPosOfCopyTargetPath;

        void OpenDestinationDirectoryPanel()
        {
            string path = m_destinationFolderPath;
            path = EditorUtility.OpenFolderPanel("Open Destination Folder", path, "");
            if (path.Length > 0)
                m_destinationFolderPath = path;
        }
        string m_destinationFolderPath = "";

        void DoShowDestinationFolderPath()
        {
            m_ScrollPosOfDestinationFolderPath = GUILayout.BeginScrollView(m_ScrollPosOfDestinationFolderPath);

                GUILayout.BeginHorizontal();

                  GUILayout.Label("Current destination folder path:");
                  m_destinationFolderPath = GUILayout.TextField(m_destinationFolderPath);

                GUILayout.EndHorizontal();

            GUILayout.EndScrollView();
        }
        private Vector2 m_ScrollPosOfDestinationFolderPath;

        void F_Do_LoadMetaFiles()
        {
            m_MetaLoadOfficer.F_LoadMetaFileContents(m_targetFilePath, m_destinationFolderPath, m_fileExtension);
        }
        MetaLoadOfficer m_MetaLoadOfficer = new MetaLoadOfficer();

        void F_Do_CopyMetaFiles()
        {
            m_MetaCopyOfficer.F_StartCoroutineCopyMetaFile(m_MetaLoadOfficer);
            m_isExcuting = true;
        }
        MetaCopyOfficer m_MetaCopyOfficer = new MetaCopyOfficer();

        void F_Do_CheckMetaFiles()
        {
            m_CheckEditorCoroutine =
                new EditorCoroutine(F_StartCheckMetaFileContentsIdenticalCoroutine(), F_CallBack_CheckContents);

            m_checkResultMessage = "";
        }
        private EditorCoroutine m_CheckEditorCoroutine;
        string m_checkResultMessage = "";

        private IEnumerator F_StartCheckMetaFileContentsIdenticalCoroutine()
        {
            bool startCheck = false;

            for (; ; )
            {
                if (m_MetaCopyOfficer.P_CopyMetaIsEnded == false)
                {
                    
                }
                else
                {
                    if (!startCheck)
                    {
                        startCheck = true;

                        m_MetaLoadOfficer.F_ReloadData();
                        m_MetaCheckOfficer.F_StartCoroutineCheckCopyResult(m_MetaLoadOfficer);
                    }
                    else if (m_MetaCheckOfficer.P_CheckingResultEnd)
                    {
                        break;
                    }
                }

                yield return null;
            }
        }
        MetaCheckOfficer m_MetaCheckOfficer = new MetaCheckOfficer();

        private void F_CallBack_CheckContents()
        {
            m_isExcuting = false;

            if (m_MetaCheckOfficer.P_CopyResultIdentical == false)
                m_checkResultMessage = "Copy failed.";
            else
                m_checkResultMessage = "Copy succeed.";

        }

        void ShowState()
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("State : ");

            if (m_isExcuting)
            {
                if (m_MetaCopyOfficer.P_CopyMetaIsEnded)
                {
                    GUILayout.Label("Execute checking...");
                }
                else {
                    GUILayout.Label("Execute copying...");
                }
            }
            else if(m_checkResultMessage.Length > 0)
                GUILayout.Label(m_checkResultMessage);
            else
                GUILayout.Label("Standby.");

            GUILayout.EndHorizontal();
        }

    }


}



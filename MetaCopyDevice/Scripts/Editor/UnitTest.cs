using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace MetaCopyDevice
{
    public class UnitTest : EditorWindow
    {
        [MenuItem("MyComp/MetaCopy/UnitTest")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(UnitTest));
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
            this.titleContent.text = "MetaCopyTest";
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Do Load Metafile test"))
                F_Test_LoadMetaFiles();

            if (GUILayout.Button("Do Copy Metafile test"))
            {
                F_Test_LoadMetaFiles();
                F_Test_CopyMetaFiles();
                F_Test_CheckMetaFiles();
            }
        }        

        public void F_Test_LoadMetaFiles() {
            Debug.Log("Execute F_Test_LoadMetaFiles");

            F_SetTestFilePath();

            m_MetaLoadOfficer.F_LoadMetaFileContents(m_targetFilePath, m_destinationFolderPath, m_fileExtension);
        }
        MetaLoadOfficer m_MetaLoadOfficer = new MetaLoadOfficer();

        private void F_SetTestFilePath()
        {
            m_targetFilePath = Application.dataPath + "/MetaCopyDevice/Resources/UnitTest/targetSprite.png";
            m_destinationFolderPath = Application.dataPath + "/MetaCopyDevice/Resources/UnitTest/TestDestination";
            m_fileExtension = m_targetFilePath.Split('.')[1];
        }
        string m_targetFilePath;    
        string m_destinationFolderPath;
        string m_fileExtension;

        public void F_Test_CopyMetaFiles() {

            Debug.Log("Execute F_Test_CopyMetaFiles");

            m_MetaCopyOfficer.F_StartCoroutineCopyMetaFile(m_MetaLoadOfficer);
        }
        MetaCopyOfficer m_MetaCopyOfficer = new MetaCopyOfficer();

        public void F_Test_CheckMetaFiles()
        {
            Debug.Log("Execute F_Test_CheckMetaFiles");

            m_CheckEditorCoroutine =
                new EditorCoroutine(F_StartCheckMetaFileContentsIdenticalCoroutine(), F_CallBack_CheckContents);
        }
        private EditorCoroutine m_CheckEditorCoroutine;

        private IEnumerator F_StartCheckMetaFileContentsIdenticalCoroutine()
        {
            bool startCheck = false;

            for (; ; )
            {
                if (m_MetaCopyOfficer.P_CopyMetaIsEnded == false)
                {
                    Debug.Log("Waiting meta file copy...");
                }
                else
                {
                    if (!startCheck)
                    {
                        Debug.Log("Start meta file check...");

                        startCheck = true;

                        m_MetaLoadOfficer.F_ReloadData();
                        m_MetaCheckOfficer.F_StartCoroutineCheckCopyResult(m_MetaLoadOfficer);
                    }
                    else if(m_MetaCheckOfficer.P_CheckingResultEnd)
                    {
                        Debug.Assert(m_MetaCheckOfficer.P_CopyResultIdentical, "Meta file is not identical between target and destination.");
                        break;
                    }
                }

                yield return null;
            }
        }
        MetaCheckOfficer m_MetaCheckOfficer = new MetaCheckOfficer();

        private void F_CallBack_CheckContents()
        {
            Debug.Log("Check finished.");
        }

    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace MetaCopyDevice
{
    public class MetaLoadOfficer
    {
        public string P_TargetFilePath { get { return m_targetFilePath; } }
        public string P_DestinationFolderPath { get { return m_destinationFolderPath; } }
        public string P_FileExtension { get { return m_fileExtension; } }
        public List<string> P_AllDestinationMetaFilePath { get { return m_allDestinationMetaFilePath; } }

        public List<string> P_TargetMetaFileContents { get { return m_targetMetaContents; } }
        public Dictionary<string, List<string>> P_DestinationMetaFileContents { get { return m_destinationMetaContents; } }

        public void F_ReloadData()
        {
            F_LoadTargetMetaFiles();
            F_LoadDestinationMetaFiles();
        }

        public void F_LoadMetaFileContents(string targetFilePath, string destinationFolderPath, string fileExtension = ".*") {

            m_targetFilePath = targetFilePath;
            m_destinationFolderPath = destinationFolderPath;
            m_fileExtension = fileExtension;
            m_allDestinationMetaFilePath = F_GetAllMetaFilePathfromDestinationFolder(m_destinationFolderPath, m_fileExtension);

            F_LoadTargetMetaFiles();
            F_LoadDestinationMetaFiles();
        }
        private string m_targetFilePath = "";
        private string m_destinationFolderPath = "";
        private string m_fileExtension = ".*";
        private List<string> m_allDestinationMetaFilePath = new List<string>();

        private void F_LoadTargetMetaFiles()
        {
            m_targetMetaContents = F_LoadFileContentsFromPath(m_targetFilePath+".meta");
        }
        private List<string> m_targetMetaContents = new List<string>();


        private void F_LoadDestinationMetaFiles()
        {
            m_destinationMetaContents.Clear();

            foreach (string destinationFilePath in m_allDestinationMetaFilePath)
            {
                List<string> metaContents = F_LoadFileContentsFromPath(destinationFilePath);
                m_destinationMetaContents.Add(destinationFilePath, metaContents);

                //Debug.Log("Meta contents of ["+ destinationFilePath + "] size = " + metaContents.Count);
            }

            //Debug.Log("m_destinationMetaContents size = "+ m_destinationMetaContents.Count);
        }
        private Dictionary<string, List<string>> m_destinationMetaContents = new Dictionary<string, List<string>>();

        private List<string> F_GetAllMetaFilePathfromDestinationFolder(string destinationFolderPath, string extension = "*")
        {
            string[] allDestinationMetaFilePaths = Directory.GetFiles(destinationFolderPath, "*." + extension + ".meta");

            List<string> result = new List<string>(allDestinationMetaFilePaths);

            return result;
        }

        private List<string> F_LoadFileContentsFromPath(string path)
        {
            List<string> contents = new List<string>();

            //Debug.Log("path = [" + path + "]");

            foreach (string line in File.ReadLines(path))
            {
                contents.Add(line);
                //Debug.Log(line);
            }
                
            return contents;
        }



    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace MetaCopyDevice
{
    public class MetaCopyOfficer
    {
        public bool P_CopyMetaIsEnded { get { return m_copyMetaEnded; } }

        public void F_StartCoroutineCopyMetaFile(MetaLoadOfficer loadOfficer)
        {
            m_copyMetaEnded = false;

            m_MetaLoadOfficer = loadOfficer;

            m_EditorCoroutine_CopyMetaFile = new EditorCoroutine(F_CopyMetaFileContentsIntoAllFolderFiles(), F_Callback_CopyMeta);
        }
        private MetaLoadOfficer m_MetaLoadOfficer;
        private EditorCoroutine m_EditorCoroutine_CopyMetaFile;

        private IEnumerator F_CopyMetaFileContentsIntoAllFolderFiles()
        {
            foreach (string destinationMetaFilePath in m_MetaLoadOfficer.P_AllDestinationMetaFilePath)
            {
                F_CopyMetaFileContents(
                    m_MetaLoadOfficer.P_DestinationMetaFileContents[destinationMetaFilePath],
                    m_MetaLoadOfficer.P_TargetMetaFileContents);

                F_WriteMetaContentsIntoPath(destinationMetaFilePath);

                yield return new WaitForSeconds(0.01f);
            }

        }

        private void F_CopyMetaFileContents(List<string> destinationLines, List<string> targetLines)
        {

            List<string> newMetaContents = new List<string>();

            //Must have first 2 lines.
            newMetaContents.Add(destinationLines[0]);
            newMetaContents.Add(destinationLines[1]);

            //First 2 lines can not be copied.
            for (int n = 2; n < targetLines.Count; n++)
            {
                newMetaContents.Add(targetLines[n]);
            }

            m_MetaContentLines_Destination = newMetaContents;
        }

        List<string> m_MetaContentLines_Destination = new List<string>();

        private void F_WriteMetaContentsIntoPath(string path)
        {
            File.WriteAllLines(path, m_MetaContentLines_Destination);
        }

        private void F_Callback_CopyMeta()
        {
            m_copyMetaEnded = true;
            AssetDatabase.Refresh();
        }
        bool m_copyMetaEnded;
    }

}

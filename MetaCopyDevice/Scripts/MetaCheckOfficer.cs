using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MetaCopyDevice
{
    public class MetaCheckOfficer
    {
        public bool P_CheckingResultEnd { get { return m_checkingResultEnd; } }
        public bool P_CopyResultIdentical { get { return m_copyResultIdentical; } }

        public void F_StartCoroutineCheckCopyResult(MetaLoadOfficer metaLoadOfficer)
        {
            F_ResetCheckParameters();
            m_metaLoadOfficer = metaLoadOfficer;

            m_EditorCoroutine_CheckCopyResult = new EditorCoroutine(F_CheckCopyResultIdentical(), F_Callback_CheckMeta);
        }
        private MetaLoadOfficer m_metaLoadOfficer;
        private EditorCoroutine m_EditorCoroutine_CheckCopyResult;

        private void F_ResetCheckParameters()
        {
            m_checkingResultEnd = false;
            m_copyResultIdentical = true;
        }
        bool m_checkingResultEnd = false;
        bool m_copyResultIdentical = true;

        private IEnumerator F_CheckCopyResultIdentical()
        {
            List<string> targetMeta = m_metaLoadOfficer.P_TargetMetaFileContents;
            List<string> destinationMetaFilePathList = m_metaLoadOfficer.P_AllDestinationMetaFilePath;

            if (destinationMetaFilePathList.Count <= 0)
                m_copyResultIdentical = false;

            foreach (string destinationMetaFilePath in destinationMetaFilePathList)
            {

                List<string> desMeta = m_metaLoadOfficer.P_DestinationMetaFileContents[destinationMetaFilePath];
                if (desMeta.Count != targetMeta.Count)
                {
                    m_copyResultIdentical = false;
                    break;
                }

                for (int n = 2; n < desMeta.Count; n++)
                {
                    if (desMeta[n].Equals(targetMeta[n]) == false)
                    {
                        m_copyResultIdentical = false;
                        break;
                    }
                }

                yield return null;

            }
        }

        private void F_Callback_CheckMeta()
        {
            m_checkingResultEnd = true;
        }

    }

}

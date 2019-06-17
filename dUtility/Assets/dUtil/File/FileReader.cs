using UnityEngine;
using System.IO;
using System.Collections.Generic;


namespace du.File {

    public interface IFReader {
        int LineLength { get; }
        string ReadAt(int line);
    }

    public class FReader : IFReader {

        #region field
        IList<string> m_lines = null;
        #endregion

        #region property
        public int LineLength { get { return m_lines?.Count ?? 0; } }
        #endregion

        #region ctor/dtor
        /// <param name="filePath"> Asset/MyData/[filePath].csv </param>
        public FReader(string fileName) {
            m_lines = new List<string>(
                System.IO.File.ReadAllText(App.AppManager.DataPath + fileName).Split('\n'));
        }
        #endregion

        #region public
        public string ReadAt(int line) {
            if (0 <= line && line < LineLength) { return m_lines[line]; }
            else { return null; }
        }
        #endregion

    }

}
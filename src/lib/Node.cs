using System;

namespace HanyaKipas.Lib
{
    public sealed class Node
    {
        private string info;

        /// <summary>
        /// Constructor node
        /// </summary>
        /// <param name="_info">isi info dari Node</param>
        public Node(string _info)
        {
            info = _info;
        }

        public string GetInfo()
        {
            return info;
        }

        /// <summary>
        /// Fungsi untuk membandingkan sebuah node dengan node lainnya
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <returns>
        /// 0 jika kedua node sama
        /// int < 0 jika node pertama lebih kecil
        /// int > 0 jika node pertama lebih besar
        /// </returns>
        public static int Compare(Node n1, Node n2)
        {
            return string.Compare(n1.info, n2.info);
        }

        public override bool Equals(object obj)
        {
            return obj is Node node &&
                   info == node.info;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(info);
        }
    }
}

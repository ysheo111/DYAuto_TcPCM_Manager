using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcPCM_Connect
{
    // Create a node sorter that implements the IComparer interface.
    public class NodeSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;

            if (tx.Tag == null) return -1;
            else if (ty.Tag == null) return 0;
            if (tx.Tag != ty.Tag)
                return ((int)tx.Tag).CompareTo((int)ty.Tag);

            return string.Compare(tx.Text, ty.Text, true);
        }
    }
}

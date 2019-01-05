using System;
using System.Collections.Generic;
using System.Text;

namespace ezModel.BaseModel
{
    public class JsTreeNode
    {
        public Int32 Id { get; set; }

        public Int32 ParentId { get; set; }

        public String Title { get; set; }
        public List<JsTreeNode> Nodes { get; set; }

        public JsTreeNode(Int32 id, String title, Int32 pid = 0)
        {
            Id = id;
            Title = title;
            ParentId = pid;
            Nodes = new List<JsTreeNode>();
        }
        public JsTreeNode(String title = "选择全部")
            : this(0, title, 0)
        {
        }
    }
}

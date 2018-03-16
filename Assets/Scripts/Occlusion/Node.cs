using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iso.occlusion
{
    public class SortNode
    {
        //保存所有比自己ZOrder值小的元素。children之间是相等关系。
        protected List<SortNode> m_Children=new List<SortNode>();

        //父结点
        protected SortNode m_Parent=null;
        
        //范围
        protected Rect m_Rect;

        public void AddChild(SortNode child)
        {
            m_Children.Add(child);
            child.parent = this;
        }

        public void RemoeChild(SortNode child)
        {
            m_Children.Remove(child);
        }

        public int GetDeep()
        {
            int deep = 0;
            SortNode parent = m_Parent;
            while (parent != null)
            {
                ++deep;
                parent = parent.parent;
            }
            return deep;
        }

        public List<SortNode> children
        {
            get
            {
                return m_Children;
            }

            set
            {
                m_Children = value;
            }
        }

        public SortNode parent
        {
            get
            {
                return m_Parent;
            }
            set
            {
                if (m_Parent != null)
                {
                    m_Parent.RemoeChild(this);
                }
                m_Parent = value;
            }
        }

        public Rect rect
        {
            get
            {
                return m_Rect;
            }
            set
            {
                m_Rect = value;
            }
        }

    }
}


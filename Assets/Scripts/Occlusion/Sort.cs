using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iso.occlusion
{
    public class Sort
    {
        protected SortNode m_RootNode;

        public void Insert(SortNode node)
        {

        }

        bool CompareChildren(SortNode current,SortNode target,int deep)
        {
            bool targetIsAdded = false;

            for(int i = 0, l = current.children.Count; i < l; ++i)
            {

            }

            return targetIsAdded;
        }

        bool Compare(SortNode current,SortNode target,int deep)
        {
            bool targetIsAdded = false;

            int side = CaculateSide(current.rect, target.rect);
            if (side < 0)
            {
                //小于target,目标结点遮挡当前结点。
                //比较当前结点和目标结点的深度值。
                //需要检查深度值。如果检查点所在的深度值大于等于当前结点所在的深度值，那么移过去深度值会增加，则执行移动操作，否则不需要
                if (target.GetDeep() >= deep)
                {
                    target.AddChild(current);
                }

                //不用处理当前结点的子结点
            }
            else if (side > 0)
            {
                //大于target,当前结点遮挡目标结点。
                //继续比较目标结点和当前结点的子结点之间的关系。
                bool subAddedFlag = CompareChildren(current, target, deep);

                if (!subAddedFlag)
                {

                    if (target.parent!=null)
                    {
                        //如果target已经被添加到树中，则比较二者的深度值.使用深度值较大者。
                        if (deep >= target.GetDeep())
                        {
                            current.AddChild(target);
                        }
                    }
                    else
                    {
                        current.AddChild(target);
                    }
                }

                //检查结点必定会被添加到当前结点的树中(子结点或子孙结点)
                targetIsAdded = true;
            }
            else
            {
                //等于target
                //由于关系不明确，需要处理子元素
                CompareChildren(current, target, deep);
                //不处理返回结果，由父结点处理
            }
            return targetIsAdded;
        }

        /**
         * 计算二个矩形的位置关系
         * 这里使用的屏幕坐标。
                 |        |
             2  |  1   | 0
           ----|-----|----
             1  |  0   | -1
           ----|-----|----
             0  | -1  | -2
                 |       |
 
         * 中间是A,其它8个位置表示B可能出现的位置。
         * 反回值表示A-B的值
         * 大于0,表示A大于B(A遮挡B);
         * 小于0,表示A小于B(B遮挡A);
         * 等于0,表示A等于B(相互不遮挡).
         * 注意屏幕坐标和opengl的坐标
 
         *  opengl的斜视角的关系
            
 
          \  2 /
           \  /
         1  \/  1
      \     /\    /
       \   /  \  /
        \ /  0 \/
     0   /\    /\   0
        /  \  /  \
       / -1 \/ -1 \
            /\
           /  \
          / -2 \
         */
        int CaculateSide(Rect a,Rect b)
        {
            int x, y = 0;

            //大于等于
            if (b.xMin >= a.xMax)// b.xMin->a.xMax || Mathf.Abs(b.xMin-a.xMax)<0.0001
            {
                //右
                x = 1;
            }
            //小于等于
            else if (b.xMax <= a.xMin)//b.xMax<a.xMin || Mathf.Abs(b.xMax-a.xMin)<0.0001
            {
                //左
                x = -1;
            }
            else//b.xMax<=a.xMax && b.xMin>=a.xMin(内中),b.xMax>=a.xMax && b.xMin<=a.xMin(外中) 都是中
            {
                //中
                x = 0;
            }

            //大于等于
            if (b.yMin >= a.yMax) // b.yMin>a.yMax||Mathf.Abs(b.yMin-a.yMax)<0.0001
            {
                //上
                y = 1;
            }
            // 小于等于
            else if (b.yMax<=a.yMin)//b.yMax<a.yMin || Mathf.Abs(b.yMax-a.yMin)<0.0001
            {
                //下
                y = -1;
            }
            else//b.yMax<=a.yMax && b.yMin>=a.yMin(内中),b.yMax>=a.yMax && b.yMin<=a.yMin(外中) 都是中
            {
                //中
                y = 0;
            }

            return x+y;
        }

        public SortNode rootNode
        {
            get
            {
                return m_RootNode;
            }

            set
            {
                m_RootNode = value;
            }
        }
    }
}


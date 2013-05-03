using System.Collections.Generic;

namespace ASML_N7
{
    public class TargetManager
    {
        private static TargetManager m_TargetManager = null;
        private List<Target> TargetList;

        /**Create delegate**/

        public delegate void AddTarget(object sender, Target target);

        /**Create event**/

        public event AddTarget AddedTarget;

        public static TargetManager GetInstance()
        {
            if (m_TargetManager == null)
            {
                m_TargetManager = new TargetManager();
            }

            return m_TargetManager;
        }

        private TargetManager()
        {
            TargetList = new List<Target>();
        }

        public void Add_Targets(Target target)
        {
            if (target != null)
            {
                TargetList.Insert(target._targetCount, target);
                AddedTarget(this, target);
            }
        }

        public void ClearList()
        {
            foreach (Target target in TargetList)
            {
                target.DecrementTargetCount();
            }
            TargetList.Clear();
        }
        public List<Target> GetList()
        {
            return TargetList;
        }
    }
}
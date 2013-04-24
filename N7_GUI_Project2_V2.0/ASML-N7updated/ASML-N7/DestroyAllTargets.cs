using System.Collections.Generic;

namespace ASML_N7
{
    public class DestroyAllTargets : DestroyMode
    {
        private TargetManager targetManager = TargetManager.GetInstance();
        private List<Target> uninitalizedList;
        private List<Target> killList;

        public DestroyAllTargets()
        {
            uninitalizedList = targetManager.GetList();
            killList = new List<Target>();

            foreach (Target target in uninitalizedList)
            {
                killList.Add(target);
            }
        }

        public override List<Target> getTargets()
        {
            return killList;
        }
    }
}
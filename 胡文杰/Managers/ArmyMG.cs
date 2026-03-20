namespace NoBadConflicts
{
    internal class ArmyMG : ManagerBase<ArmyMG>
    {
        public List<ArmyBase> armyDataOutF;
        public List<int> armyDataInF;
        public LinkedList<ArmyBase>? armyList;

        public void InitArmyData() //进入战斗初始化部队数据并新建链表
        {
            armyDataInF = new List<int>();
            for (int i = 0; i < armyDataOutF.Count; i++)
            {
                armyDataInF.Add(armyDataOutF[i].num);
            }
            armyList = new LinkedList<ArmyBase>();
        }

        public bool IsEmpty() //判断是否还有部队
        {
            if( (armyList == null || armyList.Count == 0) && armyDataInF.Sum() == 0) return true;
            else return false;
        }

        public void ArmyAction(GameMG gm) //部队行动和移除
        {
            if (armyList == null || armyList.Count == 0)
                return;
            List<ArmyBase> armyToRemove = new List<ArmyBase>();
            foreach (var army in armyList)
            {
                army.Behaviors(gm);
                if (army.ShouldDie())
                {
                    army.WhenDie(gm);
                    armyToRemove.Add(army);
                }
            }
            foreach (var army in armyToRemove)
            {
                armyList.Remove(army);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASML_N7
{
    public class Target
    {
        static int targetCount = -1;

        /**xPos, yPos, zPos are used for identifying target coordinates**/
        private int xPos;
        private int yPos;
        private int zPos;

        /**String name will be used to hold the name value of targets read in by
         * the Ini/Xml target files**/
        private string name;

        /**_isFriend is used to identify if the target is a freind or foe**/
        private bool _isFriend;

        /**_isEliminated is used to identify if the target has been successfully
         * eliminated(Hit)**/
        private bool _isEliminated;

        public int _targetCount
        {
            get { return targetCount; }
            set { targetCount = value; }
        }

        /**Get/Set for int xPos**/
        public int XPosition
        {
            get { return xPos; }
            set { xPos = value; }
        }

        /**Get/Set for int yPos**/
        public int YPosition
        {
            get { return yPos; }
            set { yPos = value; }
        }

        /**Get/Set for int zPos**/
        public int ZPosition
        {
            get { return zPos; }
            set { zPos = value; }
        }

        /**Get/Set for string name**/
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /**Get/Set for bool _isFriend**/
        public bool isFriend
        {
            get { return _isFriend; }
            set { _isFriend = value; }
        }

        /**Get/Set for bool _isEliminated**/
        public bool isEliminated
        {
            get { return _isEliminated; }
            set { _isEliminated = value; }
        }

        /**Default constructor for the Target class all x,y,z positions are
         * initialized to 0 and name is set to null. All targets before being
         * identified are labeled as unfriendly and not eliminated**/
        public Target()
        {
            targetCount++;

            xPos = 0;
            yPos = 0;
            zPos = 0;

            name = null;

            _isFriend = false;
            _isEliminated = false;
        }

        /**Overloaded constructor for the Target class xPosition, yPosition, 
         * zPosition, targetName, isFriend, and isEliminated are used to set
         * the data values in the Target class object**/
        public Target(int xPosition, int yPosition, int zPosition, string targetName, bool isFriend, bool isEliminated)
        {
            targetCount++;

            xPos = xPosition;
            yPos = yPosition;
            zPos = zPosition;

            name = targetName;

            _isFriend = isFriend;
            _isEliminated = isEliminated;
        }
    }
}

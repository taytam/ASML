using System;

namespace ASML_N7
{
    public class Target
    {
        private static int targetCount = -1;

        /**xPos, yPos, zPos are used for identifying target coordinates**/
        private int xPos;
        private int yPos;
        private int zPos;
        private int theta;
        private int phi;

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

        public int Theta
        {
            get { return theta; }
            set { theta = value; }
        }

        public int Phi
        {
            get { return phi; }
            set { phi = value; }
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
            phi = 0;
            theta = 0;

            name = null;

            _isFriend = false;
            _isEliminated = false;
        }

        public void coordinateConversion(int xPosition, int yPosition, int zPosition)
        {
            const double radToDeg = 57.29577;
            double r = Math.Sqrt((xPosition * xPosition) + (yPosition * yPosition) + (zPosition * zPosition));
            double doubleTheta = Math.Acos(zPosition / r);
            double doublePhi = Math.Atan2(yPosition, xPosition);

            double ThetaTimesrtg = radToDeg * doubleTheta;
            double PhiTimesrtg = radToDeg * doublePhi;

            Theta = Convert.ToInt32(ThetaTimesrtg);
            Phi = Convert.ToInt32(PhiTimesrtg);
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

            coordinateConversion(xPos, yPos, zPos);

            name = targetName;

            _isFriend = isFriend;
            _isEliminated = isEliminated;
        }

        public void DecrementTargetCount()
        {
            targetCount--;
        }
    }
}
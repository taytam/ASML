namespace ASML_N7
{
    public class SearchAndDestroyFactory
    {
        /**Check_File_Type is used for checking the file type
         * and then returning the right type of FileReader**/

        public DestroyMode Check_Mode(string mode)
        {
            DestroyMode searchAndDestroyMode;

            SearchAndDestroyFactory factory = new SearchAndDestroyFactory();

            string path = mode.ToLower();

            switch (path)
            {
                case "destroy only enemies":
                    searchAndDestroyMode = factory.CreateSearchAndDestroyObject(modeType.destroyOnlyEnemies);
                    return searchAndDestroyMode;
                case "destroy only friends":
                    searchAndDestroyMode = factory.CreateSearchAndDestroyObject(modeType.destroyOnlyFriends);
                    return searchAndDestroyMode;
                case "destroy all targets":
                    searchAndDestroyMode = factory.CreateSearchAndDestroyObject(modeType.destroyAllTargets);
                    return searchAndDestroyMode;
                default:
                    return null;
            }
        }

        /**CreateFileReader returns a FileReader of type ini_reader/xml_reader**/

        public DestroyMode CreateSearchAndDestroyObject(modeType file_t)
        {
            DestroyMode mode = null;
            switch (file_t)
            {
                case modeType.destroyOnlyEnemies:
                    mode = new DestroyOnlyEnemies();
                    break;

                case modeType.destroyOnlyFriends:
                    mode = new DestroyOnlyFriends();
                    break;

                case modeType.destroyAllTargets:
                    mode = new DestroyAllTargets();
                    break;

                default:
                    break;
            }
            return mode;
        }
    }

    /**Enumeration for the fileType of Reader or Writer ini or xml**/

    public enum modeType
    {
        destroyAllTargets,
        destroyOnlyEnemies,
        destroyOnlyFriends
    }
}
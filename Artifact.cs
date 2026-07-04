namespace SpaceExpedition
{
    public class Artifact
    {
        private static readonly char[] originalArray = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private static readonly char[] mappedArray = new char[] { 'H', 'Z', 'A', 'U', 'Y', 'E', 'K', 'G', 'O', 'T', 'I', 'R', 'J', 'V', 'W', 'N', 'M', 'F', 'Q', 'S', 'D', 'B', 'X', 'L', 'C', 'P' };
        public string EncodedName { get; private set; }
        public string DecodedName { get; private set; }
        public string Planet { get; private set; }
        public string DiscoveryDate { get; private set; }
        public string StorageLocation { get; private set; }
        public string Description { get; private set; }

        public Artifact(string encodedName, string decodedName, string planet, string discoveryDate, string storageLocation, string description)
        {
            EncodedName = encodedName;
            DecodedName = decodedName;
            Planet = planet;
            DiscoveryDate = discoveryDate;
            StorageLocation = storageLocation;
            Description = description;
        }

        // TODO: Change type to Artifact
        public static void ParseArtifact(string rawLine)
        {
            // 1. Split each line into 5 parts using comma delimeter
            // 2. Extract the encoded name string
            // 3. Call a recursice decoder to get decoded name
            // 4. Return a new Artifact insatnce
        }

        private static char DecodeCharacter(char encodedChar, int level)
        {
            int targetIndex = -1;
            for (int i = 0; i < originalArray.Length; i++)
            {
                if (originalArray[i] == encodedChar)
                {
                    targetIndex = i;
                    break;
                }
            }

            // Base case
            if (level == 1)
            {
                return originalArray[originalArray.Length - (targetIndex + 1)];
            }

            return DecodeCharacter(mappedArray[targetIndex], level - 1);
        }
    }
}

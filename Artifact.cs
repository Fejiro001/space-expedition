using System.Text;

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

        public static Artifact ParseArtifact(string rawLine)
        {
            // Split each line into 5 parts using comma delimeter
            string[] artifactInfo = rawLine.Split(",", 5);
            string encodedName = artifactInfo[0];
            string decodedName = DecodeName(encodedName);

            return new Artifact(encodedName, decodedName, artifactInfo[1], artifactInfo[2], artifactInfo[3], artifactInfo[4]);
        }

        private static string DecodeName(string encodedName)
        {
            StringBuilder sb = new StringBuilder();
            string[] words = encodedName.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                string[] tokens = word.Split("|");

                foreach (string token in tokens)
                {
                    string trimmedToken = token.Trim();
                    char letter = trimmedToken[0];
                    int level = int.Parse(trimmedToken.Substring(1));
                    sb.Append(DecodeCharacter(letter, level));
                }
                sb.Append(' ');
            }

            return sb.ToString().Trim();
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

        public override string ToString()
        {
            return $"| {DecodedName,-20} | {Planet,-15} | {DiscoveryDate,-15} | {StorageLocation}";
        }
    }
}

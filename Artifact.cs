using System.Text;

namespace SpaceExpedition
{
    public class Artifact
    {
        private static readonly char[] originalArray = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private static readonly char[] mappedArray = new char[] { 'H', 'Z', 'A', 'U', 'Y', 'E', 'K', 'G', 'O', 'T', 'I', 'R', 'J', 'V', 'W', 'N', 'M', 'F', 'Q', 'S', 'D', 'B', 'X', 'L', 'C', 'P' };
        // Made all setters private so they are not modified outside the class
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

        /// <summary>
        /// Separates a raw comma-delimited string line from a text file, validates the token sequence, 
        /// and converts it into a structured Artifact object.
        /// </summary>
        /// <param name="rawLine">The comma-separated text string containing the raw artifact attributes.</param>
        /// <returns>A fully initialized Artifact object if valid; otherwise, null if corrupted or invalid.</returns>
        public static Artifact? ParseArtifact(string rawLine)
        {
            if (string.IsNullOrWhiteSpace(rawLine)) return null;

            // Split each line into 5 parts using comma delimeter
            string[] artifactInfo = rawLine.Split(",", 5);

            if (artifactInfo.Length < 5)
            {
                Console.WriteLine($"[DATA WARNING] Skipping corrupted line (Missing fields): \"{rawLine}\"");
                return null;
            }

            string encodedName = artifactInfo[0];

            if (!IsValidEncodedName(encodedName))
            {
                Console.WriteLine($"[DATA WARNING] Skipping record due to invalid token sequence: '{encodedName}'");
                return null;
            }

            string decodedName = DecodeName(encodedName);
            return new Artifact(encodedName, decodedName, artifactInfo[1], artifactInfo[2], artifactInfo[3], artifactInfo[4]);
        }

        private static string DecodeName(string encodedName)
        {
            // Preferred use of StringBuilder to improve memory usage since strings are immutable
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

        /// <summary>
        /// A recursive algorithm that maps an encoded letter through coded arrays 
        /// based on its designated level layer until reaching the mirrored base case.
        /// </summary>
        /// <param name="encodedChar">The character currently being decoded.</param>
        /// <param name="level">The remaining levels of encryption layers left.</param>
        /// <returns>The decoded uppercase character.</returns>
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
            return new string('─', 100) +
                   $"\n{"DECODED NAME:",-18} {DecodedName}\n" +
                   $"{"PLANET:",-18} {Planet}\n" +
                   $"{"DISCOVERY DATE:",-18} {DiscoveryDate}\n" +
                   $"{"LOCATION:",-18} {StorageLocation}\n" +
                   $"{"DESCRIPTION:",-18} {Description}\n" +
                   new string('─', 100); // Generates a clean dividing underline
        }

        // Validates the encoded name before adding to an instance of an Artifact
        private static bool IsValidEncodedName(string encodedName)
        {
            if (string.IsNullOrWhiteSpace(encodedName)) return false;

            string[] words = encodedName.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0) return false;

            foreach (string word in words)
            {
                // Split to get individual letter-level tokens
                string[] tokens = word.Split("|");

                foreach (string token in tokens)
                {
                    // Token must be at least 2 characters
                    if (token.Length < 2) return false;

                    // First character must be a letter and uppercase
                    if (!char.IsUpper(token, 0)) return false;

                    // Everything after the letter must be a number
                    string numericPart = token.Substring(1);
                    if (!int.TryParse(numericPart, out int result))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}

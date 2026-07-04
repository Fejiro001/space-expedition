namespace SpaceExpedition
{
    public class Artifact
    {
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

        }
    }
}

namespace SpaceExpedition
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Artifact[] inventory = new Artifact[5];
            int artifactCount = 0;
            string vaultFile = "galactic_vault.txt";

            Console.WriteLine("--- GALACTIC INVENTORY SYSTEM INITIALIZED ---");
        }

        static void ResizeInventory(ref Artifact[] inventory)
        {
            int newSize = inventory.Length * 2;
            Artifact[] biggerInventory = new Artifact[newSize];

            for (int i = 0; i < inventory.Length; i++)
            {
                biggerInventory[i] = inventory[i];
            }

            Console.WriteLine($"[SYSTEM ALERT] Inventory full. Capacity expanded from {inventory.Length} to {newSize}.");

            inventory = biggerInventory;
        }

        static void ReadFile(string file, ref int artifactCount, ref Artifact[] inventory)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (artifactCount == inventory.Length)
                    {
                        ResizeInventory(ref inventory);
                    }
                    inventory[artifactCount] = Artifact.ParseArtifact(line);
                    artifactCount++;
                }
            }
        }

        // Using insertion sort
        static void SortInventory(ref int artifactCount, ref Artifact[] inventory)
        {
            for (int i = 1; i < artifactCount; i++)
            {
                Artifact key = inventory[i];
                int j = i - 1;

                while (j >= 0 && inventory[j].DecodedName.CompareTo(key.DecodedName) > 0)
                {
                    inventory[j + 1] = inventory[j];
                    j--;
                }
                inventory[j] = key;
            }
        }
    }
}

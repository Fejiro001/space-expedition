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

            StartExpedition(vaultFile, ref artifactCount, ref inventory);
        }

        static void StartExpedition(string vaultFile, ref int artifactCount, ref Artifact[] inventory)
        {
            ReadFile(vaultFile, ref artifactCount, ref inventory);
            SortInventory(ref artifactCount, ref inventory);

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n=== MAIN MENU ===");
                Console.WriteLine("1. Display Inventory");
                Console.WriteLine("2. Search for an Artifact");
                Console.WriteLine("3. Add New Discovered Artifact");
                Console.WriteLine("4. Save and Exit");
                Console.WriteLine("Select an option:");

                int choice;
                bool correctChoice;

                do
                {
                    correctChoice = int.TryParse(Console.ReadLine().Trim(), out choice)
                        && choice >= 1
                        && choice <= 4;

                    if (!correctChoice)
                    {
                        Console.WriteLine("Invalid option selected. Please try again.");
                    }
                }
                while (!correctChoice);

                switch (choice)
                {
                    case 1:
                        DisplayAllArtifacts(ref artifactCount, ref inventory);
                        break;
                    case 2:
                        SearchInventory(ref artifactCount, ref inventory);
                        break;
                    case 3:
                        AddArtifact(ref artifactCount, ref inventory);
                        break;
                    case 4:
                        // Call file writer
                        running = false;
                        Console.WriteLine("Exiting and saving data... Safe travels, exploorer!");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
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
                inventory[j + 1] = key;
            }
        }

        // Use ImplementBinarySearch to search inventory
        static void SearchInventory(ref int artifactCount, ref Artifact[] inventory)
        {
            string name;

            do
            {
                Console.WriteLine("Enter an Artifact decoded name to search for in the inventory:");
                name = Console.ReadLine().Trim();

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("\nEnter a valid string.");
                }
            }
            while (string.IsNullOrWhiteSpace(name));

            int index = ImplementBinarySearch(ref artifactCount, ref inventory, name, 0, artifactCount - 1);

            if (index < 0)
            {
                Console.WriteLine("Artifact not found.");
            }
            else
            {
                Console.WriteLine($"| {inventory[index].DecodedName,-20} | {inventory[index].Planet,-15} | {inventory[index].DiscoveryDate,-15} | {inventory[index].StorageLocation,-15} | {inventory[index].Description}");
            }
        }

        // Using binary search
        static int ImplementBinarySearch(ref int artifactCount, ref Artifact[] inventory, string target, int left, int right)
        {
            // Base case
            if (left > right)
            {
                return -1;
            }

            int mid = left + (right - left) / 2;

            if (inventory[mid].DecodedName.CompareTo(target) == 0)
            {
                return mid;
            }
            else if (inventory[mid].DecodedName.CompareTo(target) < 0)
            {
                return ImplementBinarySearch(ref artifactCount, ref inventory, target, mid + 1, right);
            }
            else
            {
                return ImplementBinarySearch(ref artifactCount, ref inventory, target, left, mid - 1);
            }
        }

        static void AddArtifact(ref int artifactCount, ref Artifact[] inventory)
        {
            Console.WriteLine("\n┌────────────────────────────────────────┐");
            Console.WriteLine("│      LOG NEW ARCHAEOLOGICAL DISCOVERY  │");
            Console.WriteLine("└────────────────────────────────────────┘");

            Console.WriteLine("Enter Encoded Name (e.g., D3|X2|C6):");
            string encoded = Console.ReadLine().Trim();

            Console.WriteLine("Enter Origin Planet/Celestial Body:");
            string planet = Console.ReadLine().Trim();

            Console.WriteLine("Enter Discovery Date:");
            string date = Console.ReadLine().Trim();

            Console.WriteLine("Enter Secure Storage Location:");
            string location = Console.ReadLine().Trim();

            Console.WriteLine("Enter Artifact Description:");
            string description = Console.ReadLine().Trim();

            string simulatedRawLine = $"{encoded},{planet},{date},{location},{description}";
            Artifact newArtifact = Artifact.ParseArtifact(simulatedRawLine);

            if (newArtifact != null)
            {
                ImplementOrderedInsertion(ref artifactCount, ref inventory, newArtifact);
                Console.WriteLine("\n[SUCCESS] Addition complete. Artifact logged alphabetically.");
            } else
            {
                Console.WriteLine("\n[ERROR] Addition aborted due to the invalid entry above.");
            }
        }

        // Using ordered insertion
        static void ImplementOrderedInsertion(ref int artifactCount, ref Artifact[] inventory, Artifact target)
        {
            if (artifactCount == inventory.Length)
            {
                ResizeInventory(ref inventory);
            }

            int i = artifactCount - 1;

            while (i >= 0 && inventory[i].DecodedName.CompareTo(target.DecodedName) > 0)
            {
                inventory[i + 1] = inventory[i];
                i--;
            }

            inventory[i + 1] = target;
            artifactCount++;
        }

        static void SaveToFile(string outputFile, int artifactCount, Artifact[] inventory)
        {
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                for (int i = 0; i < artifactCount; i++)
                {
                    sw.WriteLine(inventory[i]);
                    sw.WriteLine(",");
                }
            }
            Console.WriteLine($"[SUCCESS] Artifacts successfully exported to {outputFile}");
        }

        static void DisplayAllArtifacts(ref int artifactCount, ref Artifact[] inventory)
        {
            Console.WriteLine($"| {"Decoded Name",-20} | {"Planet",-15} | {"Discovery Date",-15} | {"Storage Location",-15} | Description");
            Console.WriteLine("-----------------------------------------------------");
            for (int i = 0; i < artifactCount - 1; i++)
            {
                inventory[i].ToString();
                Console.WriteLine("-----------------------------------------------------");
            }
        }
    }
}

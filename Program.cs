namespace SpaceExpedition
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StartExpedition();
        }

        static void StartExpedition()
        {
            Artifact[] inventory = new Artifact[5];
            int artifactCount = 0;
            string vaultFile = "../../../galactic_vault.txt";

            Console.WriteLine("--- GALACTIC INVENTORY SYSTEM INITIALIZED ---");

            // Load and sort initial data
            ReadFile(vaultFile, ref artifactCount, ref inventory);
            SortInventory(ref artifactCount, ref inventory);

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n┌────────────────────────────────────────┐");
                Console.WriteLine("│                MAIN MENU               │");
                Console.WriteLine("└────────────────────────────────────────┘");
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
                        for (int i = 0; i < artifactCount; i++)
                        {
                            Console.WriteLine(inventory[i].ToString());
                        }
                        break;
                    case 2:
                        SearchInventory(ref artifactCount, ref inventory);
                        break;
                    case 3:
                        AddArtifact(ref artifactCount, ref inventory);
                        break;
                    case 4:
                        SaveToFile("../../../expedition_summary.txt", artifactCount, inventory);
                        running = false;
                        Console.WriteLine("Exiting and saving data... Safe travels, explorer!");
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
            if (!File.Exists(file))
            {
                Console.WriteLine($"[SYSTEM ERROR] File could not be found at: {file}");
                Console.WriteLine("Initializing empty inventory. Manual entry is possible");
                return;
            }

            try
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
                        Artifact parsed = Artifact.ParseArtifact(line);
                        if (parsed != null)
                        {
                            inventory[artifactCount] = parsed;
                            artifactCount++;
                        }
                    }
                }
                Console.WriteLine($"[SUCCESS] Loaded {artifactCount} successfully.");
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("[SYSTEM ERROR] Access denied. You do not have permission to read this file.");
                Console.WriteLine(e.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine($"[SYSTEM ERROR] An I/O error occurred while reading the file.");
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("[SYSTEM ERROR] An unexpected error occurred while reading the file.");
                Console.WriteLine(e.Message);
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
                Console.WriteLine("Enter an Artifact full decoded name in uppercase to search for in the inventory:");
                name = Console.ReadLine().Trim().ToUpper(); // Automatically made it uppercase regardless

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
                Console.WriteLine($"\n{"DECODED NAME:",-18} {inventory[index].DecodedName}\n" +
                                  $"{"PLANET:",-18} {inventory[index].Planet}\n" +
                                  $"{"DISCOVERY DATE:",-18} {inventory[index].DiscoveryDate}\n" +
                                  $"{"LOCATION:",-18} {inventory[index].StorageLocation}\n" +
                                  $"{"DESCRIPTION:",-18} {inventory[index].Description}\n");
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
            if (inventory[mid].DecodedName.CompareTo(target) < 0)
            {
                return ImplementBinarySearch(ref artifactCount, ref inventory, target, mid + 1, right);
            }
            else if(inventory[mid].DecodedName.CompareTo(target) > 0)
            {
                return ImplementBinarySearch(ref artifactCount, ref inventory, target, left, mid - 1);
            }

            return mid;
        }

        static void AddArtifact(ref int artifactCount, ref Artifact[] inventory)
        {
            Console.WriteLine("\n┌────────────────────────────────────────┐");
            Console.WriteLine("│      LOG NEW ARCHAEOLOGICAL DISCOVERY  │");
            Console.WriteLine("└────────────────────────────────────────┘");

            // Get the Journey log filename from the user
            string fileName = GetNonEmptyInput("Enter the journey log file name (e.g., artifact_name.txt):");
            string relativePath = $"../../../{fileName}";

            if (!File.Exists(relativePath))
            {
                Console.WriteLine($"\n[SYSTEM ERROR] Journey log '{fileName}' could not be found");
                return;
            }

            string rawLine = "";
            try
            {
                using (StreamReader sr = new StreamReader(relativePath))
                {
                    rawLine = sr.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n[SYSTEM ERROR] Could not read journey log");
                Console.WriteLine(e.Message);
                return;
            }

            Artifact newArtifact = Artifact.ParseArtifact(rawLine);

            if (newArtifact != null)
            {
                // Check to see if the artifact allready exists in the inventory
                int duplicateCheck = ImplementBinarySearch(ref artifactCount, ref inventory, newArtifact.DecodedName, 0, artifactCount - 1);

                if (duplicateCheck >= 0)
                {
                    Console.WriteLine($"\n[ALERT] Artifact '{newArtifact.DecodedName}' already exists in the Galactic Vault. Addition aborted.");
                }
                else
                {
                    ImplementOrderedInsertion(ref artifactCount, ref inventory, newArtifact);
                    Console.WriteLine($"\n[SUCCESS] Journey log translated. Artifact '{newArtifact.DecodedName}' logged alphabetically.");
                }
            }
            else
            {
                Console.WriteLine("\n[ERROR] Addition aborted due to invalid formatting in the journey log.");
            }
        }

        // Ensures a user input is not empty
        static string GetNonEmptyInput(string prompt)
        {
            string input = "";

            while (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine().Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("[ALERT] This field is mandatory. Input cannot be empty!");
                }
            }
            return input;
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
            try
            {
                using (StreamWriter sw = new StreamWriter(outputFile))
                {
                    for (int i = 0; i < artifactCount; i++)
                    {
                        sw.WriteLine($"{inventory[i].EncodedName},{inventory[i].Planet},{inventory[i].DiscoveryDate},{inventory[i].StorageLocation},{inventory[i].Description}");
                    }
                }
                Console.WriteLine($"[SUCCESS] Artifacts successfully exported.");
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("[CRITICAL ERROR] Save Failed: Insufficient admin privileges to write.");
                Console.WriteLine(e.Message);
            }
            catch (IOException e)
            {
                Console.WriteLine($"[CRITICAL ERROR] Save Failed: Disk may be full or blocked by another process.");
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("[CRITICAL ERROR] An unexpected error occurred while saving the file.");
                Console.WriteLine(e.Message);
            }
        }
    }
}

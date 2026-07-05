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
    }
}

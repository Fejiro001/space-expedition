# Space Expedition

Space Expedition is a C# console application that acts as a digital inventory for a galactic explorer. It reads encoded data from a text file, translates the secret artifact names, and lets you interactively manage your collection.

## Features

* **Automatic Decoding:** Reads an initial database of items (`galactic_vault.txt`) and automatically decrypts their hidden names using a custom letter-shifting puzzle.
* **Smart Sorting & Searching:** The system automatically organizes your artifacts in alphabetical order. You can use the search tool to type in an artifact's name and instantly pull up its details (like what planet it's from and its description).
* **Add & Validate:** When you discover a new item, you can type in the name of a new text file. The program will read it, check to make sure it isn't already in your inventory (no duplicates allowed!), and safely add it to your list.
* **Safe Saving:** When you are done and choose to exit, the application exports your fully updated inventory into a new file called `expedition_summary.txt` so your data is never lost.

## How the Code Works

The project is neatly organized into two main files:

* **`Program.cs`:** The main engine of the app. It displays the interactive menu, handles reading and saving the text files, keeps the inventory sorted, and safely catches any typing mistakes the user makes.
* **`Artifact.cs`:** The blueprint for a single artifact. It stores all the information for an item (encoded name, decoded name, planet, date, location, and description). It also contains the math and logic needed to translate the secret names letter-by-letter.

## How to Run

1. Open the project in your C# editor (like Visual Studio or VS Code).
2. Make sure your starting data file (`galactic_vault.txt`) is placed in the correct project folder.
3. Run the application to launch the console window.
4. Type a number (1-4) to navigate the on-screen menu:
   * **1** to view all your sorted artifacts.
   * **2** to search for a specific one.
   * **3** to load a new artifact from a journey log.
   * **4** to save your work and close the app.
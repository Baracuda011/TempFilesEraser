using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace TempFilesEraser
{
	// --------------------------------------------------------------------------------
	/// <summary>
	/// Deletes all the temp files and folders from the selected paths.
	/// </summary>
	// --------------------------------------------------------------------------------
	class Program
	{
		/// <summary>
		/// Goes through all paths and deletes files and folders 
		/// </summary>
		/// <created>Nemanja Stankovic,03-Oct-20</created>
		/// <changed>Nemanja Stankovic,03-Oct-20</changed>
		static void Main()
		{
			// Read predefined paths from the app configuration.
			var tempFolders = ConfigurationManager.AppSettings["TempFolders"].ToString().Split(';');

			foreach ( var folder in tempFolders )
			{
				// Display the main notification message.
				DisplayDeleteMessage(folder);

				// Read all subfolders.
				var dirs = Directory.GetDirectories(folder);

				// Read all files.
				var files = Directory.GetFiles(folder);

				if ( dirs.Any() )
				{
					// Delete all existing subfolders.
					DeleteDirectories(dirs);
				}

				if ( files.Any() )
				{
					// Delete all existing files.
					DeleteFiles(files);
				}
			}

			Console.WriteLine("Press Enter to close the application.");
			Console.ReadLine();
		}

		/// <summary>
		/// Displays the main notification message about actions in the current folder.
		/// </summary>
		/// <param name="folder">Folder path (string)</param>
		/// <created>Nemanja Stankovic,03-Oct-20</created>
		/// <changed>Nemanja Stankovic,03-Oct-20</changed>
		private static void DisplayDeleteMessage(string folder)
		{
			Console.WriteLine("* ====================================================================================================");
			Console.WriteLine("* ");
			Console.WriteLine($"* Deleting temp files/folder in: {folder}");
			Console.WriteLine("* ");
			Console.WriteLine("* ====================================================================================================");
			Console.WriteLine("");
		}

		/// <summary>
		/// Deletes all folders in the list.
		/// </summary>
		/// <param name="dirs">List of folders (string[])</param>
		/// <created>Nemanja Stankovic,03-Oct-20</created>
		/// <changed>Nemanja Stankovic,06-Oct-20</changed>
		private static void DeleteDirectories(string[] dirs)
		{
			foreach ( var dir in dirs )
			{
				if ( Directory.Exists(dir) )
				{
					DeleteDirectory(dir);
				}
				else
				{
					Console.WriteLine($"{dir} ... doesn't exist");
				}
			}
		}

		/// <summary>
		/// Deletes all files in the list.
		/// </summary>
		/// <param name="files">List of files (string[])</param>
		/// <created>Nemanja Stankovic,03-Oct-20</created>
		/// <changed>Nemanja Stankovic,06-Oct-20</changed>
		private static void DeleteFiles(string[] files)
		{
			foreach ( var file in files )
			{
				DeleteFile(file);
			}

			Console.WriteLine("");
		}

		/// <summary>
		/// Deletes a folder with the given path.
		/// </summary>
		/// <param name="path">Folder path (string)</param>
		/// <created>Nemanja Stankovic,03-Oct-20</created>
		/// <changed>Nemanja Stankovic,06-Oct-20</changed>
		private static void DeleteDirectory(string path)
		{
			var noErrors = true;

			try
			{
				// Deletes the selected folder.
				Directory.Delete(path, true);
			}
			catch ( IOException )
			{
				// Skips a locked folder.
				noErrors = false;
				Console.WriteLine($"{path} is being used by another process ... skipped");
			}
			catch ( UnauthorizedAccessException )
			{
				// Skips a locked folder.
				noErrors = false;
				Console.WriteLine($"{path} is being used by another process ... skipped");
			}

			if ( noErrors )
			{
				Console.WriteLine($"{path} ... deleted");
			}
		}

		/// <summary>
		/// Deletes a file with the given path.
		/// </summary>
		/// <param name="path">File path (string)</param>
		/// <created>Nemanja Stankovic,03-Oct-20</created>
		/// <changed>Nemanja Stankovic,06-Oct-20</changed>
		private static void DeleteFile(string path)
		{
			var noErrors = true;

			try
			{
				// Deletes the selected file.
				File.Delete(path);
			}
			catch ( IOException )
			{
				// Skips a locked file.
				noErrors = false;
				Console.WriteLine($"{path} is being used by another process ... skipped");
			}
			catch ( UnauthorizedAccessException )
			{
				// Skips a locked file.
				noErrors = false;
				Console.WriteLine($"{path} is being used by another process ... skipped");
			}

			if ( noErrors )
			{
				Console.WriteLine($"{path} ... deleted");
			}
		}
	}
}

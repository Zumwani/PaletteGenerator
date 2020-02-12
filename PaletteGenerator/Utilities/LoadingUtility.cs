using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaletteGenerator.Utilities
{

    /// <summary>Contains utility functions showing a loading screen.</summary>
    static class LoadingUtility
    {

        static readonly List<Task> Tasks = new List<Task>();

        /// <summary>Show the loading screen during action.</summary>
        public static void ShowLoadingScreen(this Action action) =>
            ShowLoadingScreen(Task.Run(action));

        /// <summary>Show the loading screen during task.</summary>
        public static async void ShowLoadingScreen(this Task task)
        {

            if (task == null)
                return;

            Tasks.Add(task);

            if (!blocks.Any())
                await App.Dispatcher.Invoke(async () => await App.Window.LoadingOverlay.Show().Fade(1));
            await task.ContinueWith(OnTaskCompleted);

        }

        static void OnTaskCompleted(Task task = null)
        {

            Tasks.RemoveAll(t => task.Id == task?.Id);

            if (Tasks.Count == 0)
                App.Dispatcher.Invoke(async () => (await App.Window.LoadingOverlay.Fade(0)).Hide()).ConfigureAwait(false);

        }

        public class BlockLoadingScreen : IDisposable
        {
            public BlockLoadingScreen() => blocks.Add(this);
            public void Dispose()       => blocks.Remove(this);
        }

        static readonly List<BlockLoadingScreen> blocks = new List<BlockLoadingScreen>();

        /// <summary>Block loading screen until disposed.</summary>
        public static BlockLoadingScreen KeepLoadingScreenHidden => new BlockLoadingScreen();

    }

}

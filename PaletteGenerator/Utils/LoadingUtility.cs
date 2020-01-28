using System.Threading.Tasks;
using System;
using PaletteGenerator.Utils;
using System.Collections.Generic;
using System.Linq;

namespace PaletteGenerator
{

    public static class LoadingUtility
    {

        static readonly List<Task> Tasks = new List<Task>();

        public static void ClearAll()
        { Tasks.Clear(); OnTaskCompleted(); }

        public static void ShowLoadingScreen(this Action action) =>
            ShowLoadingScreen(Task.Run(action));

        public static async void ShowLoadingScreen(this Task task)
        {

            if (task == null)
                return;

            Tasks.Add(task);

            if (!blocks.Any())
                await App.Current.Dispatcher.Invoke(async () => await WindowUtility.Current.loadingOverlay.Show().Fade(0.5));
            await task.ContinueWith(OnTaskCompleted);

        }

        static void OnTaskCompleted(Task task = null)
        {

            Tasks.RemoveAll(t => task.Id == task?.Id);

            if (Tasks.Count == 0)
                App.Current.Dispatcher.Invoke(async () => (await WindowUtility.Current.loadingOverlay.Fade(0)).Hide()).ConfigureAwait(false);

        }

        public class BlockLoadingScreen : IDisposable
        {
            public BlockLoadingScreen() => blocks.Add(this);
            public void Dispose()       => blocks.Remove(this);
        }

        static readonly List<BlockLoadingScreen> blocks = new List<BlockLoadingScreen>();
        public static BlockLoadingScreen KeepLoadingScreenHidden => new BlockLoadingScreen();

    }

}

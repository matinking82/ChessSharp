using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage.Pickers;
using Microsoft.Maui.Storage;

namespace ChessazorMaui.Services
{
    public class LogServices
    {
        public async Task<bool> SaveLog(string pgn)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    using var stream = new MemoryStream(Encoding.Default.GetBytes(pgn));
                    string filename = "chesslog.txt";
                    var extension = Path.GetExtension(filename);
                    var fileSavePicker = new FileSavePicker();
                    fileSavePicker.SuggestedFileName = filename;
                    fileSavePicker.FileTypeChoices.Add(extension, new List<string> { extension });

                    if (MauiWinUIApplication.Current.Application.Windows[0].Handler.PlatformView is MauiWinUIWindow window)
                    {
                        WinRT.Interop.InitializeWithWindow.Initialize(fileSavePicker, window.WindowHandle);
                    }

                    var result = await fileSavePicker.PickSaveFileAsync();
                    if (result != null)
                    {
                        using (var fileStream = await result.OpenStreamForWriteAsync())
                        {
                            fileStream.SetLength(0);
                            await stream.CopyToAsync(fileStream);
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }


                }
                catch
                {
                    return false;
                }
            });
        }

        public async Task<string> LoadLog()
        {
            return await Task.Run(() =>
            {
                return "";
            });
        }
    }
}
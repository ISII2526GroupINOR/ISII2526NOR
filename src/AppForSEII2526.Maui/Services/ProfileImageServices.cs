using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.Maui.Services
{
    public class ProfileImageServices
    {
        public string? image {  get; set; }

        public event Action? OnImageChanged;

        public async Task LoadImage()
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select your profile image",
                FileTypes = FilePickerFileType.Images
            });

            if(result != null)
            {
                using var stream = await result.OpenReadAsync();
                using var ms = new MemoryStream();

                await stream.CopyToAsync(ms);

                image = Convert.ToBase64String(ms.ToArray());

                OnImageChanged?.Invoke();
            }
        }

        public string GetImageSource()
        {
            return image != null
                ? $"data:image.png;base64,{image}"
                : "Images/defaultprofilepicture.png";
        }
    }
}

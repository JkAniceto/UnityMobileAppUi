using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BookerHomePage : MonoBehaviour
{
    public RawImage rawImage;

    private string apiUrl = "http://127.0.0.1:8000/api/getWorkerImage";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadBookerImageFromAPI());
    }

    IEnumerator LoadBookerImageFromAPI()
    {
        string accessToken = PlayerPrefs.GetString("AccessToken");

        UnityWebRequest www = UnityWebRequest.Get(apiUrl);
        www.SetRequestHeader("Authorization", accessToken);

        Debug.Log("Sending request to: " + apiUrl);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = www.downloadHandler.text;
            Debug.Log("API Response: " + jsonResponse);

            // Parse the JSON array using the wrapper class
            ImageDataArray imageDataArray = JsonUtility.FromJson<ImageDataArray>("{\"images\":" + jsonResponse + "}");

            // Check if the array is not empty
            if (imageDataArray != null && imageDataArray.images.Length > 0)
            {
                string imageUrl = imageDataArray.images[0].url; // Assuming you want the first image in the array

                Debug.Log("Image URL: " + imageUrl);

                StartCoroutine(LoadImageTexture(imageUrl));
            }
            else
            {
                Debug.LogError("Empty or invalid API response");
                Debug.LogError("No Image Found");
            }
        }
        else
        {
            Debug.LogError("Error fetching booker image: " + www.error);
        }
    }


    IEnumerator LoadImageTexture(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            if (texture != null)
            {
                rawImage.texture = texture;
                Debug.Log("Image loaded successfully!");
            }
            else
            {
                Debug.LogError("Loaded texture is null!");
            }
        }
        else
        {
            Debug.LogError("Error loading booker image texture: " + www.error);
        }
    }


    [System.Serializable]
    private class ImageData
    {
        public string file_path;
        public string file_name;
        public string url;
    }

    [System.Serializable]
    private class ImageDataArray
    {
        public ImageData[] images;
    }

    //Kulang ng path file sa android device
    public void UploadImageBooker()
    {
        // Open a file dialog to select an image from the user's device
        string imagePath = UnityEditor.EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg");
        if (imagePath.Length != 0)
        {
            StartCoroutine(UploadImageToAPI(imagePath));
        }
    }

    IEnumerator UploadImageToAPI(string imagePath)
    {
        byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
        WWWForm form = new WWWForm();
        form.AddBinaryData("image", imageBytes, "image.png", "image/png");

        string accessToken = PlayerPrefs.GetString("AccessToken");
        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:8000/api/editWorkerImage", form);
        www.SetRequestHeader("Authorization", accessToken);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Image uploaded successfully!");
            // Handle the response if needed
        }
        else
        {
            Debug.LogError("Error uploading worker image: " + www.error);
        }
    }
}

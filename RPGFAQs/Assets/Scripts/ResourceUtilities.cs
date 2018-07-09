using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    class ResourceUtilities : MonoBehaviour
    {
        public AssetBundle GetAssetBundle(string bundleName)
        {
            var bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "dialogue"));
            if (bundle == null)
            {
                UnityWebRequest req = null;
                StartCoroutine(LoadFromWeb(bundleName, bundle, req));
                while(req == null || !req.isDone)
                {
                    System.Threading.Thread.Sleep(200);
                }
            }
            return bundle;
        }

        private IEnumerator LoadFromWeb(string bundleName, AssetBundle bundle, UnityWebRequest req)
        {
            var uri = "file:///" + Application.dataPath + "/StreamingAssets/" + bundleName;
            req = UnityWebRequestAssetBundle.GetAssetBundle(uri, 0);
            yield return req.SendWebRequest();
            bundle = DownloadHandlerAssetBundle.GetContent(req);
        }
    }
}

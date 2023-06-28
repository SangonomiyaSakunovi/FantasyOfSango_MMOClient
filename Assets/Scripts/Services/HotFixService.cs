using System.Collections;
using UnityEngine;
using YooAsset;

//Developer: SangonomiyaSakunovi

public class HotFixService : BaseService
{
    public static HotFixService Instance;

    private ResourceDownloaderOperation downloaderOperation;
    private ResourcePackage package;

    public override void InitService()
    {
        base.InitService();
        Instance = this;
    }

    public void PrepareHotFix()
    {
        StartCoroutine(PrepareAssets());
    }

    public void RunHotFix()
    {
        StartCoroutine(RunDownloader(downloaderOperation));
    }

    public void LoadTest()
    {
        StartCoroutine(LoadPrefabs());
    }

    private IEnumerator LoadPrefabs()
    {
        AssetOperationHandle handle = package.LoadAssetAsync<GameObject>("Assets/AssetPackages/Prefabs/AvaterPrefabs/Avatar_Girl_Bow_Yoimiya_Remote (merge).prefab");
        yield return handle;
        GameObject gameObject = handle.InstantiateSync();
        Debug.Log(gameObject.name);
    }

    private IEnumerator PrepareAssets()
    {
        //1. InitYooAsset
        YooAssets.Initialize();
        package = YooAssets.CreatePackage("DefaultPackage");
        YooAssets.SetDefaultPackage(package);
        EPlayMode PlayMode = ClientConfig.Instance.GetEPlayMode();
        switch (PlayMode)
        {
            case EPlayMode.EditorSimulateMode:
                {
                    var initParameters = new EditorSimulateModeParameters();
                    initParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild("DefaultPackage");
                    var initOperation = package.InitializeAsync(initParameters);
                    yield return initOperation;

                    if (initOperation.Status == EOperationStatus.Succeed)
                    {
                        Debug.Log("资源包初始化成功！");
                        LoginSystem.Instance.EnterLogin();
                        yield break;
                    }
                    else
                    {
                        Debug.LogError($"资源包初始化失败：{initOperation.Error}");
                        yield break;
                    }
                }
            case EPlayMode.HostPlayMode:
                {
                    var initParameters = new HostPlayModeParameters();
                    initParameters.QueryServices = new QueryStreamingAssetsFileServices();
                    initParameters.DefaultHostServer = ClientConfig.Instance.GetCNDServerAddress();
                    initParameters.FallbackHostServer = ClientConfig.Instance.GetCNDServerAddress();
                    var initOperation = package.InitializeAsync(initParameters);
                    yield return initOperation;

                    if (initOperation.Status == EOperationStatus.Succeed)
                    {
                        Debug.Log("资源包初始化成功！");
                    }
                    else
                    {
                        Debug.LogError($"资源包初始化失败：{initOperation.Error}");
                        yield break;
                    }
                }
                break;
            case EPlayMode.OfflinePlayMode:
                {
                    var initParameters = new OfflinePlayModeParameters();
                    var initOperation = package.InitializeAsync(initParameters);
                    yield return initOperation;

                    if (initOperation.Status == EOperationStatus.Succeed)
                    {
                        Debug.Log("资源包初始化成功！");
                        LoginSystem.Instance.EnterLogin();
                        yield break;
                    }
                    else
                    {
                        Debug.LogError($"资源包初始化失败：{initOperation.Error}");
                        yield break;
                    }
                }
        }

        //2. UpdatePackageVersion
        var updatePackageVersionOperation = package.UpdatePackageVersionAsync();
        yield return updatePackageVersionOperation;

        if (updatePackageVersionOperation.Status != EOperationStatus.Succeed)
        {
            Debug.LogError(updatePackageVersionOperation.Error);
            yield break;
        }
        string packageVersion = updatePackageVersionOperation.PackageVersion;

        //3. UpdatePackageManifest
        bool savePackageVersion = true;
        var updatePackageManifestOperation = package.UpdatePackageManifestAsync(packageVersion, savePackageVersion);
        yield return updatePackageManifestOperation;

        if (updatePackageManifestOperation.Status != EOperationStatus.Succeed)
        {
            Debug.LogError(updatePackageManifestOperation.Error);
            yield break;
        }

        //4. Download
        PrepareDownloader();
        yield break;
    }

    private void PrepareDownloader()
    {
        int downloadingMaxNum = 10;
        int failedTryAgain = 3;
        var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);

        //if (downloader.TotalDownloadCount == 0)
        //{
        //    LoginSystem.Instance.EnterLogin();
        //    Debug.Log("没有任何数据需要下载哦~");
        //    return;
        //}

        int totalDownloadCount = downloader.TotalDownloadCount;
        long totalDownloadBytes = downloader.TotalDownloadBytes;

        downloader.OnDownloadErrorCallback = OnDownloadErrorFunction;
        downloader.OnDownloadProgressCallback = OnDownloadProgressUpdateFunction;
        downloader.OnDownloadOverCallback = OnDownloadOverFunction;
        downloader.OnStartDownloadFileCallback = OnStartDownloadFileFunction;

        downloaderOperation = downloader;

        HotFixSystem.Instance.OpenHotFixWindow(totalDownloadBytes);

        Debug.Log("现在已经准备好下载器了哦~");
    }

    private IEnumerator RunDownloader(ResourceDownloaderOperation downloader)
    {
        downloader.BeginDownload();
        HotFixSystem.Instance.CloseHotFixWindow();
        LoadingSystem.Instance.OpenLoadingWindow();
        yield return downloader;
        if (downloader.Status == EOperationStatus.Succeed)
        {
            Debug.Log("下载成功");
            LoadingSystem.Instance.CloseLoadingWindow();
            LoginSystem.Instance.EnterLogin();
        }
        else
        {
            Debug.Log("下载失败");
        }
    }

    private void OnDownloadProgressUpdateFunction(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes)
    {
        Debug.Log($"文件总数: {totalDownloadCount}, 已下载文件数： {currentDownloadCount}, 总大小: {totalDownloadBytes}, 已下载大小: {currentDownloadBytes}");
        float progress = (float)currentDownloadBytes / totalDownloadBytes;
        LoadingSystem.Instance.SetLoadingProgress(progress);
    }

    private void OnStartDownloadFileFunction(string fileName, long sizeBytes)
    {
        Debug.Log($"开始下载: {fileName}, 文件大小: {sizeBytes}");
    }

    private void OnDownloadOverFunction(bool isSucceed)
    {
        Debug.Log($"下载完成情况: {isSucceed}");
    }

    private void OnDownloadErrorFunction(string fileName, string error)
    {
        Debug.Log($"下载出错: {fileName}, 出错原因: {error}");
    }

    private class QueryStreamingAssetsFileServices : IQueryServices
    {
        public bool QueryStreamingAssets(string fileName)
        {
            string buildinFolderName = YooAssets.GetStreamingAssetBuildinFolderName();
            return StreamingAssetsHelper.FileExists($"{buildinFolderName}/{fileName}");
        }
    }
}

using System.Collections;
using UnityEngine;
using YooAsset;

//Developer: SangonomiyaSakunovi

public class HotFixService : BaseService
{
    public static HotFixService Instance;

    public override void InitService()
    {
        base.InitService();
        Instance = this;              
    }

    public void DownloadSangoAssets()
    {        
        StartCoroutine(LoadAsset());
    }

    public void LoadDll()
    {

    }

    private IEnumerator LoadAsset()
    {
        //1. InitYooAsset
        YooAssets.Initialize();
        var package = YooAssets.CreatePackage("DefaultPackage");
        YooAssets.SetDefaultPackage(package);
        EPlayMode PlayMode = ClientConfig.Instance.GetEPlayMode();
        switch (PlayMode)
        {
            case EPlayMode.EditorSimulateMode:
                {
                    var initParameters = new EditorSimulateModeParameters();
                    initParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild("DefaultPackage");
                    yield return package.InitializeAsync(initParameters);
                }
                break;
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
                    yield return package.InitializeAsync(initParameters);
                }
                break;
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
        var updatePackageManifestOperation = package.UpdatePackageManifestAsync(packageVersion);
        yield return updatePackageManifestOperation;

        if (updatePackageManifestOperation.Status != EOperationStatus.Succeed)
        {
            Debug.LogError(updatePackageManifestOperation.Error);
            yield break;
        }

        //4. Download
        yield return Download();
    }

    private IEnumerator Download()
    {
        int downloadingMaxNum = 10;
        int failedTryAgain = 3;
        var package = YooAssets.GetPackage("DefaultPackage");
        var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);

        if (downloader.TotalDownloadCount == 0)
        {
            yield break;
        }

        int totalDownloadCount = downloader.TotalDownloadCount;
        long totalDownloadBytes = downloader.TotalDownloadBytes;

        downloader.OnDownloadErrorCallback = OnDownloadErrorFunction;
        downloader.OnDownloadProgressCallback = OnDownloadProgressUpdateFunction;
        downloader.OnDownloadOverCallback = OnDownloadOverFunction;
        downloader.OnStartDownloadFileCallback = OnStartDownloadFileFunction;

        downloader.BeginDownload();
        yield return downloader;

        if (downloader.Status == EOperationStatus.Succeed)
        {
            Debug.Log("下载成功");
        }
        else
        {
            Debug.Log("下载失败");
        }
    }

    private void OnStartDownloadFileFunction(string fileName, long sizeBytes)
    {
        Debug.Log($"开始下载: {fileName}, 文件大小: {sizeBytes}");
    }

    private void OnDownloadProgressUpdateFunction(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes)
    {
        Debug.Log($"文件总数: {totalDownloadCount}, 已下载文件数： {currentDownloadCount}, 总大小: {totalDownloadBytes}, 已下载大小: {currentDownloadBytes}");
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
            // StreamingAssetsHelper.cs是太空战机里提供的一个查询脚本。
            string buildinFolderName = YooAssets.GetStreamingAssetBuildinFolderName();
            return true;
        }
    }
}

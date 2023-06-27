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
                        Debug.Log("��Դ����ʼ���ɹ���");
                    }
                    else
                    {
                        Debug.LogError($"��Դ����ʼ��ʧ�ܣ�{initOperation.Error}");
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
            Debug.Log("���سɹ�");
        }
        else
        {
            Debug.Log("����ʧ��");
        }
    }

    private void OnStartDownloadFileFunction(string fileName, long sizeBytes)
    {
        Debug.Log($"��ʼ����: {fileName}, �ļ���С: {sizeBytes}");
    }

    private void OnDownloadProgressUpdateFunction(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes)
    {
        Debug.Log($"�ļ�����: {totalDownloadCount}, �������ļ����� {currentDownloadCount}, �ܴ�С: {totalDownloadBytes}, �����ش�С: {currentDownloadBytes}");
    }

    private void OnDownloadOverFunction(bool isSucceed)
    {
        Debug.Log($"����������: {isSucceed}");
    }

    private void OnDownloadErrorFunction(string fileName, string error)
    {
        Debug.Log($"���س���: {fileName}, ����ԭ��: {error}");
    }

    private class QueryStreamingAssetsFileServices : IQueryServices
    {
        public bool QueryStreamingAssets(string fileName)
        {
            // StreamingAssetsHelper.cs��̫��ս�����ṩ��һ����ѯ�ű���
            string buildinFolderName = YooAssets.GetStreamingAssetBuildinFolderName();
            return true;
        }
    }
}

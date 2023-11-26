using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Shared;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace DA_Assets.FCU
{
    [Serializable]
    public class RequestSender : MonoBehaviourBinder<FigmaConverterUnity>
    {
        [SerializeField] float pbarProgress;
        public float PbarProgress => pbarProgress;

        [SerializeField] float pbarBytes;
        public float PbarBytes => pbarBytes;

        public IEnumerator SendRequest<T>(Request request, Return<T, FigmaError> @return)
        {
            UnityWebRequest webRequest;

            switch (request.RequestType)
            {
                case RequestType.Post:
                    webRequest = UnityWebRequest.Post(request.Query, request.WWWForm);
                    break;
                default:
                    webRequest = UnityWebRequest.Get(request.Query);
                    break;
            }


            using (webRequest)
            {
                if (request.RequestHeader.IsDefault() == false)
                {
                    webRequest.SetRequestHeader(request.RequestHeader.Name, request.RequestHeader.Value);
                }

                try
                {
                    webRequest.SendWebRequest();
                }
                catch (InvalidOperationException)
                {
                    DALogger.LogError(FcuLocKey.log_enable_http_project_settings.Localize());
                    monoBeh.AssetTools.StopImport();
                    yield break;
                }
                catch (Exception ex)
                {
                    DALogger.LogError(ex);
                }

                yield return UpdateRequestProgressBar(webRequest);
                yield return MoveRequestProgressBarToEnd();

                var result = new RoutineResult<T, FigmaError>();

                if (request.RequestType == RequestType.GetFile)
                {
                    result.Success = true;
                    result.Result = (T)(object)webRequest.downloadHandler.data;
                }
                else
                {
                    string text = webRequest.downloadHandler.text;

                    request.WriteLog(text).StartDARoutine(monoBeh);

                    if (typeof(T) == typeof(string))
                    {
                        result.Success = true;
                        result.Result = (T)(object)text;
                    }
                    else
                    {
                        yield return TryParseResponse<T>(text, webRequest, x => result = x);
                    }
                }

                @return.Invoke(result);
            }
        }

        private IEnumerator TryParseResponse<T>(string text, UnityWebRequest webRequest, Return<T, FigmaError> @return)
        {
            RoutineResult<T, FigmaError> finalResult = new RoutineResult<T, FigmaError>();

            bool isRequestError;
#if UNITY_2020_1_OR_NEWER
            isRequestError = webRequest.result == UnityWebRequest.Result.ConnectionError;
#else
            isRequestError = webRequest.isNetworkError || webRequest.isHttpError;
#endif

            monoBeh.Log($"TryParseResponse | 0");

            if (isRequestError)
            {
                finalResult.Success = false;

                if (webRequest.error.Contains("SSL"))
                {
                    monoBeh.Log($"TryParseResponse | 1");

                    finalResult.Error = new FigmaError(909, text);
                }
                else
                {
                    monoBeh.Log($"TryParseResponse | 2");

                    finalResult.Error = new FigmaError((int)webRequest.responseCode, webRequest.error);
                }
            }
            else if (text.Contains("<pre>Cannot GET "))
            {
                finalResult.Error = new FigmaError(404, text);
            }
            else
            {
                monoBeh.Log($"TryParseResponse | 3");

                RoutineResult<T, Exception> result1 = null;
                yield return DAJson.FromJson<T>(text, x => result1 = x);

                if (result1.Success)
                {
                    monoBeh.Log($"TryParseResponse | 4");

                    finalResult.Success = true;
                    finalResult.Result = result1.Result;

                    if (typeof(T) == typeof(FigmaProject))
                    {
                        monoBeh.ProjectCacher.Cache(text, result1.Result, monoBeh.Settings.MainSettings.ProjectUrl).StartDARoutine(monoBeh);
                    }
                }
                else
                {
                    monoBeh.Log($"TryParseResponse | 5");

                    RoutineResult<FigmaError, Exception> result2 = null;
                    yield return DAJson.FromJson<FigmaError>(text, x => result2 = x);

                    if (result2.Success)
                    {
                        monoBeh.Log($"TryParseResponse | 6");

                        finalResult.Success = false;
                        finalResult.Error = result2.Result;
                    }
                    else
                    {
                        monoBeh.Log($"TryParseResponse | 7");

                        finalResult.Success = false;
                        finalResult.Error = new FigmaError(99999, text);
                    }
                }
            }

            if (finalResult.Success == false)
            {
                //errorCount++
            }

            @return.Invoke(finalResult);
        }

        private IEnumerator UpdateRequestProgressBar(UnityWebRequest webRequest)
        {
            while (webRequest.isDone == false)
            {
                if (pbarProgress < 1f)
                {
                    pbarProgress += WaitFor.Delay001().WaitTimeF;
                }
                else
                {
                    pbarProgress = 0;
                }

                if (webRequest.downloadedBytes == 0)
                {
                    pbarBytes += 100;
                }
                else
                {
                    pbarBytes = webRequest.downloadedBytes;
                }

                yield return WaitFor.Iterations(1);
            }
        }

        private IEnumerator MoveRequestProgressBarToEnd()
        {
            while (true)
            {
                if (pbarProgress < 1f)
                {
                    pbarProgress += WaitFor.Delay001().WaitTimeF;
                    yield return null;
                }
                else
                {
                    pbarProgress = 0f;
                    pbarBytes = 0f;
                    break;
                }
            }
        }
    }

    public struct Request
    {
        public string Query;
        public RequestType RequestType;
        public RequestHeader RequestHeader;
        public WWWForm WWWForm;
    }

    public struct RequestHeader
    {
        public string Name;
        public string Value;
    }

    public enum RequestType
    {
        Get,
        Post,
        GetFile,
    }
}
